using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.ServiceContract;
using RAGApi.Application.ServiceContract;
using RAGApi.Domain.Repository;

namespace Application.ServiceClass
{
    public class AiAgent : IAgent
    {
        private readonly IRagService _ragService;
        private readonly IMCPService _mcpService;
        private readonly ILlmService _llmService;

        public AiAgent(IRagService ragService, IMCPService mcpService, ILlmService llmService)
        {
            _ragService = ragService;
            _mcpService = mcpService;
            _llmService = llmService;
        }

        public async Task<string> HandleQueryAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return "Empty query provided.";

            try
            {
                // Build a prompt asking the LLM to choose between MCP or RAG and respond with JSON only.
                // Provide a few-shot example to bias the model towards the expected structured output.
                var sbPrompt = new StringBuilder();
                sbPrompt.AppendLine("You are an agent that chooses which tool to use to answer a user's question. Respond with a single valid JSON object and nothing else.");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("Tools available:");
                sbPrompt.AppendLine("- MCP: queries the Departments database and returns matching departments. Use when the user asks about department names, locations, managers, budgets or other factual department attributes.");
                sbPrompt.AppendLine("- RAG: use the Retrieval-Augmented Generation pipeline (search documents + call LLM) to answer general knowledge or contextual questions.");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("JSON schema (all lowercase keys):");
                sbPrompt.AppendLine("{");
                sbPrompt.AppendLine("  \"tool\": \"MCP\" | \"RAG\",");
                sbPrompt.AppendLine("  \"query\": \"string (concise search term to send to MCP; optional for RAG)\",");
                sbPrompt.AppendLine("  \"reason\": \"short explanation\"");
                sbPrompt.AppendLine("}");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("Examples:");
                sbPrompt.AppendLine("Input: \"get Delhi department names list\"");
                sbPrompt.AppendLine("Output: {\"tool\":\"MCP\",\"query\":\"Delhi\",\"reason\":\"User requests department names for a specific location\"}");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("Input: \"summarize the onboarding document\"");
                sbPrompt.AppendLine("Output: {\"tool\":\"RAG\",\"query\":\"\",\"reason\":\"This is a document summarization request\"}");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("Now process the USER QUERY below. Return only the JSON object.");
                sbPrompt.AppendLine();
                sbPrompt.AppendLine("USER QUERY:");
                sbPrompt.AppendLine(query);
                var prompt = sbPrompt.ToString();

                var decision = await _llmService.GenerateAsync(prompt);

                if (string.IsNullOrWhiteSpace(decision))
                {
                    // fallback to RAG
                    var chatResp = await _ragService.AskAsync(query);
                    return chatResp?.Answer ?? "No answer available.";
                }

                // Try to parse JSON response from the LLM
                string tool = null;
                string parsedQuery = null;
                try
                {
                    using var doc = JsonDocument.Parse(decision);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("tool", out var toolProp))
                        tool = toolProp.GetString();
                    if (root.TryGetProperty("query", out var qProp))
                        parsedQuery = qProp.GetString();
                }
                catch
                {
                    // If parsing fails, fall back to simple token search in the LLM output
                    var lines = decision.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var first = lines.FirstOrDefault()?.Trim().ToUpperInvariant() ?? "";
                    if (first.StartsWith("MCP") || first.StartsWith("RAG"))
                    {
                        tool = first.StartsWith("MCP") ? "MCP" : "RAG";
                        var queryLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("QUERY:", StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrWhiteSpace(queryLine))
                            parsedQuery = queryLine.Substring(queryLine.IndexOf(':') + 1).Trim();
                    }
                }

                tool = tool?.Trim().ToUpperInvariant();
                parsedQuery = string.IsNullOrWhiteSpace(parsedQuery) ? query : parsedQuery;

                if (tool == "MCP")
                {
                    var departments = await _mcpService.FindDepartmentsAsync(parsedQuery);
                    if (departments != null && departments.Any())
                    {
                        var json = JsonSerializer.Serialize(departments, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = false
                        });
                        return json;
                    }

                    // fallback to RAG if MCP yields nothing
                }

                // Default / RAG path
                var ragResponse = await _ragService.AskAsync(query);
                return ragResponse?.Answer ?? "No answer available.";
            }
            catch (Exception ex)
            {
                // on error, fall back to RAG
                try
                {
                    var ragResponse = await _ragService.AskAsync(query);
                    return ragResponse?.Answer ?? $"Error: {ex.Message}";
                }
                catch
                {
                    return $"Agent error: {ex.Message}";
                }
            }
        }
    }
}

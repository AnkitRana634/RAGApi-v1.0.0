using Application.Response;
using Application.ServiceContract;
using Application.VectorStore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Application.ServiceClass
{
    public class RagService : IRagService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IVectorStore _vectorStore;
        private readonly ILlmService _llmService;

        public RagService(
            IEmbeddingService embeddingService,
            IVectorStore vectorStore,
            ILlmService llmService)
        {
            _embeddingService = embeddingService;
            _vectorStore = vectorStore;
            _llmService = llmService;
        }

        public async Task<ChatResponse> AskAsync(
            string question)
        {
            var total = Stopwatch.StartNew();
            // 1. Convert question into embedding
            var sw = Stopwatch.StartNew();
            var questionEmbedding =
                await _embeddingService
                    .CreateEmbeddingAsync(question);
            Console.WriteLine($"Embedding elapsed: {sw.ElapsedMilliseconds} ms");

            // 2. Find relevant chunks
            sw.Restart();
            var relevantChunks =
                await _vectorStore
                    .SearchAsync(
                        questionEmbedding.ToArray(),
                        topK: 5);
            Console.WriteLine($"Vector search: {sw.ElapsedMilliseconds} ms");

            // 3. Combine retrieved chunks
            sw.Restart();
            var context = string.Join(
                "\n\n",
                relevantChunks.Select(x => x.Content));

             
            // 4. Create RAG prompt
            var prompt = $"""
            You are a helpful assistant.

            Answer the user's question using ONLY
            the context provided below.

            If the answer is not available in the context,
            clearly say:

            "I could not find the answer in the provided documents."

            CONTEXT:
            --------------------
            {context}
            --------------------

            USER QUESTION:
            {question}
            """;
            Console.WriteLine($"Prompt creation: {sw.ElapsedMilliseconds} ms");

            // 5. Send prompt to Ollama
            sw.Restart();
            var answer =
                await _llmService
                    .GenerateAsync(prompt);

            Console.WriteLine($"LLM answer: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine( $"TOTAL: {total.ElapsedMilliseconds} ms");

            return new ChatResponse
            {
                Answer = answer,

                Sources = relevantChunks
                    .Select(x => x.DocumentName)
                    .Distinct()
                    .ToList()
            };
        }
    }
}

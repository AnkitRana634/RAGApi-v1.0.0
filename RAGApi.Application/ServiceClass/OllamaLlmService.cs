using Application.Request;
using Application.Response;
using Application.ServiceContract;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Application.ServiceClass
{
    public class OllamaLlmService : ILlmService
    {
        private readonly HttpClient _httpClient;

        public OllamaLlmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            var request = new OllamaGenerateRequest
            {
                Model = "llama3.2",
                Prompt = prompt,
                Stream = false
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/generate",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<OllamaGenerateResponse>(
                        cancellationToken: cancellationToken);

            return result?.Response
                   ?? "No response received from Ollama.";
        }
    }
}

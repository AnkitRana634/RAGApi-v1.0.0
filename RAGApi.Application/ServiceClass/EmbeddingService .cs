using Application.Request;
using Application.Response;
using Application.ServiceContract;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Application.ServiceClass
{
    public class EmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;

        public EmbeddingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<float>> CreateEmbeddingAsync(string text)
        {
            var request = new ollamareq
            {
                model = "nomic-embed-text",
                prompt = text
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/embeddings",
                request);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<ollamaresp>();

            return result.Embedding.ToList();
        }

    }
}

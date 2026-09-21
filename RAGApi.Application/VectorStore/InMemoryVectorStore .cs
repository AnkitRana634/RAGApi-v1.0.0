using RAGApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.VectorStore
{
    public class InMemoryVectorStore:IVectorStore
    {
        private readonly List<DocumentChunk> _documents = new();

        public Task SaveAsync(DocumentChunk document)
        {
            _documents.Add(document);

            return Task.CompletedTask;
        }

        public Task<List<DocumentChunk>> SearchAsync(
            float[] queryEmbedding,
            int topK = 5)
        {
            var results = _documents
                .Select(document => new
                {
                    Document = document,

                    Similarity = CosineSimilarity(
                        queryEmbedding,
                        document.Embedding.ToArray())
                })
                .OrderByDescending(x => x.Similarity)
                .Take(topK)
                .Select(x => x.Document)
                .ToList();

            return Task.FromResult(results);
        }


        private static float CosineSimilarity(
            float[] vectorA,
            float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException(
                    "Vectors must have the same dimension.");
            }

            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];

                magnitudeB += vectorB[i] * vectorB[i];
            }

            magnitudeA = MathF.Sqrt(magnitudeA);
            magnitudeB = MathF.Sqrt(magnitudeB);

            if (magnitudeA == 0 || magnitudeB == 0)
            {
                return 0;
            }

            return dotProduct /
                   (magnitudeA * magnitudeB);
        }
    }
}

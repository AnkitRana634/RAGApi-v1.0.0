using Application.VectorStore;
using RAGApi.Domain.Repository;
using RAGApi.Models;

namespace RAGApi.Application.VectorStore
{
    internal class SQLVectorStore : IVectorStore
    {
        private readonly IDocumentRepo _documentRepo;
        public Task SaveAsync(DocumentChunk document) {
            var doc = new Domain.Entities.DocumentChunk
            {
                DocumentName = document.DocumentName,
                Content = document.Content,
                Embedding = Helper.Helper.ToBytes(document.Embedding.ToArray()),
                ChunkIndex = document.ChunkIndex,
                CreatedAt = DateTime.Now
            };
            _documentRepo.StoreAsync(doc);

            return Task.CompletedTask;
        }

        public async Task<List<DocumentChunk>> SearchAsync( float[] queryEmbedding, int topK = 5)
        {
            var results = await _documentRepo.SearchAsync(queryEmbedding, topK);
            var docs = new List<DocumentChunk>();
            foreach (var chunk in results)
            {
                docs.Add(new DocumentChunk
                {
                    DocumentId = chunk.Id.ToString(),
                    DocumentName = chunk.DocumentName,
                    ChunkIndex = chunk.ChunkIndex,
                    CreatedAt= chunk.CreatedAt,
                    Content = chunk.Content,
                    Embedding=Helper.Helper.ToFloatArray(chunk.Embedding).ToList()
                });
            }
            return await Task.FromResult(docs);
        }
    }
}

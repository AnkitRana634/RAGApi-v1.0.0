using RAGApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Domain.Repository
{
    public interface IDocumentRepo
    {
        public Task<List<DocumentChunk>> SearchAsync(float[] queryEmbedding, int topK = 5);
        public Task StoreAsync(DocumentChunk chunk);
    }
}

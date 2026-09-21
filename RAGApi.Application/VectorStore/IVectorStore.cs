using RAGApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.VectorStore
{
    public interface IVectorStore
    {
        Task SaveAsync(DocumentChunk document);

        Task<List<DocumentChunk>> SearchAsync(
            float[] queryEmbedding,
            int topK = 5);
    }
}

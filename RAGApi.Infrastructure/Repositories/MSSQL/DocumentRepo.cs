using RAGApi.Domain.Entities;
using RAGApi.Domain.Repository;
using RAGApi.Repositories;
using RAGApi.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RAGApi.Infrastructure.Repositories.MSSQL
{
    public class DocumentRepo:IDocumentRepo
    {
        RagDbContext _msSqlDBContext;
        public async Task StoreAsync(DocumentChunk chunk)
        {
            _msSqlDBContext.DocumentChunks.Add(chunk);

            await _msSqlDBContext.SaveChangesAsync();
        }
        public async Task<List<DocumentChunk>> SearchAsync(float[] queryEmbedding,int topK = 5)
        {
            var chunks =
                await _msSqlDBContext.DocumentChunks.AsNoTracking().ToListAsync();

            var results = chunks
                .Select(chunk =>
                    new
                    {
                        Document = chunk,
                        Similarity = Helper.Helper.CosineSimilarity(queryEmbedding, Helper.Helper.ToFloatArray(chunk.Embedding))
                    }
                )
                .OrderByDescending(x => x.Similarity)
                .Take(topK)
                .Select(x => x.Document)
                .ToList();

            return results;
        }
    }
}

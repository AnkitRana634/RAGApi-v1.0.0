using Application.ServiceContract;
using Application.VectorStore;
using RAGApi.Application.ServiceContract;
using RAGApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceClass
{
    public class DocumentIngestionService:IDocumentIngestionService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IVectorStore _vectorStore;
        private readonly ITextChunkingService _textChunkingService;

        public DocumentIngestionService(
            IEmbeddingService embeddingService,
            IVectorStore vectorStore,ITextChunkingService textChunkingService)
        {
            _embeddingService = embeddingService;
            _vectorStore = vectorStore;
            _textChunkingService = textChunkingService;
        }

        public async Task ProcessDocumentAsync(
            string documentName,
            string content)
        {
            var chunks = _textChunkingService.SplitText(content,1000,200);

            int index = 0;

            foreach (var chunk in chunks)
            {
                var embedding =
                    await _embeddingService
                        .CreateEmbeddingAsync(chunk);

                var documentChunk = new DocumentChunk
                {
                    Id = Guid.NewGuid().ToString(),
                    DocumentName = documentName,
                    Content = chunk,
                    Embedding = embedding,
                    ChunkIndex = index++
                };

                await _vectorStore
                    .SaveAsync(documentChunk);
            }
        }
    }
}

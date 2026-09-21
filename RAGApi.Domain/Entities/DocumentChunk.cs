using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Domain.Entities
{
    public class DocumentChunk
    {
        public long Id { get; set; }

        public string DocumentName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public byte[] Embedding { get; set; } = Array.Empty<byte>();

        public int ChunkIndex { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

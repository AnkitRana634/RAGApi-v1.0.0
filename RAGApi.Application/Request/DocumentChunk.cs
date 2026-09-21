namespace RAGApi.Models
{
    public class DocumentChunk
    {
        public string Id { get; set; }

        public string DocumentId { get; set; }

        public string DocumentName { get; set; }

        public int ChunkIndex { get; set; }

        public string Content { get; set; }

        public List<float> Embedding { get; set; }

        public DateTime CreatedAt { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}

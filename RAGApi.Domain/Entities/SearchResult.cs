using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Domain.Entities
{
    public class SearchResult
    {
        public string Content { get; set; } = string.Empty;
        public float Score {  get; set; }
    }
}

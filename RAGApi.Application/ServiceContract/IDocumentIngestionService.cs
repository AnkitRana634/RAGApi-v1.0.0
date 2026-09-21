using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Application.ServiceContract
{
    public  interface IDocumentIngestionService
    {
        public Task ProcessDocumentAsync(string documentName, string content);
    }
}

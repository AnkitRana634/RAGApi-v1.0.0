using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Application.ServiceContract
{
    public interface IDocumentExtract
    {
        public Task<string> ExtractTextAsync(IFormFile file);
    }
}

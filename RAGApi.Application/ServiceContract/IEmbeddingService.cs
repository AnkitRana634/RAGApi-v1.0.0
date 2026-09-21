using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceContract
{
    public interface IEmbeddingService
    {
        Task<List<float>> CreateEmbeddingAsync(string text);
    }
}

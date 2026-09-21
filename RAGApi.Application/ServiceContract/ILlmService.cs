using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceContract
{
    public interface ILlmService
    {
        Task<string> GenerateAsync(
            string prompt,
            CancellationToken cancellationToken = default);
    }
}

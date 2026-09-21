using Application.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceContract
{
    public interface IRagService
    {
        public Task<ChatResponse> AskAsync(string question);
    }
}

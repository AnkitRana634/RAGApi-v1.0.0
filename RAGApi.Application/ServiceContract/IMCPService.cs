using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Application.ServiceContract
{
    public interface IMCPService
    {
        public Task<List<Response.Department>> FindDepartmentsAsync(string term);
    }
}

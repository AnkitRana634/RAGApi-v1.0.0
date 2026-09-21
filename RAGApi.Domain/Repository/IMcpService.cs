using System.Collections.Generic;
using System.Threading.Tasks;
using RAGApi.Domain.Entities;

namespace RAGApi.Domain.Repository
{
    public interface IMcpService
    {
        Task<List<Department>> FindDepartmentsAsync(string term);
    }
}

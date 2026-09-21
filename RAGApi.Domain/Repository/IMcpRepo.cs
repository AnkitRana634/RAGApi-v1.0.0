using System.Collections.Generic;
using System.Threading.Tasks;
using RAGApi.Domain.Entities;

namespace RAGApi.Domain.Repository
{
    public interface IMcpRepo
    {
        /// <summary>
        /// Finds departments matching the provided search term. If term is empty or null, returns all departments.
        /// </summary>
        Task<List<Department>> FindDepartmentsAsync(string term);
    }
}

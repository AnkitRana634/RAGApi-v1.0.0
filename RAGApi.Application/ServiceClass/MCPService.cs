using RAGApi.Application.Response;
using RAGApi.Application.ServiceContract;
using RAGApi.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAGApi.Application.ServiceClass
{
    public class MCPService : IMCPService
    {
        private readonly IMcpRepo _mcpRepo;
        public MCPService(IMcpRepo mcpRepo)
        {
            _mcpRepo = mcpRepo ?? throw new ArgumentNullException(nameof(mcpRepo));
        }

        public async Task<List<Department>> FindDepartmentsAsync(string term)
        {
            var departments = await _mcpRepo.FindDepartmentsAsync(term);
            var result = new List<Department>();
            foreach (var dept in departments)
            {
                result.Add(new Response.Department
                {
                    DepartmentId = dept.DepartmentId,
                    DepartmentName = dept.DepartmentName,
                    Location = dept.Location,
                    ManagerName = dept.ManagerName,
                    Budget = dept.Budget,
                    CreatedDate = dept.CreatedDate
                });
            }
            return result;
        }
    }
}

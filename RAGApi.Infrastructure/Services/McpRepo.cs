using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RAGApi.Domain.Entities;
using RAGApi.Domain.Repository;
using RAGApi.Repositories;

namespace RAGApi.Infrastructure.Services
{
    public class McpRepo : IMcpRepo
    {
        private readonly RagDbContext _context;

        public McpRepo(RagDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindDepartmentsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Department.ToListAsync();
            }

            var q = term.ToLower();

            return await _context.Department
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RAGApi.Domain.Entities;

namespace RAGApi.Repositories
{
    public class RagDbContext : DbContext
    {
        public RagDbContext(DbContextOptions<RagDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentChunk> DocumentChunks { get; set; }
        public DbSet<Department> Department { get; set; }
    }
}

using System.Threading.Tasks;

namespace Application.ServiceContract
{
    public interface IAgent
    {
        /// <summary>
        /// Handle an incoming user query and route to RAG or MCP as appropriate.
        /// Returns a textual answer.
        /// </summary>
        Task<string> HandleQueryAsync(string query);
    }
}

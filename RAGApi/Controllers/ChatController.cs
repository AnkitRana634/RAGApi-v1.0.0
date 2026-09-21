using Application.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RAGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAgent _agent;
        public ChatController(IAgent agent)
        {
            _agent = agent;
        }
        [HttpGet("ChatWithOllama")]
        public async Task<IActionResult> Test(string question)
        {
            var response =
                await _agent.HandleQueryAsync(question);

            return Ok(new
            {
                response
            });
        }
    }
}

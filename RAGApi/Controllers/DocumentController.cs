using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAGApi.Application.ServiceContract;

namespace RAGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentExtract _extractor;
        private readonly IDocumentIngestionService _documentService;
        public DocumentController(IDocumentExtract extractor, IDocumentIngestionService documentService)
        {
            _extractor = extractor;
            _documentService = documentService;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                return BadRequest("Please upload a file.");

            try
            {
                var text = await _extractor.ExtractTextAsync(file);
                await _documentService.ProcessDocumentAsync(file.FileName,text);
                return Ok(new
                {
                    fileName = file.FileName,
                    content = text
                });
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

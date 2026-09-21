using Microsoft.AspNetCore.Http;
using RAGApi.Application.ServiceContract;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using UglyToad.PdfPig;
using System.Text;

namespace RAGApi.Application.ServiceClass
{
    public class DocumentExtract:IDocumentExtract
    {
        public async Task<string> ExtractTextAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            return extension switch
            {
                ".txt" => await ExtractTextFileAsync(file),
                ".pdf" => await ExtractPdfAsync(file),
                ".docx" => await ExtractDocxAsync(file),

                _ => throw new NotSupportedException(
                    $"File type '{extension}' is not supported.")
            };
        }

        private async Task<string> ExtractTextFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            return await reader.ReadToEndAsync();
        }

        private async Task<string> ExtractPdfAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();

            using var pdf = PdfDocument.Open(stream);

            var text = new StringBuilder();

            foreach (var page in pdf.GetPages())
            {
                text.AppendLine(page.Text);
            }

            return text.ToString();
        }

        private async Task<string> ExtractDocxAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();

            using var document = WordprocessingDocument.Open(
                stream,
                false);

            var body = document.MainDocumentPart?
                .Document?
                .Body;

            if (body == null)
                return string.Empty;

            var paragraphs = body
                .Descendants<Paragraph>()
                .Select(p => p.InnerText);

            return string.Join(
                Environment.NewLine,
                paragraphs);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Response
{
    public class OllamaGenerateResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;
    }
}

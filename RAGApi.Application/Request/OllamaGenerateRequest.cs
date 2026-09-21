using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Request
{
    public class OllamaGenerateRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }
}

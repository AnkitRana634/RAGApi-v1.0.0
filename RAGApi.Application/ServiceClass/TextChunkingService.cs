using Application.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceClass
{
    public class TextChunkingService:ITextChunkingService
    {
        public List<string> SplitText(
            string text,
            int chunkSize = 1000,
            int overlap = 200)
        {
            var chunks = new List<string>();

            for (int i = 0; i < text.Length; i += chunkSize - overlap)
            {
                int length = Math.Min(chunkSize, text.Length - i);

                chunks.Add(text.Substring(i, length));

                if (i + length >= text.Length)
                    break;
            }

            return chunks;
        }
    }
}

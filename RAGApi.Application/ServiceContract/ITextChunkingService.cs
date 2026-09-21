using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceContract
{
    public interface ITextChunkingService
    {
        public List<string> SplitText(string text,
            int chunkSize ,
            int overlap );
    }
}

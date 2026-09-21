using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Request
{
    public class ollamareq
    {
        public string model { get; set; }
        public string prompt { get; set; }
        public bool stream {  get; set; }
    }
}

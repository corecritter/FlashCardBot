using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class ResponseCard
    {
        public int version { get; set; }
        public string contentType { get; set; }
        public List<GenericAttatchment> genericAttachments { get; set; }
    }
}

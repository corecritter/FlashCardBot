using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class GenericAttatchment
    {
        public string title { get; set; }
        public string imageUrl { get; set; }
        public string attachmentLinkUrl { get; set; }
        public List<Button> buttons { get; set; }
    }
    public class Button
    {
        public string text { get; set; }
        public string value { get; set; }
    }
}

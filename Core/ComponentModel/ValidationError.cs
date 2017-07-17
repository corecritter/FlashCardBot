using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ComponentModel
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
    }
}

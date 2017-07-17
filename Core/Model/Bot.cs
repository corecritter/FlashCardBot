using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class Bot
    {
        public string Alias { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format(Constants.STRING_FORMAT_PROPERTY_VALUE, nameof(Alias), Alias));
            sb.Append(String.Format(Constants.STRING_FORMAT_PROPERTY_VALUE, nameof(Version), Version));
            sb.Append(String.Format(Constants.STRING_FORMAT_PROPERTY_VALUE, nameof(Name), Name));
            return sb.ToString();
        }
    }
}

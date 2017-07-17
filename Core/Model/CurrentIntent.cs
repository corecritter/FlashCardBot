using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public class CurrentIntent<SlotType>
    {
        public string Name { get; set; }
        public string ConfirmationStatus { get; set; }
        public SlotType Slots { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format(Constants.STRING_FORMAT_PROPERTY_VALUE, nameof(Name), Name));
            sb.Append(String.Format(Constants.STRING_FORMAT_PROPERTY_VALUE, nameof(ConfirmationStatus), ConfirmationStatus));
            return sb.ToString();
        }
    }
}

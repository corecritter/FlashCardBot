using System;
using System.Collections.Generic;
using System.Text;
using Core.ComponentModel;
using Core;
using System.Linq;

namespace Core.Model
{
    public class ManageDeckSlotType : ISlotType
    {
        public string ManageType { get; set; }
        public string DeckName { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public string Confirm { get; set; }

        public string GetSlotToElicit()
        {
            if (String.IsNullOrEmpty(ManageType) || String.IsNullOrWhiteSpace(ManageType))
                return nameof(ManageType);
            if (String.IsNullOrEmpty(DeckName) || String.IsNullOrWhiteSpace(DeckName))
                return nameof(DeckName);
            if (ManageType.ToLower() == Constants.ManageTypes.Delete.ToString().ToLower() || ManageType.ToLower() == Constants.ManageTypes.Add.ToString().ToLower())
            {
                if (String.IsNullOrEmpty(Confirm) || String.IsNullOrWhiteSpace(Confirm))
                    return nameof(Confirm);
            }
            else
            {
                if (String.IsNullOrEmpty(Front) || String.IsNullOrWhiteSpace(Front))
                    return nameof(Front);
                if (String.IsNullOrEmpty(Back) || String.IsNullOrWhiteSpace(Back))
                    return nameof(Back);
                if (String.IsNullOrEmpty(Confirm) || String.IsNullOrWhiteSpace(Confirm))
                    return nameof(Confirm);
            }
            return String.Empty;
        }

        public IEnumerable<ValidationError> Validate()
        {
            if (String.IsNullOrEmpty(ManageType) || String.IsNullOrWhiteSpace(ManageType))
                yield return new ValidationError() { ErrorMessage = String.Format(Constants.ERROR_MESSAGE_INVALID_VALUE, "Manage Type"), PropertyName = nameof(ManageType) };
            else if(!Enum.GetNames(typeof(Constants.ManageTypes)).Select(manageType => manageType.ToUpper()).ToList().Contains(ManageType.ToUpper()))
                yield return new ValidationError() { ErrorMessage = String.Empty, PropertyName = nameof(ManageType) };
            if(!String.IsNullOrEmpty(ManageType) && ManageType == Constants.ManageTypes.Modify.ToString())
            {
                if (String.IsNullOrEmpty(Front) || String.IsNullOrWhiteSpace(Front))
                    yield return new ValidationError() { ErrorMessage = String.Format(Constants.ERROR_MESSAGE_INVALID_VALUE, nameof(Front)), PropertyName = nameof(Front) };
                if (String.IsNullOrEmpty(Back) || String.IsNullOrWhiteSpace(Back))
                    yield return new ValidationError() { ErrorMessage = String.Format(Constants.ERROR_MESSAGE_INVALID_VALUE, nameof(Back)), PropertyName = nameof(Back) };
            }
        }
    }
}

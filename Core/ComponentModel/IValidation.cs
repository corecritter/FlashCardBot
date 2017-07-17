using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ComponentModel
{
    /// <summary>
    /// Anything that needs to be validated at some point should implement this
    /// </summary>
    public interface IValidation
    {
        IEnumerable<ValidationError> Validate();
    }
    /* Example
     Class : IValidation
     {
           string SomeProperty{get; set;}
           public IEnumerable<ValidationError> Validate()
           {
               if(String.IsNullOrEmpty(SomeProperty) || String.IsNullOrWhiteSpace(SomeProperty))
               {
                   yield return new ValidationError() { ErrorMessage = String.Format("{0} must have value", nameof(PropertyName)), PropertyName = nameof(SomeProperty)}
               }
           }
     }
     */
}

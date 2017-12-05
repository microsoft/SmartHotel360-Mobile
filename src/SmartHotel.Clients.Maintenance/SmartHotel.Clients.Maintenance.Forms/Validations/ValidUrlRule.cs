using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHotel.Clients.Core.Validations
{
    public class ValidUrlRule : IValidationRule<string>
    {
        public ValidUrlRule()
        {
            ValidationMessage = "Should be an URL";
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            ValidationContext ctx = new ValidationContext(this);
            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateValue(value, ctx, results, new[] { new DataTypeAttribute(DataType.Url) } ))
            {
                return false;
            }

            return results.TrueForAll(r => r == ValidationResult.Success);
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SmartHotel.Clients.Core.Validations
{
    public class EmailRule : IValidationRule<string>
    {
        public EmailRule() => ValidationMessage = "Should be an email address";

        public string ValidationMessage { get; set; }

        public bool Check(string value) => new EmailAddressAttribute().IsValid(value);
    }
}

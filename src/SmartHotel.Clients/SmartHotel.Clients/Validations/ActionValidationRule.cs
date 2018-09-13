using System;

namespace SmartHotel.Clients.Core.Validations
{
    public class ActionValidationRule<T> : IValidationRule<T>
    {
        readonly Func<T, bool> predicate;

        public string ValidationMessage { get; set; }

        public ActionValidationRule(Func<T, bool> predicate, string validationMessage)
        {
            this.predicate = predicate;
            ValidationMessage = validationMessage;
        }

        public bool Check(T value) => predicate.Invoke(value);
    }
}

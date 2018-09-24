using System;

namespace SmartHotel.Clients.Core.Validations
{
    public class ActionValidationRule<T> : IValidationRule<T>
    {
        readonly Func<T, bool> _predicate;

        public string ValidationMessage { get; set; }

        public ActionValidationRule(Func<T, bool> predicate, string validationMessage)
        {
            _predicate = predicate;
            ValidationMessage = validationMessage;
        }

        public bool Check(T value) => _predicate.Invoke(value);
    }
}

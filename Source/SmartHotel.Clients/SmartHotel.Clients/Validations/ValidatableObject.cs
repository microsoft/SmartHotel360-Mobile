using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Validations
{
    public class ValidatableObject<T> : BindableObject, IValidity
    {
        T mainValue;
        bool isValid;

        public List<IValidationRule<T>> Validations { get; }

        public ObservableRangeCollection<string> Errors { get; }

        public T Value
        {
            get => mainValue;

            set
            {
                mainValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get => isValid;

            set
            {
                isValid = value;
                Errors.Clear();
                OnPropertyChanged();
            }
        }

        public ValidatableObject()
        {
            isValid = true;
            Errors = new ObservableRangeCollection<string>();
            Validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            var errors = Validations.Where(v => !v.Check(Value))                        
                .Select(v => v.ValidationMessage);

            foreach (var error in errors)
            {
                Errors.Add(error);
            }

            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}

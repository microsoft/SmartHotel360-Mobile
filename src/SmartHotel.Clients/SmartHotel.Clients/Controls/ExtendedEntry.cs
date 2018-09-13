using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
    public class ExtendedEntry : Entry
    {
        Color lineColorToApply;

        public ExtendedEntry()
        {
            Focused += OnFocused;
            Unfocused += OnUnfocused;

            ResetLineColor();
        }

        public Color LineColorToApply
        {
            get => lineColorToApply;
            private set
            {
                lineColorToApply = value;
                OnPropertyChanged(nameof(LineColorToApply));
            }
        }

        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create("LineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public static readonly BindableProperty FocusLineColorProperty =
            BindableProperty.Create("FocusLineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color FocusLineColor
        {
            get => (Color)GetValue(FocusLineColorProperty);
            set => SetValue(FocusLineColorProperty, value);
        }

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid", typeof(bool), typeof(ExtendedEntry), true);

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public static readonly BindableProperty InvalidLineColorProperty =
            BindableProperty.Create("InvalidLineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color InvalidLineColor
        {
            get => (Color)GetValue(InvalidLineColorProperty);
            set => SetValue(InvalidLineColorProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsValidProperty.PropertyName)
            {
                CheckValidity();
            }
        }

        void OnFocused(object sender, FocusEventArgs e)
        {
            IsValid = true;
            LineColorToApply = FocusLineColor != Color.Default
                ? FocusLineColor
                : GetNormalStateLineColor();
        }

        void OnUnfocused(object sender, FocusEventArgs e) => ResetLineColor();

        void ResetLineColor() => LineColorToApply = GetNormalStateLineColor();

        void CheckValidity()
        {
            if (!IsValid)
            {
                LineColorToApply = InvalidLineColor;
            }
        }

        Color GetNormalStateLineColor() => LineColor != Color.Default
                    ? LineColor
                    : TextColor;
    }
}

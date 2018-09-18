using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
    public class CustomSlider : ContentView
    {
        private bool _alreadyAllocated = false;

        public CustomSlider()
        {
            Initialize();
        }

        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create("Minimum", typeof(double), typeof(CustomSlider), 0.0d, propertyChanged: MinimumChanged);

        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create("Maximum", typeof(double), typeof(CustomSlider), 100.0d, propertyChanged: MaximumChanged);

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value", typeof(double), typeof(CustomSlider), default(double),
                BindingMode.TwoWay, propertyChanged: ValueChanged);

        public static readonly BindableProperty BackgroundImageProperty =
            BindableProperty.Create("BackgroundImage", typeof(ImageSource), typeof(CustomSlider), default(ImageSource),
                BindingMode.TwoWay, propertyChanged: BackgroundImageChanged);

        public static readonly BindableProperty ThumbImageProperty =
            BindableProperty.Create("ThumbImage", typeof(ImageSource), typeof(CustomSlider), default(ImageSource),
                BindingMode.TwoWay, propertyChanged: ThumbImageChanged);

        public static readonly BindableProperty DisplayConverterProperty =
            BindableProperty.Create("DisplayConverter", typeof(IValueConverter), typeof(CustomSlider), default(IValueConverter),
                BindingMode.OneWay, propertyChanged: DisplayConverterChanged);

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        public ImageSource ThumbImage
        {
            get { return (ImageSource)GetValue(ThumbImageProperty); }
            set { SetValue(ThumbImageProperty, value); }
        }

        public IValueConverter DisplayConverter
        {
            get { return (IValueConverter)GetValue(DisplayConverterProperty); }
            set { SetValue(DisplayConverterProperty, value); }
        }

        protected Slider SliderControl { get; set; }

        protected Image ThumbImageControl { get; set; }

        protected Image BackgroundImageControl { get; set; }

        protected Label ValueControl { get; set; }

        protected StackLayout ValueContainer { get; set; }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (!_alreadyAllocated && SliderControl.Bounds.Width > -1 && SliderControl.Bounds.Height > -1)
            {
                if (Device.Idiom != TargetIdiom.Desktop)
                    _alreadyAllocated = true;

                MoveIndicator(SliderControl.Value);
            }
        }

        private void Initialize()
        {
            Grid content = new Grid()
            {
                Padding = new Thickness(4, 0)
            };

            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            content.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            SliderControl = new Slider
            {
                Minimum = Minimum,
                Maximum = Maximum,
                Value = Value,
                Opacity = Device.RuntimePlatform != Device.iOS ? 0 : 0.011,
                HeightRequest = 24,
                VerticalOptions = LayoutOptions.Start
            };

            SliderControl.ValueChanged += (sender, args) =>
            {
                MoveIndicator(args.NewValue);
            };

            Grid.SetRow(SliderControl, 0);
            content.Children.Add(SliderControl);

            BackgroundImageControl = new Image
            {
                Margin = new Thickness(
                    -content.Padding.Left / 2,
                    Device.RuntimePlatform == Device.UWP ? 8 : -5,
                    -content.Padding.Right / 2, 
                    0),
                Aspect = Device.Idiom == TargetIdiom.Desktop ? Aspect.AspectFill : Aspect.AspectFit,
                HeightRequest = Device.Idiom == TargetIdiom.Desktop ? 12 : 18,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true
            };

            Grid.SetRow(BackgroundImageControl, 0);
            content.Children.Add(BackgroundImageControl);

            ValueContainer = new StackLayout
            {
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Start,
                InputTransparent = true
            };

            ThumbImageControl = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Device.RuntimePlatform == Device.UWP ? 28 : 20,
                InputTransparent = true
            };

            Grid.SetRow(ValueContainer, 0);
            Grid.SetRowSpan(ValueContainer, 2);
            content.Children.Add(ValueContainer);
            ValueContainer.Children.Add(ThumbImageControl);

            ValueControl = new Label
            {
                Margin = new Thickness(0, -6, 0, 0),
                Style = Application.Current.Resources["PoppinsMediumLabelStyle"] as Style,
                FontSize = (Application.Current.Resources["MidMediumSize"] as Double?) ?? 12.0,
                HorizontalOptions = LayoutOptions.Center
            };

            UpdateDisplayValue();
            ValueContainer.Children.Add(ValueControl);

            Content = content;
        }

        private void MoveIndicator(double v)
        {
            var slideIndicatorPosition = CalculatePosition(v);
            UpdateDisplayValue();

            var thumbHalf = ThumbImageControl.Bounds.Width / 2;
            var diff = (ValueContainer.Bounds.Width - ThumbImageControl.Bounds.Width) / 2;

            ValueContainer.TranslationX = slideIndicatorPosition - diff - thumbHalf;
        }

        private void UpdateDisplayValue()
        {
            ValueControl.Text = DisplayConverter != null
                ? $"{DisplayConverter.Convert(SliderControl.Value, typeof(string), null, CultureInfo.CurrentUICulture)}"
                : SliderControl.Value.ToString("N0");

            Value = SliderControl.Value;
        }

        private double CalculatePosition(double value)
        {
            double rangeDiff = Maximum - Minimum;
            double valueDiff = value - Minimum;
            double ratio = valueDiff / rangeDiff;

            return SliderControl.Bounds.Width * ratio;
        }

        private static void ValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;

            customSlider.SliderControl.Value = Convert.ToDouble(newValue);
            customSlider.CalculatePosition(Convert.ToDouble(newValue));
        }

        private static void BackgroundImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;
            customSlider.BackgroundImageControl.Source = (ImageSource)newValue;
        }

        private static void ThumbImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;
            customSlider.ThumbImageControl.Source = (ImageSource)newValue;
        }

        private static void MinimumChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;
            customSlider.SliderControl.Minimum = (double)newValue;
        }

        private static void MaximumChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;
            customSlider.SliderControl.Maximum = (double)newValue;
        }

        private static void DisplayConverterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customSlider = (CustomSlider)bindable;
            customSlider?.UpdateDisplayValue();
        }
    }
}
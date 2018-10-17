using System;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SmartHotel.Clients.NFC.Controls
{
    public class AnimatedDonutChart : ContentView
    {
        static Color disabledColor = Color.FromHex("#D3D3D3");

        float currentValue;
        ChartView chart;

        public static readonly BindableProperty PercentageProperty =
          BindableProperty.Create(propertyName: nameof(Percentage),
              returnType: typeof(double),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: default(double),
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: OnEntryPropertyChanged);

        public double Percentage
        {
            get => (double)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        public static readonly BindableProperty DefaultColorProperty =
          BindableProperty.Create(propertyName: nameof(DefaultColor),
              returnType: typeof(Color),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: disabledColor,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public Color DefaultColor
        {
            get => (Color)GetValue(DefaultColorProperty);
            set => SetValue(DefaultColorProperty, value);
        }

        public static readonly BindableProperty ValueColorProperty =
          BindableProperty.Create(propertyName: nameof(ValueColor),
              returnType: typeof(Color),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: Color.FromHex("#009DD9"),
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public Color ValueColor
        {
            get => (Color)GetValue(ValueColorProperty);
            set => SetValue(ValueColorProperty, value);
        }

        public static readonly BindableProperty SpeedProperty =
          BindableProperty.Create(propertyName: nameof(Speed),
              returnType: typeof(float),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: 1f,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public float Speed
        {
            get => (float)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static readonly BindableProperty StrokeHeightProperty =
          BindableProperty.Create(propertyName: nameof(StrokeHeight),
              returnType: typeof(int),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: 30,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public int StrokeHeight
        {
            get => (int)GetValue(StrokeHeightProperty);
            set => SetValue(StrokeHeightProperty, value);
        }

        public static readonly BindableProperty EnabledProperty =
          BindableProperty.Create(propertyName: nameof(Enabled),
              returnType: typeof(bool),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: true,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty);
            set => SetValue(EnabledProperty, value);
        }

        public AnimatedDonutChart() : base()
        {
            currentValue = 0;

            chart = new ChartView()
            {
                BackgroundColor = Color.Transparent,
                Chart = CreateChart(currentValue, Height)
            };

            chart.PaintSurface += OnPaintCanvas;

            Content = chart;
        }

        async void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (Percentage > 0)
            {
                if (currentValue <= Percentage)
                {
                    ((ChartView)sender).Chart = CreateChart(currentValue, Height);
                    currentValue += Speed;
                }
                else
                {
                    ((ChartView)sender).PaintSurface -= OnPaintCanvas;
                }
            }
            else
            {
                ((ChartView)sender).Chart = CreateChart(currentValue, Height);
            }

            await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            ((ChartView)sender).InvalidateSurface();
        }

        static void OnEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as AnimatedDonutChart).currentValue = 0;
            (bindable as AnimatedDonutChart).chart.PaintSurface -= (bindable as AnimatedDonutChart).OnPaintCanvas;
            (bindable as AnimatedDonutChart).chart.PaintSurface += (bindable as AnimatedDonutChart).OnPaintCanvas;
            (bindable as AnimatedDonutChart).chart.InvalidateSurface();
        }

        Chart CreateChart(float i, double height)
        {
            var entries = new[]
            {
                new Microcharts.Entry(i)
                {
                    Color = Enabled? ValueColor.ToSKColor() : disabledColor.ToSKColor()
                },
                new Microcharts.Entry(100 - i)
                {
                    Color = Enabled? DefaultColor.ToSKColor() : disabledColor.ToSKColor()
                }
            };

            return new CustomDonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                HoleRadius = StrokeHeight
            };
        }
    }
}
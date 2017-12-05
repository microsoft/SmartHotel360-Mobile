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
        private static Color _disabledColor = Color.FromHex("#D3D3D3");

        private float _currentValue;
        private ChartView _chart;

        public static readonly BindableProperty PercentageProperty =
          BindableProperty.Create(propertyName: nameof(Percentage),
              returnType: typeof(double),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: default(double),
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: OnEntryPropertyChanged);

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public static readonly BindableProperty DefaultColorProperty =
          BindableProperty.Create(propertyName: nameof(DefaultColor),
              returnType: typeof(Color),
              declaringType: typeof(AnimatedDonutChart),
              defaultValue: _disabledColor,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: OnEntryPropertyChanged);

        public Color DefaultColor
        {
            get { return (Color)GetValue(DefaultColorProperty); }
            set { SetValue(DefaultColorProperty, value); }
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
            get { return (Color)GetValue(ValueColorProperty); }
            set { SetValue(ValueColorProperty, value); }
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
            get { return (float)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
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
            get { return (int)GetValue(StrokeHeightProperty); }
            set { SetValue(StrokeHeightProperty, value); }
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
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public AnimatedDonutChart() : base()
        {
            _currentValue = 0;

            _chart = new ChartView()
            {
                BackgroundColor = Color.Transparent,
                Chart = this.CreateChart(_currentValue, Height)
            };

            _chart.PaintSurface += OnPaintCanvas;

            Content = _chart;
        }

        private async void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.Percentage > 0)
            {
                if (this._currentValue <= Percentage)
                {
                    ((ChartView)sender).Chart = CreateChart(_currentValue, Height);
                    _currentValue += Speed;
                }
                else
                {
                    ((ChartView)sender).PaintSurface -= OnPaintCanvas;
                }
            }
            else
            {
                ((ChartView)sender).Chart = CreateChart(_currentValue, Height);
            }

            await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            ((ChartView)sender).InvalidateSurface();
        }

        private static void OnEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as AnimatedDonutChart)._currentValue = 0;
            (bindable as AnimatedDonutChart)._chart.PaintSurface -= (bindable as AnimatedDonutChart).OnPaintCanvas;
            (bindable as AnimatedDonutChart)._chart.PaintSurface += (bindable as AnimatedDonutChart).OnPaintCanvas;
            (bindable as AnimatedDonutChart)._chart.InvalidateSurface();
        }

        private Chart CreateChart(float i, double height)
        {
            var entries = new[]
            {
                new Microcharts.Entry(i)
                {
                    Color = this.Enabled? this.ValueColor.ToSKColor() : _disabledColor.ToSKColor()
                },
                new Microcharts.Entry(100 - i)
                {
                    Color = this.Enabled? this.DefaultColor.ToSKColor() : _disabledColor.ToSKColor()
                }
            };

            return new CustomDonutChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                HoleRadius = this.StrokeHeight
            };
        }
    }
}
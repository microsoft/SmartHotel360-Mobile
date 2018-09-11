using System;
using System.Linq;
using Microcharts;
using SkiaSharp;

namespace SmartHotel.Clients.Core.Controls
{
    public class LightChart : TemperatureChart
    {
        protected override void DrawChart(SKCanvas canvas, Entry entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = entry.Color,
                IsAntialias = true
            })
            {
                using (var path = new SKPath())
                {
                    var percent = (Math.Abs(entry.Value) - AbsoluteMinimum) / ValueRange;
                    path.AddRoundedRect(SKRect.Create(cx - radius - strokeWidth, cy / 2 + strokeWidth, (2 * radius) * percent, 2), 0.05f, 0.05f);

                    canvas.DrawPath(path, paint);
                }
            }
        }

        protected override void DrawCaption(SKCanvas canvas, int cx, int cy, float radius, float relativeScaleWidth,
            float strokeWidth)
        {
            var values = Entries.ToList();
            var currentValue = values.FirstOrDefault();

            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"{currentValue.Value}K", SKColor.Parse("#283748"), LabelTextSize * relativeScaleWidth, new SKPoint(cx - strokeWidth - CaptionMargin, cy / 2 + 3* strokeWidth), SKTextAlign.Center);
        }
    }
}
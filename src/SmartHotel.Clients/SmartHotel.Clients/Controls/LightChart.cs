using System;
using System.Linq;
using Microcharts;
using SkiaSharp;

namespace SmartHotel.Clients.Core.Controls
{
    public class LightChart : TemperatureChart
    {
        public LightChart() : base()
        {
            LineSize = 14;
            LabelTextSize = 16f;
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var relativeScaleWidth = width / 465.0f;
            var strokeWidth = relativeScaleWidth * LineSize;

            var radius = (width) / 2;
            int cx = (int)(radius + strokeWidth);
            var cy = Convert.ToInt32(height / 1.25);
            var radiusSpace = radius - 4 * strokeWidth;

            DrawChart(canvas, width, height, cx, cy, radiusSpace, strokeWidth, relativeScaleWidth);
        }

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
                    path.AddRoundedRect(SKRect.Create(cx - radius - strokeWidth, cy / 2f + strokeWidth, (2 * radius) * percent, 2), 0.05f, 0.05f);

                    canvas.DrawPath(path, paint);
                }
            }
        }

        protected override void DrawCaption(SKCanvas canvas, int cx, int cy, float radius, float relativeScaleWidth,
            float strokeWidth)
        {
            var values = Entries.ToList();
            var currentValue = values.FirstOrDefault();
            var desiredValue = values.Skip(1).Take(1).FirstOrDefault();

            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"Current: {currentValue?.Value}K", SKColor.Parse("#283748"), LabelTextSize * relativeScaleWidth,
                new SKPoint(cx - CaptionMargin, cy / 2f + 3 * strokeWidth), SKTextAlign.Center);

            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"Desired: {desiredValue?.Value}K", SKColor.Parse("#378D93"), LabelTextSize * relativeScaleWidth,
                new SKPoint(cx - CaptionMargin, cy / 2f + 3 * strokeWidth * 1.45f), SKTextAlign.Center);
        }
    }
}
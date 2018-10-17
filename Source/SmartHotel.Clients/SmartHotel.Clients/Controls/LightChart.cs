using System;
using System.Diagnostics;
using System.Linq;
using Microcharts;
using SkiaSharp;

namespace SmartHotel.Clients.Core.Controls
{
    public class LightChart : TemperatureChart
    {
        public LightChart() : base()
        {
            LineSize = 21;
            LabelTextSize = 16f;
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var relativeScaleWidth = width / 465.0f;
            var relativeScaleHeight = height / 270.0f;
            var strokeWidth = relativeScaleWidth  * LineSize;

            var radius = (width) / 2;
            var cx = (int)(radius + strokeWidth);
            var cy = Convert.ToInt32(height);
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
                var percent = (Math.Abs(entry.Value) - AbsoluteMinimum) / ValueRange;
                var x = cx - radius - strokeWidth;
                var y = cy / 2f - strokeWidth + LabelTextSize;
                canvas.DrawLine(x, y, x + (2 * radius) * percent, y, paint);
            }
        }

        protected override void DrawCaption(SKCanvas canvas, int cx, int cy, float radius, float relativeScaleWidth,
            float strokeWidth)
        {
            if (CurrentValueEntry != null)
            {
                canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"{CurrentValueEntry.Value}%", SKColor.Parse("#283748"),
                    LabelTextSize * relativeScaleWidth,
                    new SKPoint(cx - CaptionMargin, cy / 2f + LabelTextSize), SKTextAlign.Center);
            }

            // uncomment to add Desired value
            //var desiredValue = values.Skip(1).Take(1).FirstOrDefault();
            //canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"Desired: {desiredValue?.Value}%", SKColor.Parse("#378D93"), LabelTextSize * relativeScaleWidth,
            //    new SKPoint(cx - CaptionMargin, cy / 2f + 3 * strokeWidth * 1.45f), SKTextAlign.Center);
        }
    }
}
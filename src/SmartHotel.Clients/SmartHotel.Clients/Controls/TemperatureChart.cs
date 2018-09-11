using Microcharts;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SmartHotel.Clients.Core.Controls
{
    public class TemperatureChart : Chart
    {
        public TemperatureChart()
        {
            BackgroundColor = SKColor.Parse("#F2F2F2");
        }

        public float CaptionMargin { get; set; } = 12;

        public float LineSize { get; set; } = 18;

        public float StartAngle { get; set; } = -180;

        public bool CompactMargins { get; set; }

        private float AbsoluteMinimum => Entries.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private float AbsoluteMaximum => Entries.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        private float ValueRange => AbsoluteMaximum - AbsoluteMinimum;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var relativeScaleWidth = width / 465.0f;
            var strokeWidth = relativeScaleWidth * LineSize;

            var relativeMargin = Margin; 
            var radius = (width - 2 * relativeMargin) / 2;
            int cx = (int)(radius + LineSize * relativeScaleWidth);
            var cy = Convert.ToInt32(height / 1.25);
            var radiusSpace = radius - 4 * LineSize * relativeScaleWidth; 

            foreach (var entry in Entries.OrderByDescending(e => e.Value))
            {
                DrawChart(canvas, entry, radiusSpace, cx, cy, strokeWidth);
            }

            DrawCaption(canvas, cx, cy, radiusSpace, relativeScaleWidth);

            Debug.WriteLine($"relativeScaleWidth:{relativeScaleWidth}");
        }

        public void DrawChart(SKCanvas canvas, Entry entry, float radius, int cx, int cy, float strokeWidth)
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
                using (SKPath path = new SKPath())
                {
                    // Draws the temp chart in a manner that looks similar to the Ambient Light picture
                    //var percent = (Math.Abs(entry.Value) - AbsoluteMinimum) / ValueRange;
                    //path.AddRoundedRect(SKRect.Create(cx - radius, cy, (2 * radius) * percent, 2), 0.05f, 0.05f);

                    // Draws the normal temp chart as an arc
                    var sweepAngle = 180 * (Math.Abs(entry.Value) - AbsoluteMinimum) / ValueRange;
                    path.AddArc(SKRect.Create(cx - radius, cy - radius, 2 * radius, 2 * radius), StartAngle, sweepAngle);

                    canvas.DrawPath(path, paint);
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int cx, int cy, float radius, float relativeScaleWidth)
        {
            //var minimum = 0;
            //var medium = Math.Round(Entries.Max(e => e.Value) / 2);
            //var maximum = Entries.Max(e => e.Value);

            var medium = AbsoluteMinimum + ((AbsoluteMaximum - AbsoluteMinimum) / 2);

            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"{AbsoluteMinimum}°", SKColors.Black, LabelTextSize * relativeScaleWidth, new SKPoint(cx - radius - LineSize * relativeScaleWidth - CaptionMargin, cy), SKTextAlign.Center);
            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"{medium}°", SKColors.Black, LabelTextSize * relativeScaleWidth, new SKPoint(cx, cy - radius - LineSize * relativeScaleWidth - 2 * relativeScaleWidth), SKTextAlign.Center);
            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"{AbsoluteMaximum}°", SKColors.Black, LabelTextSize * relativeScaleWidth, new SKPoint(cx + radius + LineSize * relativeScaleWidth + CaptionMargin, cy), SKTextAlign.Center);

            var values = Entries.ToList();
            var currentValue = values.FirstOrDefault();
            var desiredValue = values.Skip(1).Take(1).FirstOrDefault();

            var degreeSign = '°';
            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"Current: {currentValue?.Value}{degreeSign}", SKColor.Parse("#174A51"), LabelTextSize * relativeScaleWidth, new SKPoint(cx, cy - radius * 1.8f / 4f), SKTextAlign.Center);
            canvas.DrawCaptionLabels(string.Empty, SKColor.Empty, $"Desired: {desiredValue?.Value}{degreeSign}", SKColor.Parse("#378D93"), LabelTextSize * relativeScaleWidth, new SKPoint(cx, cy - radius * 1.1f / 4f), SKTextAlign.Center);
        }
        
    }
}
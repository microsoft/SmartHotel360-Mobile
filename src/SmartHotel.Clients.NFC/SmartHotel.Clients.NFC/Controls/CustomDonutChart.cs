using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Microcharts;
using SmartHotel.Clients.NFC.Helpers;

namespace SmartHotel.Clients.NFC.Controls
{
    public class CustomDonutChart : Chart
    {
        public float HoleRadius { get; set; } = 0;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            DrawCaption(canvas, width, height);
            using (new SKAutoCanvasRestore(canvas))
            {
                canvas.Translate(width / 2, height / 2);
                var sumValue = Entries.Sum(x => Math.Abs(x.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

                var start = 0.0f;
                for (var i = 0; i < Entries.Count(); i++)
                {
                    var entry = Entries.ElementAt(i);
                    var end = start + (Math.Abs(entry.Value) / sumValue);

                    var path = RadialHelpers.CreateSectorPath(start, end, radius, radius - HoleRadius);
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                        IsAntialias = true,
                    })
                    {
                        canvas.DrawPath(path, paint);
                    }

                    start = end;
                }
            }
        }

        void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var sumValue = Entries.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<Entry>();
            var leftValues = new List<Entry>();

            var i = 0;
            var current = 0.0f;
            while (i < Entries.Count() && (current < sumValue / 2))
            {
                var entry = Entries.ElementAt(i);
                rightValues.Add(entry);
                current += Math.Abs(entry.Value);
                i++;
            }

            while (i < Entries.Count())
            {
                var entry = Entries.ElementAt(i);
                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false);
            DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}
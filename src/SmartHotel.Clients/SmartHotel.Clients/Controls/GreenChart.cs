using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotel.Clients.Core.Controls
{
    public class GreenChart : Chart
    {
        public GreenChart()
        {
            BackgroundColor = SKColor.Parse("#F6F1E9");
        }

        public float GreenChartLabelTextSize { get; set; } = 9;

        public float PointSize { get; set; } = 8;

        public PointMode PointMode { get; set; } = PointMode.Circle;

        private float ValueRange => MaxValue - MinValue;

        public float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var valueLabelSizes = MeasureValueLabels();
            var footerHeight = CalculateFooterHeight(valueLabelSizes);
            var headerHeight = CalculateHeaderHeight(valueLabelSizes);
            var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
            var origin = CalculateYOrigin(itemSize.Height, headerHeight);
            var points = CalculatePoints(itemSize, origin, headerHeight);

            DrawPointAreas(canvas, points, origin);
            DrawPoints(canvas, points);
            DrawFooter(canvas, points, itemSize, height, footerHeight);
        }

        protected SKSize CalculateItemSize(int width, int height, float footerHeight, float headerHeight)
        {
            var total = Entries.Count();
            var w = (width - ((total + 1) * Margin)) / total;
            var h = height - Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        protected SKPoint[] CalculatePoints(SKSize itemSize, float origin, float headerHeight)
        {
            var result = new List<SKPoint>();

            for (int i = 0; i < Entries.Count(); i++)
            {
                var entry = Entries.ElementAt(i);

                var x = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));
                var y = headerHeight + (((MaxValue - entry.Value) / ValueRange) * itemSize.Height);
                var point = new SKPoint(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }

        protected void DrawFooter(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            this.DrawLabels(canvas, points, itemSize, height, footerHeight);
        }

        protected void DrawLabels(SKCanvas canvas, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            for (int i = 0; i < Entries.Count(); i++)
            {
                var entry = Entries.ElementAt(i);
                var point = points[i];

                if (!string.IsNullOrEmpty(entry.Label))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = GreenChartLabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = entry.TextColor;
                        paint.IsStroke = false;

                        var bounds = new SKRect();
                        var text = entry.Label;
                        paint.MeasureText(text, ref bounds);

                        canvas.DrawText(text, point.X - (bounds.Width / 2), height - Margin + (GreenChartLabelTextSize / 2), paint);
                    }
                }
            }
        }

        protected void DrawPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, entry.Color, PointSize, PointMode);
                }
            }
        }

        protected void DrawPointAreas(SKCanvas canvas, SKPoint[] points, float origin)
        {
            if (points.Length > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    var y = Math.Min(origin, point.Y);

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color
                    })
                    {
                        var height = Math.Max(2, Math.Abs(origin - point.Y));
                        canvas.DrawRect(SKRect.Create(point.X - (PointSize / 2), y, PointSize, height), paint);
                    }
                }
            }
        }

        protected float CalculateFooterHeight(SKRect[] valueLabelSizes)
        {
            var result = Margin;

            if (Entries.Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                result += LabelTextSize - Margin;
            }

            return result;
        }

        protected float CalculateHeaderHeight(SKRect[] valueLabelSizes)
        {
            var result = Margin;

            if (Entries.Any())
            {
                var maxValueWidth = valueLabelSizes.Max(x => x.Width);

                if (maxValueWidth > 0)
                {
                    result += maxValueWidth + Margin;
                }
            }

            return result;
        }

        protected SKRect[] MeasureValueLabels()
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = LabelTextSize;
                return Entries.Select(e =>
                {
                    if (string.IsNullOrEmpty(e.ValueLabel))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    var text = e.ValueLabel;
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
        }
    }
}
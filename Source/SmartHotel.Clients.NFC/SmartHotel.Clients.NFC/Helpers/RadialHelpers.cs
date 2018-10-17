namespace SmartHotel.Clients.NFC.Helpers
{
    using System;
    using SkiaSharp;

    static class RadialHelpers
    {
        public const float PI = (float)Math.PI;

        const float uprightAngle = PI / 2f;
        const float totalAngle = 2f * PI;

        public static SKPoint GetCirclePoint(float r, float angle)
        {
            return new SKPoint(r * (float)Math.Cos(angle), r * (float)Math.Sin(angle));
        }

        public static SKPath CreateSectorPath(float start, float end, float outerRadius, float innerRadius = 0.0f, float margin = 0.0f)
        {
            var path = new SKPath();

            // If the sector has no size, then it has no path
            if (start == end)
            {
                return path;
            }

            // The the sector is a full circle, then do that
            if (end - start == 1.0f)
            {
                path.AddCircle(0, 0, outerRadius, SKPathDirection.Clockwise);
                path.AddCircle(0, 0, innerRadius, SKPathDirection.Clockwise);
                path.FillType = SKPathFillType.EvenOdd;
                return path;
            }

            // Calculate the angles
            var startAngle = (totalAngle * start) - uprightAngle;
            var endAngle = (totalAngle * end) - uprightAngle;
            var large = endAngle - startAngle > PI ? SKPathArcSize.Large : SKPathArcSize.Small;
            var sectorCenterAngle = ((endAngle - startAngle) / 2f) + startAngle;

            // Get the radius bits
            var cectorCenterRadius = ((outerRadius - innerRadius) / 2f) + innerRadius;

            // Calculate the angle for the margins
            var offsetR = outerRadius == 0 ? 0 : ((margin / (totalAngle * outerRadius)) * totalAngle);
            var offsetr = innerRadius == 0 ? 0 : ((margin / (totalAngle * innerRadius)) * totalAngle);

            // Get the points
            var a = GetCirclePoint(outerRadius, startAngle + offsetR);
            var b = GetCirclePoint(outerRadius, endAngle - offsetR);
            var c = GetCirclePoint(innerRadius, endAngle - offsetr);
            var d = GetCirclePoint(innerRadius, startAngle + offsetr);

            // Add the points to the path
            path.MoveTo(a);
            path.ArcTo(outerRadius, outerRadius, 0, large, SKPathDirection.Clockwise, b.X, b.Y);
            path.LineTo(c);

            if (innerRadius == 0.0f)
            {
                // Take a short cut
                path.LineTo(d);
            }
            else
            {
                path.ArcTo(innerRadius, innerRadius, 0, large, SKPathDirection.CounterClockwise, d.X, d.Y);
            }

            path.Close();

            return path;
        }
    }
}
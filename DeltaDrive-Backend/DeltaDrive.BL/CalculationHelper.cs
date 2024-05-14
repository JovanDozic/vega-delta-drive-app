using NetTopologySuite.Geometries;

namespace DeltaDrive.BL
{
    public static class CalculationHelper
    {
        public static double CalculateDistance(Point point1, Point point2)
        {
            var R = 6371;
            var dLat = ToRadians(point2.Y - point1.Y);
            var dLon = ToRadians(point2.X - point1.X);
            var lat1 = ToRadians(point1.Y);
            var lat2 = ToRadians(point2.Y);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}

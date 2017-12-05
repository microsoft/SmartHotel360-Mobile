using Windows.UI;

namespace SmartHotel.Clients.UWP.Extensions
{
    internal static class ColorExtensions
    {
        public static Color ToUwp(this Xamarin.Forms.Color color)
        {
            return Color.FromArgb((byte)(color.A * 255),
                                  (byte)(color.R * 255),
                                  (byte)(color.G * 255),
                                  (byte)(color.B * 255));
        }
    }
}

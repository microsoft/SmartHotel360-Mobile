namespace SmartHotel.Clients.Core
{
    public class MessengerKeys
    {
        // Booking keys
        public const string BookingRequested = "BookingRequested";
        public const string CheckoutRequested = "CheckoutRequested";

        // NFC
        public static string SendNFCToken = "SendNFCToken";

        // Login
        public static string SignInRequested = "SignInRequested";

        // Settings
        public static string LoadSettingsRequested = "LoadSettingsRequested";
    }
}

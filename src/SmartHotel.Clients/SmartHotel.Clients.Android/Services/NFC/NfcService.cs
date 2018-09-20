using Android.App;
using Android.Nfc;
using SmartHotel.Clients.Core.Services.NFC;
using SmartHotel.Clients.Droid.Services.NFC;

[assembly: Xamarin.Forms.Dependency(typeof(NfcService))]
namespace SmartHotel.Clients.Droid.Services.NFC
{
    public class NfcService : INfcService
    {
        NfcAdapter nfcDevice;

        public NfcService()
        {
            var activity = ((Activity)Xamarin.Forms.Forms.Context);
            nfcDevice = NfcAdapter.GetDefaultAdapter(activity);
        }

        public bool IsAvailable
        {
            get
            {
                return nfcDevice?.IsEnabled == true && nfcDevice.IsNdefPushEnabled;
            }
        }
    }
}
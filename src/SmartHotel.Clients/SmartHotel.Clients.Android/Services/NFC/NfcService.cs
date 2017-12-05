using Android.App;
using Android.Nfc;
using SmartHotel.Clients.Core.Services.NFC;
using SmartHotel.Clients.Droid.Services.NFC;

[assembly: Xamarin.Forms.Dependency(typeof(NfcService))]
namespace SmartHotel.Clients.Droid.Services.NFC
{
    public class NfcService : INfcService
    {
        private NfcAdapter _nfcDevice;

        public NfcService()
        {
            var activity = Xamarin.Forms.Forms.Context as Activity;
            _nfcDevice = NfcAdapter.GetDefaultAdapter(activity);
        }

        public bool IsAvailable
        {
            get
            {
                return _nfcDevice?.IsEnabled == true && _nfcDevice.IsNdefPushEnabled;
            }
        }
    }
}
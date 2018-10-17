using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
    public partial class iOSStatusBarBackground : ContentView
	{
		public iOSStatusBarBackground ()
		{
			InitializeComponent ();
            IsVisible = Device.RuntimePlatform == Device.iOS;
		}

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsVisibleProperty.PropertyName 
                && IsVisible 
                && Device.RuntimePlatform != Device.iOS)
            {
                IsVisible = false;
            }
        }
    }
}
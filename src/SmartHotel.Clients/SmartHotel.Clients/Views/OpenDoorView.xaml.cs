using Rg.Plugins.Popup.Pages;

namespace SmartHotel.Clients.Core.Views
{
    public partial class OpenDoorView : PopupPage
    {
		public OpenDoorView ()
		{
			InitializeComponent ();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
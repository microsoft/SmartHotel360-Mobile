using Rg.Plugins.Popup.Pages;

namespace SmartHotel.Clients.Core.Views
{
    public partial class CheckoutView : PopupPage
    {
		public CheckoutView ()
		{
			InitializeComponent ();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
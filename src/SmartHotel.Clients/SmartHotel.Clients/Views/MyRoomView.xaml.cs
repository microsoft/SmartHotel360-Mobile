using SmartHotel.Clients.Core.Helpers;
using SmartHotel.Clients.Core.ViewModels.Base;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Views
{
    public partial class MyRoomView : ContentPage
    {
        public MyRoomView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            StatusBarHelper.Instance.MakeTranslucentStatusBar(false);
	        if (BindingContext is IHandleViewAppearing viewAware)
	        {
		        await viewAware.OnViewAppearingAsync(this);
	        }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

	        if (BindingContext is IHandleViewDisappearing viewAware)
	        {
		        await viewAware.OnViewDisappearingAsync(this);
	        }
        }
    }
}
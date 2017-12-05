using SmartHotel.Clients.Live.ViewModels;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new LoginViewModel();
        }
    }
}
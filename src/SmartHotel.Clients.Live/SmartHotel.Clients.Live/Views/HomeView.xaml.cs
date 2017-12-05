using SmartHotel.Clients.Live.ViewModels;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.Views
{
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();

            BindingContext = new HomeViewModel();
        }
    }
}
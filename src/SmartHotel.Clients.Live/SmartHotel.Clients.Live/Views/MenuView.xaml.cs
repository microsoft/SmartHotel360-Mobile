using SmartHotel.Clients.Live.ViewModels;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.Views
{
    public partial class MenuView : ContentPage
    {
        public MenuView()
        {
            InitializeComponent();

            BindingContext = new MenuViewModel();
        }
    }
}
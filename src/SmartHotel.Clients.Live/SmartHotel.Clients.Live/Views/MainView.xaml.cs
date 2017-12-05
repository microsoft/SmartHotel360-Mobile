using SmartHotel.Clients.Live.ViewModels;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.Views
{
    public partial class MainView : MasterDetailPage
    {
        public MainView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new MainViewModel();
        }
    }
}
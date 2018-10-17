using SmartHotel.Clients.NFC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartHotel.Clients.NFC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new MainViewModel();
        }
    }
}
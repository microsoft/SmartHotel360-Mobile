using SmartHotel.Clients.Live.Services.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live
{
    public partial class App : Application
    {
        public static NavigationService NavigationService { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected async override void OnStart()
        {
            base.OnStart();

            await InitNavigation();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private Task InitNavigation()
        {
            NavigationService = new NavigationService();
            return NavigationService.InitializeAsync();
        }
    }
}

using SmartHotel.Clients.Live.ViewModels.Base;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Live.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private MenuViewModel _menuViewModel;

        public MainViewModel()
        {
            _menuViewModel = new MenuViewModel();
        }

        public MenuViewModel MenuViewModel
        {
            get
            {
                return _menuViewModel;
            }

            set
            {
                _menuViewModel = value;
                RaisePropertyChanged(() => MenuViewModel);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAll
            (
                _menuViewModel.InitializeAsync(navigationData)
            );
        }
    }
}
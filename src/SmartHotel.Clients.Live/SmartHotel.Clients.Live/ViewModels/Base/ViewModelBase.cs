using SmartHotel.Clients.Live.Services.Navigation;
using System;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Live.ViewModels.Base
{
    public class ViewModelBase : ExtendedBindableObject, IDisposable
    {
        protected readonly INavigationService NavigationService;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
                NavigationService = App.NavigationService; 
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public virtual void Dispose()
        {

        }
    }
}
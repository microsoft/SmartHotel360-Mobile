using SmartHotel.Clients.Core.Services.Dialog;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels.Base
{
    public class ViewModelBase : BindableObject
    {
        // For this maintenance sample we don't need all Clients project ViewModelBase stuff
        // This is a simplified version

        private bool _isBusy;

        protected readonly IDialogService DialogService;

        public ViewModelBase()
        {
            DialogService = new DialogService();
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}

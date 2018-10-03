using System.Threading.Tasks;
using SmartHotel.Clients.Core.Services.Dialog;

namespace SmartHotel.Clients.Core.ViewModels.Base
{
    public class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        protected readonly IDialogService DialogService;

        public ViewModelBase() => DialogService = new DialogService();

        public virtual Task InitializeAsync(object navigationData) => Task.FromResult(false);
    }
}
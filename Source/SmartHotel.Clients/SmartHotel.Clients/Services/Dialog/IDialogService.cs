using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void ShowToast(string message, int duration = 5000);
        Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel);
        Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options);
        Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options);
    }
}
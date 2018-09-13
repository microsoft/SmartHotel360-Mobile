using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Services.Dialog
{
    public class DialogService : IDialogService
    {
        Page MainPage => App.Current.MainPage;

        public Task ShowAlertAsync(string message, string title, string buttonLabel) => MainPage.DisplayAlert(title, message, buttonLabel);

        public void ShowToast(string message, int duration = 5000)
        {

        }

        public Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel) => MainPage.DisplayAlert(title, message, okLabel, cancelLabel);

        public Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options) => SelectActionAsync(message, title, "Cancel", options);

        public async Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options)
        {
            try
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                if (!options.Any())
                {
                    throw new ArgumentException("No options provided", nameof(options));
                }

                var result =
                    await MainPage.DisplayActionSheet(message, cancelLabel, null, buttons: options.ToArray());

                return options.Contains(result)
                    ? result
                    : cancelLabel;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
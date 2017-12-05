using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public void ShowToast(string message, int duration = 5000)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(duration);
            toastConfig.Position = Device.RuntimePlatform == Device. UWP ? ToastPosition.Top : ToastPosition.Bottom;

            // ICON
            /*
            var img = "notification_icon.png";
            var icon = await BitmapLoader.Current.LoadFromResource(img, null, null);
            toastConfig.SetIcon(icon);
            */

            toastConfig.SetMessageTextColor(System.Drawing.Color.White);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(33, 44, 55));

            UserDialogs.Instance.Toast(toastConfig);
        }

        public Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel)
        {
            return UserDialogs.Instance.ConfirmAsync(message, title, okLabel, cancelLabel);
        }

        public Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options)
        {
            return SelectActionAsync(message, title, "Cancel", options);
        }

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

                string result =
                    await UserDialogs.Instance.ActionSheetAsync(message, cancelLabel, null, buttons: options.ToArray());

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
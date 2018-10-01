using Newtonsoft.Json;
using Plugin.Vibrate;
using SmartHotel.Clients.NFC.Models;
using SmartHotel.Clients.NFC.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.NFC.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        int percentage;
        string title;
        string subTitle;
        string avatar;

        public MainViewModel()
        {
            Title = Resources.NfcMayus;
            SubTitle = Resources.GetPhone;
            Percentage = 0;

            MessagingCenter.Subscribe<string>(this, MessengerKeys.SendNFCToken, StartNFCService);
        }

        public int Percentage
        {
            get => percentage;
            set
            {
                percentage = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string SubTitle
        {
            get => subTitle;
            set
            {
                subTitle = value;
                OnPropertyChanged();
            }
        }

        public string Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResetCommand => new Command(Reset);

        void StartNFCService(string message)
        {
            try
            {
                CrossVibrate.Current.Vibration();

                var nfcParameter = JsonConvert.DeserializeObject<NfcParameter>(message);
             
                if (nfcParameter != null)
                {
                    Title = Resources.HelloMayus;
                    SubTitle = nfcParameter.Username;
                    Avatar = nfcParameter.Avatar;
                    Percentage = 100;
                }
            }
            catch(Exception ex)
            {
                Error();

                Debug.WriteLine($"[NFC] Error: {ex}");
            }
        }

        void Reset()
        {
            Title = Resources.NfcMayus;
            SubTitle = Resources.GetPhone;
            Avatar = string.Empty;
            Percentage = 0;
        }

        void Error()
        {
            Title = Resources.HelloMayus;
            SubTitle = "James Montemagno";
            Avatar = "james";
            Percentage = 100;
        }
    }
}
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Views.Templates
{
    public partial class NotificationItemTemplate : ContentView
    {
        public static readonly BindableProperty TapCommandProperty =
            BindableProperty.Create("TapCommand", typeof(ICommand), typeof(NotificationItemTemplate));

        public ICommand TapCommand
        {
            get => (ICommand)GetValue(TapCommandProperty);
            set => SetValue(TapCommandProperty, value);
        }

        public NotificationItemTemplate()
        {
            InitializeComponent();
        }
    }
}
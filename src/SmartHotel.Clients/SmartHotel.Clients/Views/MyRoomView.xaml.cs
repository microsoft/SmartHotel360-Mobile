using SmartHotel.Clients.Core.Helpers;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Views
{
    public partial class MyRoomView : ContentPage
    {
        public MyRoomView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StatusBarHelper.Instance.MakeTranslucentStatusBar(false);

            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, System.EventArgs e)
        {
            AmbientLightSlider.WidthRequest = Width;
        }
    }
}
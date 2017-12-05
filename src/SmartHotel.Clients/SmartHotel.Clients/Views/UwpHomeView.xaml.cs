using SmartHotel.Clients.Core.ViewModels.Base;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Views
{
    public partial class UwpHomeView : ContentPage
    {
        private const double MinWidth = 720;

        public UwpHomeView()
        {
            InitializeComponent();

            AdaptLayout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IHandleViewAppearing viewAware)
            {
                await viewAware.OnViewAppearingAsync(this);
            }

            this.SizeChanged += OnSizeChanged;
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is IHandleViewDisappearing viewAware)
            {
                await viewAware.OnViewDisappearingAsync(this);
            }

            this.SizeChanged -= OnSizeChanged;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            AdaptLayout();
        }

        private void OnSizeChanged(object sender, System.EventArgs e)
        {
            AdaptLayout();
        }

        private void AdaptLayout()
        {
            if (Width < 0)
            {
                return;
            }

            if (Width < MinWidth)
            {
                Grid.SetColumnSpan(RestaurantTitle, 2);
                Grid.SetColumnSpan(RestaurantContent, 2);
                RestaurantImage.IsVisible = false;
                MoreInfoTitle.IsVisible = false;
                MoreInfoContent.IsVisible = false;
                Grid.SetColumn(MoreInfoImage, 0);
                Grid.SetColumnSpan(MoreInfoImage, 2);
                BigCharts.IsVisible = false;
                SmallCharts.IsVisible = true;
            }
            else
            {
                Grid.SetColumnSpan(RestaurantTitle, 1);
                Grid.SetColumnSpan(RestaurantContent, 1);
                RestaurantImage.IsVisible = true;
                MoreInfoTitle.IsVisible = true;
                MoreInfoContent.IsVisible = true;
                Grid.SetColumn(MoreInfoImage, 1);
                Grid.SetColumnSpan(MoreInfoImage, 1);
                BigCharts.IsVisible = true;
                SmallCharts.IsVisible = false;
            }
        }
    }
}
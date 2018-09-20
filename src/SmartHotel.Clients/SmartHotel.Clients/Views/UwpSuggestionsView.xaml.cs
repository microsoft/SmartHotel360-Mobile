using SmartHotel.Clients.Core.Helpers;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Views
{
    public partial class UwpSuggestionsView : ContentPage
    {
        const double minWidth = 720;

        public UwpSuggestionsView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, string.Empty);

            InitializeComponent();

            MapHelper.CenterMapInDefaultLocation(MapControl);
            AdaptLayout();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SizeChanged += OnSizeChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            SizeChanged -= OnSizeChanged;
        }

        void OnSizeChanged(object sender, System.EventArgs e) => AdaptLayout();

        void AdaptLayout()
        {
            if (Width < 0)
            {
                return;
            }

            if (Width < minWidth)
            {
                Grid.SetColumn(Map, 0);
                Grid.SetColumnSpan(Map, 2);
                LeftSuggestionList.IsVisible = false;
                BottomSuggestionList.IsVisible = true;
            }
            else
            {
                Grid.SetColumn(Map, 1);
                Grid.SetColumnSpan(Map, 1);
                LeftSuggestionList.IsVisible = true;
                BottomSuggestionList.IsVisible = false;
            }
        }
    }
}
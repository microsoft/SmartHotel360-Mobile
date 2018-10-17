using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SmartHotel.Clients.UITests.Pages
{
    class BookCityPage : BasePage
    {
        readonly Query backButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("booking"),
            iOS = x => x.Marked("booking")
        };

        public BookCityPage()
        {
            backButton = x => x.Class("ImageButton");
        }

        public BookCityPage SelectCity(string cityName)
        {
            App.Tap(x => x.Marked("suggestions").Descendant().Text(cityName));

            return this;
        }

        public void Continue()
        {
            App.Tap(x => x.Marked("continue"));
            App.Screenshot("Continue to choose dates");
        }

        public void GoBack()
        {
            App.Tap(backButton);
        }
    }
}
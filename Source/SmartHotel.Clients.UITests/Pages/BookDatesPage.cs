using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SmartHotel.Clients.UITests.Pages
{
    class BookDatesPage : BasePage
    {
        readonly Query backButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("calendar"),
            iOS = x => x.Marked("calendar")
        };

        public BookDatesPage()
        {
            backButton = x => x.Class("ImageButton");
        }

        public void Continue()
        {
            App.Tap(x => x.Marked("continue"));
            App.Screenshot("Continue to select hotel");
        }
    }
}

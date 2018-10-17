using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SmartHotel.Clients.UITests.Pages
{
    class BookPage : BasePage
    {
        readonly Query backButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("hotel"),
            iOS = x => x.Marked("hotel")
        };

        public BookPage()
        {
            backButton = x => x.Class("ImageButton");
        }

        public void Complete()
        {
            App.Tap(x => x.Marked("complete"));
            App.Screenshot("Complete booking");
        }
    }
}

using Xamarin.UITest.Queries;

namespace SmartHotel.Clients.UITests.Pages
{
    class BookHotelsPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("HotelItem"),
            iOS = x => x.Marked("HotelItem")
        };

        public BookHotelsPage SelectFirstHotel()
        {
            App.Tap(x => x.Marked("HotelItem").Index(0));
            App.Screenshot("Continue to selected hotel detail");
            return this;
        }
    }
}
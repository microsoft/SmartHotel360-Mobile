namespace SmartHotel.Clients.UITests.Pages
{
    class BookHotelsPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("hotels"),
            iOS = x => x.Marked("hotels")
        };

        public BookHotelsPage SelectFirstHotel()
        {
            App.Tap(x => x.Marked("list").Descendant().Index(0));
            App.Screenshot("Continue to selected hotel detail");
            return this;
        }
    }
}
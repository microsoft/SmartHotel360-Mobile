using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SmartHotel.Clients.UITests.Pages
{
    public class HomePage : BasePage
    {
        readonly Query navigationMenuButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("home"),
            iOS = x => x.Marked("home")
        };

        public HomePage()
        {
            if (OnAndroid)
            {
                navigationMenuButton = x => x.Marked("OK");
            }
        }

        public HomePage OpenNavigationMenu()
        {
            if (OnAndroid)
            {
                App.Tap(navigationMenuButton);
                App.Screenshot("Navigation Menu Open");
            }
            if (OniOS)
            {
                App.SwipeLeftToRight();
            }

            return this;
        }
    }
}
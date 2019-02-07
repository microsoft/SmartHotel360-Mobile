using System;
using System.Linq;

namespace SmartHotel.Clients.UITests.Pages
{
    public class HomePage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("home"),
            iOS = x => x.Marked("home")
        };

        public HomePage()
        {
            App.WaitForNoElement(x => x.Marked("activityindicator"));
        }

        public HomePage OpenNavigationMenu()
        {
            var deviceScreenRect = App.Query().FirstOrDefault().Rect;

            int fromX = 0;
            int fromY = Convert.ToInt32(deviceScreenRect.Height * 0.6);
            int toX = Convert.ToInt32(deviceScreenRect.Width * 0.6);
            int toY = Convert.ToInt32(deviceScreenRect.Height * 0.6);

            App.DragCoordinates(fromX, fromY, toX, toY);

            App.Screenshot("Navigation Menu Open");

            return this;
        }
    }
}
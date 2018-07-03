using NUnit.Framework;
using SmartHotel.Clients.UITests.Pages;
using Xamarin.UITest;

namespace SmartHotel.Clients.UITests
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void SuccessSignInTest()
        {
            new LogInPage()
                .EnterCredentials(TestSettings.TestUsername, TestSettings.TestPassword)
                .SignIn();  

            new HomePage();
        }

        [Test]
        public void FailedSignInTest()
        {
            new LogInPage()
                .EnterCredentials(string.Empty, string.Empty)
                .SignIn();   
        }

        [Test]
        public void SideMenuNavigationTest()
        {
            SuccessSignInTest();
            new HomePage()
                .OpenNavigationMenu()
                .SelectMenuOption(BasePage.NavigationMenuOptions.BookRoom);
        }

        [Test]
        public void BookingTest()
        {
            SuccessSignInTest();

            new HomePage()
                .OpenNavigationMenu()
                .SelectMenuOption(BasePage.NavigationMenuOptions.BookRoom);

            new BookCityPage()
                .SelectCity(TestSettings.NewYork)
                .Continue();

            new BookDatesPage()
                .Continue();

            new BookHotelsPage()
                .SelectFirstHotel();

            new BookPage()
                .Complete();
        }
    }
}
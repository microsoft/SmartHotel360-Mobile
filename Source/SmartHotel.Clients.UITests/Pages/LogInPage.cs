using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace SmartHotel.Clients.UITests.Pages
{
    public class LogInPage : BasePage
    {
        readonly Query emailField;
        readonly Query passwordField;
        readonly Query signInButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("Username"),
            iOS = x => x.Marked("Username")
        };

        public LogInPage()
        {
            emailField = x => x.Marked("username");
            passwordField = x => x.Marked("password");
            signInButton = x => x.Marked("signin");

        }

        public LogInPage EnterCredentials(string username, string password)
        {
            App.WaitForElement(emailField);
            App.Tap(emailField);
            App.EnterText(username);
            App.DismissKeyboard();

            App.Tap(passwordField);
            App.EnterText(password);
            App.DismissKeyboard();

            App.Screenshot("Credentials Entered");

            return this;
        }

        public void SignIn()
        {
            App.Tap(signInButton);
        }
    }
}
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Helpers
{
    public class StatusBarHelper
    {
        private static readonly StatusBarHelper _instance = new StatusBarHelper();
        public const string TranslucentStatusChangeMessage = "TranslucentStatusChange";

        public static StatusBarHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        protected StatusBarHelper()
        {
        }

        public void MakeTranslucentStatusBar(bool translucent)
        {
            MessagingCenter.Send(this, TranslucentStatusChangeMessage, translucent);
        }
    }
}

using UIKit;

namespace SmartHotel.Clients.Maintenance.iOS
{
    internal static class NavigationItemHelper
    {
        public static void UpdateBadgeCounter(UINavigationItem navigationItem, int numberOfPendingTasks)
        {
            var badgeButton = new UIButton(UIButtonType.Custom);
            badgeButton.SetImage(UIImage.FromFile("ic_number.png"), UIControlState.Normal);
            badgeButton.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            badgeButton.ContentEdgeInsets = new UIEdgeInsets(10f, 10f, 10f, 10f);
            badgeButton.TitleLabel.Font = badgeButton.TitleLabel.Font.WithSize(14f);

            badgeButton.SetTitle(numberOfPendingTasks.ToString(), UIControlState.Normal);
            badgeButton.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, -45 - 10 * (badgeButton.TitleLabel.Text.Length - 1));

            var rightBarButton = new UIBarButtonItem(badgeButton);
            navigationItem.SetRightBarButtonItems(new UIBarButtonItem[] { rightBarButton }, false);
        }
    }
}
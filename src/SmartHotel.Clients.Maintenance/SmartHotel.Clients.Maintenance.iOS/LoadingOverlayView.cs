using System;
using UIKit;
using CoreGraphics;

namespace SmartHotel.Clients.Maintenance.iOS
{
    public class LoadingOverlayView : UIView
    {
        private UIActivityIndicatorView _activitySpinner;
        private UILabel _loadingLabel;

        public LoadingOverlayView(CGRect frame) : base(frame)
        {
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            _activitySpinner.Frame = new CGRect(
                centerX - (_activitySpinner.Frame.Width / 2),
                centerY - _activitySpinner.Frame.Height - 20,
                _activitySpinner.Frame.Width,
                _activitySpinner.Frame.Height);
            _activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(_activitySpinner);
            _activitySpinner.StartAnimating();

            _loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ))
            {
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.White,
                Text = "Loading ...",
                TextAlignment = UITextAlignment.Center,
                AutoresizingMask = UIViewAutoresizing.All
            };
            AddSubview(_loadingLabel);
        }

        public void Hide()
        {
            UIView.Animate(
                0.5, 
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    }
}
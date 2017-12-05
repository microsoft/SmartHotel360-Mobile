using System;
using CoreGraphics;
using SmartHotel.Clients.Maintenance.Models;
using UIKit;

namespace SmartHotel.Clients.Maintenance.iOS
{
    public partial class TaskTableViewCell : UITableViewCell
    {
        internal static nfloat CellHeight = 90f;
        private UIView _whiteRoundedView;
        private nfloat _spacing = 8;
        private nfloat _shadowSize = 1f;

        public TaskTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ContentView.BackgroundColor = UIColor.Clear;
            SelectionStyle = UITableViewCellSelectionStyle.None;

            // Card
            _whiteRoundedView = new UIView();
            _whiteRoundedView.Layer.BackgroundColor = UIColor.White.CGColor;
            _whiteRoundedView.Layer.MasksToBounds = false;
            _whiteRoundedView.Layer.CornerRadius = 3.0f;
            _whiteRoundedView.Layer.ShadowOffset = new CGSize(0, _shadowSize);
            _whiteRoundedView.Layer.ShadowOpacity = 0.2f;

            ContentView.AddSubview(_whiteRoundedView);
            ContentView.SendSubviewToBack(_whiteRoundedView);

            // Title Style
            TaskTitle.Font = UIFont.FromName("Poppins-Regular", 20f);
            TaskTitle.TextAlignment = UITextAlignment.Left;
            TaskTitle.TextColor = Styles.BlackTextColor;

            // Subtitle Style
            TaskDetail.Font = UIFont.FromName("Poppins-Regular", 15f);
            TaskDetail.TextColor = Styles.GreenTextColor;
            TaskDetail.TextAlignment = UITextAlignment.Left;

            // Status Style
            TaskStatus.TextAlignment = UITextAlignment.Right;
            TaskStatus.Font = UIFont.FromName("Poppins-Regular", 13f);
        }

        public void Update(Task item) 
        {
            TaskImage.Image = UIImage.FromFile(GetImage(item.TaskType));
            TaskTitle.Text = string.Format("Room {0}", item.Room);
            TaskDetail.Text = GetSubtitle(item.TaskType);
            TaskStatus.Text = item.Resolved ? "Resolved" : "Pending";
            MarkAsResolved(item.Resolved);
        }

        public void MarkAsResolved(bool resolved) 
        {
            TaskStatusIcon.Image = resolved
                ? UIImage.FromFile("resolved-status.png")
                : UIImage.FromFile("pending-status.png");

            TaskStatus.TextColor = resolved
                ? Styles.GreenTextColor
                : Styles.RedTextColor;        
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _whiteRoundedView.Frame = new CGRect(
                _spacing,
                _spacing * 1f,
                Frame.Width - 2 * _spacing,
                CellHeight - _spacing * 1f - 2 * _shadowSize);
        }

        private string GetSubtitle(int taskType)
        {
            switch (taskType)
            {
                case 1:
                    return "Air Conditioner";
                case 2:
                    return "Clean Room";
                case 3:
                    return "New Guest";
                case 4:
                    return "Room Service";
                case 5:
                    return "Change Towels";
                default:
                    return string.Empty;
            }
        }

        private string GetImage(int taskType)
        {
            switch (taskType)
            {
                case 1:
                    return "ic_air_conditioner.png";
                case 2:
                    return "ic_clean_room.png";
                case 3:
                    return "ic_new_guest.png";
                case 4:
                    return "ic_room_service.png";
                case 5:
                    return "ic_towel.png";
                default:
                    return string.Empty;
            }
        }
    }
}
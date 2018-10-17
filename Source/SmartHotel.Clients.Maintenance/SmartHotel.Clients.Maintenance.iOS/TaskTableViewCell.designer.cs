// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SmartHotel.Clients.Maintenance.iOS
{
    [Register ("TaskTableViewCell")]
    partial class TaskTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskDetail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TaskImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TaskStatusIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TaskDetail != null) {
                TaskDetail.Dispose ();
                TaskDetail = null;
            }

            if (TaskImage != null) {
                TaskImage.Dispose ();
                TaskImage = null;
            }

            if (TaskStatus != null) {
                TaskStatus.Dispose ();
                TaskStatus = null;
            }

            if (TaskStatusIcon != null) {
                TaskStatusIcon.Dispose ();
                TaskStatusIcon = null;
            }

            if (TaskTitle != null) {
                TaskTitle.Dispose ();
                TaskTitle = null;
            }
        }
    }
}
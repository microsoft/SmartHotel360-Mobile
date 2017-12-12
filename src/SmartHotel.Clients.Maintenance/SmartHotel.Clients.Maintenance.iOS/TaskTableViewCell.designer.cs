// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
        [GeneratedCode("iOS Designer", "1.0")]
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

			if (TaskTitle != null) {
				TaskTitle.Dispose ();
                TaskTitle = null;
			}

			if (TaskStatusIcon != null) {
				TaskStatusIcon.Dispose ();
                TaskStatusIcon = null;
			}
		}
	}
}

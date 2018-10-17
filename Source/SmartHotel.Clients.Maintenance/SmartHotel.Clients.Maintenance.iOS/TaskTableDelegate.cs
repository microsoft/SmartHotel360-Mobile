using System;
using System.Collections.Generic;
using System.Linq;
using SmartHotel.Clients.Maintenance.Models;
using UIKit;

namespace SmartHotel.Clients.Maintenance.iOS
{
    public class TaskTableDelegate : UITableViewDelegate
    {
        readonly UITableViewController _parentController;
        List<Task> _tasks;
        readonly TaskDetailPageManager _detailManager;

        public TaskTableDelegate(UITableViewController parentController, IEnumerable<Task> tasks)
        {
            _parentController = parentController;
            _tasks = tasks.ToList();
            _detailManager = new TaskDetailPageManager(parentController);
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            Console.WriteLine("Row selected: {0}", indexPath.Row);

            var item = _tasks[indexPath.Row];

            _detailManager.ShowFormsDetailPage(item, _tasks.Count(r => !r.Resolved));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _detailManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
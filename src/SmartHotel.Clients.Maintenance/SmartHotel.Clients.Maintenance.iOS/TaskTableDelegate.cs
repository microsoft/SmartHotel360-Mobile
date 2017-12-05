using SmartHotel.Clients.Maintenance.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UIKit;

namespace SmartHotel.Clients.Maintenance.iOS
{
    public class TaskTableDelegate : UITableViewDelegate
    {
        private UITableViewController _parentController;
        private List<Task> _tasks;
        private readonly TaskDetailPageManager _detailManager;

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

            _detailManager.ShowFormsDetailPage(item, _tasks.Where(r => !r.Resolved).Count());
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
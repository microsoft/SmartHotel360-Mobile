using System;
using System.Collections.Generic;
using Foundation;
using SmartHotel.Clients.Maintenance.Models;
using UIKit;

namespace SmartHotel.Clients.Maintenance.iOS
{
    // The data source for a UITableView. 
    public class TaskTableDataSource : UITableViewDataSource
    {
        public TaskTableDataSource(IEnumerable<Task> tasks) => LoadData(tasks);

        public List<Task> Items = new List<Task>();

        public string CellID => "TaskCell";

        void LoadData(IEnumerable<Task> tasks)
        {
            foreach(var t in tasks)
            {
                Items.Add(t);
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellID, indexPath) as TaskTableViewCell;
            var item = Items[indexPath.Row];

            cell.Update(item);

            return cell;
        }

        public override nint NumberOfSections(UITableView tableView) => 1;

        public override nint RowsInSection(UITableView tableView, nint section) => Items.Count;
    }
}
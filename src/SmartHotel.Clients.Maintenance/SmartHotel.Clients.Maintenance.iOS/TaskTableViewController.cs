using CoreGraphics;
using SmartHotel.Clients.Core.Services.Dialog;
using SmartHotel.Clients.Core.ViewModels;
using SmartHotel.Clients.Core.Views;
using SmartHotel.Clients.Maintenance.Models;
using SmartHotel.Clients.Maintenance.Services.Settings;
using SmartHotel.Clients.Maintenance.Services.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.iOS
{
    public partial class TaskTableViewController : UITableViewController
    {
        private LoadingOverlayView _loading;
        private UITapGestureRecognizer _titleTapGestureRecognizer;
        private readonly IDialogService _dialogService;
        private readonly ITasksService _tasksService;

        public TaskTableViewController (IntPtr handle) : base (handle)
        {
            _titleTapGestureRecognizer = new UITapGestureRecognizer(OnTitleTapped)
            {
                NumberOfTapsRequired = 2
            };

            _dialogService = new DialogService();
            _tasksService = new FakeTasksService();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItemHelper.UpdateBadgeCounter(NavigationItem, 0);
            NavigationController.NavigationBar.AddGestureRecognizer(_titleTapGestureRecognizer);
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var bounds = UIScreen.MainScreen.Bounds;

            if (_loading == null)
            {
                _loading = new LoadingOverlayView(bounds);
                View.Add(_loading);
            }

            await LoadTasksAsync();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            NavigationController.NavigationBar.RemoveGestureRecognizer(_titleTapGestureRecognizer);
        }

        private async System.Threading.Tasks.Task LoadTasksAsync()
        {
            try
            {
                var tasks = await _tasksService.GetTasksAsync();

                TableView.BackgroundColor = UIColor.FromRGB(248, 248, 248);
                TableView.TableHeaderView = GetViewForHeader();
                TableView.DataSource = new TaskTableDataSource(tasks);
                TableView.Delegate = new TaskTableDelegate(this, tasks);
                TableView.RowHeight = TaskTableViewCell.CellHeight;
                TableView.SeparatorColor = UIColor.Clear;
                TableView.ReloadData();

                NavigationItemHelper.UpdateBadgeCounter(
                    NavigationItem,
                    tasks.Where(r => !r.Resolved).Count());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Suggestions] Error: {ex}");
                await _dialogService.ShowAlertAsync("Error retrieving data", ex.Message, "Ok");
            }
            finally
            {
                _loading.Hide();
            }
        }

        private UIView GetViewForHeader()
        {
            UIView view = new UIView()
            {
                Frame = new CGRect(0, 0, 0, 84.0f),
                BackgroundColor = UIColor.FromRGB(248, 248, 248)
            };

            var titleLabel = new UILabel(new CGRect(12, 24, 300, 22))
            {
                Text = "Today",  
                Font = UIFont.FromName("Poppins-Regular", 21f),
                TextColor = Styles.BlackTextColor,
                TextAlignment = UITextAlignment.Left
            };

            view.AddSubview(titleLabel);

            var subtitleLabel = new UILabel(new CGRect(12, 48, 300, 22))
            {
                Text = DateTime.Today.ToString("dddd, MMMM dd, yyyy"),
                Font = UIFont.FromName("Poppins-Regular", 15f),
                TextColor = Styles.GreenTextColor,
                TextAlignment = UITextAlignment.Left
            };

            view.AddSubview(subtitleLabel);

            return view;
        }

        private void OnTitleTapped()
        {
            var settingsViewModel = new SettingsViewModel<RemoteSettings>(
                    new SettingsService());
            var settingsView = new SettingsView
            {
                BindingContext = settingsViewModel
            };
            settingsView.Appearing += async (o, e) =>
            {
                await settingsViewModel.InitializeAsync(null);
            };

            var settingsController = settingsView.CreateViewController();
            NavigationController.PushViewController(settingsController, true);
        }
    }
}
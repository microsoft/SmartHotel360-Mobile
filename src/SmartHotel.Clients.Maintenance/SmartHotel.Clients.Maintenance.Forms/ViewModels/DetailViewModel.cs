using SmartHotel.Clients.Core.ViewModels.Base;
using SmartHotel.Clients.Maintenance.Models;
using SmartHotel.Clients.Maintenance.Services.Tasks;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.ViewModels
{
    public class DetailViewModel : ViewModelBase
    {
        public const string TaskMarkedAsResolvedMessage = "TaskMarkedAsResolvedMessage";
        public const string GoBackToTasksMessage = "GoBackToTasksMessage";

        private bool _popup;
        private Models.Task _task;
        private readonly ITasksService _tasksService;

        public DetailViewModel(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        public bool Popup
        {
            get { return _popup; }
            set
            {
                _popup = value;
                OnPropertyChanged();
            }
        }

        public Models.Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanClose));
            }
        }

        public bool CanClose => !Task?.Resolved ?? false;

        public DateTime ResolvedDate => DateTime.Now;

        public ICommand CloseTaskCommand => new Command(CloseTask);

        public ICommand BackToTasksCommand => new Command(BackToTasks);

        private async void CloseTask()
        {
            try
            {
                await _tasksService.MarkTaskAsResolvedAsync(Task.Id);

                var popup = !Popup;
                MessagingCenter.Send(this, AppSettings.Animation, popup);
                await System.Threading.Tasks.Task.Delay(500);
                Popup = popup;

                MessagingCenter.Send(this, TaskMarkedAsResolvedMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Suggestions] Error: {ex}");
                await DialogService.ShowAlertAsync("Error retrieving data", ex.Message, "Ok");
            }
        }

        private void BackToTasks(object obj)
        {
            MessagingCenter.Send(this, GoBackToTasksMessage);
        }
    }
}
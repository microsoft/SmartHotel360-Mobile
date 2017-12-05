using SmartHotel.Clients.Maintenance.Forms.ViewModels;
using SmartHotel.Clients.Maintenance.Forms.Views;
using SmartHotel.Clients.Maintenance.Models;
using System;
using UIKit;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.iOS
{
    internal class TaskDetailPageManager : IDisposable
    {
        private readonly UIViewController _parentController;
        private readonly UINavigationController _navigationController;
        private UIViewController _detailViewController;
        private int _currentBadgeCount;

        public TaskDetailPageManager(UIViewController parentController)
        {
            _parentController = parentController;
            _navigationController = parentController.NavigationController;
            MessagingCenter.Subscribe<DetailViewModel>(this, DetailViewModel.TaskMarkedAsResolvedMessage, OnTaskMarkedAsResolved);
            MessagingCenter.Subscribe<DetailViewModel>(this, DetailViewModel.GoBackToTasksMessage, OnGoBackToTasks);
        }

        public void ShowFormsDetailPage(Models.Task task, int currentBadgeCount)
        {
            _currentBadgeCount = currentBadgeCount;

            _detailViewController = new DetailView(task).CreateViewController();

            _navigationController.PushViewController(_detailViewController, true);
            NavigationItemHelper.UpdateBadgeCounter(
                _detailViewController.NavigationItem,
                currentBadgeCount);
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<DetailViewModel>(this, DetailViewModel.TaskMarkedAsResolvedMessage);
            MessagingCenter.Unsubscribe<DetailViewModel>(this, DetailViewModel.GoBackToTasksMessage);
        }

        private void OnTaskMarkedAsResolved(DetailViewModel vm)
        {
            _currentBadgeCount--;

            NavigationItemHelper.UpdateBadgeCounter(
                _detailViewController.NavigationItem,
                _currentBadgeCount);
            NavigationItemHelper.UpdateBadgeCounter(
                _parentController.NavigationItem,
                _currentBadgeCount);
        }

        private void OnGoBackToTasks(DetailViewModel vm)
        {
            _navigationController.PopViewController(true);
        }
    }
}
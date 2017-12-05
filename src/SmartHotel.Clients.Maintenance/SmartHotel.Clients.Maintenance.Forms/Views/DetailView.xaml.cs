using SmartHotel.Clients.Maintenance.Forms.ViewModels;
using SmartHotel.Clients.Maintenance.Models;
using SmartHotel.Clients.Maintenance.Services.Tasks;
using Xamanimation;
using Xamarin.Forms;

namespace SmartHotel.Clients.Maintenance.Forms.Views
{
    public partial class DetailView : ContentPage
    {
        private DetailViewModel _detailViewModel;
        private Task _task;

        public DetailView(Task task)
        {
            InitializeComponent();

            _task = task;

            _detailViewModel = new DetailViewModel(new FakeTasksService());

            BindingContext = _detailViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(_detailViewModel != null)
            {
                _detailViewModel.Task = _task;
            }

            MessagingCenter.Subscribe<DetailViewModel, bool>(this, AppSettings.Animation, (sender, arg) => 
            {
                var popup = arg;

                if (popup)
                {
                    Popup.Animate(new FadeToAnimation {  Opacity = 1 });
                }
                else
                {
                    Popup.Animate(new FadeToAnimation { Opacity = 0 });
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<DetailViewModel, bool>(this, AppSettings.Animation);
        }
    }
}
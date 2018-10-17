using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.ViewModels;
using SmartHotel.Clients.Core.ViewModels.Base;
using SmartHotel.Clients.Core.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Services.Navigation
{
    public partial class NavigationService : INavigationService
    {
        readonly IAuthenticationService authenticationService;
        protected readonly Dictionary<Type, Type> mappings;

        protected Application CurrentApplication => Application.Current;

        public NavigationService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            if (await authenticationService.UserIsAuthenticatedAndValidAsync())
            {
                await NavigateToAsync<MainViewModel>();
            }
            else
            {
                await NavigateToAsync<LoginViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase => InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase => InternalNavigateToAsync(typeof(TViewModel), parameter);

        public Task NavigateToAsync(Type viewModelType) => InternalNavigateToAsync(viewModelType, null);

        public Task NavigateToAsync(Type viewModelType, object parameter) => InternalNavigateToAsync(viewModelType, parameter);

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainView mainPage)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (page is LoginView)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
            }
            else if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;

                if (mainPage.Detail is CustomNavigationPage navigationPage)
                {
                    var currentPage = navigationPage.CurrentPage;

                    if (currentPage.GetType() != page.GetType())
                    {
                        await navigationPage.PushAsync(page);
                    }
                }
                else
                {
                    navigationPage = new CustomNavigationPage(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
            }
            else
            {
                if (CurrentApplication.MainPage is CustomNavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new CustomNavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            var viewModel = Locator.Instance.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        void CreatePageViewModelMappings()
        {
            mappings.Add(typeof(BookingCalendarViewModel), typeof(BookingCalendarView));
            mappings.Add(typeof(BookingHotelViewModel), typeof(BookingHotelView));
            mappings.Add(typeof(BookingHotelsViewModel), typeof(BookingHotelsView));
            mappings.Add(typeof(BookingViewModel), typeof(BookingView));
            mappings.Add(typeof(CheckoutViewModel), typeof(CheckoutView));
            mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            mappings.Add(typeof(MainViewModel), typeof(MainView));
            mappings.Add(typeof(MyRoomViewModel), typeof(MyRoomView));
            mappings.Add(typeof(NotificationsViewModel), typeof(NotificationsView));
            mappings.Add(typeof(OpenDoorViewModel), typeof(OpenDoorView));
            mappings.Add(typeof(SettingsViewModel<RemoteSettings>), typeof(SettingsView));
            mappings.Add(typeof(ExtendedSplashViewModel), typeof(ExtendedSplashView));

            if (Device.Idiom == TargetIdiom.Desktop)
            {
                mappings.Add(typeof(HomeViewModel), typeof(UwpHomeView));
                mappings.Add(typeof(SuggestionsViewModel), typeof(UwpSuggestionsView));
            }
            else
            {
                mappings.Add(typeof(HomeViewModel), typeof(HomeView));
                mappings.Add(typeof(SuggestionsViewModel), typeof(SuggestionsView));
            }
        }
    }
}
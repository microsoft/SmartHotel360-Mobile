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
        private readonly IAuthenticationService _authenticationService;
        protected readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public NavigationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            if (await _authenticationService.UserIsAuthenticatedAndValidAsync())
            {
                await NavigateToAsync<MainViewModel>();
            }
            else
            {
                await NavigateToAsync<LoginViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

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
            var mainPage = CurrentApplication.MainPage as MainView;

            if (mainPage != null)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

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
                var navigationPage = mainPage.Detail as CustomNavigationPage;

                if (navigationPage != null)
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
                var navigationPage = CurrentApplication.MainPage as CustomNavigationPage;

                if (navigationPage != null)
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
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = Locator.Instance.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(BookingCalendarViewModel), typeof(BookingCalendarView));
            _mappings.Add(typeof(BookingHotelViewModel), typeof(BookingHotelView));
            _mappings.Add(typeof(BookingHotelsViewModel), typeof(BookingHotelsView));
            _mappings.Add(typeof(BookingViewModel), typeof(BookingView));
            _mappings.Add(typeof(CheckoutViewModel), typeof(CheckoutView));
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(MyRoomViewModel), typeof(MyRoomView));
            _mappings.Add(typeof(NotificationsViewModel), typeof(NotificationsView));
            _mappings.Add(typeof(OpenDoorViewModel), typeof(OpenDoorView));
            _mappings.Add(typeof(SettingsViewModel<RemoteSettings>), typeof(SettingsView));
            _mappings.Add(typeof(ExtendedSplashViewModel), typeof(ExtendedSplashView));

            if (Device.Idiom == TargetIdiom.Desktop)
            {
                _mappings.Add(typeof(HomeViewModel), typeof(UwpHomeView));
                _mappings.Add(typeof(SuggestionsViewModel), typeof(UwpSuggestionsView));
            }
            else
            {
                _mappings.Add(typeof(HomeViewModel), typeof(HomeView));
                _mappings.Add(typeof(SuggestionsViewModel), typeof(SuggestionsView));
            }
        }
    }
}
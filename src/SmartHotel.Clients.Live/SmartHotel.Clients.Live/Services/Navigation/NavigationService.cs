using SmartHotel.Clients.Live.ViewModels;
using SmartHotel.Clients.Live.ViewModels.Base;
using SmartHotel.Clients.Live.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Live.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private INavigation _navigation;
        private MainView _mainPage;

        protected Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }
        
        public async Task InitializeAsync()
        {
            await NavigateToAsync(typeof(LoginViewModel));
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

        public virtual async Task NavigateBackAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);
            SetNavigationIcon(page);

            if (page is LoginView)
            {
                var navigationPage = new CustomNavigationPage(page);
                _navigation = navigationPage.Navigation;
                SetMainPage(navigationPage);
            }
            else if (page is MainView)
            {
                _mainPage = page as MainView;

                if (_navigation != null)
                {
                    await _navigation.PushAsync(page);
                }

                var detailPage = CreateAndBindPage(typeof(HomeViewModel), null);
                var navigationPage = new CustomNavigationPage(detailPage);
                _navigation = navigationPage.Navigation;
                _mainPage.Detail = navigationPage;

                _mainPage.IsPresented = false;
            }
            else
            {
                if (_navigation != null)
                {
                    await _navigation.PushAsync(page);
                    _mainPage.IsPresented = false;
                }
            }

            await (page.BindingContext as ViewModelBase)?.InitializeAsync(parameter);
        }

        protected virtual Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (viewModelType == typeof(MainViewModel))
            {
                return typeof(MainView);
            }
            else if (viewModelType == typeof(LoginViewModel))
            {
                return typeof(LoginView);
            }
            else if (viewModelType == typeof(MenuViewModel))
            {
                return typeof(MenuView);
            }
            else if (viewModelType == typeof(HomeViewModel))
            {
                return typeof(HomeView);
            }
            else
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }
            
            Page page = (Page)Activator.CreateInstance(pageType);
            var context = GetViewModelMappings(viewModelType);
            page.BindingContext = context;

            return page;
        }

        protected virtual void SetNavigationIcon(Page page)
        {
            NavigationPage.SetBackButtonTitle(page, "Back");
        }

        protected virtual void SetMainPage(Page page)
        {
            if (CurrentApplication.MainPage != null)
            {
                var viewModel = CurrentApplication.MainPage.BindingContext as ViewModelBase;
                viewModel?.Dispose();
            }

            CurrentApplication.MainPage = page;
        }

        private object GetViewModelMappings(Type viewModel)
        {
            if (viewModel == typeof(MainViewModel))
            {
                return new MainViewModel();
            }
            else if (viewModel == typeof(LoginViewModel))
            {
                return new LoginViewModel();
            }
            else if (viewModel == typeof(MenuViewModel))
            {
                return new MenuViewModel();
            }
            else if (viewModel == typeof(HomeView))
            {
                return new HomeViewModel();
            }

            return null;
        }
    }
}
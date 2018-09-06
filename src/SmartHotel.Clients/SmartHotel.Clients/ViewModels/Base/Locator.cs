using System;
using Autofac;
using SmartHotel.Clients.Core.Services.Analytic;
using SmartHotel.Clients.Core.Services.Authentication;
using SmartHotel.Clients.Core.Services.Booking;
using SmartHotel.Clients.Core.Services.Chart;
using SmartHotel.Clients.Core.Services.Dialog;
using SmartHotel.Clients.Core.Services.Hotel;
using SmartHotel.Clients.Core.Services.Location;
using SmartHotel.Clients.Core.Services.Navigation;
using SmartHotel.Clients.Core.Services.Notification;
using SmartHotel.Clients.Core.Services.OpenUri;
using SmartHotel.Clients.Core.Services.Request;
using SmartHotel.Clients.Core.Services.Settings;
using SmartHotel.Clients.Core.Services.Suggestion;
using SmartHotel.Clients.Core.Models;
using SmartHotel.Clients.Core.Services.IoT;

namespace SmartHotel.Clients.Core.ViewModels.Base
{
    public class Locator
    {
        private IContainer _container;
        private ContainerBuilder _containerBuilder;

        private static readonly Locator _instance = new Locator();

        public static Locator Instance
        {
            get
            {
                return _instance;
            }
        }

        public Locator()
        {
            _containerBuilder = new ContainerBuilder();

            _containerBuilder.RegisterType<AnalyticService>().As<IAnalyticService>();
            _containerBuilder.RegisterType<DialogService>().As<IDialogService>();
            _containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
            _containerBuilder.RegisterType<FakeChartService>().As<IChartService>();
            _containerBuilder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            _containerBuilder.RegisterType<LocationService>().As<ILocationService>();
            _containerBuilder.RegisterType<OpenUriService>().As<IOpenUriService>();
            _containerBuilder.RegisterType<RequestService>().As<IRequestService>();
            _containerBuilder.RegisterType<DefaultBrowserCookiesService>().As<IBrowserCookiesService>();
            _containerBuilder.RegisterType<LiveIoTDataService>().As<ILiveIoTDataService>();
            _containerBuilder.RegisterType<GravatarUrlProvider>().As<IAvatarUrlProvider>();
            _containerBuilder.RegisterType(typeof(SettingsService)).As(typeof(ISettingsService<RemoteSettings>));

            if (AppSettings.UseFakes)
            {
                _containerBuilder.RegisterType<FakeBookingService>().As<IBookingService>();
                _containerBuilder.RegisterType<FakeHotelService>().As<IHotelService>();
                _containerBuilder.RegisterType<FakeNotificationService>().As<INotificationService>();
                _containerBuilder.RegisterType<FakeSuggestionService>().As<ISuggestionService>();
            }
            else
            {
                _containerBuilder.RegisterType<BookingService>().As<IBookingService>();
                _containerBuilder.RegisterType<HotelService>().As<IHotelService>();
                _containerBuilder.RegisterType<NotificationService>().As<INotificationService>();
                _containerBuilder.RegisterType<SuggestionService>().As<ISuggestionService>();
            }

            _containerBuilder.RegisterType<BookingCalendarViewModel>();
            _containerBuilder.RegisterType<BookingHotelViewModel>();
            _containerBuilder.RegisterType<BookingHotelsViewModel>();
            _containerBuilder.RegisterType<BookingViewModel>();
            _containerBuilder.RegisterType<CheckoutViewModel>();
            _containerBuilder.RegisterType<HomeViewModel>();
            _containerBuilder.RegisterType<LoginViewModel>();
            _containerBuilder.RegisterType<MainViewModel>();
            _containerBuilder.RegisterType<MenuViewModel>();
            _containerBuilder.RegisterType<MyRoomViewModel>();
            _containerBuilder.RegisterType<NotificationsViewModel>();
            _containerBuilder.RegisterType<OpenDoorViewModel>();
            _containerBuilder.RegisterType<SuggestionsViewModel>();

            _containerBuilder.RegisterType(typeof(SettingsViewModel<RemoteSettings>));
            _containerBuilder.RegisterType<ExtendedSplashViewModel>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public void Register<T>() where T : class
        {
            _containerBuilder.RegisterType<T>();
        }

        public void Build()
        {
            _container = _containerBuilder.Build();
        }
    }
}
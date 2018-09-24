using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SmartHotel.Clients.Core.Controls
{
    public partial class RemoteSettingsControl : ContentView
    {
        public static readonly BindableProperty RemoteSettingsProperty =
            BindableProperty.Create("RemoteSettings", typeof(object), typeof(RemoteSettingsControl), default(object));

        public object RemoteSettings
        {
            get => GetValue(RemoteSettingsProperty);
            set => SetValue(RemoteSettingsProperty, value);
        }

        public RemoteSettingsControl() => InitializeComponent();

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (RemoteSettingsProperty.PropertyName == propertyName)
            {
                RemoteSettingsUpdated();
            }
        }

        void RemoteSettingsUpdated()
        {
            View container = null;

            if (RemoteSettings != null)
            {
                container = GenerateControlsForObject(RemoteSettings);
            }

            Content = container;
        }

        Layout GenerateControlsForObject(object o)
        {
            var container = CreateContainer();

            if (o == null) return container;

            var allProperties = o.GetType().GetProperties();

            foreach (var property in allProperties)
            {
                if (IsSimpleType(property.PropertyType))
                {
                    var valueContainer = AddPropertyValue(property, o);
                    container.Children.Add(valueContainer);
                }
                else
                {
                    var title = AddSectionTitle(property.Name);
                    container.Children.Add(title);

                    var pValue = property.GetValue(o);
                    var objectContainer = GenerateControlsForObject(pValue);
                    container.Children.Add(objectContainer);
                }
            }

            return container;
        }

        View AddSectionTitle(string name)
        {
            var container = CreateContainer();
            var label = new Label
            {
                Text = name,
                Style = GetStyle("SettingsTitleStyle")
            };

            container.Children.Add(label);
            return container;
        }

        View AddPropertyValue(PropertyInfo property, object o)
        {
            var container = CreateContainer();
            var pValue = property.GetValue(o);

            var title = new Label
            {
                Text = property.Name,
                Style = GetStyle("SettingsValueTitleStyle")
            };

            container.Children.Add(title);

            var val = new Label
            {
                Text = pValue?.ToString() ?? "<null>",
                Style = GetStyle("SettingsValueStyle")
            };

            container.Children.Add(val);

            return container;
        }

        StackLayout CreateContainer() => new StackLayout
        {
            Padding = new Thickness(10, 5),
        };

        static bool IsSimpleType(Type type) => type.GetTypeInfo().IsPrimitive || typeof(string) == type;

        Style GetStyle(string key) => Resources[key] as Style;
    }
}
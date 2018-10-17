using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Behaviors
{
    public sealed class ItemTappedCommandListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty =
            BindableProperty.CreateAttached(
                "ItemTappedCommand",
                typeof(ICommand),
                typeof(ItemTappedCommandListView),
                default(ICommand),
                BindingMode.OneWay,
                null,
                PropertyChanged);

        static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ListView listView)
            {
                listView.ItemTapped -= ListViewOnItemTapped;
                listView.ItemTapped += ListViewOnItemTapped;
            }
        }

        static void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView list && list.IsEnabled && !list.IsRefreshing)
            {
                list.SelectedItem = null;
                var command = GetItemTappedCommand(list);
                if (command != null && command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }
            }
        }

        public static ICommand GetItemTappedCommand(BindableObject bindableObject) => (ICommand)bindableObject.GetValue(ItemTappedCommandProperty);

        public static void SetItemTappedCommand(BindableObject bindableObject, object value) => bindableObject.SetValue(ItemTappedCommandProperty, value);
    }
}
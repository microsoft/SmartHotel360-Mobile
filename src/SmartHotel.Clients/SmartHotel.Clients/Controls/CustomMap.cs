using SmartHotel.Clients.Core.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SmartHotel.Clients.Core.Controls
{
    public class CustomMap : Map
    {
        public static readonly BindableProperty CustomPinsProperty =
            BindableProperty.Create("CustomPins",
                typeof(IEnumerable<CustomPin>), typeof(CustomMap), default(IEnumerable<CustomPin>),
                BindingMode.TwoWay);

        public IEnumerable<CustomPin> CustomPins
        {
            get { return (IEnumerable<CustomPin>)base.GetValue(CustomPinsProperty); }
            set { base.SetValue(CustomPinsProperty, value); }
        }

        public static readonly BindableProperty SelectedPinProperty =
            BindableProperty.Create("SelectedPin",
                typeof(CustomPin), typeof(CustomMap), null);

        public CustomPin SelectedPin
        {
            get { return (CustomPin)base.GetValue(SelectedPinProperty); }
            set { base.SetValue(SelectedPinProperty, value); }
        }

    }
}
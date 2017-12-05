using System;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
	public partial class Calendar : ContentView
	{
		#region DisableAllDates

		public static readonly BindableProperty DisableAllDatesProperty = BindableProperty.Create(nameof(DisableAllDates), typeof(bool), typeof(Calendar), false,
				propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar)?.RaiseSpecialDatesChanged());

		/// <summary>
		/// Gets or sets wether all dates should be disabled by default or not
		/// </summary>
		/// <value></value>
		public bool DisableAllDates
		{
			get { return (bool)GetValue(DisableAllDatesProperty); }
			set { SetValue(DisableAllDatesProperty, value); }
		}

		#endregion

		#region DisableDatesLimitToMaxMinRange

		public static readonly BindableProperty DisableDatesLimitToMaxMinRangeProperty = BindableProperty.Create(nameof(DisableDatesLimitToMaxMinRange), typeof(bool), typeof(Calendar), false,
				propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar)?.RaiseSpecialDatesChanged());

		/// <summary>
		/// Gets or sets wether disabled dates limits the left/right scrolling (default is false)
		/// </summary>
		/// <value></value>
		public bool DisableDatesLimitToMaxMinRange
		{
			get { return (bool)GetValue(DisableDatesLimitToMaxMinRangeProperty); }
			set { SetValue(DisableDatesLimitToMaxMinRangeProperty, value); }
		}

		#endregion

		#region DisabledBorderWidth

		public static readonly BindableProperty DisabledBorderWidthProperty =
			BindableProperty.Create(nameof(DisabledBorderWidth), typeof(int), typeof(Calendar), Device.RuntimePlatform == Device.iOS ? 1 : 3,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBorderWidth((int)newValue, (int)oldValue));

		protected void ChangeDisabledBorderWidth(int newValue, int oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BorderWidth = newValue);
		}

		/// <summary>
		/// Gets or sets the border width of the disabled dates.
		/// </summary>
		/// <value>The width of the disabled border.</value>
		public int DisabledBorderWidth
		{
			get { return (int)GetValue(DisabledBorderWidthProperty); }
			set { SetValue(DisabledBorderWidthProperty, value); }
		}

		#endregion

		#region DisabledBorderColor

		public static readonly BindableProperty DisabledBorderColorProperty =
			BindableProperty.Create(nameof(DisabledBorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#cccccc"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBorderColor((Color)newValue, (Color)oldValue));

		protected void ChangeDisabledBorderColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BorderColor = newValue);
		}

		/// <summary>
		/// Gets or sets the border color of the disabled dates.
		/// </summary>
		/// <value>The color of the disabled border.</value>
		public Color DisabledBorderColor
		{
			get { return (Color)GetValue(DisabledBorderColorProperty); }
			set { SetValue(DisabledBorderColorProperty, value); }
		}

		#endregion

		#region DisabledBackgroundColor

		public static readonly BindableProperty DisabledBackgroundColorProperty =
			BindableProperty.Create(nameof(DisabledBackgroundColor), typeof(Color), typeof(Calendar), Color.Gray,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeDisabledBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.BackgroundColor = newValue);
		}

		/// <summary>
		/// Gets or sets the background color of the disabled dates.
		/// </summary>
		/// <value>The color of the disabled background.</value>
		public Color DisabledBackgroundColor
		{
			get { return (Color)GetValue(DisabledBackgroundColorProperty); }
			set { SetValue(DisabledBackgroundColorProperty, value); }
		}

		#endregion

		#region DisabledTextColor

		public static readonly BindableProperty DisabledTextColorProperty =
			BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(Calendar), Color.FromHex("#dddddd"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeDisabledTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.TextColor = newValue);
		}

		/// <summary>
		/// Gets or sets the text color of the disabled dates.
		/// </summary>
		/// <value>The color of the disabled text.</value>
		public Color DisabledTextColor
		{
			get { return (Color)GetValue(DisabledTextColorProperty); }
			set { SetValue(DisabledTextColorProperty, value); }
		}

		#endregion

		#region DisabledFontSize

		public static readonly BindableProperty DisabledFontSizeProperty =
			BindableProperty.Create(nameof(DisabledFontSize), typeof(double), typeof(Calendar), 20.0,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledFontSize((double)newValue, (double)oldValue));

		protected void ChangeDisabledFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the disabled dates.
		/// </summary>
		/// <value>The size of the disabled font.</value>
		public double DisabledFontSize
		{
			get { return (double)GetValue(DisabledFontSizeProperty); }
			set { SetValue(DisabledFontSizeProperty, value); }
		}

		#endregion

		#region DisabledFontAttributes

		public static readonly BindableProperty DisabledFontAttributesProperty =
					BindableProperty.Create(nameof(DisabledFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeDisabledFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the font family of disabled dates.
		/// </summary>
		public FontAttributes DisabledFontAttributes
		{
			get { return (FontAttributes)GetValue(DisabledFontAttributesProperty); }
			set { SetValue(DisabledFontAttributesProperty, value); }
		}

		#endregion

		#region DisabledFontFamily

		public static readonly BindableProperty DisabledFontFamilyProperty =
					BindableProperty.Create(nameof(DisabledFontFamily), typeof(string), typeof(Calendar), default(string),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeDisabledFontFamily((string)newValue, (string)oldValue));

		protected void ChangeDisabledFontFamily(string newValue, string oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => !b.IsEnabled).ForEach(b => b.FontFamily = newValue);
		}

		/// <summary>
		/// Gets or sets the font family of disabled dates.
		/// </summary>
		public string DisabledFontFamily
		{
			get { return GetValue(DisabledFontFamilyProperty) as string; }
			set { SetValue(DisabledFontFamilyProperty, value); }
		}

		#endregion

		protected void SetButtonDisabled(CalendarButton button)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				button.BackgroundPattern = null;
				button.BackgroundImage = null;
				button.FontSize = DisabledFontSize;
				button.BorderWidth = DisabledBorderWidth;
				button.BorderColor = DisabledBorderColor;
				button.BackgroundColor = DisabledBackgroundColor;
				button.TextColor = DisabledTextColor;
				button.FontAttributes = DisabledFontAttributes;
				button.FontFamily = DisabledFontFamily;
                if (Device.RuntimePlatform == Device.UWP)
                {
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = false;
                }
				button.IsSelected = false;
			});
		}
	}
}
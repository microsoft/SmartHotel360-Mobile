using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
	public partial class Calendar : ContentView
	{
		List<Label> dayLabels;

		#region WeekdaysTextColor

		public static readonly BindableProperty WeekdaysTextColorProperty =
			BindableProperty.Create(nameof(WeekdaysTextColor), typeof(Color), typeof(Calendar), Color.FromHex("#aaaaaa"),
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeWeekdaysTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			dayLabels.ForEach(l => l.TextColor = newValue);
		}

		/// <summary>
		/// Gets or sets the text color of the weekdays labels.
		/// </summary>
		/// <value>The color of the weekdays text.</value>
		public Color WeekdaysTextColor
		{
			get { return (Color)GetValue(WeekdaysTextColorProperty); }
			set { SetValue(WeekdaysTextColorProperty, value); }
		}

		#endregion

		#region WeekdaysBackgroundColor

		public static readonly BindableProperty WeekdaysBackgroundColorProperty =
			BindableProperty.Create(nameof(WeekdaysBackgroundColor), typeof(Color), typeof(Calendar), Color.Transparent,
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeWeekdaysBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			dayLabels.ForEach(l => l.BackgroundColor = newValue);
		}

		/// <summary>
		/// Gets or sets the background color of the weekdays labels.
		/// </summary>
		/// <value>The color of the weekdays background.</value>
		public Color WeekdaysBackgroundColor
		{
			get { return (Color)GetValue(WeekdaysBackgroundColorProperty); }
			set { SetValue(WeekdaysBackgroundColorProperty, value); }
		}

		#endregion

		#region WeekdaysFontSize

		public static readonly BindableProperty WeekdaysFontSizeProperty =
			BindableProperty.Create(nameof(WeekdaysFontSize), typeof(double), typeof(Calendar), 18.0,
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysFontSize((double)newValue, (double)oldValue));

		protected void ChangeWeekdaysFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			dayLabels.ForEach(l => l.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the weekday labels.
		/// </summary>
		/// <value>The size of the weekdays font.</value>
		public double WeekdaysFontSize
		{
			get { return (double)GetValue(WeekdaysFontSizeProperty); }
			set { SetValue(WeekdaysFontSizeProperty, value); }
		}

		#endregion

		#region WeekdaysFontFamily

		public static readonly BindableProperty WeekdaysFontFamilyProperty =
            BindableProperty.Create(nameof(WeekdaysFontFamily), typeof(string), typeof(Calendar), default(string),							
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysFontFamily((string)newValue, (string)oldValue));

		protected void ChangeWeekdaysFontFamily(string newValue, string oldValue)
		{
			if (newValue == oldValue) return;
			dayLabels.ForEach(l => l.FontFamily = newValue);
		}

		/// <summary>
		/// Gets or sets the font family of the weekday labels.
		/// </summary>
		public string WeekdaysFontFamily
		{
			get { return GetValue(WeekdaysFontFamilyProperty) as string; }
			set { SetValue(WeekdaysFontFamilyProperty, value); }
		}

		#endregion

		#region WeekdaysFormat

		public static readonly BindableProperty WeekdaysFormatProperty =
			BindableProperty.Create(nameof(WeekdaysFormat), typeof(string), typeof(Calendar), "ddd",								
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdays());

		/// <summary>
		/// Gets or sets the date format of the weekday labels.
		/// </summary>
		/// <value>The weekdays format.</value>
		public string WeekdaysFormat
		{
			get { return GetValue(WeekdaysFormatProperty) as string; }
			set { SetValue(WeekdaysFormatProperty, value); }
		}
		#endregion

		#region WeekdaysFontAttributes

		public static readonly BindableProperty WeekdaysFontAttributesProperty =
			BindableProperty.Create(nameof(WeekdaysFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,		
                propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeWeekdaysFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeWeekdaysFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			dayLabels.ForEach(l => l.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the font attributes of the weekday labels.
		/// </summary>
		public FontAttributes WeekdaysFontAttributes
		{
			get { return (FontAttributes)GetValue(WeekdaysFontAttributesProperty); }
			set { SetValue(WeekdaysFontAttributesProperty, value); }
		}

		#endregion

		#region WeekdaysShow

		public static readonly BindableProperty WeekdaysShowProperty =
			BindableProperty.Create(nameof(WeekdaysShow), typeof(bool), typeof(Calendar), true,
				propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ShowHideElements());

		/// <summary>
		/// Gets or sets wether to show the weekday labels.
		/// </summary>
		/// <value>The weekdays show.</value>
		public bool WeekdaysShow
		{
			get { return (bool)GetValue(WeekdaysShowProperty); }
			set { SetValue(WeekdaysShowProperty, value); }
		}

		#endregion

		protected void ChangeWeekdays()
		{
			if (!WeekdaysShow) return;
			var start = CalendarStartDate(StartDate);
			for (int i = 0; i < dayLabels.Count; i++)
            {
                var day = start.ToString(WeekdaysFormat);
                string showDay = char.ToUpper(day.First()) + day.Substring(1).ToLower();
                dayLabels[i].Text = showDay;
                start = start.AddDays(1);
            }
		}
	}
}
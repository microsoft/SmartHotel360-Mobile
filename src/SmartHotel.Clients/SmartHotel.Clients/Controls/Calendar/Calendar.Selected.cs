using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
    public partial class Calendar : ContentView
	{
        #region SelectedDate

        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(Calendar), null, BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if ((bindable as Calendar).ChangeSelectedDate(newValue as DateTime?))
                    {
                        (bindable as Calendar).SelectedDate = null;
                    }
                });

        /// <summary>
        /// Gets or sets a date the selected date
        /// </summary>
        /// <value>The selected date.</value>
        public DateTime? SelectedDate
		{
			get { return (DateTime?)GetValue(SelectedDateProperty); }
			set { SetValue(SelectedDateProperty, value.HasValue ? value.Value.Date : value); }
		}

        #endregion

        #region SelectRange
        public static readonly BindableProperty SelectRangeProperty = BindableProperty.Create(nameof(SelectRange), typeof(bool), typeof(Calendar), false);

        /// <summary>
        /// Gets or sets multiple Dates can be selected.
        /// </summary>
        public bool SelectRange
        {
            get { return (bool)GetValue(SelectRangeProperty); }
            set { SetValue(SelectRangeProperty, value); }
        }
        #endregion

        #region MultiSelectDates

        public static readonly BindableProperty MultiSelectDatesProperty = BindableProperty.Create(nameof(MultiSelectDates), typeof(bool), typeof(Calendar), false);

		/// <summary>
		/// Gets or sets multiple Dates can be selected.
		/// </summary>
		public bool MultiSelectDates
		{
			get { return (bool)GetValue(MultiSelectDatesProperty); }
			set { SetValue(MultiSelectDatesProperty, value); }
		}

		public static readonly BindableProperty SelectedDatesProperty = 
            BindableProperty.Create(nameof(SelectedDates), typeof(IList<DateTime>), typeof(Calendar), null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (newValue != null)
                    {
                        (bindable as Calendar).SelectedDates = newValue as IList<DateTime>;
                        foreach (var date in (bindable as Calendar).SelectedDates)
                        {
                            (bindable as Calendar).ChangeSelectedDate(date);
                        }
                    }
                });

		/// <summary>
		/// Gets the selected dates when MultiSelectDates is true
		/// </summary>
		/// <value>The selected date.</value>
		public IList<DateTime> SelectedDates
		{
			get { return (IList<DateTime>)GetValue(SelectedDatesProperty); }
			protected set { SetValue(SelectedDatesProperty, value); }
		}

		#endregion

		#region SelectedBorderWidth

		public static readonly BindableProperty SelectedBorderWidthProperty =
			BindableProperty.Create(nameof(SelectedBorderWidth), typeof(int), typeof(Calendar), Device.RuntimePlatform == Device.iOS ? 3 : 5,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderWidth((int)newValue, (int)oldValue));

		protected void ChangeSelectedBorderWidth(int newValue, int oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderWidth = newValue);
		}

		/// <summary>
		/// Gets or sets the border width of the selected date.
		/// </summary>
		/// <value>The width of the selected border.</value>
		public int SelectedBorderWidth
		{
			get { return (int)GetValue(SelectedBorderWidthProperty); }
			set { SetValue(SelectedBorderWidthProperty, value); }
		}

		#endregion

		#region SelectedBorderColor

		public static readonly BindableProperty SelectedBorderColorProperty =
			BindableProperty.Create(nameof(SelectedBorderColor), typeof(Color), typeof(Calendar), Color.FromHex("#c82727"),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBorderColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedBorderColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BorderColor = newValue);
		}

		/// <summary>
		/// Gets or sets the color of the selected date.
		/// </summary>
		/// <value>The color of the selected border.</value>
		public Color SelectedBorderColor
		{
			get { return (Color)GetValue(SelectedBorderColorProperty); }
			set { SetValue(SelectedBorderColorProperty, value); }
		}

		#endregion

		#region SelectedBackgroundColor

		public static readonly BindableProperty SelectedBackgroundColorProperty =
			BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(Calendar), Color.Default,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedBackgroundColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedBackgroundColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.BackgroundColor = (newValue != Color.Default ?  newValue : Color.Transparent));
		}

		/// <summary>
		/// Gets or sets the background color of the selected date.
		/// </summary>
		/// <value>The color of the selected background.</value>
		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}
        #endregion

        #region SelectedBackgroundImage
        public static readonly BindableProperty SelectedBackgroundImageProperty =
            BindableProperty.Create(nameof(SelectedBackgroundImage), typeof(FileImageSource), typeof(Calendar), null);

        public FileImageSource SelectedBackgroundImage {
            get { return (FileImageSource)GetValue(SelectedBackgroundImageProperty); }
            set { SetValue(SelectedBackgroundImageProperty, value); }
        }

        public static readonly BindableProperty SelectedRangeBackgroundImageProperty =
            BindableProperty.Create(nameof(SelectedRangeBackgroundImage), typeof(FileImageSource), typeof(Calendar), null);

        public FileImageSource SelectedRangeBackgroundImage
        {
            get { return (FileImageSource)GetValue(SelectedRangeBackgroundImageProperty); }
            set { SetValue(SelectedRangeBackgroundImageProperty, value); }
        }

        public static readonly BindableProperty FirstSelectedBackgroundImageProperty =
            BindableProperty.Create(nameof(FirstSelectedBackgroundImage), typeof(FileImageSource), typeof(Calendar), null);

        public FileImageSource FirstSelectedBackgroundImage
        {
            get { return (FileImageSource)GetValue(FirstSelectedBackgroundImageProperty); }
            set { SetValue(FirstSelectedBackgroundImageProperty, value); }
        }

        public static readonly BindableProperty LastSelectedBackgroundImageProperty =
            BindableProperty.Create(nameof(LastSelectedBackgroundImage), typeof(FileImageSource), typeof(Calendar), null);

        public FileImageSource LastSelectedBackgroundImage
        {
            get { return (FileImageSource)GetValue(LastSelectedBackgroundImageProperty); }
            set { SetValue(LastSelectedBackgroundImageProperty, value); }
        }

        #endregion

        #region SelectedTextColor

        public static readonly BindableProperty SelectedTextColorProperty =
			BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(Calendar), Color.Default,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedTextColor((Color)newValue, (Color)oldValue));

		protected void ChangeSelectedTextColor(Color newValue, Color oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.TextColor = (newValue != Color.Default ?  newValue: Color.Black));
		}

		/// <summary>
		/// Gets or sets the text color of the selected date.
		/// </summary>
		/// <value>The color of the selected text.</value>
		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}

		#endregion

		#region SelectedFontSize

		public static readonly BindableProperty SelectedFontSizeProperty =
			BindableProperty.Create(nameof(SelectedFontSize), typeof(double), typeof(Calendar), 20.0,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontSize((double)newValue, (double)oldValue));

		protected void ChangeSelectedFontSize(double newValue, double oldValue)
		{
			if (Math.Abs(newValue - oldValue) < 0.01) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontSize = newValue);
		}

		/// <summary>
		/// Gets or sets the font size of the selected date.
		/// </summary>
		/// <value>The size of the selected font.</value>
		public double SelectedFontSize
		{
			get { return (double)GetValue(SelectedFontSizeProperty); }
			set { SetValue(SelectedFontSizeProperty, value); }
		}

		#endregion

		#region SelectedFontAttributes

		public static readonly BindableProperty SelectedFontAttributesProperty =
			BindableProperty.Create(nameof(SelectedFontAttributes), typeof(FontAttributes), typeof(Calendar), FontAttributes.None,
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontAttributes((FontAttributes)newValue, (FontAttributes)oldValue));

		protected void ChangeSelectedFontAttributes(FontAttributes newValue, FontAttributes oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontAttributes = newValue);
		}

		/// <summary>
		/// Gets or sets the dates font attributes for selected dates.
		/// </summary>
		/// <value>The dates font attributes.</value>
		public FontAttributes SelectedFontAttributes
		{
			get { return (FontAttributes)GetValue(SelectedFontAttributesProperty); }
			set { SetValue(SelectedFontAttributesProperty, value); }
		}

		#endregion

		#region SelectedFontFamily

		public static readonly BindableProperty SelectedFontFamilyProperty =
					BindableProperty.Create(nameof(SelectedFontFamily), typeof(string), typeof(Calendar), default(string),
									propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeSelectedFontFamily((string)newValue, (string)oldValue));

		protected void ChangeSelectedFontFamily(string newValue, string oldValue)
		{
			if (newValue == oldValue) return;
			buttons.FindAll(b => b.IsSelected).ForEach(b => b.FontFamily = newValue);
		}

		/// <summary>
		/// Gets or sets the font family of selected dates.
		/// </summary>
		public string SelectedFontFamily
		{
			get { return GetValue(SelectedFontFamilyProperty) as string; }
			set { SetValue(SelectedFontFamilyProperty, value); }
		}

		#endregion

		protected void SetButtonSelected(CalendarButton button, SpecialDate special, bool first = false, bool last = false)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				button.BackgroundPattern = special != null ? special.BackgroundPattern : null;
                if (first)
                {
                    button.BackgroundImage = special != null ? special.BackgroundImage : FirstSelectedBackgroundImage != null ? FirstSelectedBackgroundImage : null;
                }
                else if (last)
                {
                    button.BackgroundImage = special != null ? special.BackgroundImage : LastSelectedBackgroundImage != null ? LastSelectedBackgroundImage : null;
                }
                else
                {
                    if (SelectedDates.Count == 1)
                    {
				        button.BackgroundImage = special != null ? special.BackgroundImage : SelectedBackgroundImage!= null? SelectedBackgroundImage : null;
                    }
                    else
                    {
                        button.BackgroundImage = special != null ? special.BackgroundImage : SelectedBackgroundImage != null ? SelectedRangeBackgroundImage : null;
                    }
                }
				var defaultBackgroundColor = button.IsOutOfMonth ? DatesBackgroundColorOutsideMonth : DatesBackgroundColor;
				var defaultTextColor = button.IsOutOfMonth ? DatesTextColorOutsideMonth : DatesTextColor;
				var defaultFontAttributes = button.IsOutOfMonth ? DatesFontAttributesOutsideMonth : DatesFontAttributes;
				var defaultFontFamily = button.IsOutOfMonth ? DatesFontFamilyOutsideMonth : DatesFontFamily;
				button.IsEnabled = true;
				button.IsSelected = true;
                button.VerticalOptions = LayoutOptions.FillAndExpand;
                button.HorizontalOptions = LayoutOptions.FillAndExpand;
                button.FontSize = SelectedFontSize;
				button.BorderWidth = SelectedBorderWidth;
				button.BorderColor = SelectedBorderColor;
				button.BackgroundColor = SelectedBackgroundColor != Color.Default ? SelectedBackgroundColor : (special != null && special.BackgroundColor.HasValue ? special.BackgroundColor.Value : defaultBackgroundColor);
				button.TextColor = SelectedTextColor != Color.Default ? SelectedTextColor : (special != null && special.TextColor.HasValue ? special.TextColor.Value : defaultTextColor);
				button.FontAttributes = SelectedFontAttributes != FontAttributes.None ? SelectedFontAttributes : (special != null && special.FontAttributes.HasValue ? special.FontAttributes.Value : defaultFontAttributes);
				button.FontFamily = !string.IsNullOrEmpty(SelectedFontFamily) ? SelectedFontFamily : (special != null && !string.IsNullOrEmpty(special.FontFamily) ? special.FontFamily :defaultFontFamily);
			});
		}

		protected bool ChangeSelectedDate(DateTime? date, bool clicked = true)
		{
			if (!date.HasValue) return false;

			if (!MultiSelectDates)
			{
				buttons.FindAll(b => b.IsSelected).ForEach(b => ResetButton(b));
				SelectedDates?.Clear();
			}

            if (MinDate.HasValue && date.Value < MinDate.Value) return false;

            if (buttons.Count == 0)
            {
                SelectedDates?.Add(SelectedDate.Value.Date);
            }

            var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date.Value.Date && b.IsEnabled);
			if (button == null) return false;
			var deselect = button.IsSelected;
			if (button.IsSelected)
			{
                if (SelectRange && SelectedDates?.Count() > 2)
                {
                    UnfillSelectedRange(SelectedDate.Value.Date);
                }
                else
                {
                    ResetButton(button);
                    if (SelectedDates?.Count() == 1)
                    {
                        button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == SelectedDates?.First() && b.IsEnabled);
                        if (button != null)
                        {
                            SetButton(button);
                        }
                    }
                }
			}
			else
			{
                if (SelectedDates != null)
                {
                    if (SelectRange && SelectedDates.Any())
                    {
                        FillSelectedRange(SelectedDate.Value.Date);
                    }
                    else
                    {
                        SelectedDates?.Add(SelectedDate.Value.Date);
                        var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
                        SetButtonSelected(button, spD);
                    }
                }
			}
			if (clicked)
			{
				DateClicked?.Invoke(this, new DateTimeEventArgs { DateTime = SelectedDate.Value });
				DateCommand?.Execute(SelectedDate.Value);
			}
			return deselect;
		}

        public void UnfillSelectedRange(DateTime date)
        {
            var firstDate = SelectedDates.OrderBy(d => d.Date).First();
            var lastDate = SelectedDates.OrderBy(d => d.Date).Last();

            if (date.Equals(firstDate.Date))
            {
                var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                if (button != null) { 
                    ResetButton(button);
                }

                // next date will be the last
                date = date.AddDays(1);
                button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                if (button != null)
                {
                    SetButton(button, true);
                }
            }
            else if (date.Equals(lastDate.Date))
            {
                var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                if (button != null)
                {
                    ResetButton(button);
                }

                // previous date will be the last
                date = date.AddDays(-1);
                button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                if (button != null)
                {
                    SetButton(button, false, true);
                }
            }
            else
            {
                // set clicked date as last date
                var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                if (button != null)
                {
                    SetButton(button, false, true);
                }

                // disable to the right
                date = date.AddDays(1);
                while (date <= lastDate.Date)
                {
                    button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                    if (button != null)
                    {
                        ResetButton(button);
                    }

                    date = date.AddDays(1);
                }
            }
        }

        public void FillSelectedRange(DateTime date)
        {
            var firstDate = SelectedDates.OrderBy(d => d.Date).First();
            var lastDate = SelectedDates.OrderBy(d => d.Date).Last();

            if (date < firstDate)
            {
                var isFirst = true;
                while (date < firstDate.Date)
                {
                    var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                    if (button != null)
                    {
                        SetButton(button, isFirst);
                        isFirst = false;
                    }

                    date = date.AddDays(1);
                }

                if (date < lastDate)
                {
                    // change button 
                    var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                    if (button != null)
                    {
                        var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
                        SetButtonSelected(button, spD);
                    }
                }
            }
            else if (date > lastDate.Date)
            {
                var isLast = true;
                while (date > lastDate.Date)
                {

                    var button2 = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                    if (button2 != null)
                    {
                        SetButton(button2, false, isLast);
                        isLast = false;
                    }

                    date = date.AddDays(-1);
                }

                if (date > firstDate)
                {
                    // change button 
                    var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
                    if (button != null)
                    {
                        var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
                        SetButtonSelected(button, spD);
                    }
                }
            }

            // set margin icons
            SetFirstIcon(SelectedDates.OrderBy(d => d.Date).First());
            SetLastIcon(lastDate = SelectedDates.OrderBy(d => d.Date).Last());
        }

        protected void SetButton(CalendarButton b, bool isFirst = false, bool isLast = false)
        {
            if (!SelectedDates.Contains(b.Date.Value.Date))
            {
                SelectedDates.Add(b.Date.Value.Date);
            }

            var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == b.Date.Value.Date);
            SetButtonSelected(b, spD, isFirst, isLast);
        }

        protected void SetFirstIcon(DateTime date)
        {
            var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
            if (button != null)
            {
                var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
                SetButtonSelected(button, spD, true);
            }
        }

        protected void SetLastIcon(DateTime date)
        {
            var button = buttons.Find(b => b.Date.HasValue && b.Date.Value.Date == date && b.IsEnabled);
            if (button != null)
            {
                var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == button.Date.Value.Date);
                SetButtonSelected(button, spD, false, true);
            }
        }

        protected void ResetButton(CalendarButton b)
		{
			if (b.Date.HasValue) SelectedDates?.Remove(b.Date.Value.Date);
			var spD = SpecialDates?.FirstOrDefault(s => s.Date.Date == b.Date.Value.Date);
			SetButtonNormal(b);
			if (spD != null)
			{
				SetButtonSpecial(b, spD);
			}
		}
	}
}

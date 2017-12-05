using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
	public partial class Calendar : ContentView
	{
		public int YearsRow { get; set; }
		public int YearsColumn { get; set; }
		List<View> normalView;
		List<CalendarButton> yearButtons;
		double w, h;

		public DateTypeEnum CalendarViewType { get; protected set; }

		public void PrevMonthYearView()
		{
			if (normalView == null)
			{
				yearButtons = new List<CalendarButton>();
				normalView = new List<View>();
				w = ContentView.Width / ShowNumOfMonths;
				h = ContentView.Height / ShowNumOfMonths;
				foreach (var child in ContentView.Children)
				{
					normalView.Add(child);
				}
			}
			switch (CalendarViewType)
			{
				case DateTypeEnum.Normal: ShowYears(); break;
				case DateTypeEnum.Month: ShowNormal(); break;
				case DateTypeEnum.Year: ShowMonths(); break;
				default: ShowNormal(); break;
			}
		}

		public void NextMonthYearView()
		{
			if (normalView == null)
			{
				normalView = new List<View>();
				yearButtons = new List<CalendarButton>();
				w = ContentView.Width / ShowNumOfMonths;
				h = ContentView.Height / ShowNumOfMonths;
				foreach (var child in ContentView.Children)
				{
					normalView.Add(child);
				}
			}
			switch (CalendarViewType)
			{
				case DateTypeEnum.Normal: ShowMonths(); break;
				case DateTypeEnum.Month: ShowYears(); break;
				case DateTypeEnum.Year: ShowNormal(); break;
				default: ShowNormal(); break;
			}
		}

		public void ShowNormal()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Content = null;
				ContentView.Children.Clear();
				foreach (var child in normalView)
				{
					ContentView.Children.Add(child);
				}
				CalendarViewType = DateTypeEnum.Normal;
				TitleLeftArrow.IsVisible = true;
				TitleRightArrow.IsVisible = true;
				Content = MainView;
			});
		}

		public void ShowMonths()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Content = null;
				ContentView.Children.Clear();
				var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
				var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
				var details = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
				details.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef };
				details.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };
				for (int r = 0; r < 4; r++)
				{
					for (int c = 0; c < 3; c++)
					{
						var b = new CalendarButton
						{
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Text = DateTimeFormatInfo.CurrentInfo.MonthNames[(r * 3) + c],
							Date = new DateTime(StartDate.Year, (r * 3) + c + 1, 1).Date,
							BackgroundColor = DatesBackgroundColor,
							TextColor = DatesTextColor,
							FontSize = DatesFontSize,
							BorderWidth = BorderWidth,
							BorderColor = BorderColor,
							FontAttributes = DatesFontAttributes,
							WidthRequest = ContentView.Width / 3 - BorderWidth,
							HeightRequest = ContentView.Height / 4 - BorderWidth
						};

						b.Clicked += (sender, e) =>
						{
							MonthYearButtonCommand?.Execute((sender as CalendarButton).Date.Value);
							MonthYearButtonClicked?.Invoke(sender, new DateTimeEventArgs { DateTime = (sender as CalendarButton).Date.Value });
							if (EnableTitleMonthYearView)
							{
								StartDate = (sender as CalendarButton).Date.Value;
								PrevMonthYearView();
							}
						};

						details.Children.Add(b, c, r);
					}
				}
				details.WidthRequest = w;
				details.HeightRequest = h;
				ContentView.Children.Add(details);
				CalendarViewType = DateTypeEnum.Month;
				TitleLeftArrow.IsVisible = false;
				TitleRightArrow.IsVisible = false;
				Content = MainView;
			});
		}

		public void ShowYears()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Content = null;
				ContentView.Children.Clear();
				yearButtons.Clear(); 
				var columDef = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
				var rowDef = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
				var details = new Grid { VerticalOptions = LayoutOptions.CenterAndExpand, RowSpacing = 0, ColumnSpacing = 0, Padding = 1, BackgroundColor = BorderColor };
				details.ColumnDefinitions = new ColumnDefinitionCollection { columDef, columDef, columDef, columDef };
				details.RowDefinitions = new RowDefinitionCollection { rowDef, rowDef, rowDef, rowDef };
				for (int r = 0; r < YearsRow; r++)
				{
					for (int c = 0; c < YearsColumn; c++)
					{
						var t = (r * YearsColumn) + c + 1;
						var b = new CalendarButton
						{
							HorizontalOptions = LayoutOptions.CenterAndExpand,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Text = string.Format("{0}", StartDate.Year + (t - (YearsColumn * YearsRow / 2))),
							Date = new DateTime(StartDate.Year + (t - (YearsColumn * YearsRow / 2)), StartDate.Month, 1).Date,
							BackgroundColor = DatesBackgroundColor,
							TextColor = DatesTextColor,
							FontSize = DatesFontSize,
							FontAttributes = DatesFontAttributes,
							BorderWidth = BorderWidth,
							BorderColor = BorderColor,
							WidthRequest = (ContentView.Width / YearsRow) - BorderWidth,
							HeightRequest = ContentView.Height / YearsColumn - BorderWidth
						};
						b.Clicked += (sender, e) =>
						{
							MonthYearButtonCommand?.Execute((sender as CalendarButton).Date.Value);
							MonthYearButtonClicked?.Invoke(sender, new DateTimeEventArgs { DateTime = (sender as CalendarButton).Date.Value });
							if (EnableTitleMonthYearView)
							{
								StartDate = (sender as CalendarButton).Date.Value;
								PrevMonthYearView();
							}
						};
						yearButtons.Add(b);
						details.Children.Add(b, c, r);
					}
				}
				details.WidthRequest = w;
				details.HeightRequest = h;
				ContentView.Children.Add(details);
				CalendarViewType = DateTypeEnum.Year;
				TitleLeftArrow.IsVisible = true;
				TitleRightArrow.IsVisible = true;
				Content = MainView;
			});
		}

		protected void NextPrevYears(bool next)
		{
			var n = (YearsRow * YearsColumn) * (next ? 1 : -1);
			foreach (var b in yearButtons)
			{
				b.TextWithoutMeasure = string.Format("{0}", int.Parse(b.TextWithoutMeasure) + n);
				b.Date = new DateTime(b.Date.Value.Year + n, b.Date.Value.Month, b.Date.Value.Day).Date;
			}
		}

		public event EventHandler<DateTimeEventArgs> MonthYearButtonClicked;

		#region MonthYearButtonCommand

		public static readonly BindableProperty MonthYearButtonCommandProperty =
			BindableProperty.Create(nameof(MonthYearButtonCommand), typeof(ICommand), typeof(Calendar), null);

		/// <summary>
		/// Gets or sets the selected date command.
		/// </summary>
		/// <value>The date command.</value>
		public ICommand MonthYearButtonCommand
		{
			get { return (ICommand)GetValue(MonthYearButtonCommandProperty); }
			set { SetValue(MonthYearButtonCommandProperty, value); }
		}

		#endregion
	}
}

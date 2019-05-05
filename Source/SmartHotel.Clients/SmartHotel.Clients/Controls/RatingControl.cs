using System.Collections.Generic;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.Controls
{
    public class RatingControl : ContentView
    {
        List<Image> StarImages { get; set; }

        public static readonly BindableProperty RatingProperty =
          BindableProperty.Create(propertyName: nameof(Rating),
              returnType: typeof(int),
              declaringType: typeof(RatingControl),
              defaultValue: 0,
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: UpdateStarsDisplay);

        public int Rating
        {
            get => (int)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public static readonly BindableProperty PrecisionProperty =
          BindableProperty.Create(propertyName: nameof(Precision),
              returnType: typeof(PrecisionType),
              declaringType: typeof(RatingControl),
              defaultValue: PrecisionType.Half,
              propertyChanged: OnPrecisionPropertyChanged);

        public PrecisionType Precision
        {
            get => (PrecisionType)GetValue(PrecisionProperty);
            set => SetValue(PrecisionProperty, value);
        }

        public static readonly BindableProperty ImageFullStarProperty =
          BindableProperty.Create(propertyName: nameof(ImageFullStar),
              returnType: typeof(ImageSource),
              declaringType: typeof(RatingControl),
              defaultValue: default(ImageSource),
              propertyChanged: UpdateStarsDisplay);

        public ImageSource ImageFullStar
        {
            get => (ImageSource)GetValue(ImageFullStarProperty);
            set => SetValue(ImageFullStarProperty, value);
        }

        public static readonly BindableProperty ImageEmptyStarProperty =
          BindableProperty.Create(propertyName: nameof(ImageEmptyStar),
              returnType: typeof(ImageSource),
              declaringType: typeof(RatingControl),
              defaultValue: default(ImageSource),
              propertyChanged: UpdateStarsDisplay);

        public ImageSource ImageEmptyStar
        {
            get => (ImageSource)GetValue(ImageEmptyStarProperty);
            set => SetValue(ImageEmptyStarProperty, value);
        }

        public static readonly BindableProperty ImageHalfStarProperty =
          BindableProperty.Create(propertyName: nameof(ImageHalfStar),
              returnType: typeof(ImageSource),
              declaringType: typeof(RatingControl),
              defaultValue: default(ImageSource),
              propertyChanged: UpdateStarsDisplay);

        public ImageSource ImageHalfStar
        {
            get => (ImageSource)GetValue(ImageHalfStarProperty);
            set => SetValue(ImageHalfStarProperty, value);
        }

        public RatingControl() : base()
        {
            var grid = new Grid()
            {
                ColumnSpacing = 4
            };

            // Create Star Image Placeholders 
            StarImages = new List<Image>();
            for (var i = 0; i < 5; i++)
            {
                StarImages.Add(new Image()
                {
                    WidthRequest = grid.WidthRequest / 5,
                    Aspect = Aspect.AspectFit
                });

                // Add image
                grid.Children.Add(StarImages[i], i, 0);
            }

            Content = grid;
        }

        static void UpdateStarsDisplay(BindableObject bindable, object oldValue, object newValue)
        {
            ((RatingControl)bindable).UpdateStarsDisplay();
        }

        static void OnPrecisionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = ((RatingControl)bindable);
            control.UpdateStarsDisplay();
        }

        // Fill the star based on the Rating value
        void UpdateStarsDisplay()
        {
            for (var i = 0; i < StarImages.Count; i++)
            {
                StarImages[i].Source = GetStarSource(i);
            }
        }

        ImageSource GetStarSource(int position)
        {
            var currentStarMaxRating = (position + 1);

            if (Precision.Equals(PrecisionType.Half))
            {
                currentStarMaxRating *= 2;
            }

            if (Rating >= currentStarMaxRating)
            {
                return ImageFullStar;
            }
            else if (Rating >= currentStarMaxRating - 1 && Precision.Equals(PrecisionType.Half))
            {
                return ImageHalfStar;
            }
            else
            {
                return ImageEmptyStar;
            }
        }

        public enum PrecisionType
        {
            Full,
            Half
        }
    }
}
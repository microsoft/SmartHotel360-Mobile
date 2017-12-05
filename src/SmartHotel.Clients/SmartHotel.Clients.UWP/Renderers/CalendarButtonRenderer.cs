using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.UWP.Renderers;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace SmartHotel.Clients.UWP.Renderers
{
    public class CalendarButtonRenderer : ButtonRenderer
    {
        protected override async void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.MinWidth = 48;
            Control.MinHeight = 48;
            await DrawBackgroundImage();
        }

        protected override async void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = Element as CalendarButton;

            if (Element.BorderWidth > 0 && (e.PropertyName == nameof(element.BorderWidth) || e.PropertyName == "Renderer"))
            {
                Control.BorderThickness = new Thickness(0);
            }
            if (e.PropertyName == nameof(element.BackgroundImage))
            {
                await DrawBackgroundImage();
            }
            if (e.PropertyName == nameof(element.Text) || e.PropertyName == nameof(element.TextWithoutMeasure))
            {
                await DrawBackgroundImage();
            }
        }

        private async Task DrawBackgroundImage()
        {
            var sourceButton = this.Element as CalendarButton;
            var targetButton = this.Control;

            if (sourceButton == null || targetButton == null) return;

            if (sourceButton.BackgroundImage != null)
            {
                var grid = new Grid
                {
                    Padding = new Thickness(0)
                };

                var _currentImage = await GetCurrentImage();


                targetButton.HorizontalContentAlignment = HorizontalAlignment.Center;


                try
                {
                    _currentImage.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.Children.Add(_currentImage);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error settings old image {e}");
                }

                if (!string.IsNullOrEmpty(sourceButton.Text) || !string.IsNullOrEmpty(sourceButton.Text))
                {
                    var label = new TextBlock
                    {
                        TextAlignment = TextAlignment.Center,
                        FontSize = 16,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = string.IsNullOrEmpty(sourceButton.TextWithoutMeasure) ? sourceButton.Text : sourceButton.TextWithoutMeasure
                    };

                    grid.Children.Add(label);
                }

                targetButton.Padding = new Thickness(0);
                targetButton.Content = grid;
            }
            else if (sourceButton.BackgroundImage == null)
            {
                var grid = new Grid
                {
                    Padding = new Thickness(0)
                };

                var label = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = string.IsNullOrEmpty(sourceButton.TextWithoutMeasure) ? sourceButton.Text : sourceButton.TextWithoutMeasure
                };

                targetButton.HorizontalContentAlignment = HorizontalAlignment.Center;
                grid.Children.Add(label);

                targetButton.Padding = new Thickness(0);
                targetButton.Content = grid;
            }
        }

        private Task<Image> GetCurrentImage()
        {
            var sourceButton = this.Element as CalendarButton;
            if (sourceButton == null) return null;

            return GetImageAsync(sourceButton.BackgroundImage);
        }

        private static IImageSourceHandler GetHandler(Xamarin.Forms.ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is Xamarin.Forms.UriImageSource)
            {
                returnValue = new UriImageSourceHandler();
            }
            else if (source is Xamarin.Forms.FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is Xamarin.Forms.StreamImageSource)
            {
                returnValue = new StreamImageSourceHandler();
            }
            return returnValue;
        }

        /// <summary>
        /// Returns a <see cref="Xamarin.Forms.Image" /> from the <see cref="ImageSource" /> provided.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource" /> to load the image from.</param>
        /// <param name="currentImage">The current image.</param>
        /// <returns>A properly sized image.</returns>
        private static async Task<Image> GetImageAsync(Xamarin.Forms.ImageSource source)
        {
            var image = new Image();
            var handler = GetHandler(source);

            var imageSource = await handler.LoadImageAsync(source);

            image.Source = imageSource;
            image.Stretch = Stretch.UniformToFill;
            image.VerticalAlignment = VerticalAlignment.Center;
            image.HorizontalAlignment = HorizontalAlignment.Center;

            return image;
        }
    }

    public static class Calendar
    {
        public static void Init()
        {
            var t1 = string.Empty;
        }
    }
}
using SmartHotel.Clients.Core.Controls;
using SmartHotel.Clients.UWP.Renderers;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ButtonFrame), typeof(ButtonFrameRenderer))]
namespace SmartHotel.Clients.UWP.Renderers
{
    public class ButtonFrameRenderer : FrameRenderer
    {
        const int shadowWidth = 1;
        SpriteVisual spriteVisual;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            if (e.NewElement != null)
            {
                if (spriteVisual == null)
                {
                    AddShadowChild();
                }
            }

            base.OnElementChanged(e);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            spriteVisual.Size = new Vector2((float)finalSize.Width + shadowWidth, (float)finalSize.Height + shadowWidth);

            return base.ArrangeOverride(finalSize);
        }

        void AddShadowChild()
        {
            var canvas = new Canvas();
            var compositor = ElementCompositionPreview.GetElementVisual(canvas).Compositor;
            spriteVisual = compositor.CreateSpriteVisual();

            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Offset = new Vector3(-shadowWidth, -shadowWidth, 0);
            dropShadow.Color = Colors.Black;
            dropShadow.Opacity = 0.6f;
            spriteVisual.Shadow = dropShadow;

            ElementCompositionPreview.SetElementChildVisual(canvas, spriteVisual);

            Children.Add(canvas);
        }
    }
}
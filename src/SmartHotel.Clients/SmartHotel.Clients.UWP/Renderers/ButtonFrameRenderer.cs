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
        private const int ShadowWidth = 1;
        private SpriteVisual _spriteVisual;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            if (e.NewElement != null)
            {
                if (_spriteVisual == null)
                {
                    AddShadowChild();
                }
            }

            base.OnElementChanged(e);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            _spriteVisual.Size = new Vector2((float)finalSize.Width + ShadowWidth, (float)finalSize.Height + ShadowWidth);

            return base.ArrangeOverride(finalSize);
        }

        private void AddShadowChild()
        {
            var canvas = new Canvas();
            var compositor = ElementCompositionPreview.GetElementVisual(canvas).Compositor;
            _spriteVisual = compositor.CreateSpriteVisual();

            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Offset = new Vector3(-ShadowWidth, -ShadowWidth, 0);
            dropShadow.Color = Colors.Black;
            dropShadow.Opacity = 0.6f;
            _spriteVisual.Shadow = dropShadow;

            ElementCompositionPreview.SetElementChildVisual(canvas, _spriteVisual);

            Children.Add(canvas);
        }
    }
}
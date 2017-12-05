using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartHotel.Clients.Core.ViewModels.Base
{
    public interface IHandleViewAppearing
    {
        Task OnViewAppearingAsync(VisualElement view);
    }

    public interface IHandleViewDisappearing
    {
        Task OnViewDisappearingAsync(VisualElement view);
    }
}
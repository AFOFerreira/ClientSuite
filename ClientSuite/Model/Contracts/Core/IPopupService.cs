using CommunityToolkit.Maui.Views;

namespace ClientSuite.Model.Contracts.Core
{
    public interface IPopupService
    {
        Task<T?> ShowAsync<T>(Popup popup);
        Task ShowAsync(Popup popup);
    }
}

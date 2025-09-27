using ClientSuite.Model.Contracts.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace ClientSuite.Data.Core
{
    public sealed class PopupService : IPopupService
    {
        public Task<T?> ShowAsync<T>(Popup popup) =>
            MainThread.InvokeOnMainThreadAsync(async () =>
                (T?)await Application.Current!.Windows[0].Page.ShowPopupAsync(popup));

        public Task ShowAsync(Popup popup) =>
            MainThread.InvokeOnMainThreadAsync(() =>
                Application.Current!.Windows[0].Page.ShowPopup(popup));
    }
}

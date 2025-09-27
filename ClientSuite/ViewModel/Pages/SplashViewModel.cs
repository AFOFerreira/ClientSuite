using ClientSuite.Model.Contracts.Core;
using ClientSuite.ViewModel.Pages.Base;
using SQLitePCL;

namespace ClientSuite.ViewModel.Pages
{
    public partial class SplashViewModel(INavigationService nav, ISqliteProvider sqliteProvider) : BaseViewModel(nav)
    {

        public void Access()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await sqliteProvider.EnsureCreatedAsync();
                await nav.ReplaceRootWithShellAsync<AppShell>();
            });
        }
    }
}

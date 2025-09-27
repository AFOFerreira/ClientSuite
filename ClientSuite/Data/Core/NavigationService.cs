using ClientSuite.Model.Contracts.Core;

namespace ClientSuite.Data.Core
{
    public sealed class NavigationService : INavigationService
    {
        private Shell? CurrentShell => Application.Current?.Windows[0].Page as Shell;
        public bool IsShell => CurrentShell is not null;

        public Task NavigateAsync(string route, bool animate = true)
            => CurrentShell?.GoToAsync(route, animate) ?? Task.CompletedTask;

        public Task NavigateAsync(string route, IDictionary<string, object?> parameters, bool animate = true)
        {
            if (CurrentShell is null) return Task.CompletedTask;

            var dict = parameters.Where(kv => kv.Value is not null)
                                 .ToDictionary(kv => kv.Key, kv => (object)kv.Value!);

            return dict.Count == 0
                ? CurrentShell.GoToAsync(route, animate)
                : CurrentShell.GoToAsync(route, dict);
        }

        public Task GoBackAsync(bool animate = true)
            => CurrentShell?.GoToAsync("..", animate) ?? Task.CompletedTask;

        public Task GoToRootAsync(bool animate = true)
            => CurrentShell?.Navigation?.PopToRootAsync(animate) ?? Task.CompletedTask;

        public void ReplaceRoot(Page page)
            => Application.Current!.Windows[0].Page = page;

        public async Task ReplaceRootWithShellAsync<TShell>(string? absoluteRoute = null) where TShell : Shell, new()
        {
            Application.Current!.Windows[0].Page = new TShell();
            if (!string.IsNullOrWhiteSpace(absoluteRoute))
                await GoToAbsoluteAsync(absoluteRoute);
        }

        public Task GoToAbsoluteAsync(string absoluteRoute)
            => CurrentShell is null ? Task.CompletedTask : CurrentShell.GoToAsync(absoluteRoute);
    }
}

namespace ClientSuite.Model.Contracts.Core
{
    public interface INavigationService
    {
        bool IsShell { get; }

        Task NavigateAsync(string route, bool animate = true);
        Task NavigateAsync(string route, IDictionary<string, object?> parameters, bool animate = true);
        Task GoBackAsync(bool animate = true);
        Task GoToRootAsync(bool animate = true);
        void ReplaceRoot(Page page);
        Task ReplaceRootWithShellAsync<TShell>(string? absoluteRoute = null) where TShell : Shell, new();
        Task GoToAbsoluteAsync(string absoluteRoute);
    }
}

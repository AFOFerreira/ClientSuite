using ClientSuite.View.Pages;

namespace ClientSuite
{
    public partial class App : Application
    {
        private readonly IServiceProvider _sp;
        private Window? _window;

        public App(IServiceProvider sp)
        {
            InitializeComponent();
            _sp = sp;
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _window = new Window(_sp.GetRequiredService<SplashPage>())
            {
                Title = "ClientSuite"
            };
            return _window;
        }

        internal void SetRoot(Page page)
        {
            if (_window is not null) _window.Page = page;
            else MainPage = page; // fallback
        }
    }

}
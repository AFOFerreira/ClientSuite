namespace ClientSuite
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("home", typeof(View.Pages.HomePage));
        }
    }
}

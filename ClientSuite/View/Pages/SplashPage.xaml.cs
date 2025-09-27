using ClientSuite.ViewModel.Pages;

namespace ClientSuite.View.Pages;

public partial class SplashPage : ContentPage
{
    private SplashViewModel _vm;

    public SplashPage(SplashViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Background = new LinearGradientBrush(
                new GradientStopCollection {
                new GradientStop(Color.FromArgb("#FFFFFF"), 0.0f),
                new GradientStop(Color.FromArgb("#F0EBFF"), 1.0f)
                },
                new Point(0, 0), new Point(1, 1));
        });
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        var logoAnim = Task.WhenAll(
          Logo.FadeTo(1, 800, Easing.CubicOut),
          Logo.ScaleTo(1.0, 800, Easing.CubicOut));
        base.OnAppearing();
        Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(1), async () =>
        {


            await logoAnim;
            await Task.Delay(TimeSpan.FromSeconds(1));
            _vm.Access();

        });

    }
}

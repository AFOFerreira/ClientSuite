using ClientSuite.ViewModel.Pages;

namespace ClientSuite.View.Pages;

public partial class HomePage : ContentPage
{
    private HomeViewModel _vm;

    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomeViewModel vm && vm.Items.Count == 0)
        {
            await vm.RefreshCommand.ExecuteAsync(null);
        }
    }
}
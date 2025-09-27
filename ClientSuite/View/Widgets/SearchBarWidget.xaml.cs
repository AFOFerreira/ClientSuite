using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ClientSuite.View.Widgets;

public partial class SearchBarWidget : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(SearchBarWidget),
            default(string),
            BindingMode.TwoWay);

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty SearchCommandProperty =
        BindableProperty.Create(
            nameof(SearchCommand),
            typeof(ICommand),
            typeof(SearchBarWidget));

    public ICommand? SearchCommand
    {
        get => (ICommand?)GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(SearchBarWidget));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(SearchBarWidget),
            "Buscar por nome, sobrenome, idade...");

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(SearchBarWidget),
            "Buscar");

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public SearchBarWidget()
    {
        InitializeComponent();

    }

    private async void OnSearchClicked(object? sender, EventArgs e)
        => await ExecuteSearchAsync();

    private async void OnSearchCompleted(object? sender, EventArgs e)
        => await ExecuteSearchAsync();

    private async Task ExecuteSearchAsync()
    {
        var parameter = CommandParameter ?? Text;

       
        if (SearchCommand is IAsyncRelayCommand asyncCmd)
        {
            if (asyncCmd.CanExecute(parameter))
                await asyncCmd.ExecuteAsync(parameter);
        }
        else if (SearchCommand?.CanExecute(parameter) == true)
        {
            SearchCommand.Execute(parameter);
        }
    }
}
using ClientSuite.Model.Entities;
using ClientSuite.ViewModel.Popups;
using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;

namespace ClientSuite.View.Popups;

public partial class ClientEditPopup : Popup
{
    private Action<(bool, string?)>? _onError;
    private Action<(bool, Client)>? _onSuccess;
    private ClientEditPopupViewModel _vm;

    public ClientEditPopup(ClientEditPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;

    }
    public void ForCreate(Action<(bool, Client)>? onSuccess, Action<(bool, string?)>? onError)
    {
        _vm.ForCreate();
        _onError = onError;
        _onSuccess = onSuccess;
    }

    public void ForEdit(Client client, Action<(bool, Client)>? onSuccess, Action<(bool, string?)>? onError)
    {
        _vm.LoadFrom(client);
        _onError = onError;
        _onSuccess = onSuccess;
    }

    private void OnCancel(object? sender, EventArgs e) => CloseAsync();

    private void OnSave(object? sender, EventArgs e)
    {

        if (_vm.TryBuild(out var client, out var error))
        {
            _onSuccess?.Invoke((true, client));
            CloseAsync();
        }
        else
        {
            _onError?.Invoke((false, error));
        }


    }
}
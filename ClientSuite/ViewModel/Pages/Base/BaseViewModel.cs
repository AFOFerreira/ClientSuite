using ClientSuite.Model.Contracts.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientSuite.ViewModel.Pages.Base
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isLoadingData;
        protected readonly INavigationService Nav;
        public BaseViewModel(INavigationService nav) => Nav = nav;

        protected static IDictionary<string, object> P(params (string key, object? value)[] items)
            => items.Where(i => i.value is not null).ToDictionary(i => i.key, i => (object)i.value!);
    }
}

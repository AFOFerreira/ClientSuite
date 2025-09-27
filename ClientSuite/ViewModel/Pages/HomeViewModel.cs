using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Contracts.Services;
using ClientSuite.Model.Entities;
using ClientSuite.View.Popups;
using ClientSuite.ViewModel.Pages.Base;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace ClientSuite.ViewModel.Pages
{
    public partial class HomeViewModel(INavigationService nav, IClientsService service, IServiceProvider sp)
        : BaseViewModel(nav)
    {
        public ObservableCollection<Client> Items { get; } = new();

        private List<Client> _allClients = new();

        [ObservableProperty] private string? _query;

        public AsyncRelayCommand<Client?> RemoveClientCommand => new(Delete);
        public AsyncRelayCommand<Client?> EditClientCommand => new(Edit);

        [RelayCommand]
        private void Filter() => ApplyFilter();

        public async Task RefreshAsync() => await Refresh();

        [RelayCommand]
        private async Task Refresh()
        {
            if (IsLoadingData) return;
            try
            {
                IsLoadingData = true;

                var list = await service.GetAll();
                _allClients = list?.ToList() ?? [];


                ApplyFilter();
            }
            finally { IsLoadingData = false; }
        }

        [RelayCommand]
        private async Task New()
        {
            var popup = sp.GetRequiredService<ClientEditPopup>();
            popup.ForCreate(onSuccess: UpsertPopupDataReturn, onError: OnErrorReturn);
            await Application.Current!.Windows[0].Page.ShowPopupAsync(popup, new PopupOptions()
            {
                Shape = null
            });
        }

        private async Task Edit(Client? c)
        {
            if (c is null) return;
            var popup = sp.GetRequiredService<ClientEditPopup>();
            popup.ForEdit(c, onSuccess: UpsertPopupDataReturn, onError: OnErrorReturn);
            await Application.Current!.Windows[0].Page.ShowPopupAsync(popup, new PopupOptions()
            {
                Shape = null
            });
        }

        private async void OnErrorReturn((bool, string?) tuple)
        {
        }

        private async void UpsertPopupDataReturn((bool, Client) tuple)
        {
            await service.UpsertAsync(tuple.Item2);

            var list = await service.GetAll();
            _allClients = list?.ToList() ?? [];
            ApplyFilter();
        }

        private async Task Delete(Client? c)
        {
            if (c is null) return;

            var ok = await Application.Current!.MainPage.DisplayAlert(
                "Excluir", $"Confirmar exclusão de \"{c.Name}\"?", "Sim", "Não");
            if (!ok) return;

            await service.DeleteAsync(c.Id);


            _allClients.RemoveAll(x => x.Id == c.Id);
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            IEnumerable<Client> query = _allClients;

            var q = (Query ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(q))
            {
                var tokens = q.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                              .Select(Simplify);

                query = query.Where(c =>
                {
                    var name = Simplify(c.Name ?? "");
                    var last = Simplify(c.LastName ?? "");
                    var full = $"{name} {last}".Trim();

                    // TODOS os tokens devem aparecer em pelo menos um dos campos
                    return tokens.All(t =>
                        name.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                        last.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                        full.Contains(t, StringComparison.OrdinalIgnoreCase));
                });
            }

            // Atualiza a coleção exibida
            Items.BatchUpdate(query);
        }


        partial void OnQueryChanged(string? value) => ApplyFilter();

        private static string Simplify(string s)
        {
            var norm = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(capacity: norm.Length);
            foreach (var ch in norm)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(ch));
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }


    internal static class ObservableCollectionExtensions
    {
        public static void BatchUpdate<T>(this ObservableCollection<T> target, IEnumerable<T> source)
        {
            target.Clear();
            foreach (var item in source) target.Add(item);
        }
    }
}

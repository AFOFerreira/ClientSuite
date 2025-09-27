using ClientSuite.Model.Contracts.Services; // IIbgeService, ICepService, DTOs (IbgeUf, IbgeMunicipio, CepResult)
using ClientSuite.Model.Dtos;
using ClientSuite.Model.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ClientSuite.ViewModel.Popups
{
    public partial class ClientEditPopupViewModel : ObservableObject
    {
        private readonly IIbgeService _ibge;
        private readonly ICepService _cep;

        public ClientEditPopupViewModel(IIbgeService ibge, ICepService cep)
        {
            _ibge = ibge;
            _cep = cep;
        }

        [ObservableProperty] private string _title = "Cliente";
        [ObservableProperty] private Guid? _id;
        [ObservableProperty] private string _name = "";
        [ObservableProperty] private string _lastName = "";
        [ObservableProperty] private string _ageText = "";
        [ObservableProperty] private Guid? _addressId;

        [ObservableProperty] private string _zipCodeText = "";
        [ObservableProperty] private string _numberText = "";
        [ObservableProperty] private string _state = "";
        [ObservableProperty] private string _city = "";
        [ObservableProperty] private string _country = "Brasil";
        [ObservableProperty] private string _complement = "";
        [ObservableProperty] private bool _nameHasError;
        [ObservableProperty] private bool _lastNameHasError;
        [ObservableProperty] private bool _ageHasError;
        [ObservableProperty] private bool _zipCodeHasError;
        [ObservableProperty] private bool _numberHasError;

        public ObservableCollection<IbgeUf> Ufs { get; } = new();
        public ObservableCollection<IbgeMunicipio> Municipios { get; } = new();

        [ObservableProperty] private IbgeUf? _selectedUf;
        [ObservableProperty] private IbgeMunicipio? _selectedMunicipio;


        public void ForCreate()
        {
            Title = "Novo Cliente";
            Id = null;
            AddressId = null;

            Name = LastName = AgeText = "";
            ZipCodeText = NumberText = State = City = Country = Complement = "";
            Country = "Brasil";
            SelectedUf = null;
            SelectedMunicipio = null;
            Municipios.Clear();

            ClearErrors();
        }

        public void LoadFrom(Client client)
        {
            Title = "Editar Cliente";
            Id = client.Id;
            Name = client.Name ?? "";
            LastName = client.LastName ?? "";
            AgeText = client.Age.ToString();
            AddressId = client.AddressId;

            if (client.Address is not null)
            {
                ZipCodeText = client.Address.ZipCode.ToString();
                NumberText = client.Address.Number.ToString();
                State = client.Address.State ?? "";
                City = client.Address.City ?? "";
                Country = client.Address.Country ?? "";
                Complement = client.Address.Complement ?? "";
            }
            else
            {
                ZipCodeText = NumberText = "";
                State = City = Country = Complement = "";
            }

            ClearErrors();
        }

        private void ClearErrors()
        {
            NameHasError = LastNameHasError = AgeHasError =
                ZipCodeHasError = NumberHasError = false;
        }

        partial void OnNameChanged(string value) => NameHasError = string.IsNullOrWhiteSpace(value);
        partial void OnLastNameChanged(string value) => LastNameHasError = string.IsNullOrWhiteSpace(value);
        partial void OnAgeTextChanged(string value) => AgeHasError = !int.TryParse(value?.Trim(), out var age) || age < 0;
        partial void OnNumberTextChanged(string value) => NumberHasError = !int.TryParse(value?.Trim(), out _);


        partial void OnZipCodeTextChanged(string value)
        {
            var onlyDigits = new string((value ?? "").Where(char.IsDigit).ToArray());
            ZipCodeHasError = onlyDigits.Length != 8;

            if (!ZipCodeHasError)
                _ = CepLookupAsync();
        }

        public bool ValidateAll()
        {
            OnNameChanged(Name);
            OnLastNameChanged(LastName);
            OnAgeTextChanged(AgeText);
            OnZipCodeTextChanged(ZipCodeText);
            OnNumberTextChanged(NumberText);

            return !(NameHasError || LastNameHasError || AgeHasError || ZipCodeHasError || NumberHasError);
        }

        [RelayCommand]
        public async Task LoadUfsAsync()
        {
            if (Ufs.Count == 0)
            {
                var list = await _ibge.GetUfsAsync();
                Ufs.Clear();
                foreach (var uf in list) Ufs.Add(uf);
            }

            if (!string.IsNullOrWhiteSpace(State))
                SelectedUf = Ufs.FirstOrDefault(u => u.Sigla.Equals(State, StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand]
        public async Task LoadMunicipiosAsync()
        {
            Municipios.Clear();
            SelectedMunicipio = null;

            if (SelectedUf is null) return;

            var list = await _ibge.GetMunicipiosAsync(SelectedUf.Sigla);
            foreach (var m in list) Municipios.Add(m);


            if (!string.IsNullOrWhiteSpace(City))
                SelectedMunicipio = Municipios.FirstOrDefault(m => m.Nome.Equals(City, StringComparison.OrdinalIgnoreCase));
        }

        partial void OnSelectedUfChanged(IbgeUf? value)
        {
            State = value?.Sigla ?? "";
            _ = LoadMunicipiosAsync();
        }

        partial void OnSelectedMunicipioChanged(IbgeMunicipio? value)
        {
            City = value?.Nome ?? "";
        }


        [RelayCommand]
        public async Task CepLookupAsync()
        {
            var result = await _cep.LookupAsync(ZipCodeText ?? "");
            if (result is null)
            {
                ZipCodeHasError = true;
                return;
            }

            ZipCodeHasError = false;

            State = result.State ?? "";
            City = result.City ?? "";
            Complement = result.Street ?? "";
            await LoadUfsAsync();
            SelectedUf = Ufs.FirstOrDefault(u => u.Sigla.Equals(State, StringComparison.OrdinalIgnoreCase));

            if (SelectedUf is not null)
            {
                await LoadMunicipiosAsync();
                SelectedMunicipio = Municipios.FirstOrDefault(m => m.Nome.Equals(City, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool TryBuild(out Client client, out string? error)
        {
            client = new();
            error = null;

            if (!ValidateAll())
            {

                return false;
            }


            int age = int.Parse(AgeText!.Trim());
            int zip = int.Parse(new string(ZipCodeText!.Where(char.IsDigit).ToArray()));
            int number = int.Parse(NumberText!.Trim());

            var addrId = AddressId ?? Guid.NewGuid();
            var address = new ClientAddress
            {
                Id = addrId,
                ZipCode = zip,
                Number = number,
                State = State?.Trim() ?? "",
                City = City?.Trim() ?? "",
                Country = Country?.Trim() ?? "",
                Complement = Complement?.Trim() ?? ""
            };

            client = new Client
            {
                Id = Id ?? Guid.NewGuid(),
                Name = Name.Trim(),
                LastName = LastName.Trim(),
                Age = age,
                AddressId = addrId,
                Address = address
            };

            return true;
        }
    }
}

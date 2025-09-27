using ClientSuite.Model.Contracts.Services;
using ClientSuite.Model.Dtos;
using ClientSuite.Model.Mappers;
using System.Net.Http.Json;

namespace ClientSuite.Data.Services
{
    public sealed class CepService(HttpClient http) : ICepService
    {
        public async Task<CepResult?> LookupAsync(string cep, CancellationToken ct = default)
        {
            cep = new string(cep.Where(char.IsDigit).ToArray());
            if (cep.Length != 8) return null;

            // 1) BrasilAPI
            try
            {
                var b = await http.GetFromJsonAsync<BrasilApiCep>($"https://brasilapi.com.br/api/cep/v1/{cep}", ct);
                if (b is not null && !string.IsNullOrWhiteSpace(b.state) && !string.IsNullOrWhiteSpace(b.city))
                    return new CepResult(b.cep, b.state, b.city, b.neighborhood, b.street, "brasilapi");
            }
            catch { /* fallback */ }

            // 2) ViaCEP
            try
            {
                var v = await http.GetFromJsonAsync<ViaCep>($"https://viacep.com.br/ws/{cep}/json", ct);
                if (v is not null && v.erro != true)
                    return new CepResult(v.cep ?? cep, v.uf ?? "", v.localidade ?? "", v.bairro, v.logradouro, "viacep");
            }
            catch { /* give up */ }

            return null;
        }



    }
}

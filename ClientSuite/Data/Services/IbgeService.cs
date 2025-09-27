using ClientSuite.Model.Contracts.Services;
using ClientSuite.Model.Dtos;
using ClientSuite.Model.Mappers;
using System.Net.Http.Json;

namespace ClientSuite.Data.Services
{
    public sealed class IbgeService(HttpClient http) : IIbgeService
    {
        private const string Base = "https://servicodados.ibge.gov.br/api/v1/localidades";

        public async Task<List<IbgeUf>> GetUfsAsync(CancellationToken ct = default)
        {
            var url = $"{Base}/estados?orderBy=nome";
            var arr = await http.GetFromJsonAsync<List<IbgeUfJson>>(url, ct) ?? [];
            return arr.Select(x => new IbgeUf(x.Id, x.Sigla, x.Nome)).ToList();
        }

        public async Task<List<IbgeMunicipio>> GetMunicipiosAsync(string ufSigla, CancellationToken ct = default)
        {
            var url = $"{Base}/estados/{ufSigla}/municipios?orderBy=nome";
            var arr = await http.GetFromJsonAsync<List<IbgeMunicipioJson>>(url, ct) ?? [];
            return arr.Select(x => new IbgeMunicipio(x.Id, x.Nome)).ToList();
        }


    }

}

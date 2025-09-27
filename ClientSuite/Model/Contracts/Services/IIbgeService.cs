using ClientSuite.Model.Dtos;

namespace ClientSuite.Model.Contracts.Services
{
    public interface IIbgeService
    {
        Task<List<IbgeUf>> GetUfsAsync(CancellationToken ct = default);
        Task<List<IbgeMunicipio>> GetMunicipiosAsync(string ufSigla, CancellationToken ct = default);

    }
}

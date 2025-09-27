using ClientSuite.Model.Dtos;

namespace ClientSuite.Model.Contracts.Services
{
    public interface ICepService
    {
        Task<CepResult?> LookupAsync(string cep, CancellationToken ct = default);
    }
}

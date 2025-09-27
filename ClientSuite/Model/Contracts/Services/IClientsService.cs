using ClientSuite.Model.Entities;

namespace ClientSuite.Model.Contracts.Services
{
    public interface IClientsService
    {
        Task<List<Client>> GetAll(CancellationToken ct = default);
        Task<Client?> GetAsync(Guid id, CancellationToken ct = default);
        Task<Guid> UpsertAsync(Client model, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

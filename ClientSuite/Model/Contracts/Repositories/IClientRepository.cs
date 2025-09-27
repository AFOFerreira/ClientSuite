using ClientSuite.Model.Contracts.Repositories.Base;
using ClientSuite.Model.Entities;

namespace ClientSuite.Model.Contracts.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<List<Client>> SearchAsync(string? q, CancellationToken ct);
    }
}

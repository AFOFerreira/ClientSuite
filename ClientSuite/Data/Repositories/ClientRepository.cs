using ClientSuite.Data.Repositories.Base;
using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Contracts.Repositories;
using ClientSuite.Model.Entities;

namespace ClientSuite.Data.Repositories
{
    public sealed class ClientRepository(ISqliteProvider provider) : RepositoryBase<Client>(provider), IClientRepository
    {
        public Task<List<Client>> SearchAsync(string? q, CancellationToken ct)
        {
            return Task.FromResult(new List<Client>());
        }
    }
}


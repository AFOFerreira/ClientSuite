using ClientSuite.Data.Repositories.Base;
using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Contracts.Repositories;
using ClientSuite.Model.Entities;

namespace ClientSuite.Data.Repositories
{
    public class ClientAddressRepository(ISqliteProvider provider) : RepositoryBase<ClientAddress>(provider), IClientAddressRepository
    {
    }
}

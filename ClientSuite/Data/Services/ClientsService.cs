using ClientSuite.Model.Contracts.Repositories;
using ClientSuite.Model.Contracts.Services;
using ClientSuite.Model.Entities;

namespace ClientSuite.Data.Services
{
    public class ClientsService(IClientRepository clientRepo, IClientAddressRepository clientAddressRepo) : IClientsService
    {
        public async Task<List<Client>> GetAll(CancellationToken ct = default)
        {
            List<Client>? allClients = await clientRepo.ListAsync(ct) ?? new List<Client>();
            if (allClients.Count != 0)
            {
                foreach (var client in allClients) 
                {
                    var address = await clientAddressRepo.GetByIdAsync(client.AddressId);
                    if (address is not null)
                    {
                        client.Address = address;
                    }
                }
            
            }
            return allClients;
        }


        public Task<Client?> GetAsync(Guid id, CancellationToken ct = default)
            => clientRepo.GetByIdAsync(id, ct);

        public async Task<Guid> UpsertAsync(Client model, CancellationToken ct = default)
        {
            try
            {
                if (model.Address != null)
                {
                    var existsAddress = model.AddressId != Guid.Empty ? await clientAddressRepo.GetByIdAsync(model.AddressId, ct) : null;
                    if (existsAddress is null)
                    {
                        if (model.AddressId == Guid.Empty) model.AddressId = Guid.NewGuid();
                        model.Address.CreatedAt = DateTime.UtcNow;
                        model.Address.UpdatedAt = DateTime.UtcNow;
                        await clientAddressRepo.InsertAsync(model.Address, ct);
                    }
                    else
                    {
                        model.Address.UpdatedAt = DateTime.UtcNow;
                        await clientAddressRepo.UpdateAsync(model.Address, ct);
                    }
                }

                var exists = model.Id != Guid.Empty ? await clientRepo.GetByIdAsync(model.Id, ct) : null;

                if (exists is null)
                {
                    if (model.Id == Guid.Empty) model.Id = Guid.NewGuid();
                    model.CreatedAt = DateTime.UtcNow;
                    model.UpdatedAt = DateTime.UtcNow;
                    await clientRepo.InsertAsync(model, ct);
                }
                else
                {
                    model.UpdatedAt = DateTime.UtcNow;
                    await clientRepo.UpdateAsync(model, ct);
                }


                return model.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await clientRepo.GetByIdAsync(id, ct);
            if (entity is not null)
                await clientRepo.DeleteAsync(entity, ct);
        }
    }
}

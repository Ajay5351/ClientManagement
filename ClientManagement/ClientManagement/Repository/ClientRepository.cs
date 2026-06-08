using AutoMapper;
using ClientManagement.Data;
using ClientManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientManagementDbContext _context;
        private readonly IMapper _mapper;

        public ClientRepository(ClientManagementDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Client>> GetAllClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return _mapper.Map<List<Client>>(clients);
        }

        public async Task<Client> GetClientById(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            return _mapper.Map<Client>(client);
        }

        public async Task<Client> AddClient(Client client)
        {
            if (client == null)
                throw new Exception("Client data is required.");

            if (string.IsNullOrWhiteSpace(client.ClientName))
                throw new Exception("Client Name is required.");

            if (client.LicenceStartDate == default)
                throw new Exception("Licence Start Date is required.");

            if (client.LicenceEndDate == default)
                throw new Exception("Licence End Date is required.");

            if (client.LicenceEndDate <= client.LicenceStartDate)
                throw new Exception("Licence End Date must be greater than Licence Start Date.");

            if (!string.IsNullOrWhiteSpace(client.Description) &&
                client.Description.Length > 500)
                throw new Exception("Description cannot exceed 500 characters.");

            client.LicenceKey = Guid.NewGuid();
            client.CreatedDate = DateTime.UtcNow;
            client.UpdatedDate = DateTime.UtcNow;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            if (client == null)
                throw new Exception("Client data is required.");

            var existingClient = await _context.Clients.FindAsync(client.ClientId);

            if (existingClient == null)
                throw new Exception("Client not found.");

            if (string.IsNullOrWhiteSpace(client.ClientName))
                throw new Exception("Client Name is required.");

            if (client.LicenceStartDate == default)
                throw new Exception("Licence Start Date is required.");

            if (client.LicenceEndDate == default)
                throw new Exception("Licence End Date is required.");

            if (client.LicenceEndDate <= client.LicenceStartDate)
                throw new Exception("Licence End Date must be greater than Licence Start Date.");

            existingClient.ClientName = client.ClientName;
            existingClient.LicenceStartDate = client.LicenceStartDate;
            existingClient.LicenceEndDate = client.LicenceEndDate;
            existingClient.Description = client.Description;
            existingClient.UpdatedDate = DateTime.UtcNow;

            _context.Clients.Update(existingClient);
            await _context.SaveChangesAsync();

            return existingClient;
        }

        public async Task<bool> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                throw new Exception("Client not found.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

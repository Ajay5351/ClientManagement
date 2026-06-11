using ClientManagement.Data;
using ClientManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientManagementDbContext _context;

        public ClientRepository(ClientManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClients()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<Client> AddClient(Client client)
        {
            client.LicenceKey = Guid.NewGuid();
            client.CreatedDate = DateTime.UtcNow;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            var existingClient = await _context.Clients.FindAsync(client.ClientId);

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

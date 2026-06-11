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

        public async Task<PagedClientResult> GetAllClients(string? term,string? sort,int page, int limit)
        {
            IQueryable<Client> clients = _context.Clients;

            // Filtering
            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.Trim().ToLower();

                clients = clients.Where(c =>
                    c.ClientName.ToLower().Contains(term) ||
                    (c.Description != null &&
                     c.Description.ToLower().Contains(term)));
            }

            // Sorting
            clients = sort?.ToLower() switch
            {
                "clientname" => clients.OrderBy(c => c.ClientName),
                "-clientname" => clients.OrderByDescending(c => c.ClientName),

                "licencestartdate" => clients.OrderBy(c => c.LicenceStartDate),
                "-licencestartdate" => clients.OrderByDescending(c => c.LicenceStartDate),

                "licenceenddate" => clients.OrderBy(c => c.LicenceEndDate),
                "-licenceenddate" => clients.OrderByDescending(c => c.LicenceEndDate),

                "createddate" => clients.OrderBy(c => c.CreatedDate),
                "-createddate" => clients.OrderByDescending(c => c.CreatedDate),

                _ => clients.OrderBy(c => c.ClientId)
            };

            // Pagination
            var totalCount = await clients.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)limit);

            var pagedClients = await clients
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PagedClientResult
            {
                Clients = pagedClients,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
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

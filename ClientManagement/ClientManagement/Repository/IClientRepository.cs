using ClientManagement.Models;

namespace ClientManagement.Repository
{
    public interface IClientRepository
    {
        Task<PagedClientResult> GetAllClients(string? term, string? sort, int page, int limit);
        Task<Client> GetClientById(int id);
        Task<Client> AddClient(Client client);
        Task<Client> UpdateClient(Client client);
        Task<bool> DeleteClient(int id);
    }
}

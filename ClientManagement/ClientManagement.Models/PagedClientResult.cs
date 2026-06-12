namespace ClientManagement.Models
{
    public class PagedClientResult
    {
        public List<Client> Clients { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
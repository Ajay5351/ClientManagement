using AutoMapper;
using ClientManagement.Data;
using ClientManagement.Models;

namespace ClientManagement.Helpers
{
    public class ApplicationHelper : Profile
    {
        public ApplicationHelper()
        {
            CreateMap<Client, ClientModel>().ReverseMap();
        }
    }
}

using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;

namespace SignalR.Application.Servers
{
    public class CreateOrUpdateServerDto : Entity
    {
        public string ServerName { get; set; }
        public string AdminId { get; set; }
    }
}

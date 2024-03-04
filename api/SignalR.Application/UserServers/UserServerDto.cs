using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;

namespace SignalR.Application.UserServers
{
    public class UserServerDto : Entity
    {
        public string UserId { get; set; }
        public long ServerId { get; set; }
    }
}

using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Channels;
using SignalR.Application.UserServers;

namespace SignalR.Application.Servers
{
    public class ServerDto : Entity
    {
        public string ServerName { get; set; }
        public string AdminId { get; set; }
        public List<UserServerDto> UserServers { get; set; }

        public ICollection<ChannelDto> Channels { get; set; }

    }
}

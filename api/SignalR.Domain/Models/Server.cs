using NetCoreAPI.Models;

namespace NetCoreAPI.Domain.Models
{
    public class Server : Entity
    {
        public string ServerName{ get; set; }
        public string AdminId { get; set; }
        public User Admin { get; set; }
        public List<UserServer> UserServers{ get; set; }

        public ICollection<Channel> Channels { get; set; }
    }
}

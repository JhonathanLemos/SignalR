using NetCoreAPI.Models;

namespace NetCoreAPI.Domain.Models
{
    public class UserServer : Entity
    {
        public string UserId{ get; set; }
        public User User{ get; set; }

        public long ServerId { get; set; }
        public Server Server { get; set; }
    }
}

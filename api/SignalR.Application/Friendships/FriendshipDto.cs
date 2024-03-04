using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;

namespace SignalR.Application.Friendships
{
    public class FriendshipDto : Entity
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public string UserName { get; set; }
    }
}

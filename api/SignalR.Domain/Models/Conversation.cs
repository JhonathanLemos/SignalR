using NetCoreAPI.Models;

namespace NetCoreAPI.Domain.Models
{
    public class Conversation : Entity
    {
        public string FirstUserId { get; set; }
        public User FirstUser { get; set; }

        public string SecondUserId { get; set; }
        public User SecondUser { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}

using NetCoreAPI.Models;

namespace NetCoreAPI.Domain.Models
{
    public class Solicitation : Entity
    {
        public string UserId { get; set; }
        public User User{ get; set; }

        public string SecondUserId { get; set; }
        public User SecondUser { get; set; }
    }
}

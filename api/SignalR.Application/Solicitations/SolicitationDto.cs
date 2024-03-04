using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;

namespace SignalR.Application.Solicitations
{
    public class SolicitationDto : Entity
    {
        public string UserId { get; set; }

        public string SecondUserId { get; set; }
        public string SecondUserName { get; set; }
    }
}

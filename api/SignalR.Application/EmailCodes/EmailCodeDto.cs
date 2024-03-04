using NetCoreAPI.Domain.Models;

namespace SignalR.Application.EmailCodes
{

    public class EmailCodeDto : Entity
    {
        public string Code { get; set; }
        public string UserId { get; set; }
    }
}

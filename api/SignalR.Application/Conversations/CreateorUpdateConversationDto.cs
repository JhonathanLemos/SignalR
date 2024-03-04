using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;

namespace SignalR.Application.Conversations
{
    public class CreateorUpdateConversationDto : Entity
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

    }
}

using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Messages;

namespace SignalR.Application.Conversation
{
    public class ConversationDto : Entity
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        public ICollection<MessageDto> Messages { get; set; }
    }
}

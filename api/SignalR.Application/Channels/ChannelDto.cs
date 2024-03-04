using NetCoreAPI.Domain.Models;
using SignalR.Application.MessageChanels;

namespace SignalR.Application.Channels
{
    public class ChannelDto : Entity
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public long ServerId { get; set; }

        public List<MessageChannelDto> MessageChannel { get; set; }

    }
}

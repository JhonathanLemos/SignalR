using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using System;

namespace SignalR.Application.MessageChanels
{
    public class MessageChannelDto : Entity
    {
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }

        public long ChannelId { get; set; }

        public string SenderId { get; set; }
    }
}

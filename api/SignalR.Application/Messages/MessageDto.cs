using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using System;

namespace SignalR.Application.Messages
{
    public class MessageDto : Entity
    {
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }

        public long ConversationId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
    }
}

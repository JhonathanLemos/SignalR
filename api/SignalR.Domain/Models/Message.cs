using NetCoreAPI.Models;
using System;

namespace NetCoreAPI.Domain.Models
{
    public class Message : Entity
    {
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }

        public long ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }
    }
}

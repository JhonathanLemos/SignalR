using NetCoreAPI.Models;
using System;

namespace NetCoreAPI.Domain.Models
{
    public class MessageChannel : Entity
    {
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }

        public long ChannelId { get; set; }
        public Channel Channel { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }
    }
}

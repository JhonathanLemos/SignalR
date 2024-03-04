namespace NetCoreAPI.Domain.Models
{
    public class Channel : Entity
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public long ServerId { get; set; }
        public Server Server { get; set; }
        public List<MessageChannel> MessageChannel { get; set; }
    }
}

namespace NetCoreAPI.Domain.Models
{

    public class EmailCode : Entity
    {
        public string Code { get; set; }
        public string UserId { get; set; }
    }
}

namespace NetCoreAPI.Application.Dtos
{
    public class UserAndNameAndLastMessage
    {
        public string UserId{ get; set; }
        public string FirstUserId{ get; set; }
        public string UserName{ get; set; }
        public string? LastMessage{ get; set; }
        public DateTime? Hour{ get; set; }
        public Boolean Show { get; set; }
    }
}

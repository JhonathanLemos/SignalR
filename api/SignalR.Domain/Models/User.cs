using Microsoft.AspNetCore.Identity;
using NetCoreAPI.Domain.Models;
using System;

namespace NetCoreAPI.Models
{
    public class User : IdentityUser
    {
        public string UserCode { get; set; }
        public List<UserServer> UserServers { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
        public ICollection<Friendship> Friends { get; set; }
        public ICollection<Solicitation> Solicitation { get; set; }
    }
}

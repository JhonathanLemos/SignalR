using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Users;

namespace NetCoreAPI.Infra.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();
                //.ForMember(x => x.UserServers, opt => opt.Ignore());
                //.ForMember(x => x.Conversations, opt => opt.Ignore())
                //.ForMember(x => x.Friends, opt => opt.Ignore());
            CreateMap<User, UserDto>();


        }
    }
}

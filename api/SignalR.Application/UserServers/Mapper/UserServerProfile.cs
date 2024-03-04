using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.UserServers;

namespace NetCoreAPI.Infra.Mappers
{
    public class UserServerProfile : Profile
    {
        public UserServerProfile()
        {
            CreateMap<UserServerDto, UserServer>();
                //.ForMember(x => x.UserServers, opt => opt.Ignore());
                //.ForMember(x => x.Conversations, opt => opt.Ignore())
                //.ForMember(x => x.Friends, opt => opt.Ignore());
            CreateMap<UserServer, UserServerDto>();


        }
    }
}

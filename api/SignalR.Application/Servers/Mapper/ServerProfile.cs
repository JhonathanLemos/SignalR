using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Servers;

namespace NetCoreAPI.Infra.Mappers
{
    public class ServerProfile : Profile
    {
        public ServerProfile()
        {
            CreateMap<ServerDto, Server>()
                .ForMember(x => x.UserServers, opt => opt.Ignore())
                .ForMember(x => x.Channels, opt => opt.Ignore());
            CreateMap<CreateOrUpdateServerDto, Server>()
               .ForMember(x => x.UserServers, opt => opt.Ignore())
               .ForMember(x => x.Channels, opt => opt.Ignore());
            CreateMap<Server, ServerDto>();
        }
    }
}

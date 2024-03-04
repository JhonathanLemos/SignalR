using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Channels;

namespace NetCoreAPI.Infra.Mappers
{
    public class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<ChannelDto, Channel>()
                .ForMember(x => x.Server, opt => opt.Ignore());
            CreateMap<Channel, ChannelDto>();
        }
    }
}

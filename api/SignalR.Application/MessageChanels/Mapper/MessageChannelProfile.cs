using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.MessageChanels;

namespace NetCoreAPI.Infra.Mappers
{
    public class MessageChannelProfile : Profile
    {
        public MessageChannelProfile()
        {
            CreateMap<MessageChannelDto, MessageChannel>();
            CreateMap<MessageChannel, MessageChannelDto>();
        }
    }
}

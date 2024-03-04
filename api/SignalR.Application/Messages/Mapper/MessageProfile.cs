using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Messages;

namespace NetCoreAPI.Infra.Mappers
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageDto, Message>()
                .ForMember(x => x.Conversation, opt => opt.Ignore())
                .ForMember(x => x.Sender, opt => opt.Ignore());
            //.ForMember(x => x.Conversations, opt => opt.Ignore())
            //.ForMember(x => x.Friends, opt => opt.Ignore());
            CreateMap<Message, MessageDto>()
                .ForMember(x => x.SenderName, opt => opt.MapFrom(x => x.Sender.UserName));




        }
    }
}

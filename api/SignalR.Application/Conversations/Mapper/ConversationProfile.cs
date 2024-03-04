using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Conversation;
using SignalR.Application.Conversations;

namespace NetCoreAPI.Infra.Mappers
{
    public class ConversationProfile : Profile
    {
        public ConversationProfile()
        {
            CreateMap<ConversationDto, Conversation>()
              .ForMember(x => x.Messages, opt => opt.Ignore());
            CreateMap<CreateorUpdateConversationDto, Conversation>()
              .ForMember(x => x.Messages, opt => opt.Ignore());
            CreateMap<Conversation, ConversationDto>();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Friendships;

namespace NetCoreAPI.Infra.Mappers
{
    public class FriendShipProfile : Profile
    {
        public FriendShipProfile()
        {
            CreateMap<FriendshipDto, Friendship>();
            CreateMap<Friendship, FriendshipDto>()
                .AfterMap((normal, dto) => Teste());
        }

        public void Teste()
        {

        }
    }
}

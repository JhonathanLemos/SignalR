using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.Solicitations;

namespace NetCoreAPI.Infra.Mappers
{
    public class SolicitationProfile : Profile
    {
        public SolicitationProfile()
        {
            CreateMap<SolicitationDto, Solicitation>();
            CreateMap<Solicitation, SolicitationDto>()
                .ForMember(x => x.SecondUserName, opt => opt.MapFrom(x => x.User.UserName));
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Models;
using SignalR.Application.EmailCodes;

namespace NetCoreAPI.Infra.Mappers
{
    public class EmailCodeProfile : Profile
    {
        public EmailCodeProfile()
        {
            CreateMap<EmailCodeDto, EmailCode>();
            CreateMap<EmailCode, EmailCodeDto>();


        }
    }
}

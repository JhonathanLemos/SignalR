using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Identidade;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using NetCoreAPI.Properties;
using SignalR.Application.EmailCodes;
using SignalR.Application.Properties;
using SignalR.Application.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCoreAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<EmailCode> _emailCode;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IdentidadeService _identidadeService;
        private readonly IMemoryCache _memoryCache;

        public LoginController(IMemoryCache memoryCache, IdentidadeService identidadeService, IRepository<EmailCode> emailCode, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _identidadeService = identidadeService;
            _memoryCache = memoryCache;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailCode = emailCode;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(Login login)
        {

            User user = new User() { UserName = login.UserName, Email = login.Email };
            await CreateUserCode(user);
            var result = await _userManager.CreateAsync(user, login.Password);
            if (result.Succeeded)
            {
                var usereDto = _mapper.Map<UserDto>(user);

                await GenerateCodeToValidateUser(usereDto);
                return Ok(new { UserId = user.Id, RegistrationResult = result });
            }
            else
            {
                return BadRequest(result.TranslateErrors());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
                return Results.BadRequest();

            if (!user.EmailConfirmed)
                return Results.BadRequest("EmailNotValidated");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded)
                return Results.BadRequest();



            var key = Encoding.ASCII.GetBytes(_configuration["JwtBearerTokenSettings:SecretKey"]);
            TimeZoneInfo brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime currentTimeInBrazil = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brazilTimeZone);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials =
               new SigningCredentials(
                   new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["JwtBearerTokenSettings:Audience"],
                Issuer = _configuration["JwtBearerTokenSettings:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(300),
                Subject = new ClaimsIdentity(new[]
    {
        new Claim("last_activity",currentTimeInBrazil.ToString("o")),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    }),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);



            return Results.Ok(new
            {

                token = tokenHandler.WriteToken(token),
                userId = user.Id,
                userName = user.UserName,
                userCode = user.UserCode
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private async Task GenerateCodeToValidateUser(UserDto user)
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string randomCode = randomNumber.ToString("D6");

            var emailCodeDto = new EmailCodeDto() { Code = randomCode, UserId = user.Id };
            await _emailCode.Add(_mapper.Map<EmailCode>(emailCodeDto));
            await _identidadeService.SendEmailAsync(user, randomCode, Resource.SubjectEmail, Resource.bodyEmailCode);

        }

        private async Task CreateUserCode(User user)
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string randomCode = randomNumber.ToString("D4");
            user.UserCode = user.UserName + "#" + randomCode;

        }

        [AllowAnonymous]
        [HttpPost("GenCode")]
        public async Task<IActionResult> GenCode(Login user)
        {
            var result = await _userManager.FindByEmailAsync(user.Email);
            if (result.EmailConfirmed)
                return Ok();
            else
            {
                await GenerateCodeToValidateUser(_mapper.Map<UserDto>(result));
                return BadRequest(new { UserId = result.Id, RegistrationResult = "ValidateUser" });
            }
        }
    }
}

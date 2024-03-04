using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using SignalR.Application.Friendships;
using SignalR.Application.Servers;
using SignalR.Application.Users;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _repository;
    private readonly IRepository<Server> _serverRepository;
    private readonly IRepository<Solicitation> _solicitationRepository;
    private readonly IHubContext<ChatHub> _hubContext;
    public UserController(IHttpContextAccessor httpContextAccessor,IUserRepository repository, IRepository<Solicitation> solicitationRepository,  IMapper mapper, IRepository<Server> serverRepository,IHubContext<ChatHub> hubContext)
    {
        _repository = repository;
        _mapper = mapper;
        _serverRepository = serverRepository;
        _solicitationRepository = solicitationRepository;
        _httpContextAccessor = httpContextAccessor;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

        if (input.Search != null)
            query = query.Where(x => x.UserName.Contains(input.Search));

        int totalItems = query.Count();

        int skip = input.PageIndex * input.PageSize;

        var items = query.Skip(skip).Take(input.PageSize).ToList();
        var itemsDto = _mapper.Map<List<UserDto>?>(items);
        var result = new
        {
            TotalItems = totalItems,
            Items = itemsDto
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _repository.GetById(id);
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(_mapper.Map<UserDto>(result));
    }

    [HttpGet("GetServersByUserId/{id}")]
    public async Task<IActionResult> GetServersByUserId(string id)
    {
        List<ServerDto> list = new List<ServerDto>();
        var result = await _repository.GetAll().Include(x => x.UserServers).Where(x => x.Id == id).FirstOrDefaultAsync();
        if(result?.UserServers is not null)
        {
        var idsList =  result.UserServers.Select(x => x.ServerId).ToList();
        list = await GetServersByIds(idsList);
        }
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(list);
    }

    [HttpGet("GetFriendsById/{id}")]
    public async Task<IActionResult> GetFriendsById(string id)
    {
        List<FriendshipDto> list = new List<FriendshipDto>();
        var result = await _repository.GetAll().Include(x => x.Friends).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (result.Friends is not null)
        {
            list = _mapper.Map<List<FriendshipDto>>(result.Friends);
        }
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(list);
    }

    [HttpGet("GetUserNameById/{id}")]
    public async Task<IActionResult> GetUserNameById(string id)
    {
        var result = await _repository.GetAll().Where(x => x.Id == id).Select(x => x.UserName).FirstOrDefaultAsync();
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(new { userName = result });
    }

    [HttpPost("SendSolicitation")]
    public async Task<IActionResult> SendSolicitation(UserAndCode userAndCode)
    {
        var user = await _repository.GetAll().Where(x => x.UserCode == userAndCode.UserCode).FirstOrDefaultAsync();
        if (user.Id != userAndCode.UserId)
        {

        await _solicitationRepository.Add(new Solicitation() { UserId = userAndCode.UserId, SecondUserId = user.Id });
        await _hubContext.Clients.User(user.Id.ToString()).SendAsync("newSolicitation");
        }
        return Ok();
    }

    private async Task<List<ServerDto>> GetServersByIds(List<long> serverIds)
    {
        var a = await _serverRepository.GetAll().Where(s => serverIds.Contains(s.Id)).ToListAsync();
        return _mapper.Map<List<ServerDto>>(a);
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        return Ok(await _repository.Add(user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UserDto userDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(userDto, entity);
        return Ok(await _repository.Update(entity));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _repository.GetById(id);
        if (result == null)
            return NotFound("Entidade nao encontrada");

        await _repository.Delete(result);
        return Ok();
    }
}

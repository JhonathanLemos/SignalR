using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using SignalR.Application.Servers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Server> _repository;
    private readonly IRepository<UserServer> _userServerRepository;
    private readonly IRepository<Channel> _channelRepository;
    public ServerController(IRepository<Server> repository, IMapper mapper, IRepository<UserServer> userServerRepository, IRepository<Channel> channelRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userServerRepository = userServerRepository;
        _channelRepository = channelRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

        if (input.Search != null)
            query = query.Where(x => x.ServerName.Contains(input.Search));

        int totalItems = query.Count();

        int skip = input.PageIndex * input.PageSize;

        var items = query.Skip(skip).Take(input.PageSize).ToList();
        var itemsDto = _mapper.Map<List<ServerDto>?>(items);
        var result = new
        {
            TotalItems = totalItems,
            Items = itemsDto
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _repository.GetAll().Include(x => x.Channels).ThenInclude(x => x.MessageChannel).Include(x => x.UserServers).Where(x => x.Id == id).ToListAsync();
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(_mapper.Map<List<ServerDto>>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateOrUpdateServerDto serverDto)
    {
        var server = _mapper.Map<Server>(serverDto);
        var result = await _repository.Add(server);
        await _channelRepository.Add(new Channel() { Nome = "Canal de texto", ServerId = result.Id, Tipo = "texto" });
        await _userServerRepository.Add(new UserServer() { ServerId = result.Id, UserId = serverDto.AdminId });
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateOrUpdateServerDto serverDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(serverDto, entity);
        return Ok(await _repository.Update(entity));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.GetById(id);
        if (result == null)
            return NotFound("Entidade nao encontrada");

        await _repository.Delete(result);
        return Ok();
    }
}

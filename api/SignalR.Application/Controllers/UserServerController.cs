using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using SignalR.Application.Servers;
using SignalR.Application.UserServers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserServerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<UserServer> _repository;
    public UserServerController(IRepository<UserServer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

       
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
        var result = await _repository.GetById(id);
        if (result == null)
            return NotFound("Entidade não encontrada");

        return Ok(_mapper.Map<ServerDto>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserServerDto userServerDto)
    {
        var userServer = _mapper.Map<UserServer>(userServerDto);
        return Ok(await _repository.Add(userServer));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ServerDto userServerDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(userServerDto, entity);
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

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using SignalR.Application.MessageChanels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageChannelController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<MessageChannel> _repository;
    public MessageChannelController(IRepository<MessageChannel> repository, IMapper mapper)
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
        var itemsDto = _mapper.Map<List<MessageChannel>?>(items);
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

        return Ok(_mapper.Map<MessageChannelDto>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MessageChannelDto messageDto)
    {
        var message = _mapper.Map<MessageChannel>(messageDto);
        return Ok(await _repository.Add(message));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] MessageChannelDto messageDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(messageDto, entity);
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

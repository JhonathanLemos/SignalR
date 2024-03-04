using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using SignalR.Application.Friendships;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendShipController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Friendship> _repository;
    private readonly IRepository<Conversation> _conversationRepository;
    public FriendShipController(IRepository<Friendship> repository, IMapper mapper, IRepository<Conversation> conversationRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _conversationRepository = conversationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

        int totalItems = query.Count();

        int skip = input.PageIndex * input.PageSize;

        var items = query.Skip(skip).Take(input.PageSize).ToList();
        var itemsDto = _mapper.Map<List<FriendshipDto>?>(items);
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

        return Ok(_mapper.Map<FriendshipDto>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] FriendshipDto friendshipDto)
    {
        var friendship = _mapper.Map<Friendship>(friendshipDto);
        await _repository.Add(friendship);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] FriendshipDto friendshipDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(friendshipDto, entity);
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


    [HttpGet("GetAllFrienshipByUserId/{id}")]
    public async Task<IActionResult> GetAllFrienshipByUserId(string id)
    {
        var listNew = new List<Friendship>();
        var user = await _conversationRepository.GetAll().Where(x => x.FirstUserId == id || x.SecondUserId == id).FirstOrDefaultAsync();
        var newUser = new List<UserAndName>() {  };
        var list = await _repository.GetAll().Include(x => x.FirstUser).Include(x => x.SecondUser).Where(x => x.FirstUserId == id || x.SecondUserId == id).ToListAsync();
        foreach (var item in list)
        {
            var result = await _conversationRepository.GetAll().Where(x => x.FirstUserId == item.FirstUserId && x.SecondUserId == item.SecondUserId || x.SecondUserId == item.FirstUserId && x.FirstUserId == item.SecondUserId).FirstOrDefaultAsync();
            if (result is null)
                listNew.Add(item);
        }
        foreach (var item in listNew)
        {
            if (item.FirstUserId == id)
            {
                newUser.Add(new UserAndName() { UserId = item.SecondUserId, UserName = item.SecondUser.UserName });
            }
            else
                newUser.Add(new UserAndName() { UserId = item.FirstUserId, UserName = item.FirstUser.UserName });
        }
        return Ok(newUser);
    }
}

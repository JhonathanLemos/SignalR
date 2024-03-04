using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using SignalR.Application.Solicitations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolicitationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Solicitation> _repository;
    private readonly IRepository<Friendship> _friendShipRepository;
    private readonly IHubContext<ChatHub> _hubContext;
    public SolicitationController(IRepository<Solicitation> repository, IMapper mapper, IRepository<Friendship> friendShipRepository, IHubContext<ChatHub> hubContext)
    {
        _repository = repository;
        _mapper = mapper;
        _friendShipRepository = friendShipRepository;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

        int totalItems = query.Count();

        int skip = input.PageIndex * input.PageSize;

        var items = query.Skip(skip).Take(input.PageSize).ToList();
        var itemsDto = _mapper.Map<List<SolicitationDto>?>(items);
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

        return Ok(_mapper.Map<SolicitationDto>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SolicitationDto solicitationDto)
    {
        var solicitation = _mapper.Map<Solicitation>(solicitationDto);
        await _repository.Add(solicitation);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] SolicitationDto solicitationDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(solicitationDto, entity);
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

    [HttpGet("GetSolicitationsByUserId/{id}")]
    public async Task<IActionResult> GetSolicitationsByUserId(string id)
    {
        var list = await _repository.GetAll().Include(x => x.User).Where(x => x.SecondUserId == id).ToListAsync();
        var listDto = _mapper.Map<List<SolicitationDto>>(list);
        return Ok(listDto);
    }

    [HttpPost("AcceptSolicitation")]
    public async Task<IActionResult> AcceptSolicitation(SolicitationDto solicitation)
    {
        var user = await _repository.GetAll().Where(x => x.UserId == solicitation.UserId && x.SecondUserId == solicitation.SecondUserId).FirstOrDefaultAsync();
        if (user is not null)
        await _repository.Delete(user);
        await _friendShipRepository.Add(new Friendship() { FirstUserId = solicitation.UserId, SecondUserId = solicitation.SecondUserId });
        await _hubContext.Clients.User(solicitation.UserId).SendAsync("attConversation");
        return Ok();
    }

    [HttpPost("DenySolicitation")]
    public async Task<IActionResult> DenySolicitation(SolicitationDto solicitation)
    {
        var result = await _repository.GetAll().Where(x => x.UserId == solicitation.UserId && x.SecondUserId == solicitation.SecondUserId).FirstOrDefaultAsync();
        if (result is not null)
        await _repository.Delete(result);
        return Ok();
    }
}

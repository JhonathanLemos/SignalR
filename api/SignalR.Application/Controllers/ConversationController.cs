using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using SignalR.Application.Conversation;
using SignalR.Application.Conversations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreAPI.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConversationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Conversation> _repository;
    private readonly IHubContext<ChatHub> _hubContext;
    public ConversationController(IRepository<Conversation> repository, IMapper mapper, IHubContext<ChatHub> hubContext)
    {
        _repository = repository;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAll input)
    {
        var query = _repository.GetAll();

        int totalItems = query.Count();

        int skip = input.PageIndex * input.PageSize;

        var items = query.Skip(skip).Take(input.PageSize).ToList();
        var itemsDto = _mapper.Map<List<Conversation>?>(items);
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

        return Ok(_mapper.Map<ConversationDto>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateorUpdateConversationDto conversationDto)
    {
        var conversation = _mapper.Map<Conversation>(conversationDto);
        await _repository.Add(conversation);
        await _hubContext.Clients.User(conversationDto.FirstUserId).SendAsync("attConversation");
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateorUpdateConversationDto conversationDto)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return NotFound("Nenhum entidade encontrada!");

        _mapper.Map(conversationDto, entity);
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

    [HttpGet("GetAllConversationsByUserId/{id}")]
    public async Task<IActionResult> GetAllConversationsByUserId(string id)
    {
        var newUser = new List<UserAndNameAndLastMessage>() { };
        var list = await _repository.GetAll().Include(x => x.FirstUser).Include(x => x.SecondUser).Include(x => x.Messages).Where(x => x.FirstUserId == id || x.SecondUserId == id).ToListAsync();
        foreach (var item in list)
        {
            if (item.FirstUserId == id)
            {
                newUser.Add(new UserAndNameAndLastMessage() {
                    UserId = item.SecondUserId,
                    UserName = item.SecondUser.UserName,
                    LastMessage = item.Messages.Count > 0 ? item?.Messages?.OrderByDescending(x => x.DataEnvio).FirstOrDefault().Conteudo : "",
                    Hour = item.Messages.Count > 0 ? item?.Messages?.OrderByDescending(x => x.DataEnvio).FirstOrDefault().DataEnvio : null,
                    FirstUserId = item.Messages.Count > 0 ? item.Messages.OrderByDescending(x => x.DataEnvio).Select(x => x.SenderId).FirstOrDefault() : "",
                    Show = true,
                });
            }
            else
                newUser.Add(new UserAndNameAndLastMessage()
                {
                    UserId = item.FirstUserId,
                    UserName = item.FirstUser.UserName,
                    LastMessage = item.Messages.Count > 0 ? item.Messages.OrderByDescending(x => x.DataEnvio).FirstOrDefault().Conteudo : "",
                    Hour = item.Messages.Count > 0 ? item?.Messages?.OrderByDescending(x => x.DataEnvio).FirstOrDefault().DataEnvio : null,
                    FirstUserId = item.Messages.Count > 0 ? item.Messages.OrderByDescending(x => x.DataEnvio).Select(x => x.SenderId).FirstOrDefault() : "",
                    Show = false,

                });
        }


        return Ok(newUser);
    }
    [HttpGet("GetAllMessagesFromConversationByUserId/{firstUserId}/{secondUserId}/{pagina}/{tamanhoPagina}")]
    public async Task<IActionResult> GetAllMessagesFromConversationByUserId(string firstUserId, string secondUserId, int pagina = 2, int tamanhoPagina = 20)
    {
        var list = await _repository.GetAll().Include(x => x.FirstUser).Include(x => x.SecondUser).Include(x => x.Messages).Where(x => x.FirstUserId == firstUserId && x.SecondUserId == secondUserId || x.FirstUserId == secondUserId && x.SecondUserId == firstUserId).FirstOrDefaultAsync();
        var listDto = _mapper.Map<ConversationDto>(list);
        var lists = listDto.Messages.OrderByDescending(x => x.DataEnvio).Skip((pagina - 1) * tamanhoPagina).ToList();
        var a = lists.OrderBy(x => x.DataEnvio).TakeLast(tamanhoPagina).ToList();

        return Ok(a);
    }
}

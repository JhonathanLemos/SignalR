using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Application.Dtos;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;

namespace NetCoreAPI.Application.Controllers
{
    
    public class ChatHub : Hub
    {
    private readonly IRepository<Conversation> _repository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<Solicitation> _solicitationRepository;
    private readonly UserManager<User> _userManager;

        public ChatHub(UserManager<User> userManager,IRepository<Conversation> repository, IRepository<Message> messageRepository, IRepository<Solicitation> solicitationRepository)
        {
            _repository = repository;
            _messageRepository = messageRepository;
            _solicitationRepository = solicitationRepository;
            _userManager = userManager;
        }

        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }

        [Authorize]
        public async Task NewMessage(string sender, string secondUser, string message)
        {
            var conversation = await _repository.GetAll().Where(x => x.FirstUserId == sender && x.SecondUserId == secondUser || x.FirstUserId == secondUser && x.SecondUserId == sender).FirstOrDefaultAsync();
            var messages = new Message() { SenderId = sender, ConversationId = conversation.Id, Conteudo = message, DataEnvio = DateTime.Now };
            await _messageRepository.Add(messages);
            await Clients.User(secondUser).SendAsync("newMessage");
            await Clients.Caller.SendAsync("newMessage");
        }

        public async Task NewSolicitation(UserAndCode userAndCode)
        {
            await Clients.User(userAndCode.UserId).SendAsync("newSolicitation");
        }


        public void LoadMessages(string userName, string message)
        {
            Clients.All.SendAsync("loadMessages", userName, message);
        }

        public void GetMessagesBetweenUsers(string firstUserId, string secondUserId)
        {
            Clients.All.SendAsync("getMessagesBetweenUsers", firstUserId, secondUserId);
           // var list = await _repository.GetAll().Where(x => x.FirstUserId == firstUserId && x.SecondUserId == secondUserId || x.SecondUserId == firstUserId && x.FirstUserId == secondUserId).FirstOrDefaultAsync();
           //var messages = list.Messages.OrderBy(x => x.DataEnvio).ToList();
           //return messages;
        }
    }

}

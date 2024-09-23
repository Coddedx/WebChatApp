using ChapAppSignalR.Hubs;
using ChapAppSignalR.Models;
using ChapAppSignalR.Services;
using ChapAppSignalR.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChapAppSignalR.Controllers
{
    public class MessageController : Controller
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly ChatAppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(ChatAppDbContext context, RabbitMqService rabbitMqService, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _rabbitMqService = rabbitMqService;
            _hubContext = hubContext;
        }

        // Eski mesajları göstermek için
        public async Task<IActionResult> Index(string id) //receiver ıd
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = _userManager.GetUserId(User);

            //await
            var messages =  _context.Messages
                .Include(m => m.Sender)
                .Where(m => (m.SenderId == senderId && m.ReceiverId == id) || (m.SenderId == id && m.ReceiverId == senderId))
                .OrderBy(m => m.SenAt)
                .ToList();

            var receiver = await _context.Users.FindAsync(id);
             var messageVM = new MessageViewModel();

            if (messages != null)
            {
                messageVM.ReceiverId = id;
                messageVM.ReceiverName = receiver?.FirstName + "" + receiver?.LastName;
                messageVM.SenderId = senderId;
                messageVM.Messages = messages.Select(m => new MessageDto
                {
                    SenderId = m.SenderId,
                    SenderName = m.Sender != null ? m.Sender.FirstName + " " + m.Sender.LastName : "Bilinmeyen???",
                    Content = m.Content,
                    SentAt = m.SenAt
                }).ToList();            
            }

            return View(messageVM);
        }


        // Yeni mesaj gönderme işlemi
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageViewModel messageVM)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = messageVM.ReceiverId,
                Content = messageVM.MessageContent,
                SenAt = DateTime.UtcNow
            };

            // Mesajı veritabanına ekleyin
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
            
            // SONRADAN (..) İÇİ DEĞİŞTİ -----------------------------------
            // RabbitMQ kullanarak mesajı kuyruklayın
            _rabbitMqService.SendMessage(senderId, messageVM.ReceiverId, newMessage.Content); //newMessage.Content

            // SignalR kullanarak mesajı alıcıya anında iletin
            await _hubContext.Clients.User(messageVM.ReceiverId).SendAsync("ReceiveMessage", senderId, newMessage.Content);

            // Göndericiye de mesajın iletilmesini sağlamak
            await _hubContext.Clients.User(senderId).SendAsync("ReceiveMessage", senderId, newMessage.Content);

            return RedirectToAction("Index", new { id = messageVM.ReceiverId });
        }

    }
}

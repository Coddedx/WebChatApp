using ChapAppSignalR.Models;
using ChapAppSignalR.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChapAppSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatAppDbContext _context;
        private readonly RabbitMqService _rabbitMqService;
        public ChatHub(ChatAppDbContext context, RabbitMqService rabbitMqService)
        {
            _context = context; 
            _rabbitMqService = rabbitMqService;

            // RabbitMQ dan mesajları dinlemeye başla
            _rabbitMqService.ReceiveMessages();
        }
        public async Task SendMessage(string receiverId,string message)
        {
            var senderId = Context.UserIdentifier; //UserIdentifier,ASP.NET Core Identity ile ilişkili bir kavramdır ve HttpContext.User nesnesinden veya SignalR'daki Hub sınıfı içindeki Context.UserIdentifier özelliğinden elde edilir o yüzden _context değil Context olmalı
            var newMessage = new Message
            {
                Content = message,
                SenderId = senderId,
                ReceiverId = receiverId,
                SenAt = DateTime.UtcNow,
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Mesajın göndericiye ve alıcıya de iletilmesi SignalR ile 
            await Clients.Users(new[] {receiverId, senderId}).SendAsync("ReceiveMessage",senderId, message); // tek kullanıcıya iletmek için User(receiverId).SendAsync("ReceiveMessage", senderId, message)
            
            // Mesajı RabbitMQ'ya gönder
            _rabbitMqService.SendMessage(senderId, receiverId, message);

            //_rabbitMqService.SendMessage($"{senderId} says: {message} to {receiverId}"); //mesajların kuyrukta bekletilmesini sağlar. RabbitMqService ile mesajlar kuyrukta tutulabilir ve belirli olaylar olduğunda işlenebilir.
        }
    }
}

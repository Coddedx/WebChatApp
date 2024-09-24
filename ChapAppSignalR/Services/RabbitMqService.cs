using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using ChapAppSignalR.Models;
using ChapAppSignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChapAppSignalR.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IHubContext<ChatHub> _hubContext;

        public RabbitMqService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.Uri = new Uri("amqps://fbocbdpm:SSsztPjxnmcLaJPQcs8sn52Q5A09di54@woodpecker.rmq.cloudamqp.com/fbocbdpm");
           
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "chatQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // SONRADAN -------------------------
           // ReceiveMessages();  //metodu arka planda sürekli dinleme yapacak ve anlık bildirimler alınabilmesi sağlanılacak
        }

        // SONRADAN (..) İÇİ DEĞİŞTİ ----------------------------
        public void SendMessage(string senderId, string receiverId, string messageContent)//string message
        {
            // SONRADAN ----------------------------
            // Mesajı formatla: "SenderId:ReceiverId:Content"
            var formattedMessage = $"{senderId}:{receiverId}:{messageContent}";
            var body = Encoding.UTF8.GetBytes(formattedMessage);


            //var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "chatQueue", basicProperties: null, body: body);
        }

        public void ReceiveMessages()
        {
            // SONRADAN--------------------------------
            //performansı artırmak için Her seferinde sadece bir mesaj alınır ve işlenir
            _channel.BasicQos(prefetchSize: 0, prefetchCount:1, global:false);

            var consumer = new EventingBasicConsumer(_channel);

            // ASYNC SONRADAN --------------------  await _hubContext.Clients.User İÇİN 
            // Received olayına dinleyici ekleme
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Mesajı işle

                // SONRADAN---------------------
                // Mesajı alıcıya SignalR üzerinden gönder
                var parts = message.Split(':'); // Mesajı ayrıştır (Örn: "senderId:receiverId:content")
                if (parts.Length >= 3)  
                {
                    var senderId = parts[0].Trim();
                    var receiverId = parts[1].Trim();
                    var content = parts[2].Trim();

                    await _hubContext.Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, content);

                    Console.WriteLine($"Received message: {message}");
                }
                else
                {
                    Console.WriteLine("Received message is not in the expected format.");
                }

            };

            // Kuyruktan mesajları tüketmeye başla
            _channel.BasicConsume(queue: "chatQueue", autoAck: true, consumer: consumer);
        }


        //public void ReceiveMessages()
        //{
        //    _channel.BasicConsume(queue: "chatQueue", autoAck: true, consumer: new EventingBasicConsumer(_channel)
        //    {
        //        Received = (model, ea) =>
        //        {
        //            var body = ea.Body.ToArray();
        //            var message = Encoding.UTF8.GetString(body);
        //            // Handle the received message
        //        }
        //    });
        //}

    }
}

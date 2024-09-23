
namespace ChapAppSignalR.Services
{
    // SONRADAN BU SINIF OLUŞTURULDU !!!!!!!!!!!!!!!!!!!!
    public class RabbitMqListener : BackgroundService
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly ILogger<RabbitMqListener> _logger;

        public RabbitMqListener(RabbitMqService rabbitMqService, ILogger<RabbitMqListener> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Listener is starting.");

            // RabbitMqService'ten mesaj alımını başlat
            _rabbitMqService.ReceiveMessages();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // 1 saniye bekle
            }

            _logger.LogInformation("RabbitMQ Listener is stopping.");
        }
    }
}

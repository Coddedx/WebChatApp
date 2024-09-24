
namespace ChapAppSignalR.Services
{
    // SONRADAN BU SINIF OLUŞTURULDU !!!!!!!!!!!!!!!!!!!!
    public class RabbitMqListener : BackgroundService //ana thread i bloke etmeden anlık bildirimler alab. için background service kullandım
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

            // RabbitMqService'ten mesaj alımını başlat asenkron çağırmalıyız ki ana thread bloke olmasın
            await Task.Run( () =>  _rabbitMqService.ReceiveMessages(), stoppingToken);


            //SONRADAN--------------------------------
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(1000, stoppingToken); // 1 saniye bekle
            //}

            //SONRADAN---------------------------------
            //Eğer uygulama kapanırsa veya arka plan hizmeti durdurulursa CancellationToken kullanarak bu işlemi güvenli bir şekilde sonlandırmış olursun.
            stoppingToken.Register(() => _logger.LogInformation("RabbitMQ Listener is stopping.")); 

           // _logger.LogInformation("RabbitMQ Listener is stopping.");
        }
    }
}

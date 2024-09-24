
namespace ChapAppSignalR.Services
{
    public class RabbitMqListener : BackgroundService //ana thread i bloke etmeden anlık bildirimler alab. için background service(arka planda sürekli olarak çalışan bir hizmet sağlar) kullandım
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly ILogger<RabbitMqListener> _logger;
        //Logging, uygulamanızın çalışması sırasında meydana gelen olayları, hata mesajlarını ve diğer önemli bilgileri kaydetme işlemidir. Bu kayıtlar, uygulamanın performansını izlemeye, sorunları teşhis etmeye ve uygulamanın nasıl çalıştığını anlamaya yardımcı olur.

        public RabbitMqListener(RabbitMqService rabbitMqService, ILogger<RabbitMqListener> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Listener is starting.");

            // RabbitMqService'ten mesaj alımını başlat asenkron çağırmalıyız ki ana thread bloke olmasın
            await Task.Run( () =>  _rabbitMqService.ReceiveMessages(), stoppingToken); //stoppingToken->Arka plan görevinin iptal edilmesi gerektiğinde, bu token kullanılarak işlem iptal edilir.

            //Eğer uygulama kapanırsa veya arka plan hizmeti durdurulursa CancellationToken(stoppingToken tetiklenir) kullanarak bu işlemi güvenli bir şekilde sonlandırmış olursun.log kaydı oluşturulur
            stoppingToken.Register(() => _logger.LogInformation("RabbitMQ Listener is stopping.")); 

           // _logger.LogInformation("RabbitMQ Listener is stopping.");
        }
    }
}

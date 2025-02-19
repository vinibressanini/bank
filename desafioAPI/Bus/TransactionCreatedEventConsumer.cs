using desafioAPI.Events;
using MassTransit;

namespace desafioAPI.Bus
{
    public class TransactionCreatedEventConsumer : IConsumer<TransactionCreatedEvent>
    {

        private readonly ILogger<TransactionCreatedEventConsumer> _logger;
        private readonly HttpClient _httpClient;

        public TransactionCreatedEventConsumer(ILogger<TransactionCreatedEventConsumer> logger,HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }   

        public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Starting notificantion sending for transaction id:{Id}", message.Transaction.Id);

            var random = new Random();
            var option = random.Next(1, 3);
            HttpResponseMessage response;

            if (option == 1)
            {
                response = await _httpClient.GetAsync("https://run.mocky.io/v3/9d2e64b2-237b-40f1-8728-57a90e622765");
                _logger.LogError("Failed while sending notification for transaction id:{Id}",message.Transaction.Id);
            }
            else
            {
                response = await _httpClient.GetAsync("https://run.mocky.io/v3/82939b29-8f80-40e8-b333-583aed5ce968");
                _logger.LogInformation("Notification id:{Id} successfully sent",message.Transaction.Id);
            }
        }
    }
}

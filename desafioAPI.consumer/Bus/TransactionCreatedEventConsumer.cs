using MassTransit;
using desafioAPI.Events;

namespace desafioAPI.consumer.Bus
{
    public class TransactionCreatedEventConsumer(ILogger<TransactionCreatedEventConsumer> _logger, HttpClient _httpClient) : IConsumer<TransactionCreatedEvent>
    {
        public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {

            var message = context.Message;
            _logger.LogInformation("\n[{Date}] Starting notificantion sending for transaction id:{Id}", DateTime.Now.TimeOfDay, message.Transaction.Id);

            var random = new Random();
            var option = random.Next(1, 3);
            HttpResponseMessage response;


            response = await _httpClient.GetAsync("https://run.mocky.io/v3/9d2e64b2-237b-40f1-8728-57a90e622765");
            _logger.LogError("[{Date}] Failed while sending notification for transaction id:{Id}\n", DateTime.Now.TimeOfDay, message.Transaction.Id);
            throw new Exception($"Failed while sending notification for transaction id:{message.Transaction.Id}");


        }
    }
}

using MassTransit;

namespace desafioAPI.Bus
{
    public class Bus<T> : IBus<T> where T : class
    {
        private readonly IBus _bus;
        private readonly ILogger<Bus<T>> _logger;

        public Bus(IBus bus, ILogger<Bus<T>> logger)
        {
            _bus = bus;
            _logger  = logger;
        }

        public async Task Publish(T message)
        {
            _logger.LogInformation("Publising message {message}",message.ToString());
            await _bus.Publish(message);
        }
    }
}

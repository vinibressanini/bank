using desafioAPI.Models;

namespace desafioAPI.Events
{
    public class TransactionCreatedEvent
    {
        public Guid Id { get; set; }
        public Transaction Transaction { get; set; }
    }
}

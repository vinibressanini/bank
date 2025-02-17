using desafioAPI.Models;

namespace desafioAPI.Exceptions
{
    public class AuthorizationException : Exception
    {

        public Transaction transaction { get; set; }
        public AuthorizationException(string message, Transaction transaction) : base(message)
        {
            this.transaction = transaction;
        }
    }
}

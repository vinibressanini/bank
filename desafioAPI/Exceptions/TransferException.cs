namespace desafioAPI.Exceptions
{
    public class TransferException : Exception
    {
        public int From { get; set; }
        public int To { get; set; }
        public decimal Amount { get; set; }
        public TransferException(string message) : base(message) { }

        public TransferException(string message, Exception inner) : base(message, inner) { }

        public TransferException(string message, Exception? inner, int from, int to, decimal amount) : base(message, inner)
        {
            From = from;
            To = to;
            Amount = amount;
        }

    }
}

namespace desafioAPI.Models
{
    public class Transaction : BaseModel
    {
        public decimal TransactionTotal { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId {  get; set; }
        public DateTime TransactionDate { get; set; }
    }
}

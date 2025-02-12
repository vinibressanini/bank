using System.Text.Json.Serialization;

namespace desafioAPI.Models
{
    public class Wallet : BaseModel
    {
        private decimal _balance;
        public decimal Balance 
        {
            get 
            {
                return _balance;    
            } 
            set 
            {
                if (value < 0) throw new ArgumentException("The account balance can't be negaitve");
                _balance = value;
            } 
        }
        public int UserId { get; set; } = 0;
        [JsonIgnore]
       
        public User? User { get; set; }
        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


    }

}

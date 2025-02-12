using desafioAPI.Models;


namespace desafioAPI.DTO
{

    public class WalletAddBalanceDTO
    {

        public Wallet Wallet { get; set; }
        public decimal Amount { get; set; }

    }
}

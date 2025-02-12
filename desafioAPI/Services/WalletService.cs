using desafioAPI.DTO;
using desafioAPI.Models;
using desafioAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace desafioAPI.Services
{
    public class WalletService
    {

        private readonly WalletRepository _repo;

        public WalletService(WalletRepository repo) => _repo = repo;


        public async Task CreateWallet(int userId)
        {

            Wallet wallet = new()
            {
                UserId = userId
            };

            try
            {
                await _repo.Create(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<decimal> GetWalletBalance(int walletId)
        {
            try
            {

                var wallet = await _repo.GetById(walletId);

                if (wallet == null) throw new ArgumentException($"No wallet found for the given ID {walletId}");

                return wallet.Balance;


            } catch
            {
                throw;
            }
        }

        public async Task AddBalance(WalletAddBalanceDTO dto)
        {

            try
            {

                dto.Wallet.Balance += dto.Amount;

                await _repo.UpdateBalance(dto.Wallet);


            }
            catch
            {
                throw;
            }


        }


    }
}

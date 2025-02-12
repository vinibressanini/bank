using desafioAPI.DTO;
using desafioAPI.Exceptions;
using desafioAPI.Models;
using desafioAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace desafioAPI.Services
{
    public class TransactionService
    {

        private readonly TransactionRepository _repo;
        private readonly WalletRepository _walletRepo;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(TransactionRepository repo, WalletRepository userRepo, ILogger<TransactionService> logger)
        {
            _repo = repo;
            _walletRepo = userRepo;
            _logger = logger;
        }

        public async Task MakeTransaction(TransactionPostRequestBody transactionDTO)
        {
            if (transactionDTO.senderId == transactionDTO.receiverId) throw new TransferException("You can't transfer money to your own account", null, transactionDTO.senderId, transactionDTO.receiverId, transactionDTO.TransactionTotal);


            Transaction transaction = new()
            {
                ReceiverId = transactionDTO.receiverId,
                SenderId = transactionDTO.senderId,
                TransactionTotal = transactionDTO.TransactionTotal,
                TransactionDate = DateTime.UtcNow,
            };

            var strategy = _walletRepo.CreateStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                try
                {

                    var dbTransaction = await _walletRepo.BeginTransaction();

                    var firstId = Math.Min(transaction.SenderId, transaction.ReceiverId);
                    var secondId = Math.Max(transaction.SenderId, transaction.ReceiverId);

                    Wallet? sender;
                    Wallet? receiver;

                    if (firstId == transaction.SenderId)
                    {
                        sender = await _walletRepo.GetByIdLock(firstId);
                        receiver = await _walletRepo.GetByIdLock(secondId);
                    }
                    else
                    {
                        receiver = await _walletRepo.GetByIdLock(firstId);
                        sender = await _walletRepo.GetByIdLock(secondId);
                    }


                    await UpdateBalance(transaction, sender, receiver);
                    await _repo.Create(transaction);

                    Thread.Sleep(5000);

                    await dbTransaction.CommitAsync();

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    throw new TransferException("Error while transfering the money", ex, transaction.SenderId, transaction.ReceiverId, transaction.TransactionTotal);

                }
            });


        }

        public async Task<List<Transaction>> GetAllUserTransactions(int walletId)
        {
            return await _repo.GetAllUserTransactions(walletId);
        }

        private async Task UpdateBalance(Transaction transaction, Wallet sender, Wallet receiver)
        {

            if (sender != null && receiver != null)
            {
                sender.Balance -= transaction.TransactionTotal;
                receiver.Balance += transaction.TransactionTotal;

                await _walletRepo.UpdateBalance(sender);
                await _walletRepo.UpdateBalance(receiver);

            }
            else
            {
                throw new TransferException("The receiver account doesn't exists anymore", null, transaction.SenderId, transaction.ReceiverId, transaction.TransactionTotal);
            }

        }

    }
}

using desafioAPI.Context;
using desafioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace desafioAPI.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>
    {

        private readonly AppDBContext _context;

        public TransactionRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Transaction?> Create(Transaction entity)
        {

            try
            {
                return await base.Create(entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Transaction>> GetAllUserTransactions(int walletId)
        {

            return await _context.Transaction.AsNoTracking().Where(t => t.SenderId == walletId || t.ReceiverId == walletId).ToListAsync();

        }

    }
}

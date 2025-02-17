using desafioAPI.Context;
using desafioAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace desafioAPI.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>
    {

        private readonly AppDBContext _context;

        public WalletRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
        public async override Task<Wallet?> Create(Wallet entity)
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


        public async Task<Wallet?> GetByIdLock(int id)
        {

            return await _context.Wallet.FromSql($"SELECT * FROM \"Wallet\" WHERE \"Id\"={id} FOR UPDATE").FirstOrDefaultAsync();

        }

        public async Task<Wallet?> UpdateBalance(Wallet entity)
        {


            try
            {

                var wallet = await _context.Wallet.FirstOrDefaultAsync(wallet => wallet.Id == entity.Id);

                wallet.Balance = entity.Balance;

                await _context.SaveChangesAsync();

                return wallet;


            }
            catch (Exception ex)
            {
                throw;
            }



        }

    }
}

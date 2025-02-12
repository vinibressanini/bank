using desafioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace desafioAPI.Context
{
    public class AppDBContext : DbContext
    {


        public DbSet<User> User { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Transaction> Transaction { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;UserID=desafio;Password=desafio;Database=desafio;Port=5432").UseNpgsql(optiosn =>
            {
                optiosn.EnableRetryOnFailure(maxRetryCount: 10,maxRetryDelay: TimeSpan.FromSeconds(2),errorCodesToAdd: null);
            });
        }


    }
}

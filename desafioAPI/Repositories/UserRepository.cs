using desafioAPI.Context;
using desafioAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace desafioAPI.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<User?> Create(User entity)
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

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {

            using (var _dbContext = new AppDBContext())
            {
                return await _dbContext.User.FirstOrDefaultAsync(user => user.Email == email && user.Password == password);
            }



        }

        public async Task<bool> IsEmailAlreadyRegistered(string email)
        {

            return await _context.User.AsNoTracking().AnyAsync(user => user.Email == email);

        }
    }
}

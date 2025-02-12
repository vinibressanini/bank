
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using desafioAPI.Context;
using desafioAPI.Models;

namespace desafioAPI.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseModel
    {

        private readonly AppDBContext _context;

        public BaseRepository(AppDBContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> Create(T entity)
        {

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;

        }


        public virtual async Task Delete(int id)
        {


            T entity = await GetById(id);

            _context.Remove<T>(entity);

            await _context.SaveChangesAsync();


        }



        public virtual async Task<T?> GetById(int id)
        {


            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);


        }

        public virtual async Task<T?> Update(T entity)
        {


            T dbEntity = await GetById(entity.Id);

            dbEntity = entity;

            await _context.SaveChangesAsync();

            return dbEntity;




        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {


            return await _context.Database.BeginTransactionAsync();


        }

        public IExecutionStrategy CreateStrategy()
        {

            return _context.Database.CreateExecutionStrategy();

        }
    }
}

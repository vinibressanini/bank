using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace desafioAPI.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);
        Task<T?> Create(T entity);
        Task Delete(int id);
        Task<T?> Update(T entity);
        Task<IDbContextTransaction> BeginTransaction();
        IExecutionStrategy CreateStrategy();
    }
}

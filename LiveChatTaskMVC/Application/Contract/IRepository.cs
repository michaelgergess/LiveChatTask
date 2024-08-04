using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiveChatTask.Application.Contract
{
    public interface IRepository<TEntity, TID>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(TID id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<int> SaveChangesAsync();
    }
}

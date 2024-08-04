using Context;
using LiveChatTask.Application.Contract;
using Microsoft.EntityFrameworkCore;
using Model.Common;

namespace Infrastructure
{
    public class Repository<TEntity, Tid> : IRepository<TEntity, Tid> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _Context;
        private readonly DbSet<TEntity> _Dbset;
        public Repository(ApplicationDbContext Context)
        {
            _Context = Context;
            _Dbset = _Context.Set<TEntity>();
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
           var result =(await _Dbset.AddAsync(entity)).Entity;
            return result;
        }
        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(_Dbset.Remove(entity).Entity);
        }
        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_Dbset.Select(s => s));
        }
        public async Task<TEntity> GetByIdAsync(Tid id)
        {
            TEntity item = await _Dbset.FindAsync(id);

          
            return item;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _Context.SaveChangesAsync();
        }
        public Task<TEntity> UpdateAsync(TEntity entity)
        {
        
            return Task.FromResult(_Dbset.Update(entity).Entity);

        }
    }
}

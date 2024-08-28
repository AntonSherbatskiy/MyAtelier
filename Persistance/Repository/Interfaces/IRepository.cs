using System.Linq.Expressions;
using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IRepository<TKey, TEntity> where TEntity : Identity
{
    Task AddAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetAsync(TKey key);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>?> GetMultipleAsync(Expression<Func<TEntity, bool>> expression);

    Task RemoveAsync(TKey key);
    void Remove(TEntity entity);

    Task UpdateAsync(TKey key, TEntity entity);
    void Update(TEntity entity);
}
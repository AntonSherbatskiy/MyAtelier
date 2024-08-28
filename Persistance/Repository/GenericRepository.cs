using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Repository;

public class GenericRepository<T> : IRepository<int, T> where T : Identity
{
    private AppDbContext _context { get; set; }

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task<T?> GetAsync(int key)
    {
        return _context.Set<T>().FirstOrDefaultAsync(val => val.Id == key);
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>?> GetMultipleAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().Where(expression).ToListAsync();
    } 

    public async Task RemoveAsync(int key)
    {
        var value = await _context.Set<T>().FirstOrDefaultAsync(val => val.Id == key);

        if (value != null)
        {
            _context.Set<T>().Remove(value);
        }
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task UpdateAsync(int key, T entity)
    {
        var value = await _context.Set<T>().FirstOrDefaultAsync(val => val.Id == key);

        if (value != null)
        {
            entity.Id = key;

            _context.Set<T>().Update(value);
        }
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
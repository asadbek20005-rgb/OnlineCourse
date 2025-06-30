using Microsoft.EntityFrameworkCore.Storage;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Infrastructure.Contexts;

namespace OnlineCourse.Infrastructure.Repositories;

public class BaseRepository<TEntity>(OnlineCourseDbContext onlineCourseDbContext) : IBaseRepositroy<TEntity> where TEntity : class
{
    private readonly OnlineCourseDbContext _context = onlineCourseDbContext;
    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Task.FromResult(_context.Set<TEntity>().Remove(entity));
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync<TId>(TId id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Task.FromResult(_context.Set<TEntity>().Update(entity));
    }
}

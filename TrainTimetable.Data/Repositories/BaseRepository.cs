using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIDAsync(int id);

    Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

    Task InsertAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);
}

public class BaseRepository<TEntity>(
    IDbContextFactory<AppDbContext> dbContextFactory) : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIDAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(_ => _.ID == id);
    }

    public async Task<IEnumerable<TEntity>> FilterAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        IQueryable<TEntity> query = dbContext.Set<TEntity>().Where(predicate);

        if (include != null)
            query = include(query);

        return await query.ToListAsync();
    }

    public async Task InsertAsync(TEntity entity)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity) {
        await using var _dbContext = await dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity) {
        await using var _dbContext = await dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}

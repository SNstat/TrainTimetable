using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIDAsync(int id);

    Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity ob);

    Task UpdateAsync(TEntity ob);

    Task RemoveAsync(TEntity ob);
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

    public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity ob) 
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Set<TEntity>().AddAsync(ob);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity ob) {
        await using var _dbContext = await dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<TEntity>().Update(ob);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(TEntity ob) {
        await using var _dbContext = await dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<TEntity>().Remove(ob);
        await _dbContext.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TrainTimetable.Data.Repositories;

public interface IDbSet
{
    int ID { get; set; }
}

public interface IBaseRepository<T> where T : class, IDbSet
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIDAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T ob);
    Task UpdateAsync(T ob);
    Task RemoveAsync(T ob);
}

public class BaseRepository<T> : IBaseRepository<T> where T : class, IDbSet
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    
    public BaseRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIDAsync(int id)
    {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await _dbContext.Set<T>().FirstOrDefaultAsync(_ => _.ID == id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T ob) {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        await _dbContext.Set<T>().AddAsync(ob);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T ob) {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<T>().Update(ob);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(T ob) {
        await using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
        _dbContext.Set<T>().Remove(ob);
        await _dbContext.SaveChangesAsync();
    }
}

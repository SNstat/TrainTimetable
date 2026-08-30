using Microsoft.EntityFrameworkCore;

namespace TrainTimetable.Data.Repositories;

public interface IDbSet
{
    int ID { get; set; }
}

public interface IBaseRepository<T> where T : class, IDbSet
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetByID(int id);
    Task Insert(T ob);
    Task Update(T ob);
    Task Delete(T ob);
}

public class BaseRepository<T> : IBaseRepository<T> where T : class, IDbSet
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    
    public BaseRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetByID(int id)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Set<T>().SingleAsync(_ => _.ID == id);
    }

    public async Task Insert(T ob) {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        await dbContext.Set<T>().AddAsync(ob);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(T ob) {
        using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<T>().Update(ob);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(T ob) {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Set<T>().Remove(ob);
        await dbContext.SaveChangesAsync();
    }
}

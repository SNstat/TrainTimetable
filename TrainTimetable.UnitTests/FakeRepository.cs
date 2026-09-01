using System.Linq.Expressions;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.UnitTests;

internal class FakeRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    private ICollection<TEntity> _entities = new List<TEntity>();

    public Task AddAsync(TEntity entity)
    {
        _entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Task.FromResult(_entities.AsEnumerable());
    }

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(TEntity ob)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity ob)
    {
        throw new NotImplementedException();
    }
}

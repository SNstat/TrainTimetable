using System.Linq.Expressions;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.UnitTests;

internal class FakeBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    private readonly ICollection<TEntity> entities = [];


    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return entities;
    }

    public async Task<TEntity?> GetByIDAsync(int id)
    {
        foreach (var _ in entities)
        {
            if (_.ID == id)
            {
                return _;
            }
        }

        return null;
    }

    public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var compiledPredicate = predicate.Compile();
        return entities.Where(compiledPredicate).ToList();
    }


    public async Task InsertAsync(TEntity entity)
    {
        entities.Add(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        foreach (var _ in entities)
        {
            if (entity.ID == _.ID)
            {
                entities.Remove(_);
                entities.Add(entity);
                break;
            }
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        entities.Remove(entity);
    }
}

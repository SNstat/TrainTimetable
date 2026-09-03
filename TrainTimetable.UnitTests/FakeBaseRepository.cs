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
        return entities.FirstOrDefault(_ => _.ID == id);
    }

    public IEnumerable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        var compiledPredicate = predicate.Compile();

        var query = entities.Where(compiledPredicate).AsQueryable();

        if (include != null)
        {
            query = include(query);
        }

        return query;
    }

    public async Task InsertAsync(TEntity entity)
    {
        entities.Add(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var _ = entities.FirstOrDefault(_ => _.ID == entity.ID);

        if( _ != null)
        {
            entities.Remove(_);
            entities.Add(entity);
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        entities.Remove(entity);
    }
}

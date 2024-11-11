using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Domain.Interfaces;

namespace Miotto.Tasks.Infra.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, IEntity, new()
{
    private static readonly IEnumerable<EntityState> EntityMaintainedStatus = [EntityState.Modified, EntityState.Added, EntityState.Deleted];

    private readonly TasksContext _tasksContext;
    protected DbSet<TEntity> Set { get; private set; }

    protected RepositoryBase(TasksContext tasksContext)
    {
        _tasksContext = tasksContext;
        Set = tasksContext.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.CreateAt = DateTime.Now;

        var entry = await Set.AddAsync(entity);

        return entry.Entity;
    }

    public Task DeleteAsync(TEntity entity)
    {
        entity.IsActive = false;
        UpdateAsync(entity);

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Set.ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
            return default;

        var entry = await Set.FindAsync(id);
        return entry;
    }

    public Task UpdateAsync(TEntity entity)
    {
        entity.UpdateAt = DateTime.Now;

        var entry = _tasksContext.Entry(entity);

        if (!EntityMaintainedStatus.Contains(entry.State))
        {
            entry.State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }
}
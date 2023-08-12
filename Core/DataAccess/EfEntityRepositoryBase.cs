using Core.DataAccess.Extensions;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepositoryBase<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public async Task CreateAsync(TEntity entity)
        {
            await using (var context = new TContext())
            {
                var entityToAdd = context.Entry(entity);
                entityToAdd.State = EntityState.Added;
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await using (var context = new TContext())
            {
                var entityToDelete = context.Entry(entity);
                entityToDelete.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            await using (var context = new TContext())
            {
                return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
        }
        // GetAllAsync for check if role exist
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            await using (var context = new TContext())
            {
                if (!context.Set<TEntity>().Any())
                    return Enumerable.Empty<TEntity>();

                return filter == null
                    ? await context.Set<TEntity>().AsNoTrackingWithIdentityResolution().ToListAsync()
                    : await context.Set<TEntity>().Where(filter).AsNoTrackingWithIdentityResolution().ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, int page = 0, int size = 25)
        {
            await using (var context = new TContext())
            {
                if (!context.Set<TEntity>().Any())
                    return Enumerable.Empty<TEntity>();

                return filter == null
                    ? await context.Set<TEntity>().ToPaginateAsync(page, size)
                    : await context.Set<TEntity>().Where(filter).ToPaginateAsync(page, size);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await using (var context = new TContext())
            {
                var entityToUpdate = context.Entry(entity);
                entityToUpdate.State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}

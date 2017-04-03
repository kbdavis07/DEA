
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DEA.DAL.EF;

namespace DEA.DAL.Repository
{
    public static class BaseRepository<TEntity> where TEntity : class
    {

        public static async Task InsertAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                db.Set<TEntity>().Add(entity);
                db.Entry(entity).State = EntityState.Added;
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                db.Set<TEntity>().Add(entity);
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                db.Set<TEntity>().Remove(entity);
                db.Entry(entity).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
        }

        public static IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            using (var db = new DEAContext())
            {
                return db.Set<TEntity>().Where(predicate);
            }
        }

        public static IQueryable<TEntity> GetAll()
        {
            using (var db = new DEAContext())
            {
                return db.Set<TEntity>();
            }
        }

    }
}
using DEA.Database.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DEA.Database.Repository
{
    public static class BaseRepository<TEntity> where TEntity : class
    {

        public static async Task InsertAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                await db.Set<TEntity>().AddAsync(entity);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                db.Set<TEntity>().Update(entity);
                await db.SaveChangesAsync();
            }
        }

        public static async Task DeleteAsync(TEntity entity)
        {
            using (var db = new DEAContext())
            {
                db.Set<TEntity>().Remove(entity);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyetaV1.Interfaces;

namespace ProyetaV1.Repositories
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
       where TEntity : class
       where TContext : DbContext

    {
        private readonly TContext _dbContext;


        protected BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;

        }

        public List<TEntity> GetAllEntities()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public TEntity Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _dbContext.Attach(entity); //traquea una entidad - toman la entidad
            _dbContext.Entry(entity).State = EntityState.Modified; //traquea los cambios
            _dbContext.SaveChanges();
            return entity;
        }

        public TEntity Delete(int id)
        {
            TEntity entity = _dbContext.Find<TEntity>(id);
            _dbContext.Remove(entity);
            return entity;

        }

    }
}

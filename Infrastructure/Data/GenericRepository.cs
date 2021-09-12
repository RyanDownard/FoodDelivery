using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (asNoTracking)
            {
                return queryable.Where(predicate).AsNoTracking().FirstOrDefault();
            }
            else
            {
                return queryable.Where(predicate).FirstOrDefault();
            }
        }

        public async virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (asNoTracking)
            {
                return await queryable.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
            }
            else
            {
                return await queryable .Where(predicate).FirstOrDefaultAsync();
            }
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().ToList().AsEnumerable();
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if(includes != null)
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if(orderBy != null)
            {
                queryable = queryable.OrderBy(orderBy);
            }

            return queryable;
        }

        public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                queryable = queryable.OrderBy(orderBy);
            }

            return await queryable.ToListAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutenticacionService.Persistence.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<TEntity> GetById(int id);

        Task<TEntity> Add(TEntity entity);

        Task<bool> Add(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);

        bool Update(IEnumerable<TEntity> entities);

        Task<bool> Delete(int id);

        bool Delete(TEntity entity);

        bool Delete(IEnumerable<TEntity> entities);


    }
}

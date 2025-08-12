using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using Microsoft.EntityFrameworkCore.Query;

namespace KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract
{
    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        public Task<int> CreateAsync(TEntity entity);   //Oluşan yeni id'yi geri döndürür...
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(int id);
        public Task<TEntity> SearchByIdAsync(int id);
        public Task<List<TEntity>> ListAsync();
        public Task<IEnumerable<TResult>> FilteredSearchAsync<TResult>(
                Expression<Func<TEntity, TResult>> select,
                Expression<Func<TEntity, bool>> where,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBY = null,
                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int take = 0);

    }
}

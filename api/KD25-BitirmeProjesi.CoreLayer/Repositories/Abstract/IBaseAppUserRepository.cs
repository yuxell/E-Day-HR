using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract
{
    public interface IBaseAppUserRepository<TEntity> where TEntity : AppUser
    {
        Task<bool> UpdateAsync(TEntity entity); // UPDATE

        Task<IEnumerable<TEntity>> ListAsync(); // GETALL

        Task<TEntity> FindAsync(int id); // GET
        Task AddAppUserAsync(AppUser appUser); // ADD

        Task<bool> UpdateAppUserRoleAsync(AppUser appUser, List<int> newRoleIds);

        public Task<IEnumerable<TResult>> FilteredSearchAsync<TResult>(
               Expression<Func<TEntity, TResult>> select,
               Expression<Func<TEntity, bool>> where,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBY = null,
               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int takeNumber = 0);
        Task<List<int>> GetUserIdsByRoleNameAsync(string roleName);
    }
}

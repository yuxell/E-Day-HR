using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Abstract
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected AppDbContext _dbContext;
        protected DbSet<TEntity> _tables;

        protected BaseRepository()
        {
            _dbContext = new AppDbContext();
            _tables = _dbContext.Set<TEntity>();
        }

        public async Task<int> CreateAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.RecordStatus = RecordStatus.IsAdded;
            await _tables.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            //return entity.ID; // entity lerde düzeltme(ID ler BaseEntity'den gelecek) yaptıktan sonra yeni oluşan kaydın ID bilgisine ulaşabiliriz
            return 1;

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _tables.FindAsync(id);

            entity.DeletedAt = DateTime.Now;
            entity.RecordStatus = RecordStatus.IsDeleted;

            _tables.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TResult>> FilteredSearchAsync<TResult>(
            Expression<Func<TEntity, TResult>> select, 
            Expression<Func<TEntity, bool>> where, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int take =0)
        {
            IQueryable<TEntity> query = _tables.AsNoTracking();

            if (where != null)
                query = query.Where(where);
            if (include != null)
                query = include(query);

            if (orderBy != null && take != 0)
                return await orderBy(query).Select(select).Take(take).ToListAsync();

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }

        public async Task<List<TEntity>> ListAsync()
        {
            return await _tables.Where(x => x.RecordStatus != RecordStatus.IsDeleted).ToListAsync();
        }

        public async Task<TEntity> SearchByIdAsync(int id)
        {
            return await _tables.FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.RecordStatus = RecordStatus.IsUpdated;
            _tables.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}

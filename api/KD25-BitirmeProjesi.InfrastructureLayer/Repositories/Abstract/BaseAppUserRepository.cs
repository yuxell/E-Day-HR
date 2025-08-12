using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Abstract
{
    public class BaseAppUserRepository<TEntity> : IBaseAppUserRepository<TEntity> where TEntity : AppUser
    {

        private readonly AppDbContext _dbContext;
        protected DbSet<TEntity> _table;

        public BaseAppUserRepository()
        {
            _dbContext = new AppDbContext();
            _table = _dbContext.Set<TEntity>();
        }

        //// BELİRLİ BİR KAYDI ID'YE GÖRE GETİR
        public async Task<TEntity> FindAsync(int id)
        {
            // ID ile veritabanından kaydı bul
            var appUser = await _table.FindAsync(id);

            // Eğer kayıt null ise veya aktif değilse null döndür
            if (appUser == null || appUser.IsActive != true)
                return null;

            // Kayıt varsa ve aktifse onu döndür
            return appUser;
        }


        /// TÜM AKTİF KAYITLARI GETİR
        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            // IsActive == true olan tüm kayıtları listele
            return await _table.Where(x => x.IsActive == true).ToListAsync();
        }


        /// VAR OLAN BİR KAYDI GÜNCELLE
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            // Kaydı güncelle
            _table.Update(entity);

            // Değişiklikleri veritabanına kaydet
            int changes = await _dbContext.SaveChangesAsync();

            // Herhangi bir değişiklik yapıldıysa true döndür
            return changes > 0;
        }


        /// YENİ BİR KULLANICI (APPUSER) EKLE
        public async Task AddAppUserAsync(AppUser appUser)
        {
            // Kullanıcı aktif olarak işaretleniyor
            appUser.IsActive = true;

            // AppUser nesnesini veritabanına ekle
            await _dbContext.Set<AppUser>().AddAsync(appUser);
            await _dbContext.SaveChangesAsync();

            // Kullanıcıya varsayılan olarak RoleId = 3 olan rol atanıyor
            var defaultUserRole = new IdentityUserRole<int>
            {
                UserId = appUser.Id,  // Yeni kullanıcının ID'si
                RoleId = 3            // Varsayılan rol ID'si (örneğin "Kullanıcı")
            };

            // Rol ilişkilendirmesini veritabanına ekle
            await _dbContext.Set<IdentityUserRole<int>>().AddAsync(defaultUserRole);
            await _dbContext.SaveChangesAsync();
        }


        /// KULLANICININ ROLLERİNİ GÜNCELLE
        public async Task<bool> UpdateAppUserRoleAsync(AppUser appUser, List<int> newRoleIds)
        {
            // Önce, kullanıcının mevcut tüm rol ilişkilerini bul
            var existingRoles = await _dbContext.Set<IdentityUserRole<int>>()
                                                 .Where(r => r.UserId == appUser.Id)
                                                 .ToListAsync();

            // Mevcut rol ilişkilerini sil
            _dbContext.Set<IdentityUserRole<int>>().RemoveRange(existingRoles);

            // Yeni rol ID'lerine göre yeni ilişkiler oluştur
            var newRoles = newRoleIds.Select(roleId => new IdentityUserRole<int>
            {
                UserId = appUser.Id, // Kullanıcı ID
                RoleId = roleId      // Yeni atanacak rol ID
            });

            // Yeni rol ilişkilerini veritabanına ekle
            await _dbContext.Set<IdentityUserRole<int>>().AddRangeAsync(newRoles);

            // Değişiklikleri kaydet
            await _dbContext.SaveChangesAsync();

            // Başarılıysa true döndür
            return true;
        }


        /// FİLTRELİ, SIRALI VE INCLUDE DESTEKLİ ARAMA
        public async Task<IEnumerable<TResult>> FilteredSearchAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,                    // Hangi alanların seçileceği
            Expression<Func<TEntity, bool>> where,                        // Filtre şartı
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,  // Sıralama
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int takeNumber = 0)  // Include (ilişkili tablolar)
        {
            // Tabloyu takip etmeden sorguya başla
            IQueryable<TEntity> query = _table.AsNoTracking();

            // Filtre varsa uygula
            if (where != null)
                query = query.Where(where);

            // Include varsa uygula (ilişkili tabloları dahil et)
            if (include != null)
                query = include(query);

            // Sonuçlardan istenen sayıda kayıt al
            if (orderBy != null && takeNumber != 0)
                return await orderBy(query).Select(select).Take(takeNumber).ToListAsync() ?? new List<TResult>();

            // Sıralama varsa uygula ve seçilen alanları döndür
            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync() ?? new List<TResult>();
            else
                // Sıralama yoksa doğrudan seçilen alanları döndür
                return await query.Select(select).ToListAsync() ?? new List<TResult>();
        }


        /// BELİRLİ BİR ROL ADINA SAHİP KULLANICILARIN ID'LERİNİ GETİR
        public async Task<List<int>> GetUserIdsByRoleNameAsync(string roleName)
        {
            // Rol adından ilgili rol kaydını bul
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            // Eğer rol bulunamazsa boş liste döndür
            if (role == null)
                return new List<int>();

            // Bu rol ID'sine sahip kullanıcı ID'lerini getir
            var userIds = await _dbContext.UserRoles
                                           .Where(ur => ur.RoleId == role.Id)
                                           .Select(ur => ur.UserId)
                                           .ToListAsync();

            return userIds;
        }

    }
}

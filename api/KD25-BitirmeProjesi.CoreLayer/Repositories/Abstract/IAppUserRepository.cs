using KD25_BitirmeProjesi.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract
{
    public interface IAppUserRepository : IBaseAppUserRepository<AppUser> 
    {
        Task<AppUser> GetUserSummaryAsync(int userId);
    }
}

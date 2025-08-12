using KD25_BitirmeProjesi.CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.InfrastructureLayer.Configurations
{
    public class AppRole_CFG : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
                new AppRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new AppRole { Id = 2, Name = "CompanyManager", NormalizedName = "COMPANYMANAGER", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new AppRole { Id = 3, Name = "Personel", NormalizedName = "PERSONEL", ConcurrencyStamp = Guid.NewGuid().ToString() }
                );
        }
    }
}

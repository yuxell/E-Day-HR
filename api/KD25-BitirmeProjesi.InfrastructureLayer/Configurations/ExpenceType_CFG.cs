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
    class ExpenceType_CFG : BaseConfiguration<Company>, IEntityTypeConfiguration<ExpenceType>
    {
        public void Configure(EntityTypeBuilder<ExpenceType> builder)
        {
            builder.Property(x => x.ExpenceTypeName)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(20)
                   .IsRequired();
        }
    }
}

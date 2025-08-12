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
    class SalaryAdvance_CFG : BaseConfiguration<Expence>, IEntityTypeConfiguration<SalaryAdvance>
    {
        public void Configure(EntityTypeBuilder<SalaryAdvance> builder)
        {
            builder.Property(x => x.SalaryAdvanceType)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .HasColumnType("money")
                   .IsRequired();

            builder.Property(x => x.Explanation)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(150);

            builder.Property(x => x.RequestDate)
                   .HasColumnType("smalldatetime");

            builder.Property(x => x.ResponseDate)
                   .HasColumnType("smalldatetime");

        }
    }
}

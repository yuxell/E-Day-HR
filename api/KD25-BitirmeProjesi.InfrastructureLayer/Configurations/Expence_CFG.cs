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
    class Expence_CFG : BaseConfiguration<Expence>, IEntityTypeConfiguration<Expence>
    {
        public void Configure(EntityTypeBuilder<Expence> builder)
        {
            builder.Property(x => x.Amount)
                   .HasColumnType("money")
                   .IsRequired();

            builder.Property(x => x.FilePath)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.Explanation)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.RequestDate)
                   .HasColumnType("smalldatetime");

            builder.Property(x => x.ResponseDate)
                   .HasColumnType("smalldatetime");

        }
    }
}

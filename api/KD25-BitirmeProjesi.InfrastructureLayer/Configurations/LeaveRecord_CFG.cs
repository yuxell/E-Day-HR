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
    class LeaveRecord_CFG : BaseConfiguration<Expence>, IEntityTypeConfiguration<LeaveRecord>
    {
        public void Configure(EntityTypeBuilder<LeaveRecord> builder)
        {
            builder.Property(x => x.StartDate)
                   .HasColumnType("smalldatetime");

            builder.Property(x => x.EndDate)
                   .HasColumnType("smalldatetime");

            builder.Property(x => x.TotalDays)
                   .HasColumnType("smallint");

            builder.Property(x => x.RequestDate)
                   .HasColumnType("smalldatetime");

            builder.Property(x => x.ResponseDate)
                   .HasColumnType("smalldatetime");
        }
    }
}

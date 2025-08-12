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
    public class Department_CFG : BaseConfiguration<Department>, IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(x => x.DepartmentName)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(20)
                   .IsRequired();
        }
    }
}

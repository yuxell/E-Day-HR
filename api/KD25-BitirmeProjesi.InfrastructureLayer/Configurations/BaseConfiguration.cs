using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using KD25_BitirmeProjesi.CoreLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KD25_BitirmeProjesi.InfrastructureLayer.Configurations
{
	public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseEntity
    {
		public void Configure(EntityTypeBuilder<TEntity> builder)
		{
            builder.Property(x => x.CreatedAt)
                .HasColumnType("smalldatetime");
                //.HasDefaultValue("GETUTCDATE()"); // EF Core modelden otomatik ekliyor olması lazım

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("smalldatetime");

            builder.Property(x => x.DeletedAt)
                .HasColumnType("smalldatetime");

        }
    }
}

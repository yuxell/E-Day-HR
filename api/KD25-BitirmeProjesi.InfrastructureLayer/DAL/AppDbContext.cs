using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KD25_BitirmeProjesi.CoreLayer;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using System.Reflection;


namespace KD25_BitirmeProjesi.InfrastructureLayer.DAL
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}

		public AppDbContext()
		{
		}

        public DbSet<Department> Departments { get; set; }
        public DbSet<SalaryAdvance> SalaryAdvances { get; set; }
        public DbSet<Expence> Expences { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ExpenceType> ExpenceTypes { get; set; }
        public DbSet<LeaveRecord> LeaveRecords { get; set; }
        public DbSet<LeaveRecordType> LeaveRecordTypes { get; set; }
     



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // İlk Admin ataması yapılıyor
            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> { UserId = 1, RoleId = 1 });
            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> { UserId = 2, RoleId = 2 });
            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> { UserId = 3, RoleId = 3 });
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer("Server=tcp:kolayik.database.windows.net,1433;Initial Catalog=kolayik;Persist Security Info=False;User ID=sqladmin;Password=Group123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
	}
}

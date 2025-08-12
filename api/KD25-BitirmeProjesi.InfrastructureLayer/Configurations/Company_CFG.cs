using KD25_BitirmeProjesi.CoreLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.InfrastructureLayer.Configurations
{
    class Company_CFG : BaseConfiguration<Company>, IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.CompanyName)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Title)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.MersisNum)
                   .HasColumnType("bigint") // çünkü uzun sayı olabilir
                   .IsRequired();

            builder.Property(x => x.TaxNum)
                   .HasColumnType("bigint")
                   .IsRequired();

            builder.Property(x => x.TaxOffice)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(40)
                   .IsRequired();

            builder.Property(x => x.Logo)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(255); // dosya yolu/URL olabilir

            builder.Property(x => x.Phone)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Address)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200);

            builder.Property(x => x.Email)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(40);

            builder.Property(x => x.NumberOfEmployees)
                   .HasColumnType("int")
                   .HasDefaultValue(0);

            builder.Property(x => x.FoundationYear)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(4); // Yıl 4 haneli olacak şekilde

            builder.Property(x => x.ContractStartDate)
                   .HasColumnType("datetime");

            builder.Property(x => x.ContractEndDate)
                   .HasColumnType("datetime");

            builder.Property(x => x.IsActive)
                   .HasColumnType("bit")
                   .HasDefaultValue(true);


            // Initail Companies
            builder.HasData(
                new Company
                {
                    ID = 1,
                    CompanyName = "company1",
                    Title = "Company One Title",
                    MersisNum = 12345678901,
                    TaxNum = 111111,
                    TaxOffice = "Istanbul Vergi Dairesi",
                    Logo = "company1-logo.png",
                    Phone = "+90 212 123 45 67",
                    Address = "Company 1 Address, Istanbul",
                    Email = "info@company1.com",
                    NumberOfEmployees = 50,
                    FoundationYear = "2001",
                    ContractStartDate = new DateTime(2024, 1, 1),
                    ContractEndDate = new DateTime(2025, 1, 1),
                    IsActive = true
                },
                new Company
                {
                    ID = 2,
                    CompanyName = "company2",
                    Title = "Company Two Title",
                    MersisNum = 22345678901,
                    TaxNum = 222222,
                    TaxOffice = "Ankara Vergi Dairesi",
                    Logo = "company2-logo.png",
                    Phone = "+90 312 234 56 78",
                    Address = "Company 2 Address, Ankara",
                    Email = "info@company2.com",
                    NumberOfEmployees = 75,
                    FoundationYear = "2005",
                    ContractStartDate = new DateTime(2024, 3, 15),
                    ContractEndDate = new DateTime(2025, 3, 15),
                    IsActive = true
                },
                new Company
                {
                    ID = 3,
                    CompanyName = "company3",
                    Title = "Company Three Title",
                    MersisNum = 32345678901,
                    TaxNum = 333333,
                    TaxOffice = "Izmir Vergi Dairesi",
                    Logo = "company3-logo.png",
                    Phone = "+90 232 345 67 89",
                    Address = "Company 3 Address, Izmir",
                    Email = "info@company3.com",
                    NumberOfEmployees = 100,
                    FoundationYear = "2010",
                    ContractStartDate = new DateTime(2024, 5, 1),
                    ContractEndDate = new DateTime(2025, 5, 1),
                    IsActive = true

                }
            );
        }
    }
}

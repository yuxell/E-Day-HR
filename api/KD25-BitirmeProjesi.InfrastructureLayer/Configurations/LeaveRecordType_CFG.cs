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
    class LeaveRecordType_CFG : BaseConfiguration<Company>, IEntityTypeConfiguration<LeaveRecordType>
    {
        public void Configure(EntityTypeBuilder<LeaveRecordType> builder)
        {
            builder.Property(x => x.LeaveRecordName)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasData(
                new LeaveRecordType { 
                    ID = 1, 
                    LeaveRecordName = "Hastalık", 
                    CreatedAt = DateTime.Now, 
                    RecordStatus = CoreLayer.Enums.RecordStatus.IsAdded },

                new LeaveRecordType {
                    ID = 2, 
                    LeaveRecordName = "Doğum", 
                    CreatedAt = DateTime.Now, 
                    RecordStatus = CoreLayer.Enums.RecordStatus.IsAdded },

                new LeaveRecordType {
                    ID = 3, 
                    LeaveRecordName = "Vefat", 
                    CreatedAt = DateTime.Now,
                    RecordStatus = CoreLayer.Enums.RecordStatus.IsAdded });
        }
    }
}

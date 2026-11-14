using G_NET69_EF_Assignment_3.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET69_EF_Assignment_3.Configurations
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(n => n.FullName).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(e => e.EmailAddress).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(d => d.DateOfBirth).HasColumnType("date").IsRequired();
        }
    }
}

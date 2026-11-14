using G_NET69_EF_Assignment_3.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET69_EF_Assignment_3.Configurations
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructors>
    {
        public void Configure(EntityTypeBuilder<Instructors> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(n => n.FullName).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(b => b.ShortBio).HasColumnType("nvarchar").HasMaxLength(500);
            builder.Property(e => e.EmailAddress).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(d => d.JoinedDate).HasColumnType("date").IsRequired();
            builder.HasData(
    new Instructors
    {
        Id = 1,
        FullName = "Ahmed Ali",
        EmailAddress = "ahmed@example.com",
        JoinedDate = new DateOnly(2022, 1, 10)
    },
    new Instructors
    {
        Id = 2,
        FullName = "Mohamed Hassan",
        EmailAddress = "mohamed@example.com",
        JoinedDate = new DateOnly(2023, 5, 15)
    },
    new Instructors
    {
        Id = 3,
        FullName = "Sara Adel",
        EmailAddress = "sara@example.com",
        JoinedDate = new DateOnly(2024, 3, 20)
    }
);

        }
    }
}

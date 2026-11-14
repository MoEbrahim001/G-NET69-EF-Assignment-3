using G_NET69_EF_Assignment_3.Classes;
using G_NET69_EF_Assignment_3.Enum;
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
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(n => n.Title).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(l => l.Level).HasColumnType("int").IsRequired();
            builder.Property(i => i.IsPublished).HasColumnType("bit").IsRequired();
            builder.Property(d => d.PublishedDate).HasColumnType("date");
            builder.Property(d => d.Description).HasColumnType("nvarchar").HasMaxLength(500);
            builder.HasOne(i => i.Instructor)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasData(
    new Course
    {
        Id = 1,
        Title = "C# Fundamentals",
        Price = 500,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedDate = new DateOnly(2025, 1, 10),
        InstructorId = 1,
        CategoryId = 1
    },
    new Course
    {
        Id = 2,
        Title = "SQL Server",
        Price = 750,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedDate = new DateOnly(2025, 2, 15),
        InstructorId = 2,
        CategoryId = 2
    },
    new Course
    {
        Id = 3,
        Title = "ASP.NET Core Web API",
        Price = 1200,
        Level = CourseLevel.Intermediate,
        IsPublished = true,
        PublishedDate = new DateOnly(2025, 3, 20),
        InstructorId = 1,
        CategoryId = 4
    },
    new Course
    {
        Id = 4,
        Title = "Angular Fundamentals",
        Price = 800,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedDate = new DateOnly(2025, 4, 10),
        InstructorId = 3,
        CategoryId = 5
    },
    new Course
    {
        Id = 5,
        Title = "Clean Architecture",
        Price = 1500,
        Level = CourseLevel.Advanced,
        IsPublished = false,
        PublishedDate = null,
        InstructorId = 2,
        CategoryId = 4
    },
    new Course
    {
        Id = 6,
        Title = "Advanced JavaScript",
        Price = 1000,
        Level = CourseLevel.Advanced,
        IsPublished = false,
        PublishedDate = null,
        InstructorId = 3,
        CategoryId = 3
    }
);
        }
    }


}

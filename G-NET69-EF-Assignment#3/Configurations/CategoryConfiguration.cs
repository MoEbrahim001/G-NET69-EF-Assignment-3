using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET69_EF_Assignment_3.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasColumnType("nvarchar").HasMaxLength(500);
            builder.HasOne(c => c.ParentCategory)
           .WithMany(c => c.SubCategories)
           .HasForeignKey(c => c.ParentCategoryId)
           .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(c => c.Courses)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasData(
    new Category
    {
        Id = 1,
        Name = "Programming",
        ParentCategoryId = null
    },
    new Category
    {
        Id = 2,
        Name = "Backend",
        ParentCategoryId = 1
    },
    new Category
    {
        Id = 3,
        Name = "Frontend",
        ParentCategoryId = 1
    },
    new Category
    {
        Id = 4,
        Name = "ASP.NET Core",
        ParentCategoryId = 2
    },
    new Category
    {
        Id = 5,
        Name = "Angular",
        ParentCategoryId = 3
    }
);
        }
    }
}

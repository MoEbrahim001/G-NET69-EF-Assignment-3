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
    internal class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.Property(e => e.EnrollmentDate).HasColumnType("date").IsRequired();
            builder.Property(p => p.Percentage).HasColumnType("decimal(5, 2)").IsRequired();
            builder.Property(s => s.Status).HasColumnType("int").IsRequired();
            builder.Property(f=>f.FinalGrade).HasColumnType("decimal(5, 2)").IsRequired(false);
            builder.HasKey(sc => new { sc.StudentId, sc.CourseId });
            builder.HasOne(s => s.Student)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c=>c.Course)
                .WithMany(m=>m.Enrollments)
                .HasForeignKey(c=>c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

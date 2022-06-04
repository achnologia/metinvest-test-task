using Metinvest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Metinvest.Infrastructure.EFCore.Configurations;

public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        builder.HasKey(x => new { x.IdStudent, x.IdCourse });
        
        builder.HasOne(x => x.Student).WithMany(s => s.Courses);
        builder.HasOne(x => x.Course).WithMany(c => c.Students);
    }
}
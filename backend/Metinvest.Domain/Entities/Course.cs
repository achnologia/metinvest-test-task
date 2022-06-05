using Metinvest.Domain.Base;

namespace Metinvest.Domain.Entities;

public class Course : Entity
{
    public string CourseName { get; private set; }
    
    public IEnumerable<StudentCourse> Students { get; private set; }
    
    public Course(string courseName)
    {
        CourseName = courseName;
    }
}
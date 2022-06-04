namespace Metinvest.Domain.Entities;

public class Student : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public string FullName => $"{FirstName} {LastName}";
    
    public IEnumerable<StudentCourse> Courses { get; private set; }
}
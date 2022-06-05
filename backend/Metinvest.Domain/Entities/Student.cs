using Metinvest.Domain.Base;

namespace Metinvest.Domain.Entities;

public class Student : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public string FullName => $"{FirstName} {LastName}";
    
    public IEnumerable<StudentCourse> Courses { get; private set; }
    public IList<Holiday> Holidays { get; private set; }

    public Student(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public bool HasOverlappingCourse(DateTime startDate, DateTime endDate)
    {
        return Courses.Any(x => x.ExistsOnDate(startDate) || x.ExistsOnDate(endDate));
    }
    
    public bool HasOverlappingCourse(int idCourse, DateTime endDate)
    {
        return Courses.Any(x => x.IdCourse != idCourse && x.ExistsOnDate(endDate));
    }
    
    public bool HasOverlappingHoliday(DateTime startDate, DateTime endDate)
    {
        return Holidays.Any(x => x.ExistsOnDate(startDate) || x.ExistsOnDate(endDate));
    }
    
    public StudentCourse GetCourseOnDate(DateTime date)
    {
        return Courses.FirstOrDefault(x => x.ExistsOnDate(date));
    }

    public void AddHoliday(Holiday holiday)
    {
        Holidays.Add(holiday);
    }
}
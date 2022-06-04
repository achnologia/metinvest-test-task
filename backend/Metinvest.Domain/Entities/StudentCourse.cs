using System.ComponentModel.DataAnnotations.Schema;

namespace Metinvest.Domain.Entities;

public class StudentCourse
{
    public int IdStudent { get; private set; }
    public int IdCourse { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    
    [ForeignKey(nameof(IdStudent))]
    public Student Student { get; private set; }
    [ForeignKey(nameof(IdCourse))]
    public Course Course { get; private set; }

    public StudentCourse(int idStudent, int idCourse, DateTime startDate, DateTime endDate)
    {
        IdStudent = idStudent;
        IdCourse = idCourse;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public bool ExistsOnDate(DateTime date)
    {
        return StartDate <= date && EndDate >= date;
    }
}
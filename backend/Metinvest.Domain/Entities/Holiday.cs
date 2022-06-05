using System.ComponentModel.DataAnnotations.Schema;
using Metinvest.Domain.Base;

namespace Metinvest.Domain.Entities;

public class Holiday : Entity
{
    public int IdStudent { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public int TotalWeeks => (int)(EndDate - StartDate).TotalDays / 7 + 1;
    
    [ForeignKey(nameof(IdStudent))]
    public Student Student { get; private set; }

    public Holiday(int idStudent, DateTime startDate, DateTime endDate)
    {
        IdStudent = idStudent;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public bool ExistsOnDate(DateTime date)
    {
        return StartDate <= date && EndDate >= date;
    }
}
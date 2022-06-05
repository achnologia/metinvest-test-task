using Metinvest.Domain.Base;
using Metinvest.Domain.Entities;

namespace Metinvest.Domain.Events;

public class HolidayCreatedEvent : IDomainEvent
{
    public HolidayCreatedEvent(Student student, Holiday holiday)
    {
        Student = student;
        Holiday = holiday;
    }
    
    public Student Student { get; }
    public Holiday Holiday { get; }
}
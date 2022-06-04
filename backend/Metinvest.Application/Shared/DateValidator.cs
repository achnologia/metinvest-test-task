using Metinvest.Domain.Exceptions;

namespace Metinvest.Application.Shared;

public class DateValidator
{
    public void ValidateStartDate(DateTime date)
    {
        if (date.DayOfWeek != DayOfWeek.Monday)
            throw new UserFriendlyException("Start date should be on Monday");
    }
    
    public void ValidateEndDate(DateTime date)
    {
        if (date.DayOfWeek != DayOfWeek.Friday)
            throw new UserFriendlyException("End date should be on Friday");
    }
}
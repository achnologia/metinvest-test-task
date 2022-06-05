using MediatR;
using Metinvest.Application.Courses.Services;
using Metinvest.Domain.Events;

namespace Metinvest.Application.Students.EventHandlers;

public class HolidayCreatedEventHandler : INotificationHandler<HolidayCreatedEvent>
{
    private readonly ICourseService _courseService;

    public HolidayCreatedEventHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task Handle(HolidayCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _courseService.ExtendCourseDuration(notification.Student, notification.Holiday, cancellationToken);
    }
}
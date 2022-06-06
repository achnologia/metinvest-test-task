using Metinvest.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Metinvest.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(UserFriendlyException), HandleUserFriendlyException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }
    
    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
        }
    }
    
    private void HandleUserFriendlyException(ExceptionContext context)
    {
        var exception = (UserFriendlyException)context.Exception;
        
        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}
using System.Reflection;
using MediatR;
using Metinvest.Application.Courses.Services;
using Metinvest.Application.StudentCourses.Services;
using Metinvest.Application.Students.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Metinvest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IStudentCourseService, StudentCourseService>();
        
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
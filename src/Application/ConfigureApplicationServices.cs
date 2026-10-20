using Application.Courses.Services.Abstract;
using Application.Courses.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
    }
}
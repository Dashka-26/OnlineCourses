using Domain.Courses;

namespace Application.Courses.Services.Abstract;

public interface ICourseService
{
    Task<IReadOnlyList<Course>> GetCourses(CancellationToken cancellationToken);
    Task<Course?> GetCourse(Guid id, CancellationToken cancellationToken);
    Task<Course> Add(string title, decimal price, bool isActive, Level level, CancellationToken cancellationToken);
    Task<Course?> Update(Guid id, string title, decimal price, bool isActive, Level level, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}
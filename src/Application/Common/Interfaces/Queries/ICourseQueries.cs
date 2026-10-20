using Domain.Courses;
using LanguageExt; // Додали using

namespace Application.Common.Interfaces.Queries;

public interface ICourseQueries
{
    Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken);
    
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);
}
using Domain.Courses;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);
    Task<Option<Course>> GetByTitle(string title, CancellationToken cancellationToken);
    
    Task<Course> Add(Course course, CancellationToken cancellationToken);
    Task<Course> Update(Course course, CancellationToken cancellationToken);
    Task Delete(Course course, CancellationToken cancellationToken);
}
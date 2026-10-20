using Domain.Courses;

namespace Application.Common.Interfaces;

public interface ICourseRepository
{
    Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken);
    Task<Course?> GetById(Guid id, CancellationToken cancellationToken);
    
    Task<Course?> GetByTitle(string title, CancellationToken cancellationToken);
    
    Task<Course> Add(Course course, CancellationToken cancellationToken);
    Task<Course> Update(Course course, CancellationToken cancellationToken);
    Task Delete(Course course, CancellationToken cancellationToken);
}
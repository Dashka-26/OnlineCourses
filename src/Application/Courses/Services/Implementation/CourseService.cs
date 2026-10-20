using Application.Common.Interfaces;
using Application.Courses.Services.Abstract;
using Domain.Courses;

namespace Application.Courses.Services.Implementation;

public class CourseService(ICourseRepository courseRepository) : ICourseService
{
    public async Task<IReadOnlyList<Course>> GetCourses(CancellationToken cancellationToken)
    {
        return await courseRepository.GetAll(cancellationToken);
    }

    public async Task<Course?> GetCourse(Guid id, CancellationToken cancellationToken)
    {
        return await courseRepository.GetById(id, cancellationToken);
    }

    public async Task<Course> Add(string title, decimal price, bool isActive, Level level, CancellationToken cancellationToken)
    {
        var existingCourse = await courseRepository.GetByTitle(title, cancellationToken);
        if (existingCourse is not null)
        {
            throw new ArgumentException($"Course with title '{title}' already exists");
        }

        var course = Course.New(Guid.NewGuid(), title, price, isActive, level);
        return await courseRepository.Add(course, cancellationToken);
    }

    public async Task<Course?> Update(Guid id, string title, decimal price, bool isActive, Level level, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetById(id, cancellationToken);
        if (course is null)
        {
            return null;
        }

        var courseWithSameTitle = await courseRepository.GetByTitle(title, cancellationToken);
        if (courseWithSameTitle is not null && courseWithSameTitle.Id != id)
        {
            throw new ArgumentException($"Course with title '{title}' already exists");
        }

        course.UpdateDetails(title, price, isActive, level);
        return await courseRepository.Update(course, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetById(id, cancellationToken);
        if (course is null)
        {
            return false;
        }

        await courseRepository.Delete(course, cancellationToken);
        return true;
    }
}
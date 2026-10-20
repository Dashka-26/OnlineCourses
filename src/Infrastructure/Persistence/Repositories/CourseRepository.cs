using Application.Common.Interfaces;
using Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository(ApplicationDbContext context) : ICourseRepository
{
    public async Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Courses.ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Courses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Course?> GetByTitle(string title, CancellationToken cancellationToken)
    {
        return await context.Courses.FirstOrDefaultAsync(x => x.Title == title, cancellationToken);
    }

    public async Task<Course> Add(Course course, CancellationToken cancellationToken)
    {
        await context.Courses.AddAsync(course, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return course;
    }

    public async Task<Course> Update(Course course, CancellationToken cancellationToken)
    {
        context.Courses.Update(course);
        await context.SaveChangesAsync(cancellationToken);
        return course;
    }

    public async Task Delete(Course course, CancellationToken cancellationToken)
    {
        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);
    }
}
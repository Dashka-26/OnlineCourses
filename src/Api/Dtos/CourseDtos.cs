using Domain.Courses;

namespace Api.Dtos;

public record CourseDto(Guid Id, string Title, decimal Price, bool IsActive, string Level, DateTime CreatedAt)
{
    public static CourseDto FromDomainModel(Course course)
        => new(course.Id.Value, course.Title, course.Price, course.IsActive, course.Level.ToString(), course.CreatedAt);
}

public record CreateCourseDto(string Title, decimal Price, bool IsActive, Level Level);

public record UpdateCourseDto(string Title, decimal Price, bool IsActive, Level Level);
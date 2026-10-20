using System.ComponentModel.DataAnnotations;
using Domain.Courses;

namespace Api.Dtos;

public record CourseDto(Guid Id, string Title, decimal Price, bool IsActive, string Level, DateTime CreatedAt)
{
    public static CourseDto FromDomainModel(Course course)
        => new(course.Id, course.Title, course.Price, course.IsActive, course.Level.ToString(), course.CreatedAt);
}

public record CreateCourseDto(
    [Required, MaxLength(255)] string Title,
    [Range(0.01, 1_000_000)] decimal Price,
    bool IsActive,
    [Required] Level Level);

public record UpdateCourseDto(
    [Required, MaxLength(255)] string Title,
    [Range(0.01, 1_000_000)] decimal Price,
    bool IsActive,
    [Required] Level Level);
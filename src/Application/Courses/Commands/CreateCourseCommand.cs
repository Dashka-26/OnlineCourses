using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using LanguageExt;
using MediatR;

namespace Application.Courses.Commands;

public record CreateCourseCommand : IRequest<Either<CourseException, Course>>
{
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required bool IsActive { get; init; }
    public required Level Level { get; init; }
}

public class CreateCourseCommandHandler(ICourseRepository courseRepository) 
    : IRequestHandler<CreateCourseCommand, Either<CourseException, Course>>
{
    public async Task<Either<CourseException, Course>> Handle(
        CreateCourseCommand request, 
        CancellationToken cancellationToken)
    {
        var existingCourse = await courseRepository.GetByTitle(request.Title, cancellationToken);

        return await existingCourse.MatchAsync(
            c => new CourseAlreadyExistsException(c.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<CourseException, Course>> CreateEntity(
        CreateCourseCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var course = Course.New(CourseId.New(), request.Title, request.Price, request.IsActive, request.Level);
            return await courseRepository.Add(course, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUnhandledException(CourseId.Empty(), exception);
        }
    }
}
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using LanguageExt;
using MediatR;

namespace Application.Courses.Commands;

public record DeleteCourseCommand : IRequest<Either<CourseException, Course>>
{
    public required Guid CourseId { get; init; }
}

public class DeleteCourseCommandHandler(ICourseRepository courseRepository) 
    : IRequestHandler<DeleteCourseCommand, Either<CourseException, Course>>
{
    public async Task<Either<CourseException, Course>> Handle(
        DeleteCourseCommand request, 
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var courseOption = await courseRepository.GetById(courseId, cancellationToken);

        return await courseOption.MatchAsync(
            c => DeleteEntity(c, cancellationToken),
            () => new CourseNotFoundException(courseId));
    }

    private async Task<Either<CourseException, Course>> DeleteEntity(
        Course course, 
        CancellationToken cancellationToken)
    {
        try
        {
            await courseRepository.Delete(course, cancellationToken);
            return course;
        }
        catch (Exception exception)
        {
            return new CourseUnhandledException(course.Id, exception);
        }
    }
}
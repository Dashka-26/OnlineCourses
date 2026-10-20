using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Domain.Courses;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Courses.Commands;

public record UpdateCourseCommand : IRequest<Either<CourseException, Course>>
{
    public required Guid CourseId { get; init; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required bool IsActive { get; init; }
    public required Level Level { get; init; }
}

public class UpdateCourseCommandHandler(ICourseRepository courseRepository) 
    : IRequestHandler<UpdateCourseCommand, Either<CourseException, Course>>
{
    public async Task<Either<CourseException, Course>> Handle(
        UpdateCourseCommand request, 
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.CourseId);
        var courseOption = await courseRepository.GetById(courseId, cancellationToken);

        return await courseOption.MatchAsync(
            c => CheckDuplicates(c.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(c, request, cancellationToken)),
            () => new CourseNotFoundException(courseId));
    }

    private async Task<Either<CourseException, Unit>> CheckDuplicates(
        CourseId currentCourseId, 
        string title, 
        CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByTitle(title, cancellationToken);

        return course.Match<Either<CourseException, Unit>>(
            c => c.Id == currentCourseId ? Unit.Default : new CourseAlreadyExistsException(c.Id),
            () => Unit.Default);
    }

    private async Task<Either<CourseException, Course>> UpdateEntity(
        Course course, 
        UpdateCourseCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            course.UpdateDetails(request.Title, request.Price, request.IsActive, request.Level);
            return await courseRepository.Update(course, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUnhandledException(course.Id, exception);
        }
    }
}
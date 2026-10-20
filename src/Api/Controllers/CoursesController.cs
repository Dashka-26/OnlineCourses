using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Courses.Commands;
using Domain.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courses")]
[ApiController]
public class CoursesController(ISender sender, ICourseQueries courseQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetCourses(CancellationToken cancellationToken)
    {
        var courses = await courseQueries.GetAll(cancellationToken);
        return Ok(courses.Select(CourseDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid id, CancellationToken cancellationToken)
    {
        var courseOption = await courseQueries.GetById(new CourseId(id), cancellationToken);
        
        return courseOption.Match<ActionResult<CourseDto>>(
            c => CourseDto.FromDomainModel(c),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse(
        [FromBody] CreateCourseDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCourseCommand
        {
            Title = request.Title,
            Price = request.Price,
            IsActive = request.IsActive,
            Level = request.Level
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseDto>>(
            c => CreatedAtAction(nameof(GetCourse), new { id = c.Id.Value }, CourseDto.FromDomainModel(c)),
            e => e.ToObjectResult());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        Guid id,
        [FromBody] UpdateCourseDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCourseCommand
        {
            CourseId = id,
            Title = request.Title,
            Price = request.Price,
            IsActive = request.IsActive,
            Level = request.Level
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseDto>>(
            c => Ok(CourseDto.FromDomainModel(c)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteCourseCommand { CourseId = id };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<IActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());
    }
}
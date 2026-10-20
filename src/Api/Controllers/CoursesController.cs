using Api.Dtos;
using Application.Courses.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courses")]
[ApiController]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(CancellationToken cancellationToken)
    {
        var courses = await courseService.GetCourses(cancellationToken);
        return Ok(courses.Select(CourseDto.FromDomainModel));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await courseService.GetCourse(id, cancellationToken);
        if (course is null)
        {
            return NotFound();
        }
        return CourseDto.FromDomainModel(course);
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse(
        [FromBody] CreateCourseDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseService.Add(request.Title, request.Price, request.IsActive, request.Level, cancellationToken);
            var dto = CourseDto.FromDomainModel(course);
            
            return CreatedAtAction(nameof(GetCourse), new { id = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        Guid id,
        [FromBody] UpdateCourseDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseService.Update(id, request.Title, request.Price, request.IsActive, request.Level, cancellationToken);
            if (course is null)
            {
                return NotFound();
            }
            return Ok(CourseDto.FromDomainModel(course));
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await courseService.Delete(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent(); 
    }
}
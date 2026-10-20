using Application.Courses.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CourseErrorFactory
{
    public static ObjectResult ToObjectResult(this CourseException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                CourseAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseNotFoundException => StatusCodes.Status404NotFound,
                CourseUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Course error handler is not implemented.")
            }
        };
    }
}
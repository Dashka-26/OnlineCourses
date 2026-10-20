using FluentValidation;

namespace Application.Courses.Commands;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Price).InclusiveBetween(0.01m, 1_000_000m);
        RuleFor(x => x.Level).IsInEnum();
    }
}
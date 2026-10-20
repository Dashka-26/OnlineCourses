using FluentValidation;

namespace Application.Courses.Commands;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        RuleFor(x => x.Price).InclusiveBetween(0.01m, 1_000_000m);
        RuleFor(x => x.Level).IsInEnum();
    }
}
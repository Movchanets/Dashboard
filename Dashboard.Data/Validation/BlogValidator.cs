using Dashboard.Data.Data.Models.ViewModels;
using FluentValidation;

namespace Dashboard.Data.Validation;


public class BlogValidator : AbstractValidator<PostVM>
{
    public BlogValidator()
    {
        RuleFor(r => r.ShortDescription).MaximumLength(300);
        RuleFor(r => r.Title).NotEmpty().MinimumLength(4);
        RuleFor(r => r.Description).NotEmpty().MaximumLength(4000);
        RuleFor(r => r.Body).NotEmpty().MaximumLength(4000);
        

    }
}
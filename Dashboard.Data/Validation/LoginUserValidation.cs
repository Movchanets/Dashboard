using Dashboard.Data.Data.Models.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Validation
{
    public class LoginUserValidation : AbstractValidator<LoginUserVM>
    {
        public LoginUserValidation()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
        }
    }
}

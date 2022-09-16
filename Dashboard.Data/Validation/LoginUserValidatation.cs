using Dashboard.Data.Data.Models.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Validation
{
   
      public class LoginUserValidatation : AbstractValidator<LoginUserVM>
    {
        public LoginUserValidatation()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
            

        }
    }
}

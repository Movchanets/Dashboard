using Dashboard.Data.Data.Models.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Validation
{
	public class ChangePasswordValidation : AbstractValidator<ChangePasswordVM>
	{
		public ChangePasswordValidation()
		{
			RuleFor(r => r.OldPassword).NotEmpty().MinimumLength(6);
			RuleFor(r => r.OldPassword).NotEmpty().MinimumLength(6);
			RuleFor(r => r.Email).EmailAddress().NotEmpty();
		}
	}
}

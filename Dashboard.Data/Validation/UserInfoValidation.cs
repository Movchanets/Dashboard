using Dashboard.Data.Data.Models.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Validation
{
	public class UserInfoValidation : AbstractValidator<UserInfoVM>
	{

		public UserInfoValidation()
		{
			RuleFor(r => r.Email).NotEmpty().EmailAddress();
			RuleFor(r => r.Name).NotEmpty();
			RuleFor(r => r.Surname).NotEmpty();
			RuleFor(r => r.UserName).NotEmpty().MaximumLength(32);
			RuleFor(r => r.Phone).NotEmpty().MaximumLength(12).MinimumLength(4);
		}
	}		
}

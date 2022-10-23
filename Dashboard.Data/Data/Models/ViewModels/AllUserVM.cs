using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Data.Models.ViewModels
{
	public class AllUserVM
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string Email { get; set; }
		public string? Phone { get; set; }
		public bool EmailConfirm { get; set; }
		public string Role {get;set;}
		public bool isBlocked { get; set; }
	}
}

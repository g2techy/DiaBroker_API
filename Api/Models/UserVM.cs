using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace G2.DB.Api.Models
{
	public class LoginVM : Infrastructure.Core.BaseViewModel
	{
		[Required(ErrorMessage = "User name is required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }
	}

	public class UserRoleVM : Infrastructure.Core.BaseViewModel
	{
		public int RoleID { get; set; }
		public string RoleName { get; set; }
	}

	public class LoggedInUserVM : Infrastructure.Core.BaseViewModel
	{
		public string UserName { get; set; }
		public string UserDisplayName { get; set; }
		public List<UserRoleVM> UserRoles { get; set; }

		public LoggedInUserVM()
		{
			UserRoles = new List<UserRoleVM>();
		}
	}

	public class UserVM : Infrastructure.Core.BaseViewModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CompanyName { get; set; }
		public string CompanyAddress { get; set; }
		public string PhoneNo { get; set; }
		public string MobileNo { get; set; }
	}

}
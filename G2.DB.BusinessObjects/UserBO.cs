using System;
using System.Collections.Generic;
using System.Text;

namespace G2.DB.BusinessObjects
{
	public class LoginBO : BaseBO
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
	public class UserRoleBO : BaseBO
	{
		public int RoleID { get; set; }
		public string RoleName { get; set; }
	}
	
    public class LoggedInUserBO : BaseBO
    {
		public string UserName { get; set; }
		public string UserDisplayName { get; set; }
		public List<UserRoleBO> UserRoles { get; set; }

		public LoggedInUserBO()
		{
			UserRoles = new List<UserRoleBO>();
		}
    }

	public class UserBO : BaseBO
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

	public class ChangePwdBO : BaseBO
	{
		public int UserID { get; set; }
		public string NewPassword { get; set; }
		public string OldPassword { get; set; }
	}

}

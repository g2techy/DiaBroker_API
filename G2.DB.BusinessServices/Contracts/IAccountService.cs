using System;
using System.Collections.Generic;
using System.Text;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Contracts
{
    public interface IAccountService
    {
		BO.LoggedInUserBO ValidateUserCreds(BO.LoginBO login);
		int RegisterUser(BO.UserBO bo);
		bool ChangePassword(BO.ChangePwdBO changePwd);
		List<BO.UserBO> AllUsers(int userID = 0);
		BO.UserBO UserDetails(int userID);
		void UpdateUser(BO.UserBO bo);
	}
}

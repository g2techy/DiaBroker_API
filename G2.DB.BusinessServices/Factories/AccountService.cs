using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Factories
{
	public class AccountService : BaseService, Contracts.IAccountService
	{
		public BO.LoggedInUserBO ValidateUserCreds(BO.LoginBO loginBO)
		{
			BO.LoggedInUserBO _loggedInUser = null;
			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Account_VerifyLoginCreds", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@UserName",DAL.ParameterDirection.In, DAL.DataType.String, loginBO.UserName),
					new DAL.DatabaseParameter("@Password",DAL.ParameterDirection.In, DAL.DataType.String, Encrypt(loginBO.Password))
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0)
					{
						var _row = _ds.Tables[0].Rows[0];
						_loggedInUser = new BO.LoggedInUserBO()
						{
							UserID = Convert.ToInt32(_row["UserID"]),
							UserName = Convert.ToString(_row["UserName"]),
							UserDisplayName = Convert.ToString(_row["UserDisplayName"])
						};

						if (_ds.Tables.Count > 1 && _ds.Tables[1] != null)
						{
							_ds.Tables[1].Rows.Cast<DataRow>().All(dr => {
								_loggedInUser.UserRoles.Add(new BO.UserRoleBO()
								{
									RoleID = Convert.ToInt32(dr["RoleID"]),
									RoleName = Convert.ToString(dr["RoleName"])
								});
								return true;
							});
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
			return _loggedInUser;
		}

		public int RegisterUser(BO.UserBO bo)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Account_AddUser", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@UserID",DAL.ParameterDirection.Out, DAL.DataType.Int),
					new DAL.DatabaseParameter("@UserName",DAL.ParameterDirection.In,DAL.DataType.String, bo.UserName, 100),
					new DAL.DatabaseParameter("@Password",DAL.ParameterDirection.In,DAL.DataType.String, Encrypt(bo.Password), 200),
					new DAL.DatabaseParameter("@FirstName",DAL.ParameterDirection.In,DAL.DataType.String, bo.FirstName, 100),
					new DAL.DatabaseParameter("@LastName",DAL.ParameterDirection.In,DAL.DataType.String, bo.LastName, 100),
					new DAL.DatabaseParameter("@CompanyName",DAL.ParameterDirection.In,DAL.DataType.String, bo.CompanyName, 200),
					new DAL.DatabaseParameter("@CompanyAddress",DAL.ParameterDirection.In,DAL.DataType.String, bo.CompanyAddress, 1000),
					new DAL.DatabaseParameter("@PhoneNo",DAL.ParameterDirection.In,DAL.DataType.String, bo.PhoneNo),
					new DAL.DatabaseParameter("@MobileNo",DAL.ParameterDirection.In,DAL.DataType.String, bo.PhoneNo)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@UserID"]);
				_ourParams = null;
			}
			catch
			{
				DatabaseAccess.RollbackTransaction();
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
			return _ID;
		}

		public bool ChangePassword(BO.ChangePwdBO changePwd)
		{
			bool _isSuccess = false;

			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Account_ChangePwd", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@UserID", DAL.ParameterDirection.In, DAL.DataType.Int, changePwd.UserID),
					new DAL.DatabaseParameter("@OldPassword", DAL.ParameterDirection.In, DAL.DataType.String, Encrypt(changePwd.OldPassword), 200),
					new DAL.DatabaseParameter("@NewPassword", DAL.ParameterDirection.In, DAL.DataType.String, Encrypt(changePwd.NewPassword), 200)
				});
				DatabaseAccess.CommitTransaction();
				_ourParams = null;
				_isSuccess = true;
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}

			return _isSuccess;
		}

		public List<BO.UserBO> AllUsers(int userID = 0)
		{
			List<BO.UserBO> _users = null;
			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Account_AllUsers", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@UserID",DAL.ParameterDirection.In, DAL.DataType.Int, userID),
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0] != null)
					{
						_users = new List<BO.UserBO>();

						_ds.Tables[0].Rows.Cast<DataRow>().All(_row => {
							_users.Add(new BO.UserBO()
							{
								UserID = Convert.ToInt32(_row["UserID"]),
								UserName = Convert.ToString(_row["UserName"]),
								FirstName = Convert.ToString(_row["FirstName"]),
								LastName = Convert.ToString(_row["LastName"]),
								CompanyName = Convert.ToString(_row["CompanyName"]),
								CompanyAddress = Convert.ToString(_row["CompanyAddress"]),
								PhoneNo = Convert.ToString(_row["PhoneNo"]),
								MobileNo = Convert.ToString(_row["MobileNo"])
							});
							return true;
						});
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
			return _users;
		}

		public BO.UserBO UserDetails(int userID)
		{
			return AllUsers(userID).FirstOrDefault();
		}
				
		public void UpdateUser(BO.UserBO bo)
		{
			try
			{
				DatabaseAccess.OpenConnection(true);
				DatabaseAccess.ExecuteProcedureDML("P_Account_UpdateUser", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@UserID",DAL.ParameterDirection.In, DAL.DataType.Int, bo.UserID),
					new DAL.DatabaseParameter("@FirstName",DAL.ParameterDirection.In,DAL.DataType.String, bo.FirstName, 100),
					new DAL.DatabaseParameter("@LastName",DAL.ParameterDirection.In,DAL.DataType.String, bo.LastName, 100),
					new DAL.DatabaseParameter("@CompanyName",DAL.ParameterDirection.In,DAL.DataType.String, bo.CompanyName, 200),
					new DAL.DatabaseParameter("@CompanyAddress",DAL.ParameterDirection.In,DAL.DataType.String, bo.CompanyAddress, 1000),
					new DAL.DatabaseParameter("@PhoneNo",DAL.ParameterDirection.In,DAL.DataType.String, bo.PhoneNo),
					new DAL.DatabaseParameter("@MobileNo",DAL.ParameterDirection.In,DAL.DataType.String, bo.PhoneNo)
				});
				DatabaseAccess.CommitTransaction();
			}
			catch
			{
				DatabaseAccess.RollbackTransaction();
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
		}

		private string Encrypt(string password)
		{
			return G2.Frameworks.Core.Encryption.Encrypt(password);
		}
		private string Decrypt(string password)
		{
			return G2.Frameworks.Core.Encryption.Decrypt(password);
		}
		
	}
}

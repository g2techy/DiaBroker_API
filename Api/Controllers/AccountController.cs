using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BO = G2.DB.BusinessObjects;
using BS = G2.DB.BusinessServices;

namespace G2.DB.Api.Controllers
{
	[Infrastructure.Filters.AuthorizationFilter(Infrastructure.Constants.UserRole.Admin)]
	[RoutePrefix("api/account")]
	public class AccountController : Infrastructure.Core.BaseApiController
	{
		#region DI settings

		private readonly BS.Contracts.IAccountService _accountService = null;
		public AccountController(BS.Contracts.IAccountService accountService)
		{
			_accountService = accountService;
		}

		#endregion

		#region API methods

		[HttpPost]
		public IHttpActionResult Register([FromBody]Models.UserVM user)
		{
			int _userID = 0;

			if (!ModelState.IsValid || user == null)
			{
				return BadRequest("Invalid user details.");
			}
			try
			{
				_userID = _accountService.RegisterUser(Infrastructure.Utilities.BOVMMapper.Map<Models.UserVM, BO.UserBO>(user));
				if (_userID <= 0)
				{
					return BadRequest("Unable to register user. Please contact admin for more detailed error.");
				}
			}
			catch (Exception ex)
			{
				base.LogException(ex);
			}

			return Ok(_userID);
		}

		[HttpPost]
		[Route("login")]
		public IHttpActionResult Login([FromBody]Models.LoginVM login)
		{
			Models.LoggedInUserVM _modal = null;

			if (!ModelState.IsValid || login == null)
			{
				return BadRequest("Please enter user name and password.");
			}
			try
			{
				var _bo = _accountService.ValidateUserCreds(Infrastructure.Utilities.BOVMMapper.Map<Models.LoginVM, BO.LoginBO>(login));
				_modal = Infrastructure.Utilities.BOVMMapper.Map<BO.LoggedInUserBO, Models.LoggedInUserVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_modal);
		}

		[HttpGet]
		[Route("users")]
		public IHttpActionResult AllUsers()
		{
			List<Models.UserVM> _model = null;

			try
			{
				var _bo = _accountService.AllUsers();
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.UserBO>, List<Models.UserVM>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
			}

			return Ok(_model);
		}

		[HttpGet]
		[Route("user/{userID}")]
		public IHttpActionResult UserDetails(int userID)
		{
			Models.UserVM _model = null;
			try
			{
				var _bo = _accountService.UserDetails(userID);
				if (_bo == null)
				{
					return BadRequest("User details not found.");
				}
				_model = Infrastructure.Utilities.BOVMMapper.Map<BO.UserBO, Models.UserVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
			}
			return Ok(_model);
		}

		#endregion

		#region Private methods

		#endregion

	}
}

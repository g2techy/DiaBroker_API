using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace G2.DB.Api.Infrastructure.Filters
{
	public class AuthorizationFilter : AuthorizationFilterAttribute
	{
		public Infrastructure.Constants.UserRole[] UserRoles { get; set; }

		public AuthorizationFilter(params Infrastructure.Constants.UserRole[] userRoles)
		{
			this.UserRoles = userRoles;
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			var _princial = HttpContext.Current.User;
			if (!_princial.Identity.IsAuthenticated)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
				return;
			}
			var _identity = _princial.Identity as System.Security.Claims.ClaimsIdentity;
			if (_identity == null)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
				{
					ReasonPhrase = "Invalid identity"
				};
			}
			var _authTokenClaim = _identity.Claims.Where(c => c.Type == Providers.OAuthProvider.ClaimNames.AuthToken).FirstOrDefault();
			if (_authTokenClaim == null)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
				{
					ReasonPhrase = "AuthToken is empty or not found"
				};
			}
			try
			{
				Infrastructure.Providers.OAuthProvider.AuthToken _authToken = Infrastructure.Providers.OAuthProvider.AuthToken.Create(_authTokenClaim.Value);
				if (_authToken.UserRoles.Any(ur => this.UserRoles.Select(r => (int)r).Contains(ur.Key)))
				{
					actionContext.Request.Properties.Add(Infrastructure.Core.BaseApiController.RequestPropKey_AuthToken, _authToken);
				}
				else
				{
					actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
					{
						ReasonPhrase = "You are not authorized to access resource"
					};
				}
			}
			catch
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
				{
					ReasonPhrase = "AuthToken is invalid or tempared"
				};
			}
			base.OnAuthorization(actionContext);
		}

		public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
		{

			return base.OnAuthorizationAsync(actionContext, cancellationToken);
		}

	}
}
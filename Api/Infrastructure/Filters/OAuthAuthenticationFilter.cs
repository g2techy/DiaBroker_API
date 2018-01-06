using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace G2.DB.Api.Infrastructure.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class OAuthAuthenticationFilter : Attribute, IAuthenticationFilter
	{
		public bool AllowMultiple => true;

		public static string AuthenticationScheme = "Bearer";

		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			HttpRequestMessage request = context.Request;
			AuthenticationHeaderValue authorization = request.Headers.Authorization;

			if (authorization == null)
			{
				return;
			}

			if (authorization.Scheme != AuthenticationScheme)
			{
				return;
			}

			if (String.IsNullOrEmpty(authorization.Parameter))
			{
				context.ErrorResult = new AuthenticationFailureResult("Missing token", request);
				return;
			}

			var authManager = HttpContext.Current.GetOwinContext().Authentication;
			var authResult = await authManager.AuthenticateAsync(AuthenticationScheme);
			if (authResult == null)
			{
				context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
				return;
			}

			IPrincipal principal = new GenericPrincipal(authResult.Identity, null);
			context.Principal = principal;
		}

		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			var challenge = new AuthenticationHeaderValue(AuthenticationScheme);
			context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
			return Task.FromResult(0);
		}
	}


	public class AuthenticationFailureResult : IHttpActionResult
	{
		public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
		{
			ReasonPhrase = reasonPhrase;
			Request = request;
		}

		public string ReasonPhrase { get; private set; }
		public HttpRequestMessage Request { get; private set; }
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute());
		}
		private HttpResponseMessage Execute()
		{
			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			response.RequestMessage = Request;
			response.ReasonPhrase = ReasonPhrase;
			return response;
		}
	}

	public class AddChallengeOnUnauthorizedResult : IHttpActionResult
	{
		public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
		{
			Challenge = challenge;
			InnerResult = innerResult;
		}

		public AuthenticationHeaderValue Challenge { get; private set; }

		public IHttpActionResult InnerResult { get; private set; }

		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
				{
					response.Headers.WwwAuthenticate.Add(Challenge);
				}
			}

			return response;
		}
	}

}
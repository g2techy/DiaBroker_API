using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace G2.DB.Api.Infrastructure.Core
{
	public class BaseApiController : ApiController
	{
		public const string RequestPropKey_AuthToken = "AuthToken";

		protected virtual int UserID
		{
			get
			{
				var _authToken = this.AuthToken;
				if (_authToken != null)
				{
					return _authToken.UserID;
				}
				return -1;
			}
		}

		protected Infrastructure.Providers.OAuthProvider.AuthToken AuthToken
		{
			get
			{
				object _authToken = null;
				if (Request.Properties.TryGetValue(RequestPropKey_AuthToken, out _authToken))
				{
					return (_authToken as Infrastructure.Providers.OAuthProvider.AuthToken);
				}
				return null;
			}
		}

		protected void LogException(Exception ex)
		{
			G2.Frameworks.Logging.DefaultLogManagerFactory.LogManager.Error(ex);
		}
	}
}
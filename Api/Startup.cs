using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace G2.DB.Api
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
			ConfigureAuth(app);
		}

		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		static Startup()
		{
			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/api/token"),
				Provider = new Infrastructure.Providers.OAuthProvider(),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
				AllowInsecureHttp = true
			};
		}

		public void ConfigureAuth(IAppBuilder app)
		{
			app.UseOAuthBearerTokens(OAuthOptions);
		}
	}
}
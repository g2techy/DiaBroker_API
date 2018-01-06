using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using BO = G2.DB.BusinessObjects;
using BS = G2.DB.BusinessServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace G2.DB.Api.Infrastructure.Providers
{
	public class OAuthProvider : OAuthAuthorizationServerProvider
	{
		public static class ClaimNames
		{
			public const string UserInfo = "userInfo";
			public const string AuthToken = "authToken";
		}

		public class AuthToken
		{
			private const string PropSeperator = "#";
			private const string RoleSeperator = "$";
			private const string RolePropSeperator = ":";
			public int UserID { get; set; }
			public IDictionary<int, string> UserRoles { get; set; }
			public AuthToken(int userID) : this(userID, new Dictionary<int, string>())
			{
			}
			public AuthToken(int userID, IDictionary<int, string> roles)
			{
				this.UserID = userID;
				this.UserRoles = roles;
			}
			public override string ToString()
			{
				string _roles = string.Empty;
				this.UserRoles.All(r =>
				   {
					   _roles += r.Key.ToString() + RolePropSeperator + r.Value + RoleSeperator;
					   return true;
				   });
				return UserID.ToString() + PropSeperator + _roles;
			}

			public static AuthToken Create(string authToken)
			{
				if (string.IsNullOrEmpty(authToken))
				{
					return null;
				}
				string[] _parts = authToken.Split(PropSeperator.ToCharArray());
				if (_parts.Length == 2)
				{
					var _newObj = new AuthToken(Convert.ToInt32(_parts[0]));
					if (!string.IsNullOrEmpty(_parts[1]))
					{
						foreach (string _role in _parts[1].Split(RoleSeperator.ToCharArray()))
						{
							string[] _roleParts = _role.Split(RolePropSeperator.ToCharArray());
							if (_roleParts.Length == 2)
							{
								if (!string.IsNullOrEmpty(_roleParts[0]) && !string.IsNullOrEmpty(_roleParts[1]))
								{
									_newObj.UserRoles.Add(Convert.ToInt32(_roleParts[0]), _roleParts[1]);
								}
							}
						}
					}
					return _newObj;
				}
				return null;
			}
		}

		#region OAuthAuthorizationServerProvider members

		public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			return Task.Factory.StartNew(() =>
			{
				var _service = new BS.Factories.AccountService();
				try
				{
					var user = _service.ValidateUserCreds(new BO.LoginBO()
					{
						UserName = context.UserName,
						Password = context.Password
					});
					if (user != null)
					{
						IDictionary<int, string> _roles = new Dictionary<int, string>();
						user.UserRoles.All(ur =>
						   {
							   _roles.Add(ur.RoleID, ur.RoleName); return true;
						   });
						var claims = new List<Claim>()
						{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim(ClaimNames.AuthToken, (new AuthToken(user.UserID, _roles).ToString()))
						};
						
						ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
						IDictionary<string, string> _authProps = new Dictionary<string, string>();
						_authProps.Add(ClaimNames.UserInfo, JsonConvert.SerializeObject(user, new JsonSerializerSettings
						{
							ContractResolver = new CamelCasePropertyNamesContractResolver()
						}));

						var ticket = new AuthenticationTicket(oAuthIdentity, CreateProperties(_authProps));
						context.Validated(ticket);
					}
					else
					{
						context.SetError("invalid_grant", "The user name or password is incorrect");
					}
				}
				catch (Exception ex)
				{
					context.SetError("invalid_grant", ex.Message);
				}
			});
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			if (context.ClientId == null)
				context.Validated();

			return Task.FromResult<object>(null);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}


		public static AuthenticationProperties CreateProperties(IDictionary<string, string> paramList)
		{
			return new AuthenticationProperties(paramList);
		}

		#endregion
	}
}

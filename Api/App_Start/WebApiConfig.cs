using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace G2.DB.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.EnableCors();
			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.Filters.Add(new Infrastructure.Filters.OAuthAuthenticationFilter());
			config.Filters.Add(new Infrastructure.Filters.ValidateModelStateFilterAttribute());
			config.Filters.Add(new Infrastructure.Filters.GlobalExceptionFilterAttribute());

			config.Services.Add(typeof(System.Web.Http.ExceptionHandling.IExceptionLogger), new Infrastructure.Filters.GlobalExceptionLogger());
			config.Services.Replace(typeof(System.Web.Http.ExceptionHandling.IExceptionHandler), new Infrastructure.Filters.GlobalExceptionHandler());
		}
	}
}

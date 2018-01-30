using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using G2.Frameworks.Logging;

namespace G2.DB.Api.Infrastructure.Filters
{
	public class GlobalExceptionLogger : ExceptionLogger
	{
		public override void Log(ExceptionLoggerContext context)
		{
			List<string> _errMsg = new List<string>();
			if (context.Request != null)
			{
				_errMsg.Add("RequestUrl: " + context.Request.RequestUri.PathAndQuery);
			}
			if (context.RequestContext.Principal != null)
			{
				_errMsg.Add("UserName: " + context.RequestContext.Principal.Identity.Name);
			}

			LogException(context.Exception, _errMsg);
		}
		
		public static void LogException(Exception ex)
		{
			LogException(ex, null);
		}

		public static void LogException(Exception ex, List<string> additionalMessages)
		{
			if (additionalMessages != null && additionalMessages.Count() > 0)
			{
				additionalMessages.ForEach(msg => DefaultLogManagerFactory.LogManager.Error(msg));
			}
			if (ex is Infrastructure.Filters.ModelStateValidationException)
			{
				var _msEx = ex as Infrastructure.Filters.ModelStateValidationException;
				DefaultLogManagerFactory.LogManager.Error("ModelState : " + string.Join("$", _msEx.Errors));
			}
			DefaultLogManagerFactory.LogManager.Error(ex);
		}
		
	}
}
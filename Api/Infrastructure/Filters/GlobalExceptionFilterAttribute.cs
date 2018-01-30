using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Net;
using System.Net.Http;

namespace G2.DB.Api.Infrastructure.Filters
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
			context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetErrorMessage(context.Exception));
		}

		public static string GetErrorMessage(Exception ex)
		{
			if (ex == null)
			{
				return "Exception is null";
			}

			Exception _innerEx = ex;
			while (_innerEx.InnerException != null)
				_innerEx = _innerEx.InnerException;

			string _errorMessage = _innerEx.Message;
			if (_innerEx is Infrastructure.Filters.ModelStateValidationException)
			{
				var _msEx = (Infrastructure.Filters.ModelStateValidationException)_innerEx;
				_errorMessage = "Model Errors : " + string.Join("$", _msEx.Errors);
			}
			return _errorMessage;
		}
	}
}
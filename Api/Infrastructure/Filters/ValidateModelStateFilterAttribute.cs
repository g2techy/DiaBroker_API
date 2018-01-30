using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace G2.DB.Api.Infrastructure.Filters
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ValidateModelStateFilterAttribute : ActionFilterAttribute
	{
		public ValidateModelStateFilterAttribute()
		{
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			ModelStateDictionary _modelState = actionContext.ModelState;
			if (!_modelState.IsValid)
			{
				//actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
				throw new ModelStateValidationException(_modelState.GetErrors());
			}
		}
	}

	public class ModelStateValidationException : Exception
	{
		public List<string> Errors { get; private set; }
		public ModelStateValidationException(List<string> errorList)
		{
			this.Errors = errorList;
		}
	}

	public static class ModelStateExtensions
	{
		public static List<string> GetErrors(this ModelStateDictionary modelState)
		{
			var _validationErrors = new List<string>();

			foreach (var state in modelState)
			{
				_validationErrors.AddRange(state.Value.Errors
					.Select(error => error.ErrorMessage)
					.ToList());
			}

			return _validationErrors;
		}
	}

}
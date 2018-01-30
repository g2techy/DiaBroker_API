using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace G2.DB.Api.Infrastructure.Filters
{
	public class GlobalExceptionHandler : ExceptionHandler
	{
		public override void Handle(ExceptionHandlerContext context)
		{
			context.Result = new ErrorActionResult()
			{
				Request = context.Request,
				Content = GlobalExceptionFilterAttribute.GetErrorMessage(context.Exception)
			};
		}
		
		private class ErrorActionResult : IHttpActionResult
		{
			public HttpRequestMessage Request { get; set; }

			public string Content { get; set; }

			public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
			{
				HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
				response.Content = new StringContent(Content);
				response.RequestMessage = Request;
				return Task.FromResult(response);
			}
		}



	}
}
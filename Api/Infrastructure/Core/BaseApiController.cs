using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace G2.DB.Api.Infrastructure.Core
{
	public class BaseApiController : ApiController
	{
		protected virtual int UserID
		{
			get
			{
				return 162236;
			}
		}

		protected void LogException(Exception ex)
		{
			G2.Frameworks.Logging.DefaultLogManagerFactory.LogManager.Error(ex);
		}
	}
}
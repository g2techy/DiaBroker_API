using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G2.DB.Api.Models
{
	public class PagerVM : Infrastructure.Core.BaseViewModel
	{
		public int StartIndex { get; set; }
		public int PageSize { get; set; }
	}
}
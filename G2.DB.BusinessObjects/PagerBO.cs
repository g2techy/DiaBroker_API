using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2.DB.BusinessObjects
{
	public class PagerBO : BaseBO
	{
		public int StartIndex { get; set; }
		public int PageSize { get; set; }
	}
}

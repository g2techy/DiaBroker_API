using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2.DB.BusinessObjects
{
	public class PartyBO : BaseBO
	{
		public int PartyID { get; set; }
		public string PartyCode { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNo { get; set; }
		public string MobileNo { get; set; }
		public List<string> SelectedPartyTypes { get; set; }
	}

	public class PartyTypeBO : BaseBO
	{
		public int PartyTypeID { get; set; }
		public string PartyTypeName { get; set; }
	}

	public class PartySearchBO : PagerBO
	{
		public string PartyCode { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class PartySearchResultBO : BaseBO
	{
		public int StartIndex { get; set; }
		public int PageSize { get; set; }
		public int RecordCount { get; set; }

		public List<PartyBO> PartyList { get; set; }

		public PartySearchResultBO(int recCnt)
		{
			RecordCount = recCnt;
			PartyList = new List<PartyBO>();
		}
		public PartySearchResultBO() : this(0) { }
	}
}


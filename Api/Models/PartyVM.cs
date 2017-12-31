using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace G2.DB.Api.Models
{

	public class PartySearchVM : PagerVM
	{
		public string PartyCode { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class PartySearchResultVM : Infrastructure.Core.BaseViewModel
	{
		public int RecordCount { get; set; }
		public int StartIndex { get; set; }
		public int PageSize { get; set; }

		public List<PartyVM> PartyList { get; set; }

		public PartySearchResultVM(int recCnt)
		{
			RecordCount = recCnt;
			PartyList = new List<PartyVM>();
		}
		public PartySearchResultVM() : this(0) { }
	}

	public class PartyVM : Infrastructure.Core.BaseViewModel
	{

		public int PartyID { get; set; }
		
		[Required]
		[MaxLength(length: 20)]
		public string PartyCode { get; set; }

		[Required]
		[MaxLength(length: 100)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(length: 100)]
		public string LastName { get; set; }

		[MaxLength(length: 100)]
		public string PhoneNo { get; set; }

		[MaxLength(length: 100)]
		public string MobileNo { get; set; }

		[Required]
		public List<string> SelectedPartyTypes { get; set; }

		public PartyVM()
		{
			SelectedPartyTypes = new List<string>();
		}

	}

	public class PartyTypeVM : Infrastructure.Core.BaseViewModel
	{
		public int PartyTypeID { get; set; }
		public string PartyTypeName { get; set; }
	}
}
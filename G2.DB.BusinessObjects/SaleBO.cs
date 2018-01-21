using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2.DB.BusinessObjects
{
	public class SaleBO : BaseBO
	{
		public int SaleID { get; set; }
		public DateTime SaleDate { get; set; }
		public string Saller { get; set; }
		public string Buyer { get; set; }
		public float TotalWeight { get; set; }
		public float RejectionWt { get; set; }
		public float SelectionWt { get; set; }
		public float UnitPrice { get; set; }
		public float NetSaleAmount { get; set; }
		public int DueDays { get; set; }
		public string TotalBrokerage { get; set; }
		public float TotalPayAmount { get; set; }
		public DateTime PayDate { get; set; }
		public string Status { get; set; }
		public string RefNo { get; set; }
		public DateTime DueDate { get; set; }
		public float LessPer { get; set; }
	}

	public class SaleSearchBO : PagerBO
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int SallerID { get; set; }
		public int BuyerID { get; set; }
		public string RefNo { get; set; }
	}

	public class SaleSearchResultBO : BaseBO
	{
		public int RecordCount { get; private set; }
		public int StartIndex { get; set; }
		public int PageSize { get; set; }

		public List<SaleBO> SalesList { get; set; }

		public SaleSearchResultBO(int recCnt)
		{
			RecordCount = recCnt;
			SalesList = new List<SaleBO>();
		}
		public SaleSearchResultBO() : this(0) { }

	}

	public class SaleAddBO : BaseBO
	{
		public int SaleID { get; set; }
		public int DueDays { get; set; }
		public string SaleDate { get; set; }
		public int SallerID { get; set; }
		public int BuyerID { get; set; }
		public float TotalWeight { get; set; }
		public float RejectionWeight { get; set; }
		public float UnitPrice { get; set; }
		public float LessPer { get; set; }
		public int Status { get; set; }
	}

	public class Party
	{
		public int PartyID { get; set; }
		public string PartyName { get; set; }
	}

	public class SaleBrokerageBO : BaseBO
	{
		public int BDID { get; set; }
		public int SaleID { get; set; }
		public int BrokerID { get; set; }
		public string BrokerName { get; set; }
		public float Brokerage { get; set; }
		public float BrokerageAmount { get; set; }
		public bool IsPaid { get; set; }
		public DateTime PayDate { get; set; }
		public string PayComments { get; set; }
	}

	public class SaleBrokerageAddBO : BaseBO
	{
		public int BDID { get; set; }
		public int SaleID { get; set; }
		public int BrokerID { get; set; }
		public float Brokerage { get; set; }
	}

	public class SaleBrokPaymentBO : BaseBO
	{
		public int BDID { get; set; }
		public DateTime PayDate { get; set; }
		public string PayComments { get; set; }
	}

	public class SalePaymentBO
	{
		public int PayID { get; set; }
		public int SaleID { get; set; }
		public DateTime PayDate { get; set; }
		public float PayAmount { get; set; }
		public string CourierFrom { get; set; }
		public string CourierTo { get; set; }
	}

	public class SalesReportBO : BaseBO
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int? SallerID { get; set; }
		public int? BuyerID { get; set; }
		public int? Status { get; set; }
		public int? DueDays { get; set; }
	}

	public class SaleStatusBO : BaseBO
	{
		public int StatusID { get; set; }
		public string StatusName { get; set; }
	}
}

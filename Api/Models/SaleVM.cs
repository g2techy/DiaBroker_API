using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace G2.DB.Api.Models
{
	public class SaleVM : Infrastructure.Core.BaseViewModel
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

	public class SaleSearchVM : PagerVM
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int SallerID { get; set; }
		public int BuyerID { get; set; }
		public string RefNo { get; set; }
	}

	public class SaleSearchResultVM : Infrastructure.Core.BaseViewModel
	{
		public int RecordCount { get; private set; }
		public int StartIndex { get; set; }
		public int PageSize { get; set; }

		public List<SaleVM> SalesList { get; set; }

		public SaleSearchResultVM(int recCnt)
		{
			RecordCount = recCnt;
			SalesList = new List<SaleVM>();
		}
		public SaleSearchResultVM() : this(0) { }
	}

	public class SaleAddVM : Infrastructure.Core.BaseViewModel
	{
		public int SaleID { get; set; }

		[Required]
		public string SaleDate { get; set; }

		[Required]
		public int SallerID { get; set; }

		[Required]
		public int BuyerID { get; set; }

		[Required]
		public int DueDays { get; set; }

		[Required]
		public float TotalWeight { get; set; }

		[Required]
		public float RejectionWeight { get; set; }

		public float SelectionWeight { get; set; }

		[Required]
		public float UnitPrice { get; set; }

		public float NetSaleAmount { get; set; }

		[Required]
		[Range(minimum: 0, maximum: 10)]
		public float LessPer { get; set; }

		public int Status { get; set; }

	}

	public class Party
	{
		public int PartyID { get; set; }
		public string PartyName { get; set; }
	}

	public class SaleBrokerage : Infrastructure.Core.BaseViewModel
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

	public class SaleBrokerageAddVM : Infrastructure.Core.BaseViewModel
	{
		[Required]
		public int BDID { get; set; }

		[Required]
		public int SaleID { get; set; }

		[Required]
		public int BrokerID { get; set; }

		[Required]
		[Range(typeof(float), "0.01", "100.00")]
		public float Brokerage { get; set; }
	}

	public class SaleBrokPaymentVM : Infrastructure.Core.BaseViewModel
	{
		[Required]
		public int BDID { get; set; }
		
		[Required]
		public string PayDate { get; set; }

		[Required]
		[MaxLength(1000)]
		public string PayComments { get; set; }
	}

	public class SalePaymentVM : Infrastructure.Core.BaseViewModel
	{
		public SalePayment Payment { get; set; }

		public List<SalePayment> PaymentList { get; set; }

		public SalePaymentVM()
		{
			PaymentList = new List<SalePayment>();
		}

	}

	public class SalePayment
	{
		public int SaleID { get; set; }

		public int PayID { get; set; }

		[Required]
		public DateTime PayDate { get; set; }

		[Required]
		public float PayAmount { get; set; }

		[Required]
		public string CourierFrom { get; set; }

		[Required]
		public string CourierTo { get; set; }
	}

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace G2.DB.Api.Models
{
	public class LoanAddVM : Infrastructure.Core.BaseViewModel
	{
		public int LoanID { get; set; }

		[Required]
		public string StartDate { get; set; }
		public string EndDate { get; set; }

		[Required]
		public int BorrowerID { get; set; }

		[Required]
		[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid amount")]
		public float PrincipalAmount { get; set; }

		[Required]
		[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid interest rate")]
		public float MonthlyInterest { get; set; }

		public string Comments { get; set; }
		public int Status { get; set; }
		public int RefNo { get; set; }
	}

	public class LoanSearchVM : PagerVM
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int BorrowerID { get; set; }
		public string RefNo { get; set; }
	}

	public class LoanSearchResultVM : PagerVM
	{
		public int RecordCount { get; private set; }
		public List<LoanVM> LoanList { get; set; }
		public LoanSearchResultVM(int recCnt)
		{
			RecordCount = recCnt;
			LoanList = new List<LoanVM>();
		}
		public LoanSearchResultVM() : this(0) { }
	}

	public class LoanVM : Infrastructure.Core.BaseViewModel
	{
		public int LoanID { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Borrower { get; set; }
		public float PrincipalAmount { get; set; }
		public float MonthlyInterest { get; set; }
		public float TotalPayAmount { get; set; }
		public DateTime PayDate { get; set; }
		public int StatusID { get; set; }
		public string StatusName { get; set; }
		public string RefNo { get; set; }
	}

	public class LoanPaymentAddVM
	{
		[Required]
		public int LoanPayID { get; set; }
		[Required]
		public int LoanID { get; set; }
		[Required]
		public DateTime PayDate { get; set; }
		[Required]
		public float PayAmount { get; set; }
		[Required]
		public int PayType { get; set; }
		[Required]
		public string PayComments { get; set; }
	}

	public class LoanPaymentVM
	{
		public int LoanPayID { get; set; }
		public int LoanID { get; set; }
		public DateTime PayDate { get; set; }
		public float PayAmount { get; set; }
		public int PayType { get; set; }
		public string PayTypeName { get; set; }
		public string PayComments { get; set; }
		public float PrincipalPaid { get; set; }
		public float InterestPaid { get; set; }
	}

	public class LoanCalcInterestVM : Infrastructure.Core.BaseViewModel
	{
		public float? PayAmount { get; set; }
		public DateTime? PayDate { get; set; }
		public float DailyRate { get; set; }
		public int IntForDays { get; set; }
		public float IntOnAmount { get; set; }
		public float CalcIntAmount { get; set; }
		public float TotalIntPaid { get; set; }
	}

	public class LoanPrintVM : Infrastructure.Core.BaseViewModel
	{
		public LoanAddVM LoanDetails { get; set; }
		public List<LoanPaymentVM> PaymentList { get; set; }
		public List<LoanCalcInterestVM> InterestList { get; set; }
	}
}
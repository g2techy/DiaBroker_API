using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G2.DB.BusinessObjects
{
	public class LoanAddBO : BaseBO
	{
		public int LoanID { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int BorrowerID { get; set; }
		public float PrincipalAmount { get; set; }
		public float MonthlyInterest { get; set; }
		public int Status { get; set; }
		public string RefNo { get; set; }
		public string Comments { get; set; }
	}

	public class LoanBO : BaseBO
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

	public class LoanSearchBO : PagerBO
	{
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public int BorrowerID { get; set; }
		public string RefNo { get; set; }
	}

	public class LoanSearchResultBO : PagerBO
	{
		public int RecordCount { get; private set; }
		public List<LoanBO> LoanList { get; set; }
		public LoanSearchResultBO(int recCnt)
		{
			RecordCount = recCnt;
			LoanList = new List<LoanBO>();
		}
		public LoanSearchResultBO() : this(0) { }
	}

	public class LoanPaymentAddBO
	{
		public int LoanPayID { get; set; }
		public int LoanID { get; set; }
		public DateTime PayDate { get; set; }
		public float PayAmount { get; set; }
		public int PayType { get; set; }
		public string PayComments { get; set; }
	}

	public class LoanPaymentBO
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

	public class LoanCalcInterestBO
	{
		public float? PayAmount { get; set; }
		public DateTime? PayDate { get; set; }
		public float DailyRate { get; set; }
		public int IntForDays { get; set; }
		public float IntOnAmount { get; set; }
		public float CalcIntAmount { get; set; }
		public float TotalIntPaid { get; set; }
	}
}

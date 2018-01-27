using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Contracts
{
	public interface ILoanService
	{
		int Add(BO.LoanAddBO loanAdd);
		int Delete(int userID, int loanID);
		BO.LoanAddBO GetLoanDetails(int userID, int saleID);
		List<BO.Party> GetBorrowerList(int userID, int partyTypeID, bool listAll = false);
		BO.LoanSearchResultBO GetLoanList(BO.LoanSearchBO saleSearch);
		int Close(int loanPayID);
		List<BO.LoanPaymentBO> GetPaymentList(int userID, int loanID);
		int AddPayment(BO.LoanPaymentAddBO payment);
		int DeletePayment(int loanPayID);
		List<BO.LoanCalcInterestBO> GetCalcInterest(int userID, int loanID, DateTime intAsOn);
	}
}

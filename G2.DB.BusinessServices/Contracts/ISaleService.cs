using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Contracts
{
	public interface ISaleService
	{
		int Add(BO.SaleAddBO saleAdd);
		int Delete(int userID, int saleID);
		BO.SaleAddBO GetSaleDetails(int userID, int saleID);
		List<BO.Party> GetPartyList(int userID, int partyTypeID, bool listAll = false);
		BO.SaleSearchResultBO GetSalesList(BO.SaleSearchBO saleSearch);

		List<BO.SaleBrokerageBO> GetBrokerageList(int userID, int saleID);
		int AddBrokerage(BO.SaleBrokerageAddBO brokerage);
		int UpdateBrokeragePayment(BO.SaleBrokPaymentBO brokerage);
		int DeleteBrokerage(int BDID);

		List<BO.SalePaymentBO> GetPaymentList(int userID, int saleID);
		int AddPayment(BO.SalePaymentBO payment);
		int DeletePayment(int payID);

		int CloseSale(int saleID);
	}
}

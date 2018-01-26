using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Contracts
{
	public interface IDashboardService : IService
	{
		BO.ChartDataBO GetSaleChartData(int userID);
		BO.ChartDataBO GetBrokerageChartData(int userID);
		BO.ChartDataBO GetBrokerageBistributionChartData(int userID);
		BO.SaleSearchResultBO GetDuePayments(BO.SaleSearchBO saleSearch);
		BO.ChartDataBO GetLoanInerestPaidChartData(int userID);
		BO.ChartDataBO GetLoanChartData(int userID);
	}
}

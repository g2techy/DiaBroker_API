using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BS = G2.DB.BusinessServices;
using BO = G2.DB.BusinessObjects;
using System.Data;

namespace G2.DB.Api.Controllers
{
	[Infrastructure.Filters.AuthorizationFilter(Infrastructure.Constants.UserRole.broker)]
	[RoutePrefix("api/dashboard")]
	public class DashboardController : Infrastructure.Core.BaseApiController
	{
		#region DI settings

		private readonly BS.Contracts.IDashboardService _dashboardService;
		public DashboardController(BS.Contracts.IDashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}

		#endregion

		#region Api Methods

		[HttpGet]
		[Route("chartData")]
		public IHttpActionResult ChartData(int chartType)
		{
			object _jsonData = null;
			try
			{
				var _chartList = (Infrastructure.Constants.ChartList)chartType;
				switch (_chartList)
				{
					case Infrastructure.Constants.ChartList.Last12MonthSales:
						_jsonData = Last12MonthsSaleChartData();
						break;
					case Infrastructure.Constants.ChartList.Last12MonthBrokerage:
						_jsonData = Last12MonthsBrokerageChartData();
						break;
					case Infrastructure.Constants.ChartList.BrokerageDistribution:
						_jsonData = BrokerageDistributionChartData();
						break;
					case Infrastructure.Constants.ChartList.Last12InterestPaid:
						_jsonData = Last12LoanInterestPaidChartData();
						break;
					case Infrastructure.Constants.ChartList.Last24LoanData:
						_jsonData = Last24MonthsLoanChartData();
						break;
					default:
						break;
				}
				if (_jsonData == null)
				{
					_jsonData = new
					{
						ErrorCode = 1,
						ErrorMessage = "Chart data not available."
					};
				}
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_jsonData);
		}

		[HttpPost]
		[Route("duePayments")]
		public IHttpActionResult DuePayments(Models.PagerVM pager)
		{
			if (pager == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid or empty model");
			}
			Models.SaleSearchResultVM _model = null;
			try
			{
				var _ssBO = Infrastructure.Utilities.BOVMMapper.Map<Models.SaleSearchVM, BO.SaleSearchBO>(new Models.SaleSearchVM()
				{
					UserID = base.UserID,
					StartIndex = pager.StartIndex,
					PageSize = pager.PageSize
				});
				var _ssrBO = _dashboardService.GetDuePayments(_ssBO);
				_model = Infrastructure.Utilities.BOVMMapper.Map<BO.SaleSearchResultBO, Models.SaleSearchResultVM>(_ssrBO);

				_model.StartIndex = pager.StartIndex;
				_model.PageSize = pager.PageSize;
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_model);
		}

		#endregion

		#region Private methods

		private object Last12MonthsSaleChartData()
		{
			return _dashboardService.GetSaleChartData(base.UserID);
		}
		private object Last12MonthsBrokerageChartData()
		{
			return _dashboardService.GetBrokerageChartData(base.UserID);
		}
		private object BrokerageDistributionChartData()
		{
			return _dashboardService.GetBrokerageBistributionChartData(base.UserID);
		}
		private object Last12LoanInterestPaidChartData()
		{
			return _dashboardService.GetLoanInerestPaidChartData(base.UserID);
		}
		private object Last24MonthsLoanChartData()
		{
			return _dashboardService.GetLoanChartData(base.UserID);
		}

		#endregion

	}
}

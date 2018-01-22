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
	[RoutePrefix("api/sale")]
	public class SaleController : Infrastructure.Core.BaseApiController
	{
		#region DI settings

		private readonly BS.Contracts.ISaleService _saleService;
		public SaleController(BS.Contracts.ISaleService saleService)
		{
			_saleService = saleService;
		}

		#endregion

		#region Api Methods 

		[HttpGet]
		[Route("partyList")]
		public IHttpActionResult GetPartyList(int partyTypeID)
		{
			List<Models.Party> _model = null;
			if (partyTypeID <= 0)
			{
				return BadRequest("Invalid party type");
			}
			try
			{
				var _bo = _saleService.GetPartyList(base.UserID, partyTypeID, true);
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.Party>, List<Models.Party>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpPost]
		[Route("search")]
		public IHttpActionResult Search([FromBody] Models.SaleSearchVM model)
		{
			Models.SaleSearchResultVM _result = null;

			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Please enter search details.");
			}
			try
			{
				model.UserID = base.UserID;
				var _bo = _saleService.GetSalesList(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleSearchVM, BO.SaleSearchBO>(model));
				_result = Infrastructure.Utilities.BOVMMapper.Map<BO.SaleSearchResultBO, Models.SaleSearchResultVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_result);
		}

		[HttpGet]
		[Route("sale")]
		public IHttpActionResult GetSaleDetails(int saleID)
		{
			Models.SaleAddVM _model = null;
			if (saleID <= 0)
			{
				return BadRequest("Invalid sale id.");
			}
			try
			{
				var _bo = _saleService.GetSaleDetails(base.UserID, saleID);
				_model = Infrastructure.Utilities.BOVMMapper.Map<BO.SaleAddBO, Models.SaleAddVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpPost]
		[Route("add")]
		public IHttpActionResult Add([FromBody] Models.SaleAddVM model)
		{
			int _saleID = 0;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				model.UserID = base.UserID;
				_saleID = _saleService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleAddVM, BO.SaleAddBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_saleID);
		}

		[HttpPost]
		[Route("delete")]
		public IHttpActionResult Delete(int saleID)
		{
			if (saleID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			try
			{
				_saleService.Delete(base.UserID, saleID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(saleID);
		}

		[HttpPost]
		[Route("close")]
		public IHttpActionResult Close(int saleID)
		{
			if (saleID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			try
			{
				_saleService.CloseSale(saleID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(saleID);
		}

		[HttpGet]
		[Route("payment")]
		public IHttpActionResult Payment(int saleID)
		{
			Models.SalePaymentVM _model = new Models.SalePaymentVM();
			try
			{
				var _bo = _saleService.GetPaymentList(base.UserID, saleID);
				_model.PaymentList = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SalePaymentBO>, List<Models.SalePayment>>(_bo);
				_model.Payment = new Models.SalePayment()
				{
					SaleID = saleID
				};
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpPost]
		[Route("addPayment")]
		public IHttpActionResult AddPayment([FromBody] Models.SalePayment model)
		{
			int _payID = 0;

			if (model == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				_payID = _saleService.AddPayment(Infrastructure.Utilities.BOVMMapper.Map<Models.SalePayment, BO.SalePaymentBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_payID);
		}

		[HttpPost]
		[Route("deletePayment")]
		public IHttpActionResult DeletePayment(int payID)
		{
			int _payID = 0;

			if (payID <=0 )
			{
				return BadRequest("Invalid payment ID.");
			}
			try
			{
				_payID = _saleService.DeletePayment(payID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_payID);
		}

		[HttpGet]
		[Route("brokerage")]
		public IHttpActionResult Brokerage(int saleID)
		{
			List<Models.SaleBrokerage> _model = null;

			if (saleID <= 0)
			{
				return BadRequest("Invalid saleID");
			}

			try
			{
				var _bo = _saleService.GetBrokerageList(base.UserID, saleID);
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SaleBrokerageBO>, List<Models.SaleBrokerage>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpPost]
		[Route("addBrokerage")]
		public IHttpActionResult AddBrokerage([FromBody] Models.SaleBrokerageAddVM model)
		{
			int _bdID = 0;

			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				_bdID = _saleService.AddBrokerage(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleBrokerageAddVM, BO.SaleBrokerageAddBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_bdID);
		}

		[HttpPost]
		[Route("deleteBrokerage")]
		public IHttpActionResult DeleteBrokerare(int BDID)
		{
			int _bdID = 0;
			if (BDID <= 0)
			{
				return BadRequest("Invalid BDID");
			}
			try
			{
				_bdID = _saleService.DeleteBrokerage(BDID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_bdID);
		}

		[HttpPost]
		[Route("brokPayment")]
		public IHttpActionResult UpdateBrokPayment([FromBody] Models.SaleBrokPaymentVM model)
		{
			int _bdID = 0;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model");
			}
			try
			{
				_bdID = _saleService.UpdateBrokeragePayment(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleBrokPaymentVM, BO.SaleBrokPaymentBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_bdID);
		}

		[HttpPost]
		[Route("report")]
		public IHttpActionResult GetSaleReport([FromBody] Models.SaleReportVM model)
		{
			DataTable _report = null;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model");
			}
			try
			{
				model.UserID = base.UserID;
				_report = _saleService.GetSalesReport(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleReportVM, BO.SalesReportBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_report);
		}

		[HttpPost]
		[Route("downloadReport")]
		public IHttpActionResult DownloadReport([FromBody] Models.SaleReportVM model)
		{
			DataTable _report = null;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model");
			}
			try
			{
				model.UserID = base.UserID;
				_report = _saleService.GetSalesReport(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleReportVM, BO.SalesReportBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return new Infrastructure.Core.ExcelActionResult(_report, "SalesReport.xlsx");
		}

		[HttpGet]
		[Route("statusList")]
		public IHttpActionResult GetStatusList()
		{
			List<Models.SaleStatusVM> _model = null;
			try
			{
				var _bo = _saleService.GetSaleStatusList();
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SaleStatusBO>, List<Models.SaleStatusVM>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpPost]
		[Route("brokerageReport")]
		public IHttpActionResult GetBrokerageReport([FromBody] Models.BrokerageReportVM model)
		{
			DataTable _report = null;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model");
			}
			try
			{
				model.UserID = base.UserID;
				_report = _saleService.GetBrokerageReport(Infrastructure.Utilities.BOVMMapper.Map<Models.BrokerageReportVM, BO.BrokerageReportBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_report);
		}

		[HttpPost]
		[Route("downloadBrokerageReport")]
		public IHttpActionResult DownloadBrokerageReport([FromBody] Models.BrokerageReportVM model)
		{
			DataTable _report = null;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model");
			}
			try
			{
				model.UserID = base.UserID;
				_report = _saleService.GetBrokerageReport(Infrastructure.Utilities.BOVMMapper.Map<Models.BrokerageReportVM, BO.BrokerageReportBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return new Infrastructure.Core.ExcelActionResult(_report, "BrokerageReport.xlsx");
		}

		#endregion

	}
}

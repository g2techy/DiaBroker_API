using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BS = G2.DB.BusinessServices;
using BO = G2.DB.BusinessObjects;
using System.Data;
using G2.DB.Api.Infrastructure.Filters;

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
			if (partyTypeID <= 0)
			{
				return BadRequest("Invalid party type");
			}
			var _bo = _saleService.GetPartyList(base.UserID, partyTypeID, true);
			List<Models.Party> _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.Party>, List<Models.Party>>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("search")]
		[CheckModelForNullFilter()]
		public IHttpActionResult Search([FromBody] Models.SaleSearchVM model)
		{
			model.UserID = base.UserID;
			var _bo = _saleService.GetSalesList(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleSearchVM, BO.SaleSearchBO>(model));
			Models.SaleSearchResultVM _result = Infrastructure.Utilities.BOVMMapper.Map<BO.SaleSearchResultBO, Models.SaleSearchResultVM>(_bo);
			return Ok(_result);
		}

		[HttpGet]
		[Route("sale")]
		public IHttpActionResult GetSaleDetails(int saleID)
		{
			if (saleID <= 0)
			{
				return BadRequest("Invalid sale id.");
			}
			var _bo = _saleService.GetSaleDetails(base.UserID, saleID);
			Models.SaleAddVM _model = Infrastructure.Utilities.BOVMMapper.Map<BO.SaleAddBO, Models.SaleAddVM>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("add")]
		[CheckModelForNullFilter()]
		public IHttpActionResult Add([FromBody] Models.SaleAddVM model)
		{
			model.UserID = base.UserID;
			int _saleID = _saleService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleAddVM, BO.SaleAddBO>(model));
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
			_saleService.Delete(base.UserID, saleID);
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
			_saleService.CloseSale(saleID);
			return Ok(saleID);
		}

		[HttpGet]
		[Route("payment")]
		public IHttpActionResult Payment(int saleID)
		{
			if (saleID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			Models.SalePaymentVM _model = new Models.SalePaymentVM();
			var _bo = _saleService.GetPaymentList(base.UserID, saleID);
			_model.PaymentList = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SalePaymentBO>, List<Models.SalePayment>>(_bo);
			_model.Payment = new Models.SalePayment()
			{
				SaleID = saleID
			};
			return Ok(_model);
		}

		[HttpPost]
		[Route("addPayment")]
		[CheckModelForNullFilter()]
		public IHttpActionResult AddPayment([FromBody] Models.SalePayment model)
		{
			int _payID = _saleService.AddPayment(Infrastructure.Utilities.BOVMMapper.Map<Models.SalePayment, BO.SalePaymentBO>(model));
			return Ok(_payID);
		}

		[HttpPost]
		[Route("deletePayment")]
		public IHttpActionResult DeletePayment(int payID)
		{
			if (payID <= 0)
			{
				return BadRequest("Invalid payment ID.");
			}
			int _payID = _saleService.DeletePayment(payID);
			return Ok(_payID);
		}

		[HttpGet]
		[Route("brokerage")]
		public IHttpActionResult Brokerage(int saleID)
		{
			if (saleID <= 0)
			{
				return BadRequest("Invalid saleID");
			}
			var _bo = _saleService.GetBrokerageList(base.UserID, saleID);
			var _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SaleBrokerageBO>, List<Models.SaleBrokerage>>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("addBrokerage")]
		[CheckModelForNullFilter()]
		public IHttpActionResult AddBrokerage([FromBody] Models.SaleBrokerageAddVM model)
		{
			var _bdID = _saleService.AddBrokerage(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleBrokerageAddVM, BO.SaleBrokerageAddBO>(model));
			return Ok(_bdID);
		}

		[HttpPost]
		[Route("deleteBrokerage")]
		public IHttpActionResult DeleteBrokerare(int BDID)
		{
			if (BDID <= 0)
			{
				return BadRequest("Invalid BDID");
			}
			var _bdID = _saleService.DeleteBrokerage(BDID);
			return Ok(_bdID);
		}

		[HttpPost]
		[Route("brokPayment")]
		[CheckModelForNullFilter()]
		public IHttpActionResult UpdateBrokPayment([FromBody] Models.SaleBrokPaymentVM model)
		{
			var _bdID = _saleService.UpdateBrokeragePayment(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleBrokPaymentVM, BO.SaleBrokPaymentBO>(model));
			return Ok(_bdID);
		}

		[HttpPost]
		[Route("report")]
		[CheckModelForNullFilter()]
		public IHttpActionResult GetSaleReport([FromBody] Models.SaleReportVM model)
		{
			model.UserID = base.UserID;
			var _report = _saleService.GetSalesReport(Infrastructure.Utilities.BOVMMapper.Map<Models.SaleReportVM, BO.SalesReportBO>(model));
			return Ok(_report);
		}

		[HttpPost]
		[Route("downloadReport")]
		[CheckModelForNullFilter()]
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
			var _bo = _saleService.GetSaleStatusList();
			var _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.SaleStatusBO>, List<Models.SaleStatusVM>>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("brokerageReport")]
		[CheckModelForNullFilter()]
		public IHttpActionResult GetBrokerageReport([FromBody] Models.BrokerageReportVM model)
		{
			model.UserID = base.UserID;
			var _report = _saleService.GetBrokerageReport(Infrastructure.Utilities.BOVMMapper.Map<Models.BrokerageReportVM, BO.BrokerageReportBO>(model));
			return Ok(_report);
		}

		[HttpPost]
		[Route("downloadBrokerageReport")]
		[CheckModelForNullFilter()]
		public IHttpActionResult DownloadBrokerageReport([FromBody] Models.BrokerageReportVM model)
		{
			model.UserID = base.UserID;
			var _report = _saleService.GetBrokerageReport(Infrastructure.Utilities.BOVMMapper.Map<Models.BrokerageReportVM, BO.BrokerageReportBO>(model));
			return new Infrastructure.Core.ExcelActionResult(_report, "BrokerageReport.xlsx");
		}

		#endregion

	}
}

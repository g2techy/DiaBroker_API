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
	[RoutePrefix("api/loan")]
	public class LoanController : Infrastructure.Core.BaseApiController
    {
		#region DI settings

		private readonly BS.Contracts.ILoanService _loanService;
		public LoanController(BS.Contracts.ILoanService loanService)
		{
			_loanService = loanService;
		}

		#endregion


		#region API Methods

		[HttpGet]
		[Route("borrowerList")]
		public IHttpActionResult GetBorrowerList(int partyTypeID)
		{
			if (partyTypeID <= 0)
			{
				return BadRequest("Invalid party type");
			}
			var _bo = _loanService.GetBorrowerList(base.UserID, partyTypeID, true);
			var _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.Party>, List<Models.Party>>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("search")]
		[CheckModelForNullFilter()]
		public IHttpActionResult Search([FromBody] Models.LoanSearchVM model)
		{
			model.UserID = base.UserID;
			var _bo = _loanService.GetLoanList(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanSearchVM, BO.LoanSearchBO>(model));
			var _result = Infrastructure.Utilities.BOVMMapper.Map<BO.LoanSearchResultBO, Models.LoanSearchResultVM>(_bo);
			return Ok(_result);
		}

		[HttpGet]
		[Route("loan")]
		public IHttpActionResult GetLoanDetails(int loanID)
		{
			if (loanID <= 0)
			{
				return BadRequest("Invalid loan id.");
			}
			var _bo = _loanService.GetLoanDetails(base.UserID, loanID);
			var _model = Infrastructure.Utilities.BOVMMapper.Map<BO.LoanAddBO, Models.LoanAddVM>(_bo);
			_model.LoanID = loanID;
			return Ok(_model);
		}

		[HttpPost]
		[Route("add")]
		[CheckModelForNullFilter()]
		public IHttpActionResult Add([FromBody] Models.LoanAddVM model)
		{
			model.UserID = base.UserID;
			var _loanID = _loanService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanAddVM, BO.LoanAddBO>(model));
			return Ok(_loanID);
		}

		[HttpPost]
		[Route("delete")]
		public IHttpActionResult Delete(int loanID)
		{
			if (loanID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			_loanService.Delete(base.UserID, loanID);
			return Ok(loanID);
		}

		[HttpGet]
		[Route("payment")]
		public IHttpActionResult Payment(int loanID)
		{
			if (loanID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			var _bo = _loanService.GetPaymentList(base.UserID, loanID);
			var _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.LoanPaymentBO>, List<Models.LoanPaymentVM>>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("addPayment")]
		[CheckModelForNullFilter()]
		public IHttpActionResult AddPayment([FromBody] Models.LoanPaymentAddVM model)
		{
			var _payID = _loanService.AddPayment(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanPaymentAddVM, BO.LoanPaymentAddBO>(model));
			return Ok(_payID);
		}

		[HttpPost]
		[Route("deletePayment")]
		public IHttpActionResult DeletePayment(int payID)
		{
			var _payID = _loanService.DeletePayment(payID);
			return Ok(_payID);
		}

		[HttpPost]
		[Route("calcInterest")]
		public IHttpActionResult CalcInterest(int loanID, string intAsOn)
		{
			if (loanID <= 0)
			{
				return BadRequest("Invalid payment ID.");
			}
			DateTime _intAsOn = DateTime.Now;
			if (!string.IsNullOrEmpty(intAsOn))
			{
				try
				{
					if (DateTime.TryParse(intAsOn, out _intAsOn))
					{
						_intAsOn.AddDays(1);
					}
				}
				catch { _intAsOn = DateTime.Now; }
			}
			var _bo = _loanService.GetCalcInterest(this.UserID, loanID, _intAsOn);
			var _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.LoanCalcInterestBO>, List<Models.LoanCalcInterestVM>>(_bo);
			return Ok(_model);
		}

		#endregion
	}
}

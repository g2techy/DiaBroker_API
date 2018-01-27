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
			List<Models.Party> _model = null;
			if (partyTypeID <= 0)
			{
				return BadRequest("Invalid party type");
			}
			try
			{
				var _bo = _loanService.GetBorrowerList(base.UserID, partyTypeID, true);
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
		public IHttpActionResult Search([FromBody] Models.LoanSearchVM model)
		{
			Models.LoanSearchResultVM _result = null;

			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Please enter search details.");
			}
			try
			{
				model.UserID = base.UserID;
				var _bo = _loanService.GetLoanList(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanSearchVM, BO.LoanSearchBO>(model));
				_result = Infrastructure.Utilities.BOVMMapper.Map<BO.LoanSearchResultBO, Models.LoanSearchResultVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_result);
		}

		[HttpGet]
		[Route("loan")]
		public IHttpActionResult GetLoanDetails(int loanID)
		{
			Models.LoanAddVM _model = null;
			if (loanID <= 0)
			{
				return BadRequest("Invalid loan id.");
			}
			try
			{
				var _bo = _loanService.GetLoanDetails(base.UserID, loanID);
				_model = Infrastructure.Utilities.BOVMMapper.Map<BO.LoanAddBO, Models.LoanAddVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			_model.LoanID = loanID;
			return Ok(_model);
		}

		[HttpPost]
		[Route("add")]
		public IHttpActionResult Add([FromBody] Models.LoanAddVM model)
		{
			int _loanID = 0;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				model.UserID = base.UserID;
				_loanID = _loanService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanAddVM, BO.LoanAddBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
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
			try
			{
				_loanService.Delete(base.UserID, loanID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(loanID);
		}

		[HttpGet]
		[Route("payment")]
		public IHttpActionResult Payment(int loanID)
		{
			List<Models.LoanPaymentVM> _model = null;
			try
			{
				var _bo = _loanService.GetPaymentList(base.UserID, loanID);
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.LoanPaymentBO>, List<Models.LoanPaymentVM>>(_bo);
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
		public IHttpActionResult AddPayment([FromBody] Models.LoanPaymentAddVM model)
		{
			int _payID = 0;

			if (model == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				_payID = _loanService.AddPayment(Infrastructure.Utilities.BOVMMapper.Map<Models.LoanPaymentAddVM, BO.LoanPaymentAddBO>(model));
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

			if (payID <= 0)
			{
				return BadRequest("Invalid payment ID.");
			}
			try
			{
				_payID = _loanService.DeletePayment(payID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_payID);
		}

		[HttpPost]
		[Route("calcInterest")]
		public IHttpActionResult CalcInterest(int loanID, string intAsOn)
		{
			List<Models.LoanCalcInterestVM> _model = null;
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
			try
			{
				var _bo = _loanService.GetCalcInterest(this.UserID, loanID, _intAsOn);
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.LoanCalcInterestBO>, List<Models.LoanCalcInterestVM>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		#endregion
	}
}

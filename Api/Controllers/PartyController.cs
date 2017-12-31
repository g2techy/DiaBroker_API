using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BS = G2.DB.BusinessServices;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.Api.Controllers
{
	[RoutePrefix("api/party")]
	[System.Web.Http.Cors.EnableCors("*", "*", "*")]
	public class PartyController : Infrastructure.Core.BaseApiController
	{
		#region DI settings

		private readonly BS.Contracts.IPartyService _partyService;
		public PartyController(BS.Contracts.IPartyService partyService)
		{
			_partyService = partyService;
		}

		#endregion

		#region API Methods

		[HttpPost]
		[Route("search")]
		public IHttpActionResult Search([FromBody] Models.PartySearchVM model)
		{
			Models.PartySearchResultVM _result = null;

			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Please enter search details.");
			}
			try
			{
				var _bo = _partyService.GetPartyList(Infrastructure.Utilities.BOVMMapper.Map<Models.PartySearchVM, BO.PartySearchBO>(model));
				_result = Infrastructure.Utilities.BOVMMapper.Map<BO.PartySearchResultBO, Models.PartySearchResultVM>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_result);
		}

		[HttpGet]
		[Route("partyTypes")]
		public IHttpActionResult GetPartyTypes()
		{
			List<Models.PartyTypeVM> _model = new List<Models.PartyTypeVM>();
			try
			{
				var _bo = _partyService.GetPartyTypeList();
				_model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.PartyTypeBO>, List<Models.PartyTypeVM>>(_bo);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(_model);
		}

		[HttpGet]
		[Route("party")]
		public IHttpActionResult GetPartyDetails(int partyID)
		{
			Models.PartyVM _model = null;
			if (partyID <= 0)
			{
				return BadRequest("Invalid party id.");
			}
			try
			{
				var _bo = _partyService.GetPartyDetails(base.UserID, partyID);
				_model = Infrastructure.Utilities.BOVMMapper.Map<BO.PartyBO, Models.PartyVM>(_bo);
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
		public IHttpActionResult AddParty([FromBody]Models.PartyVM model)
		{
			int _partyID = 0;
			if (!ModelState.IsValid || model == null)
			{
				return BadRequest("Invalid or empty model.");
			}
			try
			{
				model.UserID = base.UserID;
				_partyID = _partyService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.PartyVM, BO.PartyBO>(model));
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}

			return Ok(_partyID);
		}

		[HttpPost]
		[Route("delete")]
		public IHttpActionResult DeleteParty(int partyID)
		{
			if (partyID <= 0)
			{
				return BadRequest("Invalid parameters.");
			}
			try
			{
				_partyService.Delete(base.UserID, partyID);
			}
			catch (Exception ex)
			{
				base.LogException(ex);
				return BadRequest(ex.Message);
			}
			return Ok(partyID);
		}
		#endregion
	}
}

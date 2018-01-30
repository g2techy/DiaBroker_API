using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BS = G2.DB.BusinessServices;
using BO = G2.DB.BusinessObjects;
using G2.DB.Api.Infrastructure.Filters;

namespace G2.DB.Api.Controllers
{
	[Infrastructure.Filters.AuthorizationFilter(Infrastructure.Constants.UserRole.broker)]
	[RoutePrefix("api/party")]
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
		[CheckModelForNullFilter()]
		public IHttpActionResult Search([FromBody] Models.PartySearchVM model)
		{
			var _bo = _partyService.GetPartyList(Infrastructure.Utilities.BOVMMapper.Map<Models.PartySearchVM, BO.PartySearchBO>(model));
			Models.PartySearchResultVM _result = Infrastructure.Utilities.BOVMMapper.Map<BO.PartySearchResultBO, Models.PartySearchResultVM>(_bo);
			return Ok(_result);
		}

		[HttpGet]
		[Route("partyTypes")]
		public IHttpActionResult GetPartyTypes()
		{
			var _bo = _partyService.GetPartyTypeList();
			List<Models.PartyTypeVM> _model = Infrastructure.Utilities.BOVMMapper.Map<List<BO.PartyTypeBO>, List<Models.PartyTypeVM>>(_bo);
			return Ok(_model);
		}

		[HttpGet]
		[Route("party")]
		public IHttpActionResult GetPartyDetails(int partyID)
		{
			if (partyID <= 0)
			{
				return BadRequest("Invalid party id.");
			}
			var _bo = _partyService.GetPartyDetails(base.UserID, partyID);
			Models.PartyVM _model = Infrastructure.Utilities.BOVMMapper.Map<BO.PartyBO, Models.PartyVM>(_bo);
			return Ok(_model);
		}

		[HttpPost]
		[Route("add")]
		[CheckModelForNullFilter()]
		public IHttpActionResult AddParty([FromBody]Models.PartyVM model)
		{
			model.UserID = base.UserID;
			int _partyID = _partyService.Add(Infrastructure.Utilities.BOVMMapper.Map<Models.PartyVM, BO.PartyBO>(model));
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
			_partyService.Delete(base.UserID, partyID);
			return Ok(partyID);
		}
		#endregion
	}
}

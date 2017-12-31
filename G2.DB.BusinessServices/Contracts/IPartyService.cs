using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Contracts
{
	public interface IPartyService : IService
	{
		List<BO.PartyTypeBO> GetPartyTypeList();
		int Add(BO.PartyBO Party);
		int Delete(int clientID, int PartyID);
		BO.PartyBO GetPartyDetails(int userID, int PartyID);
		BO.PartySearchResultBO GetPartyList(BO.PartySearchBO PartySearch);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.Api.Infrastructure
{
	public class AutoMapperConfiguration
	{
		public static void Configure(AutoMapper.IMapperConfigurationExpression config)
		{
			config.CreateMap<Models.LoginVM, BO.LoginBO>().ReverseMap();
			config.CreateMap<Models.UserRoleVM, BO.UserRoleBO>().ReverseMap();
			config.CreateMap<Models.LoggedInUserVM, BO.LoggedInUserBO>().ReverseMap();
			config.CreateMap<Models.UserVM, BO.UserBO>().ReverseMap();

			config.CreateMap<Models.PartyVM, BO.PartyBO>().ReverseMap();
			config.CreateMap<Models.PagerVM, BO.PagerBO>().ReverseMap();
			config.CreateMap<Models.PartyTypeVM, BO.PartyTypeBO>().ReverseMap();
			config.CreateMap<Models.PartySearchVM, BO.PartySearchBO>().ReverseMap();
			config.CreateMap<Models.PartySearchResultVM, BO.PartySearchResultBO>().ReverseMap();

		}
	}
}
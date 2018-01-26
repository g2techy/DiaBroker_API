using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G2.DB.Api.Infrastructure.Constants
{

	public enum UserRole
	{
		Admin = 1,
		broker = 2
	}

	public enum ChartList
	{
		Last12MonthSales = 1,
		Last12MonthBrokerage,
		BrokerageDistribution,
		Last12InterestPaid,
		Last24LoanData
	}

	public static class Default
	{
		public const int PageSize = 10;
	}

}
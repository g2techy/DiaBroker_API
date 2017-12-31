using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace G2.DB.Api.Infrastructure.Utilities
{
	public class BOVMMapper
	{
		public static TDestination Map<TSource, TDestination>(TSource obj)
		{
			return AutoMapper.Mapper.Map<TDestination>(obj);
		}
	}
}
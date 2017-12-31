using System;
using System.Collections.Generic;
using System.Text;

namespace G2.DB.BusinessServices
{
	public abstract class BaseService : IService
	{
		private DAL.IDatabaseAccess _dbAccess = null;

		public DAL.IDatabaseAccess DatabaseAccess
		{
			get
			{
				if (_dbAccess == null)
				{
					_dbAccess = (new DAL.DefaultDatabaseAccessFactory()).GetAppDatabase();
				}
				return _dbAccess;
			}
		}

		public static DAL.IDatabaseAccess NewDatabaseAccess
		{
			get
			{
				return (new DAL.DefaultDatabaseAccessFactory()).GetAppDatabase();
			}
		}

		public BaseService()
		{

		}
				
	}
}

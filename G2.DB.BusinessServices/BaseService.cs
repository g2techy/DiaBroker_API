using System;
using System.Collections.Generic;
using System.Text;

namespace G2.DB.BusinessServices
{
	public abstract class BaseService : IService
	{
		public static string DefaultDateFormat = "MM/dd/yyyy";

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

		public string GetDateIntoString(string date)
		{
			string _returnVal = string.Empty;
			DateTime _dt;
			if (DateTime.TryParse(date, out _dt))
			{
				if (_dt.Year != 0001)
				{
					_returnVal = _dt.ToString(BaseService.DefaultDateFormat);
				}
			}
			return _returnVal;
		}
	}
}

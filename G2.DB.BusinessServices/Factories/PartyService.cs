using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Factories
{
	public class PartyService : BaseService, Contracts.IPartyService
	{
		#region IPartyRepository Members

		public List<BO.PartyTypeBO> GetPartyTypeList()
		{
			List<BO.PartyTypeBO> _list = new List<BO.PartyTypeBO>();

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Buyer_BuyerTypeList", null))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow _dr in _ds.Tables[0].Rows)
						{
							_list.Add(new BO.PartyTypeBO()
							{
								PartyTypeID = int.Parse(_dr["BuyerTypeID"].ToString()),
								PartyTypeName = _dr["BuyerTypeName"].ToString()
							});
						}
					}

				}
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}

			return _list;
		}

		public int Add(BO.PartyBO Party)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Buyer_AddUpdate", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BuyerID",DAL.ParameterDirection.InOut, DAL.DataType.Int, Party.PartyID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, Party.UserID),
					new DAL.DatabaseParameter("@BuyerCode",DAL.ParameterDirection.In,DAL.DataType.String, Party.PartyCode, 20),
					new DAL.DatabaseParameter("@FirstName",DAL.ParameterDirection.In,DAL.DataType.String, Party.FirstName, 100),
					new DAL.DatabaseParameter("@LastName",DAL.ParameterDirection.In,DAL.DataType.String, Party.LastName, 100),
					new DAL.DatabaseParameter("@PhoneNo",DAL.ParameterDirection.In,DAL.DataType.String, Party.PhoneNo),
					new DAL.DatabaseParameter("@MobileNo",DAL.ParameterDirection.In,DAL.DataType.String, Party.MobileNo),
					new DAL.DatabaseParameter("@BuyerTypes",DAL.ParameterDirection.In,DAL.DataType.String, string.Join(",",Party.SelectedPartyTypes), 100)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@BuyerID"]);
				_ourParams = null;
			}
			catch
			{
				DatabaseAccess.RollbackTransaction();
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
			return _ID;
		}

		public int Delete(int userID, int PartyID)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Buyer_Delete", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BuyerID",DAL.ParameterDirection.In, DAL.DataType.Int, PartyID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				});
				DatabaseAccess.CommitTransaction();
				_ID = PartyID;
				_ourParams = null;
			}
			catch
			{
				DatabaseAccess.RollbackTransaction();
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}
			return _ID;
		}

		public BO.PartyBO GetPartyDetails(int userID, int PartyID)
		{
			BO.PartyBO _Party = null;
			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Buyer_GetDetails", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BuyerID",DAL.ParameterDirection.In, DAL.DataType.Int, PartyID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0)
					{
						var _row = _ds.Tables[0].Rows[0];
						_Party = new BO.PartyBO()
						{
							PartyID = Convert.ToInt32(_row["BuyerID"]),
							PartyCode = Convert.ToString(_row["BuyerCode"]),
							FirstName = Convert.ToString(_row["FirstName"]),
							LastName = Convert.ToString(_row["LastName"]),
							PhoneNo = Convert.ToString(_row["PhoneNo"]),
							MobileNo = Convert.ToString(_row["MobileNo"])
						};
						string _PartyTypeList = Convert.ToString(_row["BuyerTypes"]);
						if (!string.IsNullOrEmpty(_PartyTypeList))
						{
							_Party.SelectedPartyTypes = _PartyTypeList.Split(',').ToList();
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}

			return _Party;
		}

		public BO.PartySearchResultBO GetPartyList(BO.PartySearchBO PartySearch)
		{
			BO.PartySearchResultBO _returnVal = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Buyer_GetBuyerList", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, PartySearch.UserID),
					new DAL.DatabaseParameter("@StartIndex",DAL.ParameterDirection.In, DAL.DataType.Int, PartySearch.StartIndex),
					new DAL.DatabaseParameter("@PageSize",DAL.ParameterDirection.In, DAL.DataType.Int, PartySearch.PageSize),
					new DAL.DatabaseParameter("@BuyerCode",DAL.ParameterDirection.In, DAL.DataType.String, PartySearch.PartyCode, 20),
					new DAL.DatabaseParameter("@FirstName",DAL.ParameterDirection.In, DAL.DataType.String, PartySearch.FirstName, 100),
					new DAL.DatabaseParameter("@LastName",DAL.ParameterDirection.In, DAL.DataType.String, PartySearch.LastName, 100)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0)
					{
						int _totalRecords = 0;
						if (_ds.Tables[0].Rows.Count > 0)
						{
							_totalRecords = int.Parse(_ds.Tables[0].Rows[0]["Row_Count"].ToString());
						}

						_returnVal = new BO.PartySearchResultBO(_totalRecords);

						foreach (DataRow _row in _ds.Tables[0].Rows)
						{
							var _PartyBM = new BO.PartyBO()
							{
								PartyID = Convert.ToInt32(_row["BuyerID"]),
								PartyCode = Convert.ToString(_row["BuyerCode"]),
								FirstName = Convert.ToString(_row["FirstName"]),
								LastName = Convert.ToString(_row["LastName"]),
								PhoneNo = Convert.ToString(_row["PhoneNo"]),
								MobileNo = Convert.ToString(_row["MobileNo"]),
								SelectedPartyTypes = new List<string>() { Convert.ToString(_row["BuyerTypeNames"]) }
							};
							_returnVal.PartyList.Add(_PartyBM);
						}

					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				DatabaseAccess.CloseConnection();
			}

			return _returnVal;
		}

		#endregion
	}
}

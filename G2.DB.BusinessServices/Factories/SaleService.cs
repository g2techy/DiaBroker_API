using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Factories
{
	public class SaleService : BaseService , Contracts.ISaleService
	{
		#region ISaleRepository Members

		public int Add(BO.SaleAddBO saleAdd)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_AddUpdate", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In, DAL.DataType.Int, saleAdd.UserID),
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.InOut,DAL.DataType.Int, saleAdd.SaleID),
					new DAL.DatabaseParameter("@DueDays",DAL.ParameterDirection.In,DAL.DataType.Int, saleAdd.DueDays),
					new DAL.DatabaseParameter("@SaleDate",DAL.ParameterDirection.In,DAL.DataType.Date, saleAdd.SaleDate),
					new DAL.DatabaseParameter("@Saller",DAL.ParameterDirection.In,DAL.DataType.Int, saleAdd.SallerID),
					new DAL.DatabaseParameter("@Buyer",DAL.ParameterDirection.In,DAL.DataType.Int, saleAdd.BuyerID),
					new DAL.DatabaseParameter("@TotalWeight",DAL.ParameterDirection.In,DAL.DataType.Decimal, saleAdd.TotalWeight),
					new DAL.DatabaseParameter("@RejectionWeight",DAL.ParameterDirection.In,DAL.DataType.Decimal, saleAdd.RejectionWeight),
					new DAL.DatabaseParameter("@UnitPrice",DAL.ParameterDirection.In,DAL.DataType.Decimal, saleAdd.UnitPrice),
					new DAL.DatabaseParameter("@LessPer",DAL.ParameterDirection.In,DAL.DataType.Decimal, saleAdd.LessPer)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@SaleID"]);
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

		public int Delete(int userID, int saleID)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_Delete", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, saleID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				});
				DatabaseAccess.CommitTransaction();
				_ID = saleID;
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

		public BO.SaleAddBO GetSaleDetails(int userID, int saleID)
		{
			BO.SaleAddBO _saleAdd = null;
			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_GetDetails", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, saleID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0)
					{
						var _row = _ds.Tables[0].Rows[0];
						_saleAdd = new BO.SaleAddBO()
						{
							SaleDate = Convert.ToDateTime(_row["SaleDate"]).ToString(BaseService.DefaultDateFormat),
							SallerID = Convert.ToInt32(_row["SallerID"]),
							BuyerID = Convert.ToInt32(_row["BuyerID"]),
							DueDays = Convert.ToInt32(_row["DueDays"]),
							TotalWeight = float.Parse(_row["Weight"].ToString()),
							RejectionWeight = float.Parse(_row["RejectionWt"].ToString()),
							UnitPrice = float.Parse(_row["UnitPrice"].ToString()),
							LessPer = float.Parse(_row["LessPer"].ToString()),
							Status = Convert.ToInt32(_row["Status"])
						};
						_saleAdd.SaleID = saleID;
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

			return _saleAdd;
		}

		public List<BO.Party> GetPartyList(int userID, int partyTypeID, bool listAll = false)
		{
			List<BO.Party> _buyerList = new List<BO.Party>();

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_GetBuyerList", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BuyerTypeID",DAL.ParameterDirection.In, DAL.DataType.Int, partyTypeID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID),
					new DAL.DatabaseParameter("@ListAll",DAL.ParameterDirection.In, DAL.DataType.Bit, (listAll ? 1 : 0))
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						foreach (DataRow _row in _ds.Tables[0].Rows)
						{
							_buyerList.Add(new BO.Party()
							{
								PartyID = Convert.ToInt32(_row["BuyerID"]),
								PartyName = Convert.ToString(_row["BuyerName"])
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

			return _buyerList;
		}

		public BO.SaleSearchResultBO GetSalesList(BO.SaleSearchBO saleSearch)
		{
			BO.SaleSearchResultBO _returnVal = null;
			try
			{
				DatabaseAccess.OpenConnection();
				var _paramList = new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In, DAL.DataType.Int, saleSearch.UserID),
					new DAL.DatabaseParameter("@StartIndex",DAL.ParameterDirection.In, DAL.DataType.Int, saleSearch.StartIndex),
					new DAL.DatabaseParameter("@PageSize",DAL.ParameterDirection.In, DAL.DataType.Int, saleSearch.PageSize)
				};
				string _stDate = GetDateIntoString(saleSearch.StartDate);
				if (!string.IsNullOrEmpty(saleSearch.StartDate))
				{
					_paramList.Add(new DAL.DatabaseParameter("@StartDate", DAL.ParameterDirection.In, DAL.DataType.String, _stDate));
				}
				string _endDate = GetDateIntoString(saleSearch.EndDate);
				if (!string.IsNullOrEmpty(saleSearch.EndDate))
				{
					_paramList.Add(new DAL.DatabaseParameter("@EndDate", DAL.ParameterDirection.In, DAL.DataType.String, _endDate));
				}
				if (saleSearch.SallerID > 0)
				{
					_paramList.Add(new DAL.DatabaseParameter("@SallerID", DAL.ParameterDirection.In, DAL.DataType.Int, saleSearch.SallerID));
				}
				if (saleSearch.BuyerID > 0)
				{
					_paramList.Add(new DAL.DatabaseParameter("@BuyerID", DAL.ParameterDirection.In, DAL.DataType.Int, saleSearch.BuyerID));
				}
				if (!string.IsNullOrEmpty(saleSearch.RefNo))
				{
					_paramList.Add(new DAL.DatabaseParameter("@RefNo", DAL.ParameterDirection.In, DAL.DataType.String, saleSearch.RefNo));
				}

				DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_GetSalesList", _paramList);
				int _totalRecords = 0;
				if (_ds != null && _ds.Tables.Count > 0)
				{
					if (_ds.Tables[0].Rows.Count > 0)
					{
						_totalRecords = int.Parse(_ds.Tables[0].Rows[0]["Row_Count"].ToString());
					}

					_returnVal = new BO.SaleSearchResultBO(_totalRecords);

					foreach (DataRow _dr in _ds.Tables[0].Rows)
					{
						var _saleBM = new BO.SaleBO()
						{
							SaleID = int.Parse(_dr["SaleID"].ToString()),
							SaleDate = DateTime.Parse(_dr["SaleDate"].ToString()),
							Saller = _dr["Saller"].ToString(),
							Buyer = _dr["Buyer"].ToString(),
							TotalWeight = float.Parse(_dr["TotalWeight"].ToString()),
							RejectionWt = float.Parse(_dr["RejectionWt"].ToString()),
							SelectionWt = float.Parse(_dr["SelectionWt"].ToString()),
							UnitPrice = float.Parse(_dr["UnitPrice"].ToString()),
							LessPer = float.Parse(_dr["LessPer"].ToString()),
							NetSaleAmount = float.Parse(_dr["NetSaleAmount"].ToString()),
							DueDays = int.Parse(_dr["DueDays"].ToString()),
							TotalBrokerage = _dr["TotalBrokerage"].ToString(),
							Status = _dr["Status"].ToString(),
							RefNo = _dr["RefNo"].ToString(),
							DueDate = DateTime.Parse(_dr["DueDate"].ToString()),
						};

						if (_dr["TotalPayAmount"] != System.DBNull.Value)
						{
							_saleBM.TotalPayAmount = float.Parse(_dr["TotalPayAmount"].ToString());
						}
						if (_dr["PayDate"] != System.DBNull.Value)
						{
							_saleBM.PayDate = DateTime.Parse(_dr["PayDate"].ToString());
						}
						_returnVal.SalesList.Add(_saleBM);
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

		public List<BO.SaleBrokerageBO> GetBrokerageList(int userID, int saleID)
		{
			List<BO.SaleBrokerageBO> _returnVal = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_GetBrokerList", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, saleID),
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_returnVal = new List<BO.SaleBrokerageBO>();
						foreach (DataRow _row in _ds.Tables[0].Rows)
						{
							_returnVal.Add(new BO.SaleBrokerageBO()
							{
								BDID = Convert.ToInt32(_row["BDID"]),
								SaleID = Convert.ToInt32(_row["SaleID"]),
								BrokerID = Convert.ToInt32(_row["BrokerID"]),
								BrokerName = Convert.ToString(_row["BrokerName"]),
								Brokerage = float.Parse(_row["Brokerage"].ToString()),
								BrokerageAmount = float.Parse(_row["BrokerageAmount"].ToString()),
								IsPaid = (Convert.ToInt32(_row["IsPaid"]) == 1),
								PayDate = Convert.ToDateTime(_row["PayDate"]),
								PayComments = Convert.ToString(_row["PayComments"])
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

			return _returnVal;
		}

		public int AddBrokerage(BO.SaleBrokerageAddBO brokerage)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_AddBrokerage", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BDID",DAL.ParameterDirection.InOut, DAL.DataType.Int,brokerage.BDID),
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, brokerage.SaleID),
					new DAL.DatabaseParameter("@BrokerID",DAL.ParameterDirection.In,DAL.DataType.Int, brokerage.BrokerID),
					new DAL.DatabaseParameter("@Brokerage",DAL.ParameterDirection.In,DAL.DataType.Decimal, brokerage.Brokerage)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@BDID"]);
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

		public int UpdateBrokeragePayment(BO.SaleBrokPaymentBO brokerage)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_BrokeragePayment", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BDID",DAL.ParameterDirection.In, DAL.DataType.Int,brokerage.BDID),
					new DAL.DatabaseParameter("@PayDate",DAL.ParameterDirection.In,DAL.DataType.Date, brokerage.PayDate),
					new DAL.DatabaseParameter("@PayComments",DAL.ParameterDirection.In,DAL.DataType.String, brokerage.PayComments, 1000)
				});
				DatabaseAccess.CommitTransaction();
				_ID = brokerage.BDID;
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

		public int DeleteBrokerage(int BDID)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_DeleteBrokerage", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@BDID",DAL.ParameterDirection.In, DAL.DataType.Int, BDID),
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.Out, DAL.DataType.Int)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@SaleID"]);
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

		public List<BO.SalePaymentBO> GetPaymentList(int userID, int saleID)
		{
			List<BO.SalePaymentBO> _returnVal = null;

			try
			{
				DatabaseAccess.OpenConnection();
				var _paramList = new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In, DAL.DataType.Int, userID),
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, saleID),
				};

				DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_GetPaymentList", _paramList);
				if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
				{
					_returnVal = new List<BO.SalePaymentBO>();
					foreach (DataRow _dr in _ds.Tables[0].Rows)
					{
						_returnVal.Add(new BO.SalePaymentBO()
						{
							PayID = int.Parse(_dr["PayID"].ToString()),
							PayDate = DateTime.Parse(_dr["PayDate"].ToString()),
							PayAmount = float.Parse(_dr["PayAmount"].ToString()),
							CourierFrom = _dr["PayCourierFrom"].ToString(),
							CourierTo = _dr["PayCourierTo"].ToString()
						});
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

		public int AddPayment(BO.SalePaymentBO payment)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_AddPayment", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@PayID", DAL.ParameterDirection.InOut, DAL.DataType.Int, payment.PayID),
					new DAL.DatabaseParameter("@SaleID", DAL.ParameterDirection.In, DAL.DataType.Int, payment.SaleID),
					new DAL.DatabaseParameter("@PayDate", DAL.ParameterDirection.In, DAL.DataType.DateTime, payment.PayDate),
					new DAL.DatabaseParameter("@PayAmount", DAL.ParameterDirection.In, DAL.DataType.Decimal, payment.PayAmount),
					new DAL.DatabaseParameter("@CourierFrom", DAL.ParameterDirection.In, DAL.DataType.String, payment.CourierFrom, 100),
					new DAL.DatabaseParameter("@CourierTo", DAL.ParameterDirection.In, DAL.DataType.String, payment.CourierTo, 100)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@PayID"]);
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

		public int DeletePayment(int payID)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_DeletePayment", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@PayID",DAL.ParameterDirection.In, DAL.DataType.Int, payID),
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.Out, DAL.DataType.Int)
				});
				DatabaseAccess.CommitTransaction();
				_ID = Convert.ToInt32(_ourParams["@SaleID"]);
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

		public int CloseSale(int saleID)
		{
			int _ID;
			try
			{
				DatabaseAccess.OpenConnection(true);
				var _ourParams = DatabaseAccess.ExecuteProcedureDML("P_Sale_UpdateStatus", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@SaleID",DAL.ParameterDirection.In, DAL.DataType.Int, saleID),
					new DAL.DatabaseParameter("@Status",DAL.ParameterDirection.In, DAL.DataType.Int, 4)
				});
				DatabaseAccess.CommitTransaction();
				_ID = saleID;
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

		#endregion
	}
}

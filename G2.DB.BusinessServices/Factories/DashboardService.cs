using System;
using System.Collections.Generic;
using System.Data;
using BO = G2.DB.BusinessObjects;

namespace G2.DB.BusinessServices.Factories
{
	public class DashboardService : BaseService, Contracts.IDashboardService
	{
		public BO.ChartDataBO GetBrokerageChartData(int userID)
		{
			BO.ChartDataBO _chartData = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_BrokerageChartData", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_chartData = new BO.ChartDataBO();
						var _selfSeries = new BO.ChartSeriesBO()
						{
							Name = "Brokerage (Self)",
							Data = new List<object>()
						};
						var _otherSeries = new BO.ChartSeriesBO()
						{
							Name = "Brokerage (Others)",
							Data = new List<object>()
						};
						foreach (DataRow _dr in _ds.Tables[0].Rows)
						{
							_chartData.Categories.Add(Convert.ToString(_dr["Category"]));
							_selfSeries.Data.Add(_dr["BrokerageSelf"]);
							_otherSeries.Data.Add(_dr["BrokerageOthers"]);
						}
						_chartData.Series.Add(_selfSeries);
						_chartData.Series.Add(_otherSeries);
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

			return _chartData;
		}

		public BO.ChartDataBO GetSaleChartData(int userID)
		{
			BO.ChartDataBO _chartData = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_SaleChartData", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_chartData = new BO.ChartDataBO();
						var _series = new BO.ChartSeriesBO()
						{
							Name = "Monthly Sale Amout",
							Data = new List<object>()
						};
						foreach (DataRow _dr in _ds.Tables[0].Rows)
						{
							_chartData.Categories.Add(Convert.ToString(_dr["Category"]));
							_series.Data.Add(_dr["NetSaleAmount"]);
						}
						_chartData.Series.Add(_series);
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

			return _chartData;
		}

		public BO.ChartDataBO GetBrokerageBistributionChartData(int userID)
		{
			BO.ChartDataBO _chartData = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_BrokDistributionChartData", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_chartData = new BO.ChartDataBO();
						var _series = new BO.ChartSeriesBO()
						{
							Name = "Brokerage",
							Data = new List<object>()
						};
						foreach (DataRow _dr in _ds.Tables[0].Rows)
						{
							_series.Data.Add(new
							{
								Name = Convert.ToString(_dr["BrokerName"]),
								Y = _dr["BrokerageAmt"]
							});
						}
						_chartData.Series.Add(_series);
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

			return _chartData;
		}

		public BO.SaleSearchResultBO GetDuePayments(BO.SaleSearchBO saleSearch)
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

				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Sale_DuePayments", _paramList))
				{
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

		public BO.ChartDataBO GetLoanInerestPaidChartData(int userID)
		{
			BO.ChartDataBO _chartData = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Loan_InerestPaidChartData", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_chartData = new BO.ChartDataBO();

						DataTable _data = _ds.Tables[0];
						DataTable _categoriesDT = _ds.Tables[0].DefaultView.ToTable(true, "PayMonth");
						foreach (DataRow _dr in _categoriesDT.Rows)
						{
							_chartData.Categories.Add(Convert.ToString(_dr["PayMonth"]));
						}

						foreach (DataRow _dr in _ds.Tables[0].DefaultView.ToTable(true, "Borrower").Rows)
						{
							string _borrower = _dr["Borrower"].ToString();
							var _series = new BO.ChartSeriesBO()
							{
								Name = _borrower,
								Data = new List<object>()
							};

							foreach (DataRow _drCat in _categoriesDT.Rows)
							{
								string _payMonth = Convert.ToString(_drCat["PayMonth"]);
								DataRow[] _borPayDR = _data.Select(string.Format("PayMonth='{0}' And Borrower='{1}'", _payMonth, _borrower));
								if (_borPayDR.Length > 0)
								{
									_series.Data.Add(_borPayDR[0]["TotalPayAmount"]);
								}
								else
								{
									_series.Data.Add(null);
								}
							}
							_chartData.Series.Add(_series);
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

			return _chartData;
		}

		public BO.ChartDataBO GetLoanChartData(int userID)
		{
			BO.ChartDataBO _chartData = null;

			try
			{
				DatabaseAccess.OpenConnection();
				using (DataSet _ds = DatabaseAccess.ExecuteProcedure("P_Loan_ChartData", new List<DAL.DatabaseParameter>()
				{
					new DAL.DatabaseParameter("@ClientID",DAL.ParameterDirection.In,DAL.DataType.Int, userID)
				}))
				{
					if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
					{
						_chartData = new BO.ChartDataBO();

						DataTable _data = _ds.Tables[0];

						var _paSerise = new BO.ChartSeriesBO()
						{
							Name = "Total Principal Amount",
							Data = new List<object>()
						};

						var _pPaidSerise = new BO.ChartSeriesBO()
						{
							Name = "Total Principal Paid",
							Data = new List<object>(),
							Stack = "Paid"
						};
						var _iPaidSerise = new BO.ChartSeriesBO()
						{
							Name = "Total Interest Paid",
							Data = new List<object>(),
							Stack = "Paid"
						};

						foreach (DataRow _dr in _data.Rows)
						{
							_chartData.Categories.Add(Convert.ToString(_dr["Borrower"]));

							_paSerise.Data.Add(_dr["TotalPrincipalAmount"]);
							_pPaidSerise.Data.Add(_dr["TotalPrincipalPaid"]);
							_iPaidSerise.Data.Add(_dr["TotalInterestPaid"]);

						}

						_chartData.Series.Add(_paSerise);
						_chartData.Series.Add(_iPaidSerise);
						_chartData.Series.Add(_pPaidSerise);

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

			return _chartData;
		}
	}
}

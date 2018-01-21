using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace G2.DB.Api.Infrastructure.Core
{
	public class ExcelActionResult : IHttpActionResult
	{
		private DataTable _data;
		private string _fileName = "data.xlsx";

		public ExcelActionResult(DataTable data, string exportFileName)
		{
			this._data = data;
			this._fileName = exportFileName;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage _response = null;
			MemoryStream _ms = new MemoryStream();
			using (XLWorkbook wb = new XLWorkbook())
			{
				wb.Worksheets.Add(_data);
				wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				wb.Style.Font.Bold = true;
				wb.SaveAs(_ms);

				_response = new HttpResponseMessage(HttpStatusCode.OK);

				_response.Content = new StreamContent(_ms);
				_response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
				_response.Content.Headers.ContentDisposition.FileName = _fileName;
				_response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

				_response.Content.Headers.ContentLength = _ms.Length;
				_ms.Seek(0, SeekOrigin.Begin);

			}

			return Task.FromResult(_response);
		}
	}
}
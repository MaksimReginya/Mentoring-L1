using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using ClosedXML.Extensions;
using NorthwindHttpHandler.DataAccess.GeneratedEntities;
using NorthwindHttpHandler.Exceptions;

namespace NorthwindHttpHandler.ReportGenerator
{
	public class Generator
	{
		private IQueryable<Order> _orders;
		private readonly NameValueCollection _queryString;
		private readonly XlsxGenerator _xlsxGenerator;
		private readonly XmlGenerator _xmlGenerator;

		public Generator(IQueryable<Order> orders, NameValueCollection queryString)
		{
			_orders = orders;
			_queryString = queryString;
			_xlsxGenerator = new XlsxGenerator();
			_xmlGenerator = new XmlGenerator();
		}

		public void CreateReport(HttpResponse httpResponse, ReportFormat format)
		{
			this.FilterOrders();

			if (format == ReportFormat.Xlsx)
			{
				using (var wb = _xlsxGenerator.CreateWorkbook(_orders))
				{
					wb.DeliverToHttpResponse(httpResponse, "NorthwindOrders.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				}
			}
			else
			{
				httpResponse.ContentType = "application/xml";
				_xmlGenerator.WriteXmlToResponse(httpResponse, _orders);
			}
		}

		private void FilterOrders()
		{
			_orders = _orders.OrderBy(order => order.OrderID);

			this.FilterByCustomerId();
			this.FilterByDate();
			this.FilterByTake();
			this.FilterBySkip();
		}

		private void FilterByCustomerId()
		{
			string customerId = _queryString["customer"];
			if (!string.IsNullOrEmpty(customerId))
			{
				_orders = _orders.Where(order => order.CustomerID.Equals(customerId, StringComparison.OrdinalIgnoreCase));
			}
		}

		private void FilterByDate()
		{
			string dateFrom = _queryString["dateFrom"];
			string dateTo = _queryString["dateTo"];
			if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
			{
				throw new InvalidRequestException("dateFrom and dateTo cannot be specified at the same time");
			}
			else if (!string.IsNullOrEmpty(dateFrom))
			{
				DateTime date = DateTime.Parse(dateFrom);
				_orders = _orders.Where(order => order.OrderDate.Value >= date);
			}
			else if (!string.IsNullOrEmpty(dateTo))
			{
				DateTime date = DateTime.Parse(dateTo);
				_orders = _orders.Where(order => order.OrderDate.Value <= date);
			}
		}

		private void FilterByTake()
		{
			if (!int.TryParse(_queryString["take"], out int take))
			{
				return;
			}

			_orders = _orders.Take(take);
		}

		private void FilterBySkip()
		{
			if (!int.TryParse(_queryString["skip"], out int skip))
			{
				return;
			}

			_orders = _orders.Skip(skip);
		}
	}
}

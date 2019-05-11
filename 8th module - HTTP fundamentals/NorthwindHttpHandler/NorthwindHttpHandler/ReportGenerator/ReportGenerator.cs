using System;
using System.Collections.Specialized;
using System.Linq;
using NorthwindHttpHandler.DataAccess.GeneratedEntities;
using NorthwindHttpHandler.Exceptions;

namespace NorthwindHttpHandler.ReportGenerator
{
	public class Generator
	{
		private IQueryable<Order> _orders;
		private readonly NameValueCollection _queryString;
		private readonly ReportFormat _format;

		public Generator(IQueryable<Order> orders, NameValueCollection queryString, ReportFormat format)
		{
			_orders = orders;
			_queryString = queryString;
			_format = format;
		}

		public void CreateReport()
		{
			this.FilterByCustomerId();
			this.FilterByDate();
			this.FilterByTake();
			this.FilterBySkip();

		}

		private void FilterOrders()
		{
			
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

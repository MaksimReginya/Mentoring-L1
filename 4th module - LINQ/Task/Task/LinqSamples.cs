// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 001")]
		[Description("This sample finds all customers whose total turnover exceeds a certain value X.")]
		public void Linq001()
		{
			decimal X = 55500;

			var customers = dataSource.Customers
				.Where(customer => customer.Orders.Sum(order => order.Total) > X)
				.Select(_ => new
				{
					_.CustomerID,
					TotalOrdersSum = _.Orders.Sum(order => order.Total)
				});

			Console.WriteLine($"------------------------------------X = {X}-------------------------------------------------");
			foreach (var customer in customers)
			{
				Console.Write($"CustomerID: {customer.CustomerID}, TotalOrdersSum: {customer.TotalOrdersSum}");
			}

			X = 35500;
			Console.WriteLine($"------------------------------------X = {X}-------------------------------------------------");

			foreach (var customer in customers)
			{
				Console.Write($"CustomerID: {customer.CustomerID}, TotalOrdersSum: {customer.TotalOrdersSum}");
			}

			Console.WriteLine("-------------------------------------------------------------------------------------");
		}

		[Category("Grouping and Projection Operators")]
		[Title("GroupJoin and Select - Task 002")]
		[Description("This sample for each customer finds all suppliers who live in the same country and city.")]
		public void Linq002()
		{
			// With grouping
			var customersWithSuppliers = dataSource.Customers.GroupJoin(
				dataSource.Suppliers,
				c => new { c.City, c.Country },
				s => new { s.City, s.Country },
				(_customer, _suppliers) => new
				{
					_customer.CustomerID,
					_customer.City,
					_customer.Country,
					Suppliers = _suppliers.Select(_s => new { _s.SupplierName, _s.City, _s.Country })
				});

			foreach (var customer in customersWithSuppliers)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}, City: {customer.City}, Country: {customer.Country}");

				foreach (var supplier in customer.Suppliers)
				{
					Console.WriteLine($"SupplierName: {supplier.SupplierName}, City: {supplier.City}, Country: {supplier.Country}");
				}

				Console.WriteLine("-------------------------------------------------------------------------------------");
			}

			Console.WriteLine("**************************************************************************************");

			// Without grouping
			var customersWithSuppliers1 = dataSource.Customers.Select(
				customer => new
				{
					customer.CustomerID,
					customer.City,
					customer.Country,
					Suppliers = dataSource.Suppliers
								.Where(supplier => 
											supplier.City.Equals(customer.City, StringComparison.OrdinalIgnoreCase)
											&& supplier.Country.Equals(customer.Country, StringComparison.OrdinalIgnoreCase))
								.Select(supplier => new { supplier.SupplierName, supplier.City, supplier.Country })
				});

			foreach (var customer in customersWithSuppliers1)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}, City: {customer.City}, Country: {customer.Country}");

				foreach (var supplier in customer.Suppliers)
				{
					Console.WriteLine($"SupplierName: {supplier.SupplierName}, City: {supplier.City}, Country: {supplier.Country}");
				}

				Console.WriteLine("-------------------------------------------------------------------------------------");
			}
		}

		[Category("Quantifiers")]
		[Title("Any - Task 003")]
		[Description("This sample finds all customers who has orders with total greater than X.")]
		public void Linq003()
		{
			decimal X = 10000;
			IEnumerable<Customer> customers = dataSource.Customers
				.Where(customer => customer.Orders.Any(order => order.Total > X));

			Console.WriteLine($"X: {X}");
			foreach (Customer customer in customers)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}");

				foreach (Order order in customer.Orders.OrderByDescending(order => order.Total))
				{
					Console.WriteLine($"OrderID: {order.OrderID}, Total: {order.Total}");
				}

				Console.WriteLine("-------------------------------------------------------------------------------------");
			}
		}
	}
}

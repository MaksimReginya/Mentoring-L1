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
		[Title("Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 001")]
		[Description("This sample finds all customers whose total turnover exceeds a certain value X.")]
		public void Linq001()
		{
			decimal X = 55500;

			IEnumerable<Customer> customers = dataSource.Customers.Where(
				(customer) => customer.Orders.Sum((order) => order.Total) > X);

			Console.WriteLine($"------------------------------------X = {X}-------------------------------------------------");
			foreach (Customer customer in customers)
			{
				ObjectDumper.Write(customer);
			}

			X = 35500;
			Console.WriteLine($"------------------------------------X = {X}-------------------------------------------------");

			foreach (Customer customer in customers)
			{
				ObjectDumper.Write(customer);
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
					Suppliers = _suppliers.Select(_s => _s.SupplierName)
				});

			foreach (var customer in customersWithSuppliers)
			{
				Console.WriteLine(customer.CustomerID);

				foreach (string supplier in customer.Suppliers)
				{
					Console.WriteLine(supplier);
				}

				Console.WriteLine();
			}

			Console.WriteLine("-------------------------------------------------------------------------------------");

			// Without grouping
			var customersWithSuppliers1 = dataSource.Customers.Select(
				customer => new
				{
					customer.CustomerID,
					SupplierNames = dataSource.Suppliers
								.Where(supplier => 
											supplier.City.Equals(customer.City, StringComparison.OrdinalIgnoreCase)
											&& supplier.Country.Equals(customer.Country, StringComparison.OrdinalIgnoreCase))
								.Select(supplier => supplier.SupplierName)
				});

			foreach (var customer in customersWithSuppliers1)
			{
				Console.WriteLine(customer.CustomerID);

				foreach (string supplierName in customer.SupplierNames)
				{
					Console.WriteLine(supplierName);
				}

				Console.WriteLine();
			}
		}
	}
}

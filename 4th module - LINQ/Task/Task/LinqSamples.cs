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

		[Category("Task")]
		[Title("Task 001")]
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

		[Category("Task")]
		[Title("Task 002")]
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

		[Category("Task")]
		[Title("Task 003")]
		[Description("This sample finds all customers who have orders with 'Total' greater than X.")]
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

		[Category("Task")]
		[Title("Task 004")]
		[Description("This sample finds all customers with their first order month and year.")]
		public void Linq004()
		{
			var customers = dataSource.Customers
				.Where(customer => customer.Orders.Any())
				.Select(customer => new
				{
					customer.CustomerID,
					FirstOrderDate = customer.Orders
						.Min(order => order.OrderDate)
				});

			foreach (var customer in customers)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}, Year: {customer.FirstOrderDate.Year}, Month: {customer.FirstOrderDate.Month}");
			}
		}

		[Category("Task")]
		[Title("Task 005")]
		[Description("This sample finds all customers with their first order month and year (sorted by Year, Month, TotalSum (desc), CustomerID).")]
		public void Linq005()
		{
			var customers = dataSource.Customers
				.Where(customer => customer.Orders.Any())
				.Select(customer => new
				{
					customer.CustomerID,
					FirstOrderDate = customer.Orders
						.Min(order => order.OrderDate),
					TotalSum = customer.Orders.Sum(order => order.Total)
				})
				.OrderBy(customer => customer.FirstOrderDate.Year)
				.ThenBy(customer => customer.FirstOrderDate.Month)
				.ThenByDescending(customer => customer.TotalSum)
				.ThenBy(customer => customer.CustomerID);

			foreach (var customer in customers)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}," +
					$" Year: {customer.FirstOrderDate.Year}," +
					$" Month: {customer.FirstOrderDate.Month}" +
					$" TotalSum: {customer.TotalSum}");
			}
		}

		[Category("Task")]
		[Title("Task 006")]
		[Description("This sample finds all customers with 'non-numeric postal code' OR 'empty region' OR 'phone without operator code'.")]
		public void Linq006()
		{
			var customers = dataSource.Customers
				.Where(customer =>
					!int.TryParse(customer.PostalCode, out var res)
					|| string.IsNullOrEmpty(customer.Region)
					|| !customer.Phone.StartsWith("("));
				

			foreach (var c in customers)
			{
				Console.WriteLine($"CustomerID: {c.CustomerID}, Region: {c.Region ?? "<empty>"}, PostalCode: {c.PostalCode}, Phone: {c.Phone}");
			}
		}

		[Category("Task")]
		[Title("Task 007")]
		[Description("This sample groups all products by categories, then groups by existing units in stock, then sorts by price.")]
		public void Linq007()
		{
			var productGroups = dataSource.Products
				.GroupBy(product => product.Category)
				.Select(group => new
				{
					Category = group.Key,
					ProductsInStock = group.GroupBy(product => product.UnitsInStock > 0)
						.Select(inStockGroup => new
						{
							AreUnitsInStock = inStockGroup.Key,
							Products = inStockGroup.OrderBy(product => product.UnitPrice)
						})
				});

			foreach (var productsGroup in productGroups)
			{
				Console.WriteLine($"Category: {productsGroup.Category}");
				foreach (var productsInStock in productsGroup.ProductsInStock)
				{
					Console.WriteLine($"\tAre units in stock: {productsInStock.AreUnitsInStock}");
					foreach (var product in productsInStock.Products)
					{
						Console.WriteLine($"\t\tProduct name: {product.ProductName}, Unit price: {product.UnitPrice}");
					}
				}
			}
		}

		[Category("Task")]
		[Title("Task 008")]
		[Description("This sample groups all products by Cheap, Average and Expensive prices.")]
		public void Linq008()
		{
			decimal cheapBorder = 20, expensiveBorder = 40;

			var productGroups = dataSource.Products
				.GroupBy(product => product.UnitPrice < cheapBorder ? "Cheap"
					: product.UnitPrice < expensiveBorder ? "Average" : "Expensive");

			foreach (var productsGroup in productGroups)
			{
				Console.WriteLine($"{productsGroup.Key} products:");
				foreach (var product in productsGroup)
				{
					Console.WriteLine($"\tProduct name: {product.ProductName}, Unit price: {product.UnitPrice}");
				}
			}
		}

		[Category("Task")]
		[Title("Task 009")]
		[Description("This sample groups all customers by City and finds average Intensity and Profitability.")]
		public void Linq009()
		{
			var customerGroups = dataSource.Customers
				.GroupBy(customer => customer.City)
				.Select(group => new
				{
					City = group.Key,
					Profitability = group.Average(product => product.Orders.Sum(o => o.Total)),
					Intensity = group.Average(product => product.Orders.Length)
				});

			foreach (var customersGroup in customerGroups)
			{
				Console.WriteLine(
					$"City: {customersGroup.City}, " +
					$"Profitability: {Math.Round(customersGroup.Profitability, 2)}, " +
					$"Intensity: {Math.Round(customersGroup.Intensity, 2)}");
			}
		}

		[Category("Task")]
		[Title("Task 010")]
		[Description("This sample groups all customers by statistic for 'Month', 'Year', 'Month and Year'.")]
		public void Linq010()
		{
			var customerGroups = dataSource.Customers
				.Select(customer => new
				{
					customer.CustomerID,
					StatForMonths = customer.Orders
										.GroupBy(order => order.OrderDate.Month)
										.Select(group => new { Month = group.Key, OrdersCount = group.Count() }),
					StatForYears = customer.Orders
										.GroupBy(order => order.OrderDate.Year)
										.Select(group => new { Year = group.Key, OrdersCount = group.Count() }),
					StatForMonthsAndYears = customer.Orders
										.GroupBy(order => new { order.OrderDate.Year, order.OrderDate.Month })
										.Select(group => new { group.Key.Year, group.Key.Month, OrdersCount = group.Count() })
				});

			foreach (var customer in customerGroups)
			{
				Console.WriteLine($"CustomerID: {customer.CustomerID}");
				Console.WriteLine("\tStatistic for months:");
				foreach (var monthStat in customer.StatForMonths)
				{
					Console.WriteLine($"\t\tMonth: {monthStat.Month}, Orders count: {monthStat.OrdersCount}");
				}
				Console.WriteLine("\tStatistic for years:");
				foreach (var yearStat in customer.StatForYears)
				{
					Console.WriteLine($"\t\tYear: {yearStat.Year}, Orders count: {yearStat.OrdersCount}");
				}
				Console.WriteLine("\tStatistic for months and years:");
				foreach (var monthYearStat in customer.StatForMonthsAndYears)
				{
					Console.WriteLine($"\t\tMonth: {monthYearStat.Month}, Year: {monthYearStat.Year}, Orders count: {monthYearStat.OrdersCount}");
				}
			}
		}
	}
}

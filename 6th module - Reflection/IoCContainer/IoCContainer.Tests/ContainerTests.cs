using System.Reflection;
using IoCContainer.Tests.CustomCodeForTesting;
using NUnit.Framework;

namespace IoCContainer.Tests
{
	[TestFixture]
    public class ContainerTests
    {
		private Container _container;

		[SetUp]
		public void Init()
		{
			_container = new Container();
		}

		[Test]
		public void CreateInstance_WhenAssemblyAdded_InstancesInjectedWithConstructor()
		{
			_container.AddAssembly(Assembly.GetExecutingAssembly());

			CustomerBLL_Constructor customerBll_constructor = (CustomerBLL_Constructor)_container.CreateInstance(typeof(CustomerBLL_Constructor));

			Assert.IsNotNull(customerBll_constructor);
			Assert.IsTrue(customerBll_constructor.GetType().Equals(typeof(CustomerBLL_Constructor)));
		}

		[Test]
		public void CreateInstanceGeneric_WhenAssemblyAdded_InstancesInjectedWithConstructor()
		{
			_container.AddAssembly(Assembly.GetExecutingAssembly());

			CustomerBLL_Constructor customerBll_constructor = _container.CreateInstance<CustomerBLL_Constructor>();

			Assert.IsNotNull(customerBll_constructor);
			Assert.IsTrue(customerBll_constructor.GetType().Equals(typeof(CustomerBLL_Constructor)));
		}

		[Test]
		public void CreateInstance_WhenTypesAdded_InstancesInjectedWithConstructor()
		{
			_container.AddType(typeof(CustomerBLL_Constructor));
			_container.AddType(typeof(Logger));
			_container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

			CustomerBLL_Constructor customerBll_constructor = (CustomerBLL_Constructor)_container.CreateInstance(typeof(CustomerBLL_Constructor));

			Assert.IsNotNull(customerBll_constructor);
			Assert.IsTrue(customerBll_constructor.GetType().Equals(typeof(CustomerBLL_Constructor)));
		}

		[Test]
		public void CreateInstanceGeneric_WhenTypesAdded_InstancesInjectedWithConstructor()
		{
			_container.AddType(typeof(CustomerBLL_Constructor));
			_container.AddType(typeof(Logger));
			_container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

			CustomerBLL_Constructor customerBll_constructor = _container.CreateInstance<CustomerBLL_Constructor>();

			Assert.IsNotNull(customerBll_constructor);
			Assert.IsTrue(customerBll_constructor.GetType().Equals(typeof(CustomerBLL_Constructor)));
		}

		[Test]
		public void CreateInstance_WhenAssemblyAdded_InstancesInjectedWithProperties()
		{
			_container.AddAssembly(Assembly.GetExecutingAssembly());

			CustomerBLL_Properties customerBll_properties = (CustomerBLL_Properties)_container.CreateInstance(typeof(CustomerBLL_Properties));

			Assert.IsNotNull(customerBll_properties);
			Assert.IsTrue(customerBll_properties.GetType().Equals(typeof(CustomerBLL_Properties)));
			Assert.IsNotNull(customerBll_properties.CustomerDAL);
			Assert.IsTrue(customerBll_properties.CustomerDAL.GetType().Equals(typeof(CustomerDAL)));
			Assert.IsNotNull(customerBll_properties.Logger);
			Assert.IsTrue(customerBll_properties.Logger.GetType().Equals(typeof(Logger)));
		}

		[Test]
		public void CreateInstanceGeneric_WhenAssemblyAdded_InstancesInjectedWithProperties()
		{
			_container.AddAssembly(Assembly.GetExecutingAssembly());

			CustomerBLL_Properties customerBll_properties = _container.CreateInstance<CustomerBLL_Properties>();

			Assert.IsNotNull(customerBll_properties);
			Assert.IsTrue(customerBll_properties.GetType().Equals(typeof(CustomerBLL_Properties)));
			Assert.IsNotNull(customerBll_properties.CustomerDAL);
			Assert.IsTrue(customerBll_properties.CustomerDAL.GetType().Equals(typeof(CustomerDAL)));
			Assert.IsNotNull(customerBll_properties.Logger);
			Assert.IsTrue(customerBll_properties.Logger.GetType().Equals(typeof(Logger)));
		}

		[Test]
		public void CreateInstance_WhenTypesAdded_InstancesInjectedWithProperties()
		{
			_container.AddType(typeof(CustomerBLL_Properties));
			_container.AddType(typeof(Logger));
			_container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

			CustomerBLL_Properties customerBll_properties = (CustomerBLL_Properties)_container.CreateInstance(typeof(CustomerBLL_Properties));

			Assert.IsNotNull(customerBll_properties);
			Assert.IsTrue(customerBll_properties.GetType().Equals(typeof(CustomerBLL_Properties)));
			Assert.IsNotNull(customerBll_properties.CustomerDAL);
			Assert.IsTrue(customerBll_properties.CustomerDAL.GetType().Equals(typeof(CustomerDAL)));
			Assert.IsNotNull(customerBll_properties.Logger);
			Assert.IsTrue(customerBll_properties.Logger.GetType().Equals(typeof(Logger)));
		}

		[Test]
		public void CreateInstanceGeneric_WhenTypesAdded_InstancesInjectedWithProperties()
		{
			_container.AddType(typeof(CustomerBLL_Properties));
			_container.AddType(typeof(Logger));
			_container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

			CustomerBLL_Properties customerBll_properties = _container.CreateInstance<CustomerBLL_Properties>();

			Assert.IsNotNull(customerBll_properties);
			Assert.IsTrue(customerBll_properties.GetType().Equals(typeof(CustomerBLL_Properties)));
			Assert.IsNotNull(customerBll_properties.CustomerDAL);
			Assert.IsTrue(customerBll_properties.CustomerDAL.GetType().Equals(typeof(CustomerDAL)));
			Assert.IsNotNull(customerBll_properties.Logger);
			Assert.IsTrue(customerBll_properties.Logger.GetType().Equals(typeof(Logger)));
		}
	}
}

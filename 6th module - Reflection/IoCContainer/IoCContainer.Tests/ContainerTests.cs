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
		public void CreateInstanceGeneric_WhenAssemblyAdded_TypeCreatedSuccessfully()
		{
			_container.AddAssembly(Assembly.GetExecutingAssembly());

			CustomerBLL_Constructor customerBll_constructor = _container.CreateInstance<CustomerBLL_Constructor>();

			Assert.IsNotNull(customerBll_constructor);
			Assert.IsTrue(customerBll_constructor.GetType().Equals(typeof(CustomerBLL_Constructor)));
		}
	}
}

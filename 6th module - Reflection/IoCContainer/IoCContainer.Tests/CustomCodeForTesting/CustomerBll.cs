using IoCContainer.Attributes;

namespace IoCContainer.Tests.CustomCodeForTesting
{
	[ImportConstructor]
	public class CustomerBLL_Constructor
	{
		public CustomerBLL_Constructor(ICustomerDAL dal, Logger logger)
		{
		}
	}

	public class CustomerBLL_Properties
	{
		[Import]
		public ICustomerDAL CustomerDAL { get; set; }

		[Import]
		public Logger Logger { get; set; }
	}
}

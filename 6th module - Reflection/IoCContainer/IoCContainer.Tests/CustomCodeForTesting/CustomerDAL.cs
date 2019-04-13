using IoCContainer.Attributes;

namespace IoCContainer.Tests.CustomCodeForTesting
{
	[Export(typeof(ICustomerDAL))]
	public class CustomerDAL : ICustomerDAL
	{
		public CustomerDAL()
		{
		}
	}
}

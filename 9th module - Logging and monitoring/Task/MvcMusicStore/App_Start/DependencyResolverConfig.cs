using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MvcMusicStore.Infrastructure.Logger;
using MvcMusicStore.Infrastructure.PerfomanceCounters;
using PerformanceCounterHelper;

namespace MvcMusicStore.App_Start
{
	public class DependencyResolverConfig
	{
		public static IDependencyResolver GetConfiguredDependencyResolver()
		{
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterControllers(typeof(MvcApplication).Assembly);
			ConfigureBindings(builder);
			IContainer container = builder.Build();

			return new AutofacDependencyResolver(container);
		}

		private static void ConfigureBindings(ContainerBuilder builder)
		{
			builder.Register(c => PerformanceHelper.CreateCounterHelper<Counters>())
				.SingleInstance()
				.As<CounterHelper<Counters>>();
			builder.RegisterType<Logger>().As<ILogger>();
		}
	}
}
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcMusicStore.App_Start;
using MvcMusicStore.Infrastructure.Logger;

namespace MvcMusicStore
{
	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			DependencyResolver.SetResolver(DependencyResolverConfig.GetConfiguredDependencyResolver());
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			PerformanceCountersConfig.ConfigureCounters();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception exception = this.Server.GetLastError();
			ILogger logger = DependencyResolver.Current.GetService(typeof(ILogger)) as ILogger;
			logger?.Error(exception.Message, exception);
		}
	}
}

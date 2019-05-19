using System;
using System.Web.Mvc;
using MvcMusicStore.Infrastructure.PerfomanceCounters;
using PerformanceCounterHelper;

namespace MvcMusicStore.App_Start
{
	public class PerformanceCountersConfig
	{
		public static void ConfigureCounters()
		{
			CounterHelper<Counters> counterHelper = DependencyResolver.Current.GetService(typeof(CounterHelper<Counters>)) as CounterHelper<Counters>;
			foreach (Counters counter in Enum.GetValues(typeof(Counters)))
			{
				counterHelper.Reset(counter);
			}
		}
	}
}
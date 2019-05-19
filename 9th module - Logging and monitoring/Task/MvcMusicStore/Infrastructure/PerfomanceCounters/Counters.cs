using System.Diagnostics;
using PerformanceCounterHelper;

namespace MvcMusicStore.Infrastructure.PerfomanceCounters
{
	[PerformanceCounterCategory("MvcMusicStorePerformanceCounters", PerformanceCounterCategoryType.SingleInstance, "Performance counters")]
	public enum Counters
	{
		[PerformanceCounter("SuccessLogInCounter", "Counts a number of successful log in attempts.", PerformanceCounterType.NumberOfItems32)]
		SuccessLogInCounter = 1,

		[PerformanceCounter("SuccessLogOutCounter", "Counts a number of successful log out attempts.", PerformanceCounterType.NumberOfItems32)]
		SuccessLogOutCounter = 2,

		[PerformanceCounter("SignUpAttemptCounter", "Counts a number of all sign up attempts.", PerformanceCounterType.NumberOfItems32)]
		SignUpAttemptCounter = 3
	}
}
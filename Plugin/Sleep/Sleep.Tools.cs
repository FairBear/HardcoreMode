using AIProject;
using HarmonyLib;
using Manager;
using System;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Sleep
		{
			static EnvironmentSimulator.DateTimeSerialization defaultDTSerial =
				new EnvironmentSimulator.DateTimeSerialization { Time = new DateTime(1, 1, 1, 8, 0, 0) };
			static bool wakeTimeSet = false;

			public static void SetSleepThresholds(EnvironmentSimulator.DateTimeThreshold[] thresholds)
			{
				new Traverse(Resources.Instance.PlayerProfile)
					.Field("_canSleepTime")
					.SetValue(thresholds);
			}

			public static void SetWakeTimeFromNow(int hours)
			{
				SetWakeTime(Map.Instance.Simulator.Now.AddHours(hours));
			}

			public static void SetWakeTimeFixed(int hour)
			{
				int day = 1;

				if (Map.Instance.Simulator.Now.Hour >= hour)
					day = 2;

				SetWakeTime(new DateTime(1, 1, day, hour, 0, 0));
			}

			public static void SetWakeTime(DateTime time)
			{
				if (wakeTimeSet)
					return;

				wakeTimeSet = true;

				EnvironmentSimulator.DateTimeSerialization DTSerial =
				new EnvironmentSimulator.DateTimeSerialization { Time = time };

				new Traverse(Resources.Instance.PlayerProfile)
					.Field("_wakeTime")
					.SetValue(DTSerial);
			}

			public static void SetWakeTime()
			{
				if (SetHoursAsleep.Value)
					SetWakeTimeFromNow(hours);
				else
					SetWakeTimeFixed(WakeHour.Value);
			}

			public static void ResetWakeTime()
			{
				wakeTimeSet = false;

				new Traverse(Resources.Instance.PlayerProfile)
					.Field("_wakeTime")
					.SetValue(defaultDTSerial);
			}
		}
	}
}

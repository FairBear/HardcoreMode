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
			public static EnvironmentSimulator.DateTimeThreshold[] thresholdsBackup;
			public static EnvironmentSimulator.DateTimeThreshold[] thresholdsNew;
			public static bool asleep = false;

			public static void Update()
			{
				if (thresholdsNew == null)
				{
					thresholdsBackup = Manager.Resources.Instance.PlayerProfile.CanSleepTime;
					thresholdsNew = new EnvironmentSimulator.DateTimeThreshold[thresholdsBackup.Length];

					EnvironmentSimulator.DateTimeSerialization midnight =
						new EnvironmentSimulator.DateTimeSerialization(
							new Traverse(Map.Instance.Player).Field("_midnightTime").GetValue<DateTime>()
						);
					EnvironmentSimulator.DateTimeThreshold threshold =
						new EnvironmentSimulator.DateTimeThreshold()
						{
							min = midnight,
							max = midnight
						};

					for (int i = 0; i < thresholdsNew.Length; i++)
						thresholdsNew[i] = threshold;
				}
			}
		}
	}
}

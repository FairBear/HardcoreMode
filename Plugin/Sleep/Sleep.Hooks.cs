using AIProject;
using HarmonyLib;
using Manager;
using System;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static bool sleepPatch = false;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(PlayerActor), "ElapseTime", typeof(Action), typeof(bool))]
		public static bool Prefix_PlayerActor_ElapseTime(PlayerActor __instance,
														 Action action,
														 bool fadeOut = true)
		{
			__instance.ElapseTime(
				() =>
				{
					action?.Invoke();

					return true;
				},
				null
			);

			return false;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(PlayerActor), "ElapseTime", typeof(Func<bool>), typeof(Func<bool>))]
		public static bool Prefix_PlayerActor_ElapseTime(PlayerActor __instance,
														 Func<bool> conditionBefore,
														 Func<bool> conditionAfter = null)
		{
			if (sleepPatch)
			{
				sleepPatch = false;

				return true;
			}

			sleepPatch = true;

			__instance.ElapseTime(
				() =>
				{
					bool flag = true;

					if (conditionBefore != null)
						flag = conditionBefore();

					if (flag)
					{
						Sleep.asleep = true;

						Sleep.SetSleepThresholds(Sleep.thresholdsNew);
						Sleep.SetWakeTime();
					}

					return flag;
				},
				() =>
				{
					Sleep.asleep = false;

					Sleep.SetSleepThresholds(Sleep.thresholdsBackup);
					Sleep.ResetWakeTime();

					if (conditionAfter != null)
						return conditionAfter();

					return true;
				}
			);

			return false;
		}

		[HarmonyPrefix, HarmonyPatch(typeof(Map), "CanSleepInTime")]
		public static bool Prefix_Map_CanSleepInTime(ref bool __result)
		{
			if (!SleepAnytime.Value)
				return true;

			__result = true;

			return false;
		}
	}
}

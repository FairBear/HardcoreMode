using AIProject;
using System.Collections.Generic;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Status
		{
			static readonly HashSet<LifeStatsController> agentWarn =
				new HashSet<LifeStatsController>();
			static bool healthWarn = false;
			static bool foodWarn = false;
			static bool staminaWarn = false;

			public static void SetRect()
			{
				rect = new Rect(StatusX.Value, StatusY.Value, WIDTH, Screen.height);
			}

			public static int Timer(ref float timer, float max)
			{
				timer += Time.deltaTime;

				if (timer >= max)
				{
					int mult = (int)Mathf.Floor(timer / max);
					timer %= max;

					return mult;
				}

				return 0;
			}

			static void TryWarn(LifeStatsController controller, int threshold)
			{
				float stat = controller["health"];

				if (agentWarn.Contains(controller))
				{
					if (stat > threshold)
						agentWarn.Remove(controller);
				}
				else if (stat <= threshold)
				{
					agentWarn.Add(controller);

					string name = controller.agent.CharaName;
					string preposition = stat < threshold ? "below" : "at";

					MapUIContainer.AddNotify($"{name}'s health is {preposition} {threshold}%!");
				}
			}

			static void TryWarn(string key, int threshold, ref bool flag)
			{
				float stat = playerController[key];

				if (flag)
				{
					if (stat > threshold)
						flag = false;
				}
				else if (stat <= threshold)
				{
					flag = true;
					string preposition = stat < threshold ? "below" : "at";

					MapUIContainer.AddNotify($"Your {key} is {preposition} {threshold}%!");
				}
			}

			public static void TryWarn()
			{
				TryWarn("health", HealthWarn.Value, ref healthWarn);
				TryWarn("food", FoodWarn.Value, ref foodWarn);
				TryWarn("stamina", StaminaWarn.Value, ref staminaWarn);

				int agentThreshold = AgentWarn.Value;

				foreach (LifeStatsController controller in agentControllers)
					TryWarn(controller, agentThreshold);
			}
		}
	}
}

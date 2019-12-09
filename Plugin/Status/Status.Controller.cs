using AIProject;
using AIProject.Definitions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Status
		{
			public static void Update()
			{
				if (playerController == null)
					return;

				if (StatusKey.Value.IsDown())
					visible = !visible;

				float dt = Time.deltaTime * 24f / Manager.Map.Instance.EnvironmentProfile.DayLengthInMinute;

				foreach (LifeStatsController controller in agentControllers)
					Update_Agent(controller, dt);

				Update_Player(dt);
				TryWarn();
			}

			static void Update_Player(float dt)
			{
				bool flag0 = PlayerLife.Value;
				bool flag1 = PlayerDeath.Value;

				if (playerController["food"] > 0f)
				{
					if (flag0)
						playerController["food"] -=
							FoodLoss.Value * dt /
							FoodLossFactor.Value /
							(Sleep.asleep ? FoodLossSleepFactor.Value : 1);
				}
				else if (flag1 && playerController["health"] > 0)
				{
					playerController["health"] -= HealthLoss.Value * dt / 60f;

					if (playerController["health"] == 0 && PermaDeath.Value)
						TryDelete(Manager.Map.Instance.Player.ChaControl);
				}

				if (Sleep.asleep)
				{
					float factor = dt / 60f;

					if (flag0)
						playerController["stamina"] += StaminaRate.Value * factor;

					if (flag1)
						playerController["health"] += HealthRate.Value * factor;
				}
				else if (flag0)
					playerController["stamina"] -= StaminaLoss.Value * dt / StaminaLossFactor.Value;
			}

			static void Update_Agent(LifeStatsController controller, float dt)
			{
				if (controller["health"] > 0)
				{
					if (AgentDeath.Value)
						if (controller.agent.StateType == State.Type.Sleep ||
							controller.agent.StateType == State.Type.Immobility)
							controller["health"] += AgentHealthRate.Value * dt / 60f;
						else if (controller.agent.StateType == State.Type.Collapse)
						{
							controller["health"] -= AgentHealthLoss.Value * dt / 60f;

							if (controller["health"] == 0)
							{
								if (PermaDeath.Value)
									TryDelete(controller.agent.ChaControl);

								MapUIContainer.AddNotify($"{controller.agent.CharaName} died.");
							}
						}
				}
				else if (AgentRevive.Value && !PermaDeath.Value)
				{
					controller["revive"] -= 100f * dt / 60f / 24f;

					if (controller["revive"] == 0)
					{
						controller["revive", "food", "stamina", "health"] = 100f;

						if (AgentReviveReset.Value)
						{
							List<int> keys = controller.ChaControl.fileGameInfo.flavorState.Keys.ToList();

							foreach (int key in keys)
								controller.agent.AgentData.SetFlavorSkill(key, 0);

							controller.agent.SetPhase(0);

							controller.agent.ChaControl.fileGameInfo.lifestyle = -1;
						}

						MapUIContainer.AddNotify($"{controller.agent.CharaName} has been revived.");
					}
				}
			}
		}
	}
}

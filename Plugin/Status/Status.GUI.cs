using System.Linq;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Status
		{
			const float WIDTH = 160f;

			const float LABEL_WIDTH = 140f;

			static Rect rect = new Rect(0, 0, WIDTH, Screen.height);
			static Rect innerRect = new Rect(0f, 0f, WIDTH, Screen.height);

			static GUIStyle healthStyle;
			static GUIStyle foodStyle;
			static GUIStyle staminaStyle;

			static bool visible = true;

			static void Draw_Player()
			{
				bool flag0 = PlayerDeath.Value;
				bool flag1 = PlayerLife.Value;

				if (!flag1 && !flag0)
					return;

				GUILayout.BeginVertical(GUI.skin.box);
				{
					if (flag0)
						GUILayout.Label($"Health: {playerController["health"]:F0}%", healthStyle);

					if (flag1)
					{
						GUILayout.Label($"Food: {playerController["food"]:F0}%", foodStyle);
						GUILayout.Label($"Stamina: {playerController["stamina"]:F0}%", staminaStyle);
					}
				}
				GUILayout.EndVertical();
			}

			static void Draw_Agent()
			{
				bool flag0 = AgentDeath.Value;
				bool flag1 = AgentRevive.Value;

				if (agentControllers.Count == 0 ||
					(!flag0 && !flag1 && !agentControllers.Any(v => v["health"] == 0)))
					return;

				GUILayout.BeginVertical(GUI.skin.box);
				{
					foreach (LifeStatsController controller in agentControllers)
					{
						if (controller.ChaControl == null)
						{
							agentControllersDump.Add(controller);
							continue;
						}

						if (controller["health"] > 0)
						{
							if (flag0)
								GUILayout.Label(
									$"{controller.agent.CharaName}: {controller["health"]:F0}%",
									healthStyle,
									GUILayout.Width(LABEL_WIDTH),
									GUILayout.ExpandWidth(false)
								);
						}
						else if (flag1)
							GUILayout.Label(
								$"{controller.agent.CharaName}: {24f * controller["revive"] / 100f:F0} Hour(s)",
								GUILayout.Width(LABEL_WIDTH),
								GUILayout.ExpandWidth(false)
							);
					}
				}
				GUILayout.EndVertical();
			}

			static void Draw(int id)
			{
				GUILayout.BeginArea(innerRect);
				{
					GUILayout.BeginVertical();
					{
						Draw_Player();
						Draw_Agent();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}

			static public void OnGUI()
			{
				if (playerController == null ||
					!visible)
					return;

				if (healthStyle == null)
				{
					healthStyle = new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 12,
						normal =
					{
						textColor = Color.green
					}
					};

					foodStyle = new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 12,
						normal =
					{
						textColor = Color.yellow
					}
					};

					staminaStyle = new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 12
					};
				}

				rect = GUI.Window(
					WindowIDStatus.Value,
					rect,
					Draw,
					"",
					GUI.skin.label
				);
			}
		}
	}
}

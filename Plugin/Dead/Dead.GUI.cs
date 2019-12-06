using AIProject;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Dead
		{
			const float WIDTH = 200f;
			const float HEIGHT = 120f;

			static Rect rect = new Rect((Screen.width - WIDTH) / 2f, 20f, WIDTH, HEIGHT);
			static Rect innerRect = new Rect(0f, 10f, WIDTH, HEIGHT);

			static GUIStyle labelStyle;
			static GUIStyle subLabelStyle;

			static void Draw(int id)
			{
				GUILayout.BeginArea(innerRect);
				{
					GUILayout.BeginVertical();
					{
						GUILayout.Label("You Died", labelStyle);
						GUILayout.Label("Change your player character.", subLabelStyle);

						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								if (GUILayout.Button("Device Menu"))
									OpenDeviceMenu();

								if (GUILayout.Button("System Menu"))
									MapUIContainer.SetActiveSystemMenuUI(true);
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}

			static public void OnGUI()
			{
				if (!PlayerDeath.Value ||
					playerController == null ||
					playerController["health"] > 0 ||
					!MapUIContainer.IsInstance() ||
					MapUIContainer.AnyUIActive())
					return;

				if (labelStyle == null)
				{
					labelStyle = new GUIStyle(GUI.skin.label)
					{
						fontSize = 14,
						fontStyle = FontStyle.Bold,
						alignment = TextAnchor.MiddleCenter
					};

					subLabelStyle = new GUIStyle(GUI.skin.label)
					{
						fontSize = 12,
						alignment = TextAnchor.MiddleCenter
					};
				}

				rect = GUI.Window(
					WindowIDDead.Value,
					rect,
					Draw,
					"",
					GUI.skin.box
				);
			}
		}
	}
}

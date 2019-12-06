using AIProject;
using Manager;
using System;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Sleep
		{
			const float HOURS_MARGIN_TOP = 30f;
			const float HOURS_MARGIN_BOTTOM = 10f;
			const float HOURS_MARGIN_LEFT = 34f;
			const float HOURS_MARGIN_RIGHT = 10f;
			const float HOURS_WIDTH = 80f;
			const float HOURS_HEIGHT = 200f;
			const float HOURS_INNER_WIDTH = HOURS_WIDTH - HOURS_MARGIN_LEFT - HOURS_MARGIN_RIGHT;
			const float HOURS_INNER_HEIGHT = HOURS_HEIGHT - HOURS_MARGIN_TOP - HOURS_MARGIN_BOTTOM;

			const float SLEEP_WIDTH = 200f;
			const float SLEEP_HEIGHT = 60f;

			static Rect hoursRect;
			static Rect hoursInnerRect = new Rect(
				HOURS_MARGIN_LEFT,
				HOURS_MARGIN_TOP,
				HOURS_INNER_WIDTH,
				HOURS_INNER_HEIGHT
			);
			static Rect hoursDragRect = new Rect(0f, 0f, HOURS_WIDTH, 20f);

			static Rect sleepRect = new Rect((Screen.width - SLEEP_WIDTH) / 2f, 20f, SLEEP_WIDTH, SLEEP_HEIGHT);
			static Rect sleepInnerRect = new Rect(0f, 0f, SLEEP_WIDTH, SLEEP_HEIGHT);

			static GUIStyle labelStyle;

			static bool justHid = true;
			public static int hours = 8;

			static void DrawHours(int id)
			{
				GUI.DragWindow(hoursDragRect);
				GUILayout.BeginArea(hoursInnerRect);
				{
					hours = (int)GUILayout.VerticalSlider(hours, 24f, 1f);
				}
				GUILayout.EndArea();
			}

			static void DrawHours()
			{
				if (!SetHoursAsleep.Value ||
					!MapUIContainer.CommandList.IsActiveControl ||
					MapUIContainer.CommandList.Options?[0] != Map.Instance.Player.SleepCommandInfos[0])
				{
					justHid = true;
					return;
				}

				if (justHid)
				{
					justHid = false;
					hoursRect = new Rect(20f, (Screen.height - HOURS_HEIGHT) / 2f, HOURS_WIDTH, HOURS_HEIGHT);
				}

				hoursRect = GUI.Window(
					WindowIDSleep.Value,
					hoursRect,
					DrawHours,
					$"{hours} Hour{(hours > 1 ? "s" : "")}"
				);
			}

			static void DrawSleep(int id)
			{
				GUILayout.BeginArea(sleepInnerRect);
				{
					GUILayout.BeginVertical();
					{
						DateTime now = Map.Instance.Simulator.Now;
						DateTime next = Manager.Resources.Instance.PlayerProfile.WakeTime.Time;

						GUILayout.Label($"{now.TimeOfDay}", labelStyle);
						GUILayout.Label($"{next - now}", labelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
			
			static void DrawSleep()
			{
				if (labelStyle == null)
					labelStyle = new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 16,
						alignment = TextAnchor.MiddleCenter
					};

				sleepRect = GUI.Window(
					WindowIDSleep.Value,
					sleepRect,
					DrawSleep,
					"",
					GUI.skin.label
				);
			}

			static public void OnGUI()
			{
				if (!Map.IsInstance() ||
					!MapUIContainer.IsInstance() ||
					Map.Instance.Player == null)
				{
					justHid = true;
					return;
				}

				if (asleep)
					DrawSleep();
				else
					DrawHours();
			}
		}
	}
}

using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class FoodMenu
		{
			const float MARGIN_TOP = 20f;
			const float MARGIN_BOTTOM = 10f;
			const float MARGIN_LEFT = 10f;
			const float MARGIN_RIGHT = 10f;
			const float WIDTH = 300f;
			const float HEIGHT = 300f;
			static readonly float X = Screen.width - WIDTH - 20f;
			static readonly float Y = (Screen.height - HEIGHT) / 2f;
			const float INNER_WIDTH = WIDTH - MARGIN_LEFT - MARGIN_RIGHT;
			const float INNER_HEIGHT = HEIGHT - MARGIN_TOP - MARGIN_BOTTOM;

			const float BUTTON_WIDTH = 60f;

			static Rect rect = new Rect(X, Y, WIDTH, HEIGHT);
			static Rect innerRect = new Rect(MARGIN_LEFT, MARGIN_TOP, INNER_WIDTH, INNER_HEIGHT);
			static Rect dragRect = new Rect(0f, 0f, WIDTH, 20f);
			static Vector2 scroll = new Vector2();

			public static IOrderedEnumerable<Tuple<StuffItem, string, int, int>>  foods;
			public static bool visible = false;

			static void Draw_Food()
			{
				bool refresh = false;

				foreach (Tuple<StuffItem, string, int, int> food in foods)
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label($"x{food.Item1.Count} {food.Item2} [{food.Item3}]");

						if (GUILayout.Button($"+{food.Item4}%", GUILayout.Width(BUTTON_WIDTH)))
						{
							playerController["food"] += food.Item4;

							if (food.Item1.Count <= 1)
							{
								List<StuffItem> list = Map.Instance.Player.PlayerData.ItemList;

								if (refresh = list.Contains(food.Item1))
									list.Remove(food.Item1);
							}
							else
								food.Item1.Count--;
						}
					}
					GUILayout.EndHorizontal();
				}

				if (refresh)
					foods = GetEdible();
			}

			static void Draw(int id)
			{
				GUI.DragWindow(dragRect);
				GUILayout.BeginArea(innerRect);
				{
					GUILayout.BeginVertical();
					{
						scroll = GUILayout.BeginScrollView(scroll);
						{
							Draw_Food();
						}
						GUILayout.EndScrollView();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}

			static public void OnGUI()
			{
				if (!visible ||
					!PlayerLife.Value ||
					foods == null ||
					playerController == null)
					return;

				rect = GUI.Window(
					WindowIDFoodMenu.Value,
					rect,
					Draw,
					"Food"
				);
			}
		}
	}
}

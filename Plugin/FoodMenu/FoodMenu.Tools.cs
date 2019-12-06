using AIProject;
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
			public static IOrderedEnumerable<Tuple<StuffItem, string, int, int>> GetEdible()
			{
				if (!Map.IsInstance() || Map.Instance.Player == null)
					return null;

				Manager.Resources resources = Manager.Resources.Instance;
				Manager.Resources.GameInfoTables gameInfo = resources.GameInfo;
				List<Tuple<StuffItem, string, int, int>> foods = new List<Tuple<StuffItem, string, int, int>>();
				Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> foodParamTable =
					resources.GameInfo.FoodParameterTable;

				foreach (StuffItem food in Map.Instance.Player.PlayerData.ItemList)
					if (foodParamTable.TryGetValue(
							food.CategoryID,
							out Dictionary<int, Dictionary<int, FoodParameterPacket>> paramTable
						) &&
						paramTable.TryGetValue(food.ID, out Dictionary<int, FoodParameterPacket> _))
					{
						StuffItemInfo info = gameInfo.GetItem(food.CategoryID, food.ID);

						foods.Add(new Tuple<StuffItem, string, int, int>(
							food,
							info.Name,
							info.Rate,
							GetFoodRecovered(info.Rate)
						));
					}

				return foods.OrderByDescending(v => v.Item3);
			}

			public static int GetFoodRecovered(int rate)
			{
				return Mathf.Clamp(rate * FoodRate.Value / 100, FoodMinRate.Value, 100);
			}
		}
	}
}

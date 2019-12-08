using AIProject;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class FoodMenu
		{
			public static void Update()
			{
				try
				{
					if (MapUIContainer.AnyUIActive())
					{
						if (visible)
							visible = false;

						return;
					}
				}
				catch { }
				
				if (FoodKey.Value.IsDown())
				{
					visible = !visible;

					if (visible)
						foods = GetEdible();
				}
			}

			public static void LateUpdate()
			{
				if (visible)
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}
	}
}

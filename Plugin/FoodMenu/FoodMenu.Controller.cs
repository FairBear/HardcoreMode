using AIProject;
using Manager;

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
		}
	}
}

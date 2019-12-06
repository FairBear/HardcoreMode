namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class FoodMenu
		{
			public static void Update()
			{
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

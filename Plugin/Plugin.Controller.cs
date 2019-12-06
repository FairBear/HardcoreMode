using System.Collections.Generic;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		public static LifeStatsController playerController;
		static readonly HashSet<LifeStatsController> controllersQueue = new HashSet<LifeStatsController>();
		static readonly HashSet<LifeStatsController> agentControllers = new HashSet<LifeStatsController>();
		static readonly HashSet<LifeStatsController> agentControllersDump = new HashSet<LifeStatsController>();

		void Update()
		{
			Status.Update();
			FoodMenu.Update();
			Sleep.Update();
		}

		void LateUpdate()
		{
			Dead.LateUpdate();
		}

		void OnGUI()
		{
			UpdateControllers();

			Status.OnGUI();
			FoodMenu.OnGUI();
			Sleep.OnGUI();
			Dead.OnGUI();
		}
	}
}

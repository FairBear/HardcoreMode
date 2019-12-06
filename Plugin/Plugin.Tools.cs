using AIProject;
using Manager;
using System.Collections.Generic;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		public static void AddController(LifeStatsController controller)
		{
			if (!agentControllers.Contains(controller))
				controllersQueue.Add(controller);
		}

		public static void UpdateControllers()
		{
			if (playerController?.ChaControl == null)
				playerController = null;

			if (!Map.IsInstance() ||
				Map.Instance.Player == null ||
				Map.Instance.AgentTable == null)
				return;

			foreach (LifeStatsController controller in controllersQueue)
			{
				if (Map.Instance.Player.ChaControl == controller.ChaControl)
				{
					playerController = controller;
					continue;
				}

				foreach (KeyValuePair<int, AgentActor> agent in Map.Instance.AgentTable)
					if (agent.Value.ChaControl == controller.ChaControl)
					{
						controller.agent = agent.Value;

						agentControllers.Add(controller);
						break;
					}
			}

			controllersQueue.Clear();

			foreach (LifeStatsController controller in agentControllersDump)
				agentControllers.Remove(controller);

			agentControllersDump.Clear();
		}
	}
}

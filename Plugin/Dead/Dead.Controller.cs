using AIProject;
using Manager;
using UnityEngine;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Dead
		{
			static bool wasDead = false;
			public static float debounce = 0f;

			public static void LateUpdate()
			{
				if (playerController == null)
					return;

				if (PlayerDeath.Value && playerController["health"] == 0)
				{
					if (MapUIContainer.AnyUIActive())
						debounce = 1f;
					else if (debounce > 0f)
						debounce -= Time.unscaledDeltaTime;

					wasDead = true;
					PlayerActor player = Map.Instance.Player;
					player.Controller.enabled = debounce > 0f;

					if (player.ChaControl.enabled)
						player.ChaControl.enabled = false;

					if (player.Animation.enabled)
						player.Animation.enabled = false;

					if (player.Locomotor.enabled)
						player.Locomotor.enabled = false;

					if (player.ChaControl.visibleAll)
						player.ChaControl.visibleAll = false;

					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
				else if (wasDead)
				{
					wasDead = false;

					Map.Instance.Player.EnableEntity();
				}
			}
		}
	}
}

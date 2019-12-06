using AIProject;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AI;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static bool movePatch = false;

		[HarmonyPrefix, HarmonyPatch(typeof(NavMeshAgent), "Move")]
		public static bool Prefix_NavMeshAgent_Move(NavMeshAgent __instance,
													Vector3 offset)
		{
			if (!PlayerLife.Value ||
				__instance != Manager.Map.Instance.Player.NavMeshAgent)
				return true;

			if (movePatch)
			{
				movePatch = false;

				return true;
			}

			bool flag =
				playerController["food"] <= LowFood.Value ||
				playerController["stamina"] <= LowStamina.Value;

			if (!flag ||
				Input.GetKey(KeyCode.LeftShift) ||
				Input.GetKey(KeyCode.RightShift))
				return true;

			movePatch = true;

			LocomotionProfile.PlayerSpeedSetting speed =
				Manager.Resources.Instance.LocomotionProfile.PlayerSpeed;
			Vector3 platformVelocity =
				new Traverse(__instance)
					.Field("platformVelocity")
					.GetValue<Vector3>();
			platformVelocity =
				new Vector3(platformVelocity.x, 0f, platformVelocity.z);

			offset -= platformVelocity;
			offset *= speed.walkSpeed / speed.normalSpeed;

			__instance.Move(offset + platformVelocity);

			return false;
		}
	}
}

using AIProject;
using AIProject.Player;
using AIProject.Scene;
using HarmonyLib;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		[HarmonyPrefix, HarmonyPatch(typeof(EditPlayer), "StartChange")]
		public static void Prefix_EditPlayer_OnAwake(PlayerActor player)
		{
			if (!PlayerLife.Value && !PlayerDeath.Value)
				return;

			player.ChaControl.chaFile.SaveCharaFile(
				player.ChaControl.chaFile.charaFileName,
				player.ChaControl.sex,
				false
			);
		}

		[HarmonyPrefix, HarmonyPatch(typeof(MapScene), "SaveProfile")]
		public static void Prefix_MapScene_SaveProfile(bool isAuto = false)
		{
			if ((!PlayerLife.Value && !PlayerDeath.Value) ||
				playerController == null)
				return;

			PlayerActor player = Manager.Map.Instance.Player;

			player.ChaControl.chaFile.SaveCharaFile(
				player.ChaControl.chaFile.charaFileName,
				player.ChaControl.sex,
				false
			);
		}
	}
}

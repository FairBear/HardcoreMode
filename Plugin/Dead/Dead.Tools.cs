using AIProject;
using Manager;
using System.Linq;

namespace HardcoreMode
{
	public partial class HardcoreMode
	{
		static partial class Dead
		{
			static void OpenDeviceMenu()
			{
				DevicePoint[] devices = FindObjectsOfType<DevicePoint>();
				DevicePoint device = devices?.FirstOrDefault(v => v.ID == 0);

				if (device != null)
				{
					PlayerActor player = Map.Instance.Player;
					player.CurrentDevicePoint = device;

					Resources.Instance.SoundPack.Play(SoundPack.SystemSE.BootDevice);
					MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
					MapUIContainer.RefreshCommands(0, player.DeviceCommandInfos);
					MapUIContainer.SetActiveCommandList(true, "データ端末");
					player.PlayerController.ChangeState("DeviceMenu");
				}
			}
		}
	}
}

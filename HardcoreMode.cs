using BepInEx;
using BepInEx.Configuration;
using BepInEx.Harmony;
using KKAPI.Chara;
using System;
using UnityEngine;
using KeyboardShortcut = BepInEx.Configuration.KeyboardShortcut;

namespace HardcoreMode
{
	[BepInPlugin(GUID, Name, Version)]
	[BepInProcess("AI-Syoujyo")]
	public partial class HardcoreMode : BaseUnityPlugin
	{
		const string GUID = "com.fairbair.hardcoremode";
		const string Name = "Hardcore Mode";
		const string Version = "1.0.2";
		const string BEHAVIOR = "HardcoreMode.LifeStats";

		const string SECTION_GENERAL = "General";
		const string SECTION_SURVIVAL = "Survival Stats";
		const string SECTION_SLEEP = "Sleep Override";
		const string SECTION_LOSS = "Stat Loss";
		const string SECTION_RECOVER = "Stat Recovery";
		const string SECTION_PENALTY = "Penalties";
		const string SECTION_WARN = "Warnings";

		const string DESCRIPTION_PLAYER_LIFE =
			"This enables the player survival stats. " +
			"Does not include the 'Health' stat.";
		const string DESCRIPTION_PLAYER_DEATH =
			"Enables the player's health stat and allows the player to die. " +
			"The player is permanently dead and you will have to change characters. " +
			"You can revive the dead character by loading them as an agent.";
		const string DESCRIPTION_AGENT_DEATH =
			"Enables the agent's health stat and allows the agents to die. " +
			"When dead, the agent is permanently collapsed.";
		const string DESCRIPTION_AGENT_REVIVE =
			"Allows the agents to revive from death after 1 in-game day. " +
			"Revival is halted when this is disabled.";

		const string DESCRIPTION_SET_HOURS_ASLEEP =
			"Allows you to set the total number of hours asleep. " +
			"A window will be shown along with the sleep dialog.";
		const string DESCRIPTION_WAKE_HOUR =
			"The hour at which your character wakes up. " +
			"If you sleep after the designated hour, " +
			"you'll wake up the next day at the same hour. " +
			"This does not apply if you enable 'Set Hours Asleep.'";

		const string DESCRIPTION_LOSS_FACTOR =
			"Total rate is applied over the course of this interval. " +
			"A factor of 10 means it takes 10 minutes to apply the total rate. " +
			"Interval is in in-game minutes.";
		const string DESCRIPTION_AGENT_HEALTH_LOSS =
			"The amount of health lost overtime when an agent has collapsed. " +
			"Putting them to bed will halt the loss rate. " +
			"Interval is in in-game hours.";
		const string DESCRIPTION_HEALTH_LOSS =
			"The amount of health lost overtime when the player has 0% food. " +
			"Interval is in in-game hours.";
		const string DESCRIPTION_FOOD_LOSS =
			"The amount of food lost overtime. " +
			"100% means they lose 100% of their food over the set interval.";
		const string DESCRIPTION_FOOD_LOSS_SLEEP_FACTOR =
			"This is divided to the food decay value when you are asleep. " +
			"This means that you become less hungry if you are asleep.";
		const string DESCRIPTION_STAMINA_LOSS =
			"The amount of stamina lost overtime. " +
			"100% means they lose 100% of their stamina over the set interval.";

		const string DESCRIPTION_AGENT_HEALTH_RATE =
			"How much health is recovered overtime when an agent sleeps? " +
			"Interval is in in-game hours.";
		const string DESCRIPTION_HEALTH_RATE =
			"How much health is recovered overtime when the player sleeps? " +
			"Interval is in in-game hours.";
		const string DESCRIPTION_STAMINA_RATE =
			"How much stamina is recovered overtime when the player sleeps? " +
			"100% means they recover 100% of their stamina per minute. " +
			"Interval is in in-game hours.";
		const string DESCRIPTION_FOOD_RATE =
			"How much food is replenished based on the food's exchange rate? " +
			"100% means they gain 100% of the food's exchange rate as food " +
			"(If the item's exchange rate is 100, then you recover 100% of your food.)";
		const string DESCRIPTION_FOOD_MIN_RATE =
			"The amount you recover from eating cannot be below this value. " +
			"This means you will always at least recover this much food whenever you eat something.";

		const string DESCRIPTION_LOW_FOOD =
			"Having your food below or equal to this value will force you to walk.";
		const string DESCRIPTION_LOW_STAMINA =
			"Having your stamina below or equal to this value will force you to walk.";
		const string DESCRIPTION_AGENT_REVIVE_RESET =
			"When enabled, this will reset their stats and hearts to 0 when they revive.";

		const string DESCRIPTION_HEALTH_WARN =
			"The game warns you if your health falls below or equal to this value.";
		const string DESCRIPTION_AGENT_WARN =
			"The game warns you if an agent's health falls below or equal to this value.";
		const string DESCRIPTION_FOOD_WARN =
			"The game warns you if your food falls below or equal to this value.";
		const string DESCRIPTION_STAMINA_WARN =
			"The game warns you if your stamina falls below or equal to this value.";

		internal static ConfigEntry<int> WindowIDStatus { get; set; }
		internal static ConfigEntry<int> WindowIDFoodMenu { get; set; }
		internal static ConfigEntry<int> WindowIDSleep { get; set; }
		internal static ConfigEntry<int> WindowIDDead { get; set; }
		internal static ConfigEntry<int> StatusX { get; set; }
		internal static ConfigEntry<int> StatusY { get; set; }

		internal static ConfigEntry<KeyboardShortcut> StatusKey { get; set; }
		internal static ConfigEntry<bool> PlayerLife { get; set; }
		internal static ConfigEntry<bool> PlayerDeath { get; set; }
		internal static ConfigEntry<bool> AgentDeath { get; set; }
		internal static ConfigEntry<bool> AgentRevive { get; set; }

		internal static ConfigEntry<bool> SleepAnytime { get; set; }
		internal static ConfigEntry<bool> SetHoursAsleep { get; set; }
		internal static ConfigEntry<int> WakeHour { get; set; }

		internal static ConfigEntry<int> AgentHealthLoss { get; set; }
		internal static ConfigEntry<int> HealthLoss { get; set; }
		internal static ConfigEntry<int> FoodLoss { get; set; }
		internal static ConfigEntry<int> FoodLossFactor { get; set; }
		internal static ConfigEntry<float> FoodLossSleepFactor { get; set; }
		internal static ConfigEntry<int> StaminaLoss { get; set; }
		internal static ConfigEntry<int> StaminaLossFactor { get; set; }

		internal static ConfigEntry<KeyboardShortcut> FoodKey { get; set; }
		internal static ConfigEntry<int> AgentHealthRate { get; set; }
		internal static ConfigEntry<int> HealthRate { get; set; }
		internal static ConfigEntry<int> StaminaRate { get; set; }
		internal static ConfigEntry<int> FoodRate { get; set; }
		internal static ConfigEntry<int> FoodMinRate { get; set; }

		internal static ConfigEntry<int> LowFood { get; set; }
		internal static ConfigEntry<int> LowStamina { get; set; }
		internal static ConfigEntry<bool> AgentReviveReset { get; set; }


		internal static ConfigEntry<int> HealthWarn { get; set; }
		internal static ConfigEntry<int> AgentWarn { get; set; }
		internal static ConfigEntry<int> FoodWarn { get; set; }
		internal static ConfigEntry<int> StaminaWarn { get; set; }

		void Awake()
		{
			WindowIDStatus = Config.Bind(SECTION_GENERAL, "__Window ID (Status HUD)", 83462);
			WindowIDFoodMenu = Config.Bind(SECTION_GENERAL, "__Window ID (Player Food)", 83463);
			WindowIDSleep = Config.Bind(SECTION_GENERAL, "__Window ID (Sleep Hours)", 83464);
			WindowIDDead = Config.Bind(SECTION_GENERAL, "__Window ID (Dead)", 83465);
			StatusX = Config.Bind(SECTION_GENERAL, "Status UI Offset X", 150);
			StatusY = Config.Bind(SECTION_GENERAL, "Status UI Offset Y", 10);

			StatusKey = Config.Bind(SECTION_SURVIVAL, "Status UI Key", new KeyboardShortcut(KeyCode.T));
			PlayerLife = Config.Bind(SECTION_SURVIVAL, "Player Life Stats", true, DESCRIPTION_PLAYER_LIFE);
			PlayerDeath = Config.Bind(SECTION_SURVIVAL, "Player Death", true, DESCRIPTION_PLAYER_DEATH);
			AgentDeath = Config.Bind(SECTION_SURVIVAL, "Agent Death", true, DESCRIPTION_AGENT_DEATH);
			AgentRevive = Config.Bind(SECTION_SURVIVAL, "Agent Revival", true, DESCRIPTION_AGENT_REVIVE);

			SleepAnytime = Config.Bind(SECTION_SLEEP, "Sleep Anytime", true);
			SetHoursAsleep = Config.Bind(SECTION_SLEEP, "Set Hours Asleep", true, DESCRIPTION_SET_HOURS_ASLEEP);
			WakeHour = Config.Bind(SECTION_SLEEP, "Wake Up Hour", 8, new ConfigDescription(DESCRIPTION_WAKE_HOUR, new AcceptableValueRange<int>(0, 23)));

			AgentHealthLoss = Config.Bind(SECTION_LOSS, "Agent Health Loss (per Game-Hour)", 30, new ConfigDescription(DESCRIPTION_AGENT_HEALTH_LOSS, new AcceptableValueRange<int>(0, 100)));
			HealthLoss = Config.Bind(SECTION_LOSS, "Health Loss (per Game-Hour)", 20, new ConfigDescription(DESCRIPTION_HEALTH_LOSS, new AcceptableValueRange<int>(0, 100)));
			FoodLoss = Config.Bind(SECTION_LOSS, "Food Loss", 1, new ConfigDescription(DESCRIPTION_FOOD_LOSS, new AcceptableValueRange<int>(0, 100)));
			FoodLossFactor = Config.Bind(SECTION_LOSS, "Food Loss Factor (Game-Minutes)", 6, DESCRIPTION_LOSS_FACTOR);
			FoodLossSleepFactor = Config.Bind(SECTION_LOSS, "Food Loss Sleep Factor", 3f, DESCRIPTION_FOOD_LOSS_SLEEP_FACTOR);
			StaminaLoss = Config.Bind(SECTION_LOSS, "Stamina Loss", 2, new ConfigDescription(DESCRIPTION_STAMINA_LOSS, new AcceptableValueRange<int>(0, 100)));
			StaminaLossFactor = Config.Bind(SECTION_LOSS, "Stamina Loss Factor (Game-Minutes)", 29, DESCRIPTION_LOSS_FACTOR);

			FoodKey = Config.Bind(SECTION_RECOVER, "Player Food Menu Key", new KeyboardShortcut(KeyCode.F));
			AgentHealthRate = Config.Bind(SECTION_RECOVER, "Agent Health Recovery (per Game-Hour)", 5, DESCRIPTION_AGENT_HEALTH_RATE);
			HealthRate = Config.Bind(SECTION_RECOVER, "Player Health Recovery (per Game-Hour)", 1, DESCRIPTION_HEALTH_RATE);
			StaminaRate = Config.Bind(SECTION_RECOVER, "Stamina Recovery (per Game-Hour)", 12, new ConfigDescription(DESCRIPTION_STAMINA_RATE, new AcceptableValueRange<int>(0, 100)));
			FoodRate = Config.Bind(SECTION_RECOVER, "Food Recovery (per Exchange Rate)", 100, new ConfigDescription(DESCRIPTION_FOOD_RATE, new AcceptableValueRange<int>(0, 100)));
			FoodMinRate = Config.Bind(SECTION_RECOVER, "Food Minimum Recovery", 10, new ConfigDescription(DESCRIPTION_FOOD_MIN_RATE, new AcceptableValueRange<int>(0, 100)));

			LowFood = Config.Bind(SECTION_PENALTY, "Low Food Threshold", 0, new ConfigDescription(DESCRIPTION_LOW_FOOD, new AcceptableValueRange<int>(0, 100)));
			LowStamina = Config.Bind(SECTION_PENALTY, "Low Stamina Threshold", 0, new ConfigDescription(DESCRIPTION_LOW_STAMINA, new AcceptableValueRange<int>(0, 100)));
			AgentReviveReset = Config.Bind(SECTION_PENALTY, "Agent Death Penalty", true, DESCRIPTION_AGENT_REVIVE_RESET);

			HealthWarn = Config.Bind(SECTION_WARN, "Health Warning", 30, new ConfigDescription(DESCRIPTION_HEALTH_WARN, new AcceptableValueRange<int>(0, 100)));
			AgentWarn = Config.Bind(SECTION_WARN, "Agent Health Warning", 30, new ConfigDescription(DESCRIPTION_AGENT_WARN, new AcceptableValueRange<int>(0, 100)));
			FoodWarn = Config.Bind(SECTION_WARN, "Food Warning", 20, new ConfigDescription(DESCRIPTION_FOOD_WARN, new AcceptableValueRange<int>(0, 100)));
			StaminaWarn = Config.Bind(SECTION_WARN, "Stamina Warning", 20, new ConfigDescription(DESCRIPTION_STAMINA_WARN, new AcceptableValueRange<int>(0, 100)));

			InitSetting(StatusX, Status.SetRect);
			InitSetting(StatusY, Status.SetRect);

			InitSetting(FoodLossSleepFactor, () => FoodLossSleepFactor.BoxedValue = Mathf.Max(1, FoodLossSleepFactor.Value));

			CharacterApi.RegisterExtraBehaviour<LifeStatsController>(BEHAVIOR);
			HarmonyWrapper.PatchAll(typeof(HardcoreMode));
		}

		internal static void InitSetting<T>(ConfigEntry<T> entry, Action setter)
		{
			setter();

			entry.SettingChanged += (sender, args) => setter();
		}
	}
}

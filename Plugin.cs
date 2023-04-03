using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace Obeliskial_Options
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGUID = "com.meds.obeliskialoptions";
        private const string ModName = "Obeliskial Options";
        public const string ModVersion = "1.0.3";
        public const string ModDate = "20230403";
        private readonly Harmony harmony = new(ModGUID);
        internal static ManualLogSource Log;
        // public static ConfigEntry<bool> medsRepeatPurchase { get; private set; }
        // public static ConfigEntry<bool> medsRepeatHeroes { get; private set; }
        // public static ConfigEntry<bool> medsLegalCloning { get; private set; }
        // public static ConfigEntry<bool> medsGetClaimation { get; private set; }
        public static ConfigEntry<bool> medsShopRarity { get; private set; }
        public static ConfigEntry<bool> medsProfane { get; private set; }
        public static ConfigEntry<bool> medsCorruptGiovanna { get; private set; }
        public static ConfigEntry<bool> medsItemCorrupt { get; private set; }
        public static ConfigEntry<bool> medsShopCorrupt { get; private set; }
        public static ConfigEntry<bool> medsDiscountDivination { get; private set; }
        public static ConfigEntry<bool> medsDiscountDoomroll { get; private set; }
        public static ConfigEntry<bool> medsEmotional { get; private set; }
        public static ConfigEntry<bool> medsSmallSanitySupplySelling { get; private set; }
        public static ConfigEntry<bool> medsRavingRerolls { get; private set; }
        public static ConfigEntry<bool> medsUseClaimation { get; private set; }
        public static ConfigEntry<bool> medsSoloShop { get; private set; }
        public static ConfigEntry<bool> medsMaxMultiplayerMembers { get; private set; }
        public static ConfigEntry<bool> medsPlentifulPetPurchases { get; private set; }
        public static ConfigEntry<bool> medsStockedShop { get; private set; }
        public static ConfigEntry<bool> medsStraya { get; private set; }
        public static ConfigEntry<string> medsStrayaServer { get; private set; }

        // debug options
        public static ConfigEntry<bool> medsDebugKeyItems { get; private set; }
        public static ConfigEntry<bool> medsDebugAlwaysFail { get; private set; }
        public static ConfigEntry<bool> medsDebugAlwaysSucceed { get; private set; }
        public static ConfigEntry<bool> medsDebugJuice { get; private set; }
        public static ConfigEntry<bool> medsDebugCraftCorruptedCards { get; private set; }
        public static ConfigEntry<bool> medsDebugInfiniteCardCraft { get; private set; }
        public static ConfigEntry<bool> medsDebugDeveloperMode { get; private set; }
        public static ConfigEntry<bool> medsDebugPerkPoints { get; private set; }
        public static ConfigEntry<bool> medsDebugModifyPerks { get; private set; }
        public static ConfigEntry<bool> medsDebugTravelAnywhere { get; private set; }
        public static ConfigEntry<bool> medsDebugNoTravelRequirements { get; private set; }
        public static ConfigEntry<bool> medsDebugNoPerkRequirements { get; private set; }

        private void Awake()
        {
            Log = Logger;
            // Plugin.medsRepeatPurchase = this.Config.Bind<bool>("Options", "medsRepeatPurchase", true, "Allow items to be purchased by multiple heroes.");
            // Plugin.medsRepeatHeroes = this.Config.Bind<bool>("Options", "medsRepeatHeroes", true, "(IN TESTING) Can use the same hero in multiple sluts.");
            // Plugin.medsLegalCloning = this.Config.Bind<bool>("Options", "Legal Cloning", false, "(NOT WORKING) Allows multiple of a hero in the party.");
            // Plugin.medsGetClaimation = this.Config.Bind<bool>("Options", "High Madness - Acquire Claims", false, "(NOT WORKING - NOT EVEN STARTED) Acquire new claims on any madness.");
            medsShopRarity = Config.Bind(new ConfigDefinition("Options", "Adjusted Shop Rarity"), true, new ConfigDescription("Modify shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt."));
            medsProfane = Config.Bind(new ConfigDefinition("Options", "Allow Profanities"), true, new ConfigDescription("Allow profanities in your good Christian Piss server."));
            medsCorruptGiovanna = Config.Bind(new ConfigDefinition("Options", "Corrupted Card Rewards"), false, new ConfigDescription("Card rewards are always corrupted."));
            medsItemCorrupt = Config.Bind(new ConfigDefinition("Options", "Corrupted Item Rewards"), false, new ConfigDescription("Make item rewards always corrupted."));
            medsShopCorrupt = Config.Bind(new ConfigDefinition("Options", "Corrupted Items in Town Shop"), true, new ConfigDescription("Allow town shops to have corrupted goods."));
            medsDiscountDivination = Config.Bind(new ConfigDefinition("Options", "Discount Divination"), true, new ConfigDescription("Discounts are applied to divinations."));
            medsDiscountDoomroll = Config.Bind(new ConfigDefinition("Options", "Discount Doomroll"), true, new ConfigDescription("Discounts are applied to shop rerolls."));
            medsEmotional = Config.Bind(new ConfigDefinition("Options", "Emotional"), true, new ConfigDescription("Use more emotes during combat."));
            medsSmallSanitySupplySelling = Config.Bind(new ConfigDefinition("Options", "High Madness - Sell Supplies"), true, new ConfigDescription("Sell supplies on high madness."));
            medsRavingRerolls = Config.Bind(new ConfigDefinition("Options", "High Madness - Shop Rerolls"), true, new ConfigDescription("Rerolls on high madness."));
            medsUseClaimation = Config.Bind(new ConfigDefinition("Options", "High Madness - Use Claims"), true, new ConfigDescription("(IN TESTING) Use claims on any madness."));
            medsSoloShop = Config.Bind(new ConfigDefinition("Options", "Individual Player Shops"), true, new ConfigDescription("Does not send shop purchase records in multiplayer."));
            medsMaxMultiplayerMembers = Config.Bind(new ConfigDefinition("Options", "Max Multiplayer Members"), true, new ConfigDescription("Default to 4 players in multiplayer."));
            medsPlentifulPetPurchases = Config.Bind(new ConfigDefinition("Options", "Plentiful Pet Purchases"), true, new ConfigDescription("(IN TESTING) Buy more than one of each pet."));
            medsStockedShop = Config.Bind(new ConfigDefinition("Options", "Post-Scarcity Shops"), true, new ConfigDescription("Does not record who purchased what in the shop."));
            medsStraya = Config.Bind(new ConfigDefinition("Options", "Force Select Server"), false, new ConfigDescription("Force server selection to location of your choice (default: Australia)."));
            // CaptureWidth = Config.Bind("Section", "Key", 1, new ConfigDescription("Description", new AcceptableValueRange<int>(0, 100)));
            // ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
            medsStrayaServer = Config.Bind(new ConfigDefinition("Options", "Server To Force"), "au", new ConfigDescription("Which server should be forced if the above option is true?", new AcceptableValueList<string>("asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "za", "sa", "kr", "us", "usw")));
            // debug options
            medsDebugKeyItems = Config.Bind(new ConfigDefinition("Debug", "All Key Items"), false, new ConfigDescription("Give all key items in Adventure Mode. Items are added when you load into town."));
            medsDebugAlwaysFail = Config.Bind(new ConfigDefinition("Debug", "Always Fail Event Rolls"), false, new ConfigDescription("Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible."));
            medsDebugAlwaysSucceed = Config.Bind(new ConfigDefinition("Debug", "Always Succeed Event Rolls"), false, new ConfigDescription("Always succeed event rolls, though event text might not match. Critically succeeds if possible."));
            medsDebugJuice = Config.Bind(new ConfigDefinition("Debug", "Become Rich"), false, new ConfigDescription("Many cash, cryttals, supplies."));
            medsDebugCraftCorruptedCards = Config.Bind(new ConfigDefinition("Debug", "Craft Corrupted Cards"), false, new ConfigDescription("Allow crafting of corrupted cards."));
            medsDebugInfiniteCardCraft = Config.Bind(new ConfigDefinition("Debug", "Craft Infinite Cards"), false, new ConfigDescription("Infinite card crafts (set available card count to 99)."));
            medsDebugDeveloperMode = Config.Bind(new ConfigDefinition("Debug", "Developer Mode"), false, new ConfigDescription("(IN TESTING) Turns on AtO devs’ developer mode. Back up your save!"));
            medsDebugPerkPoints = Config.Bind(new ConfigDefinition("Debug", "Many Perk Points"), false, new ConfigDescription("(MILDLY BUGGY) Set maximum perk points to 1000."));
            medsDebugModifyPerks = Config.Bind(new ConfigDefinition("Debug", "Modify Perks Whenever"), false, new ConfigDescription("(IN TESTING) Change perks whenever you want."));
            medsDebugTravelAnywhere = Config.Bind(new ConfigDefinition("Debug", "Travel Anywhere"), false, new ConfigDescription("(IN TESTING) Travel to any node."));
            medsDebugNoPerkRequirements = Config.Bind(new ConfigDefinition("Debug", "No Perk Requirements"), false, new ConfigDescription("(IN TESTING) Can select perk without selecting its precursor perks; ignore minimum selected perk count for each row."));
            medsDebugNoTravelRequirements = Config.Bind(new ConfigDefinition("Debug", "No Travel Requirements"), false, new ConfigDescription("(IN TESTING) Can travel to nodes that are normally invisible (e.g. western treasure node in Faeborg)."));

            harmony.PatchAll();
            Log.LogInfo($"Plugin {ModGUID} is loaded! Prayge ");
        }
    }
}

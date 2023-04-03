using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System;

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
            Log = base.Logger;
            // Plugin.medsRepeatPurchase = this.Config.Bind<bool>("Options", "medsRepeatPurchase", true, "Allow items to be purchased by multiple heroes.");
            // Plugin.medsRepeatHeroes = this.Config.Bind<bool>("Options", "medsRepeatHeroes", true, "(IN TESTING) Can use the same hero in multiple sluts.");
            // Plugin.medsLegalCloning = this.Config.Bind<bool>("Options", "Legal Cloning", false, "(NOT WORKING) Allows multiple of a hero in the party.");
            // Plugin.medsGetClaimation = this.Config.Bind<bool>("Options", "High Madness - Acquire Claims", false, "(NOT WORKING - NOT EVEN STARTED) Acquire new claims on any madness.");
            Plugin.medsShopRarity = this.Config.Bind<bool>("Options", "Adjusted Shop Rarity", true, "Modify shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt.");
            Plugin.medsProfane = this.Config.Bind<bool>("Options", "Allow Profanities", true, "Allow profanities in your good Christian Piss server.");
            Plugin.medsCorruptGiovanna = this.Config.Bind<bool>("Options", "Corrupted Card Rewards", false, "Card rewards are always corrupted.");
            Plugin.medsItemCorrupt = this.Config.Bind<bool>("Options", "Corrupted Item Rewards", false, "Make item rewards always corrupted.");
            Plugin.medsShopCorrupt = this.Config.Bind<bool>("Options", "Corrupted Items in Town Shop", true, "Allow town shops to have corrupted goods.");
            Plugin.medsDiscountDivination = this.Config.Bind<bool>("Options", "Discount Divination", true, "Discounts are applied to divinations.");
            Plugin.medsDiscountDoomroll = this.Config.Bind<bool>("Options", "Discount Doomroll", true, "Discounts are applied to shop rerolls.");
            Plugin.medsEmotional = this.Config.Bind<bool>("Options", "Emotional", true, "Use more emotes during combat.");
            Plugin.medsSmallSanitySupplySelling = this.Config.Bind<bool>("Options", "High Madness - Sell Supplies", true, "Sell supplies on high madness.");
            Plugin.medsRavingRerolls = this.Config.Bind<bool>("Options", "High Madness - Shop Rerolls", true, "Rerolls on high madness.");
            Plugin.medsUseClaimation = this.Config.Bind<bool>("Options", "High Madness - Use Claims", true, "(IN TESTING) Use claims on any madness.");
            Plugin.medsSoloShop = this.Config.Bind<bool>("Options", "Individual Player Shops", true, "Does not send shop purchase records in multiplayer.");
            Plugin.medsMaxMultiplayerMembers = this.Config.Bind<bool>("Options", "Max Multiplayer Members", true, "(IN TESTING) Default to 4 players in multiplayer.");
            Plugin.medsPlentifulPetPurchases = this.Config.Bind<bool>("Options", "Plentiful Pet Purchases", true, "(IN TESTING) Buy more than one of each pet.");
            Plugin.medsStockedShop = this.Config.Bind<bool>("Options", "Post-Scarcity Shops", true, "Does not record who purchased what in the shop.");
            Plugin.medsStraya = this.Config.Bind<bool>("Options", "Strayan", false, "Default server selection to Australia.");
            
            // debug options
            Plugin.medsDebugKeyItems = this.Config.Bind<bool>("Debug", "All Key Items", false, "Give all key items in Adventure Mode. Items are added when you load into town.");
            Plugin.medsDebugAlwaysFail = this.Config.Bind<bool>("Debug", "Always Fail Event Rolls", false, "Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible.");
            Plugin.medsDebugAlwaysSucceed = this.Config.Bind<bool>("Debug", "Always Succeed Event Rolls", false, "Always succeed event rolls, though event text might not match. Critically succeeds if possible.");
            Plugin.medsDebugJuice = this.Config.Bind<bool>("Debug", "Become Rich", false, "Many cash, cryttals, supplies.");
            Plugin.medsDebugCraftCorruptedCards = this.Config.Bind<bool>("Debug", "Craft Corrupted Cards", false, "Allow crafting of corrupted cards.");
            Plugin.medsDebugInfiniteCardCraft = this.Config.Bind<bool>("Debug", "Craft Infinite Cards", false, "Infinite card crafts (set available card count to 99).");
            Plugin.medsDebugDeveloperMode = this.Config.Bind<bool>("Debug", "Developer Mode", false, "(IN TESTING) Turns on AtO devs’ developer mode. Backup your save!");
            Plugin.medsDebugPerkPoints = this.Config.Bind<bool>("Debug", "Many Perk Points", false, "(MILDLY BUGGY) Set maximum perk points to 1000.");
            Plugin.medsDebugModifyPerks = this.Config.Bind<bool>("Debug", "Modify Perks Whenever", false, "(IN TESTING) Change perks whenever you want.");
            Plugin.medsDebugTravelAnywhere = this.Config.Bind<bool>("Debug", "Travel Anywhere", false, "(IN TESTING) Travel to any node.");
            Plugin.medsDebugNoPerkRequirements = this.Config.Bind<bool>("Debug", "No Perk Requirements", false, "(IN TESTING) Can select perk without selecting its precursor perks; ignore minimum selected perk count for each row.");
            Plugin.medsDebugNoTravelRequirements = this.Config.Bind<bool>("Debug", "No Travel Requirements", false, "(IN TESTING) Can travel to nodes that are normally invisible (e.g. western treasure node in Faeborg).");

            this.harmony.PatchAll();
            Plugin.Log.LogInfo($"Plugin {ModGUID} is loaded! Prayge ");
        }
    }
}

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Obeliskial_Options
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGUID = "com.meds.obeliskialoptions";
        private const string ModName = "Obeliskial Options";
        public const string ModVersion = "1.1.0";
        public const string ModDate = "20230406";
        private readonly Harmony harmony = new(ModGUID);
        internal static ManualLogSource Log;
        public static int iShopsWithNoPurchase = 0;

        // public static ConfigEntry<bool> medsLegalCloning { get; private set; }
        // public static ConfigEntry<bool> medsGetClaimation { get; private set; }

        // Debug
        public static ConfigEntry<bool> medsKeyItems { get; private set; }
        public static ConfigEntry<bool> medsJuice { get; private set; }
        public static ConfigEntry<bool> medsDeveloperMode { get; private set; }
        public static ConfigEntry<string> medsExportSettings { get; private set; }
        public static ConfigEntry<string> medsImportSettings { get; private set; }

        // Cards & Decks
        public static ConfigEntry<bool> medsDiminutiveDecks { get; private set; }
        public static ConfigEntry<string> medsDenyDiminishingDecks { get; private set; }
        public static ConfigEntry<bool> medsCraftCorruptedCards { get; private set; }
        public static ConfigEntry<bool> medsInfiniteCardCraft { get; private set; }

        // Corruption & Madness
        public static ConfigEntry<bool> medsSmallSanitySupplySelling { get; private set; }
        public static ConfigEntry<bool> medsRavingRerolls { get; private set; }
        public static ConfigEntry<bool> medsUseClaimation { get; private set; }

        // Events & Nodes
        public static ConfigEntry<bool> medsAlwaysFail { get; private set; }
        public static ConfigEntry<bool> medsAlwaysSucceed { get; private set; }
        public static ConfigEntry<bool> medsNoTravelRequirements { get; private set; }
        public static ConfigEntry<bool> medsNoPlayerClassRequirements { get; private set; }
        public static ConfigEntry<bool> medsNoPlayerItemRequirements { get; private set; }
        public static ConfigEntry<bool> medsNoPlayerRequirements { get; private set; }
        public static ConfigEntry<bool> medsTravelAnywhere { get; private set; }

        // Loot
        public static ConfigEntry<bool> medsCorruptGiovanna { get; private set; }
        public static ConfigEntry<bool> medsLootCorrupt { get; private set; }

        // Perks
        public static ConfigEntry<bool> medsPerkPoints { get; private set; }
        public static ConfigEntry<bool> medsModifyPerks { get; private set; }
        public static ConfigEntry<bool> medsNoPerkRequirements { get; private set; }

        // Shop
        public static ConfigEntry<bool> medsShopRarity { get; private set; }
        public static ConfigEntry<float> medsShopBadLuckProtection { get; private set; }
        public static ConfigEntry<bool> medsMapShopCorrupt { get; private set; }
        public static ConfigEntry<bool> medsObeliskShopCorrupt { get; private set; }
        public static ConfigEntry<bool> medsTownShopCorrupt { get; private set; }
        public static ConfigEntry<bool> medsDiscountDivination { get; private set; }
        public static ConfigEntry<bool> medsDiscountDoomroll { get; private set; }
        public static ConfigEntry<bool> medsPlentifulPetPurchases { get; private set; }
        public static ConfigEntry<bool> medsStockedShop { get; private set; }
        public static ConfigEntry<bool> medsSoloShop { get; private set; }

        // Should Be Vanilla
        public static ConfigEntry<bool> medsProfane { get; private set; }
        public static ConfigEntry<bool> medsEmotional { get; private set; }
        public static ConfigEntry<bool> medsStraya { get; private set; }
        public static ConfigEntry<string> medsStrayaServer { get; private set; }
        public static ConfigEntry<bool> medsMaxMultiplayerMembers { get; private set; }
        public static ConfigEntry<bool> medsOverlyTenergetic { get; private set; }
        public static ConfigEntry<bool> medsBugfixEquipmentHP { get; private set; }
        public static ConfigEntry<bool> medsSkipCinematics { get; private set; }
        public static ConfigEntry<bool> medsAutoContinue { get; private set; }


        private void Awake()
        {
            Log = Logger;
            // Plugin.medsLegalCloning = this.Config.Bind<bool>("Options", "Legal Cloning", false, "(NOT WORKING) Allows multiple of a hero in the party.");
            // Plugin.medsGetClaimation = this.Config.Bind<bool>("Options", "High Madness - Acquire Claims", false, "(NOT WORKING - NOT EVEN STARTED) Acquire new claims on any madness.");
            // CaptureWidth = Config.Bind("Section", "Key", 1, new ConfigDescription("Description", new AcceptableValueRange<int>(0, 100)));
            // ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
            // Config.Bind("Section", "Int slider", 32, new ConfigDescription("You can use sliders for any number type", new AcceptableValueRange<int>(0, 100)));

            // Debug
            medsKeyItems = Config.Bind(new ConfigDefinition("Debug", "All Key Items"), false, new ConfigDescription("Give all key items in Adventure Mode. Items are added when you load into a town; if you've already passed the town and want the key items, use Travel Anywhere to go back to town? I'll add more methods in the future :)."));
            medsJuice = Config.Bind(new ConfigDefinition("Debug", "Become Rich"), false, new ConfigDescription("Many cash, cryttals, supplies."));
            medsDeveloperMode = Config.Bind(new ConfigDefinition("Debug", "Developer Mode"), false, new ConfigDescription("(IN TESTING) Turns on AtO devs’ developer mode. Back up your saves before using!"));
            medsExportSettings = Config.Bind(new ConfigDefinition("Debug", "Export Settings"), "", new ConfigDescription("(IN TESTING) Export settings (for use with 'Import Settings')."));
            medsImportSettings = Config.Bind(new ConfigDefinition("Debug", "Import Settings"), "", new ConfigDescription("(IN TESTING) Paste settings here to import them."));

            // Cards & Decks
            medsDiminutiveDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Ignore Minimum Deck Size"), true, new ConfigDescription("(IN TESTING - working ok) Allow you to remove cards even when deck contains less than 15."));
            medsDenyDiminishingDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Card Removal"), "Can Remove Anything", new ConfigDescription("What cards can be removed at the church?", new AcceptableValueList<string>("Cannot Remove Cards", "Cannot Remove Curses", "Can Only Remove Curses", "Can Remove Anything")));
            medsCraftCorruptedCards = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Corrupted Cards"), false, new ConfigDescription("Allow crafting of corrupted cards."));
            medsInfiniteCardCraft = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Infinite Cards"), false, new ConfigDescription("Infinite card crafts (set available card count to 99)."));

            // Corruption & Madness
            medsSmallSanitySupplySelling = Config.Bind(new ConfigDefinition("Corruption & Madness", "Sell Supplies"), true, new ConfigDescription("Sell supplies on high madness."));
            medsRavingRerolls = Config.Bind(new ConfigDefinition("Corruption & Madness", "Shop Rerolls"), true, new ConfigDescription("Allow multiple shop rerolls on high madness."));
            medsUseClaimation = Config.Bind(new ConfigDefinition("Corruption & Madness", "Use Claims"), true, new ConfigDescription("Use claims on any madness. Note that you cannot _get_ claims on high madness (yet...)."));

            // Events & Nodes
            medsAlwaysFail = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Fail Event Rolls"), false, new ConfigDescription("Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible."));
            medsAlwaysSucceed = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Succeed Event Rolls"), false, new ConfigDescription("Always succeed event rolls, though event text might not match. Critically succeeds if possible."));
            medsNoTravelRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Travel Requirements"), false, new ConfigDescription("(NOT WORKING - show path to node, but not actual node) Can travel to nodes that are normally invisible (e.g. western treasure node in Faeborg)."));
            medsNoPlayerClassRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Class Requirements"), false, new ConfigDescription("(IN TESTING) ignore class requirements? e.g. pretend you have a healer? might let you ignore specific character requirements"));
            medsNoPlayerItemRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Item Requirements"), false, new ConfigDescription("(IN TESTING) ignore equipment/pet requirements? e.g. should let you 'drop off the crate' @ Tsnemo's ship?"));
            medsNoPlayerRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Requirements"), false, new ConfigDescription("(IN TESTING) ignore key item???? requirements."));
            medsTravelAnywhere = Config.Bind(new ConfigDefinition("Events & Nodes", "Travel Anywhere"), false, new ConfigDescription("(IN TESTING) Travel to any node."));

            // Loot
            medsCorruptGiovanna = Config.Bind(new ConfigDefinition("Loot", "Corrupted Card Rewards"), false, new ConfigDescription("Card rewards are always corrupted (includes divinations)."));
            medsLootCorrupt = Config.Bind(new ConfigDefinition("Loot", "Corrupted Loot Rewards"), false, new ConfigDescription("Make item loot rewards always corrupted."));

            // Perks
            medsPerkPoints = Config.Bind(new ConfigDefinition("Perks", "Many Perk Points"), false, new ConfigDescription("(MILDLY BUGGY) Set maximum perk points to 1000."));
            medsModifyPerks = Config.Bind(new ConfigDefinition("Perks", "Modify Perks Whenever"), false, new ConfigDescription("(IN TESTING) Change perks whenever you want."));
            medsNoPerkRequirements = Config.Bind(new ConfigDefinition("Perks", "No Perk Requirements"), false, new ConfigDescription("(IN TESTING) Can select perk without selecting its precursor perks; ignore minimum selected perk count for each row."));

            // Shop
            medsShopRarity = Config.Bind(new ConfigDefinition("Shop", "Adjusted Shop Rarity"), false, new ConfigDescription("Modify shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt."));
            medsShopBadLuckProtection = Config.Bind(new ConfigDefinition("Shop", "Bad Luck Protection"), 0.1f, new ConfigDescription("Increases rarity of shops/loot based on number of shops/loot seen since an item was last acquired. Default: 0.1% increase in item rarity per shop seen without purchase, multiplied by town tier (1-4).", new AcceptableValueRange<float>(0, 100)));
            medsMapShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Map Shops"), true, new ConfigDescription("Allow shops on the map (e.g. werewolf shop in Senenthia) to have corrupted goods for sale."));
            medsObeliskShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Obelisk Shops"), true, new ConfigDescription("Allow obelisk corruption shops to have corrupted goods for sale."));
            medsTownShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Town Shops"), true, new ConfigDescription("Allow town shops to have corrupted goods for sale."));
            medsDiscountDivination = Config.Bind(new ConfigDefinition("Shop", "Discount Divination"), true, new ConfigDescription("Discounts are applied to divinations."));
            medsDiscountDoomroll = Config.Bind(new ConfigDefinition("Shop", "Discount Doomroll"), true, new ConfigDescription("Discounts are applied to shop rerolls."));
            medsPlentifulPetPurchases = Config.Bind(new ConfigDefinition("Shop", "Plentiful Pet Purchases"), true, new ConfigDescription("Buy more than one of each pet."));
            medsStockedShop = Config.Bind(new ConfigDefinition("Shop", "Post-Scarcity Shops"), true, new ConfigDescription("Does not record who purchased what in the shop."));
            medsSoloShop = Config.Bind(new ConfigDefinition("Shop", "Individual Player Shops"), true, new ConfigDescription("Does not send shop purchase records in multiplayer."));

            // Should Be Vanilla
            medsProfane = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Allow Profanities"), true, new ConfigDescription("Allow profanities in your good Christian Piss server."));
            medsEmotional = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Emotional"), true, new ConfigDescription("Use more emotes during combat."));
            medsStraya = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Force Select Server"), false, new ConfigDescription("Force server selection to location of your choice (default: Australia)."));
            medsStrayaServer = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Force Select Server Selection"), "au", new ConfigDescription("Which server should be forced if the above option is true?", new AcceptableValueList<string>("asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "za", "sa", "kr", "us", "usw")));
            medsMaxMultiplayerMembers = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Max Multiplayer Members"), true, new ConfigDescription("Default to 4 players in multiplayer."));
            medsOverlyTenergetic = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Overly Tenergetic"), true, new ConfigDescription("(IN TESTING - sometimes doesn't display properly, but seems functional) Allow characters to have more than 10 energy."));
            medsBugfixEquipmentHP = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Bugfix: Equipment HP"), true, new ConfigDescription("(IN TESTING) Fixes a vanilla bug that allows infinite stacking of HP by buying the same item repeatedly."));
            medsSkipCinematics = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Skip Cinematics"), true, new ConfigDescription("(IN TESTING) Skip cinematics."));
            medsAutoContinue = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Continue"), true, new ConfigDescription("(IN TESTING) Automatically press continue."));

            medsImportSettings.Value = "";
            medsExportSettings.Value = SettingsToString();
            
            harmony.PatchAll();
            Log.LogInfo($"{ModGUID} {ModVersion} has loaded! Prayge ");
        }
        public static string SettingsToString(bool forMP = false)
        {
            string[] str = new string[34];
            str[0] = medsShopRarity.Value ? "1" : "0";
            str[1] = medsMapShopCorrupt.Value ? "1" : "0";
            str[2] = medsObeliskShopCorrupt.Value ? "1" : "0";
            str[3] = medsTownShopCorrupt.Value ? "1" : "0";
            str[4] = medsLootCorrupt.Value ? "1" : "0";
            str[5] = medsPerkPoints.Value ? "1" : "0";
            str[6] = medsCorruptGiovanna.Value ? "1" : "0";
            str[7] = medsKeyItems.Value ? "1" : "0";
            str[8] = medsAlwaysSucceed.Value ? "1" : "0";
            str[9] = medsAlwaysFail.Value ? "1" : "0";
            str[10] = medsCraftCorruptedCards.Value ? "1" : "0";
            str[11] = medsInfiniteCardCraft.Value ? "1" : "0";
            str[12] = medsStockedShop.Value ? "1" : "0";
            str[13] = medsSoloShop.Value ? "1" : "0";
            str[14] = medsDeveloperMode.Value ? "1" : "0";
            str[15] = medsJuice.Value ? "1" : "0";
            str[16] = medsUseClaimation.Value ? "1" : "0";
            str[17] = medsDiscountDivination.Value ? "1" : "0";
            str[18] = medsDiscountDoomroll.Value ? "1" : "0";
            str[19] = medsRavingRerolls.Value ? "1" : "0";
            str[20] = medsSmallSanitySupplySelling.Value ? "1" : "0";
            str[21] = medsModifyPerks.Value ? "1" : "0";
            str[22] = medsNoPerkRequirements.Value ? "1" : "0";
            str[23] = medsPlentifulPetPurchases.Value ? "1" : "0";
            str[24] = medsTravelAnywhere.Value ? "1" : "0";
            str[25] = medsNoTravelRequirements.Value ? "1" : "0";
            str[26] = medsNoPlayerClassRequirements.Value ? "1" : "0";
            str[27] = medsNoPlayerItemRequirements.Value ? "1" : "0";
            str[28] = medsNoPlayerRequirements.Value ? "1" : "0";
            str[29] = medsOverlyTenergetic.Value ? "1" : "0";
            str[30] = medsDiminutiveDecks.Value ? "1" : "0";
            str[31] = medsDenyDiminishingDecks.Value;
            str[32] = medsShopBadLuckProtection.Value.ToString();
            str[33] = medsBugfixEquipmentHP.Value ? "1" : "0";
            string jstr = string.Join("|", str);
            if (!forMP)
            {
                str = new string[7];
                str[0] = medsProfane.Value ? "1" : "0";
                str[0] = "%|" + str[0];
                str[1] = medsEmotional.Value ? "1" : "0";
                str[2] = medsStraya.Value ? "1" : "0";
                str[3] = medsStrayaServer.Value;
                str[4] = medsMaxMultiplayerMembers.Value ? "1" : "0";
                str[5] = medsSkipCinematics.Value ? "1" : "0";
                str[6] = medsAutoContinue.Value ? "1" : "0";
                jstr += string.Join("|", str);
            }
            return jstr;
        }
        public static void StringToSettings(string incomingSettings)
        {
            string[] str = incomingSettings.Split("|%|");
            if (str.Length == 2)
            {
                string[] nonMPstr = str[1].Split("|");
                if (nonMPstr.Length >= 1)
                    medsProfane.Value = nonMPstr[0] == "1";
                if (nonMPstr.Length >= 2)
                    medsEmotional.Value = nonMPstr[1] == "1";
                if (nonMPstr.Length >= 3)
                    medsStraya.Value = nonMPstr[2] == "1";
                if (nonMPstr.Length >= 4)
                    medsStrayaServer.Value = nonMPstr[3];
                if (nonMPstr.Length >= 5)
                    medsMaxMultiplayerMembers.Value = nonMPstr[4] == "1";
                if (nonMPstr.Length >= 6)
                    medsSkipCinematics.Value = nonMPstr[5] == "1";
                if (nonMPstr.Length >= 7)
                    medsAutoContinue.Value = nonMPstr[6] == "1";
            }
            str = str[0].Split("|");
            if (str.Length >= 1)
                medsShopRarity.Value = str[0] == "1";
            if (str.Length >= 2)
                medsMapShopCorrupt.Value = str[1] == "1";
            if (str.Length >= 3)
                medsObeliskShopCorrupt.Value = str[2] == "1";
            if (str.Length >= 4)
                medsTownShopCorrupt.Value = str[3] == "1";
            if (str.Length >= 5)
                medsLootCorrupt.Value = str[4] == "1";
            if (str.Length >= 6)
                medsPerkPoints.Value = str[5] == "1";
            if (str.Length >= 7)
                medsCorruptGiovanna.Value = str[6] == "1";
            if (str.Length >= 8)
                medsKeyItems.Value = str[7] == "1";
            if (str.Length >= 9)
                medsAlwaysSucceed.Value = str[8] == "1";
            if (str.Length >= 10)
                medsAlwaysFail.Value = str[9] == "1";
            if (str.Length >= 11)
                medsCraftCorruptedCards.Value = str[10] == "1";
            if (str.Length >= 12)
                medsInfiniteCardCraft.Value = str[11] == "1";
            if (str.Length >= 13)
                medsStockedShop.Value = str[12] == "1";
            if (str.Length >= 14)
                medsSoloShop.Value = str[13] == "1";
            if (str.Length >= 15)
                medsDeveloperMode.Value = str[14] == "1";
            if (str.Length >= 16)
                medsJuice.Value = str[15] == "1";
            if (str.Length >= 17)
                medsUseClaimation.Value = str[16] == "1";
            if (str.Length >= 18)
                medsDiscountDivination.Value = str[17] == "1";
            if (str.Length >= 19)
                medsDiscountDoomroll.Value = str[18] == "1";
            if (str.Length >= 20)
                medsRavingRerolls.Value = str[19] == "1";
            if (str.Length >= 21)
                medsSmallSanitySupplySelling.Value = str[20] == "1";
            if (str.Length >= 22)
                medsModifyPerks.Value = str[21] == "1";
            if (str.Length >= 23)
                medsNoPerkRequirements.Value = str[22] == "1";
            if (str.Length >= 24)
                medsPlentifulPetPurchases.Value = str[23] == "1";
            if (str.Length >= 25)
                medsTravelAnywhere.Value = str[24] == "1";
            if (str.Length >= 26)
                medsNoTravelRequirements.Value = str[25] == "1";
            if (str.Length >= 27)
                medsNoPlayerClassRequirements.Value = str[26] == "1";
            if (str.Length >= 28)
                medsNoPlayerItemRequirements.Value = str[27] == "1";
            if (str.Length >= 29)
                medsNoPlayerRequirements.Value = str[28] == "1";
            if (str.Length >= 30)
                medsOverlyTenergetic.Value = str[29] == "1";
            if (str.Length >= 31)
                medsDiminutiveDecks.Value = str[30] == "1";
            if (str.Length >= 32)
                medsDenyDiminishingDecks.Value = str[31];
            if (str.Length >= 33)
                medsShopBadLuckProtection.Value = float.Parse(str[32]);
            if (str.Length >= 33)
                medsBugfixEquipmentHP.Value = str[33] == "1";
            medsExportSettings.Value = SettingsToString();
        }
    }
}

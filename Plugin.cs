using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Obeliskial_Options
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string ModGUID = "com.meds.obeliskialoptions";
        private const string ModName = "Obeliskial Options";
        public const string ModVersion = "1.2.0";
        public const string ModDate = "20230516";
        private readonly Harmony harmony = new(ModGUID);
        internal static ManualLogSource Log;
        public static int iShopsWithNoPurchase = 0;
        private static bool bUpdatingSettings = false;

        // public static ConfigEntry<bool> medsGetClaimation { get; private set; }

        // Debug
        public static ConfigEntry<bool> medsKeyItems { get; private set; }
        public static ConfigEntry<bool> medsJuiceGold { get; private set; }
        public static ConfigEntry<bool> medsJuiceDust { get; private set; }
        public static ConfigEntry<bool> medsJuiceSupplies { get; private set; }
        public static ConfigEntry<bool> medsDeveloperMode { get; private set; }
        public static ConfigEntry<string> medsExportSettings { get; private set; }
        public static ConfigEntry<string> medsImportSettings { get; private set; }

        // Cards & Decks
        public static ConfigEntry<bool> medsDiminutiveDecks { get; private set; }
        public static ConfigEntry<string> medsDenyDiminishingDecks { get; private set; }
        public static ConfigEntry<bool> medsCraftCorruptedCards { get; private set; }
        public static ConfigEntry<bool> medsInfiniteCardCraft { get; private set; }

        // Characters
        public static ConfigEntry<bool> medsDLCClones { get; private set; }
        public static ConfigEntry<string> medsDLCCloneTwo { get; private set; }
        public static ConfigEntry<string> medsDLCCloneThree { get; private set; }
        public static ConfigEntry<string> medsDLCCloneFour { get; private set; }
        public static ConfigEntry<string> medsDLCCloneTwoName { get; private set; }
        public static ConfigEntry<string> medsDLCCloneThreeName { get; private set; }
        public static ConfigEntry<string> medsDLCCloneFourName { get; private set; }

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
        public static ConfigEntry<int> medsShopBadLuckProtection { get; private set; }
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
        public static ConfigEntry<bool> medsMPLoadAutoCreateRoom { get; private set; }
        public static ConfigEntry<bool> medsMPLoadAutoReady { get; private set; }
        public static ConfigEntry<bool> medsSpacebarContinue { get; private set; }


        // Multiplayer
        public static bool medsMPShopRarity = false;
        public static bool medsMPMapShopCorrupt = false;
        public static bool medsMPObeliskShopCorrupt = false;
        public static bool medsMPTownShopCorrupt = false;
        public static bool medsMPItemCorrupt = false;
        public static bool medsMPPerkPoints = false;
        public static bool medsMPCorruptGiovanna = false;
        public static bool medsMPKeyItems = false;
        public static bool medsMPAlwaysSucceed = false;
        public static bool medsMPAlwaysFail = false;
        public static bool medsMPCraftCorruptedCards = false;
        public static bool medsMPInfiniteCardCraft = false;
        public static bool medsMPStockedShop = false;
        public static bool medsMPSoloShop = false;
        public static bool medsMPDeveloperMode = false;
        public static bool medsMPJuiceGold = false;
        public static bool medsMPJuiceDust = false;
        // public static bool medsMPJuiceSupplies = false;
        public static bool medsMPUseClaimation = false;
        public static bool medsMPDiscountDivination = false;
        public static bool medsMPDiscountDoomroll = false;
        public static bool medsMPRavingRerolls = false;
        public static bool medsMPSmallSanitySupplySelling = false;
        public static bool medsMPModifyPerks = false;
        public static bool medsMPPlentifulPetPurchases = false;
        public static bool medsMPNoPerkRequirements = false;
        public static bool medsMPTravelAnywhere = false;
        public static bool medsMPNoTravelRequirements = false;
        public static bool medsMPNoPlayerClassRequirements = false;
        public static bool medsMPNoPlayerItemRequirements = false;
        public static bool medsMPNoPlayerRequirements = false;
        public static bool medsMPOverlyTenergetic = false;
        public static bool medsMPDiminutiveDecks = false;
        public static string medsMPDenyDiminishingDecks = "";
        public static int medsMPShopBadLuckProtection = 0;
        public static bool medsMPBugfixEquipmentHP = false;
        public static bool medsMPDLCClones = false;
        public static string medsMPDLCCloneTwo = "";
        public static string medsMPDLCCloneThree = "";
        public static string medsMPDLCCloneFour = "";
        public static string[] medsSubclassList = { "mercenary", "sentinel", "berserker", "warden", "ranger", "assassin", "archer", "minstrel", "elementalist", "pyromancer", "loremaster", "warlock", "cleric", "priest", "voodoowitch", "prophet", "bandit" };

        private void Awake()
        {
            Log = Logger;
            // Plugin.medsGetClaimation = this.Config.Bind<bool>("Options", "High Madness - Acquire Claims", false, "(NOT WORKING - NOT EVEN STARTED) Acquire new claims on any madness.");
            
            // Debug
            medsKeyItems = Config.Bind(new ConfigDefinition("Debug", "All Key Items"), false, new ConfigDescription("Give all key items in Adventure Mode. Items are added when you load into a town; if you've already passed the town and want the key items, use Travel Anywhere to go back to town? I'll add more methods in the future :)."));
            medsJuiceGold = Config.Bind(new ConfigDefinition("Debug", "Gold ++"), false, new ConfigDescription("Many cash."));
            medsJuiceDust = Config.Bind(new ConfigDefinition("Debug", "Dust ++"), false, new ConfigDescription("Many cryttals."));
            medsJuiceSupplies = Config.Bind(new ConfigDefinition("Debug", "Supplies ++"), false, new ConfigDescription("Many supplies."));
            medsDeveloperMode = Config.Bind(new ConfigDefinition("Debug", "Developer Mode"), false, new ConfigDescription("Turns on AtO devs’ developer mode. Back up your saves before using!"));
            medsExportSettings = Config.Bind(new ConfigDefinition("Debug", "Export Settings"), "", new ConfigDescription("Export settings (for use with 'Import Settings')."));
            medsImportSettings = Config.Bind(new ConfigDefinition("Debug", "Import Settings"), "", new ConfigDescription("Paste settings here to import them."));

            // Cards & Decks
            medsDiminutiveDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Ignore Minimum Deck Size"), true, new ConfigDescription("Allow you to remove cards even when deck contains less than 15."));
            medsDenyDiminishingDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Card Removal"), "Can Remove Anything", new ConfigDescription("What cards can be removed at the church?", new AcceptableValueList<string>("Cannot Remove Cards", "Cannot Remove Curses", "Can Only Remove Curses", "Can Remove Anything")));
            medsCraftCorruptedCards = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Corrupted Cards"), false, new ConfigDescription("Allow crafting of corrupted cards."));
            medsInfiniteCardCraft = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Infinite Cards"), false, new ConfigDescription("Infinite card crafts (set available card count to 99)."));

            // Characters
            medsDLCClones = Config.Bind(new ConfigDefinition("Characters", "Enable Clones"), true, new ConfigDescription("(IN TESTING) Adds three clone characters to the DLC section of Hero Selection."));
            medsDLCCloneTwo = Config.Bind(new ConfigDefinition("Characters", "Clone 2"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 2?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneThree = Config.Bind(new ConfigDefinition("Characters", "Clone 3"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 3?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneFour = Config.Bind(new ConfigDefinition("Characters", "Clone 4"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 4?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneTwoName = Config.Bind(new ConfigDefinition("Characters", "Clone 2 Name"), "Clone", new ConfigDescription("What should the character in DLC slot 2 be called?"));
            medsDLCCloneThreeName = Config.Bind(new ConfigDefinition("Characters", "Clone 3 Name"), "Copy", new ConfigDescription("What should the character in DLC slot 3 be called?"));
            medsDLCCloneFourName = Config.Bind(new ConfigDefinition("Characters", "Clone 4 Name"), "Counterfeit", new ConfigDescription("What should the character in DLC slot 4 be called?"));

            // Corruption & Madness
            medsSmallSanitySupplySelling = Config.Bind(new ConfigDefinition("Corruption & Madness", "Sell Supplies"), true, new ConfigDescription("Sell supplies on high madness."));
            medsRavingRerolls = Config.Bind(new ConfigDefinition("Corruption & Madness", "Shop Rerolls"), true, new ConfigDescription("Allow multiple shop rerolls on high madness."));
            medsUseClaimation = Config.Bind(new ConfigDefinition("Corruption & Madness", "Use Claims"), true, new ConfigDescription("Use claims on any madness. Note that you cannot _get_ claims on high madness (yet...)."));

            // Events & Nodes
            medsAlwaysFail = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Fail Event Rolls"), false, new ConfigDescription("Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible."));
            medsAlwaysSucceed = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Succeed Event Rolls"), false, new ConfigDescription("Always succeed event rolls, though event text might not match. Critically succeeds if possible."));
            medsNoTravelRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Travel Requirements"), false, new ConfigDescription("(NOT WORKING - shows path to node, but not actual node) Can travel to nodes that are normally invisible (e.g. western treasure node in Faeborg)."));
            medsNoPlayerClassRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Class Requirements"), false, new ConfigDescription("(IN TESTING - BUGGY AF) ignore class requirements? e.g. pretend you have a healer? might let you ignore specific character requirements"));
            medsNoPlayerItemRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Item Requirements"), false, new ConfigDescription("(IN TESTING - BUGGY AF) ignore equipment/pet requirements? e.g. should let you 'drop off the crate' @ Tsnemo's ship?"));
            medsNoPlayerRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Player Requirements"), false, new ConfigDescription("(IN TESTING - BUGGY AF) ignore key item???? requirements."));
            medsTravelAnywhere = Config.Bind(new ConfigDefinition("Events & Nodes", "Travel Anywhere"), false, new ConfigDescription("Travel to any node."));

            // Loot
            medsCorruptGiovanna = Config.Bind(new ConfigDefinition("Loot", "Corrupted Card Rewards"), false, new ConfigDescription("Card rewards are always corrupted (includes divinations)."));
            medsLootCorrupt = Config.Bind(new ConfigDefinition("Loot", "Corrupted Loot Rewards"), false, new ConfigDescription("Make item loot rewards always corrupted."));

            // Party
            /*medsSetTeam = Config.Bind(new ConfigDefinition("Party", "Set Team"), false, new ConfigDescription("(IN TESTING) Force the team composition set below.")); // Automatically turns off afterwards; only use when necessary!"));
            medsSetTeam1 = Config.Bind(new ConfigDefinition("Party", "Set Team 1"), "loremaster", new ConfigDescription("Team member in first slot from the left.", new AcceptableValueList<string>(medsSubclassList)));
            medsSetTeam2 = Config.Bind(new ConfigDefinition("Party", "Set Team 2"), "loremaster", new ConfigDescription("Team member in second slot from the left.", new AcceptableValueList<string>(medsSubclassList)));
            medsSetTeam3 = Config.Bind(new ConfigDefinition("Party", "Set Team 3"), "loremaster", new ConfigDescription("Team member in third slot from the left.", new AcceptableValueList<string>(medsSubclassList)));
            medsSetTeam4 = Config.Bind(new ConfigDefinition("Party", "Set Team 4"), "loremaster", new ConfigDescription("Team member in final slot.", new AcceptableValueList<string>(medsSubclassList)));
            medsPerksFrom1 = Config.Bind(new ConfigDefinition("Party", "Perks From 1"), "mercenary", new ConfigDescription("Which perks should first slot use?", new AcceptableValueList<string>(medsSubclassList)));
            medsPerksFrom2 = Config.Bind(new ConfigDefinition("Party", "Perks From 2"), "ranger", new ConfigDescription("Which perks should second slot use?", new AcceptableValueList<string>(medsSubclassList)));
            medsPerksFrom3 = Config.Bind(new ConfigDefinition("Party", "Perks From 3"), "elementalist", new ConfigDescription("Which perks should third slot use?", new AcceptableValueList<string>(medsSubclassList)));
            medsPerksFrom4 = Config.Bind(new ConfigDefinition("Party", "Perks From 4"), "cleric", new ConfigDescription("Which perks should fourth slot use?", new AcceptableValueList<string>(medsSubclassList)));*/

            // Perks
            medsPerkPoints = Config.Bind(new ConfigDefinition("Perks", "Many Perk Points"), false, new ConfigDescription("(IN TESTING - visually buggy but functional) Set maximum perk points to 1000."));
            medsModifyPerks = Config.Bind(new ConfigDefinition("Perks", "Modify Perks Whenever"), false, new ConfigDescription("(IN TESTING) Change perks whenever you want."));
            medsNoPerkRequirements = Config.Bind(new ConfigDefinition("Perks", "No Perk Requirements"), false, new ConfigDescription("Can select perk without selecting its precursor perks; ignore minimum selected perk count for each row."));

            // Shop
            medsShopRarity = Config.Bind(new ConfigDefinition("Shop", "Adjusted Shop Rarity"), false, new ConfigDescription("Modify shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt."));
            medsShopBadLuckProtection = Config.Bind(new ConfigDefinition("Shop", "Bad Luck Protection"), 10, new ConfigDescription("Increases rarity of shops/loot based on number of shops/loot seen since an item was last acquired. Value/100000*ActNumber = percent increase in item rarity per shop seen without purchase.", new AcceptableValueRange<int>(0, 100000)));
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
            medsMaxMultiplayerMembers = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Max Multiplayer Members"), true, new ConfigDescription("Change the default player count in multiplDefault to 4 players in multiplayer."));
            medsOverlyTenergetic = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Overly Tenergetic"), true, new ConfigDescription("(IN TESTING - visually buggy but functional) Allow characters to have more than 10 energy."));
            medsBugfixEquipmentHP = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Bugfix: Equipment HP"), true, new ConfigDescription("(IN TESTING - visually buggy but functional) Fixes a vanilla bug that allows infinite stacking of HP by buying the same item repeatedly."));
            medsSkipCinematics = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Skip Cinematics"), false, new ConfigDescription("Skip cinematics."));
            medsAutoContinue = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Continue"), false, new ConfigDescription("(IN TESTING - visually buggy but functional) Automatically press 'Continue' in events."));
            medsMPLoadAutoCreateRoom = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Create Room on MP Load"), true, new ConfigDescription("(IN TESTING) Use previous settings to automatically create lobby room when loading multiplayer game."));
            medsMPLoadAutoReady = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Ready on MP Load"), true, new ConfigDescription("(IN TESTING) Automatically readies up non-host players when loading multiplayer game."));
            medsSpacebarContinue = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Spacebar to Continue"), true, new ConfigDescription("(IN TESTING) Spacebar clicks the 'Continue' button in events for you."));

            medsImportSettings.Value = "";
            medsExportSettings.Value = SettingsToString();

            medsKeyItems.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsJuiceGold.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsJuiceDust.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsJuiceSupplies.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDeveloperMode.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsExportSettings.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsImportSettings.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDiminutiveDecks.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDenyDiminishingDecks.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsCraftCorruptedCards.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsInfiniteCardCraft.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsSmallSanitySupplySelling.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsRavingRerolls.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsUseClaimation.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsAlwaysFail.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsAlwaysSucceed.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoTravelRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoPlayerClassRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoPlayerItemRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoPlayerRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsTravelAnywhere.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsCorruptGiovanna.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsLootCorrupt.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsPerkPoints.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsModifyPerks.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoPerkRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsShopRarity.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsShopBadLuckProtection.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsMapShopCorrupt.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsObeliskShopCorrupt.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsTownShopCorrupt.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDiscountDivination.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDiscountDoomroll.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsPlentifulPetPurchases.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsStockedShop.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsSoloShop.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsProfane.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsEmotional.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsStraya.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsStrayaServer.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsMaxMultiplayerMembers.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsOverlyTenergetic.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsBugfixEquipmentHP.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsSkipCinematics.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsAutoContinue.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsMPLoadAutoCreateRoom.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsMPLoadAutoReady.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsSpacebarContinue.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCClones.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneTwo.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneThree.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneFour.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneTwoName.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneThreeName.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDLCCloneFourName.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };

            medsImportSettings.SettingChanged += (obj, args) => { StringToSettings(medsImportSettings.Value); };


            harmony.PatchAll();
            Log.LogInfo($"{ModGUID} {ModVersion} has loaded! Prayge ");
        }
        public static string SettingsToString(bool forMP = false)
        {
            string[] str = new string[36];
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
            str[15] = (medsDLCClones.Value ? "1" : "0") + "&" + medsDLCCloneTwo.Value + "&" + medsDLCCloneThree.Value + "&" + medsDLCCloneFour.Value;
            // str[15] = (medsSetTeam.Value ? "1" : "0") + "&" + medsSetTeam1.Value + "&" + medsSetTeam2.Value + "&" + medsSetTeam3.Value + "&" + medsSetTeam4.Value + "&" + medsPerksFrom1.Value + "&" + medsPerksFrom2.Value + "&" + medsPerksFrom3.Value + "&" + medsPerksFrom4.Value;
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
            str[34] = medsJuiceGold.Value ? "1" : "0";
            str[35] = medsJuiceDust.Value ? "1" : "0";
            string jstr = string.Join("|", str);
            if (!forMP)
            {
                str = new string[14];
                str[0] = medsProfane.Value ? "1" : "0";
                str[0] = "%|" + str[0];
                str[1] = medsEmotional.Value ? "1" : "0";
                str[2] = medsStraya.Value ? "1" : "0";
                str[3] = medsStrayaServer.Value;
                str[4] = medsMaxMultiplayerMembers.Value ? "1" : "0";
                str[5] = medsSkipCinematics.Value ? "1" : "0";
                str[6] = medsAutoContinue.Value ? "1" : "0";
                str[7] = medsJuiceSupplies.Value ? "1" : "0";
                str[8] = medsMPLoadAutoCreateRoom.Value ? "1" : "0";
                str[9] = medsMPLoadAutoReady.Value ? "1" : "0";
                str[10] = medsSpacebarContinue.Value ? "1" : "0";
                str[11] = medsDLCCloneTwoName.Value;
                str[12] = medsDLCCloneThreeName.Value;
                str[13] = medsDLCCloneFourName.Value;
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
                if (nonMPstr.Length >= 8)
                    medsJuiceSupplies.Value = nonMPstr[7] == "1";
                if (nonMPstr.Length >= 9)
                    medsMPLoadAutoCreateRoom.Value = nonMPstr[8] == "1";
                if (nonMPstr.Length >= 10)
                    medsMPLoadAutoReady.Value = nonMPstr[9] == "1";
                if (nonMPstr.Length >= 11)
                    medsSpacebarContinue.Value = nonMPstr[10] == "1";
                if (nonMPstr.Length >= 12)
                    medsDLCCloneTwoName.Value = nonMPstr[11];
                if (nonMPstr.Length >= 13)
                    medsDLCCloneThreeName.Value = nonMPstr[12];
                if (nonMPstr.Length >= 14)
                    medsDLCCloneFourName.Value = nonMPstr[13];
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
            {
                medsDLCClones.Value = str[15].Split("&")[0] == "1";
                medsDLCCloneTwo.Value = str[15].Split("&")[1];
                medsDLCCloneThree.Value = str[15].Split("&")[2];
                medsDLCCloneFour.Value = str[15].Split("&")[3];
            }
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
                medsShopBadLuckProtection.Value = int.Parse(str[32]);
            if (str.Length >= 34)
                medsBugfixEquipmentHP.Value = str[33] == "1";
            if (str.Length >= 35)
                medsJuiceGold.Value = str[34] == "1";
            if (str.Length >= 36)
                medsJuiceDust.Value = str[35] == "1";
            medsExportSettings.Value = SettingsToString();
            medsImportSettings.Value = "";
        }

        public static void SaveMPSettings(string _newSettings)
        {
            Plugin.Log.LogInfo("RECEIVING SETTINGS: " + _newSettings);
            string[] str = _newSettings.Split("|");
            if (str.Length >= 1)
                medsMPShopRarity = str[0] == "1";
            if (str.Length >= 2)
                medsMPObeliskShopCorrupt = str[1] == "1";
            if (str.Length >= 3)
                medsMPMapShopCorrupt = str[2] == "1";
            if (str.Length >= 4)
                medsMPTownShopCorrupt = str[3] == "1";
            if (str.Length >= 5)
                medsMPItemCorrupt = str[4] == "1";
            if (str.Length >= 6)
                medsMPPerkPoints = str[5] == "1";
            if (str.Length >= 7)
                medsMPCorruptGiovanna = str[6] == "1";
            if (str.Length >= 8)
                medsMPKeyItems = str[7] == "1";
            if (str.Length >= 9)
                medsMPAlwaysSucceed = str[8] == "1";
            if (str.Length >= 10)
                medsMPAlwaysFail = str[9] == "1";
            if (str.Length >= 11)
                medsMPCraftCorruptedCards = str[10] == "1";
            if (str.Length >= 12)
                medsMPInfiniteCardCraft = str[11] == "1";
            if (str.Length >= 13)
                medsMPStockedShop = str[12] == "1";
            if (str.Length >= 14)
                medsMPSoloShop = str[13] == "1";
            if (str.Length >= 15)
                medsMPDeveloperMode = str[14] == "1";
            if (str.Length >= 16)
            {
                bool SettingsChanged = false;
                if (!(str[15].Split("&")[0] == "1") == medsMPDLCClones || (str[15].Split("&")[0] == "1" && (medsMPDLCCloneTwo != str[15].Split("&")[1] || medsMPDLCCloneThree != str[15].Split("&")[2] || medsMPDLCCloneFour != str[15].Split("&")[3]))) // different to current setting!
                    SettingsChanged = true;
                medsMPDLCClones = str[15].Split("&")[0] == "1";
                medsMPDLCCloneTwo = str[15].Split("&")[1];
                medsMPDLCCloneThree = str[15].Split("&")[2];
                medsMPDLCCloneFour = str[15].Split("&")[3];
                if (SettingsChanged)
                    SubClassReplace();
            }
            if (str.Length >= 17)
                medsMPUseClaimation = str[16] == "1";
            if (str.Length >= 18)
                medsMPDiscountDivination = str[17] == "1";
            if (str.Length >= 19)
                medsMPDiscountDoomroll = str[18] == "1";
            if (str.Length >= 20)
                medsMPRavingRerolls = str[19] == "1";
            if (str.Length >= 21)
                medsMPSmallSanitySupplySelling = str[20] == "1";
            if (str.Length >= 22)
                medsMPModifyPerks = str[21] == "1";
            if (str.Length >= 23)
                medsMPNoPerkRequirements = str[22] == "1";
            if (str.Length >= 24)
                medsMPPlentifulPetPurchases = str[23] == "1";
            if (str.Length >= 25)
                medsMPTravelAnywhere = str[24] == "1";
            if (str.Length >= 26)
                medsMPNoTravelRequirements = str[25] == "1";
            if (str.Length >= 27)
                medsMPNoPlayerClassRequirements = str[26] == "1";
            if (str.Length >= 28)
                medsMPNoPlayerItemRequirements = str[27] == "1";
            if (str.Length >= 29)
                medsMPNoPlayerRequirements = str[28] == "1";
            if (str.Length >= 30)
                medsMPOverlyTenergetic = str[29] == "1";
            if (str.Length >= 31)
                medsMPDiminutiveDecks = str[30] == "1";
            if (str.Length >= 32)
                medsMPDenyDiminishingDecks = str[31];
            if (str.Length >= 33)
                medsMPShopBadLuckProtection = int.Parse(str[32]);
            if (str.Length >= 34)
                medsMPBugfixEquipmentHP = str[33] == "1";
            if (str.Length >= 35)
                medsMPJuiceGold = str[34] == "1";
            if (str.Length >= 36)
                medsMPJuiceDust = str[35] == "1";
            Plugin.Log.LogInfo("RECEIVED " + str.Length + " SETTINGS!");
        }

        public static void SendSettingsMP()
        {
            int inum = 666666;
            bool b = true;
            string _values = Plugin.SettingsToString(true);
            SaveMPSettings(_values);
            PhotonView medsphotonView = PhotonView.Get((Component)NetworkManager.Instance);
            // send to other players
            Plugin.Log.LogInfo("SHARING SETTINGS: " + _values);
            medsphotonView.RPC("NET_LoadScene", RpcTarget.Others, (object)_values, (object)b, (object)inum);
            bUpdatingSettings = false;
        }

        public static void SettingsUpdated()
        {
            bUpdatingSettings = true;
            medsExportSettings.Value = SettingsToString();
            if ((UnityEngine.Object)NetworkManager.Instance != (UnityEngine.Object)null && (UnityEngine.Object)GameManager.Instance != (UnityEngine.Object)null && GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
            {
                SendSettingsMP();
            }
            else
            {
                bUpdatingSettings = false;
                SubClassReplace();
            }
        }

        public static void SubClassReplace()
        {
            // the below subclassreplace was  ???
            /*foreach (KeyValuePair<string, SubClassData> keyValuePair in Globals.Instance.SubClass)
            {
                if ((UnityEngine.Object)keyValuePair.Value != (UnityEngine.Object)null && keyValuePair.Value.MainCharacter)
                {
                    SubClassData medsSCD = keyValuePair.Value as SubClassData;
                    if (keyValuePair.Key == Plugin.)
                    Globals.Instance.SubClass.Remove("")
                }
            }*/

            Plugin.Log.LogInfo("CREATECLONES START");
            if (!(Plugin.IsHost() ? Plugin.medsDLCClones.Value : Plugin.medsMPDLCClones))
                return;
            string medsSCDId = "";
            string medsSCDName = "";
            string medsSCDReplaceWith = "";
            // SubClassData medsSCD = new();
            for (int chr = 1; chr <= 3; chr++)
            {
                if (chr == 1)
                {
                    medsSCDId = "medsdlctwo";
                    medsSCDName = Plugin.medsDLCCloneTwoName.Value;
                    medsSCDReplaceWith = (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo);
                }
                else if (chr == 2)
                {
                    medsSCDId = "medsdlcthree";
                    medsSCDName = Plugin.medsDLCCloneThreeName.Value;
                    medsSCDReplaceWith = (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree);
                }
                else if (chr == 3)
                {
                    medsSCDId = "medsdlcfour";
                    medsSCDName = Plugin.medsDLCCloneFourName.Value;
                    medsSCDReplaceWith = (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour);
                }
                SubClassData medsSCD = UnityEngine.Object.Instantiate<SubClassData>(Globals.Instance.SubClass[medsSCDReplaceWith]);
                medsSCD.Id = medsSCDId;
                medsSCD.CharacterName = medsSCDName;
                medsSCD.OrderInList = chr;
                medsSCD.SubClassName = medsSCDId;
                medsSCD.ExpansionCharacter = true;
                Globals.Instance.SubClass[medsSCDId] = medsSCD;
                Plugin.Log.LogInfo(medsSCDId + " ADDED!");
            }
            Plugin.Log.LogInfo("CREATECLONES END");
            // add duplicate cardbacks for medsDLC characters
            Dictionary<string, CardbackData> medsCardbackDataSource = Traverse.Create(Globals.Instance).Field("_CardbackDataSource").GetValue<Dictionary<string, CardbackData>>();
            for (int a = 97; a <= 122; a++)
            {
                if (medsCardbackDataSource.ContainsKey("medsdlctwo" + ((char)a).ToString()))
                    medsCardbackDataSource.Remove("medsdlctwo" + ((char)a).ToString());
                if (medsCardbackDataSource.ContainsKey("medsdlcthree" + ((char)a).ToString()))
                    medsCardbackDataSource.Remove("medsdlcthree" + ((char)a).ToString());
                if (medsCardbackDataSource.ContainsKey("medsdlcfour" + ((char)a).ToString()))
                    medsCardbackDataSource.Remove("medsdlcfour" + ((char)a).ToString());
            }
            Dictionary<string, CardbackData> medsCardbacksToAdd = new();
            int b = 97;
            int c = 97;
            int d = 97;
            // loop through all cardbacks, duplicating those used for the current clones
            foreach (KeyValuePair<string, CardbackData> keyValuePair in medsCardbackDataSource)
            {
                // Plugin.Log.LogInfo(keyValuePair.Key + medsCardbackDataSource.Count);
                if ((UnityEngine.Object)keyValuePair.Value.CardbackSubclass != (UnityEngine.Object)null && keyValuePair.Value.CardbackSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo))
                {
                    CardbackData medsSingleCardback = UnityEngine.Object.Instantiate<CardbackData>(medsCardbackDataSource[keyValuePair.Key]);
                    medsSingleCardback.CardbackId = "medsdlctwo" + ((char)b).ToString();
                    medsSingleCardback.CardbackSubclass = Globals.Instance.SubClass["medsdlctwo"];
                    medsCardbacksToAdd[medsSingleCardback.CardbackId] = medsSingleCardback;
                    b++;
                }
                if ((UnityEngine.Object)keyValuePair.Value.CardbackSubclass != (UnityEngine.Object)null && keyValuePair.Value.CardbackSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree))
                {
                    CardbackData medsSingleCardback = UnityEngine.Object.Instantiate<CardbackData>(medsCardbackDataSource[keyValuePair.Key]);
                    medsSingleCardback.CardbackId = "medsdlcthree" + ((char)c).ToString();
                    medsSingleCardback.CardbackSubclass = Globals.Instance.SubClass["medsdlcthree"];
                    medsCardbacksToAdd[medsSingleCardback.CardbackId] = medsSingleCardback;
                    c++;
                }
                if ((UnityEngine.Object)keyValuePair.Value.CardbackSubclass != (UnityEngine.Object)null && keyValuePair.Value.CardbackSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour))
                {
                    CardbackData medsSingleCardback = UnityEngine.Object.Instantiate<CardbackData>(medsCardbackDataSource[keyValuePair.Key]);
                    medsSingleCardback.CardbackId = "medsdlcfour" + ((char)d).ToString();
                    medsSingleCardback.CardbackSubclass = Globals.Instance.SubClass["medsdlcfour"];
                    medsCardbacksToAdd[medsSingleCardback.CardbackId] = medsSingleCardback;
                    d++;
                }
            }
            medsCardbacksToAdd = medsCardbackDataSource.Concat(medsCardbacksToAdd).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
            Traverse.Create(Globals.Instance).Field("_CardbackDataSource").SetValue(medsCardbacksToAdd);

            // add duplicate skins for medsDLC characters
            Dictionary<string, SkinData> medsSkinDataSource = Traverse.Create(Globals.Instance).Field("_SkinDataSource").GetValue<Dictionary<string, SkinData>>();
            for (int a = 97; a <= 122; a++)
            {
                if (medsSkinDataSource.ContainsKey("medsdlctwo" + ((char)a).ToString()))
                    medsSkinDataSource.Remove("medsdlctwo" + ((char)a).ToString());
                if (medsSkinDataSource.ContainsKey("medsdlcthree" + ((char)a).ToString()))
                    medsSkinDataSource.Remove("medsdlcthree" + ((char)a).ToString());
                if (medsSkinDataSource.ContainsKey("medsdlcfour" + ((char)a).ToString()))
                    medsSkinDataSource.Remove("medsdlcfour" + ((char)a).ToString());
            }
            Dictionary<string, SkinData> medsSkinsToAdd = new();
            b = 97;
            c = 97;
            d = 97;
            // loop through all skins, duplicating those used for the current clones
            foreach (KeyValuePair<string, SkinData> keyValuePair in medsSkinDataSource)
            {
                if ((UnityEngine.Object)keyValuePair.Value.SkinSubclass != (UnityEngine.Object)null && keyValuePair.Value.SkinSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo))
                {
                    SkinData medsSingleSkin = UnityEngine.Object.Instantiate<SkinData>(medsSkinDataSource[keyValuePair.Key]);
                    medsSingleSkin.SkinId = "medsdlctwo" + ((char)b).ToString();
                    medsSingleSkin.SkinSubclass = Globals.Instance.SubClass["medsdlctwo"];
                    medsSkinsToAdd[medsSingleSkin.SkinId] = medsSingleSkin;
                    b++;
                }
                if ((UnityEngine.Object)keyValuePair.Value.SkinSubclass != (UnityEngine.Object)null && keyValuePair.Value.SkinSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree))
                {
                    SkinData medsSingleSkin = UnityEngine.Object.Instantiate<SkinData>(medsSkinDataSource[keyValuePair.Key]);
                    medsSingleSkin.SkinId = "medsdlcthree" + ((char)c).ToString();
                    medsSingleSkin.SkinSubclass = Globals.Instance.SubClass["medsdlcthree"];
                    medsSkinsToAdd[medsSingleSkin.SkinId] = medsSingleSkin;
                    c++;
                }
                if ((UnityEngine.Object)keyValuePair.Value.SkinSubclass != (UnityEngine.Object)null && keyValuePair.Value.SkinSubclass.Id.ToLower() == (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour))
                {
                    SkinData medsSingleSkin = UnityEngine.Object.Instantiate<SkinData>(medsSkinDataSource[keyValuePair.Key]);
                    medsSingleSkin.SkinId = "medsdlcfour" + ((char)d).ToString();
                    medsSingleSkin.SkinSubclass = Globals.Instance.SubClass["medsdlcfour"];
                    medsSkinsToAdd[medsSingleSkin.SkinId] = medsSingleSkin;
                    d++;
                }
            }
            medsSkinsToAdd = medsSkinDataSource.Concat(medsSkinsToAdd).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
            Traverse.Create(Globals.Instance).Field("_SkinDataSource").SetValue(medsSkinsToAdd);
        }
        public static bool IsHost()
        {
            if ((GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()) || !GameManager.Instance.IsMultiplayer())
                return true;
            return false;
        }
        /* Maybe later, Rebecca.
         * 
        public static string SubClass2Name()
        {
            string medsName = "";

            return medsName;
        }*/

        public static void SaveServerSelection()
        {
            switch (Plugin.medsStrayaServer.Value)
            {
                case "asia":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 0);
                    break;
                case "au":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 1);
                    break;
                case "cae":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 2);
                    break;
                case "eu":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 3);
                    break;
                case "in":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 4);
                    break;
                case "jp":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 5);
                    break;
                case "ru":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 6);
                    break;
                case "rue":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 7);
                    break;
                case "za":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 8);
                    break;
                case "sa":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 9);
                    break;
                case "kr":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 10);
                    break;
                case "us":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 11);
                    break;
                case "usw":
                    SaveManager.SaveIntoPrefsInt("networkRegion", 12);
                    break;
                default:
                    break;
            }
        }
    }
}

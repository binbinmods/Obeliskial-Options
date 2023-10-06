using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static Enums;
using TMPro;
using Steamworks.Data;
using Steamworks;
using System.Text.RegularExpressions;

/*
FULL LIST OF ATO CLASSES->METHODS THAT ARE PATCHED:
AlertManager
    IsActive
AtOManager
    SaveBoughtItem
    NET_SaveBoughtItem
    CharInTown
    GetTownTier
    IsTownRerollAvailable
    SetCurrentNode
    GenerateObeliskMap
    AddItemToHero
    GetPlayerGold
    GetPlayerDust
    NodeScore
    CalculateScore
    UpgradeTownTier
BotonSkin
    OnMouseUp
CardCraftManager
    CanCraftThisCard
    SetMaxQuantity
    GetCardAvailability
    ShowElements
    ShowListCardsForCraft
CardItem
    OnMouseUp
    OnMouseUpController
    ShowEmoteIcon
    RemoveEmoteIcon
    HaveEmoteIcon
Character
    GetTraitAuraCurseModifiers
    ModifyEnergy
    SetAura
    GetDrawCardsTurn
    GetDrawCardsTurnForDisplayInDeck
    GetAuraCurseQuantityModification
CinematicManager
    DoCinematic
ConflictManager
    EnableButtonsForPlayerChoosing
EmoteManager
    Init
    SelectNextCharacter
    SetBlocked
EmoteTarget
    SetIcons
    SetActiveHeroOnCardEmoteButton
EventManager
    FinalResolution
    Start
    Ready
    SetEvent
Functions
    GetCardByRarity
Globals
    Awake
    CreateGameContent
    CreateAuraCurses
    CreateCardClones
    CreateCharClones
    CreateTraitClones
    GetLootData
    GetCostReroll
    GetDivinationCost
HeroSelectionManager
    Start
    StartCo
    ShowFollowStatus
HeroSelection
    SetSprite
    SetSpriteSilueta
InputController
    DoKeyBinding
IntroNewGameManager
    Start
Item
    DoItem
LobbyManager
    InitLobby
    ShowCreate
Loot
    GetLootItems
MadnessManager
    IsActive
MainMenuManager
    Start
    SetMenuCurrentProfile
    Multiplayer
    JoinMultiplayer
MapManager
    CanTravelToThisNode
    DrawNodes
    Awake
    IncludeMapPrefab
    TravelToThisNode
    BeginMapContinue
    PlayerSelectedNode
    NET_PlayerSelectedNode
    GetNodeFromId
    GetMapNodes
    DrawArrow
MatchManager
    SendEmoteCard
    DoEmoteCard
    SetCharactersPing
    EmoteTarget
    DealNewCard
    GenerateNewCard
    GenerateNewCardCo
NetworkManager
    LoadScene
    NET_LoadScene
PerkNode
    OnMouseUp
    OnMouseEnter
    SetIconLock
    SetLocked
    SetRequired
PerkTree
    CanModify
    SelectPerk
    Show
ProfanityFilter
    CensorString
PlayerManager
    IsCardUnlocked
    GainSupply
    GetPlayerSupplyActual
    IsHeroUnlocked
    GetProgress
    ModifyProgress
PlayerUIManager
    SetGold
    SetDust
    SetSupply
RewardsManager
    NET_CardSelected
    Start
    StartCo
SaveManager
    RestorePlayerData
SettingsManager
    IsActive
SteamManager
    DoSteam
    RestorePlayerData
    SetScore
    SetObeliskScore
Texts
    GetText
TomeManager
    SelectTomeCards
TownManager
    Start
    ShowButtons
TownUpgradeWindow
    SetButtons
Trait
    DoTrait
    TextChargesLeft
UIEnergySelector
    TurnOn
[various onmouseups to disable them when F1locked: BotHeroChar, BotonCardback, BotonEndTurn, BotonFilter, BotonGeneric, BotonMenuGameMode, BotonRollover, BotonScore, BotonSkin, BotonSupply, botTownUpgrades, BoxPlayer, CardCraftSelectorEnergy, CardCraftSelectorRarity, CardItem, CardVertical, CharacterGOItem, CharacterItem, CharacterLoot, CharPopupClose, CombatTarget, DeckInHero, DeckPile, EmoteManager, HeroSelection, InitiativePortrait, ItemCombatIcon, Node, OverCharacter, PerkChallengeItem, PerkColumnItem, PerkNode, RandomHeroSelector, Reply, Tomebutton, TomeEdge, TomeNumber, TomeRun, TownBuilding, TraitLevel]
*/

namespace Obeliskial_Options
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.meds.obeliskialoptions";
        private const string ModName = "Obeliskial Options";
        public const string ModVersion = "1.6.4";
        public const int ModDate = 20231006;
        private readonly Harmony harmony = new(ModGUID);
        internal static ManualLogSource Log;
        public static int iShopsWithNoPurchase = 0;
        private static bool bUpdatingSettings = false;
        public static string[] medsSubclassList = { "mercenary", "sentinel", "berserker", "warden", "ranger", "assassin", "archer", "minstrel", "elementalist", "pyromancer", "loremaster", "warlock", "cleric", "priest", "voodoowitch", "prophet", "bandit", "fallen", "paladin" };
        public static string medsDLCCloneTwoSkin = "medsdlctwoa";
        public static string medsDLCCloneThreeSkin = "medsdlcthreea";
        public static string medsDLCCloneFourSkin = "medsdlcfoura";
        public static string medsDLCCloneTwoCardback = "medsdlctwoa";
        public static string medsDLCCloneThreeCardback = "medsdlcthreea";
        public static string medsDLCCloneFourCardback = "medsdlcfoura";
        public static Dictionary<string, CardData> medsCardsSource = new();
        public static Dictionary<string, SubClassData> medsSubClassesSource = new();
        public static Dictionary<string, CardbackData> medsCardbacksSource = new();
        public static Dictionary<string, SkinData> medsSkinsSource = new();
        public static Dictionary<string, TraitData> medsTraitsSource = new();
        public static List<string> medsCustomTraitsSource = new();
        public static Dictionary<string, NPCData> medsNPCsSource = new();
        public static Dictionary<string, AuraCurseData> medsAurasCursesSource = new();
        public static Dictionary<string, NodeData> medsNodeDataSource = new();
        public static Dictionary<string, LootData> medsLootDataSource = new();
        public static Dictionary<string, PerkData> medsPerksSource = new();
        public static Dictionary<string, PerkNodeData> medsPerksNodesSource = new();
        public static Dictionary<string, ChallengeTrait> medsChallengeTraitsSource = new();
        public static Dictionary<string, CombatData> medsCombatDataSource = new();
        public static Dictionary<string, EventData> medsEventDataSource = new();
        public static Dictionary<string, EventRequirementData> medsEventRequirementDataSource = new();
        public static Dictionary<string, ZoneData> medsZoneDataSource = new();
        public static SortedDictionary<string, KeyNotesData> medsKeyNotesDataSource = new();
        public static Dictionary<string, ChallengeData> medsChallengeDataSource = new();
        public static Dictionary<string, PackData> medsPackDataSource = new();
        public static Dictionary<string, ItemData> medsItemDataSource = new();
        public static Dictionary<string, CardPlayerPackData> medsCardPlayerPackDataSource = new();
        public static Dictionary<string, CorruptionPackData> medsCorruptionPackDataSource = new();
        public static Dictionary<string, CinematicData> medsCinematicDataSource = new();
        public static Dictionary<string, Sprite> medsSprites = new();
        public static Dictionary<int, TierRewardData> medsTierRewardDataSource = new();
        public static Dictionary<string, Node> medsNodeSource = new();
        public static Dictionary<string, string[]> medsSecondRunImport = new();
        public static Dictionary<string, string> medsSecondRunImport2 = new();
        public static Dictionary<string[], string> medsSecondRunImport3 = new();
        public static Dictionary<string, string> medsCardsNeedingItems = new();
        public static Dictionary<string, string> medsCardsNeedingItemEnchants = new();
        public static List<string> medsDropOnlyItems = new();
        public static List<string> medsCustomUnlocks = new();
        public static TMP_SpriteAsset medsFallbackSpriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        public static Dictionary<string, AudioClip> medsAudioClips = new();
        public static Dictionary<string, Sprite> medsVanillaSprites = new();
        public static Dictionary<string, GameObject> medsGOs = new();
        public static Dictionary<string, string> medsNodeCombatEventRelation = new();
        public static Dictionary<string, CardPlayerPairsPackData> medsCardPlayerPairsPackDataSource = new();
        public static Dictionary<string, string> medsNodeEvent = new();
        public static Dictionary<string, int> medsNodeEventPercent = new();
        public static Dictionary<string, int> medsNodeEventPriority = new();
        public static Dictionary<string, string> medsTexts = new();
        public static Dictionary<string, ThermometerTierData> medsThermometerTierData = new();
        public static Dictionary<string, string> medsSecondRunCombatEvent = new();
        public static Dictionary<string, string> medsSecondRunCinematicCombat = new();
        public static Dictionary<string, string> medsSecondRunCinematicEvent = new();
        public static Dictionary<string, string[]> medsSecondRunNodesConnected = new();
        public static Dictionary<string, EventReplyDataText> medsEventReplyDataText = new();
        public static Dictionary<string, string> medsCardsNeedingSummonUnits = new();
        public static float medsBLPTownTierPower = 5f;
        public static float medsBLPRollPower = 1f;
        public static float medsBLPMythicMult = 1f;
        public static float medsBLPEpicMult = 8f;
        public static float medsBLPRareMult = 128f;
        public static float medsBLPUncommonMult = 256f;
        public static List<string> medsDoNotDropList = new List<string>() { "asmody", "asmodyrare", "betty", "bettyrare", "boneclaws", "boneclawsa", "boneclawsb", "boneclawsrare", "brokenitem", "bunny", "bunnyrare", "burneditem", "champy", "champyrare", "chompy", "chompyrare", "chumpy", "chumpyrare", "combatbandages", "combatbandagesa", "combatbandagesb", "armageddon", "armageddona", "armageddonb", "armageddonrare", "ashysky", "ashyskya", "ashyskyb", "ashyskyrare", "backlash", "backlasha", "backlashb", "backlashrare", "bloodpuddle", "bomblottery", "bomblotterya", "bomblotteryb", "bomblotteryrare", "burningweapons", "burningweaponsa", "burningweaponsb", "burningweaponsrare", "chaospuddle", "chaoticwind", "chaoticwinda", "chaoticwindb", "chaoticwindrare", "coldfront", "colorfulpuddle", "colorfulpuddlea", "colorfulpuddleb", "colorfulpuddlerare", "darkpuddle", "deathgrip", "electricpuddle", "empower", "empowera", "empowerb", "empowerrare", "forestallies", "fungaloutbreak", "fungaloutbreaka", "fungaloutbreakb", "fungaloutbreakrare", "heavenlyarmaments", "heavenlyarmamentsa", "heavenlyarmamentsb", "heavenlyarmamentsrare", "heavyweaponry", "heavyweaponrya", "heavyweaponryb", "heavyweaponryrare", "hexproof", "hexproofa", "hexproofb", "hexproofrare", "holypuddle", "hypotermia", "hypotermiaa", "hypotermiab", "hypotermiarare", "icypuddle", "ironclad", "ironclada", "ironcladb", "ironcladrare", "lavabursts", "lavapuddle", "livingforest", "livingforesta", "livingforestb", "livingforestrare", "lonelyblob", "lonelybloba", "lonelyblobb", "lonelyblobrare", "meatfeast", "meatfeasta", "meatfeastb", "melancholy", "melancholya", "melancholyb", "melancholyrare", "metalpuddle", "noxiousparasites", "noxiousparasitesa", "noxiousparasitesb", "noxiousparasitesrare", "pacifism", "pacifisma", "pacifismb", "pacifismrare", "poisonfields", "poisonfieldsa", "poisonfieldsb", "poisonfieldsrare", "putrefaction", "putrefactiona", "putrefactionb", "putrefactionrare", "resurrection", "resurrectiona", "resurrectionb", "revenge", "revengea", "revengeb", "revengerare", "rosegarden", "rosegardena", "rosegardenb", "rosegardenrare", "sacredground", "sacredgrounda", "sacredgroundb", "sacredgroundrare", "snowfall", "snowfalla", "snowfallb", "snowfallrare", "spookynight", "starrynight", "starrynighta", "starrynightb", "starrynightrare", "subzero", "subzeroa", "subzerob", "subzerorare", "sugarrush", "thornproliferation", "thornproliferationa", "thornproliferationb", "thornproliferationrare", "thunderstorm", "thunderstorma", "thunderstormb", "thunderstormrare", "toxicpuddle", "trickortreat", "upwind", "upwinda", "upwindb", "upwindrare", "vigorous", "vigorousa", "vigorousb", "vigorousrare", "waterpuddle", "windsofamnesia", "windsofamnesiaa", "windsofamnesiab", "windsofamnesiarare", "cursedjewelering", "daley", "daleyrare", "bloodblobpet", "bloodblobpetrare", "chaosblobpet", "chaosblobpetrare", "darkblobpet", "darkblobpetrare", "electricblobpet", "electricblobpetrare", "holyblobpet", "holyblobpetrare", "icyblobpet", "icyblobpetrare", "lavablobpet", "lavablobpetrare", "metalblobpet", "metalblobpetrare", "toxicblobpet", "toxicblobpetrare", "waterblobpet", "waterblobpetrare", "familyjewels", "familyjewelsa", "familyjewelsb", "flamy", "flamyrare", "forestbanner", "forestbannera", "forestbannerb", "harley", "harleya", "harleyb", "harleyrare", "heavypackage", "hightchancellorstaff", "hightchancellorstaffa", "hightchancellorstaffb", "hightchancellorstaffrare", "jinglebell", "jinglebella", "jinglebellb", "liante", "lianterare", "meatbag", "meatbaga", "meatbagb", "mozzy", "mozzyrare", "oculy", "oculyrare", "orby", "orbyrare", "powerglove", "powerglovea", "powergloveb", "prophetstaff", "prophetstaffa", "prophetstaffb", "prophetstaffrare", "raggeddoll", "raggeddolla", "raggeddollb", "rangerarmor", "rangerarmora", "rangerarmorb", "reforgedcore", "reforgedcorea", "reforgedcoreb", "sharpy", "sharpyrare", "slimy", "slimyrare", "soullantern", "soullanterna", "soullanternb", "stormy", "stormyrare", "thewolfslayer", "thewolfslayera", "thewolfslayerb", "thewolfslayerrare", "tombstone", "venomflask", "venomflaska", "venomflaskb", "wolfy", "wolfyrare", "woodencrosier", "woodencrosiera", "woodencrosierb", "woodencrosierrare", "corruptedplate", "corruptedplatea", "corruptedplateb", "corruptedplaterare", "cuby", "cubyd", "cubydrare", "cubyrare", "familyjewelsrare", "fenny", "fennyrare", "gildedplate", "gildedplatea", "gildedplateb", "gildedplaterare", "scaraby", "scarabyrare" };
        public static List<string> medsDropOnlySoU = new List<string>() { "architectsring", "architectsringrare", "blackpyramid", "blackpyramidrare", "burialmask", "burialmaskrare", "crimsonraiment", "crimsonraimentrare", "desertjam", "desertjamrare", "durandal", "durandalrare", "energyshield", "energyshield rare", "energyshieldrare", "fistofthedamned", "fistofthedamnedrare", "holyrune", "holyrunerare", "lunaring", "lunaringrare", "necromancerrobe", "necromancerroberare", "sacredaxe", "sacredaxerare", "scarabshield", "scarabshieldrare", "shadowrune", "shadowrunerare", "solring", "solringrare", "suppressionhelmet", "suppressionhelmetrare", "tessaract", "tessaractrare", "thejuggernaut", "thejuggernautrare", "topazring", "topazringrare", "turban", "turbanrare", "undeathichor", "undeathichorrare", "unholyhammer", "unholyhammerrare" };
        public static List<string> medsKeepRequirements = new List<string>() { "_demo", "_tier1", "_tier2", "_tier3", "_tier4", "caravan", "crocomenburn", "ulmininup", "ulminindown", "ulmininportal", "ulmininsanddown" };
        public static List<string> medsObeliskNodes = new List<string>() { "sen_34", "aqua_36", "faen_39", "ulmin_40", "velka_33" };
        public static int medsMaxHeroesInClass = 6;
        public static Dictionary<string, ZoneDataText> medsCustomZones = new();
        public static Dictionary<string, GameObject> medsCustomZoneGOs = new();
        public static Dictionary<string, List<NodeDataText>> medsNodesByZone = new();
        public static bool medsLoadedCustomNodes = false;
        public static Dictionary<string, Vector3> medsNodePositions = new();
        public static GameObject medsBaseRoadGO = (GameObject)null;
        public static Dictionary<string, List<Vector3>> medsCustomRoads = new();
        public static GameObject medsInvisibleGOHolder = new();
        public static List<string> medsVanillaIntroNodes = new List<string>() { "sen_0", "tutorial_0", "secta_0", "spider_0", "forge_0", "sewers_0", "sewers_1", "wolf_0", "pyr_0", "velka_0", "aqua_0", "voidlow_0", "faen_0", "ulmin_0", "voidhigh_0" };
        public static GameObject medsZoneTransitionGO = (GameObject)null;
        public static Dictionary<string, PrestigeDeck> medsPrestigeDecks = new();

        // public static Dictionary<string, SubClassData> medsCustomSubClassData = new();


        // public static ConfigEntry<bool> medsGetClaimation { get; private set; }

        // Debug
        public static ConfigEntry<bool> medsKeyItems { get; private set; }
        public static ConfigEntry<bool> medsJuiceGold { get; private set; }
        public static ConfigEntry<bool> medsJuiceDust { get; private set; }
        public static ConfigEntry<bool> medsJuiceSupplies { get; private set; }
        public static ConfigEntry<bool> medsDeveloperMode { get; private set; }
        public static ConfigEntry<string> medsExportSettings { get; private set; }
        public static ConfigEntry<string> medsImportSettings { get; private set; }
        // public static ConfigEntry<bool> medsExportPlayerProfiles { get; private set; }
        // public static ConfigEntry<bool> medsImportPlayerProfiles { get; private set; }
        public static ConfigEntry<bool> medsVanillaContentLog { get; private set; }

        // Cards & Decks
        public static ConfigEntry<int> medsDiminutiveDecks { get; private set; }
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
        public static ConfigEntry<bool> medsOver50s { get; private set; }
        //public static ConfigEntry<bool> medsCustomContent { get; private set; }
        public static ConfigEntry<bool> medsExportJSON { get; private set; }
        public static ConfigEntry<bool> medsExportSprites { get; private set; }

        // Corruption & Madness
        public static ConfigEntry<bool> medsSmallSanitySupplySelling { get; private set; }
        public static ConfigEntry<bool> medsRavingRerolls { get; private set; }
        public static ConfigEntry<bool> medsUseClaimation { get; private set; }

        // Events & Nodes
        // public static ConfigEntry<bool> medsAlwaysFail { get; private set; }
        // public static ConfigEntry<bool> medsAlwaysSucceed { get; private set; }
        public static ConfigEntry<bool> medsNoClassRequirements { get; private set; }
        public static ConfigEntry<bool> medsNoEquipmentRequirements { get; private set; }
        public static ConfigEntry<bool> medsNoKeyItemRequirements { get; private set; }
        public static ConfigEntry<bool> medsTravelAnywhere { get; private set; }
        public static ConfigEntry<bool> medsVisitAllZones { get; private set; }

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
        public static ConfigEntry<bool> medsDropShop { get; private set; }

        // Should Be Vanilla
        public static ConfigEntry<bool> medsProfane { get; private set; }
        public static ConfigEntry<bool> medsEmotional { get; private set; }
        public static ConfigEntry<bool> medsStraya { get; private set; }
        public static ConfigEntry<string> medsStrayaServer { get; private set; }
        public static ConfigEntry<bool> medsMaxMultiplayerMembers { get; private set; }
        public static ConfigEntry<bool> medsOverlyTenergetic { get; private set; }
        public static ConfigEntry<bool> medsOverlyCardergetic { get; private set; }
        public static ConfigEntry<bool> medsBugfixEquipmentHP { get; private set; }
        public static ConfigEntry<bool> medsSkipCinematics { get; private set; }
        public static ConfigEntry<bool> medsAutoContinue { get; private set; }
        public static ConfigEntry<bool> medsMPLoadAutoCreateRoom { get; private set; }
        public static ConfigEntry<bool> medsMPLoadAutoReady { get; private set; }
        public static ConfigEntry<bool> medsSpacebarContinue { get; private set; }
        public static ConfigEntry<int> medsConflictResolution { get; private set; }
        public static ConfigEntry<bool> medsAllThePets { get; private set; }

        // Combat
        // public static ConfigEntry<int> medsBlessBehavior { get; private set; }

        // Multiplayer
        public static bool medsMPShopRarity = false;
        public static bool medsMPMapShopCorrupt = false;
        public static bool medsMPObeliskShopCorrupt = false;
        public static bool medsMPTownShopCorrupt = false;
        public static bool medsMPItemCorrupt = false;
        public static bool medsMPPerkPoints = false;
        public static bool medsMPCorruptGiovanna = false;
        public static bool medsMPKeyItems = false;
        // public static bool medsMPAlwaysSucceed = false;
        // public static bool medsMPAlwaysFail = false;
        public static bool medsMPCraftCorruptedCards = false;
        public static bool medsMPInfiniteCardCraft = false;
        public static bool medsMPStockedShop = false;
        public static bool medsMPSoloShop = false;
        public static bool medsMPDropShop = false;
        private static bool medsMPAllThePets = false;
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
        //public static bool medsMPNoTravelRequirements = false;
        private static bool medsMPNoClassRequirements = false;
        private static bool medsMPNoEquipmentRequirements = false;
        private static bool medsMPNoKeyItemRequirements = false;
        public static bool medsMPOverlyTenergetic = false;
        public static bool medsMPOverlyCardergetic = false;
        public static int medsMPDiminutiveDecks = 1;
        public static string medsMPDenyDiminishingDecks = "";
        public static int medsMPShopBadLuckProtection = 0;
        public static bool medsMPBugfixEquipmentHP = false;
        public static bool medsMPDLCClones = false;
        public static string medsMPDLCCloneTwo = "";
        public static string medsMPDLCCloneThree = "";
        public static string medsMPDLCCloneFour = "";
        public static bool medsMPVisitAllZones = false;
        public static int medsMPConflictResolution = 0;
        // public static int medsMPBlessBehavior = 0;

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
            //medsExportPlayerProfiles = Config.Bind(new ConfigDefinition("Debug", "Export Player Profiles"), true, new ConfigDescription("Export player profiles for use with Profile Editor."));
            //medsImportPlayerProfiles = Config.Bind(new ConfigDefinition("Debug", "Import Player Profiles"), false, new ConfigDescription("Import edited player profiles."));
            medsVanillaContentLog = Config.Bind(new ConfigDefinition("Debug", "Vanilla Content Logging"), false, new ConfigDescription("Logs the loading of each individual piece of vanilla content."));
            //medsCustomContent = Config.Bind(new ConfigDefinition("Debug", "Enable Custom Content"), true, new ConfigDescription("(IN TESTING) Loads custom cards/items/sprites[/auracurses]."));
            medsExportJSON = Config.Bind(new ConfigDefinition("Debug", "Export Vanilla Content"), false, new ConfigDescription("Export vanilla data to Custom Content-compatible JSON files."));
            medsExportSprites = Config.Bind(new ConfigDefinition("Debug", "Export Sprites"), true, new ConfigDescription("Export sprites when exporting vanilla content."));

            // Cards & Decks
            medsDiminutiveDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Minimum Deck Size"), 1, new ConfigDescription("Set the minimum deck size."));
            medsDenyDiminishingDecks = Config.Bind(new ConfigDefinition("Cards & Decks", "Card Removal"), "Can Remove Anything", new ConfigDescription("What cards can be removed at the church?", new AcceptableValueList<string>("Cannot Remove Cards", "Cannot Remove Curses", "Can Only Remove Curses", "Can Remove Anything")));
            medsCraftCorruptedCards = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Corrupted Cards"), false, new ConfigDescription("Allow crafting of corrupted cards."));
            medsInfiniteCardCraft = Config.Bind(new ConfigDefinition("Cards & Decks", "Craft Infinite Cards"), false, new ConfigDescription("Infinite card crafts (set available card count to 99)."));

            // Characters
            medsDLCClones = Config.Bind(new ConfigDefinition("Characters", "Enable Clones"), true, new ConfigDescription("Adds three clone characters to the DLC section of Hero Selection."));
            medsDLCCloneTwo = Config.Bind(new ConfigDefinition("Characters", "Clone 1"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 4?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneThree = Config.Bind(new ConfigDefinition("Characters", "Clone 2"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 5?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneFour = Config.Bind(new ConfigDefinition("Characters", "Clone 3"), "loremaster", new ConfigDescription("Which subclass should be cloned into DLC slot 6?", new AcceptableValueList<string>(medsSubclassList)));
            medsDLCCloneTwoName = Config.Bind(new ConfigDefinition("Characters", "Clone 1 Name"), "Clone", new ConfigDescription("What should the character in DLC slot 4 be called?"));
            medsDLCCloneThreeName = Config.Bind(new ConfigDefinition("Characters", "Clone 2 Name"), "Copy", new ConfigDescription("What should the character in DLC slot 5 be called?"));
            medsDLCCloneFourName = Config.Bind(new ConfigDefinition("Characters", "Clone 3 Name"), "Counterfeit", new ConfigDescription("What should the character in DLC slot 6 be called?"));
            medsOver50s = Config.Bind(new ConfigDefinition("Characters", "Level Past 50"), true, new ConfigDescription("Allows characters to be raised up to rank 500."));

            // Corruption & Madness
            medsSmallSanitySupplySelling = Config.Bind(new ConfigDefinition("Corruption & Madness", "Sell Supplies"), true, new ConfigDescription("Sell supplies on high madness."));
            medsRavingRerolls = Config.Bind(new ConfigDefinition("Corruption & Madness", "Shop Rerolls"), true, new ConfigDescription("Allow multiple shop rerolls on high madness."));
            medsUseClaimation = Config.Bind(new ConfigDefinition("Corruption & Madness", "Use Claims"), true, new ConfigDescription("Use claims on any madness. Note that you cannot _get_ claims on high madness (yet...)."));

            // Events & Nodes
            // medsAlwaysFail = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Fail Event Rolls"), false, new ConfigDescription("Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible."));
            // medsAlwaysSucceed = Config.Bind(new ConfigDefinition("Events & Nodes", "Always Succeed Event Rolls"), false, new ConfigDescription("Always succeed event rolls, though event text might not match. Critically succeeds if possible."));
            // medsNoTravelRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Travel Requirements"), false, new ConfigDescription("(NOT WORKING - shows path to node, but not actual node) Can travel to nodes that are normally invisible (e.g. western treasure node in Faeborg)."));
            medsNoClassRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Class Requirements"), false, new ConfigDescription("(IN TESTING) Events and replies ignore class requirements."));
            medsNoEquipmentRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Equipment Requirements"), false, new ConfigDescription("(IN TESTING) Events and replies ignore equipment/pet requirements."));
            medsNoKeyItemRequirements = Config.Bind(new ConfigDefinition("Events & Nodes", "No Key Item Requirements"), false, new ConfigDescription("(IN TESTING) Events and replies ignore key item / quest requirements."));
            medsTravelAnywhere = Config.Bind(new ConfigDefinition("Events & Nodes", "Travel Anywhere"), false, new ConfigDescription("Travel to any node."));
            medsVisitAllZones = Config.Bind(new ConfigDefinition("Events & Nodes", "Visit All Zones"), false, new ConfigDescription("You can choose any location to visit from the obelisk (e.g. can go to the Void early, can visit all locations before going, etc.)."));

            // Loot
            medsCorruptGiovanna = Config.Bind(new ConfigDefinition("Loot", "Corrupted Card Rewards"), false, new ConfigDescription("Card rewards are always corrupted (includes divinations)."));
            medsLootCorrupt = Config.Bind(new ConfigDefinition("Loot", "Corrupted Loot Rewards"), false, new ConfigDescription("Make item loot rewards always corrupted."));

            // Perks
            medsPerkPoints = Config.Bind(new ConfigDefinition("Perks", "Many Perk Points"), false, new ConfigDescription("(IN TESTING - visually buggy but functional) Set maximum perk points to 1000."));
            medsModifyPerks = Config.Bind(new ConfigDefinition("Perks", "Modify Perks Whenever"), false, new ConfigDescription("(IN TESTING) Change perks whenever you want."));
            medsNoPerkRequirements = Config.Bind(new ConfigDefinition("Perks", "No Perk Requirements"), false, new ConfigDescription("Can select perk without selecting its precursor perks; ignore minimum selected perk count for each row."));

            // Shop
            medsShopRarity = Config.Bind(new ConfigDefinition("Shop", "Adjusted Shop Rarity"), false, new ConfigDescription("Modify shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt."));
            medsShopBadLuckProtection = Config.Bind(new ConfigDefinition("Shop", "Bad Luck Protection"), 10, new ConfigDescription("Increases rarity of shops/loot based on number of shops/loot seen since an item was last acquired. If you play with discounted rerolls I recommend setting this to ~10; if you play without, I recommend ~300 :)", new AcceptableValueRange<int>(0, 10000)));
            medsMapShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Map Shops"), true, new ConfigDescription("Allow shops on the map (e.g. werewolf shop in Senenthia) to have corrupted goods for sale."));
            medsObeliskShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Obelisk Shops"), true, new ConfigDescription("Allow obelisk corruption shops to have corrupted goods for sale."));
            medsTownShopCorrupt = Config.Bind(new ConfigDefinition("Shop", "Corrupted Town Shops"), true, new ConfigDescription("Allow town shops to have corrupted goods for sale."));
            medsDiscountDivination = Config.Bind(new ConfigDefinition("Shop", "Discount Divination"), true, new ConfigDescription("Discounts are applied to divinations."));
            medsDiscountDoomroll = Config.Bind(new ConfigDefinition("Shop", "Discount Doomroll"), true, new ConfigDescription("Discounts are applied to shop rerolls."));
            medsPlentifulPetPurchases = Config.Bind(new ConfigDefinition("Shop", "Plentiful Pet Purchases"), true, new ConfigDescription("Buy more than one of each pet."));
            medsStockedShop = Config.Bind(new ConfigDefinition("Shop", "Post-Scarcity Shops"), true, new ConfigDescription("Does not record who purchased what in the shop."));
            medsSoloShop = Config.Bind(new ConfigDefinition("Shop", "Individual Player Shops"), true, new ConfigDescription("Does not send shop purchase records in multiplayer."));
            medsDropShop = Config.Bind(new ConfigDefinition("Shop", "Drop-Only Items Appear In Shops"), true, new ConfigDescription("Items that would normally not appear in shops, such as the Yggdrasil Root or Yogger's Cleaver, will appear."));

            // Should Be Vanilla
            medsProfane = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Allow Profanities"), true, new ConfigDescription("Allow profanities in your good Christian Piss server."));
            medsEmotional = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Emotional"), true, new ConfigDescription("Use more emotes during combat."));
            medsStraya = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Force Select Server"), false, new ConfigDescription("Force server selection to location of your choice (default: Australia)."));
            medsStrayaServer = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Force Select Server Selection"), "au", new ConfigDescription("Which server should be forced if the above option is true?", new AcceptableValueList<string>("asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "za", "sa", "kr", "us", "usw")));
            medsMaxMultiplayerMembers = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Max Multiplayer Members"), true, new ConfigDescription("Change the default player count in multiplDefault to 4 players in multiplayer."));
            medsOverlyTenergetic = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Overly Tenergetic"), true, new ConfigDescription("Allow characters to have more than 10 energy."));
            medsOverlyCardergetic = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Overly Cardergetic"), true, new ConfigDescription("(IN TESTING) Allow characters to have more than 10 cards."));
            medsBugfixEquipmentHP = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Bugfix: Equipment HP"), true, new ConfigDescription("(IN TESTING - visually buggy but functional) Fixes a vanilla bug that allows infinite stacking of HP by buying the same item repeatedly."));
            medsSkipCinematics = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Skip Cinematics"), false, new ConfigDescription("Skip cinematics."));
            medsAutoContinue = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Continue"), false, new ConfigDescription("(IN TESTING - visually buggy but functional) Automatically press 'Continue' in events."));
            medsMPLoadAutoCreateRoom = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Create Room on MP Load"), true, new ConfigDescription("Use previous settings to automatically create lobby room when loading multiplayer game."));
            medsMPLoadAutoReady = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Auto Ready on MP Load"), true, new ConfigDescription("Automatically readies up non-host players when loading multiplayer game."));
            medsSpacebarContinue = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Spacebar to Continue"), true, new ConfigDescription("Spacebar clicks the 'Continue' button in events for you."));
            medsConflictResolution = Config.Bind(new ConfigDefinition("Should Be Vanilla", "Conflict Resolution"), 4, new ConfigDescription("(IN TESTING) Automatically select (1) lowest card; (2) closest to 2; (3) highest card; or (4) random to determine multiplayer conflicts."));
            medsAllThePets = Config.Bind(new ConfigDefinition("Should Be Vanilla", "All The Pets"), true, new ConfigDescription("(IN TESTING) Shows blob pets and Harley in the Tome of Knowledge and shop."));

            // Combat
            // medsBlessBehavior = Config.Bind(new ConfigDefinition("Combat", "Bless Behavior"), 0, new ConfigDescription("(IN TESTING) Bless/sharp/fortify behaviour. (0) default (applies to both damage types on a card); (1) first (applies to first damage type on card); (2) split (damage split equally between damage types)."));

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
            // medsAlwaysFail.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            // medsAlwaysSucceed.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            // medsNoTravelRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoClassRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoEquipmentRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsNoKeyItemRequirements.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
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
            medsOverlyCardergetic.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
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
            medsOver50s.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsDropShop.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsVisitAllZones.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsConflictResolution.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            medsAllThePets.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };
            // medsBlessBehavior.SettingChanged += (obj, args) => { if (!bUpdatingSettings) { SettingsUpdated(); }; };

            medsImportSettings.SettingChanged += (obj, args) => { StringToSettings(medsImportSettings.Value); };

            UniverseLib.Universe.Init(1f, ObeliskialUI.InitUI, LogHandler, new()
            {
                Disable_EventSystem_Override = false, // or null
                Force_Unlock_Mouse = true, // or null
                Unhollowed_Modules_Folder = null
            });
            harmony.PatchAll();
            Log.LogInfo($"{ModGUID} {ModVersion} has loaded! Prayge ");
        }

        void LogHandler(string message, UnityEngine.LogType type)
        {
            string log = message?.ToString() ?? "";
            switch (type)
            {
                case UnityEngine.LogType.Assert:
                case UnityEngine.LogType.Log:
                    Log.LogInfo(log);
                    break;
                case UnityEngine.LogType.Warning:
                    Log.LogWarning(log);
                    break;
                case UnityEngine.LogType.Error:
                case UnityEngine.LogType.Exception:
                    Log.LogError(log);
                    break;
            }
        }
        public static string SettingsToString(bool forMP = false)
        {
            string[] str = new string[39];
            str[0] = medsShopRarity.Value ? "1" : "0";
            str[1] = medsMapShopCorrupt.Value ? "1" : "0";
            str[2] = medsObeliskShopCorrupt.Value ? "1" : "0";
            str[3] = medsTownShopCorrupt.Value ? "1" : "0";
            str[4] = medsLootCorrupt.Value ? "1" : "0";
            str[5] = medsPerkPoints.Value ? "1" : "0";
            str[6] = medsCorruptGiovanna.Value ? "1" : "0";
            str[7] = medsKeyItems.Value ? "1" : "0";
            str[8] = "0"; // medsAlwaysSucceed.Value ? "1" : "0";
            //str[9] = "0"; // medsAlwaysFail.Value ? "1" : "0";
            str[9] = medsOverlyCardergetic.Value ? "1" : "0";
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
            str[25] = medsAllThePets.Value ? "1" : "0";
            str[26] = medsNoClassRequirements.Value ? "1" : "0";
            str[27] = medsNoEquipmentRequirements.Value ? "1" : "0";
            str[28] = medsNoKeyItemRequirements.Value ? "1" : "0";
            str[29] = medsOverlyTenergetic.Value ? "1" : "0";
            str[30] = medsDiminutiveDecks.Value.ToString();
            str[31] = medsDenyDiminishingDecks.Value;
            str[32] = medsShopBadLuckProtection.Value.ToString();
            str[33] = medsBugfixEquipmentHP.Value ? "1" : "0";
            str[34] = medsJuiceGold.Value ? "1" : "0";
            str[35] = medsJuiceDust.Value ? "1" : "0";
            str[36] = medsDropShop.Value ? "1" : "0";
            str[37] = medsVisitAllZones.Value ? "1" : "0";
            str[38] = medsConflictResolution.Value.ToString();
            string jstr = string.Join("|", str);
            if (!forMP)
            {
                str = new string[15];
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
                str[14] = medsOver50s.Value ? "1" : "0";
                // str[15] = medsAllThePets.Value ? "1" : "0";
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
                if (nonMPstr.Length >= 15)
                    medsOver50s.Value = nonMPstr[14] == "1";
                //if (nonMPstr.Length >= 16)
                    //medsAllThePets.Value = nonMPstr[15] == "1";
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
            //if (str.Length >= 9)
                //medsAlwaysSucceed.Value = str[8] == "1";
            //if (str.Length >= 10)
                //medsAlwaysFail.Value = str[9] == "1";
            if (str.Length >= 10)
                medsOverlyCardergetic.Value = str[9] == "1";
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
                medsAllThePets.Value = str[25] == "1";
            if (str.Length >= 27)
                medsNoClassRequirements.Value = str[26] == "1";
            if (str.Length >= 28)
                medsNoEquipmentRequirements.Value = str[27] == "1";
            if (str.Length >= 29)
                medsNoClassRequirements.Value = str[28] == "1";
            if (str.Length >= 30)
                medsOverlyTenergetic.Value = str[29] == "1";
            if (str.Length >= 31)
                medsDiminutiveDecks.Value = int.Parse(str[30]);
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
            if (str.Length >= 37)
                medsDropShop.Value = str[36] == "1";
            if (str.Length >= 38)
                medsVisitAllZones.Value = str[37] == "1";
            if (str.Length >= 39)
                medsConflictResolution.Value = int.Parse(str[38]);
            medsExportSettings.Value = SettingsToString();
            medsImportSettings.Value = "";
        }

        public static void SaveMPSettings(string _newSettings)
        {
            List<string> settingsChanged = new();
            Plugin.Log.LogDebug("RECEIVING SETTINGS: " + _newSettings);
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
            //if (str.Length >= 9)
                //medsMPAlwaysSucceed = str[8] == "1";
            //if (str.Length >= 10)
                //medsMPAlwaysFail = str[9] == "1";
            if (str.Length >= 10)
                medsMPOverlyCardergetic = str[9] == "1";
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
                bool subclassChange = false;
                if (!(str[15].Split("&")[0] == "1") == medsMPDLCClones || (str[15].Split("&")[0] == "1" && (medsMPDLCCloneTwo != str[15].Split("&")[1] || medsMPDLCCloneThree != str[15].Split("&")[2] || medsMPDLCCloneFour != str[15].Split("&")[3]))) // different to current setting!
                    subclassChange = true;
                medsMPDLCClones = str[15].Split("&")[0] == "1";
                medsMPDLCCloneTwo = str[15].Split("&")[1];
                medsMPDLCCloneThree = str[15].Split("&")[2];
                medsMPDLCCloneFour = str[15].Split("&")[3];
                if (subclassChange)
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
                medsMPAllThePets = str[25] == "1";
            if (str.Length >= 27)
                medsMPNoClassRequirements = str[26] == "1";
            if (str.Length >= 28)
                medsMPNoEquipmentRequirements = str[27] == "1";
            if (str.Length >= 29)
                medsMPNoKeyItemRequirements = str[28] == "1";
            if (str.Length >= 30)
                medsMPOverlyTenergetic = str[29] == "1";
            if (str.Length >= 31)
                medsMPDiminutiveDecks = int.Parse(str[30]);
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
            if (str.Length >= 37)
                medsMPDropShop = str[36] == "1";
            if (str.Length >= 38)
                medsMPVisitAllZones = str[37] == "1";
            if (str.Length >= 39)
                medsMPConflictResolution = int.Parse(str[38]);
            Log.LogDebug("RECEIVED " + str.Length + " SETTINGS!");
            if (medsAllThePets.Value != medsMPAllThePets)
                settingsChanged.Add("All The Pets");
            if (medsNoClassRequirements.Value != medsMPNoClassRequirements)
                settingsChanged.Add("No Class Requirements");
            if (medsNoEquipmentRequirements.Value != medsMPNoEquipmentRequirements)
                settingsChanged.Add("No Equipment Requirements");
            if (medsNoKeyItemRequirements.Value != medsMPNoKeyItemRequirements)
                settingsChanged.Add("No Key Item Requirements");
            if (settingsChanged.Count > 0)
            {
                AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(UpdateSettingsForYou);
                AlertManager.Instance.AlertConfirmDouble("ERROR!\nThe host of this game has settings that are incompatible with yours and require a restart: " + String.Join(", ", settingsChanged.ToArray()) + "\nWould you like these settings to be changed?");
            }
            UpdateDropOnlyItems();
            UpdateVisitAllZones();
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
            string newSettings = SettingsToString();
            if (
                (medsExportSettings.Value.Length > 25 && medsExportSettings.Value[25] != newSettings[25]) || // all the pets
                (medsExportSettings.Value.Length > 26 && medsExportSettings.Value[26] != newSettings[26]) || // no class requirements
                (medsExportSettings.Value.Length > 27 && medsExportSettings.Value[27] != newSettings[27]) || // no equipment requirements
                (medsExportSettings.Value.Length > 28 && medsExportSettings.Value[28] != newSettings[28])) // no key item requirements
                AlertManager.Instance.AlertConfirm("These settings will not properly take effect until you restart the game.");
            if (medsExportSettings.Value.Length > 37 && medsExportSettings.Value[37] != newSettings[37])
                UpdateVisitAllZones();
            medsExportSettings.Value = newSettings;
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
        public static void UpdateSettingsForYou()
        {
            AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(UpdateSettingsForYou);
            medsAllThePets.Value = medsMPAllThePets;
            medsNoClassRequirements.Value = medsMPNoClassRequirements;
            medsNoEquipmentRequirements.Value = medsMPNoEquipmentRequirements;
            medsNoKeyItemRequirements.Value = medsMPNoKeyItemRequirements;
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

            Plugin.Log.LogDebug("CREATECLONES START");
            // PlayerManager.Instance.SetSkin("medsdlctwo", medsSkinData.SkinId);
            if (!(Plugin.IsHost() ? Plugin.medsDLCClones.Value : Plugin.medsMPDLCClones))
                return;
            Plugin.medsSubClassesSource = Traverse.Create(Globals.Instance).Field("_SubClassSource").GetValue<Dictionary<string, SubClassData>>();
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
                medsSCD.OrderInList = chr + 2; // (+2 = 3 vanilla dlc characters currently exist)
                medsSCD.SubClassName = medsSCDId;
                medsSCD.MainCharacter = true;
                medsSCD.ExpansionCharacter = true;
                Plugin.medsSubClassesSource[medsSCDId] = medsSCD;
                Globals.Instance.SubClass[medsSCDId] = UnityEngine.Object.Instantiate<SubClassData>(medsSCD);
            }
            Traverse.Create(Globals.Instance).Field("_SubClassSource").SetValue(Plugin.medsSubClassesSource);
            Plugin.Log.LogDebug("CREATECLONES END");
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
            Plugin.Log.LogDebug("duplicating cardbacks...");
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
            medsDLCCloneTwoCardback = "medsdlctwo" + ((char)(b - 1)).ToString();
            medsDLCCloneThreeCardback = "medsdlcthree" + ((char)(c - 1)).ToString();
            medsDLCCloneFourCardback = "medsdlcfour" + ((char)(d - 1)).ToString();
            medsCardbacksToAdd = medsCardbackDataSource.Concat(medsCardbacksToAdd).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
            Traverse.Create(Globals.Instance).Field("_CardbackDataSource").SetValue(medsCardbacksToAdd);
            Plugin.Log.LogDebug("CREATECLONECARDBACKS END");
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
            medsDLCCloneTwoSkin = "medsdlctwo" + ((char)(b - 1)).ToString();
            medsDLCCloneThreeSkin = "medsdlcthree" + ((char)(c - 1)).ToString();
            medsDLCCloneFourSkin = "medsdlcfour" + ((char)(d - 1)).ToString();
            medsSkinsToAdd = medsSkinDataSource.Concat(medsSkinsToAdd).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
            Traverse.Create(Globals.Instance).Field("_SkinDataSource").SetValue(medsSkinsToAdd);
            Plugin.Log.LogDebug("CREATECLONESKINS END");
            // create event replies for clones
            /* SOOO SLOW
            Plugin.medsEventDataSource = Traverse.Create(Globals.Instance).Field("_Events").GetValue<Dictionary<string, EventData>>();
            foreach (string key in Plugin.medsCloneTwoEvents)
            {

            }
            foreach (string key in Plugin.medsEventDataSource.Keys)
            {
                Plugin.Log.LogDebug("checking event key: " + key);
                bool erFound = false;
                EventReplyData[] tempERD = Plugin.medsEventDataSource[key].Replys;
                for (int a = 0; a < Plugin.medsEventDataSource[key].Replys.Length; a++)
                {
                    EventReplyData reply = Plugin.medsEventDataSource[key].Replys[a];
                    if (reply.RequiredClass != (SubClassData)null && !reply.RepeatForAllCharacters)
                    {
                        List<string> subclassAdd = new();
                        if (reply.RequiredClass.Id == (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo))
                            subclassAdd.Add("medsdlctwo");
                        if (reply.RequiredClass.Id == (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree))
                            subclassAdd.Add("medsdlcthree");
                        if (reply.RequiredClass.Id == (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour))
                            subclassAdd.Add("medsdlcfour");
                        foreach (string sub in subclassAdd)
                        {
                            EventReplyData eventReplyData = reply.ShallowCopy();
                            eventReplyData.RequiredClass = Globals.Instance.GetSubClassData(sub);
                            Array.Resize(ref tempERD, tempERD.Length + 1);
                            tempERD[tempERD.Length - 1] = eventReplyData;
                            erFound = true;
                        }
                    }
                }
                if (erFound)
                    Plugin.medsEventDataSource[key].Replys = tempERD;
            }
            Traverse.Create(Globals.Instance).Field("_Events").SetValue(Plugin.medsEventDataSource);*/
        }
        public static bool IsHost()
        {
            if ((GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()) || !GameManager.Instance.IsMultiplayer())
                return true;
            return false;
        }
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

        public static void ExportSprite(Sprite spriteToExport, string spriteType)
        {
            RecursiveFolderCreate("Obeliskial_exported", "sprite", spriteType);
            string filePath = Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "sprite", spriteType, spriteToExport.name + ".png");
            RenderTexture renderTex = RenderTexture.GetTemporary((int)spriteToExport.texture.width, (int)spriteToExport.texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            // we flip it when doing the Graphics.Blit because the sprites are packed (which... flips them? idk?)
            Graphics.Blit(spriteToExport.texture, renderTex, new Vector2(1, -1), new Vector2(0, 1));
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D((int)spriteToExport.textureRect.width, (int)spriteToExport.textureRect.height);
            readableText.ReadPixels(new Rect(spriteToExport.textureRect.x, spriteToExport.textureRect.y, spriteToExport.textureRect.width, spriteToExport.textureRect.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            // flip it back
            Texture2D finalImage = new Texture2D((int)spriteToExport.textureRect.width, (int)spriteToExport.textureRect.height);
            for (int i = 0; i < readableText.width; i++)
                for (int j = 0; j < readableText.height; j++)
                    finalImage.SetPixel(i, readableText.height - j - 1, readableText.GetPixel(i, j));
            finalImage.Apply();
            File.WriteAllBytes(filePath, ImageConversion.EncodeToPNG(finalImage));
        }

        public static void RecursiveFolderCreate(params string[] path) // really brings you back to Budget, doesn't it?
        {
            string fPath = Paths.ConfigPath;
            DirectoryInfo medsDI = new DirectoryInfo(fPath);
            if (!medsDI.Exists)
                medsDI.Create();
            for (int a = 0; a < path.Length; a++)
            {
                medsDI = new DirectoryInfo(Path.Combine(medsDI.FullName, path[a]));
                if (!medsDI.Exists)
                    medsDI.Create();
            }
        }

        public static void WriteToJSON(string exportType, string exportText, string exportID)
        {
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", exportType, exportID + ".json"), exportText);
        }
        /*public static void WriteToJSON(string exportType, string exportText, int a, int b)
        {
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", exportType, "combined", String.Format("{0:00000}", (b - 1) * 100 + 1) + "-" + String.Format("{0:00000}", a)) + ".json", exportText);
        }*/

        public static void ExtractData<T>(T[] data)
        {
            //string combined = "{";
            //int h = 1; // counts hundreds for combined files
            for (int a = 1; a <= data.Length; a++)
            {
                string type = "";
                string id = "";
                string text = "";
                string textFULL = "";
                bool pretty = true;
                if (data[a - 1].GetType() == typeof(SubClassData))
                {
                    type = "subclass";
                    SubClassData d = (SubClassData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(TraitData))
                {
                    type = "trait";
                    TraitData d = (TraitData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CardData))
                {
                    type = "card";
                    CardData d = (CardData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(PerkData))
                {
                    type = "perk";
                    PerkData d = (PerkData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(AuraCurseData))
                {
                    type = "auraCurse";
                    AuraCurseData d = (AuraCurseData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(NPCData))
                {
                    type = "npc";
                    NPCData d = (NPCData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(NodeData))
                {
                    type = "node";
                    NodeData d = (NodeData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                    textFULL = JsonUtility.ToJson(DataTextConvert.ToFULLText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(LootData))
                {
                    type = "loot";
                    LootData d = (LootData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(PerkNodeData))
                {
                    type = "perkNode";
                    PerkNodeData d = (PerkNodeData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(ChallengeData))
                {
                    type = "challengeData";
                    ChallengeData d = (ChallengeData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(ChallengeTrait))
                {
                    type = "challengeTrait";
                    ChallengeTrait d = (ChallengeTrait)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CombatData))
                {
                    type = "combatData";
                    CombatData d = (CombatData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(EventData))
                {
                    type = "event";
                    EventData d = (EventData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(EventReplyDataText))
                {
                    type = "eventReply";
                    EventReplyDataText d = (EventReplyDataText)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(d, pretty);
                }
                else if (data[a - 1].GetType() == typeof(EventRequirementData))
                {
                    type = "eventRequirement";
                    EventRequirementData d = (EventRequirementData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(ZoneData))
                {
                    type = "zone";
                    ZoneData d = (ZoneData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(KeyNotesData))
                {
                    type = "keynote";
                    KeyNotesData d = (KeyNotesData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(PackData))
                {
                    type = "pack";
                    PackData d = (PackData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CardPlayerPackData))
                {
                    type = "cardPlayerPack";
                    CardPlayerPackData d = (CardPlayerPackData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CardPlayerPackData))
                {
                    type = "pairsPack";
                    CardPlayerPairsPackData d = (CardPlayerPairsPackData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(ItemData))
                {
                    type = "item";
                    ItemData d = (ItemData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CardbackData))
                {
                    type = "cardback";
                    CardbackData d = (CardbackData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(SkinData))
                {
                    type = "skin";
                    SkinData d = (SkinData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CorruptionPackData))
                {
                    type = "corruptionPack";
                    CorruptionPackData d = (CorruptionPackData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(CinematicData))
                {
                    type = "cinematic";
                    CinematicData d = (CinematicData)(object)data[a - 1];
                    id = DataTextConvert.ToString(d);
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else if (data[a - 1].GetType() == typeof(TierRewardData))
                {
                    type = "tierReward";
                    TierRewardData d = (TierRewardData)(object)data[a - 1];
                    id = d.TierNum.ToString();
                    text = JsonUtility.ToJson(DataTextConvert.ToText(d), pretty);
                }
                else
                {
                    Log.LogError("Unknown type while extracting data: " + data[a - 1].GetType());
                    return;
                }
                //text = text.Replace(@""":false,", @""":0,").Replace(@""":false}", @""":0}").Replace(@""":true,", @""":1,").Replace(@""":true}", @""":1}");
                if (a == 1)
                {
                    RecursiveFolderCreate("Obeliskial_exported", type);
                    File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + ".json"), "[");
                    if (textFULL != "")
                    {
                        RecursiveFolderCreate("Obeliskial_exported", type + "_FULL");
                        File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + "_FULL.json"), "[");
                    }
                }
                WriteToJSON(type, text, id);
                if (textFULL != "")
                    WriteToJSON(type + "_FULL", textFULL, id);
                if (a == data.Length)
                {
                    // WriteToJSON(type, combined.Remove(combined.Length - 1) + "}", a, h);
                    File.AppendAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + ".json"), text + "]");
                    if (textFULL != "")
                        File.AppendAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + "_FULL.json"), textFULL + "]");
                    Log.LogInfo("exported " + a + " " + type + " values!");
                }
                else
                {
                    File.AppendAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + ".json"), text + ",");
                    if (textFULL != "")
                        File.AppendAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "!combined", type + "_FULL.json"), textFULL + ",");
                }
            }
        }

        public static void UpdateDropOnlyItems()
        {
            medsItemDataSource = Traverse.Create(Globals.Instance).Field("_ItemDataSource").GetValue<Dictionary<string, ItemData>>();
            if (IsHost() ? medsDropShop.Value : medsMPDropShop)
                foreach (KeyValuePair<string, ItemData> kvp in medsItemDataSource)
                    if (!Plugin.medsDoNotDropList.Contains(kvp.Value.Id) && (SteamManager.Instance.PlayerHaveDLC("2511580") || !Plugin.medsDropOnlySoU.Contains(kvp.Value.Id)))
                        kvp.Value.DropOnly = false;
            else
                foreach (string s in medsDropOnlyItems)
                    medsItemDataSource[s].DropOnly = true;
            Traverse.Create(Globals.Instance).Field("_ItemDataSource").SetValue(medsItemDataSource);
        }

        public static void UpdateVisitAllZones()
        {
            // REALLY, DOESN'T THE AMOUNT OF COMMENTING HERE COMPARED TO EVERYWHERE ELSE TELL YOU HOW FUCKING JANK THIS IS?
            // SAD.
            medsEventDataSource = Traverse.Create(Globals.Instance).Field("_Events").GetValue<Dictionary<string, EventData>>();
            medsNodeDataSource = Traverse.Create(Globals.Instance).Field("_NodeDataSource").GetValue<Dictionary<string, NodeData>>();
            EventReplyData newReply = medsEventDataSource["e_velka33_tier2"].Replys[0].ShallowCopy();
            newReply.IndexForAnswerTranslation = 666; // spooky.
            string replyText = Texts.Instance.GetText("events_e_velka33_tier2_rp0");
            string replyTextS = Texts.Instance.GetText("events_e_velka33_tier2_rp0_s");
            bool doWeVisitAllZones = IsHost() ? medsVisitAllZones.Value : medsMPVisitAllZones;
            //Plugin.Log.LogDebug("ARE WE VISITING ALL ZONES, MY CHILD? " + doWeVisitAllZones.ToString());
            // ohhh we need to set requirement for newreply??
            newReply.Requirement = doWeVisitAllZones ? (EventRequirementData)null : Globals.Instance.GetRequirementData("medsimpossiblerequirement");
            foreach (string nodeID in medsObeliskNodes)
            {
                if (medsNodeDataSource.ContainsKey(nodeID))
                {
                    // EventData[] newEvents = new EventData[0];
                    for (int a = 0; a < medsNodeDataSource[nodeID].NodeEvent.Length; a++)
                    { // for every possible event on that node...
                        string eventID = medsNodeDataSource[nodeID].NodeEvent[a].EventId;
                        if (medsNodeDataSource[nodeID].NodeEvent[a].Requirement != (EventRequirementData)null && (medsNodeDataSource[nodeID].NodeEvent[a].Requirement.RequirementId == "medsimpossiblerequirement" || medsNodeDataSource[nodeID].NodeEvent[a].Requirement.RequirementId == "_tier2"))
                        { // if the event has a requirement (either medsimpossiblerequirement, which we set as a placeholder/deliberately impossible-to-receive requirement; or _tier2, which is vanilla)
                            //Plugin.Log.LogDebug("setting tier2/4 on node " + nodeID + " event " + medsNodeDataSource[nodeID].NodeEvent[a].EventId);
                            // if setting is enabled, sets _tier4 so this event never shows (i.e., we don't use the vanilla "travel through big portal" event)
                            // otherwise, set to _tier2 (i.e., show "travel through big portal" event if in act 3)
                            medsEventDataSource[medsNodeDataSource[nodeID].NodeEvent[a].EventId].Requirement = Globals.Instance.GetRequirementData(doWeVisitAllZones ? "medsimpossiblerequirement" : "_tier2");
                            medsNodeDataSource[nodeID].NodeEvent[a].Requirement = Globals.Instance.GetRequirementData(doWeVisitAllZones ? "medsimpossiblerequirement" : "_tier2");
                            // Plugin.Log.LogDebug("req: " + Globals.Instance.GetRequirementData(doWeVisitAllZones ? "medsimpossiblerequirement" : "_tier2").RequirementId);
                        }
                        else if (medsNodeDataSource[nodeID].NodeEvent[a].Replys.Length > 2)
                        { // if the event _does not_ have a requirement (vanilla act 1/2 event)
                            // node
                            bool existingReply = false;
                            for (int b = 0; b < medsNodeDataSource[nodeID].NodeEvent[a].Replys.Length; b++)
                            {
                                if (medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].IndexForAnswerTranslation == 666)
                                { // my fake "travel through big portal" eventreply
                                    existingReply = true;
                                    // if setting enabled, remove requirement; otherwise set impossible to receive requirement so option does not show
                                    medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].Requirement = doWeVisitAllZones ? (EventRequirementData)null : Globals.Instance.GetRequirementData("medsimpossiblerequirement");
                                }
                                else
                                {
                                    // hide option if previously visited that zone; mark that zone as previously visited if option selected.
                                    medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].RequirementBlocked = doWeVisitAllZones ? Globals.Instance.GetRequirementData("medsvisited" + medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].SsNodeTravel.NodeZone.ZoneId.ToLower()) : (EventRequirementData)null;
                                    medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].SsRequirementUnlock = Globals.Instance.GetRequirementData(doWeVisitAllZones ? "medsvisited" + medsNodeDataSource[nodeID].NodeEvent[a].Replys[b].SsNodeTravel.NodeZone.ZoneId.ToLower() : (nodeID == "sen_34" ? "_tier1" : "_tier2"));
                                }
                            }
                            EventReplyData[] tempERD = new EventReplyData[0];
                            if (!existingReply)
                            {
                                // if reply does not exist, create it
                                // maybe this could be less dumb by converting it from an array into a list, and then back? is that really less dumb?
                                // I just feel like this code is obtuse.
                                // idk.
                                tempERD = medsNodeDataSource[nodeID].NodeEvent[a].Replys;
                                Array.Resize(ref tempERD, tempERD.Length + 1);
                                tempERD[tempERD.Length - 1] = newReply.ShallowCopy();
                                medsNodeDataSource[nodeID].NodeEvent[a].Replys = tempERD;
                            }
                            //event
                            existingReply = false;
                            for (int b = 0; b < medsEventDataSource[eventID].Replys.Length; b++)
                            {
                                if (medsEventDataSource[eventID].Replys[b].IndexForAnswerTranslation == 666)
                                { // my fake "travel through big portal" eventreply
                                    existingReply = true;
                                    // if setting enabled, remove requirement; otherwise set impossible to receive requirement so option does not show
                                    medsEventDataSource[eventID].Replys[b].Requirement = doWeVisitAllZones ? (EventRequirementData)null : Globals.Instance.GetRequirementData("_tier4");
                                }
                                else
                                {
                                    // hide option if previously visited that zone; mark that zone as previously visited if option selected.
                                    medsEventDataSource[eventID].Replys[b].RequirementBlocked = Globals.Instance.GetRequirementData("medsvisited" + medsEventDataSource[eventID].Replys[b].SsNodeTravel.NodeZone.ZoneId.ToLower());
                                    medsEventDataSource[eventID].Replys[b].SsRequirementUnlock = Globals.Instance.GetRequirementData(doWeVisitAllZones ? "medsvisited" + medsEventDataSource[eventID].Replys[b].SsNodeTravel.NodeZone.ZoneId.ToLower() : (nodeID == "sen_34" ? "_tier1" : "_tier2"));
                                }
                            }
                            if (!existingReply)
                            {
                                // if reply does not exist, create it
                                // maybe this could be less dumb by converting it from an array into a list, and then back? is that really less dumb?
                                // I just feel like this code is obtuse.
                                // idk.
                                //Plugin.Log.LogDebug("node " + nodeID + " event " + eventID + ": " + medsEventDataSource[medsNodeDataSource[nodeID].NodeEvent[a].EventId].Replys.Length + " replies at beginning!");
                                tempERD = medsEventDataSource[medsNodeDataSource[nodeID].NodeEvent[a].EventId].Replys;
                                Array.Resize(ref tempERD, tempERD.Length + 1);
                                tempERD[tempERD.Length - 1] = newReply.ShallowCopy();
                                medsEventDataSource[medsNodeDataSource[nodeID].NodeEvent[a].EventId].Replys = tempERD;
                                //Plugin.Log.LogDebug("node " + nodeID + " event " + eventID + ": " + medsEventDataSource[medsNodeDataSource[nodeID].NodeEvent[a].EventId].Replys.Length + " replies at end!");
                            }
                            // set text for translation (though translation isn't set up :D)
                            Plugin.medsTexts["events_" + medsNodeDataSource[nodeID].NodeEvent[a].EventId + "_rp" + newReply.IndexForAnswerTranslation] = replyText;
                            Plugin.medsTexts["events_" + medsNodeDataSource[nodeID].NodeEvent[a].EventId + "_rp" + newReply.IndexForAnswerTranslation + "_s"] = replyTextS;
                        }
                    }
                }
            }
            Traverse.Create(Globals.Instance).Field("_Events").SetValue(medsEventDataSource);
            Traverse.Create(Globals.Instance).Field("_NodeDataSource").SetValue(medsNodeDataSource);
        }

        public static void OptimalPathSeed()
        {
            List<string[]> nodeList = new();
            // Senenthia: 2 events, 4 corruptors
            nodeList.Add(new string[4] { "Betty", "sen_6", "e_sen6_b", "Senenthia" });
            nodeList.Add(new string[4] { "combat", "sen_9", "", "Senenthia" });
            nodeList.Add(new string[4] { "combat", "secta_2", "", "Senenthia" });
            nodeList.Add(new string[4] { "combat", "sen_19", "", "Senenthia" });
            nodeList.Add(new string[4] { "Soldier Trainer", "sen_37", "e_sen37_a", "Senenthia" });
            nodeList.Add(new string[4] { "combat", "sen_28", "", "Senenthia" });

            // Aquarfall: 7 corruptors
            nodeList.Add(new string[4] { "combat", "aqua_4", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "aqua_12", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "aqua_10", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "aqua_15", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "spider_3", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "spider_4", "", "Aquarfall" });
            nodeList.Add(new string[4] { "combat", "aqua_33", "", "Aquarfall" });

            // Faeborg: 2 events, 6 corruptors
            /*nodeList.Add(new string[4] { "Monster Trainer", "faen_7", "", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "faen_8", "", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "faen_14", "", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "faen_24", "", "Faeborg" });
            nodeList.Add(new string[4] { "Binks", "faen_40", "e_faen40_a", "Faeborg" });
            nodeList.Add(new string[4] { "Charls", "faen_40", "e_faen40_b", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "sewers_2", "", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "sewers_12", "", "Faeborg" });
            nodeList.Add(new string[4] { "combat", "faen_37", "", "Faeborg" }); */

            // Velkarath: 6 corruptors
            nodeList.Add(new string[4] { "combat", "velka_2", "", "Velkarath" });
            nodeList.Add(new string[4] { "combat", "velka_5", "", "Velkarath" });
            nodeList.Add(new string[4] { "combat", "velka_13", "", "Velkarath" });
            nodeList.Add(new string[4] { "combat", "forge_1", "", "Velkarath" }); // using upper path because it's more consistent
            nodeList.Add(new string[4] { "combat", "velka_28", "", "Velkarath" });
            nodeList.Add(new string[4] { "combat", "velka_31", "", "Velkarath" });

            // Voidlow: 1 event, 5 corruptors
            nodeList.Add(new string[4] { "combat", "voidlow_2", "", "Voidlow" });
            nodeList.Add(new string[4] { "combat", "voidlow_9", "", "Voidlow" });
            nodeList.Add(new string[4] { "combat", "voidlow_10", "", "Voidlow" });
            nodeList.Add(new string[4] { "Chromatic Slime", "voidlow_27", "e_voidlow27_a", "Voidlow" });
            nodeList.Add(new string[4] { "combat", "voidlow_19", "", "Voidlow" });
            nodeList.Add(new string[4] { "combat", "voidlow_22", "", "Voidlow" });

            // Voidhigh: 2 corruptors
            nodeList.Add(new string[4] { "combat", "voidhigh_2", "", "Voidhigh" });
            nodeList.Add(new string[4] { "combat", "voidhigh_10", "", "Voidhigh" });

            for (int a = 1325259; a <= 9999999; a++)
            {
                string seed = a.ToString();
                CheckSeed(seed, nodeList);
            }

            /*for (int a = 0; a <= 9; a++)
            {
                for (int b = 0; b <= 9; b++)
                {
                    for (int c = 0; c <= 9; c++)
                    {
                        for (int d = 0; d <= 9; d++)
                        {
                            for (int e = 0; e <= 9; e++)
                            {
                                for (int f = 0; f <= 9; f++)
                                {
                                    for (int g = 0; g <= 9; g++)
                                    {
                                        string seed = a.ToString() + b.ToString() + c.ToString() + d.ToString() + e.ToString() + f.ToString() + g.ToString();
                                        CheckSeed(seed, nodeList);
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }

        public static void CheckSeed(string seed, List<string[]> nodeList)
        {
            int medsCommon = 0;
            int medsUncommon = 0;
            int medsRare = 0;
            int medsEpic = 0;
            int medsEvents = 0;
            Dictionary<string, int> zoneEventCount = new();
            Dictionary<string, int> zoneCommonCount = new();
            Dictionary<string, int> zoneUncommonCount = new();
            Dictionary<string, int> zoneRareCount = new();
            Dictionary<string, int> zoneEpicCount = new();
            zoneEventCount["Senenthia"] = 0;
            zoneEventCount["Aquarfall"] = 0;
            zoneEventCount["Faeborg"] = 0;
            zoneEventCount["Velkarath"] = 0;
            zoneEventCount["Voidlow"] = 0;
            zoneEventCount["Voidhigh"] = 0;
            zoneCommonCount["Senenthia"] = 0;
            zoneCommonCount["Aquarfall"] = 0;
            zoneCommonCount["Faeborg"] = 0;
            zoneCommonCount["Velkarath"] = 0;
            zoneCommonCount["Voidlow"] = 0;
            zoneCommonCount["Voidhigh"] = 0;
            zoneUncommonCount["Senenthia"] = 0;
            zoneUncommonCount["Aquarfall"] = 0;
            zoneUncommonCount["Faeborg"] = 0;
            zoneUncommonCount["Velkarath"] = 0;
            zoneUncommonCount["Voidlow"] = 0;
            zoneUncommonCount["Voidhigh"] = 0;
            zoneRareCount["Senenthia"] = 0;
            zoneRareCount["Aquarfall"] = 0;
            zoneRareCount["Faeborg"] = 0;
            zoneRareCount["Velkarath"] = 0;
            zoneRareCount["Voidlow"] = 0;
            zoneRareCount["Voidhigh"] = 0;
            zoneEpicCount["Senenthia"] = 0;
            zoneEpicCount["Aquarfall"] = 0;
            zoneEpicCount["Faeborg"] = 0;
            zoneEpicCount["Velkarath"] = 0;
            zoneEpicCount["Voidlow"] = 0;
            zoneEpicCount["Voidhigh"] = 0;

            foreach (string[] nodeData in nodeList)
            {
                NodeData _node = Globals.Instance.GetNodeData(nodeData[1]);

                // Log.LogInfo("DHS: " + (_node.NodeId + seed + nameof(AtOManager.Instance.AssignSingleGameNode)));
                UnityEngine.Random.InitState((_node.NodeId + seed + nameof(AtOManager.Instance.AssignSingleGameNode)).GetDeterministicHashCode());
                // Log.LogInfo("DHC: " + (_node.NodeId + seed + nameof(AtOManager.Instance.AssignSingleGameNode)).GetDeterministicHashCode());
                if (UnityEngine.Random.Range(0, 100) < _node.ExistsPercent)
                {
                    bool flag1 = true;
                    bool flag2 = true;
                    if (_node.NodeEvent != null && _node.NodeEvent.Length != 0 && _node.NodeCombat != null && _node.NodeCombat.Length != 0)
                    {
                        if (UnityEngine.Random.Range(0, 100) < _node.CombatPercent)
                            flag1 = false;
                        else
                            flag2 = false;
                    }

                    if (flag1 && _node.NodeEvent != null && _node.NodeEvent.Length != 0) // event!
                    {
                        string str = "";
                        Dictionary<string, int> source = new Dictionary<string, int>();
                        for (int index = 0; index < _node.NodeEvent.Length; ++index)
                        {
                            int num = 10000;
                            if (index < _node.NodeEventPriority.Length)
                                num = _node.NodeEventPriority[index];
                            source.Add(_node.NodeEvent[index].EventId, num);
                        }
                        if (source.Count > 0)
                        {
                            Dictionary<string, int> dictionary1 = source.OrderBy<KeyValuePair<string, int>, int>((Func<KeyValuePair<string, int>, int>)(x => x.Value)).ToDictionary<KeyValuePair<string, int>, string, int>((Func<KeyValuePair<string, int>, string>)(x => x.Key), (Func<KeyValuePair<string, int>, int>)(x => x.Value));
                            int num1 = 1;
                            int num2 = dictionary1.ElementAt<KeyValuePair<string, int>>(0).Value;
                            while (num1 < dictionary1.Count && dictionary1.ElementAt<KeyValuePair<string, int>>(num1).Value == num2)
                                ++num1;
                            if (num1 == 1)
                            {
                                str = dictionary1.ElementAt<KeyValuePair<string, int>>(0).Key;
                            }
                            else
                            {
                                if (_node.NodeEventPercent != null && _node.NodeEvent.Length == _node.NodeEventPercent.Length)
                                {
                                    Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
                                    int index1 = 0;
                                    for (int index2 = 0; index2 < num1; ++index2)
                                    {
                                        int index3 = 0;
                                        while (index2 < _node.NodeEvent.Length)
                                        {
                                            if (_node.NodeEvent[index3].EventId == dictionary1.ElementAt<KeyValuePair<string, int>>(index1).Key)
                                            {
                                                dictionary2.Add(_node.NodeEvent[index3].EventId, _node.NodeEventPercent[index3]);
                                                ++index1;
                                                break;
                                            }
                                            ++index3;
                                        }
                                    }
                                    int num3 = UnityEngine.Random.Range(0, 100);
                                    int num4 = 0;
                                    foreach (KeyValuePair<string, int> keyValuePair in dictionary2)
                                    {
                                        num4 += keyValuePair.Value;
                                        if (num3 < num4)
                                        {
                                            str = keyValuePair.Key;
                                            break;
                                        }
                                    }
                                }
                                if (str == "")
                                {
                                    int index = UnityEngine.Random.Range(0, num1);
                                    str = dictionary1.ElementAt<KeyValuePair<string, int>>(index).Key;
                                }
                            }
                            if (str == nodeData[2])
                            {
                                // this is the event we want!
                                medsEvents++;
                                zoneEventCount[nodeData[3]]++;
                            }
                        }
                    }
                    else if (nodeData[0] == "combat" && flag2 && _node.NodeCombat != null && _node.NodeCombat.Length != 0) // combat!
                    {
                        string combatID = _node.NodeCombat[0].CombatId;
                        string str = _node.NodeId + seed;
                        int deterministicHashCode = str.GetDeterministicHashCode();
                        UnityEngine.Random.InitState(deterministicHashCode);

                        List<string> stringList = new List<string>();
                        for (int index = 0; index < Globals.Instance.CardListByType[Enums.CardType.Corruption].Count; ++index)
                        {
                            CardData cardData = Globals.Instance.GetCardData(Globals.Instance.CardListByType[Enums.CardType.Corruption][index], false);
                            if ((UnityEngine.Object)cardData != (UnityEngine.Object)null && !cardData.OnlyInWeekly)
                                stringList.Add(Globals.Instance.CardListByType[Enums.CardType.Corruption][index]);
                        }
                        bool flag3 = false;
                        int medsRandomCorruptionIndex;
                        string medsCorruptionIdCard = "";
                        CardData medsCDataCorruption = null;
                        while (!flag3)
                        {
                            int index1 = UnityEngine.Random.Range(0, stringList.Count);
                            medsRandomCorruptionIndex = index1;
                            medsCorruptionIdCard = stringList[index1];

                            if (!(medsCorruptionIdCard == "resurrection") && !(medsCorruptionIdCard == "resurrectiona") && !(medsCorruptionIdCard == "resurrectionb") && !(medsCorruptionIdCard == "resurrectionrare"))
                            {
                                for (int index2 = 0; index2 < deterministicHashCode % 10; ++index2)
                                    UnityEngine.Random.Range(0, 100);
                                medsCDataCorruption = Globals.Instance.GetCardData(medsCorruptionIdCard, false);
                                if (!((UnityEngine.Object)medsCDataCorruption == (UnityEngine.Object)null) && (!medsCDataCorruption.OnlyInWeekly))
                                    flag3 = true;
                            }
                        }

                        if ((UnityEngine.Object)medsCDataCorruption == (UnityEngine.Object)null)
                            medsCDataCorruption = Globals.Instance.GetCardData(medsCorruptionIdCard, false);
                        if (medsCDataCorruption.CardRarity == CardRarity.Common)
                        {
                            zoneCommonCount[nodeData[3]]++;
                            medsCommon++;
                        }
                        else if (medsCDataCorruption.CardRarity == CardRarity.Uncommon)
                        {
                            zoneUncommonCount[nodeData[3]]++;
                            medsUncommon++;
                        }
                        else if (medsCDataCorruption.CardRarity == CardRarity.Rare)
                        {
                            zoneRareCount[nodeData[3]]++;
                            medsRare++;
                        }
                        else if (medsCDataCorruption.CardRarity == CardRarity.Epic)
                        {
                            zoneEpicCount[nodeData[3]]++;
                            medsEpic++;
                        }
                    }
                }
            }
            /*string z = "Senenthia";
            Log.LogInfo("SEED " + seed + ": SENEN " + zoneEventCount[z] + "/2 events, " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/4 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");
            z = "Aquarfall";
            Log.LogInfo("SEED " + seed + ": AQUAR " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/7 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");
            // z = "Faeborg";
            // Log.LogInfo("SEED " + seed + ": FAEBO " + zoneEventCount[z] + "/2 events, " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/6 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");
            z = "Velkarath";
            Log.LogInfo("SEED " + seed + ": VELKA " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/6 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");
            z = "Voidlow";
            Log.LogInfo("SEED " + seed + ": VOIDL " + zoneEventCount[z] + "/1 events, " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/5 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");
            z = "Voidhigh";
            Log.LogInfo("SEED " + seed + ": VOIDH " + (zoneCommonCount[z] + zoneUncommonCount[z] + zoneRareCount[z] + zoneEpicCount[z]).ToString() + "/2 combats (" + zoneEpicCount[z] + "E " + zoneRareCount[z] + "R " + zoneUncommonCount[z] + "U " + zoneCommonCount[z] + "C)");*/
            Log.LogInfo("SEED " + seed + ": TOTAL " + medsEvents + "/3 events, " + (medsCommon + medsUncommon + medsRare + medsEpic).ToString() + "/24 combats (" + medsEpic + "E " + medsRare + "R " + medsUncommon + "U " + medsCommon + "C)");
        }

        public static void SetTeamExperience(int xp)
        {
            Hero[] medsTeamAtO = Traverse.Create(AtOManager.Instance).Field("teamAtO").GetValue<Hero[]>();
            for (int index = 0; index < medsTeamAtO.Length; ++index)
                medsTeamAtO[index].Experience = xp;
            Traverse.Create(AtOManager.Instance).Field("teamAtO").SetValue(medsTeamAtO);
        }

        public async static void CheckLeaderboards(string leaderboardType)
        {
            Leaderboard? leaderboard = new Leaderboard?();
            leaderboard = await SteamUserStats.FindLeaderboardAsync(leaderboardType);

            if (!leaderboard.HasValue)
            {
                Debug.Log((object)"Couldn't Get Leaderboard!");
            }
            else
            {
                LeaderboardEntry[] scoreboardGlobal = await leaderboard.Value.GetScoresAsync(450);
                Leaderboard leaderboard1 = leaderboard.Value;
                // LeaderboardEntry[] scoreboardFriends = await leaderboard1.GetScoresFromFriendsAsync();
                leaderboard1 = leaderboard.Value;
                LeaderboardEntry[] scoreboardSingle = await leaderboard1.GetScoresAroundUserAsync(0, 0);
                string theList = "ID\tScore\tDetails\t2\t3\t4\t5\t6\t7\t8\t9\t10";
                for (int a = 0; a < scoreboardGlobal.Length; a++)
                    theList += "\n" + scoreboardGlobal[a].User.Id.ToString() + "\t" + scoreboardGlobal[a].Score + "\t" + string.Join("\t", scoreboardGlobal[a].Details);
                File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "scoreboardGlobal.json"), theList);
                theList = "";
                for (int a = 0; a < scoreboardSingle.Length; a++)
                    theList += "\n" + scoreboardSingle[a].User.Id.ToString() + "\t" + scoreboardSingle[a].Score + "\t" + string.Join("\t", scoreboardSingle[a].Details);
                File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "scoreboardSingle.json"), theList);
            }
        }

        public static void FullCardSpriteOutput()
        {

            if (!((UnityEngine.Object)CardScreenManager.Instance != (UnityEngine.Object)null))
                return;
            SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(0);
            CardScreenManager.Instance.ShowCardScreen(true);
            // for each card in cards
            Dictionary<string, CardData> allCards = Traverse.Create(Globals.Instance).Field("_CardsSource").GetValue<Dictionary<string, CardData>>();
            Plugin.Log.LogInfo("i herd u liek memory leaks ;)");
            foreach (KeyValuePair<string, CardData> kvp in allCards)
            {
                Plugin.Log.LogInfo("EXTRACTING CARD IMAGE:" + kvp.Key);
                CardScreenManager.Instance.SetCardData(kvp.Value);
                GameObject cardGO = Traverse.Create(CardScreenManager.Instance).Field("cardGO").GetValue<GameObject>();
                if ((UnityEngine.Object)cardGO != (UnityEngine.Object)null)
                {
                    cardGO.transform.Find("BorderCard").gameObject.SetActive(false);
                    Texture2D snapshot = snapshotCamera.TakeObjectSnapshot(cardGO, UnityEngine.Color.clear, new Vector3(0, 0.008f, 1), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(0.78f, 0.78f, 0.78f), 297, 450);
                    SnapshotCamera.SavePNG(snapshot, kvp.Key, Directory.CreateDirectory(Path.Combine(Application.dataPath, "../Card Images", DataTextConvert.ToString(kvp.Value.CardClass))).FullName);
                    /*if (kvp.Value.CardType == CardType.Corruption)
                        SnapshotCamera.SavePNG(snapshot, kvp.Key, Directory.CreateDirectory(Path.Combine(Application.dataPath, "../Card Images", DataTextConvert.ToString(kvp.Value.CardType))).FullName);*/
                    UnityEngine.Object.Destroy(snapshot);
                    UnityEngine.Object.Destroy(cardGO);
                }
            }
        }

        public static void WilburCardJSONExport()
        {
            string combinedCore = "{\"cards\":[";
            string combinedBoonInjury = "{\"cards\":[";
            string combinedItem = "{\"cards\":[";
            string combinedEnchantment = "{\"cards\":[";
            /*int h = 1; // counts hundreds for combined files
            int a = 1;*/
            foreach (string id in Globals.Instance.CardListNotUpgraded)
            {
                CardData card = Globals.Instance.GetCardData(id);
                if ((UnityEngine.Object)card != (UnityEngine.Object)null)
                {
                    string combined = "{\"name\":\"" + card.CardName + "\",\"cardTypes\":[\"" + Texts.Instance.GetText(DataTextConvert.ToString(card.CardType)) + "\"";
                    foreach (Enums.CardType t in card.CardTypeAux)
                        combined += ",\"" + Texts.Instance.GetText(DataTextConvert.ToString(t)) + "\"";
                    combined += "],\"versions\":{\"base\":" + WilburIndividualCardData(card);
                    // blue upgrade
                    CardData upgrade1 = Globals.Instance.GetCardData(card.UpgradesTo1);
                    if ((UnityEngine.Object)upgrade1 != (UnityEngine.Object)null)
                        combined += ",\"blue\":" + WilburIndividualCardData(upgrade1);
                    // yellow upgrade
                    CardData upgrade2 = Globals.Instance.GetCardData(card.UpgradesTo2);
                    if ((UnityEngine.Object)upgrade2 != (UnityEngine.Object)null)
                        combined += ",\"yellow\":" + WilburIndividualCardData(upgrade2);
                    // purple upgrade
                    if ((UnityEngine.Object) card.UpgradesToRare != (UnityEngine.Object)null)
                    {
                        // jank time? jank time.
                        CardData upgradeRare = Globals.Instance.GetCardData(card.UpgradesToRare.Id);
                        if ((UnityEngine.Object)upgradeRare != (UnityEngine.Object)null)
                            combined += ",\"rare\":" + WilburIndividualCardData(upgradeRare);
                    }
                    combined += "}},";
                    if (card.CardClass == Enums.CardClass.Warrior || card.CardClass == Enums.CardClass.Scout || card.CardClass == Enums.CardClass.Mage || card.CardClass == Enums.CardClass.Healer)
                    {
                        combinedCore += combined;
                    }
                    else if (card.CardClass == Enums.CardClass.Boon || card.CardClass == Enums.CardClass.Injury)
                    {
                        combinedBoonInjury += combined;
                    }
                    else if (card.CardClass == Enums.CardClass.Item || card.CardClass == Enums.CardClass.Enchantment || card.CardClass == Enums.CardClass.Special) // Enchantment cardClass isn't actually used, so this is just in case it _is_ used in the future
                    {
                        if ((UnityEngine.Object) card.ItemEnchantment != (UnityEngine.Object)null)
                        {
                            combinedEnchantment += combined;
                        }
                        else if ((UnityEngine.Object)card.Item != (UnityEngine.Object)null)
                        {
                            combinedItem += combined;
                        }
                        else
                        {
                            Plugin.Log.LogDebug("Did not export WilburCard " + id + " (invalid cardClass)");
                            continue;
                        }
                    }
                    else
                    {
                        Plugin.Log.LogDebug("Did not export WilburCard " + id + " (invalid cardClass)");
                        continue;
                    }
                    Plugin.Log.LogDebug("Exported WilburCard " + id);
                }
                else
                {
                    Plugin.Log.LogWarning("WARNING: could not WilburExport card " + id + " (could not find in Globals.Instance.Cards)");
                }
            }
            // remove trailing comma and write to file
            combinedCore = combinedCore.Remove(combinedCore.Length - 1) + "]}";
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "Wilbur_card_export_core.json"), combinedCore);
            combinedBoonInjury = combinedBoonInjury.Remove(combinedBoonInjury.Length - 1) + "]}";
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "Wilbur_card_export_booninjury.json"), combinedBoonInjury);
            combinedItem = combinedItem.Remove(combinedItem.Length - 1) + "]}";
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "Wilbur_card_export_item.json"), combinedItem);
            combinedEnchantment = combinedEnchantment.Remove(combinedEnchantment.Length - 1) + "]}";
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "Wilbur_card_export_enchantment.json"), combinedEnchantment);
            Plugin.Log.LogInfo("WilburExport complete!");
        }
        public static string WilburIndividualCardData(CardData card)
        {
            WilburCard wc = new();
            wc.target = card.Target;
            wc.cost = card.EnergyCost.ToString() + "<:ato_energy:1017803954308001902>";
            wc.description = WilburDescriptionCleaner(card.DescriptionNormalized);
            wc.vanish = card.Vanish;
            wc.innate = card.Innate;
            string unwieldy = JsonUtility.ToJson(wc).Replace(",\"innate\":false", "").Replace(",\"vanish\":false", "").Replace(@"\n\n", @"\n").Replace(@"\n\n", @"\n").Replace(@"\n- ", @"\n").Replace(@" -""", @"""");
            unwieldy = Regex.Replace(unwieldy, @"\\n(\d)", @" $1").Replace("  ", " ");
            return unwieldy;
        }
        public static string WilburDescriptionCleaner(string desc)
        {
            string newDesc = desc;
            // replace unused formatting tags (spacing, size, color, nobr, line height)
            newDesc = Regex.Replace(newDesc, @"<line-height=15%>\s*<[brBR]*>\s*<\/line-height>", "");
            newDesc = Regex.Replace(newDesc, @"<line-height=40%>\s*<[brBR]*>\s*<\/line-height>", "\n");
            newDesc = Regex.Replace(newDesc, @"<\/*(voffset|line-height|space|size|nobr|color)(=[\d#ABCDEFabcdef%.+-]*)*>", "");
            // replace <BR> with newline
            newDesc = newDesc.Replace("<BR>", "\n");
            newDesc = newDesc.Replace("<br>", "\n");
            // replace sprites with emojis
            newDesc = newDesc.Replace("<sprite name=bleed>", "<:ato_bleed:831708103178321941>");
            newDesc = newDesc.Replace("<sprite name=bless>", "<:ato_bless:831707950984593438>");
            newDesc = newDesc.Replace("<sprite name=block>", "<:ato_block:831707452901818368>");
            newDesc = newDesc.Replace("<sprite name=blunt>", "<:ato_blunt:831707206629064704>");
            newDesc = newDesc.Replace("<sprite name=buffer>", "<:ato_buffer:1017803855058182227>");
            newDesc = newDesc.Replace("<sprite name=burn>", "<:ato_burn:831708488195506202>");
            newDesc = newDesc.Replace("<sprite name=chill>", "<:ato_chill:831708639106170931>");
            newDesc = newDesc.Replace("<sprite name=cold>", "<:ato_cold:831708619330027561>");
            newDesc = newDesc.Replace("<sprite name=courage>", "<:ato_courage:1017803913040232488>");
            newDesc = newDesc.Replace("<sprite name=crack>", "<:ato_crack:831709232339615805>");
            newDesc = newDesc.Replace("<sprite name=dark>", "<:ato_dark:831708725571616788>");
            newDesc = newDesc.Replace("<sprite name=decay>", "<:ato_decay:831711525269536768>");
            newDesc = newDesc.Replace("<sprite name=disarm>", "<:ato_disarm:1017803928341065738>");
            newDesc = newDesc.Replace("<sprite name=doom>", "<:ato_doom:1017803942614282250>");
            newDesc = newDesc.Replace("<sprite name=energize>", "<:ato_energize:831710263631020072>");
            newDesc = newDesc.Replace("<sprite name=energy>", "<:ato_energy:1017803954308001902>");
            newDesc = newDesc.Replace("<sprite name=card>", "<:ato_card:1017803874498777118>");
            newDesc = newDesc.Replace("<sprite name=cards>", "<:ato_card:1017803874498777118>");
            newDesc = newDesc.Replace("<sprite name=evasion>", "<:ato_evasion:1017803990492254248>");
            newDesc = newDesc.Replace("<sprite name=fast>", "<:ato_fast:831709865957130260>");
            newDesc = newDesc.Replace("<sprite name=fatigue>", "<:ato_fatigue:1017804029348298882>");
            newDesc = newDesc.Replace("<sprite name=fire>", "<:ato_fire:831708462093697024>");
            newDesc = newDesc.Replace("<sprite name=fortify>", "<:ato_fortify:1017804040589017139>");
            newDesc = newDesc.Replace("<sprite name=fury>", "<:ato_fury:1017804054333771827>");
            newDesc = newDesc.Replace("<sprite name=heal>", "<:ato_heal:831708023822090292>");
            newDesc = newDesc.Replace("<sprite name=holy>", "<:ato_holy:831707893559853076>");
            newDesc = newDesc.Replace("<sprite name=health>", "<:ato_health:831711486132224070>");
            newDesc = newDesc.Replace("<sprite name=insane>", "<:ato_insane:831708800276627516>");
            newDesc = newDesc.Replace("<sprite name=inspire>", "<:ato_inspire:1017804069810741259>");
            newDesc = newDesc.Replace("<sprite name=insulate>", "<:ato_insulate:1017804082406232115>");
            newDesc = newDesc.Replace("<sprite name=invulnerable>", "<:ato_invulnerable:1017804093953159199>");
            newDesc = newDesc.Replace("<sprite name=lightning>", "<:ato_lightning:831708386659139646>");
            newDesc = newDesc.Replace("<sprite name=mark>", "<:ato_mark:831709147774320681>");
            newDesc = newDesc.Replace("<sprite name=mind>", "<:ato_mind:831708773490884619>");
            newDesc = newDesc.Replace("<sprite name=mitigate>", "<:ato_mitigate:1017804107995680848>");
            newDesc = newDesc.Replace("<sprite name=paralyze>", "<:ato_paralyze:1017804120633131018>");
            newDesc = newDesc.Replace("<sprite name=piercing>", "<:ato_piercing:831707237499797534>");
            newDesc = newDesc.Replace("<sprite name=powerful>", "<:ato_powerful:1017804148260995102>");
            newDesc = newDesc.Replace("<sprite name=poison>", "<:ato_poison:1017804131588657243>");
            newDesc = newDesc.Replace("<sprite name=cardrandom>", "<:ato_random:1095706686016200714>");
            newDesc = newDesc.Replace("<sprite name=regeneration>", "<:ato_regeneration:836680199109476392>");
            newDesc = newDesc.Replace("<sprite name=reinforce>", "<:ato_reinforce:1017804164107079732>");
            newDesc = newDesc.Replace("<sprite name=sanctify>", "<:ato_sanctify:831707981733298186>");
            newDesc = newDesc.Replace("<sprite name=shackle>", "<:ato_shackle:1017804176492871715>");
            newDesc = newDesc.Replace("<sprite name=shadow>", "<:ato_shadow:831708702981619783>");
            newDesc = newDesc.Replace("<sprite name=sharp>", "<:ato_sharp:1017804192024383608>");
            newDesc = newDesc.Replace("<sprite name=shield>", "<:ato_shield:831707596405997578>");
            newDesc = newDesc.Replace("<sprite name=sight>", "<:ato_sight:831708916340490240>");
            newDesc = newDesc.Replace("<sprite name=silence>", "<:ato_silence:1017804207732035665>");
            newDesc = newDesc.Replace("<sprite name=slashing>", "<:ato_slash:831707157426602034>");
            newDesc = newDesc.Replace("<sprite name=slow>", "<:ato_slow:831710190662844416>");
            newDesc = newDesc.Replace("<sprite name=spark>", "<:ato_spark:831708425775480892>");
            newDesc = newDesc.Replace("<sprite name=stanzai>", "<:ato_stanza1:831710428156395563>");
            newDesc = newDesc.Replace("<sprite name=stanzaii>", "<:ato_stanza2:831710440731574283>");
            newDesc = newDesc.Replace("<sprite name=stanzaiii>", "<:ato_stanza3:831710474202251265>");
            newDesc = newDesc.Replace("<sprite name=stealth>", "<:ato_stealth:831709537592803358>");
            newDesc = newDesc.Replace("<sprite name=stealthbonus>", "<:ato_stealth:831709537592803358>");
            newDesc = newDesc.Replace("<sprite name=stress>", "<:ato_stress:1017804219270561792>");
            newDesc = newDesc.Replace("<sprite name=thorns>", "<:ato_thorns:831709250656927805>");
            newDesc = newDesc.Replace("<sprite name=taunt>", "<:ato_taunt:831711779494821898>");
            newDesc = newDesc.Replace("<sprite name=vitality>", "<:ato_vitality:836679899904868384>");
            newDesc = newDesc.Replace("<sprite name=vulnerable>", "<:ato_vulnerable:834829138488983563>");
            newDesc = newDesc.Replace("<sprite name=weak>", "<:ato_weak:1017804231773790268>");
            newDesc = newDesc.Replace("<sprite name=wet>", "<:ato_wet:1017804243526221884>");
            newDesc = newDesc.Replace("<sprite name=scourge>", "<:ato_scourge:1142114629431087174>");
            newDesc = newDesc.Replace("<sprite name=zeal>", "<:ato_zeal:1142114574544404552>");
            // fix whitespace
            newDesc = newDesc.Replace("  ", " ");
            newDesc = newDesc.Replace("  ", " ");
            newDesc = newDesc.Replace("\n ", "\n");
            newDesc = newDesc.Replace(" \n", "\n");
            newDesc = newDesc.Replace(" <:ato_", "<:ato_");
            return newDesc.Trim();
        }
        
        public static void MapNodeExport() // exports map node positions into text format
        {
            Node[] foundNodes = Resources.FindObjectsOfTypeAll<Node>();
            string s = "name\tzone\tlocalx\tlocaly\tlocalz\tposx\tposy\tposz";
            foreach (Node n in foundNodes)
                s += "\n" + n.name + "\t" + n.nodeData.NodeZone.ZoneId + "\t" + n.transform.localPosition.x.ToString() + "\t" + n.transform.localPosition.y.ToString() + "\t" + n.transform.localPosition.z.ToString() + "\t" + n.transform.position.x.ToString() + "\t" + n.transform.position.y.ToString() + "\t" + n.transform.position.z.ToString();
            File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "nodePos.txt"), s);
        }

        public static void RoadExport(bool forExcel = false) // exports roads into text format
        {
            if (forExcel)
            {
                string s = "name\tpos1\tpos2\tpos3\tpos4\tpos5\tpos6\tpos7\tpos8\tpos9\tpos10\tpos11";
                for (int a = 0; a < MapManager.Instance.mapList.Count; a++)
                {
                    foreach (Transform transform1 in MapManager.Instance.mapList[a].transform)
                    {
                        if (transform1.gameObject.name == "Roads")
                        {
                            for (int b = 0; b < transform1.childCount; b++)
                            {
                                s += "\n" + transform1.GetChild(b).gameObject.name;
                                LineRenderer lr = transform1.GetChild(b).gameObject.GetComponent<LineRenderer>();
                                Vector3[] v3s = new Vector3[lr.positionCount];
                                lr.GetPositions(v3s);
                                foreach (Vector3 v3 in v3s)
                                {
                                    float mX = v3.x + transform1.position.x;
                                    float mY = v3.y + transform1.position.y;
                                    s += ",(" + mX + "," + mY + ")";
                                }
                            }
                        }
                    }
                }
                File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "linePosForExcel.txt"), s);
            }
            else
            {
                // actual roadsTXT
                string s = @"\\vanilla roadsTXT. Please ONLY use the roads you need for custom paths, because otherwise load times will be significantly increased and interactions between mods may cause errors and strange behaviour!";
                s += "\n" + @"\\node_from-node_to|(x1,y1),(x2,y2),(x3,y3),(x4,y4),... [etc]";
                for (int a = 0; a < MapManager.Instance.mapList.Count; a++)
                {
                    foreach (Transform transform1 in MapManager.Instance.mapList[a].transform)
                    {
                        if (transform1.gameObject.name == "Roads")
                        {
                            for (int b = 0; b < transform1.childCount; b++)
                            {
                                s += "\n" + transform1.GetChild(b).gameObject.name + "|";
                                LineRenderer lr = transform1.GetChild(b).gameObject.GetComponent<LineRenderer>();
                                Vector3[] v3s = new Vector3[lr.positionCount];
                                lr.GetPositions(v3s);
                                foreach (Vector3 v3 in v3s)
                                {
                                    float mX = v3.x + transform1.position.x;
                                    float mY = v3.y + transform1.position.y;
                                    s += ",(" + mX + "," + mY + ")";
                                }
                            }
                        }
                    }
                }
                s = s.Replace("|,", "|");
                RecursiveFolderCreate("Obeliskial_exported", "roadsTXT");
                File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "roadsTXT", "vanilla.txt"), s);
            }
        }
    }
}
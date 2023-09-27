using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks.Data;
using Steamworks;
using System.Linq;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using static Unity.Audio.Handle;
using static Enums;
using UnityEngine.UIElements;
using System.Xml.Linq;
using System.Text;

namespace Obeliskial_Options
{
    internal class SupportingActs
    {

        public static void OnGameLobbyJoinRequested(Lobby _lobby, SteamId _friendId)
        {
            Debug.Log((object)nameof(OnGameLobbyJoinRequested));
            Debug.Log((object)_lobby.Id);
            Debug.Log((object)_friendId);
            SteamMatchmaking.JoinLobbyAsync(_lobby.Id);
        }

        public static void OnNewLaunchParameters()
        {
            Debug.Log((object)nameof(OnNewLaunchParameters));
            Debug.Log((object)("[Steam] launchParam -> " + SteamApps.GetLaunchParam("+connect_lobby")));
        }

        public static void OnChatMessage(Friend _friendId, string _string0, string _string1)
        {
            Debug.Log((object)nameof(OnChatMessage));
            Debug.Log((object)_friendId);
            Debug.Log((object)_string0);
            Debug.Log((object)_string1);
        }

        public static void OnGameRichPresenceJoinRequested(Friend _friendId, string _action)
        {
            Debug.Log((object)nameof(OnGameRichPresenceJoinRequested));
            Debug.Log((object)_friendId);
            Debug.Log((object)_action);
        }

        public static void OnLobbyCreated(Result result, Lobby _lobby)
        {
            SteamManager.Instance.lobby = _lobby;
            SteamManager.Instance.lobby.SetPublic();
            SteamManager.Instance.lobby.SetJoinable(true);
            SteamManager.Instance.lobby.SetData("RoomName", NetworkManager.Instance.GetRoomName());
            Debug.Log((object)("[Lobby] OnLobbyCreated " + SteamManager.Instance.lobby.Id.ToString()));
            SteamFriends.OpenGameInviteOverlay(SteamManager.Instance.lobby.Id);
        }

        public static void OnLobbyMemberJoined(Lobby _lobby, Friend _friendId)
        {
        }

        public static void OnLobbyEntered(Lobby _lobby)
        {
            Debug.Log((object)"[Lobby] OnLobbyEntered");
            if (_lobby.IsOwnedBy(SteamManager.Instance.steamId))
                return;
            string data = _lobby.GetData("RoomName");
            Debug.Log((object)("Steam wants to join room -> " + data));
            NetworkManager.Instance.WantToJoinRoomName = data;
            SteamManager.Instance.steamLoaded = true;
        }

        public static void OnLobbyInvite(Friend _friendId, Lobby _lobby)
        {
            Debug.Log((object)"[Lobby] OnLobbyInvite");
            Debug.Log((object)_friendId);
        }

        public static int TeamHeroToInt(Hero[] medsTeam)
        {
            int team = 0;
            for (int index = 0; index < 4; ++index)
            {
                string subclassName = medsTeam[index].SubclassName;
                if (subclassName == "medsdlctwo")
                {
                    subclassName = (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo);
                }
                else if (subclassName == "medsdlcthree")
                {
                    subclassName = (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree);
                }
                else if (subclassName == "medsdlcfour")
                {
                    subclassName = (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour);
                }
                team += (Array.IndexOf(Plugin.medsSubclassList, subclassName) + 1) * (int)Math.Pow(100, index);
            }
            Plugin.Log.LogDebug("TeamHeroToInt: " + team);
            return team;
        }
        public static string TeamIntToString(int team)
        {
            int[] iTeam = new int[4];
            string[] sTeam = new string[4];

            iTeam[3] = team / 1000000;
            iTeam[2] = (team % 1000000) / 10000;
            iTeam[1] = (team % 10000) / 100;
            iTeam[0] = (team % 100);
            for (int a = 0; a < 4; a++)
            {
                if (iTeam[a] < 1 || iTeam[a] > Plugin.medsSubclassList.Length)
                    sTeam[a] = "UNKNOWN";
                else
                    sTeam[a] = Plugin.medsSubclassList[iTeam[a] - 1];
            }
            Plugin.Log.LogDebug("TeamIntToString: " + string.Join(", ", sTeam));
            return string.Join(", ", sTeam);
        }
        public static async Task SetScoreLeaderboard(int score, bool singleplayer = true, string mode = "RankingAct4")
        {
            int gameId32 = Functions.StringToAsciiInt32(AtOManager.Instance.GetGameId());
            int details = Convert.ToInt32(gameId32 + score * 101);

            int seed = AtOManager.Instance.GetGameId().GetDeterministicHashCode();

            int team = TeamHeroToInt(AtOManager.Instance.GetTeam());
            int nodes = 0; // #TODO: nodelist
            string[] gameVersion = GameManager.Instance.gameVersion.Split(".");
            int vanillaVersion = int.Parse(gameVersion[0]) * 10000 + int.Parse(gameVersion[1]) * 100 + int.Parse(gameVersion[2]);
            gameVersion = Plugin.ModVersion.Split(".");
            int obeliskialVersion = int.Parse(gameVersion[0]) * 10000 + int.Parse(gameVersion[1]) * 100 + int.Parse(gameVersion[2]);
            

            Leaderboard? leaderboardAsync = await SteamUserStats.FindLeaderboardAsync(mode + (singleplayer ? "" : "Coop"));
            if (leaderboardAsync.HasValue)
            {
                LeaderboardUpdate? nullable = await leaderboardAsync.Value.SubmitScoreAsync(score, new int[7]
                {
                        gameId32,
                        details,
                        vanillaVersion,
                        obeliskialVersion,
                        seed,
                        team,
                        nodes
                });
            }
            else
                Debug.Log((object)"Couldn't Get Leaderboard!");
        }
    }
    [HarmonyPatch]
    internal class Patch20230319
    {
        public static Vector3 medsPosIni;
        public static Vector3 medsPosIniBlocked;
        public static bool bSelectingPerk = false;
        //public static bool bRemovingCards = false;
        //public static bool bPreventCardRemoval = false;
        public static bool bFinalResolution = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), "Start")]
        public static void MMStartPostfix(ref MainMenuManager __instance)
        {
            // __instance.version.text += __instance.version.text.Replace("(", "    (").Replace(")", ")     ");
            __instance.version.text += "\nOO v" + Plugin.ModVersion + "\n" + Plugin.ModDate;
            // TMP_Text meds1 = __instance.gameModeSelectionChoose.GetComponent<TMP_Text>();
            // TMP_SpriteAsset meds2 = meds1.spriteAsset;
            // Plugin.Log.LogDebug("meds1: " + meds1.name);
            // Plugin.Log.LogDebug("meds2: " + meds2.name);
            // Plugin.Log.LogDebug("meds3: " + meds2.spriteCharacterTable.Count);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "GetLootData")]
        public static void GetLootDataPostfix(ref LootData __result)
        {
            // Plugin.Log.LogInfo("GETLOOTDATA START, shops with no purchase: " + Plugin.iShopsWithNoPurchase);
            if (__result == (LootData)null)
                return;
            Plugin.Log.LogDebug("GetLootData uncommon: " + __result.DefaultPercentUncommon + " rare: " + __result.DefaultPercentRare + " epic: " + __result.DefaultPercentEpic + " mythic: " + __result.DefaultPercentMythic);
            // instantiate a new version of the LootData so we're not changing the original values!
            __result = UnityEngine.Object.Instantiate<LootData>(__result);
            if (Plugin.IsHost() ? Plugin.medsShopRarity.Value : Plugin.medsMPShopRarity)
            {
                float num0 = 0f;
                if (MadnessManager.Instance.IsMadnessTraitActive("impedingdoom"))
                    num0 += 0.25f;
                if (MadnessManager.Instance.IsMadnessTraitActive("decadence"))
                    num0 += 0.5f;
                if (MadnessManager.Instance.IsMadnessTraitActive("restrictedpower"))
                    num0 += 1f;
                if (MadnessManager.Instance.IsMadnessTraitActive("resistantmonsters"))
                    num0 += 0.75f;
                if (MadnessManager.Instance.IsMadnessTraitActive("poverty"))
                    num0 += 1f;
                if (MadnessManager.Instance.IsMadnessTraitActive("overchargedmonsters"))
                    num0 += 1.5f;
                if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats"))
                    num0 += 0.75f;
                if (MadnessManager.Instance.IsMadnessTraitActive("despair"))
                    num0 += 1.25f;
                num0 += (float)AtOManager.Instance.GetNgPlus(false);
                float num1 = 1f;
                if (AtOManager.Instance.corruptionId == "shop")
                    num1 += 2f * ((float)AtOManager.Instance.GetTownTier() + 1);
                if (AtOManager.Instance.corruptionId == "exoticshop")
                    num1 += 5f * ((float)AtOManager.Instance.GetTownTier() + 1);
                __result.DefaultPercentRare += (float)Math.Pow((float)AtOManager.Instance.GetTownTier() + 1, Plugin.medsBLPTownTierPower) * num0 * num1 / 50f * Plugin.medsBLPRareMult;
                __result.DefaultPercentEpic += (float)Math.Pow((float)AtOManager.Instance.GetTownTier() + 1, Plugin.medsBLPTownTierPower) * num0 * num1 / 50f * Plugin.medsBLPEpicMult;
                __result.DefaultPercentMythic += (float)Math.Pow((float)AtOManager.Instance.GetTownTier() + 1, Plugin.medsBLPTownTierPower) * num0 * num1 / 50f * Plugin.medsBLPMythicMult;
                Plugin.Log.LogDebug("ShopRarity uncommon: " + __result.DefaultPercentUncommon + " rare: " + __result.DefaultPercentRare + " epic: " + __result.DefaultPercentEpic + " mythic: " + __result.DefaultPercentMythic);
            }
            float fBadLuckProt = Plugin.IsHost() ? (float)Plugin.medsShopBadLuckProtection.Value : (float)Plugin.medsMPShopBadLuckProtection;
            // Plugin.Log.LogInfo("fBadLuckProt over 0??? " + fBadLuckProt);
            if (fBadLuckProt > 0f)
            {
                fBadLuckProt = fBadLuckProt * (float)Math.Pow((float)AtOManager.Instance.GetTownTier() + 1, Plugin.medsBLPTownTierPower) * (float)Math.Pow((float)Plugin.iShopsWithNoPurchase, Plugin.medsBLPRollPower) / 100000;
                Plugin.Log.LogDebug("fBadLuckPro: " + fBadLuckProt);
                __result.DefaultPercentUncommon += fBadLuckProt * Plugin.medsBLPUncommonMult;
                __result.DefaultPercentRare += fBadLuckProt * Plugin.medsBLPRareMult;
                __result.DefaultPercentEpic += fBadLuckProt * Plugin.medsBLPEpicMult;
                __result.DefaultPercentMythic += fBadLuckProt * Plugin.medsBLPMythicMult * (float)AtOManager.Instance.GetTownTier();
                if (__result.DefaultPercentMythic >= 100f)
                {
                    __result.DefaultPercentMythic = 100f;
                    __result.DefaultPercentEpic = 0f;
                    __result.DefaultPercentRare = 0f;
                    __result.DefaultPercentUncommon = 0f;
                }
                else if (__result.DefaultPercentMythic + __result.DefaultPercentEpic > 100f)
                {
                    __result.DefaultPercentEpic = 100f - __result.DefaultPercentMythic;
                    __result.DefaultPercentRare = 0f;
                    __result.DefaultPercentUncommon = 0f;
                }
                else if (__result.DefaultPercentMythic + __result.DefaultPercentEpic + __result.DefaultPercentRare > 100f)
                {
                    __result.DefaultPercentRare = 100f - __result.DefaultPercentMythic - __result.DefaultPercentEpic;
                    __result.DefaultPercentUncommon = 0f;
                }
                else if (__result.DefaultPercentMythic + __result.DefaultPercentEpic + __result.DefaultPercentRare + __result.DefaultPercentUncommon > 100f)
                {
                    __result.DefaultPercentUncommon = 100f - __result.DefaultPercentMythic - __result.DefaultPercentEpic - __result.DefaultPercentRare;
                }
                Plugin.Log.LogDebug("BadLuckProt uncommon: " + __result.DefaultPercentUncommon + " rare: " + __result.DefaultPercentRare + " epic: " + __result.DefaultPercentEpic + " mythic: " + __result.DefaultPercentMythic);
                Plugin.iShopsWithNoPurchase += 1;
                Plugin.Log.LogDebug("shops with no purchase increased to: " + Plugin.iShopsWithNoPurchase);
            }
            //Plugin.Log.LogDebug("end of GetLootData");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Loot), "GetLootItems", new Type[] { typeof(string), typeof(string) })]
        public static void GetLootItemsPostfix(ref List<string> __result)
        {
            // Plugin.Log.LogDebug($"rare commencement madnessdif: {AtOManager.Instance.GetMadnessDifficulty()}! obeliskmadness: {AtOManager.Instance.GetObeliskMadness()}! ngplus: {AtOManager.Instance.GetNgPlus()}! {__result.Count}!");
            if (__result != null)
            {
                for (int index3 = 0; index3 < __result.Count; ++index3)
                {
                    int num6 = UnityEngine.Random.Range(0, 100);
                    // Plugin.Log.LogDebug($"num6: {num6}!");
                    CardData cardData = Globals.Instance.GetCardData(__result[index3], false);
                    if (!((UnityEngine.Object)cardData == (UnityEngine.Object)null))
                    {
                        int num5 = 0;
                        num5 += Functions.FuncRoundToInt((float)AtOManager.Instance.GetTownTier());
                        num5 *= 2;
                        float num0 = 0f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("impedingdoom"))
                            num0 += 0.25f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("decadence"))
                            num0 += 0.5f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("restrictedpower"))
                            num0 += 1f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("resistantmonsters"))
                            num0 += 0.75f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("poverty"))
                            num0 += 0.5f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("overchargedmonsters"))
                            num0 += 1.5f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats"))
                            num0 += 0.75f;
                        if (MadnessManager.Instance.IsMadnessTraitActive("despair"))
                            num0 += 1.25f;
                        num0 += (float)AtOManager.Instance.GetNgPlus(false);
                        num5 += Functions.FuncRoundToInt((float)num0);
                        if (!AtOManager.Instance.CharInTown())
                            num5 += 40;
                        if (AtOManager.Instance.corruptionId == "shop")
                            num5 -= 10;
                        if (AtOManager.Instance.corruptionId == "exoticshop")
                            num5 += 10;
                        if (!(AtOManager.Instance.corruptionId == "exoticshop") && !(AtOManager.Instance.corruptionId == "rareshop") && !(AtOManager.Instance.corruptionId == "shop") && !(AtOManager.Instance.CharInTown()) && Plugin.medsLootCorrupt.Value)
                            num5 += 100;
                        bool flag = false;
                        if ((cardData.CardRarity == Enums.CardRarity.Mythic || cardData.CardRarity == Enums.CardRarity.Epic) && num6 < 3 + num5)
                            flag = true;
                        else if (cardData.CardRarity == Enums.CardRarity.Rare && num6 < 7 + num5)
                            flag = true;
                        else if (cardData.CardRarity == Enums.CardRarity.Uncommon && num6 < 11 + num5)
                            flag = true;
                        else if (cardData.CardRarity == Enums.CardRarity.Common && num6 < 15 + num5)
                            flag = true;
                        bool bAllowCorrupt = true;
                        if (AtOManager.Instance.CharInTown())
                        {
                            // town shop
                            bAllowCorrupt = Plugin.IsHost() ? Plugin.medsTownShopCorrupt.Value : Plugin.medsMPTownShopCorrupt;
                        }
                        else if ((AtOManager.Instance.corruptionId == "exoticshop") || (AtOManager.Instance.corruptionId == "rareshop") || (AtOManager.Instance.corruptionId == "shop"))
                        {
                            // challenge shop
                            bAllowCorrupt = Plugin.IsHost() ? Plugin.medsObeliskShopCorrupt.Value : Plugin.medsMPObeliskShopCorrupt;
                        }
                        else
                        {
                            // node shop? I can't imagine what else this could be.
                            bAllowCorrupt = Plugin.IsHost() ? Plugin.medsMapShopCorrupt.Value : Plugin.medsMPMapShopCorrupt;
                        }
                        if (bAllowCorrupt && flag && (UnityEngine.Object)cardData.UpgradesToRare != (UnityEngine.Object)null)
                            __result[index3] = cardData.UpgradesToRare.Id;
                        // Plugin.Log.LogDebug($"num6: {num6}! num5: {num5}! {flag}!");
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ProfanityFilter.ProfanityFilter), "CensorString", new Type[] { typeof(string), typeof(char), typeof(bool) })]
        public static bool CensorStringPrefix(ref string __result, string sentence)
        {
            if (Plugin.medsProfane.Value)
            {
                __result = sentence;
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Functions), "GetCardByRarity")]
        public static void GetCardByRarityPostfix(ref string __result, CardData _cardData)
        {
            if (Plugin.IsHost() ? Plugin.medsCorruptGiovanna.Value : Plugin.medsMPCorruptGiovanna)
                __result = _cardData?.UpgradesToRare?.Id ?? __result;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TownManager), "Start")]
        public static void StartPostfix()
        {
            // last updated... 1.0.0, maybe? need to do it again (or just move to PlayerReq method)
            if (Plugin.IsHost() ? Plugin.medsKeyItems.Value : Plugin.medsMPKeyItems)
            {
                // Plugin.Log.LogInfo($"giving key items!");
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("altarcorrupted"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("ancientsong"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("apprentice"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("arenachampion"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("asmodyquest1"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("asmodyquest2"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("asmodyquest3"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("assassinquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("bakerson"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("bakersonsaved"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("bakersonscorched"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("barakexit"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("belphyorhorn"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("belphyorquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("belphyorquestdone"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("belphyorscroll"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("belphyorsummoned"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("bigfish"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatcenter"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatdown"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatfaenlor"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatfail"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatrime"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("boatup"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("caravan"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("caravannopay"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("caravanpay"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("childofthestorm"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("cranecode"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("crocomenburn"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("crocomenhelp"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("crocomensteal"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("crossroadnorth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("crossroadsouth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("eeriecgestiout"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("elemlava"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("elemrock"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("elemstone"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("elvenarmory"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("elvenmansion"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("farminfested"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("forestrail"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("freeboat"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goblinhelp"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goblinnorth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goblinquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goldensheep"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goldensheepquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goldenwool"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("goldtrophy"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("grainsack"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("hammer"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("happyowlrunes"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("harpyegg"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("hugeruby"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("impaltar"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("jeweledkey"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("keyaquarfall"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("keynorth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("keyobelisk"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("keyvelkarath"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("largeemerald"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("lavacascade"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("lizardmenhelp"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("lorequest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("magicsapphyre"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("magictorch"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("meetraul"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("merchantcard"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("minstrelquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("moonstone"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("mosquitoegg"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("naganotes"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("obsidianingots"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("oldjournal"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("oldnotes"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("oldrope"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("pigcaptured"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("piratecoin"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("priestquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("prophetquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("pyromancerquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("rabbitmeat"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("rime"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("rimeoftheancienti"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("rimeoftheancientii"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("samaritan"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("sentinelquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("sewersexit"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("sheeplost"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("sheeplostnode"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("slimebait"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("slimefriend"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("smalllog"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("spiderpassage"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("stargazervisited"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("testsubject"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("thiefhealed"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("treasurehunt"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("treasurehuntii"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("treasuremap"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("treasurespot"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("tsnemogem"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("tsnemotip1"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("tsnemotip2"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("voidnorth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("voidnorthpass"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("voidsouth"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("voidsouthpass"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("wardenquest"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("waterwatchtower"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("wolfstory"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("ylmerseed"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("youngharpy"));
                AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("zonahielo"));
                if (NetworkManager.Instance.IsMaster() && GameManager.Instance.IsMultiplayer())
                {
                    AtOManager.Instance.AddPlayerRequirementOthers("altarcorrupted");
                    AtOManager.Instance.AddPlayerRequirementOthers("ancientsong");
                    AtOManager.Instance.AddPlayerRequirementOthers("apprentice");
                    AtOManager.Instance.AddPlayerRequirementOthers("arenachampion");
                    AtOManager.Instance.AddPlayerRequirementOthers("asmodyquest1");
                    AtOManager.Instance.AddPlayerRequirementOthers("asmodyquest2");
                    AtOManager.Instance.AddPlayerRequirementOthers("asmodyquest3");
                    AtOManager.Instance.AddPlayerRequirementOthers("assassinquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("bakerson");
                    AtOManager.Instance.AddPlayerRequirementOthers("bakersonsaved");
                    AtOManager.Instance.AddPlayerRequirementOthers("bakersonscorched");
                    AtOManager.Instance.AddPlayerRequirementOthers("barakexit");
                    AtOManager.Instance.AddPlayerRequirementOthers("belphyorhorn");
                    AtOManager.Instance.AddPlayerRequirementOthers("belphyorquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("belphyorquestdone");
                    AtOManager.Instance.AddPlayerRequirementOthers("belphyorscroll");
                    AtOManager.Instance.AddPlayerRequirementOthers("belphyorsummoned");
                    AtOManager.Instance.AddPlayerRequirementOthers("bigfish");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatcenter");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatdown");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatfaenlor");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatfail");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatrime");
                    AtOManager.Instance.AddPlayerRequirementOthers("boatup");
                    AtOManager.Instance.AddPlayerRequirementOthers("caravan");
                    AtOManager.Instance.AddPlayerRequirementOthers("caravannopay");
                    AtOManager.Instance.AddPlayerRequirementOthers("caravanpay");
                    AtOManager.Instance.AddPlayerRequirementOthers("childofthestorm");
                    AtOManager.Instance.AddPlayerRequirementOthers("cranecode");
                    AtOManager.Instance.AddPlayerRequirementOthers("crocomenburn");
                    AtOManager.Instance.AddPlayerRequirementOthers("crocomenhelp");
                    AtOManager.Instance.AddPlayerRequirementOthers("crocomensteal");
                    AtOManager.Instance.AddPlayerRequirementOthers("crossroadnorth");
                    AtOManager.Instance.AddPlayerRequirementOthers("crossroadsouth");
                    AtOManager.Instance.AddPlayerRequirementOthers("eeriecgestiout");
                    AtOManager.Instance.AddPlayerRequirementOthers("elemlava");
                    AtOManager.Instance.AddPlayerRequirementOthers("elemrock");
                    AtOManager.Instance.AddPlayerRequirementOthers("elemstone");
                    AtOManager.Instance.AddPlayerRequirementOthers("elvenarmory");
                    AtOManager.Instance.AddPlayerRequirementOthers("elvenmansion");
                    AtOManager.Instance.AddPlayerRequirementOthers("farminfested");
                    AtOManager.Instance.AddPlayerRequirementOthers("forestrail");
                    AtOManager.Instance.AddPlayerRequirementOthers("freeboat");
                    AtOManager.Instance.AddPlayerRequirementOthers("goblinhelp");
                    AtOManager.Instance.AddPlayerRequirementOthers("goblinnorth");
                    AtOManager.Instance.AddPlayerRequirementOthers("goblinquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("goldensheep");
                    AtOManager.Instance.AddPlayerRequirementOthers("goldensheepquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("goldenwool");
                    AtOManager.Instance.AddPlayerRequirementOthers("goldtrophy");
                    AtOManager.Instance.AddPlayerRequirementOthers("grainsack");
                    AtOManager.Instance.AddPlayerRequirementOthers("hammer");
                    AtOManager.Instance.AddPlayerRequirementOthers("happyowlrunes");
                    AtOManager.Instance.AddPlayerRequirementOthers("harpyegg");
                    AtOManager.Instance.AddPlayerRequirementOthers("hugeruby");
                    AtOManager.Instance.AddPlayerRequirementOthers("impaltar");
                    AtOManager.Instance.AddPlayerRequirementOthers("jeweledkey");
                    AtOManager.Instance.AddPlayerRequirementOthers("keyaquarfall");
                    AtOManager.Instance.AddPlayerRequirementOthers("keynorth");
                    AtOManager.Instance.AddPlayerRequirementOthers("keyobelisk");
                    AtOManager.Instance.AddPlayerRequirementOthers("keyvelkarath");
                    AtOManager.Instance.AddPlayerRequirementOthers("largeemerald");
                    AtOManager.Instance.AddPlayerRequirementOthers("lavacascade");
                    AtOManager.Instance.AddPlayerRequirementOthers("lizardmenhelp");
                    AtOManager.Instance.AddPlayerRequirementOthers("lorequest");
                    AtOManager.Instance.AddPlayerRequirementOthers("magicsapphyre");
                    AtOManager.Instance.AddPlayerRequirementOthers("magictorch");
                    AtOManager.Instance.AddPlayerRequirementOthers("meetraul");
                    AtOManager.Instance.AddPlayerRequirementOthers("merchantcard");
                    AtOManager.Instance.AddPlayerRequirementOthers("minstrelquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("moonstone");
                    AtOManager.Instance.AddPlayerRequirementOthers("mosquitoegg");
                    AtOManager.Instance.AddPlayerRequirementOthers("naganotes");
                    AtOManager.Instance.AddPlayerRequirementOthers("obsidianingots");
                    AtOManager.Instance.AddPlayerRequirementOthers("oldjournal");
                    AtOManager.Instance.AddPlayerRequirementOthers("oldnotes");
                    AtOManager.Instance.AddPlayerRequirementOthers("oldrope");
                    AtOManager.Instance.AddPlayerRequirementOthers("pigcaptured");
                    AtOManager.Instance.AddPlayerRequirementOthers("piratecoin");
                    AtOManager.Instance.AddPlayerRequirementOthers("priestquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("prophetquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("pyromancerquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("rabbitmeat");
                    AtOManager.Instance.AddPlayerRequirementOthers("rime");
                    AtOManager.Instance.AddPlayerRequirementOthers("rimeoftheancienti");
                    AtOManager.Instance.AddPlayerRequirementOthers("rimeoftheancientii");
                    AtOManager.Instance.AddPlayerRequirementOthers("samaritan");
                    AtOManager.Instance.AddPlayerRequirementOthers("sentinelquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("sewersexit");
                    AtOManager.Instance.AddPlayerRequirementOthers("sheeplost");
                    AtOManager.Instance.AddPlayerRequirementOthers("sheeplostnode");
                    AtOManager.Instance.AddPlayerRequirementOthers("slimebait");
                    AtOManager.Instance.AddPlayerRequirementOthers("slimefriend");
                    AtOManager.Instance.AddPlayerRequirementOthers("smalllog");
                    AtOManager.Instance.AddPlayerRequirementOthers("spiderpassage");
                    AtOManager.Instance.AddPlayerRequirementOthers("stargazervisited");
                    AtOManager.Instance.AddPlayerRequirementOthers("testsubject");
                    AtOManager.Instance.AddPlayerRequirementOthers("thiefhealed");
                    AtOManager.Instance.AddPlayerRequirementOthers("treasurehunt");
                    AtOManager.Instance.AddPlayerRequirementOthers("treasurehuntii");
                    AtOManager.Instance.AddPlayerRequirementOthers("treasuremap");
                    AtOManager.Instance.AddPlayerRequirementOthers("treasurespot");
                    AtOManager.Instance.AddPlayerRequirementOthers("tsnemogem");
                    AtOManager.Instance.AddPlayerRequirementOthers("tsnemotip1");
                    AtOManager.Instance.AddPlayerRequirementOthers("tsnemotip2");
                    AtOManager.Instance.AddPlayerRequirementOthers("voidnorth");
                    AtOManager.Instance.AddPlayerRequirementOthers("voidnorthpass");
                    AtOManager.Instance.AddPlayerRequirementOthers("voidsouth");
                    AtOManager.Instance.AddPlayerRequirementOthers("voidsouthpass");
                    AtOManager.Instance.AddPlayerRequirementOthers("wardenquest");
                    AtOManager.Instance.AddPlayerRequirementOthers("waterwatchtower");
                    AtOManager.Instance.AddPlayerRequirementOthers("wolfstory");
                    AtOManager.Instance.AddPlayerRequirementOthers("ylmerseed");
                    AtOManager.Instance.AddPlayerRequirementOthers("youngharpy");
                    AtOManager.Instance.AddPlayerRequirementOthers("zonahielo");
                }
            }
        }
        /* superceded by Sandbox Mode
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventManager), "FinalResolution")]
        public static void FinalResolutionPrefix(ref bool ___groupWinner, ref bool[] ___charWinner, ref bool ___criticalSuccess, ref bool ___criticalFail, EventReplyData ___replySelected, ref EventManager __instance)
        {
            if (Plugin.IsHost() ? Plugin.medsAlwaysSucceed.Value : Plugin.medsMPAlwaysSucceed)
            {
                ___groupWinner = true;
                for (int index = 0; index < 4; ++index)
                    ___charWinner[index] = true;

                if (!((UnityEngine.Object)___replySelected.SscAddCard1 == (UnityEngine.Object)null) || !((UnityEngine.Object)___replySelected.SscAddCard2 == (UnityEngine.Object)null) || !((UnityEngine.Object)___replySelected.SscAddCard3 == (UnityEngine.Object)null))
                    ___criticalSuccess = true;
                ___criticalFail = false;
            }
            else if (Plugin.IsHost() ? Plugin.medsAlwaysFail.Value : Plugin.medsMPAlwaysFail)
            {
                ___groupWinner = false;
                for (int index = 0; index < 4; ++index)
                    ___charWinner[index] = false;
                if (!((UnityEngine.Object)___replySelected.SscAddCard1 == (UnityEngine.Object)null) || !((UnityEngine.Object)___replySelected.SscAddCard2 == (UnityEngine.Object)null) || !((UnityEngine.Object)___replySelected.SscAddCard3 == (UnityEngine.Object)null))
                    ___criticalFail = true;
                ___criticalSuccess = false;
            }
        }*/

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "FinalResolution")]
        public static void FinalResolutionPostfix(ref EventManager __instance)
        {
            if (Plugin.medsAutoContinue.Value)
            {
                bool medsStatusReady = Traverse.Create(__instance).Field("statusReady").GetValue<bool>();
                if (!medsStatusReady)
                    __instance.Ready(true);
            }
            if (Plugin.medsSpacebarContinue.Value)
                bFinalResolution = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "Start")]
        public static void EventManagerStartPostfix()
        {
            if (Plugin.medsSpacebarContinue.Value)
                bFinalResolution = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventManager), "Ready")]
        public static void EventManagerReady()
        {
            if (Plugin.medsSpacebarContinue.Value)
                bFinalResolution = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardCraftManager), "CanCraftThisCard")]
        public static void CanCraftThisCardPostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsCraftCorruptedCards.Value : Plugin.medsMPCraftCorruptedCards)
                __result = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardCraftManager), "SetMaxQuantity")]
        public static void SetMaxQuantityPrefix(ref int _maxQuantity)
        {
            if ((_maxQuantity >= 0) && (Plugin.IsHost() ? Plugin.medsInfiniteCardCraft.Value : Plugin.medsMPInfiniteCardCraft))
                _maxQuantity = -1;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardCraftManager), "GetCardAvailability")]
        public static void GetCardAvailabilityPostfix(ref int[] __result)
        {
            if (Plugin.IsHost() ? Plugin.medsInfiniteCardCraft.Value : Plugin.medsMPInfiniteCardCraft)
                __result[1] = 99;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "SaveBoughtItem")]
        public static void SaveBoughtItemPostfix()
        {
            if (Plugin.IsHost() ? Plugin.medsStockedShop.Value : Plugin.medsMPStockedShop)
            {
                AtOManager.Instance.boughtItems = (Dictionary<string, List<string>>)null;
                AtOManager.Instance.boughtItemInShopByWho = (Dictionary<string, int>)null;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "SaveBoughtItem")]
        public static bool SaveBoughtItemPrefix()
        {

            if (Plugin.IsHost() ? Plugin.medsSoloShop.Value : Plugin.medsMPSoloShop)
                return false;
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "NET_SaveBoughtItem")]
        public static bool NET_SaveBoughtItemPrefix()
        {
            if (Plugin.IsHost() ? Plugin.medsSoloShop.Value : Plugin.medsMPSoloShop)
                return false;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), "SetMenuCurrentProfile")]
        public static void SetMenuCurrentProfilePostfix()
        {
            MainMenuManager.Instance.profileMenuText.text += $" (Obeliskial)";
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "DoSteam")]
        public static bool DoSteamPrefix(ref SteamManager __instance)
        {
            uint releaseAppId = 1385380;
            try
            {
                SteamClient.Init(releaseAppId);
            }
            catch (System.Exception)
            {
                __instance.steamConnected = false;
            }
            if (!__instance.steamConnected)
                return false;
            if (SteamApps.IsSubscribedToApp((AppId)releaseAppId))
                GameManager.Instance.SetDemo(false);
            __instance.steamName = SteamClient.Name;
            __instance.steamId = SteamClient.SteamId;
            if (Plugin.medsDeveloperMode.Value)
                GameManager.Instance.SetDeveloperMode(true);
            __instance.GetDLCInformation();

            SteamFriends.OnGameRichPresenceJoinRequested += new Action<Friend, string>(SupportingActs.OnGameRichPresenceJoinRequested);
            SteamMatchmaking.OnLobbyCreated += new Action<Result, Lobby>(SupportingActs.OnLobbyCreated);
            SteamMatchmaking.OnLobbyMemberJoined += new Action<Lobby, Friend>(SupportingActs.OnLobbyMemberJoined);
            SteamFriends.OnGameLobbyJoinRequested += new Action<Lobby, SteamId>(SupportingActs.OnGameLobbyJoinRequested);
            SteamMatchmaking.OnLobbyEntered += new Action<Lobby>(SupportingActs.OnLobbyEntered);
            SteamMatchmaking.OnLobbyInvite += new Action<Friend, Lobby>(SupportingActs.OnLobbyInvite);
            SteamFriends.OnChatMessage += new Action<Friend, string, string>(SupportingActs.OnChatMessage);
            SteamApps.OnNewLaunchParameters += new Action(SupportingActs.OnNewLaunchParameters);
            SteamApps.GetLaunchParam("+connect_lobby");
            int num = -1;
            string s = "";
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            for (int index = 0; index < commandLineArgs.Length; ++index)
            {
                if (index == num)
                    s = commandLineArgs[index];
                else if (commandLineArgs[index] == "+connect_lobby")
                    num = index + index;
            }
            if (s != "")
            {
                SteamId lobbyId = (SteamId)ulong.Parse(s);
                try
                {
                    SteamMatchmaking.JoinLobbyAsync(lobbyId);
                }
                catch
                {
                    __instance.steamLoaded = false;
                }
            }
            else
                __instance.steamLoaded = true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), "GainSupply")]
        public static bool GainSupplyPrefix(ref PlayerManager __instance, ref int quantity)
        {
            __instance.SupplyActual += quantity;
            PlayerUIManager.Instance.SetSupply(true);
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), "Multiplayer")]
        public static void MultiplayerPostfix()
        {
            if (Plugin.medsStraya.Value)
                Plugin.SaveServerSelection();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), "JoinMultiplayer")]
        public static void JoinMultiplayerPostfix()
        {
            if (Plugin.medsStraya.Value)
                Plugin.SaveServerSelection();
        }


        // Modify Perks
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "CanModify")]
        public static void CanModifyPostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
                __result = true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPrefix()
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
                bSelectingPerk = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "CharInTown")]
        public static void CharInTownPostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GetTownTier")]
        public static void GetTownTierPostfix(ref int __result)
        {
            if (bSelectingPerk)
                __result = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SettingsManager), "IsActive")]
        public static void SettingsManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AlertManager), "IsActive")]
        public static void AlertManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MadnessManager), "IsActive")]
        public static void MadnessManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPrefix(ref PerkNode __instance)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
            {
                Traverse.Create(__instance).Field("nodeLocked").SetValue(false);
                __instance.iconLock.gameObject.SetActive(false);
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPrefix(ref PerkNode __instance)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
            {
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "Show")]
        public static void ShowPostfix(ref PerkTree __instance, ref int ___totalAvailablePoints)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
            {
                __instance.buttonReset.gameObject.SetActive(value: true);
                __instance.buttonImport.gameObject.SetActive(value: true);
                __instance.buttonExport.gameObject.SetActive(value: true);
                __instance.saveSlots.gameObject.SetActive(value: true);
                __instance.buttonConfirm.gameObject.SetActive(value: true);
                //__instance.buttonConfirm.Enable();
            }
            if (Plugin.IsHost() ? Plugin.medsPerkPoints.Value : Plugin.medsMPPerkPoints)
                ___totalAvailablePoints = 1000;
            return;
        }

        // 20230401 ModifyPerks fix?

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetIconLock")]
        public static void SetIconLockPrefix(ref bool _state)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
                _state = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetLocked")]
        public static void SetLockedPrefix(ref bool _status)
        {
            if (Plugin.IsHost() ? Plugin.medsModifyPerks.Value : Plugin.medsMPModifyPerks)
                _status = false;
        }

        /*
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkTree), "GetPointsAvailable")]
        public static bool GetPointsAvailablePrefix(ref int ___availablePoints)
        {
            if (Plugin.medsPerkPoints.Value)
                ___availablePoints = 500;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Functions), "GetCardByRarity")]
        public static void GetCardByRarityPostfix(ref string __result, CardData _cardData)
        {
            if (Plugin.medsCorruptGiovanna.Value)
                __result = _cardData?.UpgradesToRare?.Id ?? __result;
        }
        */


        [HarmonyPrefix]
        [HarmonyPatch(typeof(TownManager), "ShowButtons")]
        public static void ShowButtonsPrefix(out int __state)
        {
            __state = AtOManager.Instance.GetNgPlus(false);
            if (Plugin.IsHost() ? Plugin.medsUseClaimation.Value : Plugin.medsMPUseClaimation)
                AtOManager.Instance.SetNgPlus(0);

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TownManager), "ShowButtons")]
        public static void ShowButtonsPostfix(int __state)
        {
            if (Plugin.IsHost() ? Plugin.medsUseClaimation.Value : Plugin.medsMPUseClaimation)
                AtOManager.Instance.SetNgPlus(__state);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "GetCostReroll")]
        public static void GetCostRerollPostfix(ref int __result)
        {
            if (Plugin.IsHost() ? Plugin.medsDiscountDoomroll.Value : Plugin.medsMPDiscountDoomroll)
            {
                int num1;
                switch (AtOManager.Instance.GetTownTier())
                {
                    case 0:
                        num1 = 150;
                        break;
                    case 1:
                        num1 = 200;
                        break;
                    case 2:
                        num1 = 250;
                        break;
                    default:
                        num1 = 300;
                        break;
                }
                int num2 = 4;
                if (GameManager.Instance.IsMultiplayer())
                {
                    num2 = 0;
                    Hero[] team = AtOManager.Instance.GetTeam();
                    for (int index = 0; index < 4; ++index)
                    {
                        if (team[index].Owner == NetworkManager.Instance.GetPlayerNick())
                            ++num2;
                    }
                }
                int costReroll = num1 * num2;
                float num3 = 1f;
                if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_4"))
                    num3 -= 0.5f;
                float num4 = 1f;
                for (int index = 0; index < 4; ++index)
                    num4 -= (AtOManager.Instance.GetHero(index).GetItemDiscountModification() / 100f);
                __result = Functions.FuncRoundToInt((float)costReroll * num3 * num4);
                if (__result < 0)
                    __result = 0;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "GetDivinationCost")]
        public static void GetDivinationCostPostfix(ref Globals __instance, ref int __result, ref string divinationTier)
        {
            if (Plugin.IsHost() ? Plugin.medsDiscountDivination.Value : Plugin.medsMPDiscountDivination)
            {
                int divinationCost = 0;
                bool medsOk = false;
                switch (divinationTier)
                {
                    case "0":
                        divinationCost = 400;
                        medsOk = true;
                        break;
                    case "1":
                        divinationCost = 800;
                        medsOk = true;
                        break;
                    case "2":
                        divinationCost = 1600;
                        medsOk = true;
                        break;
                    case "3":
                        divinationCost = 3200;
                        medsOk = true;
                        break;
                    case "4":
                        divinationCost = 5000;
                        medsOk = true;
                        break;
                }
                float num1 = 1f;
                if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_5"))
                    num1 -= 0.4f;
                else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_3"))
                    num1 -= 0.25f;
                else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_1"))
                    num1 -= 0.1f;
                float num2 = 1f;
                for (int index = 0; index < 4; ++index)
                    num2 -= (AtOManager.Instance.GetHero(index).GetItemDiscountModification() / 100f);
                __result = Functions.FuncRoundToInt((float)divinationCost * num1 * num2);
                if ((__result < 0) && medsOk)
                    __result = 0;
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "IsTownRerollAvailable")]
        public static void IsTownRerollAvailablePostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsRavingRerolls.Value : Plugin.medsMPRavingRerolls)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SaveManager), "RestorePlayerData")]
        public static void RestorePlayerDataPostfix()
        {
            if (Plugin.medsJuiceSupplies.Value)
                PlayerManager.Instance.SupplyActual = UnityEngine.Random.Range(500, 999);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TownUpgradeWindow), "SetButtons")]
        public static void SetButtonsPostfix(ref TownUpgradeWindow __instance)
        {
            if (Plugin.IsHost() ? Plugin.medsSmallSanitySupplySelling.Value : Plugin.medsMPSmallSanitySupplySelling)
                __instance.sellSupplyButton.gameObject.SetActive(true);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardCraftManager), "GetCardAvailability")]
        public static void GetCardAvailabilityPostfix(ref int[] __result, string cardId)
        {
            if (Plugin.IsHost() ? Plugin.medsPlentifulPetPurchases.Value : Plugin.medsMPPlentifulPetPurchases)
            {
                CardData cardData1 = Globals.Instance.GetCardData(cardId, false);
                if (cardData1.CardUpgraded != Enums.CardUpgraded.No && cardData1.UpgradedFrom != "")
                    cardData1 = Globals.Instance.GetCardData(cardData1.UpgradedFrom.ToLower());
                if (cardData1.CardClass == Enums.CardClass.Item && cardData1.CardType == Enums.CardType.Pet)
                    __result[0] = 0;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LobbyManager), "InitLobby")]
        public static void InitLobbyPostfix(ref LobbyManager __instance)
        {
            if (Plugin.medsMaxMultiplayerMembers.Value)
                __instance.UICreatePlayers.value = 2;
        }

        /////////////////////////////////////////// 20230401 ///////////////////////////////////////////
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapManager), "CanTravelToThisNode")]
        public static void CanTravelToThisNodePostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                __result = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetRequired")]
        public static void SetRequiredPrefix(ref bool _status)
        {
            if (Plugin.IsHost() ? Plugin.medsNoPerkRequirements.Value : Plugin.medsMPNoPerkRequirements)
                _status = false;
        }

        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(NodeData), "VisibleIfNotRequirement")]
        public static void VisibleIfNotRequirementPostfix(ref bool __result)
        {
            if (Plugin.medsNoTravelRequirements.Value)
            {
                __result = true;
            }
        }*/

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "DrawNodes")]
        public static void DrawNodesPrefix(out List<string> __state)
        {
            __state = AtOManager.Instance.mapVisitedNodes;
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                AtOManager.Instance.mapVisitedNodes = new List<string>();
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapManager), "DrawNodes")]
        public static void DrawNodesPostfix(List<string> __state)
        {
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                AtOManager.Instance.mapVisitedNodes = __state;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "SetCurrentNode")]
        public static void SetCurrentNodePostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GenerateObeliskMap")]
        public static void GenerateObeliskMapPrefix(ref AtOManager __instance, out List<string> __state)
        {
            __state = __instance.mapVisitedNodes;
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                __instance.mapVisitedNodes = new List<string>();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GenerateObeliskMap")]
        public static void GenerateObeliskMapPostfix(ref AtOManager __instance, List<string> __state)
        {
            if (Plugin.IsHost() ? Plugin.medsTravelAnywhere.Value : Plugin.medsMPTravelAnywhere)
                __instance.mapVisitedNodes = __state;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIEnergySelector), "TurnOn")]
        public static void TurnOnPrefix(ref UIEnergySelector __instance, ref int maxToBeAssigned)
        {
            if (Plugin.IsHost() ? Plugin.medsOverlyTenergetic.Value : Plugin.medsMPOverlyTenergetic)
            {
                if (maxToBeAssigned == 0)
                    maxToBeAssigned = 100;
                Traverse.Create(__instance).Field("maxEnergy").SetValue(100);
                Traverse.Create(__instance).Field("maxEnergyToBeAssigned").SetValue(100);
                // int myvalue = int.Parse(Traverse.Create(__instance).Field("maxEnergy").GetValue() as string);
                // Plugin.Log.LogInfo("MYVAL");
                // Plugin.Log.LogInfo(myvalue);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "ModifyEnergy")]
        public static void ModifyEnergyPrefix(ref Character __instance, ref int _energy, out int __state)
        {
            __state = __instance.EnergyCurrent + _energy;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "ModifyEnergy")]
        public static void ModifyEnergyPostfix(ref Character __instance, int __state)
        {
            if (!(__instance.IsHero))
                return;
            if (__instance != null && __instance.IsHero && __state > 10 && (Plugin.IsHost() ? Plugin.medsOverlyTenergetic.Value : Plugin.medsMPOverlyTenergetic) && (UnityEngine.Object)__instance.HeroItem != (UnityEngine.Object)null && (UnityEngine.Object)__instance.HeroItem.energyTxt != (UnityEngine.Object)null)
            {
                //Plugin.Log.LogDebug(__instance.GameName);
                //Plugin.Log.LogDebug(__instance.IsHero);
                //Plugin.Log.LogDebug(__instance.Id);
                //Plugin.Log.LogDebug(__state.ToString());
                __instance.EnergyCurrent = __state;
                //Plugin.Log.LogDebug("Why are any of us here");
                __instance.HeroItem.energyTxt.text = __instance.EnergyCurrent.ToString();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardCraftManager), "ShowElements")]
        public static bool ShowElementsPrefix(ref CardCraftManager __instance, string cardId, string direction)
        {
            if (__instance.craftType == 1) // removing cards
            {
                CardData cardData = Globals.Instance.GetCardData(cardId, false);
                if ((UnityEngine.Object)cardData == (UnityEngine.Object)null)
                    return true;
                BotonGeneric medsButtonRemove = Traverse.Create(__instance).Field("BG_Remove").GetValue<BotonGeneric>();
                bool flag = true;
                if (direction == "")
                {
                    medsButtonRemove.gameObject.SetActive(false);
                    __instance.transformRemoveText.gameObject.SetActive(false);
                }
                else
                {
                    medsButtonRemove.gameObject.SetActive(true);
                    __instance.transformRemoveText.gameObject.SetActive(true);
                    if (!__instance.CanBuy("Remove"))
                        flag = false;
                    Hero medsHero = Traverse.Create(__instance).Field("currentHero").GetValue<Hero>();
                    if (medsHero.GetTotalCardsInDeck(true) <= (Plugin.IsHost() ? Plugin.medsDiminutiveDecks.Value : Plugin.medsMPDiminutiveDecks) && cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon)
                        flag = false;
                    switch (Plugin.IsHost() ? Plugin.medsDenyDiminishingDecks.Value : Plugin.medsMPDenyDiminishingDecks)
                    {
                        case "Cannot Remove Cards":
                            flag = false;
                            break;
                        case "Cannot Remove Curses":
                            if (cardData.CardClass == Enums.CardClass.Injury)
                                flag = false;
                            break;
                        case "Can Only Remove Curses":
                            if (cardData.CardClass != Enums.CardClass.Injury)
                                flag = false;
                            break;
                    }
                    if (flag)
                    {
                        medsButtonRemove.Enable();
                    }
                    else
                    {
                        medsButtonRemove.Disable();
                    }
                }
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NetworkManager), "LoadScene")]
        public static void LoadScenePrefix(ref string scene, ref NetworkManager __instance)
        {
            if (scene == "HeroSelection" && GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()) //multiplayer host, going into lobby
                Plugin.SendSettingsMP();
            else if (scene == "HeroSelection")
                Plugin.UpdateDropOnlyItems();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NetworkManager), "NET_LoadScene")]
        public static bool NET_LoadScenePrefix(ref string scene, ref int gameType)
        {
            if (gameType == 666666)
            {
                Plugin.SaveMPSettings(scene);
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "AddItemToHero")]
        public static void AddItemToHeroPrefix(ref string _cardName, ref AtOManager __instance, ref int _heroIndex, out int __state)
        {
            __state = 0;
            CardData cardData = Globals.Instance.GetCardData(_cardName, false);
            if ((UnityEngine.Object)cardData != (UnityEngine.Object)null)
            {
                Hero[] medsTeamAtO = __instance.GetTeam();
                Character character = (Character)medsTeamAtO[_heroIndex];
                __state = character.GetMaxHP();
                if (cardData.CardType == Enums.CardType.Weapon)
                {
                    // max hp bugfix
                    if (Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Weapon, false) != null)
                        __state -= Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Weapon, false).Item.MaxHealth;
                    // bad luck protection
                    Plugin.iShopsWithNoPurchase = 0;
                }
                else if (cardData.CardType == Enums.CardType.Armor)
                {
                    // max hp bugfix
                    if (Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Armor, false) != null)
                        __state -= Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Armor, false).Item.MaxHealth;
                    // bad luck protection
                    Plugin.iShopsWithNoPurchase = 0;
                }
                else if (cardData.CardType == Enums.CardType.Jewelry)
                {
                    // max hp bugfix
                    if (Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Jewelry, false) != null)
                        __state -= Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Jewelry, false).Item.MaxHealth;
                    // bad luck protection
                    Plugin.iShopsWithNoPurchase = 0;
                }
                else if (cardData.CardType == Enums.CardType.Accesory)
                {
                    // max hp bugfix
                    if (Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Accesory, false) != null)
                        __state -= Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Accesory, false).Item.MaxHealth;
                    // bad luck protection
                    Plugin.iShopsWithNoPurchase = 0;
                }
                else if (cardData.CardType == Enums.CardType.Pet)
                {
                    // max hp bugfix
                    if (Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Pet, false) != null)
                        __state -= Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Pet, false).Item.MaxHealth;
                    // don't count pets towards bad luck protection
                }
            }

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "AddItemToHero")]
        public static void AddItemToHeroPostfix(ref string _cardName, ref AtOManager __instance, ref int _heroIndex, int __state)
        {
            if (Plugin.IsHost() ? Plugin.medsBugfixEquipmentHP.Value : Plugin.medsMPBugfixEquipmentHP)
            {
                Hero[] medsTeamAtO = __instance.GetTeam();
                Character character = (Character)medsTeamAtO[_heroIndex];
                CardData cardD = Globals.Instance.GetCardData(_cardName, false);
                int medsMaxHP = __state;
                switch (cardD.CardType)
                {
                    case Enums.CardType.Weapon:
                        medsMaxHP += Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Weapon, false).Item.MaxHealth;
                        break;
                    case Enums.CardType.Armor:
                        medsMaxHP += Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Armor, false).Item.MaxHealth;
                        break;
                    case Enums.CardType.Jewelry:
                        medsMaxHP += Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Jewelry, false).Item.MaxHealth;
                        break;
                    case Enums.CardType.Accesory:
                        medsMaxHP += Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Accesory, false).Item.MaxHealth;
                        break;
                    case Enums.CardType.Pet:
                        medsMaxHP += Globals.Instance.GetCardData(medsTeamAtO[_heroIndex].Pet, false).Item.MaxHealth;
                        break;
                }
                if (medsMaxHP != character.GetMaxHP())
                    character.ModifyMaxHP(medsMaxHP - character.GetMaxHP());
            }
        }

        /*
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNodeData), "PerkRequired")]
        public static void PerkRequired(ref bool __result)
        {
            if (Plugin.medsNoPerkRequirements.Value)
            {
                __result = true;
            }
        }*/

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CinematicManager), "DoCinematic")]
        public static void DoCinematicPostfix(ref CinematicManager __instance)
        {
            if (Plugin.medsSkipCinematics.Value)
                __instance.SkipCinematic();
        }

        // NEW JUICE METHOD
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "GetPlayerGold")]
        public static void GetPlayerGoldPrefix(ref AtOManager __instance)
        {
            if (GameManager.Instance.IsMultiplayer() && Plugin.medsMPJuiceGold)
            {
                string medsplayerNick = NetworkManager.Instance.GetPlayerNick();
                Dictionary<string, int> medsmpPlayersGold = __instance.GetMpPlayersGold();
                foreach (var playerKey in medsmpPlayersGold.Keys.ToList())
                {
                    medsmpPlayersGold[playerKey] = UnityEngine.Random.Range(500000, 999999);
                    if (playerKey == medsplayerNick)
                        Traverse.Create(__instance).Field("playerGold").SetValue(medsmpPlayersGold[playerKey]);
                }
            }
            else if (!(GameManager.Instance.IsMultiplayer()) && Plugin.medsJuiceGold.Value)
            {
                Traverse.Create(__instance).Field("playerGold").SetValue(UnityEngine.Random.Range(500000, 999999));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "GetPlayerDust")]
        public static void GetPlayerDustPrefix(ref AtOManager __instance)
        {
            if (GameManager.Instance.IsMultiplayer() && Plugin.medsMPJuiceDust)
            {
                string medsplayerNick = NetworkManager.Instance.GetPlayerNick();
                Dictionary<string, int> medsmpPlayersDust = __instance.GetMpPlayersDust();
                foreach (var playerKey in medsmpPlayersDust.Keys.ToList())
                {
                    medsmpPlayersDust[playerKey] = UnityEngine.Random.Range(500000, 999999);
                    if (playerKey == medsplayerNick)
                        Traverse.Create(__instance).Field("playerDust").SetValue(medsmpPlayersDust[playerKey]);
                }
            }
            else if (!(GameManager.Instance.IsMultiplayer()) && Plugin.medsJuiceDust.Value)
            {
                Traverse.Create(__instance).Field("playerDust").SetValue(UnityEngine.Random.Range(500000, 999999));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), "GetPlayerSupplyActual")]
        public static void GetPlayerSupplyActualPrefix(ref PlayerManager __instance)
        {
            if (Plugin.medsJuiceSupplies.Value)
                Traverse.Create(__instance).Field("supplyActual").SetValue(UnityEngine.Random.Range(500, 999));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerUIManager), "SetGold")]
        public static void SetGoldPrefix(ref bool animation)
        {
            if (Plugin.IsHost() ? Plugin.medsJuiceGold.Value : Plugin.medsMPJuiceGold)
                animation = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerUIManager), "SetDust")]
        public static void SetDustPrefix(ref bool animation)
        {
            if (Plugin.IsHost() ? Plugin.medsJuiceDust.Value : Plugin.medsMPJuiceDust)
                animation = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerUIManager), "SetSupply")]
        public static void SetSupplyPrefix(ref bool animation)
        {
            if (Plugin.medsJuiceSupplies.Value)
                animation = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LobbyManager), "ShowCreate")]
        public static void ShowCreatePostfix(ref LobbyManager __instance)
        {
            if (Plugin.medsMPLoadAutoCreateRoom.Value && GameManager.Instance.GameStatus == Enums.GameStatus.LoadGame)
                __instance.CreateRoom();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeroSelectionManager), "ShowFollowStatus")]
        public static void ShowFollowStatusPostfix(ref HeroSelectionManager __instance)
        {
            if (Plugin.medsMPLoadAutoReady.Value && GameManager.Instance.GameStatus == Enums.GameStatus.LoadGame && GameManager.Instance.IsMultiplayer())
            {
                Coroutine medsmanualReadyCo = Traverse.Create(__instance).Field("manualReadyCo").GetValue<Coroutine>();
                if (medsmanualReadyCo != null)
                    __instance.StopCoroutine(medsmanualReadyCo);
                Traverse.Create(__instance).Field("statusReady").SetValue(true);
                NetworkManager.Instance.SetManualReady(true);
                __instance.ReadySetButton(true);
            }
        }

        //        [HarmonyPostfix]
        //        [HarmonyPatch(typeof(AtOManager), "ClearGame")]
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(GameManager), "GenerateHeroes")]
        //public static void GenerateHeroesPostfix()
        //{

        //}

        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "CreateHero")]
        public static void CreateHeroPostfix(string subClass, ref Hero __result)
        
            __result.GameName = 
        }*/

        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "CreateCharClones")]
        public static void CreateCharClonesPostfix()
        {

        }*/

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), "IsHeroUnlocked")]
        public static void IsHeroUnlockedPrefix(ref string subclass)
        {
            if (subclass == "medsdlctwo")
                subclass = (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo);
            else if (subclass == "medsdlcthree")
                subclass = (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree);
            else if (subclass == "medsdlcfour")
                subclass = (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour);
            if (subclass == "medscustomone")
                subclass = "mercenary"; // always unlocked
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonSkin), "OnMouseUp")]
        public static bool BotonSkinOnMouseUpPrefix(ref BotonSkin __instance)
        {
            // This isn't strictly necessary, given skins are cloned, but I suspect I'll use it for custom characters/skins later?
            // Basically, when clicking the button to change skins, this code uses the subclass id attached to the character popup rather than the subclass id attached to the skin.
            string medsSubClassId = HeroSelectionManager.Instance.charPopup.GetActive();
            if (!(medsSubClassId == "medsdlctwo" || medsSubClassId == "medsdlcthree" || medsSubClassId == "medsdlcfour"))
                return true;
            bool medsLocked = Traverse.Create(__instance).Field("locked").GetValue<bool>();
            if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || medsLocked)
                return false;
            SkinData medsSkinData = Traverse.Create(__instance).Field("skinData").GetValue<SkinData>();
            PlayerManager.Instance.SetSkin(medsSubClassId, medsSkinData.SkinId);
            HeroSelectionManager.Instance.SetSkinIntoSubclassData(medsSubClassId, medsSkinData.SkinId);
            HeroSelectionManager.Instance.charPopup.DoSkins();
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), "GetProgress")]
        public static void GetProgressPrefix(ref string _subclassId)
        {
            if (_subclassId == "medsdlctwo")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo);
            else if (_subclassId == "medsdlcthree")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree);
            else if (_subclassId == "medsdlcfour")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "Awake")]
        public static void GlobalsAwakePostfix()
        {
            if (Plugin.medsOver50s.Value)
            {
                List<int> medsPerkLevel = Globals.Instance.PerkLevel;

                for (int a = 1; a <= 950; a++)
                {
                    Globals.Instance.PerkLevel.Add(Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1] + 4000 + a * 100);
                }
                Traverse.Create(Globals.Instance).Field("_PerkLevel").SetValue(medsPerkLevel);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), "ModifyProgress")]
        public static void ModifyProgressPrefix(ref string _subclassId)
        {
            if (_subclassId == "medsdlctwo")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneTwo.Value : Plugin.medsMPDLCCloneTwo);
            else if (_subclassId == "medsdlcthree")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneThree.Value : Plugin.medsMPDLCCloneThree);
            else if (_subclassId == "medsdlcfour")
                _subclassId = (Plugin.IsHost() ? Plugin.medsDLCCloneFour.Value : Plugin.medsMPDLCCloneFour);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "NodeScore")]
        public static void NodeScorePrefix()
        {
            Hero[] medsTeamAtO = Traverse.Create(AtOManager.Instance).Field("teamAtO").GetValue<Hero[]>();
            int medsMapVisitedNodesTMP = Traverse.Create(AtOManager.Instance).Field("mapVisitedNodesTMP").GetValue<int>();
            List<string> medsMapVisitedNodes = Traverse.Create(AtOManager.Instance).Field("mapVisitedNodes").GetValue<List<string>>();
            int medsCombatExpertise = Traverse.Create(AtOManager.Instance).Field("combatExpertise").GetValue<int>();
            int medsCombatExpertiseTMP = Traverse.Create(AtOManager.Instance).Field("combatExpertiseTMP").GetValue<int>();
            int medsExperienceGainedTMP = Traverse.Create(AtOManager.Instance).Field("experienceGainedTMP").GetValue<int>();
            int medsTotalDeathsTMP = Traverse.Create(AtOManager.Instance).Field("totalDeathsTMP").GetValue<int>();
            int medsBossesKilled = Traverse.Create(AtOManager.Instance).Field("bossesKilled").GetValue<int>();
            int medsBossesKilledTMP = Traverse.Create(AtOManager.Instance).Field("bossesKilledTMP").GetValue<int>();
            int medsCorruptionCommonCompleted = Traverse.Create(AtOManager.Instance).Field("corruptionCommonCompleted").GetValue<int>();
            int medsCorruptionCommonCompletedTMP = Traverse.Create(AtOManager.Instance).Field("corruptionCommonCompletedTMP").GetValue<int>();
            int medsCorruptionUncommonCompleted = Traverse.Create(AtOManager.Instance).Field("corruptionUncommonCompleted").GetValue<int>();
            int medsCorruptionUncommonCompletedTMP = Traverse.Create(AtOManager.Instance).Field("corruptionUncommonCompletedTMP").GetValue<int>();
            int medsCorruptionRareCompleted = Traverse.Create(AtOManager.Instance).Field("corruptionRareCompleted").GetValue<int>();
            int medsCorruptionRareCompletedTMP = Traverse.Create(AtOManager.Instance).Field("corruptionRareCompletedTMP").GetValue<int>();
            int medsCorruptionEpicCompleted = Traverse.Create(AtOManager.Instance).Field("corruptionEpicCompleted").GetValue<int>();
            int medsCorruptionEpicCompletedTMP = Traverse.Create(AtOManager.Instance).Field("corruptionEpicCompletedTMP").GetValue<int>();

            if (medsTeamAtO == null)
                return;
            bool flag = medsMapVisitedNodesTMP == 0;
            int num1 = 0;
            for (int index = 0; index < medsMapVisitedNodes.Count; ++index)
            {
                if ((UnityEngine.Object)Globals.Instance.GetNodeData(medsMapVisitedNodes[index]) != (UnityEngine.Object)null && (UnityEngine.Object)Globals.Instance.GetNodeData(medsMapVisitedNodes[index]).NodeZone != (UnityEngine.Object)null && !Globals.Instance.GetNodeData(medsMapVisitedNodes[index]).NodeZone.DisableExperienceOnThisZone)
                    ++num1;
            }
            int num2 = num1 - medsMapVisitedNodesTMP;
            if (!GameManager.Instance.IsObeliskChallenge())
            {
                if (num1 < 2)
                {
                    medsMapVisitedNodesTMP = 0;
                    num2 = 0;
                }
                else
                {
                    if (medsMapVisitedNodesTMP == 0)
                        num2 -= 2;
                    medsMapVisitedNodesTMP = num1;
                }
            }
            else if (num1 < 1)
            {
                medsMapVisitedNodesTMP = 0;
                num2 = 0;
            }
            else
            {
                if (medsMapVisitedNodesTMP == 0)
                    --num2;
                medsMapVisitedNodesTMP = num1;
            }
            int num3 = num2 * 36;
            int num4 = medsCombatExpertise - medsCombatExpertiseTMP;
            medsCombatExpertiseTMP = medsCombatExpertise;
            int num5 = num4;
            if (num5 < 0)
                num5 = 0;
            int num6 = num5 * 13;
            int num7 = 0;
            int num8 = 0;
            if (medsTeamAtO != null)
            {
                for (int index = 0; index < medsTeamAtO.Length; ++index)
                {
                    num7 += medsTeamAtO[index].Experience;
                    num8 += medsTeamAtO[index].TotalDeaths;
                }
            }
            int num9 = num7 - medsExperienceGainedTMP;
            medsExperienceGainedTMP = num7;
            int num10 = Functions.FuncRoundToInt((float)num9 * 0.5f);
            int num11 = num8 - medsTotalDeathsTMP;
            medsTotalDeathsTMP = num8;
            int num12 = -num11 * 100;
            int num13 = medsBossesKilled - medsBossesKilledTMP;
            medsBossesKilledTMP = medsBossesKilled;
            int num14 = num13 * 80;
            int num15 = medsCorruptionCommonCompleted - medsCorruptionCommonCompletedTMP;
            medsCorruptionCommonCompletedTMP = medsCorruptionCommonCompleted;
            int num16 = medsCorruptionUncommonCompleted - medsCorruptionUncommonCompletedTMP;
            medsCorruptionUncommonCompletedTMP = medsCorruptionUncommonCompleted;
            int num17 = medsCorruptionRareCompleted - medsCorruptionRareCompletedTMP;
            medsCorruptionRareCompletedTMP = medsCorruptionRareCompleted;
            int num18 = medsCorruptionEpicCompleted - medsCorruptionEpicCompletedTMP;
            medsCorruptionEpicCompletedTMP = medsCorruptionEpicCompleted;
            int num19 = num15 * 40 + num16 * 80 + num17 * 130 + num18 * 200;
            int num20 = num3 + num6 + num12 + num10 + num14 + num19;
            Plugin.Log.LogDebug("num1: " + num1);
            Plugin.Log.LogDebug("num2: " + num2);
            Plugin.Log.LogDebug("num3: " + num3);
            Plugin.Log.LogDebug("num4: " + num4);
            Plugin.Log.LogDebug("num5: " + num5);
            Plugin.Log.LogDebug("num6: " + num6);
            Plugin.Log.LogDebug("num7: " + num7);
            Plugin.Log.LogDebug("num8: " + num8);
            Plugin.Log.LogDebug("num9: " + num9);
            Plugin.Log.LogDebug("num10: " + num10);
            Plugin.Log.LogDebug("num11: " + num11);
            Plugin.Log.LogDebug("num12: " + num12);
            Plugin.Log.LogDebug("num13: " + num13);
            Plugin.Log.LogDebug("num14: " + num14);
            Plugin.Log.LogDebug("num15: " + num15);
            Plugin.Log.LogDebug("num16: " + num16);
            Plugin.Log.LogDebug("num17: " + num17);
            Plugin.Log.LogDebug("num18: " + num18);
            Plugin.Log.LogDebug("num19: " + num19);
            Plugin.Log.LogDebug("num20: " + num20);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "CalculateScore")]
        public static void CalculateScorePrefix(bool _calculateMadnessMultiplier, int _auxValue)
        {
            int medsTotalScoreTMP = Traverse.Create(AtOManager.Instance).Field("totalScoreTMP").GetValue<int>();
            Plugin.Log.LogDebug("_CMM: " + _calculateMadnessMultiplier);
            Plugin.Log.LogDebug("_aux: " + _auxValue);
            Plugin.Log.LogDebug("totalScoreTMP: " + medsTotalScoreTMP);
            medsTotalScoreTMP += Functions.FuncRoundToInt((float)(medsTotalScoreTMP * Functions.GetMadnessScoreMultiplier(AtOManager.Instance.GetMadnessDifficulty(), !GameManager.Instance.IsObeliskChallenge()) / 100));
            Plugin.Log.LogDebug("score: " + medsTotalScoreTMP);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ConflictManager), "EnableButtonsForPlayerChoosing")]
        public static void EnableButtonsForPlayerChoosingPostfix(ref ConflictManager __instance)
        {
            int medsMethod = Plugin.IsHost() ? Plugin.medsConflictResolution.Value : Plugin.medsMPConflictResolution;
            Plugin.Log.LogDebug("medsMethod: " + medsMethod);
            switch (medsMethod)
            {
                case 1:
                    Plugin.Log.LogDebug("pressing button 0");
                    MapManager.Instance.ConflictSelection(0);
                    break;
                case 2:
                    Plugin.Log.LogDebug("pressing button 1");
                    MapManager.Instance.ConflictSelection(1);
                    break;
                case 3:
                    Plugin.Log.LogDebug("pressing button 2");
                    MapManager.Instance.ConflictSelection(2);
                    break;
                case 4:
                    int medsRandom = UnityEngine.Random.Range(0, 3);
                    Plugin.Log.LogDebug("pressing button " + medsRandom);
                    MapManager.Instance.ConflictSelection(medsRandom);
                    break;
            }
        }

        /* not needed with new Visit All Zones method
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "AddPlayerRequirement")]
        public static bool AddPlayerRequirementPrefix(ref AtOManager __instance, EventRequirementData requirement)
        {
            if (requirement.RequirementId == "_tier2" && (Plugin.IsHost() ? Plugin.medsVisitAllZones.Value : Plugin.medsMPVisitAllZones))
            {
                List<string> medsPlayerRequirements = Traverse.Create(__instance).Field("playerRequeriments").GetValue<List<string>>();
                if (!medsPlayerRequirements.Contains("_tier1b"))
                {
                    medsPlayerRequirements.Add("_tier1b");
                    return false;
                }
            }
            return true;
        }*/

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "OnMouseUpController")]
        public static bool OnMouseUpControllerPrefix(ref CardItem __instance)
        {
            if (!Plugin.medsEmotional.Value)
                return true;
            if ((bool)(UnityEngine.Object)MatchManager.Instance && !MatchManager.Instance.IsYourTurn())
            {
                Plugin.Log.LogDebug("onmouseup, considered not my turn");
                if (__instance.cardfordiscard || __instance.cardforaddcard)
                {

                    MatchManager.Instance.SendEmoteCard(100 + int.Parse(__instance.name.Replace("TMP_", "")));
                }
                else
                {
                    MatchManager.Instance.SendEmoteCard(__instance.tablePosition);
                }
                return false;
            }
            else if (__instance.cardoutsideloot && (UnityEngine.Object)LootManager.Instance != (UnityEngine.Object)null && !LootManager.Instance.IsMyLoot)
            {

                /*return false;*/
            }
            else if (__instance.cardoutsidereward && (UnityEngine.Object)RewardsManager.Instance != (UnityEngine.Object)null)
            {
                /*if (__instance.disableT.gameObject.activeSelf) // if 'greyed out' on rewards screen
                {
                    string[] splitName = __instance.name.Split("_");
                    if (splitName.Length > 2) {
                        PhotonView medsPhotonView = Traverse.Create(RewardsManager.Instance).Field("photonView").GetValue<PhotonView>();
                        int index = int.Parse(splitName[splitName.Length - 2]) + 1000;
                        Hero[] team = AtOManager.Instance.GetTeam();
                        for (int heroInt = 0; heroInt < 4; ++heroInt)
                        {
                            if (team[heroInt].Owner == NetworkManager.Instance.GetPlayerNick())
                            {
                                medsPhotonView.RPC("NET_CardSelected", RpcTarget.All, (object)(short)index, (object)__instance.name + "|" + team[heroInt].SubclassName);
                                return false;
                            }
                        }
                    }
                }*/
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "OnMouseUp")]
        public static bool OnMouseUpPrefix(ref CardItem __instance)
        {
            if (!Plugin.medsEmotional.Value)
                return true;
            if ((bool)(UnityEngine.Object)MatchManager.Instance && !MatchManager.Instance.IsYourTurn())
            {
                Plugin.Log.LogDebug("onmouseup, considered not my turn");
                if (__instance.cardfordiscard || __instance.cardforaddcard)
                {

                    MatchManager.Instance.SendEmoteCard(100 + int.Parse(__instance.name.Replace("TMP_", "")));
                }
                else
                {
                    MatchManager.Instance.SendEmoteCard(__instance.tablePosition);
                }
                return false;
            }
            else if (__instance.cardoutsideloot && (UnityEngine.Object)LootManager.Instance != (UnityEngine.Object)null && !LootManager.Instance.IsMyLoot)
            {

                /*return false;*/
            }
            else if (__instance.cardoutsidereward && (UnityEngine.Object)RewardsManager.Instance != (UnityEngine.Object)null)
            {
                /*if (__instance.disableT.gameObject.activeSelf) // if 'greyed out' on rewards screen
                {
                    string[] splitName = __instance.name.Split("_");
                    if (splitName.Length > 2) {
                        PhotonView medsPhotonView = Traverse.Create(RewardsManager.Instance).Field("photonView").GetValue<PhotonView>();
                        int index = int.Parse(splitName[splitName.Length - 2]) + 1000;
                        Hero[] team = AtOManager.Instance.GetTeam();
                        for (int heroInt = 0; heroInt < 4; ++heroInt)
                        {
                            if (team[heroInt].Owner == NetworkManager.Instance.GetPlayerNick())
                            {
                                medsPhotonView.RPC("NET_CardSelected", RpcTarget.All, (object)(short)index, (object)__instance.name + "|" + team[heroInt].SubclassName);
                                return false;
                            }
                        }
                    }
                }*/
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(RewardsManager), "NET_CardSelected")]
        public static bool NET_CardSelectedPrefix(ref RewardsManager __instance, short _index, string cardId)
        {
            /*int index = _index;
            if (index >= 1000)
            {
                index -= 1000;
                if (cardId.Split("|").Length < 2 || __instance.characterRewardArray[index] != null && __instance.characterRewardArray[index].GetComponent<CharacterReward>() != null && __instance.characterRewardArray[index].GetComponent<CharacterReward>().cardsByInternalId[cardId.Split("|")[0]] != null)
                {
                    CardItem medsCI = __instance.characterRewardArray[index].GetComponent<CharacterReward>().cardsByInternalId[cardId.Split("|")[0]];
                    SubClassData medsSCD = Globals.Instance.GetSubClassData(cardId.Split("|")[1]);
                    if (medsSCD == null)
                        return false;
                    Sprite stickerBase = medsSCD.StickerBase;
                    if ((UnityEngine.Object)medsCI.emote0.sprite == (UnityEngine.Object)stickerBase)
                    {
                        medsCI.emote0.sprite = (Sprite)null;
                        medsCI.emote0.gameObject.SetActive(false);
                    }
                    else if ((UnityEngine.Object)medsCI.emote1.sprite == (UnityEngine.Object)stickerBase)
                    {
                        medsCI.emote1.sprite = (Sprite)null;
                        medsCI.emote1.gameObject.SetActive(false);
                    }
                    else if ((UnityEngine.Object)medsCI.emote2.sprite == (UnityEngine.Object)stickerBase)
                    {
                        medsCI.emote2.sprite = (Sprite)null;
                        medsCI.emote2.gameObject.SetActive(false);
                    }
                    else if ((UnityEngine.Object)medsCI.emote0.sprite == (UnityEngine.Object)null)
                    {
                        medsCI.emote0.sprite = stickerBase;
                        medsCI.emote0.gameObject.SetActive(true);
                        medsCI.emote0.sortingOrder = 20100 + index * 10 + 1;
                    }
                    else if ((UnityEngine.Object)medsCI.emote1.sprite == (UnityEngine.Object)null)
                    {
                        medsCI.emote1.sprite = stickerBase;
                        medsCI.emote1.gameObject.SetActive(true);
                        medsCI.emote1.sortingOrder = 20100 + index * 10 + 2;
                    }
                    else if ((UnityEngine.Object)medsCI.emote2.sprite == (UnityEngine.Object)null)
                    {
                        medsCI.emote2.sprite = stickerBase;
                        medsCI.emote2.gameObject.SetActive(true);
                        medsCI.emote2.sortingOrder = 20100 + index * 10 + 3;
                    }
                    return false;
                }
            }*/
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "SendEmoteCard")]
        public static bool SendEmoteCardPrefix(ref MatchManager __instance, int tablePosition)
        {
            Plugin.Log.LogDebug("SendEmoteCard commenced!\nheroActive: " + __instance.GetHeroActive() + "\ntablePosition: " + tablePosition + "\nemoteHeroActive: " + __instance.emoteManager.heroActive);
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "DoEmoteCard")]
        public static bool DoEmoteCardPrefix(ref MatchManager __instance, byte _tablePosition, byte _heroIndex)
        {
            if (Plugin.medsEmotional.Value)
            {
                Plugin.Log.LogDebug("DoEmoteCard commenced!\n_tablePosition: " + _tablePosition + "\n_heroIndex: " + _heroIndex);
                int index1 = (int)_tablePosition;
                if (index1 >= 100)
                {
                    index1 -= 100;
                    Dictionary<string, GameObject> medsCardGos = Traverse.Create(__instance).Field("cardGos").GetValue<Dictionary<string, GameObject>>();
                    if (medsCardGos == null || medsCardGos.Keys.Count <= index1 || !(medsCardGos.ContainsKey("TMP_" + index1)) || medsCardGos["TMP_" + index1].GetComponent<CardItem>() == null)
                        return false;
                    if (medsCardGos["TMP_" + index1].GetComponent<CardItem>().HaveEmoteIcon(_heroIndex))
                    {
                        Plugin.Log.LogDebug("removing cardgo");
                        // remove emote
                        medsCardGos["TMP_" + index1].GetComponent<CardItem>().RemoveEmoteIcon(_heroIndex);
                    }
                    else
                    {
                        medsCardGos["TMP_" + index1].GetComponent<CardItem>().ShowEmoteIcon(_heroIndex);
                        if (!__instance.IsYourTurn())
                            return false;
                        GameManager.Instance.PlayLibraryAudio("Pop6", 2.9f);
                    }
                }
                else
                {
                    List<CardItem> medsCardItemTable = Traverse.Create(__instance).Field("cardItemTable").GetValue<List<CardItem>>();
                    if (medsCardItemTable == null)
                        return true;
                    Plugin.Log.LogDebug("passed carditemtable");
                    // player clicks on a card that has sticker
                    Plugin.Log.LogDebug("count: " + medsCardItemTable.Count);
                    if (medsCardItemTable.Count <= index1 || !((UnityEngine.Object)medsCardItemTable[index1] != (UnityEngine.Object)null))
                        return false;
                    if (medsCardItemTable[index1].HaveEmoteIcon(_heroIndex))
                    {
                        Plugin.Log.LogDebug("removing");
                        // remove emote
                        medsCardItemTable[index1].RemoveEmoteIcon(_heroIndex);
                    }
                    else // clicks on a card that does not already have sticker
                    {
                        /* let's put more stickers in :D
                        for (int index2 = 0; index2 < medsCardItemTable.Count; ++index2)
                        {
                            if (index2 != (int)_tablePosition && (UnityEngine.Object)medsCardItemTable[index2] != (UnityEngine.Object)null)
                                medsCardItemTable[index2].RemoveEmoteIcon(_heroIndex);
                        }*/

                        Plugin.Log.LogDebug("unfalse, showing " + _heroIndex);

                        medsCardItemTable[index1].ShowEmoteIcon(_heroIndex);
                        Plugin.Log.LogDebug("ifnotmyturn");
                        if (!__instance.IsYourTurn())
                            return false;
                        GameManager.Instance.PlayLibraryAudio("Pop6", 2.9f);
                    }
                }
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "ShowEmoteIcon")]
        public static bool ShowEmoteIconPrefix(ref CardItem __instance, byte _heroIndex)
        {
            if (!Plugin.medsEmotional.Value)
                return true;

            Plugin.Log.LogDebug("ShowEmoteIcon commenced!\n_heroIndex: " + _heroIndex);
            __instance.ShowEmoteTransform();
            SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[_heroIndex]);
            Plugin.Log.LogDebug("gotsubclass");
            if ((UnityEngine.Object)medsSCD == (UnityEngine.Object)null)
                return false;
            Plugin.Log.LogDebug("subclass id: " + medsSCD.Id);
            Sprite stickerBase = medsSCD.StickerBase;
            Plugin.Log.LogDebug("stickerBase name: " + stickerBase.name);
            if ((UnityEngine.Object)__instance.emote0.sprite == (UnityEngine.Object)stickerBase || (UnityEngine.Object)__instance.emote1.sprite == (UnityEngine.Object)stickerBase || (UnityEngine.Object)__instance.emote2.sprite == (UnityEngine.Object)stickerBase)
                return false;
            Plugin.Log.LogDebug("1875");
            if (__instance.cardfordiscard || __instance.cardforaddcard)
            {
                int medsTMP_ = int.Parse(__instance.name.Replace("TMP_", ""));
                if (medsTMP_ >= 0)
                {
                    __instance.emote0.sortingOrder = 20100 + medsTMP_ * 10 + 1;
                    __instance.emote1.sortingOrder = 20100 + medsTMP_ * 10 + 2;
                    __instance.emote2.sortingOrder = 20100 + medsTMP_ * 10 + 3;
                }
            }
            if ((UnityEngine.Object)__instance.emote0.sprite == (UnityEngine.Object)null)
            {
                Plugin.Log.LogDebug("emote0");
                __instance.emote0.sprite = stickerBase;

                Plugin.Log.LogDebug("emote0 sprite name: " + __instance.emote0.sprite.name);
                __instance.emote0.gameObject.SetActive(true);
            }
            else if ((UnityEngine.Object)__instance.emote1.sprite == (UnityEngine.Object)null)
            {
                Plugin.Log.LogDebug("emote1");
                __instance.emote1.sprite = stickerBase;
                __instance.emote1.gameObject.SetActive(true);
            }
            else
            {
                if (!((UnityEngine.Object)__instance.emote2.sprite == (UnityEngine.Object)null))
                    return false;
                Plugin.Log.LogDebug("emote2");
                __instance.emote2.sprite = stickerBase;
                __instance.emote2.gameObject.SetActive(true);
            }
            Plugin.Log.LogDebug("finished");
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "RemoveEmoteIcon")]
        public static bool RemoveEmoteIconPrefix(ref CardItem __instance, byte _heroIndex)
        {
            if (!Plugin.medsEmotional.Value)
                return true;
            Plugin.Log.LogDebug("RemoveEmoteIcon commenced!\n_heroIndex: " + _heroIndex);
            SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[(int)_heroIndex]);
            if (medsSCD == null)
                return false;
            Sprite stickerBase = medsSCD.StickerBase;
            if ((UnityEngine.Object)__instance.emote0.sprite == (UnityEngine.Object)stickerBase)
            {
                __instance.emote0.sprite = (Sprite)null;
                __instance.emote0.gameObject.SetActive(false);
            }
            else if ((UnityEngine.Object)__instance.emote1.sprite == (UnityEngine.Object)stickerBase)
            {
                __instance.emote1.sprite = (Sprite)null;
                __instance.emote1.gameObject.SetActive(false);
            }
            else if ((UnityEngine.Object)__instance.emote2.sprite == (UnityEngine.Object)stickerBase)
            {
                __instance.emote2.sprite = (Sprite)null;
                __instance.emote2.gameObject.SetActive(false);
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "SetCharactersPing")]
        public static bool SetCharactersPingPrefix(ref MatchManager __instance, int _action)
        {
            if (!Plugin.medsEmotional.Value)
                return true;
            if (__instance.waitingDeathScreen || __instance.WaitingForActionScreen() || __instance.emoteManager.IsBlocked() || !__instance.emoteManager.gameObject.activeSelf)
                return false;
            __instance.emoteManager.HideEmotes();
            if (__instance.emoteManager.EmoteNeedsTarget(_action))
            {
                __instance.ShowCharactersPing(_action);
            }
            else
            {
                if (__instance.emoteManager.heroActive <= -1 || __instance.emoteManager.heroActive >= Plugin.medsSubclassList.Length || Plugin.medsSubclassList[__instance.emoteManager.heroActive] == "")
                    return false;
                __instance.EmoteTarget(Plugin.medsSubclassList[__instance.emoteManager.heroActive], _action);
            }
            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "HaveEmoteIcon")]
        public static bool HaveEmoteIconPrefix(ref CardItem __instance, byte _heroIndex, ref bool __result)
        {
            if (!Plugin.medsEmotional.Value)
                return true;
            __result = false;
            SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[(int)_heroIndex]);
            if (medsSCD == null)
                return false;
            Sprite stickerBase = medsSCD.StickerBase;
            if ((UnityEngine.Object)__instance.emote0.sprite == (UnityEngine.Object)stickerBase || (UnityEngine.Object)__instance.emote1.sprite == (UnityEngine.Object)stickerBase || (UnityEngine.Object)__instance.emote2.sprite == (UnityEngine.Object)stickerBase)
                __result = true;
            return false;
        }



        [HarmonyPostfix]
        [HarmonyPatch(typeof(EmoteManager), "Awake")]
        public static void AwakePostfix(ref EmoteManager __instance)
        {
            if (Plugin.medsEmotional.Value)
            {
                medsPosIni = __instance.characterPortrait.transform.localPosition;
                medsPosIniBlocked = __instance.characterPortraitBlocked.transform.parent.transform.localPosition;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteManager), "Init")]
        public static void InitPrefix(ref EmoteManager __instance)
        {
            if (Plugin.medsEmotional.Value && GameManager.Instance.IsMultiplayer())
            {
                List<string> myHeroIDs = new();
                foreach (Hero medsHero in MatchManager.Instance.GetTeamHero())
                {
                    Plugin.Log.LogDebug("subclassname: " + medsHero.SubclassName);
                    if ((medsHero.Owner == NetworkManager.Instance.GetPlayerNick() || medsHero.Owner == "") && !myHeroIDs.Contains(medsHero.SubclassName))
                        myHeroIDs.Add(medsHero.SubclassName);
                }
                for (var a = 0; a < Plugin.medsSubclassList.Length; a++)
                {
                    if (myHeroIDs.Contains(Plugin.medsSubclassList[a]) && __instance.heroActive == -1)
                        __instance.heroActive = a - 1;
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteManager), "SelectNextCharacter")]
        public static bool SelectNextCharacterPrefix(ref EmoteManager __instance)
        {
            if (Plugin.medsEmotional.Value && GameManager.Instance.IsMultiplayer())
            {
                ++__instance.heroActive;
                if (__instance.heroActive >= Plugin.medsSubclassList.Length)
                    __instance.heroActive = 0;
                SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[__instance.heroActive]);
                if (medsSCD != null)
                {
                    __instance.characterPortrait.sprite = __instance.characterPortraitBlocked.sprite = medsSCD.StickerBase;
                    __instance.characterPortrait.transform.localPosition = medsPosIni + new Vector3(medsSCD.StickerOffsetX, 0f, 0f);
                    __instance.characterPortraitBlocked.transform.parent.transform.localPosition = medsPosIniBlocked + new Vector3(medsSCD.StickerOffsetX, 0f, 0f);
                }
                for (int i = 0; i < __instance.emotes.Length; i++)
                    __instance.emotes[i].SetBlocked(_state: false);
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteTarget), "SetIcons")]
        public static bool SetIconsPrefix(ref EmoteTarget __instance, int _heroIndex, int _action)
        {

            if (Plugin.medsEmotional.Value && GameManager.Instance.IsMultiplayer())
            {
                if (!(MatchManager.Instance != null))
                    return false;

                SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[_heroIndex]);
                //Hero hero = MatchManager.Instance.GetHero(_heroIndex);
                if (medsSCD != null)
                {
                    if (MatchManager.Instance.emoteManager.EmoteNeedsTarget(_action))
                    {
                        __instance.characterT.gameObject.SetActive(value: true);
                        __instance.icon.sprite = MatchManager.Instance.emoteManager.emotesSprite[_action];
                        __instance.portraitStickerBase.sprite = medsSCD.GetEmoteBase();
                    }
                    else
                    {
                        __instance.characterT.gameObject.SetActive(value: false);
                        __instance.icon.sprite = medsSCD.GetEmote(_action);
                        __instance.iconStickerBase.sprite = medsSCD.GetEmoteBase();
                        Plugin.Log.LogDebug("StickerOffsetX: " + medsSCD.StickerOffsetX);
                        __instance.transform.localPosition += new Vector3(medsSCD.StickerOffsetX, 0f, 0f);
                    }
                    __instance.gameObject.SetActive(value: true);
                    if (_action != 2 && _action != 3)
                    {
                        Animator medsAnimator = Traverse.Create(__instance).Field("animator").GetValue<Animator>();
                        medsAnimator.SetTrigger("sticker");
                        Traverse.Create(__instance).Field("animator").SetValue(medsAnimator);
                    }
                }
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteTarget), "SetActiveHeroOnCardEmoteButton")]
        public static bool SetActiveHeroOnCardEmoteButtonPrefix(ref EmoteTarget __instance)
        {

            if (Plugin.medsEmotional.Value && GameManager.Instance.IsMultiplayer())
            {
                if (!((UnityEngine.Object)MatchManager.Instance != (UnityEngine.Object)null))
                    return false;
                SubClassData medsSCD = Globals.Instance.GetSubClassData(Plugin.medsSubclassList[MatchManager.Instance.emoteManager.heroActive]);
                if (medsSCD != (UnityEngine.Object)null)
                    __instance.icon.sprite = medsSCD.StickerBase;
                return false;
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "EmoteTarget")]
        public static bool EmoteTargetPrefix(ref MatchManager __instance, string _id, int _action, int _heroIndex = -1, bool _fromNet = false)
        {
            Plugin.Log.LogDebug("EmoteTarget has commenced!\n_id: " + _id + "\n_action: " + _action + "\n_heroIndex: " + _heroIndex + "\n_fromNet: " + _fromNet);
            if (Plugin.medsEmotional.Value && GameManager.Instance.IsMultiplayer())
            {
                int medsHeroIndex = _heroIndex;
                if (!_fromNet)
                    medsHeroIndex = __instance.emoteManager.heroActive;
                if (!_fromNet && GameManager.Instance.IsMultiplayer())
                {
                    PhotonView medsPhotonView = Traverse.Create(__instance).Field("photonView").GetValue<PhotonView>();
                    medsPhotonView.RPC("NET_EmoteTarget", RpcTarget.Others, (object)_id, (object)(byte)_action, (object)medsHeroIndex);
                }
                Transform transform = (Transform)null;
                CharacterItem characterItem = (CharacterItem)null;
                Hero[] medsHero = Traverse.Create(__instance).Field("TeamHero").GetValue<Hero[]>();
                NPC[] medsNPC = Traverse.Create(__instance).Field("TeamNPC").GetValue<NPC[]>();
                List<int> medsMyIndex = new();
                for (int index = 0; index < medsHero.Length; ++index)
                {
                    if (medsHero[index] != null && medsHero[index].Id == _id && medsHero[index].Alive)
                    {
                        transform = medsHero[index].HeroItem.transform;
                        characterItem = (CharacterItem)medsHero[index].HeroItem;
                        break;
                    }
                }
                if ((UnityEngine.Object)transform == (UnityEngine.Object)null)
                {
                    for (int index = 0; index < medsNPC.Length; ++index)
                    {
                        if (medsNPC[index] != null && medsNPC[index].Id == _id && medsNPC[index].Alive)
                        {
                            transform = medsNPC[index].NPCItem.transform;
                            characterItem = (CharacterItem)medsNPC[index].NPCItem;
                            break;
                        }
                    }
                }
                if ((UnityEngine.Object)transform == (UnityEngine.Object)null)
                {
                    for (int index = 0; index < medsHero.Length; ++index)
                    {
                        if (medsHero[index] != null && medsHero[index].Owner == NetworkManager.Instance.GetPlayerNick())
                            medsMyIndex.Add(index);
                    }
                    if (medsMyIndex.Count > 0)
                    {
                        Plugin.Log.LogDebug("medsMyIndex: " + medsMyIndex.Join());
                        int index = UnityEngine.Random.Range(0, medsMyIndex.Count);
                        Plugin.Log.LogDebug("index: " + index);
                        transform = medsHero[index].HeroItem.transform;
                        characterItem = (CharacterItem)medsHero[index].HeroItem;
                    }
                }
                if ((UnityEngine.Object)transform != (UnityEngine.Object)null && (UnityEngine.Object)characterItem != (UnityEngine.Object)null)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.emoteTargetPrefab, Vector3.zero, Quaternion.identity);
                    gameObject.transform.position = characterItem.emoteCharacterPing.transform.position;
                    /**/
                    gameObject.GetComponent<global::EmoteTarget>().SetIcons(medsHeroIndex, _action);
                    GameManager.Instance.PlayLibraryAudio("Pop3", 2.9f);
                }
                if (!_fromNet)
                    __instance.ResetCharactersPing();
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteManager), "SetBlocked")]
        public static void SetBlockedPrefix(ref bool _state)
        {
            if (Plugin.medsEmotional.Value)
                _state = false;
        }

        /*[HarmonyPrefix]
        [HarmonyPatch(typeof(Functions), "StringToAsciiInt32")]
        public static void StringToAsciiInt32Postfix(string str, ref int __result)
        {
            if ((UnityEngine.Object)AtOManager.Instance != (UnityEngine.Object)null && AtOManager.Instance.GetGameId == str)
            {
                // this + score * 101
                // max: 2,147,483,647
                // estimated max score: ~127k.
                // 130k * 101 = 13,130,000
                // so, really, we have about 2.13b numbers to use :D
            }
        }*/
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetScore")]
        public static bool SetScorePrefix(int score, bool singleplayer = true)
        {
            if (score <= 0)
                return false;
            SupportingActs.SetScoreLeaderboard(score, singleplayer, "RankingAct4");
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetObeliskScore")]
        public static bool SetObeliskScorePrefix(int score, bool singleplayer = true)
        {
            if (score <= 0)
                return false;
            SupportingActs.SetScoreLeaderboard(score, singleplayer, "Challenge");
            return false;
        }

        /*[HarmonyPrefix]
        [HarmonyPatch(typeof(TomeManager), "SelectTomeCards")]
        public static bool SelectTomeCardsPrefix(int index = -1, bool absolute = false)
        {
            
        }*/

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "SetAura")]
        public static void SetAuraPrefix(ref Character __instance, ref AuraCurseData _acData, ref int charges)
        {
            if (_acData.Id.ToLower() == "block")
            {
                string medsSubClassName = Traverse.Create(__instance).Field("subclassName").GetValue<string>();
                if (AtOManager.Instance.CharacterHaveTrait(medsSubClassName, "queenofthorns"))
                {
                    _acData = Globals.Instance.GetAuraCurseData("thorns");
                    charges = Functions.FuncRoundToInt((float)charges * 0.3f);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotHeroChar), "OnMouseUp")]
        public static bool BotHeroCharClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonCardback), "OnMouseUp")]
        public static bool BotonCardbackClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonEndTurn), "OnMouseUp")]
        public static bool BotonEndTurnClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonFilter), "OnMouseUp")]
        public static bool BotonFilterClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonGeneric), "OnMouseUp")]
        public static bool BotonGenericClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonMenuGameMode), "OnMouseUp")]
        public static bool BotonMenuGameModeClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonRollover), "OnMouseUp")]
        public static bool BotonRolloverClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonScore), "OnMouseUp")]
        public static bool BotonScoreClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonSkin), "OnMouseUp")]
        public static bool BotonSkinClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BotonSupply), "OnMouseUp")]
        public static bool BotonSupplyClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(botTownUpgrades), "OnMouseUp")]
        public static bool botTownUpgradesClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BoxPlayer), "OnMouseUp")]
        public static bool BoxPlayerClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardCraftSelectorEnergy), "OnMouseUp")]
        public static bool CardCraftSelectorEnergyClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardCraftSelectorRarity), "OnMouseUp")]
        public static bool CardCraftSelectorRarityClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "OnMouseUp")]
        public static bool CardItemClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardVertical), "OnMouseUp")]
        public static bool CardVerticalClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterGOItem), "OnMouseUp")]
        public static bool CharacterGOItemClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterItem), "fOnMouseUp")]
        public static bool CharacterItemClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterLoot), "OnMouseUp")]
        public static bool CharacterLootClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharPopupClose), "OnMouseUp")]
        public static bool CharPopupCloseClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CombatTarget), "OnMouseUp")]
        public static bool CombatTargetClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DeckInHero), "OnMouseUp")]
        public static bool DeckInHeroClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DeckPile), "OnMouseUp")]
        public static bool DeckPileClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EmoteManager), "OnMouseUp")]
        public static bool EmoteManagerClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HeroSelection), "OnMouseUp")]
        public static bool HeroSelectionClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(InitiativePortrait), "OnMouseUp")]
        public static bool InitiativePortraitClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ItemCombatIcon), "fOnMouseUp")]
        public static bool ItemCombatIconClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Node), "OnMouseUp")]
        public static bool NodeClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(OverCharacter), "OnMouseUp")]
        public static bool OverCharacterClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkChallengeItem), "OnMouseUp")]
        public static bool PerkChallengeItemClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkColumnItem), "OnMouseUp")]
        public static bool PerkColumnItemClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static bool PerkNodeClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RandomHeroSelector), "OnMouseUp")]
        public static bool RandomHeroSelectorClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Reply), "OnMouseUp")]
        public static bool ReplyClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TomeButton), "OnMouseUp")]
        public static bool TomeButtonClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TomeEdge), "OnMouseUp")]
        public static bool TomeEdgeClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TomeNumber), "OnMouseUp")]
        public static bool TomeNumberClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TomeRun), "OnMouseUp")]
        public static bool TomeRunClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TownBuilding), "OnMouseUp")]
        public static bool TownBuildingClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TraitLevel), "OnMouseUp")]
        public static bool TraitLevelClickCapture()
        {
            if (ObeliskialUI.ShowUI && ObeliskialUI.lockAtOToggle.isOn)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InputController), "DoKeyBinding")]
        public static bool DoKeyBindingPrefix(ref InputAction.CallbackContext _context)
        {
            if (Keyboard.current != null && _context.control == Keyboard.current[Key.F1])
            {
                ObeliskialUI.ShowUI = !ObeliskialUI.ShowUI;
                return false;
            }
            else if (Plugin.medsSpacebarContinue.Value && bFinalResolution && _context.control == Keyboard.current[Key.Space])
            {
                EventManager.Instance.Ready(true);
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventManager), "SetEvent")]
        public static void SetEventPrefix(ref EventData _eventData)
        {
            // clones copy character events?
            if (Plugin.medsDLCClones.Value)
            {
                Plugin.medsEventDataSource = Traverse.Create(Globals.Instance).Field("_Events").GetValue<Dictionary<string, EventData>>();
                if (_eventData != (EventData)null && Plugin.medsEventDataSource.ContainsKey(_eventData.EventId))
                {
                    bool erFound = false;
                    EventReplyData[] tempERD = Plugin.medsEventDataSource[_eventData.EventId].Replys;
                    for (int a = 0; a < Plugin.medsEventDataSource[_eventData.EventId].Replys.Length; a++)
                    {
                        EventReplyData reply = Plugin.medsEventDataSource[_eventData.EventId].Replys[a];
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
                    {
                        Plugin.medsEventDataSource[_eventData.EventId].Replys = tempERD;
                        Traverse.Create(Globals.Instance).Field("_Events").SetValue(Plugin.medsEventDataSource);
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "UpgradeTownTier")]
        public static void UpgradeTownTierPrefix(ref AtOManager __instance)
        {
            if (Plugin.IsHost() ? Plugin.medsVisitAllZones.Value : Plugin.medsMPVisitAllZones)
            {
                if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier1")))
                    AtOManager.Instance.RemovePlayerRequirement(Globals.Instance.GetRequirementData("_tier1"));
                if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")))
                    AtOManager.Instance.RemovePlayerRequirement(Globals.Instance.GetRequirementData("_tier2"));
                if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier3")))
                    AtOManager.Instance.RemovePlayerRequirement(Globals.Instance.GetRequirementData("_tier3"));
                if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("medsvisitedvoidlow")))
                {
                    Plugin.Log.LogDebug("APPARENTLY WE HAVE VISITED VOIDLOW ?? ?? ??");
                    AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("_tier3"));
                }
                else
                {
                    int a = 0;
                    if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("medsvisitedulminin")))
                        a++;
                    if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("medsvisitedvelkarath")))
                        a++;
                    if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("medsvisitedaquarfall")))
                        a++;
                    if (AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("medsvisitedfaeborg")))
                        a++;
                    Plugin.Log.LogDebug("UpgradeTownTier a: " + a.ToString());
                    if (a == 1)
                        AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("_tier1"));
                    else if (a > 1)
                        AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("_tier2"));
                }
            }
        }

        /*
        this _is_ doable, just beyond my current ability?
        basically you need to replace GenerateNewCard so it doesn't do this check:
          if (num < numCards)
            numCards = num;
        but that involves calling the GenerateNewCardCo IEnumerator, which... I have no fucking clue how to do with reflections?

        UPDATE: I FUCKIN' DID IT MOM
        LOOK AT THE TRANSPILER BELOW THX
        */

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "GetDrawCardsTurn")]
        public static bool GetDrawCardsTurnPrefix(ref Character __instance, ref int __result)
        {
            if (Plugin.IsHost() ? Plugin.medsOverlyCardergetic.Value : Plugin.medsMPOverlyCardergetic)
            {
                int drawCardsTurn = 5 + Traverse.Create(__instance).Field("drawModifier").GetValue<int>();
                if (drawCardsTurn < 0)
                    drawCardsTurn = 0;
                __result = drawCardsTurn;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "GetDrawCardsTurnForDisplayInDeck")]
        public static bool GetDrawCardsTurnForDisplayInDeckPrefix(ref Character __instance, ref int __result)
        {
            if (Plugin.IsHost() ? Plugin.medsOverlyCardergetic.Value : Plugin.medsMPOverlyCardergetic)
            {
                int drawCardsTurn = 5 + __instance.GetAuraDrawModifiers(false);
                if (drawCardsTurn < 0)
                    drawCardsTurn = 0;
                __result = drawCardsTurn;
                return false;
            }
            return true;
        }


        /*[HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "DealNewCard")]
        public static System.Collections.IEnumerator DealNewCardPrefix(Enums.CardFrom fromPlace, string comingFromCardId)
        {
            Plugin.Log.LogDebug("DEALNEWCARDPREFIX");
            
            yield return (object)null;
        }*/


        /* maybe unnecessary???
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "GenerateNewCard")]
        public static bool GenerateNewCardPrefix(ref MatchManager __instance,
          int numCards,
          string theCard,
          bool createCard,
          Enums.CardPlace where,
          CardData cardDataForModification = null,
          CardData copyDataFromThisCard = null,
          int heroIndex = -1,
          bool isHero = true,
          int indexForBatch = 0)
        {
            Plugin.Log.LogDebug("HELLO? AM I EVEN GENERATING NEW CARDS???");
            return false;
            if (__instance.MatchIsOver)
                return false;
            if (Plugin.IsHost() ? Plugin.medsOverlyCardergetic.Value : Plugin.medsMPOverlyCardergetic)
            {
                Plugin.Log.LogDebug("HeLLO, I am preparing for the IEnumerator...");
                if (where == Enums.CardPlace.Hand && numCards <= 0)
                    return false;
                Traverse.Create(__instance).Field("gameBusy").SetValue(true);
                MethodInfo myE = __instance.GetType().GetMethod("GenerateNewCardCo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                __instance.StartCoroutine((System.Collections.IEnumerator)myE.Invoke(__instance, new object[] { numCards, theCard, createCard, where, cardDataForModification, copyDataFromThisCard, heroIndex, isHero, indexForBatch }));
                Plugin.Log.LogDebug("at least the abs(%) of that working is positive :) :)");
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MatchManager), "GenerateNewCardCo")]
        public static void GenerateNewCardCoPrefix(ref int numCards)
        {
            Plugin.Log.LogInfo("HELLO IT IS I, THE IENUMERATOR! " + numCards.ToString());
        }
        */

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "Awake")]
        public static void AwakePrefix(ref MapManager __instance)
        {
            Plugin.Log.LogDebug("MAPMANAGER AWAKE: " + __instance.mapList.Count);
            if (Plugin.medsInvisibleGOHolder == null)
            {
                Plugin.Log.LogDebug("Creating container for Obeliskial Options GameObjects...");
                Plugin.medsInvisibleGOHolder = new("ObeliskialOptionsContainer");
                Plugin.medsInvisibleGOHolder.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(Plugin.medsInvisibleGOHolder);
            }
            if (!Plugin.medsLoadedCustomNodes)
            {
                foreach (KeyValuePair<string, List<NodeDataText>> kvp in Plugin.medsNodesByZone)
                {
                    Plugin.Log.LogDebug("checking zone:" + kvp.Key);
                    // THE PLAN: 
                    // runs through all customZones, builds customGOs
                    // runs through all vanillaZones, checks that all NodesByZone exist; if not, adds to customGOs
                    // finally, customGOs overwrite vanillaGOs whenever they exist (maybe already written below? needs testing)
                    bool zoneExists = false;
                    bool zoneNeedsUpdate = false;
                    for (int a = 0; a < __instance.mapList.Count; a++)
                    {
                        if (__instance.mapList[a].name.ToLower() == kvp.Key.ToLower())
                        {
                            Plugin.Log.LogDebug("found matching mapList zone: " + __instance.mapList[a].name.ToLower());
                            // zone already exists .: vanilla zone
                            // make a copy of it in case we actually use it
                            GameObject copiedVanillaMap = UnityEngine.Object.Instantiate<GameObject>(__instance.mapList[a], new Vector3(0f, 0f), Quaternion.identity, Plugin.medsInvisibleGOHolder.transform);
                            copiedVanillaMap.name = __instance.mapList[a].name;
                            // check that all nodes exist
                            zoneExists = true;
                            List<string> medsMovedNodes = new();
                            bool medsAddedPositions = false;
                            foreach (Transform transform1 in copiedVanillaMap.transform)
                            {
                                if (transform1.gameObject.name == "Nodes")
                                {
                                    if (!medsAddedPositions)
                                    {
                                        foreach (Transform tNode in transform1)
                                            if ((UnityEngine.Object)tNode.gameObject.GetComponent<Node>() != (UnityEngine.Object)null && (UnityEngine.Object)tNode.gameObject.GetComponent<Node>().nodeData != (UnityEngine.Object)null)
                                                Plugin.medsNodePositions[tNode.gameObject.GetComponent<Node>().nodeData.NodeId.ToLower()] = new Vector3(tNode.position.x, tNode.position.y);
                                        medsAddedPositions = true;
                                    }

                                    foreach (NodeDataText medsNDT in kvp.Value)
                                    {
                                        bool nodeExists = false;
                                        foreach (Transform transform2 in transform1)
                                        {
                                            if ((UnityEngine.Object)transform2.GetComponent<Node>().nodeData != (UnityEngine.Object)null && transform2.GetComponent<Node>().nodeData.NodeId.ToLower() == medsNDT.NodeId.ToLower())
                                            {
                                                nodeExists = true;
                                                // set position
                                                if (medsNDT.medsPosX != 0.0f || medsNDT.medsPosY != 0.0f)
                                                {
                                                    if (!medsMovedNodes.Contains(medsNDT.NodeId.ToLower()))
                                                        medsMovedNodes.Add(medsNDT.NodeId.ToLower());
                                                    Plugin.Log.LogDebug("Changing node " + medsNDT.NodeId.ToLower() + " position: " + medsNDT.medsPosX.ToString() + "," + medsNDT.medsPosY.ToString());
                                                    transform2.position = new Vector3(medsNDT.medsPosX, medsNDT.medsPosY);
                                                    Plugin.medsNodePositions[medsNDT.NodeId.ToLower()] = new Vector3(medsNDT.medsPosX, medsNDT.medsPosY);
                                                    zoneNeedsUpdate = true;
                                                }
                                                break;
                                            }
                                        }
                                        if (!nodeExists)
                                        {
                                            GameObject newNodeGO = UnityEngine.Object.Instantiate<GameObject>(transform1.GetChild(0).gameObject, new Vector3(medsNDT.medsPosX, medsNDT.medsPosY), Quaternion.identity, transform1);
                                            Plugin.Log.LogDebug("ADDING NODE: " + medsNDT.NodeId.ToLower() + " TO VANILLA ZONE " + kvp.Key.ToLower());
                                            if (!medsMovedNodes.Contains(medsNDT.NodeId.ToLower()))
                                                medsMovedNodes.Add(medsNDT.NodeId.ToLower());
                                            Plugin.medsNodePositions[medsNDT.NodeId.ToLower()] = new Vector3(medsNDT.medsPosX, medsNDT.medsPosY);
                                            newNodeGO.name = medsNDT.NodeId.ToLower();
                                            newNodeGO.transform.name = medsNDT.NodeId.ToLower();
                                            newNodeGO.GetComponent<Node>().name = medsNDT.NodeId.ToLower();
                                            newNodeGO.GetComponent<Node>().nodeData = Plugin.medsNodeDataSource[medsNDT.NodeId.ToLower()];
                                            newNodeGO.GetComponent<Node>().nodeData.name = medsNDT.NodeId.ToLower();
                                            zoneNeedsUpdate = true;
                                        }
                                    }
                                    break;
                                }
                            }
                            Plugin.Log.LogDebug("zone " + kvp.Key + " needsUpdate: " + zoneNeedsUpdate.ToString());
                            if (zoneNeedsUpdate)
                            {
                                foreach (Transform transform1 in copiedVanillaMap.transform)
                                {
                                    if (transform1.gameObject.name == "Roads")
                                    {
                                        List<string> medsRoadList = new();
                                        if (Plugin.medsBaseRoadGO == (GameObject)null)
                                        {
                                            Plugin.medsBaseRoadGO = UnityEngine.Object.Instantiate<GameObject>(transform1.GetChild(0).gameObject, new Vector3(0, 0), Quaternion.identity, Plugin.medsInvisibleGOHolder.transform);
                                            //Plugin.medsBaseRoadGO.SetActive(false); // not necessary if they're all stored in the invisible GO holder?
                                        }
                                        for (int b = transform1.childCount - 1; b >= 0; b--)
                                        {
                                            Transform childT = transform1.GetChild(b);
                                            string[] sNodes = childT.gameObject.name.Split("-");
                                            for (int c = 0; c < sNodes.Length; c++)
                                            {
                                                if (medsMovedNodes.Contains(sNodes[c].ToLower()))
                                                {
                                                    // at least one node that this road connects has moved, so we destroy the road
                                                    childT.parent = null;
                                                    // #PUNDESTROY
                                                    //childT.gameObject.SetActive(false);
                                                    UnityEngine.Object.Destroy(childT.gameObject);
                                                    break;
                                                }
                                            }
                                        }
                                        // add all roads to road list
                                        for (int b = transform1.childCount - 1; b >= 0; b--)
                                            if (!medsRoadList.Contains(transform1.GetChild(b).gameObject.name.ToLower()))
                                                medsRoadList.Add(transform1.GetChild(b).gameObject.name.ToLower());
                                        // check each node for new roads that need to be created
                                        foreach (NodeDataText medsNDT in kvp.Value)
                                        {
                                            foreach (string medsNodeToConnect in medsNDT.NodesConnected)
                                            {
                                                string roadName = medsNDT.NodeId.ToLower() + "-" + medsNodeToConnect.ToLower();
                                                if (!medsRoadList.Contains(roadName))
                                                {
                                                    // if road does not exist, create it
                                                    Plugin.Log.LogDebug("creating road " + roadName + " because it does not already exist in zone " + kvp.Key);
                                                    // instantiate baseRoadGO then update the LineRender positions?
                                                    if (Plugin.medsCustomRoads.ContainsKey(roadName))
                                                    {
                                                        GameObject newRoad = UnityEngine.Object.Instantiate<GameObject>(Plugin.medsBaseRoadGO, new Vector3(0, 0), Quaternion.identity, transform1);
                                                        newRoad.name = roadName;
                                                        LineRenderer lr = newRoad.GetComponent<LineRenderer>();
                                                        // use the vector3s defined in the roads TXT file
                                                        lr.positionCount = Plugin.medsCustomRoads[roadName].Count;
                                                        lr.SetPositions(Plugin.medsCustomRoads[roadName].ToArray());
                                                    }
                                                    else if (Plugin.medsNodePositions.ContainsKey(medsNDT.NodeId.ToLower()) && Plugin.medsNodePositions.ContainsKey(medsNodeToConnect.ToLower()))
                                                    {
                                                        GameObject newRoad = UnityEngine.Object.Instantiate<GameObject>(Plugin.medsBaseRoadGO, new Vector3(0, 0), Quaternion.identity, transform1);
                                                        newRoad.name = roadName;
                                                        LineRenderer lr = newRoad.GetComponent<LineRenderer>();
                                                        // just make a straight line between the two nodes
                                                        lr.positionCount = 2;
                                                        lr.SetPositions(new Vector3[] { Plugin.medsNodePositions[medsNDT.NodeId.ToLower()], Plugin.medsNodePositions[medsNodeToConnect.ToLower()] });
                                                    }
                                                    else
                                                    {
                                                        // idk. warn! error! something!
                                                        Plugin.Log.LogError("Could not initialize road " + roadName + " because node positions are unknown!");
                                                    }
                                                    medsRoadList.Add(roadName);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                // add to custom zone GOs
                                Plugin.medsCustomZoneGOs[kvp.Key.ToLower()] = copiedVanillaMap;
                                //UnityEngine.Object.DontDestroyOnLoad(Plugin.medsCustomZoneGOs[kvp.Key.ToLower()]); // no longer needed because these are stored in a custom GO
                            }
                            else
                            {
                                // #PUNDESTROY
                                //copiedVanillaMap.SetActive(false);
                                UnityEngine.Object.Destroy(copiedVanillaMap);
                                // destroy the copy we made earlier, because it won't be used
                            }
                            Plugin.Log.LogDebug("zone " + kvp.Key + " needsUpdate: " + zoneNeedsUpdate.ToString() + " (end)");
                            break;
                        }
                    }
                    if (!zoneExists)
                    {
                        int y = 0;
                        for (int a = 0; a < __instance.mapList.Count; a++)
                        {
                            if (__instance.mapList[a].name.ToLower() == "spiderlair")
                            {
                                y = a;
                                break;
                            }
                        }

                        GameObject newMap = UnityEngine.Object.Instantiate<GameObject>(__instance.mapList[y], new Vector3(0f, 0f, 0f), Quaternion.identity, Plugin.medsInvisibleGOHolder.transform);
                        newMap.name = kvp.Key;
                        foreach (Transform transform1 in newMap.transform)
                        {
                            if (transform1.gameObject.name == "Nodes")
                            {
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": starting Nodes");
                                for (int a = transform1.childCount - 1; a > 0; a--)
                                { // destroy existing nodes in Spiderlair
                                    Transform childT = transform1.GetChild(a);
                                    childT.parent = null;
                                    // #PUNDESTROY
                                    //childT.gameObject.SetActive(false);
                                    UnityEngine.Object.Destroy(childT.gameObject);
                                }
                                GameObject baseGO = transform1.GetChild(0).gameObject;
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": checking each node");
                                foreach (NodeDataText medsNDT in kvp.Value)
                                {
                                    GameObject newNodeGO = UnityEngine.Object.Instantiate<GameObject>(baseGO, new Vector3(medsNDT.medsPosX, medsNDT.medsPosY), Quaternion.identity, transform1);
                                    Plugin.Log.LogDebug("ADDING NODE " + medsNDT.NodeId.ToLower() + " TO CUSTOM ZONE " + kvp.Key.ToLower());
                                    Plugin.medsNodePositions[medsNDT.NodeId.ToLower()] = new Vector3(medsNDT.medsPosX, medsNDT.medsPosY);
                                    newNodeGO.name = medsNDT.NodeId.ToLower();
                                    newNodeGO.transform.name = medsNDT.NodeId.ToLower();
                                    newNodeGO.GetComponent<Node>().name = medsNDT.NodeId.ToLower();
                                    newNodeGO.GetComponent<Node>().nodeData = Plugin.medsNodeDataSource[medsNDT.NodeId.ToLower()];
                                    newNodeGO.GetComponent<Node>().nodeData.name = medsNDT.NodeId.ToLower();
                                }
                                baseGO.transform.parent = null;
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": destroying baseGO");
                                // #PUNDESTROY
                                //baseGO.SetActive(false);
                                UnityEngine.Object.Destroy(baseGO);
                            }
                            else if (transform1.gameObject.name == "Background_Bg")
                            {
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": background image");
                                if (Plugin.medsCustomZones.ContainsKey(kvp.Key.ToLower()))
                                    transform1.gameObject.GetComponent<SpriteRenderer>().sprite = DataTextConvert.GetSprite(Plugin.medsCustomZones[kvp.Key.ToLower()].BackgroundImg);
                            }
                        }
                        // deal with roads
                        foreach (Transform transform1 in newMap.transform)
                        {
                            if (transform1.gameObject.name == "Roads")
                            {
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": destroying existing roads");
                                for (int a = transform1.childCount - 1; a >= 0; a--)
                                {
                                    Transform childT = transform1.GetChild(a);
                                    childT.parent = null;
                                    // #PUNDESTROY
                                    //childT.gameObject.SetActive(false);
                                    UnityEngine.Object.Destroy(childT.gameObject);
                                }
                                Plugin.Log.LogDebug("custom zone " + kvp.Key + ": creating new roads");

                                foreach (NodeDataText medsNDT in kvp.Value)
                                {
                                    foreach (string medsNodeToConnect in medsNDT.NodesConnected)
                                    {
                                        string roadName = medsNDT.NodeId.ToLower() + "-" + medsNodeToConnect.ToLower();
                                        Plugin.Log.LogDebug("creating road " + roadName + " because it does not already exist in zone " + kvp.Key);
                                        // instantiate baseRoadGO then update the LineRender positions?
                                        if (Plugin.medsCustomRoads.ContainsKey(roadName))
                                        {
                                            GameObject newRoad = UnityEngine.Object.Instantiate<GameObject>(Plugin.medsBaseRoadGO, new Vector3(0, 0), Quaternion.identity, transform1);
                                            newRoad.name = roadName;
                                            LineRenderer lr = newRoad.GetComponent<LineRenderer>();
                                            // use the vector3s defined in the roads TXT file
                                            lr.positionCount = Plugin.medsCustomRoads[roadName].Count;
                                            lr.SetPositions(Plugin.medsCustomRoads[roadName].ToArray());
                                        }
                                        else if (Plugin.medsNodePositions.ContainsKey(medsNDT.NodeId.ToLower()) && Plugin.medsNodePositions.ContainsKey(medsNodeToConnect.ToLower()))
                                        {
                                            GameObject newRoad = UnityEngine.Object.Instantiate<GameObject>(Plugin.medsBaseRoadGO, new Vector3(0, 0), Quaternion.identity, transform1);
                                            newRoad.name = roadName;
                                            LineRenderer lr = newRoad.GetComponent<LineRenderer>();
                                            // just make a straight line between the two nodes
                                            lr.positionCount = 2;
                                            lr.SetPositions(new Vector3[] { Plugin.medsNodePositions[medsNDT.NodeId.ToLower()], Plugin.medsNodePositions[medsNodeToConnect.ToLower()] });
                                        }
                                        else
                                        {
                                            // idk. warn! error! something!
                                            Plugin.Log.LogError("Could not initialize road " + roadName + " because node positions are unknown!");
                                        }
                                    }
                                }
                            }
                        }
                        Plugin.Log.LogDebug("adding custom zone " + kvp.Key.ToLower() + " to custom zone GOs");
                        Plugin.medsCustomZoneGOs[kvp.Key.ToLower()] = newMap;
                        //Plugin.Log.LogDebug("custom zone: adding to CustomZoneGOs2");
                        //UnityEngine.Object.DontDestroyOnLoad(Plugin.medsCustomZoneGOs[kvp.Key.ToLower()]); // no longer needed because these are stored in a custom GO
                        //Plugin.Log.LogDebug("custom zone: adding to CustomZoneGOs3");
                    }
                }
                Plugin.medsLoadedCustomNodes = true;
            }
            Plugin.Log.LogDebug("adding zone GameObjects to mapList");
            foreach (KeyValuePair<String, GameObject> kvp in Plugin.medsCustomZoneGOs)
            {
                bool zoneFound = false;
                for (int a = __instance.mapList.Count - 1; a >= 0; a--)
                {
                    if (__instance.mapList[a].name.ToLower() == kvp.Key.ToLower())
                    {
                        // custom zone has been found
                        zoneFound = true;
                        // but we should destroy the object, right? for performance's sake?
                        if (!(__instance.mapList[a].gameObject == Plugin.medsCustomZoneGOs[kvp.Key.ToLower()].gameObject))
                        {
                            Plugin.Log.LogDebug("removing old zone GO: " + kvp.Key.ToLower());
                            GameObject oldMap = __instance.mapList[a].gameObject;
                            __instance.mapList.Remove(__instance.mapList[a]);
                            // #PUNDESTROY
                            //oldMap.SetActive(false);
                            UnityEngine.Object.Destroy(oldMap);
                            // MAY not have to be a copy?? but what if we Destroy _ours_? D:
                            // here, the if I wrapped this in should handle that; if they're the same, it won't be exiled and executed!
                            __instance.mapList.Add(Plugin.medsCustomZoneGOs[kvp.Key.ToLower()]);
                            Plugin.Log.LogDebug("added custom zone GO: " + kvp.Key.ToLower());
                        }
                        break;
                    }
                }
                if (!zoneFound)
                {
                    __instance.mapList.Add(Plugin.medsCustomZoneGOs[kvp.Key.ToLower()]);
                    Plugin.Log.LogDebug("added custom zone GO: " + kvp.Key.ToLower());
                }
            }
            Plugin.Log.LogDebug("MAPMANAGER PREFIX END");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapManager), "IncludeMapPrefab")]
        public static void IncludeMapPrefabPostfix(ref MapManager __instance, bool __result, string nodeId)
        {
            NodeData medsNodeData = Globals.Instance.GetNodeData(nodeId);
            if ((UnityEngine.Object)medsNodeData != (UnityEngine.Object)null && (UnityEngine.Object)medsNodeData.NodeZone != (UnityEngine.Object)null)
            {
                string zoneId = medsNodeData.NodeZone.ZoneId.ToLower();
                for (int index2 = 0; index2 < __instance.worldTransform.childCount; ++index2)
                {
                    if (Plugin.medsCustomZoneGOs.ContainsKey(zoneId) && __instance.worldTransform.GetChild(index2).gameObject.name.ToLower() == Plugin.medsCustomZoneGOs[zoneId].name.ToLower())
                    {
                        foreach (Transform transform1 in __instance.worldTransform.GetChild(index2))
                        {
                            if (transform1.gameObject.name == "Nodes")
                            {
                                foreach (Transform transform2 in transform1)
                                {
                                    transform2.GetComponent<Node>().nodeData = Globals.Instance.GetNodeData(transform2.GetComponent<Node>().nodeData.NodeId);
                                }
                            }
                        }
                        //__instance.worldTransform.GetChild(index2).localScale = new Vector3(1, 1, 1);
                        //__instance.worldTransform.GetChild(index2).gameObject.SetActive(true);

                        break;
                    }
                }
            }
            bool res = __result;
            Plugin.Log.LogDebug("IncludeMapPrefabPostfix: " + res.ToString());
        }


        // all of the below is just for testing


        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "TravelToThisNode")]
        public static void TravelToThisNodePrefix(ref MapManager __instance, Node _node)
        {
            Plugin.Log.LogDebug("TRAVELTOTHISNODE PREFIX");
            if ((UnityEngine.Object)_node == (UnityEngine.Object)null)
            {
                Plugin.Log.LogDebug("node is null! :(");
            }
            else if ((UnityEngine.Object)_node.nodeData == (UnityEngine.Object)null)
            {
                Plugin.Log.LogDebug("nodedata is null!");
            }
            else
            {
                Plugin.Log.LogDebug("nodeid: " + _node.nodeData.NodeId);
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "BeginMapContinue")]
        public static void BeginMapContinuePrefix(ref MapManager __instance)
        {
            Plugin.Log.LogDebug("BeginMapContinue PREFIX");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "PlayerSelectedNode")]
        public static void PlayerSelectedNodePrefix(ref MapManager __instance, Node _node)
        {
            Plugin.Log.LogDebug("PlayerSelectedNode PREFIX");
            if (_node == null)
            {
                Plugin.Log.LogDebug("PSN node null");
            }
            else if (_node.nodeData == null)
            {
                Plugin.Log.LogDebug("PSN nodedata null");
            }
            else
            {
                Plugin.Log.LogDebug("PSN node ID: " + _node.nodeData.NodeId);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "NET_PlayerSelectedNode")]
        public static void NET_PlayerSelectedNodePrefix(ref MapManager __instance)
        {
            Plugin.Log.LogDebug("NET_PlayerSelectedNode PREFIX");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapManager), "Awake")]
        public static void AwakePostfix(ref MapManager __instance)
        {
            Plugin.Log.LogDebug("MAPMANAGER POSTFIX");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "GetNodeFromId")]
        public static void GetNodeFromIdPrefix(ref MapManager __instance, string nodeId)
        {
            Plugin.Log.LogDebug("GetNodeFromId PREFIX: " + nodeId);
            /*foreach (KeyValuePair<string, Node> kvp in __instance.GetMapNodeDict())
                Plugin.Log.LogDebug("KVP KEY: " + kvp.Key);*/
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapManager), "GetNodeFromId")]
        public static void GetNodeFromIdPostfix(ref MapManager __instance, Node __result)
        {
            Plugin.Log.LogDebug("GetNodeFromId POSTFIX: ");
            /*foreach (KeyValuePair<string, Node> kvp in __instance.GetMapNodeDict())
                Plugin.Log.LogDebug("KVP KEY: " + kvp.Key);*/
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "GetMapNodes")]
        public static void GetMapNodesPrefix(ref MapManager __instance)
        {
            Plugin.Log.LogDebug("GetMapNodesPrefix, worldtransform children: " + __instance.worldTransform.childCount);

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "IncludeMapPrefab")]
        public static void IncludeMapPrefabPrefix(ref MapManager __instance, string nodeId)
        {
            Plugin.Log.LogDebug("IncludeMapPrefabPrefix, worldtransform children: " + __instance.worldTransform.childCount);
            Plugin.Log.LogDebug("nodeId is " + nodeId);
            NodeData medsND = Globals.Instance.GetNodeData(nodeId);
            Plugin.Log.LogDebug(medsND.NodeId);
            Plugin.Log.LogDebug(medsND.NodeZone.ZoneId);
            
            for (int index1 = 0; index1 < __instance.mapList.Count; ++index1)
            {
                Plugin.Log.LogDebug("index1: " + index1.ToString());
                if (__instance.mapList[index1].name.ToLower() == medsND.NodeZone.ZoneId.ToLower())
                {

                    Plugin.Log.LogDebug("FOUND IT");
                    bool flag2 = false;
                    for (int index2 = 0; index2 < __instance.worldTransform.childCount; ++index2)
                    {
                        if (__instance.worldTransform.GetChild(index2).gameObject.name == __instance.mapList[index1].name)
                        {
                            Plugin.Log.LogDebug("FLAG2");
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        Plugin.Log.LogDebug("NOFLAG2");
                        //UnityEngine.Object.Instantiate<GameObject>(__instance.mapList[index1], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, __instance.worldTransform).name = __instance.mapList[index1].name;
                    }
                }
                else
                {
                    Plugin.Log.LogDebug("Did not find: " + __instance.mapList[index1].name.ToLower());
                }
            }
        }
        /*
        // JANK TIME! WEE WOO
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MenuSaveButton), "SetGameData")]
        public static void SetGameDataPrefix(GameData _gameData, ref MenuSaveButton __instance)
        {
            NodeData nD = Globals.Instance.GetNodeData(_gameData.CurrentMapNode);
            if (_gameData.GameType == Enums.GameType.Adventure && (UnityEngine.Object)nD != (UnityEngine.Object)null && (UnityEngine.Object)nD.NodeZone != (UnityEngine.Object)null)
            {
                Plugin.Log.LogDebug("SetGameData zoneID: " + nD.NodeZone.ZoneId.ToLower());
                if (Globals.Instance.ZoneDataSource.ContainsKey(nD.NodeZone.ZoneId.ToLower()))
                {
                    Plugin.Log.LogDebug("CONTAINS ZONEID!");
                    string s = Globals.Instance.ZoneDataSource[nD.NodeZone.ZoneId.ToLower()].ZoneName;
                    Plugin.Log.LogDebug("getting text: " + s);
                    Plugin.Log.LogDebug("result: " + Texts.Instance.GetText(s));
                }
                else
                {
                    Plugin.Log.LogDebug("DOESNOTCONTAIN ZONEID!");
                    string s = nD.NodeZone.ZoneId.ToLower();
                    Plugin.Log.LogDebug("getting text: " + s);
                    Plugin.Log.LogDebug("result: " + Texts.Instance.GetText(s));
                }
            }
        }*/
    }
    /*
    THIS TRANSPILER WORKS!
    WOO
    (but cards end up all the way on the right, so probably need to look at CardItem.PositionCardInTable + MatchManager.RepositionCards ??)


    [HarmonyPatch]
    public static class DealNewCard_Transpiler
    {
        static MethodBase TargetMethod()
        {
            var mainClass = typeof(MatchManager).GetNestedTypes(AccessTools.all).FirstOrDefault(t => t.FullName.Contains("d__512"));
            return mainClass.GetMethods(AccessTools.all).FirstOrDefault(m => m.Name.Contains("MoveNext"));
        }
        /*[HarmonyTranspiler]
        [HarmonyPatch(typeof(MatchManager), "DealNewCard")]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Plugin.Log.LogDebug("THE CALL IS COMING FROM WITHIN THE TRANSPILER");
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count - 1; i++)
            {
                string s = "OPCODE: ";
                if (codes[i].opcode == System.Reflection.Emit.OpCodes.Call &&
                    codes[i].operand != null &&
                    codes[i].operand.ToString().Contains("CountHeroHand", StringComparison.OrdinalIgnoreCase) &&
                    codes[i + 1].opcode == System.Reflection.Emit.OpCodes.Ldc_I4_S &&
                    codes[i + 1].operand != null &&
                    codes[i + 1].operand.ToString() == "10")
                {
                    codes[i + 1].operand = (sbyte)100;
                    //Plugin.Log.LogDebug("FOUNDOPER")
                    //codes[i + 1].operand = 
                }
                s += codes[i].opcode.ToString() + " ";
                if (codes[i].operand != null)
                    s += codes[i].operand.ToString() + " (" + codes[i].operand.GetType().ToString() + ")";
                Plugin.Log.LogDebug(s);
            }
            return codes.AsEnumerable();
        }
    }*/
}

﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks.Data;
using Steamworks;
using static Enums;
using System.IO;
using System.Linq;
using UnityEngine.InputSystem;
using BepInEx;
using System.Collections;
using JetBrains.Annotations;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;
using System.Reflection.Emit;

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
    }
    [HarmonyPatch]
    internal class Patch20230319
    {
        public static Vector3 medsPosIni;
        public static Vector3 medsPosIniBlocked;
        public static bool bSelectingPerk = false;
        public static bool bRemovingCards = false;
        public static bool bPreventCardRemoval = false;
        public static bool bFinalResolution = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "Start")]
        public static void GMStartPostfix(ref GameManager __instance)
        {
            __instance.gameVersion = __instance.gameVersion + " (OO v" + Plugin.ModVersion + ")";
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MainMenuManager), "Start")]
        public static void MMStartPostfix(ref MainMenuManager __instance)
        {
            __instance.version.text = __instance.version.text.Replace("(", "    (").Replace(")", ")     ") + Plugin.ModDate;
            TMP_Text meds1 = __instance.gameModeSelectionChoose.GetComponent<TMP_Text>();
            TMP_SpriteAsset meds2 = meds1.spriteAsset;
            Plugin.Log.LogInfo("meds1: " + meds1.name);
            Plugin.Log.LogInfo("meds2: " + meds2.name);
            Plugin.Log.LogInfo("meds3: " + meds2.spriteCharacterTable.Count);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "GetLootData")]
        public static void GetLootDataPostfix(ref LootData __result)
        {
            // Plugin.Log.LogInfo("GetLootData: " + __result);
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
                    num0 += 0.5f;
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
                __result.DefaultPercentMythic += (((float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() + 1f) * num0 * num1 / 50f);
                __result.DefaultPercentRare += (((float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() + 1f) * num0 * num1 / 50f);
                __result.DefaultPercentEpic += (((float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() * (float)AtOManager.Instance.GetTownTier() + 1f) * num0 * num1 / 50f);
            }
            float fBadLuckProt = Plugin.IsHost() ? (float)Plugin.medsMPShopBadLuckProtection : (float)Plugin.medsShopBadLuckProtection.Value;
            if (fBadLuckProt > 0f)
            {
                fBadLuckProt = fBadLuckProt * ((float)AtOManager.Instance.GetTownTier() + 1) * ((float)AtOManager.Instance.GetTownTier() + 1) * (float)Plugin.iShopsWithNoPurchase / 100000;
                __result.DefaultPercentMythic += fBadLuckProt;
                __result.DefaultPercentEpic += fBadLuckProt;
                __result.DefaultPercentRare += fBadLuckProt;
                __result.DefaultPercentUncommon += fBadLuckProt;
                Plugin.iShopsWithNoPurchase += 1;
            }
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
        }

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

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InputController), "DoKeyBinding")]
        public static bool DoKeyBindingPrefix(ref InputAction.CallbackContext _context)
        {

            if (Plugin.medsSpacebarContinue.Value && bFinalResolution && _context.control == Keyboard.current[Key.Space])
            {
                EventManager.Instance.Ready(true);
                return false;
            }
            return true;
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
        [HarmonyPatch(typeof(EmoteManager), "SelectNextCharacter")]
        public static bool SelectNextCharacterPrefix(ref EmoteManager __instance)
        {
            if (Plugin.medsEmotional.Value)
            {
                Hero[] teamHero = MatchManager.Instance.GetTeamHero();
                if (teamHero != null)
                {
                    int num = 0;
                    bool flag = false;
                    while (!flag && num < 100)
                    {
                        num++;
                        __instance.heroActive++;
                        if (__instance.heroActive > 3)
                            __instance.heroActive = 0;
                        if (teamHero[__instance.heroActive] != null || !GameManager.Instance.IsMultiplayer())
                            flag = true;
                    }
                    if (teamHero[__instance.heroActive] != null && teamHero[__instance.heroActive].HeroData != null)
                    {
                        SpriteRenderer spriteRenderer = __instance.characterPortrait;
                        Sprite sprite = (__instance.characterPortraitBlocked.sprite = teamHero[__instance.heroActive].HeroData.HeroSubClass.StickerBase);
                        spriteRenderer.sprite = sprite;
                        __instance.characterPortrait.transform.localPosition = medsPosIni + new Vector3(teamHero[__instance.heroActive].HeroData.HeroSubClass.StickerOffsetX, 0f, 0f);
                        __instance.characterPortraitBlocked.transform.parent.transform.localPosition = medsPosIniBlocked + new Vector3(teamHero[__instance.heroActive].HeroData.HeroSubClass.StickerOffsetX, 0f, 0f);
                    }
                }
                if (teamHero[__instance.heroActive] == null || !(teamHero[__instance.heroActive].HeroData != null))
                {
                    return false;
                }
                if (teamHero[__instance.heroActive].Alive)
                {
                    for (int i = 0; i < __instance.emotes.Length; i++)
                    {
                        __instance.emotes[i].SetBlocked(_state: false);
                    }
                    return false;
                }
                for (int j = 0; j < __instance.emotes.Length; j++)
                {
                    if (!__instance.EmoteNeedsTarget(j))
                        __instance.emotes[j].SetBlocked(_state: true);
                }
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
                    __result = 1;
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
            if (__state > 10 && (Plugin.IsHost() ? Plugin.medsOverlyTenergetic.Value : Plugin.medsMPOverlyTenergetic))
                __instance.EnergyCurrent = __state;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardCraftManager), "ShowElements")]
        public static void ShowElementsPrefix(ref CardCraftManager __instance, string cardId)
        {
            if (__instance.craftType == 1) // removing cards
            {
                if (Plugin.IsHost() ? Plugin.medsDiminutiveDecks.Value : Plugin.medsMPDiminutiveDecks)
                    bRemovingCards = true;
                CardData cardData = Globals.Instance.GetCardData(cardId, false);
                switch (Plugin.IsHost() ? Plugin.medsDenyDiminishingDecks.Value : Plugin.medsMPDenyDiminishingDecks)
                {
                    case "Cannot Remove Cards":
                        bPreventCardRemoval = true;
                        break;
                    case "Cannot Remove Curses":
                        if (cardData.CardClass == Enums.CardClass.Injury)
                            bPreventCardRemoval = true;
                        break;
                    case "Can Only Remove Curses":
                        if (cardData.CardClass != Enums.CardClass.Injury)
                            bPreventCardRemoval = true;
                        break;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardCraftManager), "ShowElements")]
        public static void ShowElementsPostfix(ref CardCraftManager __instance)
        {
            bRemovingCards = false;
            bPreventCardRemoval = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Hero), "GetTotalCardsInDeck")]
        public static void GetTotalCardsInDeckPostfix(ref int __result)
        {
            if ((bRemovingCards) && (Plugin.IsHost() ? Plugin.medsDiminutiveDecks.Value : Plugin.medsMPDiminutiveDecks))
                __result = 50;
            if (bPreventCardRemoval)
                __result = 10;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "PlayerHasRequirement")]
        public static void PlayerHasRequirementPostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsNoPlayerRequirements.Value : Plugin.medsMPNoPlayerRequirements)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "PlayerHasRequirementClass")]
        public static void PlayerHasRequirementClassPostfix(ref bool __result)
        {
            if (Plugin.IsHost() ? Plugin.medsNoPlayerClassRequirements.Value : Plugin.medsMPNoPlayerClassRequirements)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "PlayerHasRequirementItem")]
        public static void PlayerHasRequirementItemPostfix(ref int __result)
        {
            if (Plugin.IsHost() ? Plugin.medsNoPlayerItemRequirements.Value : Plugin.medsMPNoPlayerItemRequirements)
                __result = 0;
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
        [HarmonyPatch(typeof(HeroSelectionManager), "Start")]
        public static void HSMStartPrefix()
        {
            if ((UnityEngine.Object)PlayerManager.Instance != (UnityEngine.Object)null)
            {
                // reset skin if it doesn’t exist for this character
                if (!PlayerManager.Instance.SkinUsed.Keys.Contains("medsdlctwo") || String.Compare(PlayerManager.Instance.SkinUsed["medsdlctwo"], Plugin.medsDLCCloneTwoSkin) > 0)
                    PlayerManager.Instance.SkinUsed["medsdlctwo"] = "medsdlctwoa";
                if (!PlayerManager.Instance.SkinUsed.Keys.Contains("medsdlcthree") || String.Compare(PlayerManager.Instance.SkinUsed["medsdlcthree"], Plugin.medsDLCCloneThreeSkin) > 0)
                    PlayerManager.Instance.SkinUsed["medsdlcthree"] = "medsdlcthreea";
                if (!PlayerManager.Instance.SkinUsed.Keys.Contains("medsdlcfour") || String.Compare(PlayerManager.Instance.SkinUsed["medsdlcfour"], Plugin.medsDLCCloneFourSkin) > 0)
                    PlayerManager.Instance.SkinUsed["medsdlcfour"] = "medsdlcfoura";
                // reset cardback if it doesn’t exist for this character
                if (!PlayerManager.Instance.CardbackUsed.Keys.Contains("medsdlctwo") || String.Compare(PlayerManager.Instance.CardbackUsed["medsdlctwo"], Plugin.medsDLCCloneTwoCardback) > 0)
                    PlayerManager.Instance.CardbackUsed["medsdlctwo"] = "medsdlctwoa";
                if (!PlayerManager.Instance.CardbackUsed.Keys.Contains("medsdlcthree") || String.Compare(PlayerManager.Instance.CardbackUsed["medsdlcthree"], Plugin.medsDLCCloneThreeCardback) > 0)
                    PlayerManager.Instance.CardbackUsed["medsdlcthree"] = "medsdlcthreea";
                if (!PlayerManager.Instance.CardbackUsed.Keys.Contains("medsdlcfour") || String.Compare(PlayerManager.Instance.CardbackUsed["medsdlcfour"], Plugin.medsDLCCloneFourCardback) > 0)
                    PlayerManager.Instance.CardbackUsed["medsdlcfour"] = "medsdlcfoura";
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "NodeScore")]
        public static void NodeScorePrefix()
        {
            // #TODO: make sure you disable this! :)
            if (1 == 1)
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
                Plugin.Log.LogInfo("num1: " + num1);
                Plugin.Log.LogInfo("num2: " + num2);
                Plugin.Log.LogInfo("num3: " + num3);
                Plugin.Log.LogInfo("num4: " + num4);
                Plugin.Log.LogInfo("num5: " + num5);
                Plugin.Log.LogInfo("num6: " + num6);
                Plugin.Log.LogInfo("num7: " + num7);
                Plugin.Log.LogInfo("num8: " + num8);
                Plugin.Log.LogInfo("num9: " + num9);
                Plugin.Log.LogInfo("num10: " + num10);
                Plugin.Log.LogInfo("num11: " + num11);
                Plugin.Log.LogInfo("num12: " + num12);
                Plugin.Log.LogInfo("num13: " + num13);
                Plugin.Log.LogInfo("num14: " + num14);
                Plugin.Log.LogInfo("num15: " + num15);
                Plugin.Log.LogInfo("num16: " + num16);
                Plugin.Log.LogInfo("num17: " + num17);
                Plugin.Log.LogInfo("num18: " + num18);
                Plugin.Log.LogInfo("num19: " + num19);
                Plugin.Log.LogInfo("num20: " + num20);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AtOManager), "CalculateScore")]
        public static void CalculateScorePrefix(bool _calculateMadnessMultiplier, int _auxValue)
        {
            // #TODO: make sure you disable this! :)
            if (1 == 1)
            {

                int medsTotalScoreTMP = Traverse.Create(AtOManager.Instance).Field("totalScoreTMP").GetValue<int>();
                Plugin.Log.LogInfo("_CMM: " + _calculateMadnessMultiplier);
                Plugin.Log.LogInfo("_aux: " + _auxValue);
                Plugin.Log.LogInfo("totalScoreTMP: " + medsTotalScoreTMP);
                medsTotalScoreTMP += Functions.FuncRoundToInt((float)(medsTotalScoreTMP * Functions.GetMadnessScoreMultiplier(AtOManager.Instance.GetMadnessDifficulty(), !GameManager.Instance.IsObeliskChallenge()) / 100));
                Plugin.Log.LogInfo("score: " + medsTotalScoreTMP);
            }
        }
    }
}

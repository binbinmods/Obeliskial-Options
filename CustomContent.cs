﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using BepInEx;
using System.Reflection;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.TextCore;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

namespace Obeliskial_Options
{
    [HarmonyPatch]
    internal class CustomContent
    {
        private static int ngValue; // OK
        private static int ngValueMaster; // OK
        private static string ngCorruptors; // OK
        private static int obeliskMadnessValue; // OK
        private static int obeliskMadnessValueMaster; // OK
        private static PhotonView photonView;
        private static BoxSelection[] boxSelection;
        private static Dictionary<GameObject, bool> boxFilled = new Dictionary<GameObject, bool>();
        private static Dictionary<GameObject, HeroSelection> boxHero = new Dictionary<GameObject, HeroSelection>();
        private static Dictionary<string, SubClassData[]> subclassDictionary = new Dictionary<string, SubClassData[]>();
        private static Dictionary<string, SubClassData> nonHistorySubclassDictionary = new Dictionary<string, SubClassData>();
        private static Dictionary<string, string> SubclassByName = new Dictionary<string, string>();

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Globals), "CreateGameContent")]
        public static bool CreateGameContentPrefix()
        {
            if (Plugin.medsCustomContent.Value)
            {
                Plugin.Log.LogInfo("LOADING GAME CONTENT; PLEASE WAIT!");
                CreateCustomContent();
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Globals), "CreateGameContent")]
        public static void CreateGameContentPostfix()
        {
            DirectoryInfo medsDI = new DirectoryInfo(Paths.ConfigPath);
            if (!medsDI.Exists)
                medsDI.Create();

            // handle things that would otherwise be handled in content loading
            if (!(Plugin.medsCustomContent.Value))
            {

                // store list of drop-only items in case the setting is changed later
                Plugin.medsCardsSource = Traverse.Create(Globals.Instance).Field("_CardsSource").GetValue<Dictionary<string, CardData>>();
                Plugin.medsItemDataSource = Traverse.Create(Globals.Instance).Field("_ItemDataSource").GetValue<Dictionary<string, ItemData>>();
                foreach (KeyValuePair<string, ItemData> kvp in Plugin.medsItemDataSource)
                {
                    if (kvp.Value.DropOnly && !Plugin.medsDropOnlyItems.Contains(kvp.Key))
                        Plugin.medsDropOnlyItems.Add(kvp.Key);
                    if (Plugin.medsDropShop.Value && !Plugin.medsDoNotDropList.Contains(kvp.Value.Id))
                        Plugin.medsItemDataSource[kvp.Key].DropOnly = false;
                    if (Plugin.medsAllThePets.Value && kvp.Value.QuestItem == true) { Plugin.medsItemDataSource[kvp.Key].QuestItem = false; };
                }
                if (Plugin.medsAllThePets.Value)
                {
                    foreach (string key in Plugin.medsCardsSource.Keys)
                    {
                        if (!(Plugin.medsCardsSource[key].ShowInTome) && Plugin.medsCardsSource[key].CardClass == Enums.CardClass.Item)
                        {
                            Plugin.medsCardsSource[key].ShowInTome = true;
                        }
                        if ((UnityEngine.Object)Plugin.medsCardsSource[key].Item != (UnityEngine.Object)null && Plugin.medsCardsSource[key].Item.QuestItem)
                        {
                            Plugin.medsCardsSource[key].Item.QuestItem = false;
                        }
                    }
                    Traverse.Create(Globals.Instance).Field("_CardsSource").SetValue(Plugin.medsCardsSource);
                }
                Traverse.Create(Globals.Instance).Field("_ItemDataSource").SetValue(Plugin.medsItemDataSource);
                Globals.Instance.CardItemByType = new();
                Globals.Instance.CardEnergyCost = new();
                Globals.Instance.CreateCardClones();
            }
            Plugin.medsEventRequirementDataSource = Traverse.Create(Globals.Instance).Field("_Requirements").GetValue<Dictionary<string, EventRequirementData>>();
            EventRequirementData medsERD = ScriptableObject.CreateInstance<EventRequirementData>();
            medsERD.AssignToPlayerAtBegin = false;
            medsERD.Description = "Obeliskial Options placeholder for town tier changes";
            medsERD.ItemSprite = (Sprite)null;
            medsERD.RequirementId = "_tier1b";
            medsERD.RequirementTrack = false;
            medsERD.TrackSprite = (Sprite)null;
            Plugin.medsEventRequirementDataSource["_tier1b"] = medsERD;
            Traverse.Create(Globals.Instance).Field("_Requirements").SetValue(Plugin.medsEventRequirementDataSource);

            // export vanilla content
            if (Plugin.medsExportJSON.Value)
            {
                Plugin.Log.LogInfo("PRAYGE; THE EXPORT HAS BEGUN");
                Plugin.medsSubClassesSource = Traverse.Create(Globals.Instance).Field("_SubClassSource").GetValue<Dictionary<string, SubClassData>>();
                Plugin.medsTraitsSource = Traverse.Create(Globals.Instance).Field("_TraitsSource").GetValue<Dictionary<string, TraitData>>();
                Plugin.medsCardsSource = Traverse.Create(Globals.Instance).Field("_CardsSource").GetValue<Dictionary<string, CardData>>();
                Plugin.medsPerksSource = Traverse.Create(Globals.Instance).Field("_PerksSource").GetValue<Dictionary<string, PerkData>>();
                Plugin.medsAurasCursesSource = Traverse.Create(Globals.Instance).Field("_AurasCursesSource").GetValue<Dictionary<string, AuraCurseData>>();
                Plugin.medsNPCsSource = Traverse.Create(Globals.Instance).Field("_NPCsSource").GetValue<Dictionary<string, NPCData>>();
                Plugin.medsNodeDataSource = Traverse.Create(Globals.Instance).Field("_NodeDataSource").GetValue<Dictionary<string, NodeData>>();
                Plugin.medsLootDataSource = Traverse.Create(Globals.Instance).Field("_LootDataSource").GetValue<Dictionary<string, LootData>>();
                Plugin.medsPerksNodesSource = Traverse.Create(Globals.Instance).Field("_PerksNodesSource").GetValue<Dictionary<string, PerkNodeData>>();
                Plugin.medsChallengeDataSource = Traverse.Create(Globals.Instance).Field("_WeeklyDataSource").GetValue<Dictionary<string, ChallengeData>>();
                Plugin.medsChallengeTraitsSource = Traverse.Create(Globals.Instance).Field("_ChallengeTraitsSource").GetValue<Dictionary<string, ChallengeTrait>>();
                Plugin.medsCombatDataSource = Traverse.Create(Globals.Instance).Field("_CombatDataSource").GetValue<Dictionary<string, CombatData>>();
                Plugin.medsEventDataSource = Traverse.Create(Globals.Instance).Field("_Events").GetValue<Dictionary<string, EventData>>();
                Plugin.medsEventRequirementDataSource = Traverse.Create(Globals.Instance).Field("_Requirements").GetValue<Dictionary<string, EventRequirementData>>();
                Plugin.medsZoneDataSource = Traverse.Create(Globals.Instance).Field("_ZoneDataSource").GetValue<Dictionary<string, ZoneData>>();
                Plugin.medsKeyNotesDataSource = Globals.Instance.KeyNotes;
                Plugin.medsPackDataSource = Traverse.Create(Globals.Instance).Field("_PackDataSource").GetValue<Dictionary<string, PackData>>();
                Plugin.medsCardPlayerPackDataSource = Traverse.Create(Globals.Instance).Field("_CardPlayerPackDataSource").GetValue<Dictionary<string, CardPlayerPackData>>();
                Plugin.medsCardbacksSource = Traverse.Create(Globals.Instance).Field("_CardbackDataSource").GetValue<Dictionary<string, CardbackData>>();
                Plugin.medsSkinsSource = Traverse.Create(Globals.Instance).Field("_SkinDataSource").GetValue<Dictionary<string, SkinData>>();
                Plugin.medsCorruptionPackDataSource = Traverse.Create(Globals.Instance).Field("_CorruptionPackDataSource").GetValue<Dictionary<string, CorruptionPackData>>();
                Plugin.medsCinematicDataSource = Traverse.Create(Globals.Instance).Field("_Cinematics").GetValue<Dictionary<string, CinematicData>>();
                Plugin.medsTierRewardDataSource = Traverse.Create(Globals.Instance).Field("_TierRewardDataSource").GetValue<Dictionary<int, TierRewardData>>();
                Plugin.medsNodeCombatEventRelation = Traverse.Create(Globals.Instance).Field("_NodeCombatEventRelation").GetValue<Dictionary<string, string>>();
                Plugin.medsCardPlayerPairsPackDataSource = Traverse.Create(Globals.Instance).Field("_CardPlayerPairsPackDataSource").GetValue<Dictionary<string, CardPlayerPairsPackData>>();

                string fullList = "id\tname\tclass\n";
                foreach (KeyValuePair<string, CardData> kvp in Plugin.medsCardsSource)
                {
                    fullList += kvp.Key + "\t" + kvp.Value.CardName + "\t" + DataTextConvert.ToString(kvp.Value.CardClass) + "\n";
                }
                Plugin.RecursiveFolderCreate("Obeliskial_exported", "card");
                if (Plugin.medsExportSprites.Value)
                    Plugin.RecursiveFolderCreate("Obeliskial_exported", "sprite");

                File.WriteAllText(Path.Combine(Paths.ConfigPath, "Obeliskial_exported", "cardlist.json"), fullList);
                Plugin.ExtractData(Plugin.medsSubClassesSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsTraitsSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCardsSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsPerksSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsAurasCursesSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsNPCsSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsNodeDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsLootDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsPerksNodesSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsChallengeDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsChallengeTraitsSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCombatDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsEventDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsEventRequirementDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsZoneDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsKeyNotesDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsPackDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCardPlayerPackDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsItemDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCardbacksSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsSkinsSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCorruptionPackDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsCinematicDataSource.Select(item => item.Value).ToArray());
                Plugin.ExtractData(Plugin.medsTierRewardDataSource.Select(item => item.Value).ToArray());

                // some quick and dirty EventReplyData extraction; messy as fuck
                string combined = "{";
                Plugin.RecursiveFolderCreate("Obeliskial_exported", "eventReply", "combined");
                int h = 1; // counts hundreds for combined files
                int a = 1;
                foreach (KeyValuePair<string, EventReplyData> kvp in Plugin.medsEventReplyData)
                {
                    string id = kvp.Key;
                    string text = JsonUtility.ToJson(DataTextConvert.ToText(kvp.Value), true);
                    combined += "\"" + id + "\":" + text + ",";
                    Plugin.WriteToJSON("eventReply", text, id);
                    if (a >= h * 100)
                    {
                        Plugin.WriteToJSON("eventReply", combined.Remove(combined.Length - 1) + "}", a, h);
                        h++;
                        combined = "{";
                    }
                    a++;
                }
                Plugin.WriteToJSON("eventReply", combined.Remove(combined.Length - 1) + "}", a - 1, h);
                Plugin.Log.LogInfo("exported " + (a - 1).ToString() + " eventReply values!");

                Plugin.medsExportJSON.Value = false; // turn off after exporting*/
                Plugin.Log.LogInfo("OUR PRAYERS WERE ANSWERED");
            }



            Plugin.SubClassReplace();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerManager), "IsCardUnlocked")]
        public static void IsCardUnlockedPostfix(string _cardId, ref bool __result)
        {
            if (!__result && Plugin.medsCustomUnlocks.Contains(_cardId))
                __result = true;
            if (Plugin.medsAllThePets.Value && _cardId == "tombstone" && !__result)
                PlayerManager.Instance.CardUnlock(_cardId);
            // #TODO: unlock custom enchantments, items, pets?
            // Plugin.Log.LogInfo("checking unlock: " + _cardId);
            //if (Plugin.medsCustomContent.Value && Plugin.medsCardsToImport.ContainsKey(_cardId))
            //__result = true;
        }

        public static void CreateCustomContent()
        {

            Plugin.Log.LogInfo("Loading resources...");
            KeyNotesData[] keyNotesDataArray = Resources.LoadAll<KeyNotesData>("KeyNotes");
            AuraCurseData[] auraCurseDataArray1 = Resources.LoadAll<AuraCurseData>("Auras");
            AuraCurseData[] auraCurseDataArray2 = Resources.LoadAll<AuraCurseData>("Curses");
            CardData[] cardDataArray = Resources.LoadAll<CardData>("Cards");
            TraitData[] traitDataArray = Resources.LoadAll<TraitData>("Traits");
            HeroData[] heroDataArray = Resources.LoadAll<HeroData>("Heroes");
            PerkData[] perkDataArray = Resources.LoadAll<PerkData>("Perks");
            PackData[] packDataArray = Resources.LoadAll<PackData>("Packs");
            SubClassData[] subClassDataArray = Resources.LoadAll<SubClassData>("SubClass");
            NPCData[] npcDataArray = Resources.LoadAll<NPCData>("NPCs");
            PerkNodeData[] perkNodeDataArray = Resources.LoadAll<PerkNodeData>("PerkNode");
            EventData[] eventDataArray = Resources.LoadAll<EventData>("World/Events");
            EventRequirementData[] eventRequirementDataArray = Resources.LoadAll<EventRequirementData>("World/Events/Requirements");
            CombatData[] combatDataArray = Resources.LoadAll<CombatData>("World/Combats");
            NodeData[] nodeDataArray = Resources.LoadAll<NodeData>("World/MapNodes");
            TierRewardData[] tierRewardDataArray = Resources.LoadAll<TierRewardData>("Rewards");
            ItemData[] itemDataArray = Resources.LoadAll<ItemData>("Items");
            LootData[] lootDataArray = Resources.LoadAll<LootData>("Loot");
            SkinData[] skinDataArray = Resources.LoadAll<SkinData>("Skins");
            CardbackData[] cardbackDataArray = Resources.LoadAll<CardbackData>("Cardbacks");
            CorruptionPackData[] corruptionPackDataArray = Resources.LoadAll<CorruptionPackData>("CorruptionRewards");
            CardPlayerPackData[] cardPlayerPackDataArray = Resources.LoadAll<CardPlayerPackData>("CardPlayer");
            CinematicData[] cinematicDataArray = Resources.LoadAll<CinematicData>("Cinematics");
            ZoneData[] zoneDataArray = Resources.LoadAll<ZoneData>("World/Zones");
            ChallengeTrait[] challengeTraitArray = Resources.LoadAll<ChallengeTrait>("Challenge/Traits");
            ChallengeData[] challengeDataArray = Resources.LoadAll<ChallengeData>("Challenge/Weeks");
            CardPlayerPairsPackData[] playerPairsPackDataArray = Resources.LoadAll<CardPlayerPairsPackData>("CardPlayerPairs");

            // attach mod (fallback) spriteasset to AtO spriteasset
            Plugin.medsFallbackSpriteAsset.name = "ModFallbackSpriteAsset";
            foreach (TMP_SpriteAsset medsSAResistsIcons in Resources.FindObjectsOfTypeAll<TMP_SpriteAsset>())
            {
                // Plugin.Log.LogDebug("SOMP: " + medsSAResistsIcons.name);
                if (medsSAResistsIcons.name == "ResistsIcons")
                {
                    //Plugin.medsFallbackSpriteAsset.version = "1.1.0";

                    // TMP_SpriteAsset medsSAFallback = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

                    // Plugin.Log.LogDebug("new");

                    // medsSAResistsIcons.fallbackSpriteAssets.Add(medsSAFallback);



                    /*for (int index = 0; index < this.spriteInfoList.Count; ++index)
                    {
                        TMP_Sprite spriteInfo = this.spriteInfoList[index];
                        TMP_SpriteGlyph tmpSpriteGlyph = new TMP_SpriteGlyph();
                        tmpSpriteGlyph.index = (uint)index;
                        tmpSpriteGlyph.sprite = spriteInfo.sprite;
                        tmpSpriteGlyph.metrics = new GlyphMetrics(spriteInfo.width, spriteInfo.height, spriteInfo.xOffset, spriteInfo.yOffset, spriteInfo.xAdvance);
                        tmpSpriteGlyph.glyphRect = new GlyphRect((int)spriteInfo.x, (int)spriteInfo.y, (int)spriteInfo.width, (int)spriteInfo.height);
                        tmpSpriteGlyph.scale = 1f;
                        tmpSpriteGlyph.atlasIndex = 0;
                        this.m_SpriteGlyphTable.Add(tmpSpriteGlyph);
                        TMP_SpriteCharacter tmpSpriteCharacter = new TMP_SpriteCharacter();
                        tmpSpriteCharacter.glyph = (Glyph)tmpSpriteGlyph;
                        tmpSpriteCharacter.unicode = spriteInfo.unicode == 0 ? 65534U : (uint)spriteInfo.unicode;
                        tmpSpriteCharacter.name = spriteInfo.name;
                        tmpSpriteCharacter.scale = spriteInfo.scale;
                        this.m_SpriteCharacterTable.Add(tmpSpriteCharacter);
                    }
                    this.UpdateLookupTables();*/
                }
            }

            // initialise vanilla vars? (why is this not in the declaration, do you think?)
            Globals.Instance.CardItemByType = new();
            Globals.Instance.CardEnergyCost = new();



            // basically just work your way down this list, importing your custom versions at the same time?

            Plugin.RecursiveFolderCreate("Obeliskial_importing", "subclass");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "trait");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "card");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "perk");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "npc");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "node");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "loot");
            // Plugin.RecursiveFolderCreate("Obeliskial_importing", "item");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "auraCurse");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "perkNode");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "challengeData");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "challengeTrait");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "combatData");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "event");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "eventRequirement");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "zone");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "pack");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "cardPlayerPack");

            // load default sprites
            Plugin.Log.LogInfo("Loading default sprites...");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "sprite");
            // auraCurse
            Texture2D spriteTexture = new Texture2D(2, 2);
            byte[] medsDefaultSprite = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 44, 0, 0, 0, 44, 8, 6, 0, 0, 0, 30, 132, 90, 1, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 12, 227, 73, 68, 65, 84, 88, 71, 189, 89, 107, 140, 93, 213, 121, 93, 231, 117, 207, 185, 247, 158, 123, 231, 206, 120, 222, 30, 63, 198, 134, 169, 29, 202, 0, 33, 32, 8, 15, 213, 68, 38, 37, 73, 69, 74, 67, 91, 151, 54, 52, 85, 129, 150, 63, 169, 84, 161, 162, 182, 42, 18, 170, 210, 42, 137, 104, 213, 18, 245, 103, 132, 82, 135, 84, 80, 242, 80, 105, 129, 212, 24, 28, 8, 15, 67, 235, 215, 96, 155, 177, 241, 120, 30, 246, 60, 238, 157, 59, 247, 125, 158, 123, 119, 237, 59, 51, 182, 3, 243, 174, 218, 53, 179, 117, 206, 156, 179, 207, 217, 107, 175, 239, 219, 223, 247, 237, 51, 248, 63, 128, 198, 102, 177, 57, 108, 41, 54, 87, 181, 174, 174, 174, 244, 19, 79, 60, 161, 11, 33, 52, 54, 117, 188, 178, 25, 107, 105, 124, 79, 243, 229, 27, 66, 111, 95, 247, 85, 217, 214, 92, 11, 79, 187, 123, 251, 119, 236, 204, 100, 178, 3, 78, 218, 221, 97, 59, 118, 167, 105, 89, 142, 161, 233, 38, 52, 205, 212, 116, 205, 208, 56, 76, 20, 199, 113, 173, 90, 47, 248, 158, 63, 23, 4, 129, 55, 53, 122, 254, 63, 138, 133, 162, 215, 181, 185, 183, 211, 118, 172, 146, 95, 111, 204, 205, 230, 11, 195, 31, 28, 57, 114, 116, 126, 132, 79, 66, 215, 245, 248, 19, 132, 57, 147, 143, 95, 107, 254, 189, 99, 199, 246, 157, 87, 93, 119, 195, 30, 33, 197, 77, 213, 82, 105, 199, 192, 117, 55, 220, 106, 57, 9, 155, 211, 214, 83, 217, 22, 104, 186, 206, 166, 129, 252, 230, 159, 144, 18, 146, 39, 66, 200, 203, 141, 215, 226, 72, 240, 190, 134, 194, 196, 8, 2, 63, 132, 219, 209, 5, 211, 52, 144, 73, 39, 49, 242, 193, 137, 31, 253, 219, 179, 207, 126, 69, 141, 183, 20, 154, 132, 73, 80, 231, 249, 146, 74, 239, 218, 181, 171, 53, 211, 209, 126, 119, 182, 189, 227, 247, 58, 251, 182, 221, 153, 202, 100, 83, 138, 70, 163, 92, 68, 231, 230, 45, 72, 88, 22, 44, 203, 128, 209, 212, 16, 136, 69, 68, 194, 36, 173, 55, 173, 135, 40, 138, 169, 172, 224, 145, 79, 145, 172, 32, 215, 144, 215, 212, 185, 97, 154, 8, 194, 8, 190, 31, 240, 24, 34, 149, 180, 49, 125, 246, 212, 55, 126, 178, 127, 255, 95, 53, 31, 94, 2, 139, 132, 231, 223, 254, 49, 236, 218, 189, 107, 160, 111, 240, 250, 151, 90, 59, 123, 182, 166, 221, 12, 172, 68, 2, 6, 21, 84, 228, 108, 158, 39, 72, 52, 97, 153, 205, 163, 101, 234, 188, 199, 89, 43, 149, 57, 33, 245, 27, 83, 81, 69, 46, 138, 37, 9, 11, 170, 171, 68, 215, 16, 146, 164, 186, 30, 70, 234, 168, 38, 19, 33, 226, 76, 34, 175, 142, 51, 255, 245, 206, 239, 190, 243, 218, 235, 63, 88, 160, 176, 20, 154, 234, 46, 9, 221, 208, 164, 219, 146, 235, 54, 18, 201, 166, 98, 22, 25, 217, 84, 52, 105, 219, 72, 38, 76, 158, 147, 48, 137, 38, 19, 234, 154, 137, 116, 210, 65, 107, 26, 200, 101, 76, 184, 41, 11, 57, 215, 65, 91, 214, 69, 150, 166, 118, 83, 14, 239, 219, 72, 57, 156, 40, 159, 165, 17, 154, 77, 89, 67, 91, 176, 72, 117, 182, 24, 142, 15, 159, 57, 182, 48, 252, 178, 88, 150, 176, 136, 100, 158, 98, 22, 140, 166, 116, 234, 197, 84, 145, 231, 22, 253, 45, 65, 133, 29, 59, 129, 20, 143, 233, 4, 73, 186, 46, 122, 210, 117, 108, 118, 125, 164, 77, 7, 142, 78, 146, 52, 121, 78, 247, 145, 50, 28, 36, 52, 94, 51, 109, 78, 90, 145, 85, 239, 82, 230, 53, 216, 20, 89, 29, 146, 10, 235, 166, 94, 54, 77, 179, 176, 48, 252, 178, 88, 150, 240, 190, 125, 251, 202, 34, 138, 167, 213, 11, 149, 43, 152, 100, 111, 146, 112, 211, 111, 13, 139, 164, 82, 72, 32, 131, 118, 221, 194, 96, 78, 71, 143, 213, 137, 70, 113, 55, 226, 201, 86, 200, 11, 41, 136, 169, 110, 36, 230, 218, 224, 206, 5, 64, 217, 133, 17, 184, 48, 5, 73, 179, 191, 109, 42, 247, 210, 249, 190, 121, 111, 12, 163, 80, 45, 246, 137, 123, 239, 189, 119, 53, 194, 218, 178, 62, 172, 240, 165, 7, 30, 120, 190, 109, 203, 206, 47, 39, 29, 101, 78, 155, 110, 96, 82, 213, 36, 28, 35, 9, 59, 38, 1, 223, 69, 154, 97, 182, 47, 25, 163, 84, 114, 81, 45, 153, 144, 245, 42, 108, 90, 163, 92, 35, 57, 229, 78, 40, 160, 156, 229, 72, 25, 31, 34, 85, 67, 77, 159, 131, 39, 171, 240, 34, 15, 94, 24, 96, 174, 82, 107, 250, 113, 105, 98, 228, 95, 95, 252, 254, 254, 223, 90, 24, 122, 57, 200, 21, 9, 223, 125, 223, 125, 127, 221, 51, 112, 205, 227, 73, 71, 249, 160, 67, 101, 28, 164, 244, 12, 236, 32, 11, 171, 210, 6, 92, 108, 131, 56, 209, 14, 148, 116, 70, 142, 20, 194, 134, 14, 61, 226, 194, 11, 104, 250, 152, 71, 154, 158, 35, 32, 117, 219, 24, 132, 35, 97, 94, 59, 3, 63, 149, 71, 195, 41, 192, 211, 42, 168, 248, 101, 148, 171, 117, 46, 200, 24, 249, 115, 195, 79, 190, 252, 252, 115, 79, 46, 12, 189, 28, 228, 178, 46, 161, 80, 154, 157, 61, 45, 104, 46, 229, 191, 154, 164, 207, 133, 22, 180, 106, 6, 184, 208, 137, 240, 240, 22, 212, 223, 216, 140, 242, 225, 78, 184, 78, 7, 110, 249, 106, 26, 15, 124, 47, 137, 71, 223, 117, 240, 219, 47, 152, 216, 251, 45, 13, 87, 125, 17, 176, 93, 29, 213, 215, 183, 193, 123, 185, 31, 193, 107, 219, 129, 169, 118, 24, 116, 21, 61, 76, 50, 148, 40, 173, 24, 87, 68, 12, 191, 86, 25, 154, 31, 117, 101, 172, 168, 112, 79, 119, 207, 174, 193, 59, 247, 28, 238, 191, 230, 218, 100, 38, 225, 34, 17, 182, 32, 89, 236, 133, 117, 146, 131, 191, 210, 143, 76, 15, 112, 245, 151, 52, 252, 242, 62, 13, 93, 215, 107, 72, 182, 206, 63, 71, 193, 160, 242, 135, 194, 201, 31, 10, 28, 126, 90, 98, 252, 231, 140, 203, 30, 195, 226, 206, 34, 226, 59, 79, 193, 235, 24, 199, 156, 62, 142, 74, 92, 65, 28, 84, 194, 225, 183, 15, 221, 244, 206, 207, 223, 61, 161, 172, 178, 18, 86, 84, 184, 167, 187, 247, 130, 148, 113, 193, 107, 120, 84, 65, 153, 154, 33, 65, 153, 158, 100, 83, 29, 192, 237, 127, 169, 227, 87, 255, 81, 199, 246, 61, 151, 201, 42, 44, 146, 85, 216, 253, 235, 58, 190, 122, 64, 199, 13, 15, 49, 235, 49, 64, 199, 103, 217, 113, 38, 7, 20, 91, 160, 251, 140, 131, 84, 89, 70, 209, 116, 117, 174, 62, 38, 153, 88, 86, 131, 34, 204, 144, 190, 52, 30, 249, 163, 71, 170, 188, 59, 173, 242, 152, 74, 4, 160, 127, 74, 250, 167, 48, 99, 92, 255, 53, 13, 55, 61, 202, 200, 97, 47, 116, 190, 132, 165, 94, 167, 225, 238, 111, 235, 248, 165, 95, 163, 194, 54, 239, 31, 218, 1, 109, 154, 101, 136, 199, 144, 167, 219, 8, 107, 245, 99, 175, 190, 114, 162, 180, 44, 145, 43, 176, 162, 194, 15, 61, 244, 176, 160, 137, 78, 50, 65, 49, 46, 71, 164, 162, 126, 152, 149, 72, 120, 211, 0, 21, 163, 233, 23, 49, 249, 223, 18, 63, 122, 80, 224, 31, 118, 8, 236, 255, 124, 140, 35, 223, 101, 10, 159, 93, 184, 73, 24, 52, 206, 221, 223, 214, 144, 163, 27, 139, 178, 13, 109, 54, 11, 217, 176, 17, 251, 228, 93, 11, 142, 215, 203, 205, 82, 100, 85, 172, 72, 88, 249, 83, 163, 86, 57, 38, 98, 149, 62, 35, 196, 26, 155, 237, 115, 240, 16, 135, 191, 35, 240, 209, 43, 18, 179, 103, 100, 147, 220, 75, 127, 34, 48, 244, 3, 137, 226, 57, 13, 231, 14, 0, 47, 252, 129, 192, 191, 63, 26, 99, 118, 248, 178, 110, 106, 146, 253, 119, 105, 76, 62, 180, 67, 201, 129, 168, 51, 85, 51, 172, 53, 138, 193, 251, 42, 133, 175, 204, 102, 30, 43, 119, 33, 97, 191, 86, 61, 18, 248, 62, 188, 152, 69, 138, 206, 16, 228, 86, 161, 111, 174, 96, 140, 138, 254, 244, 79, 5, 94, 254, 186, 192, 129, 199, 5, 70, 14, 209, 220, 237, 30, 114, 55, 207, 32, 59, 88, 129, 221, 18, 225, 253, 127, 1, 142, 127, 159, 54, 185, 194, 18, 125, 159, 213, 224, 118, 211, 187, 230, 52, 228, 199, 166, 112, 250, 205, 83, 207, 223, 177, 229, 235, 7, 104, 192, 53, 43, 188, 172, 235, 40, 133, 11, 19, 19, 195, 182, 133, 26, 75, 24, 120, 168, 34, 72, 204, 65, 220, 53, 4, 231, 214, 49, 228, 243, 85, 156, 57, 86, 66, 212, 57, 139, 150, 219, 166, 144, 254, 204, 56, 172, 109, 117, 196, 153, 6, 116, 186, 128, 193, 25, 215, 153, 187, 24, 21, 47, 33, 201, 240, 173, 22, 165, 168, 177, 194, 111, 116, 253, 116, 223, 246, 103, 191, 177, 179, 247, 230, 56, 14, 231, 239, 179, 144, 91, 17, 171, 26, 225, 177, 199, 254, 108, 252, 189, 87, 255, 243, 205, 83, 71, 223, 195, 197, 201, 51, 240, 237, 34, 130, 76, 30, 242, 246, 147, 72, 62, 120, 20, 185, 125, 103, 144, 249, 194, 40, 220, 59, 39, 145, 222, 62, 3, 221, 9, 16, 21, 146, 136, 102, 44, 48, 239, 129, 89, 184, 57, 241, 69, 48, 193, 97, 238, 188, 68, 162, 209, 50, 183, 219, 248, 205, 231, 216, 41, 226, 132, 100, 138, 46, 205, 34, 78, 25, 117, 69, 172, 74, 248, 145, 135, 31, 22, 174, 101, 223, 63, 51, 62, 250, 228, 133, 241, 115, 241, 84, 254, 12, 66, 183, 136, 176, 109, 26, 90, 119, 126, 190, 181, 81, 117, 51, 96, 36, 201, 32, 158, 176, 209, 24, 82, 185, 88, 98, 247, 125, 26, 238, 96, 232, 91, 132, 82, 113, 102, 72, 162, 174, 7, 34, 217, 218, 250, 42, 239, 212, 13, 27, 126, 130, 143, 37, 153, 143, 84, 252, 94, 45, 82, 176, 182, 209, 87, 238, 195, 41, 191, 245, 214, 219, 181, 161, 195, 71, 158, 156, 60, 247, 225, 183, 206, 143, 126, 132, 82, 245, 34, 23, 95, 3, 177, 83, 67, 8, 250, 54, 83, 178, 87, 180, 49, 119, 170, 13, 133, 215, 55, 179, 52, 149, 24, 124, 16, 184, 247, 187, 26, 28, 134, 220, 69, 76, 29, 145, 56, 250, 140, 228, 102, 79, 23, 90, 171, 252, 64, 183, 101, 222, 202, 160, 154, 237, 68, 172, 88, 176, 30, 250, 5, 107, 44, 133, 85, 21, 86, 80, 47, 82, 43, 219, 53, 204, 191, 171, 215, 42, 227, 83, 147, 23, 224, 7, 1, 26, 245, 24, 65, 217, 130, 63, 145, 69, 237, 39, 253, 240, 222, 105, 167, 74, 58, 110, 255, 11, 141, 100, 13, 216, 217, 203, 163, 151, 199, 36, 222, 248, 27, 129, 153, 179, 64, 208, 94, 56, 205, 234, 114, 194, 112, 49, 197, 4, 228, 229, 186, 17, 103, 115, 52, 201, 26, 176, 38, 194, 10, 106, 225, 28, 31, 58, 85, 40, 231, 103, 190, 89, 152, 158, 97, 184, 243, 224, 87, 13, 248, 147, 73, 4, 135, 182, 33, 26, 205, 64, 84, 18, 184, 135, 153, 239, 246, 199, 127, 241, 181, 229, 113, 90, 233, 41, 137, 227, 63, 164, 91, 108, 229, 100, 115, 197, 159, 233, 105, 124, 148, 216, 132, 217, 220, 86, 120, 237, 61, 144, 44, 149, 87, 85, 87, 97, 205, 132, 155, 160, 6, 155, 28, 247, 153, 192, 243, 207, 53, 84, 89, 88, 55, 16, 49, 9, 200, 15, 55, 49, 54, 75, 236, 253, 166, 134, 235, 31, 100, 246, 99, 93, 179, 216, 159, 5, 25, 94, 98, 232, 123, 243, 239, 185, 143, 235, 40, 163, 186, 227, 116, 131, 233, 237, 253, 68, 27, 70, 179, 91, 180, 114, 207, 213, 136, 54, 245, 104, 194, 82, 139, 115, 13, 108, 22, 187, 172, 201, 28, 10, 7, 95, 125, 187, 150, 74, 165, 14, 52, 234, 62, 23, 17, 131, 157, 175, 35, 230, 130, 219, 190, 7, 184, 238, 247, 89, 224, 115, 241, 44, 98, 250, 132, 108, 102, 189, 147, 47, 144, 172, 203, 250, 247, 211, 195, 168, 167, 39, 10, 45, 237, 109, 71, 211, 125, 200, 247, 94, 3, 191, 119, 167, 38, 210, 106, 141, 174, 81, 186, 245, 41, 76, 147, 153, 22, 85, 171, 54, 14, 52, 55, 152, 36, 172, 182, 239, 177, 17, 163, 239, 22, 46, 48, 14, 188, 136, 179, 175, 8, 124, 111, 175, 192, 249, 119, 5, 52, 135, 75, 243, 179, 195, 8, 122, 46, 162, 102, 141, 94, 236, 232, 111, 31, 235, 189, 78, 171, 111, 185, 6, 97, 71, 31, 221, 129, 113, 121, 45, 238, 160, 176, 46, 194, 42, 19, 113, 115, 171, 133, 115, 225, 209, 216, 247, 26, 17, 83, 182, 34, 12, 18, 30, 126, 81, 226, 252, 33, 101, 40, 137, 145, 215, 36, 51, 160, 68, 101, 138, 68, 62, 197, 162, 253, 158, 99, 8, 183, 142, 33, 200, 78, 162, 20, 159, 159, 238, 30, 212, 42, 219, 6, 17, 246, 13, 104, 50, 193, 226, 105, 225, 171, 192, 154, 176, 46, 194, 138, 142, 226, 247, 135, 247, 63, 53, 26, 122, 226, 67, 6, 163, 230, 214, 93, 119, 5, 198, 223, 3, 126, 252, 53, 129, 167, 7, 4, 158, 251, 13, 129, 217, 15, 73, 118, 107, 25, 209, 230, 60, 162, 214, 60, 194, 220, 12, 202, 241, 4, 243, 101, 101, 172, 255, 70, 4, 91, 6, 52, 145, 164, 175, 171, 8, 180, 30, 44, 118, 95, 147, 15, 171, 15, 33, 52, 157, 236, 202, 14, 74, 209, 48, 135, 100, 200, 13, 234, 108, 6, 26, 23, 94, 146, 238, 208, 251, 25, 13, 215, 254, 142, 134, 182, 171, 25, 186, 216, 87, 109, 125, 226, 116, 5, 81, 154, 233, 59, 81, 131, 150, 20, 156, 99, 80, 28, 184, 81, 19, 25, 198, 231, 166, 223, 174, 209, 21, 22, 177, 62, 151, 96, 239, 106, 73, 106, 33, 235, 31, 35, 76, 78, 168, 226, 91, 207, 187, 208, 189, 4, 118, 126, 30, 248, 226, 63, 49, 164, 253, 185, 142, 207, 253, 173, 142, 109, 55, 51, 13, 87, 89, 71, 235, 116, 27, 131, 165, 41, 221, 198, 178, 19, 106, 59, 84, 177, 157, 121, 178, 107, 245, 219, 43, 177, 46, 194, 106, 0, 175, 74, 115, 112, 29, 89, 34, 153, 151, 17, 9, 89, 17, 23, 155, 196, 150, 219, 88, 133, 113, 203, 164, 234, 222, 222, 27, 185, 159, 251, 2, 235, 229, 144, 213, 4, 149, 86, 197, 191, 250, 60, 165, 169, 207, 85, 81, 84, 81, 60, 55, 66, 86, 161, 73, 120, 213, 244, 188, 0, 229, 18, 205, 239, 104, 116, 93, 19, 214, 172, 98, 35, 205, 16, 150, 250, 226, 211, 127, 153, 129, 42, 39, 11, 244, 97, 51, 226, 246, 135, 111, 22, 172, 117, 213, 135, 64, 181, 64, 89, 170, 206, 174, 86, 145, 173, 132, 117, 41, 172, 212, 98, 70, 146, 220, 22, 73, 45, 50, 102, 98, 221, 151, 113, 138, 241, 181, 4, 156, 254, 177, 68, 109, 122, 190, 223, 200, 65, 217, 44, 230, 77, 105, 65, 208, 37, 72, 181, 73, 92, 69, 25, 110, 6, 74, 107, 143, 250, 159, 196, 186, 8, 43, 40, 255, 115, 92, 14, 25, 25, 83, 129, 86, 243, 162, 12, 23, 20, 119, 14, 39, 246, 75, 70, 135, 24, 207, 252, 138, 192, 139, 127, 204, 153, 41, 130, 106, 255, 103, 179, 232, 103, 98, 225, 114, 163, 194, 42, 118, 7, 220, 12, 45, 188, 108, 3, 88, 183, 15, 171, 50, 48, 219, 165, 146, 92, 152, 15, 245, 185, 90, 192, 144, 229, 237, 28, 67, 173, 103, 18, 35, 111, 104, 24, 121, 157, 53, 227, 148, 142, 250, 45, 167, 81, 252, 242, 107, 136, 220, 18, 51, 33, 43, 59, 201, 237, 85, 28, 71, 97, 195, 231, 42, 216, 56, 174, 36, 188, 186, 161, 72, 216, 73, 1, 157, 219, 181, 24, 201, 74, 41, 178, 170, 245, 48, 91, 192, 236, 109, 111, 161, 250, 233, 19, 104, 220, 60, 132, 218, 173, 199, 81, 254, 220, 187, 240, 182, 141, 176, 42, 187, 136, 32, 93, 228, 102, 187, 161, 182, 174, 74, 221, 208, 111, 52, 26, 107, 217, 10, 45, 135, 117, 43, 108, 178, 170, 234, 217, 6, 185, 103, 239, 29, 101, 97, 212, 138, 97, 106, 14, 158, 123, 17, 141, 246, 49, 84, 6, 143, 162, 246, 169, 33, 212, 175, 58, 197, 18, 108, 28, 13, 119, 26, 190, 81, 230, 230, 213, 167, 15, 147, 178, 82, 56, 100, 93, 250, 255, 73, 88, 165, 81, 135, 25, 234, 254, 7, 238, 137, 61, 175, 50, 170, 204, 29, 218, 101, 42, 201, 20, 156, 157, 130, 151, 189, 200, 9, 144, 104, 114, 150, 110, 94, 133, 208, 232, 191, 36, 171, 26, 87, 109, 16, 5, 1, 55, 246, 27, 199, 250, 92, 98, 1, 170, 88, 217, 212, 13, 217, 168, 85, 206, 9, 17, 146, 84, 136, 144, 59, 234, 192, 168, 32, 52, 171, 36, 170, 118, 34, 116, 3, 45, 98, 239, 249, 175, 25, 77, 85, 73, 216, 178, 185, 233, 251, 95, 96, 93, 10, 43, 52, 3, 190, 82, 154, 79, 122, 158, 55, 169, 254, 175, 209, 140, 0, 170, 113, 97, 177, 118, 67, 212, 60, 170, 15, 83, 243, 9, 99, 17, 177, 16, 65, 58, 157, 94, 216, 31, 111, 12, 235, 38, 172, 160, 72, 171, 22, 69, 209, 172, 136, 231, 255, 201, 34, 152, 85, 212, 49, 102, 232, 90, 60, 111, 94, 231, 143, 18, 88, 241, 14, 35, 17, 228, 114, 57, 37, 251, 134, 177, 33, 194, 139, 240, 253, 176, 16, 171, 189, 185, 130, 34, 167, 252, 148, 80, 223, 226, 22, 9, 55, 201, 46, 40, 173, 155, 137, 218, 193, 131, 7, 27, 205, 78, 27, 196, 37, 194, 107, 77, 207, 87, 162, 163, 103, 243, 132, 78, 135, 150, 170, 230, 92, 0, 149, 15, 116, 77, 43, 243, 56, 69, 35, 156, 229, 241, 24, 233, 254, 140, 81, 226, 229, 76, 174, 245, 159, 23, 186, 109, 24, 202, 35, 47, 97, 165, 111, 197, 75, 225, 177, 167, 158, 110, 187, 48, 57, 121, 31, 159, 43, 155, 186, 81, 52, 45, 163, 108, 25, 86, 217, 78, 36, 106, 150, 105, 212, 50, 153, 84, 227, 218, 129, 1, 255, 43, 123, 239, 186, 226, 99, 213, 138, 248, 184, 104, 151, 254, 158, 23, 20, 248, 31, 57, 211, 50, 143, 235, 44, 247, 222, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
            spriteTexture.LoadImage(medsDefaultSprite);
            Plugin.medsSprites["medsDefaultAuraCurse"] = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);

            // card 
            spriteTexture = new Texture2D(2, 2);
            medsDefaultSprite = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 1, 0, 0, 0, 1, 0, 8, 6, 0, 0, 0, 92, 114, 168, 102, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 186, 6, 73, 68, 65, 84, 120, 94, 236, 189, 7, 128, 36, 73, 117, 38, 252, 170, 50, 179, 124, 181, 31, 63, 59, 118, 103, 214, 123, 195, 226, 89, 172, 240, 194, 158, 64, 32, 36, 14, 29, 178, 200, 223, 33, 129, 144, 244, 235, 228, 205, 9, 132, 147, 142, 19, 32, 225, 36, 88, 188, 89, 88, 118, 151, 245, 150, 245, 126, 188, 109, 111, 203, 187, 204, 255, 251, 94, 68, 84, 87, 247, 206, 46, 102, 167, 123, 204, 214, 171, 142, 206, 200, 136, 200, 72, 23, 223, 247, 222, 139, 136, 204, 140, 133, 97, 24, 147, 174, 116, 165, 43, 79, 75, 137, 219, 101, 87, 186, 210, 149, 167, 161, 116, 9, 160, 43, 93, 121, 26, 75, 151, 0, 186, 210, 149, 167, 177, 116, 9, 224, 100, 145, 104, 81, 8, 143, 16, 22, 151, 121, 146, 240, 253, 59, 239, 234, 246, 13, 61, 13, 164, 219, 9, 120, 162, 10, 129, 234, 150, 46, 28, 9, 232, 20, 119, 135, 185, 124, 162, 187, 237, 242, 168, 18, 92, 188, 179, 108, 103, 28, 82, 18, 241, 88, 116, 2, 161, 130, 192, 184, 143, 192, 93, 102, 16, 86, 205, 31, 81, 87, 142, 99, 233, 18, 192, 137, 32, 14, 200, 20, 7, 108, 7, 246, 134, 13, 53, 145, 239, 204, 237, 247, 255, 250, 119, 127, 35, 123, 224, 129, 7, 210, 189, 253, 125, 61, 85, 137, 6, 42, 213, 74, 79, 163, 90, 201, 132, 181, 122, 50, 22, 139, 39, 162, 88, 44, 129, 101, 16, 143, 199, 18, 81, 24, 166, 98, 49, 73, 133, 97, 61, 209, 108, 69, 201, 176, 21, 165, 195, 86, 213, 143, 162, 40, 136, 66, 241, 98, 81, 60, 1, 224, 39, 90, 210, 244, 99, 113, 196, 91, 226, 97, 223, 108, 47, 241, 72, 194, 0, 75, 20, 69, 154, 161, 7, 119, 100, 88, 139, 69, 177, 40, 106, 33, 183, 217, 140, 123, 141, 16, 149, 122, 113, 191, 22, 69, 177, 178, 239, 123, 37, 63, 153, 24, 197, 126, 119, 245, 244, 245, 63, 186, 229, 180, 51, 119, 127, 255, 171, 95, 46, 162, 130, 46, 89, 28, 3, 233, 18, 192, 241, 42, 14, 78, 46, 16, 30, 45, 145, 119, 253, 197, 7, 50, 119, 93, 115, 109, 127, 173, 92, 92, 83, 42, 204, 110, 108, 53, 155, 235, 91, 205, 198, 166, 90, 163, 177, 190, 90, 169, 174, 147, 48, 54, 228, 197, 164, 23, 183, 54, 89, 15, 107, 105, 108, 26, 196, 128, 182, 120, 196, 219, 140, 96, 254, 128, 209, 152, 248, 80, 219, 172, 58, 6, 56, 199, 226, 190, 196, 17, 60, 47, 68, 220, 19, 223, 11, 176, 14, 108, 179, 60, 202, 197, 227, 113, 137, 99, 27, 254, 176, 177, 150, 167, 196, 89, 153, 21, 205, 131, 0, 240, 202, 12, 33, 66, 173, 21, 74, 171, 5, 10, 169, 213, 37, 108, 134, 82, 175, 213, 164, 214, 168, 105, 154, 23, 139, 87, 130, 32, 121, 56, 22, 143, 29, 14, 91, 173, 50, 8, 169, 17, 15, 130, 38, 200, 166, 138, 106, 42, 190, 31, 148, 25, 71, 93, 5, 228, 77, 249, 129, 95, 72, 36, 18, 197, 100, 38, 59, 135, 109, 10, 141, 102, 173, 240, 250, 215, 253, 183, 71, 255, 249, 239, 254, 186, 169, 59, 238, 202, 79, 44, 93, 2, 56, 222, 132, 136, 196, 125, 249, 153, 183, 188, 185, 127, 223, 158, 253, 67, 126, 179, 178, 105, 110, 118, 102, 115, 185, 80, 216, 2, 156, 158, 14, 32, 109, 172, 54, 194, 181, 205, 86, 216, 3, 28, 38, 130, 32, 144, 68, 50, 33, 137, 108, 74, 242, 61, 189, 178, 114, 112, 72, 178, 233, 156, 244, 246, 247, 75, 255, 138, 30, 241, 83, 73, 137, 251, 129, 228, 114, 105, 5, 113, 204, 135, 82, 7, 162, 61, 207, 147, 116, 38, 3, 160, 10, 43, 193, 58, 9, 192, 164, 199, 97, 220, 199, 185, 196, 58, 193, 78, 224, 115, 29, 173, 5, 193, 28, 160, 71, 114, 192, 193, 206, 27, 0, 243, 210, 8, 155, 32, 129, 80, 0, 90, 105, 33, 132, 32, 129, 102, 3, 4, 128, 120, 189, 90, 149, 114, 185, 44, 19, 195, 35, 178, 231, 209, 157, 50, 122, 120, 68, 102, 166, 166, 144, 111, 72, 163, 14, 114, 168, 213, 42, 90, 182, 85, 199, 182, 32, 147, 176, 137, 192, 122, 16, 88, 134, 1, 4, 70, 208, 183, 82, 185, 204, 175, 148, 102, 11, 159, 54, 123, 62, 178, 188, 253, 87, 127, 37, 245, 224, 131, 59, 122, 235, 141, 170, 207, 35, 173, 150, 171, 193, 138, 117, 235, 202, 219, 79, 219, 86, 248, 212, 255, 249, 7, 146, 141, 57, 169, 167, 161, 116, 9, 224, 24, 201, 99, 80, 158, 191, 251, 214, 159, 207, 31, 218, 189, 107, 77, 97, 122, 122, 69, 216, 172, 110, 43, 213, 106, 27, 106, 245, 250, 246, 86, 163, 185, 33, 172, 183, 214, 162, 125, 175, 8, 195, 40, 75, 64, 38, 146, 190, 12, 244, 245, 74, 223, 224, 128, 172, 92, 183, 81, 250, 7, 87, 200, 154, 245, 235, 176, 236, 151, 222, 161, 1, 73, 164, 82, 146, 202, 164, 1, 204, 184, 180, 154, 4, 9, 52, 49, 236, 248, 22, 192, 71, 0, 75, 216, 144, 38, 172, 0, 5, 58, 8, 128, 32, 106, 2, 168, 42, 196, 181, 130, 56, 210, 242, 4, 26, 133, 117, 104, 50, 29, 124, 228, 209, 18, 96, 85, 154, 72, 139, 2, 235, 62, 202, 128, 86, 172, 55, 18, 233, 190, 60, 237, 29, 64, 58, 204, 3, 80, 137, 198, 105, 13, 40, 112, 185, 45, 247, 135, 114, 17, 178, 34, 30, 99, 29, 199, 22, 162, 34, 238, 15, 235, 205, 70, 195, 16, 0, 45, 135, 122, 83, 170, 32, 141, 98, 169, 44, 229, 82, 73, 243, 234, 21, 198, 203, 50, 66, 18, 217, 185, 251, 174, 23, 188, 228, 213, 207, 252, 204, 71, 255, 190, 241, 226, 215, 188, 97, 96, 231, 253, 247, 111, 173, 148, 74, 219, 106, 149, 202, 86, 88, 21, 27, 154, 97, 125, 75, 163, 213, 88, 29, 243, 252, 126, 172, 123, 56, 6, 178, 72, 2, 71, 80, 196, 33, 204, 98, 89, 136, 251, 126, 41, 230, 199, 16, 151, 81, 63, 158, 152, 133, 155, 82, 192, 73, 148, 61, 63, 49, 13, 190, 27, 198, 33, 207, 12, 174, 93, 49, 187, 98, 104, 253, 212, 121, 231, 63, 99, 238, 163, 127, 243, 126, 58, 93, 39, 133, 116, 9, 224, 40, 202, 31, 254, 221, 31, 39, 190, 249, 165, 111, 101, 19, 94, 166, 191, 6, 223, 27, 38, 122, 170, 90, 40, 165, 26, 241, 86, 38, 72, 164, 251, 164, 217, 28, 108, 54, 42, 43, 195, 86, 124, 19, 252, 238, 45, 128, 205, 202, 102, 51, 90, 85, 171, 53, 115, 84, 176, 201, 108, 70, 210, 217, 172, 244, 244, 246, 202, 224, 208, 144, 12, 174, 28, 146, 181, 0, 251, 202, 85, 43, 37, 223, 219, 35, 61, 61, 89, 241, 19, 73, 137, 160, 197, 235, 4, 6, 205, 105, 152, 214, 181, 122, 85, 106, 213, 58, 192, 209, 84, 0, 51, 52, 161, 53, 41, 4, 163, 106, 117, 232, 62, 226, 58, 2, 208, 120, 195, 65, 44, 10, 200, 197, 98, 180, 44, 1, 11, 87, 0, 235, 180, 22, 144, 162, 121, 174, 56, 107, 136, 136, 94, 72, 76, 173, 2, 236, 75, 193, 13, 184, 211, 202, 48, 57, 26, 39, 121, 25, 92, 131, 88, 152, 3, 80, 55, 194, 186, 150, 229, 177, 249, 176, 36, 60, 250, 17, 190, 15, 183, 131, 113, 79, 247, 201, 189, 40, 86, 177, 93, 131, 174, 3, 72, 130, 199, 171, 100, 152, 72, 200, 0, 136, 111, 160, 63, 39, 95, 250, 212, 103, 154, 55, 95, 117, 213, 127, 122, 190, 191, 178, 81, 173, 157, 10, 130, 91, 159, 74, 103, 96, 24, 37, 37, 7, 194, 76, 224, 122, 54, 234, 53, 217, 253, 224, 125, 114, 217, 11, 95, 36, 27, 182, 110, 150, 225, 3, 7, 164, 92, 44, 9, 136, 66, 74, 165, 34, 150, 21, 236, 7, 215, 179, 76, 130, 105, 74, 3, 129, 75, 158, 3, 3, 136, 2, 39, 24, 22, 193, 128, 179, 32, 174, 41, 236, 107, 4, 22, 209, 112, 24, 53, 198, 7, 87, 173, 249, 167, 195, 187, 247, 140, 160, 224, 9, 41, 93, 2, 120, 138, 178, 97, 227, 186, 115, 102, 103, 102, 222, 130, 198, 190, 29, 254, 234, 150, 82, 169, 60, 0, 221, 151, 67, 179, 73, 195, 92, 14, 224, 71, 251, 17, 212, 8, 47, 50, 219, 121, 16, 192, 110, 79, 230, 36, 147, 78, 200, 196, 216, 136, 36, 83, 89, 121, 198, 11, 95, 42, 91, 207, 216, 38, 131, 171, 86, 72, 111, 95, 191, 120, 52, 201, 17, 90, 0, 113, 173, 210, 80, 160, 151, 203, 48, 157, 75, 53, 128, 161, 169, 141, 149, 245, 17, 16, 4, 9, 91, 168, 54, 84, 253, 49, 138, 255, 64, 157, 154, 240, 42, 92, 33, 8, 25, 69, 92, 55, 102, 49, 45, 125, 68, 33, 1, 216, 202, 20, 192, 220, 32, 66, 83, 193, 127, 221, 150, 85, 153, 12, 156, 27, 206, 79, 201, 132, 22, 134, 15, 11, 4, 228, 66, 141, 207, 210, 230, 136, 24, 35, 17, 176, 12, 221, 3, 115, 0, 4, 186, 198, 104, 230, 155, 152, 169, 199, 10, 157, 11, 38, 135, 168, 87, 83, 113, 188, 204, 111, 224, 186, 244, 247, 230, 229, 236, 179, 54, 203, 53, 223, 186, 82, 174, 248, 183, 79, 105, 185, 243, 159, 123, 185, 108, 56, 237, 12, 73, 194, 181, 57, 188, 115, 135, 108, 58, 235, 28, 169, 204, 205, 202, 181, 95, 189, 2, 132, 58, 32, 191, 244, 187, 191, 3, 96, 99, 123, 144, 80, 58, 147, 212, 123, 192, 142, 149, 38, 174, 105, 12, 251, 168, 149, 43, 82, 129, 123, 210, 128, 149, 65, 107, 131, 196, 48, 62, 50, 42, 83, 147, 83, 82, 42, 20, 165, 92, 152, 67, 40, 32, 62, 39, 115, 51, 211, 176, 168, 90, 210, 55, 208, 127, 217, 228, 240, 248, 237, 60, 188, 19, 81, 186, 4, 240, 20, 100, 203, 246, 173, 207, 26, 63, 120, 240, 107, 65, 34, 49, 216, 75, 141, 189, 122, 181, 12, 173, 24, 82, 13, 30, 163, 166, 130, 73, 158, 134, 86, 15, 208, 208, 115, 208, 68, 177, 0, 90, 14, 62, 121, 174, 167, 135, 61, 115, 242, 193, 63, 249, 83, 57, 255, 153, 207, 146, 23, 191, 238, 13, 104, 120, 6, 228, 213, 106, 13, 218, 14, 218, 28, 141, 148, 218, 79, 241, 160, 216, 32, 8, 0, 59, 172, 211, 132, 159, 7, 47, 32, 134, 232, 60, 94, 9, 18, 19, 119, 248, 167, 230, 103, 156, 85, 209, 210, 54, 245, 217, 242, 16, 87, 149, 3, 159, 254, 215, 52, 130, 30, 208, 68, 1, 103, 194, 147, 204, 116, 91, 187, 81, 8, 75, 128, 32, 119, 22, 188, 135, 100, 102, 181, 0, 14, 17, 104, 209, 136, 125, 7, 190, 86, 71, 107, 33, 68, 58, 120, 17, 249, 44, 205, 128, 28, 150, 165, 214, 103, 26, 226, 36, 23, 205, 198, 118, 42, 90, 23, 19, 140, 192, 192, 145, 100, 16, 151, 51, 207, 60, 85, 14, 237, 219, 35, 255, 244, 103, 127, 38, 173, 90, 83, 46, 121, 241, 203, 100, 235, 217, 231, 75, 105, 102, 74, 102, 39, 198, 36, 213, 211, 39, 183, 126, 239, 155, 178, 114, 229, 74, 249, 185, 119, 255, 154, 20, 43, 117, 25, 57, 60, 1, 224, 194, 45, 130, 69, 148, 203, 164, 36, 147, 74, 72, 42, 157, 150, 20, 238, 75, 50, 153, 2, 33, 39, 36, 65, 146, 134, 69, 66, 203, 137, 251, 229, 189, 104, 0, 38, 236, 203, 104, 129, 44, 72, 24, 255, 241, 161, 15, 201, 35, 247, 221, 187, 235, 25, 207, 123, 206, 197, 55, 125, 255, 7, 116, 31, 78, 72, 233, 18, 192, 19, 200, 45, 211, 53, 239, 29, 207, 185, 240, 84, 248, 160, 107, 27, 181, 202, 184, 151, 74, 247, 85, 11, 115, 155, 97, 66, 195, 47, 111, 85, 250, 6, 135, 134, 39, 198, 70, 127, 21, 26, 248, 37, 191, 240, 219, 191, 25, 13, 174, 94, 3, 55, 211, 147, 0, 62, 54, 77, 72, 106, 234, 122, 211, 248, 178, 97, 11, 141, 8, 235, 244, 105, 217, 224, 25, 63, 229, 148, 245, 114, 253, 183, 190, 33, 125, 32, 140, 117, 155, 78, 131, 73, 90, 6, 105, 16, 204, 246, 118, 96, 65, 127, 186, 221, 236, 21, 251, 6, 48, 70, 171, 106, 84, 123, 247, 185, 73, 132, 165, 224, 86, 50, 135, 117, 112, 73, 224, 59, 209, 14, 125, 22, 81, 211, 223, 164, 177, 140, 214, 200, 116, 68, 148, 36, 44, 200, 153, 110, 138, 217, 194, 204, 195, 121, 168, 197, 65, 193, 54, 238, 248, 168, 231, 9, 74, 130, 159, 233, 138, 99, 148, 139, 160, 93, 149, 84, 98, 129, 30, 147, 27, 49, 224, 113, 113, 59, 37, 35, 10, 202, 48, 79, 215, 21, 236, 76, 34, 221, 89, 6, 211, 125, 50, 147, 21, 112, 137, 116, 88, 65, 155, 55, 175, 147, 116, 42, 144, 191, 252, 95, 127, 40, 51, 163, 227, 208, 252, 41, 217, 118, 206, 69, 0, 174, 47, 7, 119, 63, 38, 149, 226, 156, 92, 112, 217, 101, 242, 194, 215, 190, 94, 198, 166, 103, 101, 122, 106, 86, 143, 131, 103, 231, 174, 51, 239, 133, 158, 7, 246, 7, 34, 151, 12, 44, 131, 32, 233, 75, 38, 153, 196, 245, 240, 36, 141, 101, 50, 17, 131, 235, 149, 214, 50, 9, 172, 243, 158, 126, 236, 255, 251, 115, 217, 191, 111, 207, 119, 235, 229, 234, 203, 181, 162, 19, 84, 186, 4, 208, 33, 47, 126, 253, 155, 250, 30, 187, 255, 158, 243, 231, 38, 166, 94, 88, 175, 20, 46, 71, 210, 185, 113, 63, 200, 75, 171, 81, 79, 247, 244, 199, 1, 126, 95, 129, 0, 128, 52, 42, 240, 191, 43, 85, 57, 243, 162, 11, 229, 181, 239, 248, 69, 57, 176, 239, 32, 174, 38, 27, 23, 1, 9, 180, 161, 92, 156, 190, 49, 5, 87, 216, 129, 214, 128, 51, 38, 3, 3, 253, 50, 117, 96, 47, 48, 235, 75, 182, 127, 5, 76, 211, 186, 110, 63, 111, 181, 27, 45, 207, 94, 112, 179, 13, 155, 62, 254, 19, 80, 42, 72, 35, 232, 53, 13, 219, 81, 211, 210, 47, 39, 9, 104, 239, 154, 1, 26, 253, 108, 54, 120, 211, 232, 205, 150, 10, 37, 27, 39, 33, 16, 116, 172, 202, 12, 7, 98, 91, 86, 9, 248, 169, 197, 129, 115, 53, 238, 0, 203, 57, 240, 32, 138, 227, 136, 1, 181, 212, 234, 60, 36, 18, 16, 215, 9, 124, 3, 100, 238, 131, 230, 54, 151, 230, 248, 249, 143, 213, 51, 159, 235, 92, 186, 184, 30, 3, 98, 230, 244, 240, 207, 252, 225, 31, 217, 66, 43, 65, 174, 135, 67, 48, 214, 70, 171, 213, 148, 158, 124, 86, 182, 109, 219, 36, 223, 252, 175, 47, 201, 247, 175, 248, 18, 75, 43, 137, 174, 88, 181, 90, 78, 61, 109, 187, 92, 122, 249, 11, 36, 63, 184, 74, 14, 30, 30, 151, 10, 44, 43, 214, 168, 199, 205, 58, 72, 204, 8, 92, 159, 119, 149, 56, 34, 146, 128, 27, 131, 253, 48, 31, 251, 78, 37, 205, 168, 9, 167, 53, 100, 51, 9, 217, 112, 202, 70, 88, 17, 135, 229, 95, 255, 234, 47, 225, 50, 84, 63, 88, 169, 148, 126, 199, 110, 122, 66, 202, 211, 154, 0, 254, 230, 227, 255, 145, 248, 183, 15, 254, 229, 182, 153, 185, 194, 37, 149, 98, 241, 242, 122, 173, 250, 44, 52, 142, 45, 3, 67, 67, 177, 245, 155, 55, 200, 246, 115, 207, 147, 85, 107, 86, 201, 103, 62, 246, 49, 217, 116, 218, 217, 114, 250, 133, 23, 105, 163, 65, 179, 149, 194, 244, 140, 92, 255, 141, 175, 201, 139, 94, 245, 42, 217, 140, 114, 19, 227, 83, 170, 49, 216, 184, 180, 177, 170, 186, 179, 4, 0, 161, 217, 238, 132, 77, 57, 157, 74, 75, 188, 89, 129, 102, 78, 72, 29, 108, 65, 243, 146, 229, 9, 49, 211, 32, 3, 69, 128, 18, 0, 34, 220, 70, 181, 60, 3, 139, 182, 227, 113, 128, 31, 100, 65, 2, 8, 17, 90, 88, 135, 198, 37, 17, 24, 48, 233, 225, 240, 63, 106, 33, 144, 240, 135, 234, 217, 240, 117, 69, 1, 203, 14, 47, 128, 195, 18, 64, 204, 67, 30, 45, 6, 78, 247, 129, 25, 15, 251, 133, 185, 122, 126, 45, 28, 15, 55, 51, 202, 25, 229, 216, 9, 168, 75, 179, 63, 154, 248, 172, 153, 151, 137, 151, 192, 197, 245, 178, 112, 19, 8, 15, 27, 188, 164, 64, 102, 95, 165, 146, 10, 129, 174, 98, 234, 97, 154, 22, 52, 39, 139, 128, 13, 116, 31, 36, 36, 214, 201, 244, 152, 108, 218, 180, 14, 150, 71, 40, 119, 221, 114, 171, 4, 129, 39, 27, 112, 223, 86, 175, 91, 135, 115, 240, 101, 100, 116, 82, 134, 71, 38, 117, 27, 189, 47, 172, 9, 113, 173, 3, 219, 155, 107, 96, 210, 56, 156, 218, 132, 53, 192, 251, 66, 211, 159, 249, 60, 198, 100, 2, 238, 11, 8, 97, 110, 174, 36, 131, 253, 121, 217, 188, 113, 147, 220, 127, 207, 221, 242, 201, 191, 255, 123, 25, 24, 92, 245, 158, 177, 145, 3, 31, 214, 74, 78, 80, 121, 218, 17, 192, 175, 254, 233, 223, 164, 174, 252, 252, 167, 46, 152, 153, 157, 124, 105, 165, 88, 120, 89, 212, 104, 157, 231, 251, 65, 122, 245, 250, 117, 178, 253, 156, 179, 101, 219, 89, 103, 201, 170, 181, 107, 129, 35, 95, 230, 10, 5, 48, 254, 58, 185, 251, 230, 155, 228, 75, 159, 252, 148, 188, 232, 103, 223, 32, 107, 54, 109, 70, 187, 244, 100, 223, 35, 15, 202, 141, 48, 225, 255, 199, 31, 252, 190, 212, 80, 182, 90, 105, 24, 92, 160, 209, 112, 248, 139, 141, 149, 63, 254, 41, 160, 209, 214, 52, 197, 54, 98, 230, 247, 247, 100, 181, 177, 205, 22, 202, 38, 95, 193, 4, 81, 237, 15, 255, 23, 0, 96, 7, 25, 77, 111, 5, 58, 2, 241, 16, 113, 105, 193, 47, 45, 79, 211, 99, 0, 127, 172, 5, 83, 91, 215, 73, 30, 200, 231, 246, 252, 1, 92, 202, 63, 212, 226, 4, 2, 151, 42, 102, 217, 130, 57, 29, 97, 223, 116, 19, 34, 218, 241, 30, 118, 18, 199, 54, 1, 182, 133, 21, 19, 113, 200, 61, 14, 34, 66, 1, 227, 30, 144, 4, 72, 14, 252, 33, 14, 162, 112, 117, 182, 184, 174, 0, 195, 10, 147, 80, 29, 133, 81, 229, 68, 23, 215, 107, 132, 242, 56, 55, 229, 190, 78, 2, 192, 127, 3, 78, 20, 80, 237, 143, 131, 55, 172, 133, 63, 28, 15, 178, 72, 137, 60, 47, 250, 238, 36, 129, 68, 34, 0, 88, 1, 226, 122, 93, 14, 143, 78, 200, 232, 216, 148, 212, 106, 13, 221, 15, 127, 60, 106, 238, 222, 213, 77, 119, 204, 144, 136, 17, 222, 23, 166, 145, 0, 146, 48, 243, 25, 79, 4, 236, 208, 12, 37, 155, 77, 233, 48, 100, 111, 46, 39, 91, 183, 108, 150, 27, 175, 190, 70, 254, 243, 95, 62, 6, 75, 227, 148, 151, 140, 14, 239, 187, 218, 86, 113, 66, 202, 211, 130, 0, 126, 255, 239, 254, 54, 184, 226, 147, 255, 113, 97, 113, 114, 234, 213, 181, 98, 241, 229, 149, 90, 249, 108, 9, 98, 193, 218, 245, 167, 200, 57, 23, 94, 32, 231, 192, 140, 95, 5, 173, 209, 68, 123, 152, 152, 154, 147, 169, 153, 89, 29, 118, 162, 182, 203, 102, 211, 114, 209, 57, 167, 201, 23, 63, 253, 25, 185, 233, 234, 171, 100, 235, 25, 103, 203, 154, 173, 167, 203, 93, 87, 125, 91, 205, 235, 95, 252, 159, 127, 32, 19, 51, 37, 94, 72, 221, 23, 77, 80, 29, 11, 7, 88, 84, 235, 179, 157, 3, 136, 218, 113, 199, 198, 104, 9, 128, 141, 47, 15, 19, 150, 173, 185, 84, 174, 34, 61, 14, 77, 6, 4, 106, 115, 133, 80, 155, 227, 214, 120, 240, 58, 154, 117, 104, 43, 224, 192, 107, 18, 4, 62, 226, 0, 34, 48, 25, 35, 1, 52, 45, 1, 52, 0, 254, 102, 18, 1, 26, 139, 24, 100, 154, 37, 1, 5, 39, 0, 77, 48, 19, 133, 122, 12, 220, 135, 3, 29, 138, 132, 32, 128, 152, 15, 144, 32, 68, 40, 43, 62, 0, 226, 1, 244, 36, 3, 44, 21, 248, 113, 67, 4, 172, 139, 161, 5, 75, 129, 22, 130, 89, 231, 174, 144, 22, 113, 54, 0, 214, 245, 36, 176, 23, 68, 117, 193, 117, 30, 10, 3, 179, 24, 81, 51, 162, 165, 86, 0, 181, 58, 197, 28, 151, 70, 113, 120, 60, 73, 154, 34, 40, 75, 212, 115, 157, 23, 130, 53, 120, 1, 138, 17, 200, 13, 201, 229, 122, 244, 90, 134, 205, 134, 76, 207, 206, 73, 25, 100, 172, 149, 176, 30, 20, 5, 117, 178, 186, 182, 117, 66, 113, 36, 192, 165, 33, 27, 35, 140, 147, 4, 184, 207, 76, 138, 157, 149, 77, 73, 194, 45, 104, 242, 126, 101, 179, 178, 21, 22, 198, 215, 63, 247, 159, 242, 221, 47, 127, 169, 176, 233, 204, 179, 159, 177, 235, 254, 123, 31, 177, 155, 158, 144, 114, 82, 19, 192, 182, 139, 46, 220, 60, 59, 54, 250, 186, 226, 236, 204, 27, 106, 165, 234, 133, 28, 28, 94, 185, 102, 141, 156, 247, 140, 75, 228, 236, 139, 46, 144, 1, 152, 247, 17, 76, 231, 41, 128, 126, 122, 102, 78, 199, 210, 181, 49, 88, 109, 70, 101, 72, 168, 228, 178, 25, 233, 235, 233, 145, 71, 239, 191, 79, 174, 250, 234, 151, 165, 0, 130, 64, 65, 217, 126, 246, 89, 242, 242, 183, 189, 29, 132, 81, 80, 208, 83, 253, 155, 109, 128, 31, 152, 160, 182, 221, 193, 154, 6, 72, 213, 172, 159, 23, 142, 199, 83, 107, 177, 108, 179, 73, 19, 149, 8, 50, 187, 54, 38, 125, 2, 184, 10, 128, 189, 132, 132, 117, 52, 214, 42, 0, 220, 128, 150, 7, 192, 9, 122, 105, 98, 75, 130, 191, 197, 117, 132, 6, 124, 85, 37, 1, 16, 0, 243, 113, 91, 105, 21, 232, 190, 44, 1, 56, 34, 48, 231, 135, 237, 149, 28, 72, 8, 72, 3, 152, 99, 62, 52, 58, 193, 238, 1, 104, 32, 128, 8, 129, 75, 130, 158, 164, 16, 249, 72, 71, 94, 72, 141, 239, 1, 232, 44, 3, 130, 8, 145, 22, 97, 59, 146, 8, 118, 170, 164, 65, 203, 128, 251, 13, 73, 58, 4, 56, 206, 151, 254, 187, 42, 126, 254, 211, 139, 67, 118, 160, 5, 66, 115, 92, 163, 186, 180, 81, 0, 214, 30, 39, 133, 100, 165, 235, 38, 141, 238, 8, 171, 161, 139, 68, 3, 73, 45, 37, 164, 107, 64, 185, 54, 168, 221, 66, 215, 177, 31, 198, 53, 197, 164, 145, 136, 93, 121, 183, 13, 9, 146, 150, 89, 30, 154, 95, 103, 69, 98, 157, 67, 135, 171, 86, 12, 202, 154, 53, 43, 229, 147, 255, 231, 131, 114, 215, 77, 183, 236, 188, 244, 101, 175, 190, 232, 150, 111, 127, 165, 160, 27, 157, 160, 114, 210, 17, 192, 43, 223, 249, 206, 244, 157, 223, 251, 222, 115, 10, 133, 153, 159, 175, 149, 43, 175, 196, 45, 29, 28, 90, 185, 86, 78, 59, 251, 12, 0, 255, 82, 89, 185, 110, 45, 26, 112, 32, 179, 0, 109, 177, 88, 150, 58, 204, 68, 222, 120, 54, 74, 5, 177, 10, 26, 3, 254, 107, 51, 69, 59, 101, 227, 101, 87, 218, 192, 80, 191, 52, 202, 69, 185, 241, 219, 223, 146, 199, 238, 191, 87, 158, 241, 210, 151, 203, 25, 23, 95, 172, 29, 76, 158, 206, 163, 7, 212, 9, 100, 221, 214, 104, 90, 214, 235, 217, 122, 249, 95, 235, 133, 86, 99, 67, 101, 73, 150, 211, 14, 52, 6, 222, 10, 248, 240, 212, 250, 241, 16, 64, 38, 160, 27, 41, 4, 0, 188, 6, 205, 95, 69, 168, 97, 219, 6, 202, 130, 12, 162, 6, 106, 64, 30, 203, 73, 147, 101, 13, 57, 24, 2, 224, 129, 35, 160, 74, 238, 94, 181, 50, 129, 203, 192, 163, 224, 177, 217, 99, 226, 129, 40, 255, 40, 248, 129, 86, 2, 26, 96, 143, 2, 75, 4, 0, 180, 174, 147, 8, 2, 92, 47, 150, 35, 49, 36, 16, 15, 0, 124, 191, 142, 0, 173, 139, 242, 234, 50, 160, 60, 251, 15, 104, 49, 176, 63, 146, 132, 67, 115, 157, 29, 119, 220, 35, 253, 115, 238, 87, 205, 121, 254, 8, 68, 119, 52, 40, 71, 99, 138, 71, 73, 113, 203, 121, 49, 214, 148, 114, 136, 110, 171, 53, 153, 114, 4, 48, 2, 231, 33, 80, 115, 83, 76, 39, 39, 137, 129, 75, 102, 107, 73, 30, 133, 238, 78, 103, 67, 26, 30, 194, 194, 70, 32, 36, 6, 14, 19, 150, 209, 62, 50, 73, 95, 146, 169, 148, 172, 4, 1, 12, 12, 244, 201, 7, 63, 240, 1, 217, 183, 103, 247, 213, 205, 106, 253, 165, 40, 106, 42, 60, 65, 229, 164, 33, 128, 181, 91, 54, 159, 49, 51, 57, 254, 250, 90, 173, 250, 134, 168, 17, 158, 239, 37, 124, 217, 114, 218, 233, 114, 241, 115, 159, 35, 219, 78, 63, 29, 13, 209, 87, 45, 63, 87, 174, 72, 163, 209, 82, 45, 75, 173, 108, 26, 210, 19, 8, 219, 136, 205, 103, 59, 102, 35, 230, 148, 220, 108, 38, 45, 181, 194, 140, 164, 243, 189, 48, 55, 57, 149, 156, 229, 72, 0, 160, 9, 219, 134, 72, 6, 174, 145, 113, 91, 211, 250, 176, 32, 240, 64, 0, 36, 7, 213, 192, 170, 169, 233, 195, 227, 88, 160, 205, 9, 124, 79, 53, 58, 65, 15, 173, 94, 75, 137, 84, 177, 68, 144, 42, 128, 94, 39, 208, 9, 126, 132, 58, 246, 71, 18, 176, 229, 153, 46, 168, 35, 6, 18, 81, 155, 155, 150, 4, 126, 218, 248, 169, 245, 217, 185, 199, 67, 66, 104, 139, 30, 19, 254, 41, 57, 0, 232, 109, 2, 192, 210, 6, 2, 90, 221, 0, 196, 9, 118, 5, 58, 192, 47, 201, 134, 132, 136, 183, 130, 154, 198, 153, 22, 129, 8, 218, 100, 129, 122, 180, 43, 67, 173, 14, 106, 95, 64, 12, 0, 52, 125, 18, 230, 32, 24, 215, 52, 92, 37, 227, 50, 89, 211, 92, 51, 181, 136, 30, 175, 6, 148, 225, 179, 7, 122, 70, 216, 206, 102, 119, 8, 53, 186, 158, 173, 198, 9, 118, 246, 49, 180, 203, 33, 162, 100, 175, 23, 193, 196, 233, 230, 177, 183, 159, 22, 89, 155, 28, 236, 5, 162, 246, 231, 188, 140, 84, 50, 0, 240, 123, 100, 197, 138, 33, 92, 166, 80, 254, 254, 127, 189, 87, 170, 141, 250, 199, 203, 179, 115, 191, 166, 5, 79, 96, 57, 161, 9, 96, 219, 165, 23, 14, 13, 239, 218, 253, 178, 70, 169, 246, 223, 154, 141, 250, 229, 169, 108, 58, 187, 105, 251, 169, 50, 114, 240, 144, 60, 239, 21, 175, 148, 115, 159, 113, 25, 52, 61, 64, 95, 172, 234, 148, 89, 79, 65, 0, 80, 80, 131, 168, 106, 106, 55, 13, 182, 7, 8, 214, 113, 243, 53, 149, 255, 80, 190, 221, 24, 180, 245, 104, 68, 67, 50, 145, 48, 218, 30, 13, 130, 194, 89, 114, 74, 0, 40, 67, 98, 209, 125, 176, 145, 179, 193, 105, 133, 16, 214, 5, 208, 199, 65, 0, 132, 38, 1, 26, 39, 88, 161, 177, 61, 106, 252, 38, 192, 94, 71, 3, 135, 182, 143, 85, 225, 207, 43, 232, 225, 143, 86, 64, 0, 149, 36, 8, 97, 158, 0, 68, 193, 110, 172, 128, 136, 26, 159, 253, 3, 92, 42, 248, 73, 40, 60, 110, 146, 13, 118, 163, 251, 102, 195, 135, 32, 219, 166, 180, 69, 215, 156, 107, 64, 109, 175, 128, 7, 120, 53, 14, 18, 112, 196, 16, 112, 9, 112, 43, 1, 128, 8, 146, 208, 252, 36, 129, 68, 77, 162, 20, 214, 17, 24, 15, 97, 37, 72, 135, 5, 161, 65, 235, 196, 49, 224, 16, 213, 23, 183, 215, 141, 194, 253, 115, 182, 159, 198, 112, 189, 104, 214, 155, 131, 157, 63, 78, 94, 79, 146, 172, 186, 5, 208, 206, 134, 52, 22, 138, 33, 151, 249, 84, 85, 254, 10, 112, 166, 185, 11, 49, 47, 172, 193, 184, 0, 176, 78, 184, 228, 6, 144, 246, 61, 199, 125, 108, 194, 106, 33, 33, 13, 12, 244, 202, 246, 109, 155, 101, 114, 100, 68, 254, 207, 251, 222, 47, 65, 50, 249, 63, 103, 167, 166, 254, 94, 11, 158, 192, 114, 194, 17, 192, 175, 189, 255, 143, 252, 175, 125, 250, 51, 151, 206, 205, 205, 190, 185, 92, 44, 254, 44, 252, 204, 13, 167, 108, 220, 36, 231, 94, 124, 137, 92, 240, 204, 103, 72, 165, 82, 150, 171, 190, 249, 45, 121, 230, 75, 95, 35, 179, 115, 115, 106, 218, 114, 98, 8, 239, 41, 129, 71, 209, 19, 182, 103, 237, 218, 134, 241, 221, 153, 104, 12, 65, 3, 23, 215, 98, 0, 90, 52, 18, 214, 69, 149, 22, 131, 11, 225, 67, 99, 210, 61, 208, 182, 130, 98, 220, 150, 13, 199, 128, 159, 137, 76, 179, 120, 107, 239, 23, 191, 136, 157, 118, 172, 143, 96, 5, 216, 161, 185, 3, 248, 250, 177, 58, 192, 206, 0, 176, 171, 198, 7, 224, 163, 50, 227, 25, 196, 105, 9, 48, 64, 157, 214, 225, 6, 80, 251, 59, 176, 187, 160, 38, 63, 246, 136, 184, 59, 22, 250, 222, 238, 20, 20, 95, 122, 88, 182, 145, 99, 115, 246, 250, 183, 128, 91, 166, 43, 156, 172, 21, 48, 191, 52, 65, 53, 184, 2, 152, 1, 224, 133, 217, 175, 174, 0, 8, 64, 53, 127, 18, 160, 79, 145, 4, 170, 38, 45, 5, 82, 192, 82, 227, 9, 146, 128, 37, 3, 110, 135, 58, 116, 234, 48, 251, 11, 40, 188, 1, 216, 63, 59, 96, 85, 171, 91, 0, 155, 222, 126, 36, 234, 49, 67, 120, 62, 106, 78, 32, 78, 176, 170, 53, 97, 51, 205, 38, 122, 14, 6, 236, 70, 24, 211, 217, 148, 74, 2, 174, 34, 136, 189, 63, 6, 252, 166, 60, 227, 28, 138, 229, 207, 221, 63, 39, 180, 76, 130, 132, 39, 231, 159, 119, 166, 220, 127, 231, 157, 242, 111, 255, 248, 15, 178, 122, 237, 41, 175, 58, 188, 111, 239, 183, 109, 145, 19, 86, 78, 8, 2, 248, 215, 207, 124, 201, 255, 147, 247, 253, 193, 217, 51, 211, 227, 63, 83, 47, 85, 94, 135, 164, 11, 251, 250, 7, 188, 115, 46, 186, 88, 182, 158, 117, 166, 244, 172, 94, 35, 213, 122, 83, 210, 201, 132, 60, 116, 215, 237, 146, 239, 237, 147, 32, 219, 167, 0, 165, 41, 110, 144, 203, 27, 137, 184, 149, 246, 61, 182, 75, 130, 198, 152, 160, 72, 160, 6, 130, 176, 65, 161, 25, 106, 30, 148, 187, 221, 8, 62, 58, 144, 195, 103, 233, 181, 90, 182, 99, 36, 155, 135, 88, 44, 240, 208, 166, 216, 159, 160, 197, 17, 199, 214, 216, 10, 192, 164, 198, 23, 231, 223, 227, 88, 232, 227, 67, 163, 251, 4, 126, 21, 218, 223, 129, 191, 12, 109, 175, 4, 224, 172, 0, 164, 161, 156, 246, 3, 168, 182, 71, 176, 96, 167, 229, 65, 116, 171, 198, 199, 50, 191, 50, 38, 131, 219, 226, 210, 179, 33, 38, 201, 124, 76, 210, 67, 34, 185, 85, 34, 89, 164, 251, 48, 38, 106, 179, 145, 148, 38, 68, 74, 163, 88, 142, 99, 55, 147, 145, 212, 139, 216, 20, 64, 168, 78, 139, 76, 237, 13, 165, 138, 50, 74, 2, 180, 8, 8, 126, 93, 226, 106, 88, 34, 136, 209, 45, 0, 168, 9, 110, 227, 10, 0, 236, 212, 254, 201, 42, 136, 0, 241, 52, 92, 2, 37, 4, 99, 25, 68, 105, 4, 88, 12, 36, 129, 56, 173, 7, 59, 210, 192, 171, 75, 161, 230, 229, 101, 228, 117, 228, 131, 76, 122, 209, 248, 223, 90, 9, 166, 28, 47, 166, 57, 71, 115, 97, 1, 85, 53, 247, 153, 198, 82, 72, 193, 57, 240, 231, 196, 113, 1, 45, 10, 237, 11, 112, 162, 219, 51, 223, 88, 0, 78, 232, 10, 80, 219, 119, 10, 215, 83, 156, 179, 129, 123, 125, 209, 69, 231, 200, 213, 95, 253, 154, 124, 253, 115, 159, 45, 157, 254, 140, 203, 158, 249, 224, 141, 55, 62, 96, 139, 157, 176, 114, 92, 19, 192, 115, 95, 253, 134, 204, 189, 183, 95, 255, 223, 203, 211, 51, 111, 141, 90, 225, 5, 240, 201, 18, 91, 225, 215, 159, 251, 236, 103, 203, 218, 205, 27, 165, 218, 140, 164, 56, 91, 132, 31, 14, 63, 45, 149, 144, 181, 107, 87, 202, 248, 240, 132, 246, 216, 198, 180, 67, 110, 161, 70, 230, 141, 119, 81, 211, 78, 0, 72, 237, 185, 54, 90, 211, 8, 51, 92, 121, 6, 14, 41, 113, 83, 0, 81, 203, 67, 243, 35, 240, 177, 91, 21, 20, 215, 142, 62, 250, 252, 220, 159, 110, 110, 235, 35, 48, 177, 77, 0, 226, 97, 199, 158, 78, 214, 161, 143, 174, 218, 30, 218, 12, 62, 61, 53, 125, 28, 224, 143, 211, 196, 87, 224, 35, 143, 154, 223, 153, 252, 0, 127, 196, 178, 236, 19, 80, 179, 31, 251, 101, 127, 1, 246, 57, 116, 90, 92, 214, 95, 26, 147, 141, 207, 139, 43, 232, 7, 182, 1, 139, 61, 238, 60, 142, 44, 173, 6, 142, 31, 213, 56, 31, 220, 201, 212, 206, 72, 201, 128, 22, 65, 173, 32, 178, 239, 250, 80, 118, 94, 21, 202, 248, 195, 145, 248, 41, 206, 124, 196, 137, 41, 41, 128, 0, 64, 2, 234, 18, 208, 180, 87, 119, 128, 86, 0, 2, 193, 78, 50, 0, 1, 68, 105, 88, 3, 92, 102, 64, 6, 25, 18, 3, 2, 44, 133, 24, 202, 73, 2, 219, 147, 68, 116, 142, 129, 1, 188, 3, 174, 154, 237, 0, 38, 175, 37, 173, 0, 118, 28, 234, 253, 107, 31, 46, 173, 57, 206, 228, 195, 126, 213, 74, 224, 54, 230, 94, 176, 243, 207, 220, 191, 121, 97, 62, 235, 112, 100, 98, 132, 149, 153, 10, 219, 36, 163, 245, 224, 252, 181, 255, 1, 43, 182, 154, 70, 179, 161, 247, 150, 179, 14, 207, 63, 255, 44, 249, 247, 15, 126, 72, 238, 184, 254, 250, 3, 103, 63, 235, 217, 23, 220, 123, 195, 245, 83, 166, 212, 137, 43, 199, 53, 1, 244, 174, 28, 250, 165, 242, 204, 236, 255, 27, 24, 92, 33, 23, 60, 227, 18, 57, 235, 226, 139, 164, 153, 76, 203, 244, 244, 172, 204, 204, 21, 244, 33, 141, 4, 204, 113, 222, 160, 21, 43, 250, 244, 38, 206, 205, 86, 20, 28, 108, 224, 60, 49, 62, 106, 170, 67, 108, 16, 187, 48, 183, 94, 111, 48, 181, 62, 23, 250, 207, 45, 32, 212, 78, 38, 74, 115, 213, 140, 14, 4, 218, 56, 9, 126, 14, 155, 113, 204, 158, 13, 81, 65, 207, 126, 5, 179, 33, 242, 25, 7, 194, 80, 150, 128, 247, 65, 28, 218, 171, 15, 173, 29, 215, 94, 123, 128, 158, 218, 158, 29, 122, 0, 121, 172, 146, 146, 88, 9, 193, 106, 125, 37, 0, 0, 63, 210, 14, 63, 99, 242, 171, 198, 71, 8, 130, 184, 228, 172, 134, 223, 240, 220, 184, 172, 123, 6, 226, 219, 99, 146, 89, 129, 253, 162, 152, 2, 229, 40, 73, 163, 140, 243, 198, 33, 204, 236, 21, 217, 245, 93, 144, 193, 149, 161, 204, 29, 2, 81, 236, 130, 150, 172, 243, 66, 0, 56, 36, 2, 245, 241, 105, 13, 0, 56, 180, 6, 72, 6, 180, 0, 44, 248, 25, 194, 108, 69, 194, 76, 9, 241, 138, 73, 79, 162, 44, 251, 9, 148, 72, 0, 112, 92, 22, 29, 170, 132, 166, 103, 167, 157, 226, 88, 215, 145, 207, 155, 106, 133, 247, 148, 195, 182, 28, 251, 55, 169, 248, 15, 0, 243, 209, 100, 110, 179, 184, 15, 128, 98, 64, 141, 125, 45, 72, 214, 59, 171, 177, 5, 4, 128, 74, 104, 5, 48, 184, 242, 36, 0, 230, 173, 89, 61, 36, 103, 156, 190, 85, 254, 238, 127, 189, 87, 246, 239, 218, 243, 200, 75, 222, 244, 243, 23, 95, 249, 133, 79, 151, 77, 169, 19, 87, 142, 119, 2, 120, 103, 113, 98, 250, 19, 191, 244, 219, 239, 145, 77, 48, 245, 31, 120, 116, 151, 204, 206, 206, 209, 152, 86, 208, 161, 197, 67, 41, 121, 146, 132, 233, 207, 55, 222, 112, 186, 38, 201, 128, 211, 58, 137, 5, 226, 221, 116, 30, 153, 83, 212, 165, 254, 25, 15, 223, 164, 1, 92, 92, 162, 113, 177, 74, 142, 47, 115, 178, 9, 125, 65, 54, 2, 7, 114, 54, 50, 221, 156, 27, 162, 69, 48, 141, 22, 134, 199, 167, 198, 116, 27, 182, 24, 154, 248, 32, 6, 254, 96, 230, 187, 94, 253, 88, 211, 128, 57, 78, 31, 30, 26, 157, 26, 95, 8, 252, 50, 150, 37, 128, 30, 65, 77, 125, 171, 245, 35, 150, 211, 33, 61, 28, 16, 128, 31, 195, 146, 154, 253, 252, 119, 248, 178, 253, 85, 113, 89, 123, 41, 232, 40, 237, 206, 96, 249, 228, 240, 157, 145, 28, 188, 37, 148, 209, 123, 35, 217, 123, 67, 8, 114, 32, 9, 224, 188, 1, 100, 246, 13, 168, 143, 79, 95, 159, 36, 0, 107, 64, 72, 0, 89, 134, 138, 68, 57, 16, 64, 174, 136, 64, 139, 128, 29, 135, 117, 227, 14, 96, 59, 157, 123, 64, 66, 145, 166, 212, 233, 2, 232, 169, 153, 62, 22, 198, 21, 139, 4, 38, 0, 74, 45, 79, 208, 106, 154, 21, 226, 85, 45, 121, 144, 134, 33, 0, 62, 47, 96, 137, 4, 153, 156, 233, 184, 96, 3, 179, 3, 21, 150, 55, 245, 27, 178, 80, 2, 96, 101, 88, 103, 14, 211, 248, 110, 128, 179, 206, 216, 34, 81, 163, 42, 255, 248, 254, 63, 150, 122, 173, 254, 245, 70, 173, 254, 179, 90, 193, 9, 46, 199, 53, 1, 172, 88, 191, 246, 37, 147, 195, 35, 223, 125, 243, 47, 191, 75, 130, 193, 149, 50, 49, 57, 13, 115, 26, 192, 224, 29, 83, 252, 83, 131, 27, 128, 187, 224, 243, 197, 18, 8, 136, 234, 100, 28, 166, 225, 214, 106, 125, 132, 167, 17, 75, 0, 204, 131, 234, 228, 130, 96, 246, 149, 11, 76, 75, 81, 80, 35, 93, 27, 8, 2, 254, 233, 190, 88, 88, 45, 2, 132, 0, 69, 56, 254, 175, 199, 161, 53, 2, 232, 236, 140, 3, 96, 57, 22, 175, 227, 242, 48, 219, 233, 191, 171, 214, 135, 150, 55, 224, 199, 18, 90, 159, 235, 234, 231, 91, 223, 63, 162, 201, 223, 97, 234, 115, 56, 239, 148, 203, 60, 57, 231, 231, 61, 57, 239, 29, 32, 155, 128, 251, 56, 246, 82, 153, 138, 100, 118, 191, 200, 158, 239, 135, 114, 251, 71, 91, 176, 12, 112, 125, 172, 69, 96, 220, 2, 90, 3, 166, 51, 80, 173, 128, 108, 85, 36, 87, 150, 168, 7, 33, 15, 43, 128, 132, 64, 215, 64, 251, 5, 64, 6, 176, 2, 244, 213, 66, 56, 237, 102, 136, 237, 212, 143, 2, 200, 29, 160, 237, 125, 48, 111, 43, 66, 22, 210, 57, 146, 160, 98, 110, 13, 214, 23, 94, 27, 246, 234, 171, 0, 208, 180, 20, 9, 100, 87, 130, 229, 219, 130, 123, 170, 128, 183, 226, 8, 96, 62, 141, 238, 158, 39, 23, 158, 127, 186, 124, 243, 179, 159, 151, 31, 124, 231, 59, 146, 95, 57, 248, 238, 217, 145, 241, 255, 107, 11, 156, 208, 226, 16, 113, 92, 74, 34, 147, 157, 1, 172, 235, 149, 98, 81, 114, 217, 180, 2, 85, 5, 75, 106, 217, 78, 33, 8, 121, 163, 232, 243, 155, 245, 69, 55, 122, 129, 32, 83, 1, 78, 109, 77, 240, 99, 85, 77, 78, 4, 164, 233, 251, 240, 96, 14, 176, 46, 23, 124, 63, 16, 31, 150, 5, 173, 11, 63, 240, 245, 193, 19, 143, 54, 50, 47, 161, 54, 62, 0, 151, 254, 185, 2, 30, 241, 10, 52, 57, 77, 252, 66, 70, 100, 186, 7, 33, 47, 177, 25, 132, 169, 30, 137, 77, 98, 125, 42, 135, 180, 156, 196, 102, 145, 63, 151, 145, 168, 136, 243, 35, 25, 192, 244, 207, 228, 124, 57, 231, 13, 9, 249, 185, 43, 18, 242, 166, 47, 250, 114, 254, 47, 29, 63, 224, 167, 164, 7, 98, 178, 250, 252, 152, 92, 242, 27, 113, 121, 215, 109, 129, 188, 242, 35, 129, 244, 111, 4, 17, 114, 86, 162, 237, 183, 80, 235, 134, 231, 52, 135, 128, 115, 140, 205, 230, 37, 62, 219, 35, 126, 33, 47, 222, 92, 222, 44, 203, 56, 255, 74, 86, 188, 122, 86, 130, 70, 6, 188, 145, 4, 15, 128, 28, 81, 79, 251, 193, 39, 10, 111, 11, 73, 22, 86, 154, 177, 208, 120, 247, 121, 221, 205, 61, 35, 248, 233, 170, 57, 89, 124, 223, 219, 237, 230, 8, 226, 8, 190, 147, 4, 204, 190, 168, 46, 66, 104, 200, 186, 172, 30, 234, 147, 242, 212, 180, 220, 113, 195, 141, 108, 23, 133, 158, 254, 193, 27, 108, 209, 19, 94, 142, 107, 2, 200, 101, 123, 71, 177, 40, 206, 206, 76, 235, 172, 44, 246, 116, 243, 230, 168, 127, 143, 37, 1, 79, 147, 191, 13, 84, 250, 226, 238, 110, 107, 35, 104, 65, 89, 112, 210, 15, 223, 139, 199, 50, 212, 226, 240, 223, 129, 120, 142, 217, 115, 233, 35, 104, 157, 118, 51, 150, 83, 237, 111, 69, 247, 131, 125, 208, 66, 232, 108, 71, 84, 82, 106, 50, 162, 225, 104, 99, 133, 18, 147, 6, 26, 14, 252, 124, 130, 56, 94, 73, 75, 188, 192, 198, 159, 147, 248, 12, 194, 20, 192, 63, 1, 224, 79, 146, 4, 0, 126, 164, 69, 22, 248, 180, 4, 184, 77, 50, 237, 203, 150, 231, 5, 242, 146, 191, 78, 200, 11, 254, 204, 147, 83, 95, 17, 151, 204, 16, 234, 36, 67, 29, 135, 226, 167, 98, 146, 93, 17, 147, 11, 223, 21, 151, 95, 190, 221, 151, 151, 254, 141, 47, 171, 206, 156, 39, 2, 118, 112, 198, 202, 184, 6, 197, 12, 174, 69, 70, 188, 217, 172, 196, 73, 132, 32, 131, 216, 28, 174, 73, 33, 39, 65, 41, 43, 62, 203, 88, 247, 135, 86, 83, 68, 235, 135, 15, 55, 193, 189, 227, 200, 9, 1, 13, 37, 142, 91, 202, 56, 1, 11, 11, 3, 247, 148, 66, 240, 199, 97, 125, 104, 223, 140, 222, 244, 197, 253, 6, 188, 239, 230, 126, 170, 13, 192, 63, 189, 223, 174, 173, 208, 213, 96, 156, 117, 153, 39, 47, 249, 199, 187, 205, 121, 7, 177, 120, 74, 242, 253, 253, 114, 243, 181, 215, 73, 169, 88, 144, 32, 149, 218, 125, 233, 101, 47, 134, 253, 115, 114, 200, 113, 77, 0, 233, 108, 154, 47, 109, 156, 155, 157, 156, 146, 84, 2, 141, 137, 183, 135, 13, 128, 119, 8, 162, 55, 18, 63, 5, 40, 2, 133, 249, 122, 91, 237, 77, 164, 232, 205, 102, 156, 68, 160, 190, 62, 26, 13, 86, 59, 131, 105, 20, 243, 117, 184, 192, 116, 53, 253, 209, 72, 176, 162, 249, 218, 25, 174, 245, 217, 186, 105, 246, 179, 115, 175, 83, 251, 17, 252, 212, 240, 0, 123, 12, 141, 94, 168, 249, 213, 10, 200, 90, 141, 143, 50, 244, 253, 97, 254, 123, 112, 29, 122, 215, 120, 114, 201, 175, 4, 242, 51, 31, 242, 229, 220, 183, 199, 165, 111, 147, 217, 215, 137, 34, 236, 163, 184, 244, 61, 158, 188, 249, 203, 190, 92, 242, 203, 190, 244, 172, 161, 43, 99, 137, 128, 4, 71, 162, 3, 1, 8, 206, 63, 70, 50, 4, 17, 10, 150, 198, 10, 2, 49, 48, 159, 110, 81, 13, 101, 113, 45, 77, 255, 9, 181, 189, 185, 119, 188, 244, 110, 182, 30, 238, 136, 94, 123, 14, 129, 186, 254, 28, 37, 97, 117, 25, 152, 133, 127, 29, 162, 219, 115, 217, 182, 242, 52, 25, 226, 210, 121, 143, 73, 54, 230, 93, 3, 42, 172, 7, 233, 124, 255, 96, 189, 92, 150, 59, 161, 253, 243, 253, 3, 146, 235, 233, 123, 240, 138, 79, 126, 248, 132, 239, 252, 115, 114, 92, 19, 192, 155, 223, 245, 158, 50, 204, 241, 137, 106, 169, 36, 217, 44, 76, 72, 104, 109, 54, 0, 119, 127, 29, 64, 25, 40, 188, 157, 110, 252, 93, 211, 52, 142, 27, 235, 252, 116, 44, 201, 246, 90, 128, 229, 181, 8, 52, 0, 200, 131, 143, 186, 186, 209, 2, 51, 110, 108, 52, 137, 9, 208, 54, 220, 47, 118, 28, 211, 37, 54, 196, 66, 173, 10, 52, 210, 56, 26, 122, 172, 142, 198, 138, 198, 203, 70, 28, 209, 236, 133, 134, 87, 147, 127, 6, 192, 103, 131, 39, 240, 97, 10, 71, 32, 134, 200, 250, 255, 36, 12, 78, 45, 62, 255, 109, 129, 252, 252, 149, 208, 250, 127, 234, 201, 192, 169, 230, 216, 78, 84, 233, 219, 24, 147, 151, 254, 83, 92, 94, 245, 81, 16, 217, 91, 224, 42, 193, 117, 82, 183, 0, 231, 43, 36, 61, 186, 3, 211, 184, 22, 150, 4, 140, 59, 148, 151, 8, 105, 116, 23, 216, 55, 162, 29, 162, 156, 247, 64, 75, 128, 67, 124, 0, 166, 153, 208, 67, 51, 139, 240, 230, 61, 49, 251, 227, 213, 114, 254, 255, 252, 127, 182, 4, 222, 123, 42, 12, 179, 212, 89, 153, 154, 106, 201, 195, 85, 0, 209, 28, 211, 24, 58, 18, 16, 80, 38, 149, 76, 202, 161, 221, 187, 101, 102, 102, 74, 214, 109, 217, 6, 75, 48, 126, 191, 150, 57, 73, 228, 184, 38, 128, 63, 250, 197, 55, 53, 112, 227, 70, 248, 222, 248, 120, 212, 16, 63, 225, 233, 205, 119, 55, 183, 109, 246, 243, 110, 225, 126, 234, 18, 55, 154, 198, 161, 1, 51, 242, 216, 17, 72, 19, 143, 192, 7, 200, 225, 0, 48, 151, 213, 235, 255, 121, 235, 64, 147, 218, 194, 60, 213, 252, 16, 147, 133, 255, 74, 10, 38, 26, 33, 207, 139, 160, 169, 248, 8, 110, 131, 230, 43, 150, 52, 101, 231, 160, 205, 96, 246, 171, 153, 11, 2, 160, 169, 79, 141, 79, 139, 64, 65, 160, 29, 126, 220, 206, 151, 179, 223, 20, 200, 187, 110, 73, 200, 203, 63, 12, 224, 111, 53, 199, 116, 50, 8, 239, 205, 214, 151, 197, 229, 229, 31, 242, 228, 149, 31, 14, 100, 197, 169, 150, 4, 120, 238, 36, 1, 184, 3, 74, 136, 116, 133, 104, 5, 88, 235, 40, 206, 107, 87, 68, 168, 192, 93, 104, 160, 156, 62, 228, 196, 169, 206, 184, 222, 218, 231, 103, 238, 147, 185, 247, 116, 253, 204, 53, 115, 15, 4, 49, 159, 121, 106, 225, 49, 176, 156, 6, 180, 19, 228, 26, 229, 128, 251, 141, 192, 86, 228, 72, 192, 140, 42, 88, 215, 129, 73, 38, 89, 247, 153, 203, 229, 164, 56, 51, 195, 222, 0, 109, 111, 210, 138, 238, 53, 185, 39, 135, 152, 22, 126, 28, 11, 0, 60, 94, 169, 84, 244, 101, 140, 252, 0, 134, 3, 190, 46, 113, 35, 85, 83, 219, 178, 36, 135, 246, 10, 196, 104, 113, 210, 1, 167, 119, 154, 52, 219, 102, 180, 88, 135, 18, 208, 250, 22, 138, 89, 119, 251, 225, 165, 162, 149, 192, 56, 82, 140, 159, 91, 199, 178, 134, 60, 53, 251, 13, 200, 233, 219, 10, 192, 79, 83, 87, 208, 160, 99, 4, 62, 173, 2, 219, 211, 207, 17, 129, 213, 240, 147, 223, 240, 185, 64, 94, 251, 41, 0, 127, 219, 226, 253, 158, 60, 146, 200, 197, 228, 156, 183, 198, 229, 117, 159, 14, 228, 172, 215, 243, 122, 33, 240, 58, 112, 232, 147, 215, 5, 215, 135, 215, 41, 78, 183, 128, 1, 238, 0, 201, 50, 86, 162, 149, 196, 145, 147, 64, 162, 58, 128, 218, 98, 224, 253, 194, 181, 210, 251, 196, 54, 64, 139, 141, 100, 78, 112, 211, 18, 212, 93, 98, 201, 248, 252, 141, 101, 155, 104, 41, 192, 205, 118, 78, 56, 145, 203, 89, 124, 20, 181, 244, 240, 211, 34, 218, 56, 76, 148, 10, 134, 17, 198, 171, 229, 185, 90, 177, 200, 153, 17, 39, 143, 28, 247, 4, 16, 79, 196, 15, 87, 171, 21, 92, 124, 190, 145, 5, 154, 130, 128, 196, 205, 83, 159, 223, 222, 79, 130, 220, 208, 0, 2, 110, 36, 147, 217, 32, 8, 118, 157, 217, 101, 145, 110, 139, 67, 168, 243, 209, 168, 76, 73, 147, 212, 33, 204, 83, 182, 103, 30, 205, 79, 214, 137, 168, 166, 163, 209, 120, 48, 75, 227, 45, 52, 192, 58, 53, 191, 153, 196, 19, 43, 96, 57, 139, 70, 61, 195, 30, 111, 154, 179, 88, 210, 175, 101, 71, 24, 128, 31, 7, 240, 19, 73, 79, 46, 251, 45, 128, 255, 243, 190, 108, 123, 37, 181, 209, 227, 247, 125, 50, 202, 170, 243, 98, 242, 51, 31, 100, 39, 33, 77, 123, 144, 0, 128, 77, 34, 136, 21, 113, 109, 120, 141, 72, 4, 176, 154, 34, 94, 55, 184, 8, 236, 60, 141, 131, 36, 194, 50, 231, 68, 224, 26, 195, 18, 96, 63, 137, 135, 159, 94, 49, 130, 92, 73, 125, 30, 169, 198, 212, 103, 39, 47, 3, 11, 1, 206, 4, 53, 130, 25, 223, 55, 109, 96, 129, 240, 158, 226, 126, 178, 54, 205, 166, 174, 224, 210, 221, 22, 52, 1, 208, 135, 164, 97, 5, 176, 141, 149, 230, 102, 167, 55, 158, 190, 253, 132, 125, 3, 240, 145, 228, 184, 39, 128, 100, 50, 179, 155, 95, 135, 153, 158, 156, 210, 55, 181, 56, 141, 188, 24, 60, 234, 35, 242, 238, 217, 228, 118, 46, 111, 190, 235, 217, 89, 176, 9, 77, 197, 121, 191, 191, 83, 8, 116, 87, 189, 210, 4, 54, 87, 237, 195, 70, 198, 142, 39, 118, 80, 233, 252, 125, 52, 98, 250, 243, 240, 93, 85, 115, 113, 168, 107, 14, 1, 102, 172, 118, 4, 178, 145, 171, 63, 235, 203, 202, 211, 125, 121, 225, 159, 7, 242, 204, 223, 231, 148, 93, 236, 59, 177, 224, 96, 78, 122, 225, 208, 225, 37, 191, 30, 151, 183, 93, 25, 72, 110, 8, 228, 77, 34, 112, 110, 1, 45, 2, 117, 13, 112, 127, 225, 30, 112, 196, 32, 206, 145, 3, 92, 67, 15, 4, 224, 53, 113, 29, 249, 186, 51, 246, 183, 176, 195, 85, 1, 141, 251, 141, 123, 142, 21, 4, 94, 75, 115, 127, 140, 144, 176, 173, 70, 135, 104, 234, 130, 246, 194, 28, 187, 45, 219, 147, 146, 189, 21, 219, 84, 40, 172, 142, 86, 100, 42, 203, 239, 29, 227, 22, 207, 78, 215, 87, 172, 93, 87, 215, 149, 147, 68, 142, 123, 2, 240, 98, 137, 9, 246, 254, 242, 99, 12, 125, 189, 96, 98, 164, 117, 130, 159, 113, 3, 124, 133, 173, 222, 108, 157, 168, 179, 72, 200, 240, 108, 47, 220, 222, 212, 97, 44, 132, 133, 66, 93, 128, 60, 100, 144, 24, 204, 52, 98, 164, 32, 174, 190, 36, 52, 80, 156, 111, 234, 105, 161, 161, 214, 161, 241, 43, 104, 180, 104, 168, 177, 130, 241, 249, 57, 172, 21, 3, 248, 217, 152, 117, 8, 12, 13, 156, 15, 255, 156, 114, 25, 192, 255, 23, 190, 2, 128, 195, 102, 79, 87, 225, 117, 221, 116, 121, 92, 126, 225, 234, 64, 54, 62, 27, 36, 202, 185, 19, 236, 236, 83, 139, 0, 215, 139, 100, 202, 121, 19, 234, 58, 193, 53, 128, 43, 224, 225, 26, 147, 4, 226, 236, 15, 224, 4, 41, 125, 234, 145, 129, 247, 144, 247, 133, 110, 128, 214, 142, 64, 114, 120, 252, 245, 53, 229, 58, 155, 58, 239, 51, 135, 252, 72, 2, 186, 10, 50, 65, 43, 98, 35, 105, 55, 16, 205, 209, 23, 133, 230, 242, 61, 58, 227, 179, 217, 104, 180, 146, 169, 12, 115, 79, 26, 57, 238, 9, 32, 200, 38, 134, 177, 168, 77, 142, 143, 73, 46, 157, 16, 223, 177, 53, 113, 105, 129, 206, 155, 171, 29, 60, 204, 179, 126, 225, 98, 177, 69, 13, 9, 224, 70, 235, 71, 42, 208, 8, 76, 31, 1, 51, 141, 214, 112, 150, 191, 206, 7, 71, 196, 116, 38, 193, 159, 141, 251, 146, 140, 167, 36, 17, 161, 65, 54, 18, 226, 233, 3, 60, 108, 164, 57, 241, 56, 169, 5, 65, 53, 63, 181, 89, 149, 174, 1, 202, 196, 124, 57, 227, 181, 190, 252, 236, 127, 120, 114, 234, 203, 237, 1, 116, 69, 45, 160, 55, 93, 17, 151, 139, 222, 229, 73, 42, 135, 11, 206, 23, 154, 168, 53, 64, 18, 96, 0, 193, 178, 51, 16, 33, 2, 193, 74, 25, 46, 1, 93, 45, 237, 20, 132, 213, 198, 41, 210, 124, 55, 34, 71, 8, 112, 191, 249, 94, 6, 246, 7, 16, 214, 26, 56, 119, 160, 221, 6, 120, 15, 57, 250, 227, 243, 182, 242, 230, 155, 246, 194, 244, 14, 192, 171, 189, 128, 251, 175, 175, 53, 163, 160, 48, 219, 12, 9, 32, 147, 197, 253, 133, 203, 217, 106, 182, 166, 207, 56, 239, 252, 162, 41, 112, 114, 136, 187, 74, 199, 173, 100, 243, 125, 135, 112, 51, 230, 70, 135, 135, 117, 206, 191, 190, 107, 143, 55, 135, 255, 58, 196, 117, 12, 58, 160, 119, 138, 163, 108, 130, 155, 125, 58, 166, 247, 24, 17, 37, 2, 163, 237, 185, 116, 150, 3, 235, 118, 228, 194, 134, 228, 197, 97, 134, 66, 91, 121, 28, 150, 130, 233, 170, 29, 122, 48, 79, 233, 191, 198, 105, 250, 179, 87, 155, 190, 44, 124, 90, 206, 254, 211, 198, 12, 109, 117, 225, 59, 125, 121, 221, 103, 60, 233, 61, 229, 8, 7, 245, 52, 151, 84, 95, 76, 94, 242, 143, 49, 121, 206, 31, 122, 146, 29, 192, 189, 224, 131, 82, 124, 17, 138, 246, 13, 144, 0, 140, 37, 64, 119, 128, 241, 136, 253, 5, 32, 214, 56, 200, 55, 30, 50, 224, 94, 112, 142, 128, 222, 92, 92, 95, 130, 185, 125, 167, 33, 136, 242, 54, 171, 187, 136, 85, 103, 1, 180, 219, 7, 215, 109, 26, 11, 44, 110, 79, 20, 214, 134, 86, 1, 240, 131, 80, 160, 16, 130, 68, 114, 239, 63, 253, 209, 123, 187, 46, 192, 114, 202, 198, 211, 78, 47, 198, 226, 254, 100, 113, 166, 32, 137, 192, 215, 79, 89, 211, 47, 99, 160, 240, 198, 234, 147, 121, 246, 71, 110, 55, 100, 160, 217, 72, 49, 186, 64, 139, 219, 246, 161, 122, 130, 249, 250, 143, 166, 95, 67, 193, 175, 38, 62, 234, 227, 246, 252, 177, 167, 56, 136, 123, 250, 105, 47, 9, 125, 9, 161, 129, 194, 182, 166, 130, 121, 74, 63, 149, 46, 0, 27, 43, 52, 87, 68, 127, 159, 141, 24, 224, 127, 193, 7, 124, 121, 201, 63, 28, 247, 151, 247, 152, 10, 103, 56, 94, 242, 235, 49, 121, 238, 251, 60, 201, 175, 132, 229, 166, 19, 135, 72, 174, 8, 236, 8, 132, 27, 160, 51, 6, 73, 4, 37, 132, 50, 174, 181, 146, 0, 174, 127, 200, 201, 66, 184, 190, 17, 239, 25, 174, 57, 65, 108, 110, 170, 169, 28, 98, 8, 159, 55, 222, 116, 24, 154, 30, 125, 236, 71, 203, 97, 83, 37, 1, 54, 10, 215, 161, 104, 132, 182, 161, 10, 146, 146, 176, 58, 11, 133, 25, 237, 76, 246, 130, 196, 29, 38, 227, 228, 145, 227, 190, 133, 94, 253, 133, 207, 20, 1, 236, 195, 211, 19, 211, 244, 193, 36, 157, 54, 159, 102, 82, 147, 141, 172, 207, 27, 207, 31, 110, 170, 18, 0, 110, 42, 151, 78, 180, 147, 15, 230, 31, 111, 175, 187, 197, 154, 171, 38, 62, 243, 77, 67, 100, 154, 177, 6, 204, 182, 108, 60, 140, 114, 118, 88, 188, 133, 8, 135, 253, 104, 130, 18, 228, 28, 207, 87, 127, 149, 125, 0, 8, 165, 68, 91, 243, 243, 33, 160, 231, 188, 215, 151, 75, 223, 3, 10, 57, 78, 167, 240, 30, 79, 194, 206, 208, 243, 223, 25, 147, 203, 255, 63, 206, 30, 196, 69, 135, 133, 165, 111, 63, 2, 9, 168, 133, 197, 190, 0, 4, 79, 251, 4, 176, 228, 27, 146, 234, 112, 191, 216, 119, 64, 43, 128, 1, 205, 88, 239, 62, 238, 21, 227, 20, 37, 121, 174, 3, 216, 198, 215, 231, 67, 61, 88, 87, 235, 207, 5, 10, 90, 5, 218, 130, 121, 8, 136, 29, 198, 104, 91, 104, 87, 198, 77, 20, 180, 183, 148, 76, 79, 78, 104, 219, 75, 166, 211, 143, 217, 141, 78, 26, 57, 238, 9, 0, 18, 5, 137, 96, 152, 207, 3, 52, 234, 117, 73, 167, 160, 105, 53, 85, 243, 140, 116, 222, 83, 101, 114, 151, 137, 37, 211, 116, 142, 247, 124, 17, 106, 120, 250, 116, 250, 57, 106, 6, 90, 16, 202, 4, 204, 196, 31, 162, 108, 74, 90, 26, 141, 72, 63, 196, 193, 14, 168, 42, 72, 128, 67, 126, 4, 63, 129, 79, 179, 31, 235, 6, 252, 36, 8, 79, 46, 121, 55, 192, 255, 155, 113, 29, 3, 239, 202, 143, 39, 126, 50, 38, 231, 252, 124, 76, 94, 246, 119, 118, 132, 128, 68, 74, 77, 95, 1, 208, 75, 28, 21, 32, 9, 228, 96, 113, 97, 73, 171, 139, 247, 160, 142, 64, 66, 166, 21, 160, 83, 130, 121, 243, 120, 191, 204, 253, 87, 55, 79, 201, 156, 36, 192, 255, 244, 253, 217, 169, 204, 36, 147, 167, 37, 241, 175, 217, 108, 64, 195, 243, 171, 197, 102, 6, 40, 3, 63, 175, 78, 37, 192, 87, 183, 143, 28, 56, 200, 250, 171, 233, 116, 122, 7, 54, 57, 169, 132, 87, 237, 184, 151, 152, 31, 236, 174, 87, 171, 58, 18, 192, 175, 237, 106, 26, 110, 160, 211, 214, 238, 44, 116, 232, 7, 76, 110, 58, 246, 104, 254, 209, 156, 7, 243, 123, 161, 62, 234, 235, 195, 26, 48, 239, 7, 52, 27, 112, 123, 154, 250, 108, 62, 166, 62, 83, 21, 57, 68, 59, 21, 169, 253, 217, 209, 164, 239, 236, 163, 121, 74, 243, 223, 250, 255, 36, 1, 0, 95, 251, 3, 152, 7, 240, 111, 184, 204, 147, 11, 222, 229, 73, 102, 208, 30, 87, 87, 126, 108, 161, 181, 180, 253, 53, 49, 121, 209, 95, 226, 26, 195, 138, 50, 163, 3, 32, 1, 92, 103, 237, 16, 212, 254, 0, 142, 14, 100, 196, 99, 167, 160, 186, 2, 41, 241, 112, 111, 162, 38, 238, 187, 42, 121, 78, 23, 108, 226, 110, 82, 8, 114, 222, 77, 186, 135, 230, 46, 243, 238, 170, 101, 135, 24, 237, 2, 62, 50, 220, 32, 240, 249, 208, 17, 31, 51, 183, 66, 237, 207, 183, 5, 243, 65, 49, 110, 125, 248, 208, 8, 54, 242, 135, 87, 173, 221, 124, 216, 22, 57, 105, 196, 32, 225, 56, 23, 0, 121, 63, 77, 176, 137, 145, 49, 201, 128, 0, 244, 6, 90, 51, 141, 129, 98, 180, 246, 98, 65, 30, 0, 28, 66, 131, 171, 229, 7, 113, 224, 111, 111, 203, 205, 77, 21, 42, 44, 166, 69, 53, 157, 36, 128, 166, 195, 206, 63, 54, 74, 16, 64, 156, 147, 122, 232, 6, 16, 248, 28, 190, 34, 41, 192, 119, 77, 247, 197, 229, 130, 119, 122, 178, 242, 236, 199, 31, 69, 87, 126, 60, 33, 9, 156, 241, 250, 152, 60, 255, 143, 205, 53, 53, 15, 87, 209, 226, 66, 96, 95, 11, 231, 7, 192, 13, 136, 193, 42, 224, 60, 11, 186, 10, 218, 111, 208, 2, 76, 105, 165, 241, 94, 105, 127, 16, 239, 49, 239, 173, 245, 229, 23, 52, 115, 222, 159, 142, 175, 53, 187, 34, 20, 100, 117, 182, 9, 42, 11, 206, 53, 152, 24, 25, 65, 92, 14, 94, 119, 253, 85, 51, 166, 224, 201, 35, 39, 4, 1, 244, 228, 115, 135, 120, 111, 198, 70, 198, 165, 7, 4, 16, 15, 112, 216, 72, 112, 254, 154, 222, 107, 148, 35, 171, 155, 30, 125, 6, 110, 201, 84, 74, 92, 223, 58, 11, 117, 142, 127, 198, 29, 96, 26, 183, 101, 135, 15, 168, 64, 203, 51, 93, 151, 202, 22, 168, 131, 245, 242, 137, 52, 117, 1, 24, 140, 102, 226, 228, 30, 6, 243, 2, 15, 164, 53, 227, 178, 229, 114, 79, 206, 250, 57, 83, 115, 87, 126, 122, 225, 155, 142, 158, 253, 191, 98, 114, 222, 219, 65, 0, 124, 36, 88, 175, 183, 121, 103, 162, 71, 119, 0, 68, 224, 149, 96, 1, 240, 141, 74, 32, 224, 88, 13, 101, 112, 253, 113, 183, 160, 40, 176, 13, 117, 182, 146, 188, 11, 243, 55, 150, 90, 30, 184, 151, 6, 204, 251, 206, 153, 129, 174, 13, 240, 175, 109, 85, 66, 248, 238, 135, 90, 173, 42, 149, 66, 65, 146, 217, 222, 7, 115, 11, 233, 226, 164, 16, 94, 161, 227, 95, 194, 214, 110, 248, 234, 149, 177, 195, 195, 146, 0, 21, 107, 231, 26, 254, 180, 183, 159, 17, 222, 59, 4, 222, 84, 189, 173, 122, 179, 89, 6, 167, 231, 226, 122, 115, 77, 208, 54, 161, 17, 110, 99, 111, 254, 2, 177, 151, 5, 22, 128, 102, 113, 30, 58, 59, 2, 249, 206, 126, 154, 166, 214, 223, 215, 87, 118, 193, 58, 200, 12, 64, 115, 189, 1, 238, 197, 113, 244, 210, 142, 19, 89, 120, 29, 95, 245, 47, 113, 57, 229, 153, 0, 52, 181, 59, 93, 44, 90, 2, 236, 19, 224, 3, 87, 116, 1, 42, 88, 130, 24, 188, 166, 121, 137, 136, 126, 20, 21, 224, 215, 123, 102, 219, 68, 155, 0, 40, 88, 111, 182, 96, 242, 131, 1, 140, 159, 223, 82, 237, 174, 247, 159, 237, 166, 131, 16, 216, 30, 184, 158, 205, 101, 101, 102, 106, 82, 138, 115, 179, 146, 206, 247, 156, 116, 29, 128, 148, 19, 130, 0, 182, 159, 115, 238, 88, 44, 238, 13, 79, 78, 76, 232, 80, 32, 63, 214, 216, 22, 189, 191, 184, 121, 29, 55, 208, 0, 221, 0, 127, 222, 223, 55, 69, 92, 26, 55, 115, 115, 7, 152, 193, 173, 25, 180, 15, 145, 98, 203, 234, 214, 228, 125, 53, 51, 9, 122, 4, 196, 249, 210, 10, 46, 105, 29, 244, 110, 136, 203, 246, 87, 233, 129, 116, 229, 40, 9, 73, 254, 5, 31, 240, 164, 127, 19, 174, 51, 39, 255, 104, 159, 0, 52, 190, 29, 130, 37, 17, 120, 85, 144, 64, 45, 169, 159, 75, 35, 1, 68, 77, 220, 91, 220, 55, 51, 69, 152, 119, 206, 221, 85, 3, 232, 90, 189, 10, 159, 223, 124, 244, 149, 238, 129, 233, 47, 50, 55, 92, 219, 129, 21, 147, 6, 2, 200, 102, 101, 106, 124, 66, 90, 141, 58, 63, 60, 250, 160, 201, 61, 185, 196, 160, 227, 56, 151, 107, 190, 253, 173, 217, 80, 188, 3, 163, 163, 99, 184, 129, 117, 37, 0, 253, 100, 54, 196, 220, 56, 220, 112, 235, 239, 113, 149, 39, 165, 227, 250, 246, 145, 81, 237, 31, 208, 251, 203, 142, 159, 152, 185, 193, 22, 233, 52, 38, 72, 4, 20, 38, 59, 27, 79, 223, 48, 131, 237, 217, 137, 164, 99, 205, 52, 45, 105, 146, 98, 251, 249, 233, 168, 104, 112, 168, 156, 175, 229, 126, 186, 205, 237, 95, 14, 217, 248, 252, 152, 92, 240, 139, 190, 164, 122, 112, 173, 219, 36, 64, 247, 11, 230, 63, 173, 1, 117, 3, 64, 8, 28, 54, 212, 190, 0, 220, 15, 146, 52, 59, 110, 219, 86, 0, 65, 14, 147, 31, 49, 174, 162, 132, 182, 25, 186, 139, 250, 136, 72, 103, 224, 38, 104, 4, 140, 178, 3, 57, 149, 10, 132, 86, 39, 202, 23, 251, 6, 123, 14, 32, 249, 164, 19, 211, 242, 143, 127, 137, 18, 153, 228, 222, 217, 201, 113, 169, 84, 138, 250, 173, 54, 189, 95, 6, 213, 230, 235, 176, 140, 88, 54, 167, 240, 6, 83, 248, 223, 22, 3, 88, 141, 230, 119, 250, 94, 183, 183, 110, 66, 91, 218, 85, 176, 169, 32, 79, 71, 2, 144, 175, 233, 36, 15, 27, 215, 37, 214, 225, 83, 158, 250, 82, 75, 42, 199, 137, 112, 94, 123, 189, 20, 201, 236, 129, 72, 14, 223, 17, 201, 158, 107, 66, 217, 125, 85, 40, 123, 174, 142, 228, 192, 141, 145, 76, 237, 50, 26, 145, 229, 154, 181, 227, 231, 184, 143, 36, 231, 190, 35, 38, 219, 95, 1, 112, 211, 221, 98, 199, 160, 246, 9, 208, 18, 48, 36, 96, 158, 185, 96, 103, 96, 160, 47, 103, 225, 61, 227, 207, 180, 133, 249, 251, 170, 171, 246, 84, 121, 238, 237, 241, 126, 151, 136, 162, 156, 67, 162, 247, 17, 129, 195, 195, 169, 68, 82, 166, 71, 56, 19, 61, 54, 188, 225, 180, 211, 14, 154, 130, 39, 151, 156, 40, 4, 0, 77, 29, 61, 82, 173, 148, 100, 122, 108, 66, 122, 243, 57, 51, 158, 203, 187, 230, 110, 42, 193, 201, 37, 255, 169, 70, 167, 230, 103, 199, 16, 115, 153, 196, 145, 0, 178, 191, 185, 209, 236, 16, 50, 47, 146, 208, 138, 52, 157, 49, 173, 131, 250, 194, 36, 235, 146, 218, 131, 212, 97, 94, 5, 6, 193, 118, 46, 155, 50, 176, 221, 104, 149, 99, 37, 157, 228, 195, 248, 35, 95, 137, 228, 218, 247, 181, 228, 155, 239, 106, 200, 87, 223, 209, 144, 239, 253, 86, 83, 174, 121, 111, 75, 190, 255, 251, 13, 249, 6, 210, 254, 253, 249, 77, 249, 240, 169, 77, 249, 210, 155, 90, 114, 199, 135, 34, 217, 123, 109, 164, 175, 250, 230, 183, 0, 142, 55, 201, 173, 138, 201, 69, 239, 142, 203, 218, 139, 160, 217, 233, 126, 81, 211, 83, 227, 211, 18, 32, 248, 105, 1, 112, 78, 64, 231, 188, 0, 142, 6, 232, 15, 119, 78, 175, 13, 9, 207, 250, 254, 109, 75, 145, 22, 192, 188, 11, 64, 97, 154, 179, 6, 3, 184, 154, 210, 108, 200, 228, 216, 168, 196, 18, 222, 222, 107, 190, 248, 197, 146, 102, 156, 100, 114, 194, 16, 64, 42, 155, 217, 21, 54, 67, 153, 24, 31, 147, 84, 58, 9, 16, 51, 117, 97, 131, 117, 176, 228, 61, 229, 205, 236, 204, 213, 7, 133, 22, 8, 203, 34, 152, 63, 189, 16, 14, 195, 52, 13, 23, 110, 203, 127, 172, 148, 75, 27, 239, 144, 198, 49, 110, 26, 60, 87, 106, 251, 27, 255, 50, 148, 143, 158, 222, 148, 43, 126, 174, 46, 119, 126, 188, 33, 123, 111, 104, 72, 241, 112, 75, 230, 16, 166, 247, 54, 100, 102, 127, 83, 138, 163, 45, 169, 21, 90, 82, 30, 11, 101, 231, 183, 91, 114, 245, 31, 213, 229, 51, 47, 169, 203, 127, 189, 174, 9, 210, 136, 228, 254, 207, 134, 250, 218, 239, 227, 73, 86, 159, 47, 114, 222, 219, 1, 126, 184, 93, 250, 186, 117, 237, 20, 180, 253, 1, 28, 14, 164, 37, 96, 135, 4, 181, 211, 16, 4, 205, 7, 125, 248, 77, 64, 139, 127, 44, 57, 71, 128, 17, 198, 73, 8, 84, 25, 246, 134, 83, 58, 78, 153, 121, 252, 206, 68, 165, 48, 43, 83, 19, 19, 252, 16, 232, 195, 76, 54, 185, 39, 151, 156, 48, 4, 224, 5, 169, 29, 0, 113, 125, 248, 208, 33, 125, 51, 80, 27, 173, 110, 193, 117, 252, 241, 230, 57, 243, 142, 64, 214, 239, 76, 32, 143, 218, 127, 177, 180, 147, 184, 105, 71, 182, 33, 23, 222, 113, 243, 51, 211, 73, 33, 214, 4, 104, 147, 139, 37, 130, 226, 232, 177, 107, 27, 213, 153, 72, 238, 252, 88, 40, 95, 254, 185, 150, 220, 242, 119, 45, 5, 121, 140, 31, 231, 8, 106, 240, 157, 91, 146, 234, 107, 32, 212, 77, 232, 53, 33, 153, 111, 72, 144, 69, 200, 181, 196, 75, 162, 172, 215, 144, 217, 67, 77, 185, 243, 163, 45, 249, 238, 111, 181, 228, 43, 111, 13, 229, 186, 63, 13, 213, 85, 56, 30, 132, 111, 31, 222, 254, 234, 24, 172, 0, 52, 87, 186, 2, 77, 186, 2, 104, 3, 218, 31, 0, 101, 192, 215, 177, 49, 208, 61, 224, 147, 133, 176, 2, 248, 194, 80, 206, 255, 96, 159, 15, 219, 0, 63, 252, 217, 22, 123, 90, 218, 102, 32, 238, 62, 43, 41, 216, 123, 203, 41, 192, 133, 66, 81, 170, 149, 138, 196, 61, 239, 164, 236, 0, 164, 156, 48, 4, 176, 106, 221, 218, 17, 52, 133, 177, 209, 253, 195, 146, 132, 121, 22, 169, 73, 111, 14, 223, 61, 12, 68, 225, 77, 84, 230, 71, 224, 173, 108, 3, 123, 65, 91, 102, 58, 223, 16, 108, 87, 33, 216, 76, 133, 224, 247, 245, 241, 81, 214, 25, 2, 227, 13, 180, 20, 104, 15, 181, 0, 176, 29, 73, 32, 142, 116, 62, 100, 194, 186, 145, 54, 117, 12, 6, 136, 248, 121, 174, 125, 215, 69, 114, 245, 123, 67, 185, 233, 175, 90, 50, 114, 183, 253, 176, 165, 95, 149, 88, 166, 34, 233, 190, 170, 228, 86, 54, 37, 179, 162, 42, 217, 149, 21, 201, 173, 174, 72, 118, 85, 85, 50, 43, 17, 134, 170, 146, 30, 4, 65, 128, 20, 50, 3, 45, 137, 243, 59, 126, 126, 69, 248, 69, 159, 122, 37, 148, 61, 215, 181, 228, 102, 144, 201, 21, 111, 14, 229, 190, 255, 48, 192, 225, 117, 61, 150, 146, 93, 41, 114, 233, 111, 208, 10, 192, 125, 177, 86, 128, 6, 18, 128, 237, 7, 160, 11, 160, 22, 0, 95, 33, 6, 162, 224, 228, 32, 106, 126, 126, 222, 171, 201, 206, 91, 186, 4, 248, 115, 231, 210, 38, 0, 174, 243, 246, 182, 127, 34, 253, 125, 125, 114, 240, 240, 136, 52, 26, 141, 48, 155, 203, 61, 162, 5, 79, 66, 57, 97, 8, 224, 79, 222, 255, 167, 19, 126, 32, 7, 199, 71, 71, 84, 155, 115, 142, 246, 124, 163, 52, 195, 58, 20, 115, 75, 169, 249, 1, 82, 27, 71, 65, 252, 153, 124, 39, 157, 13, 154, 229, 184, 198, 37, 47, 72, 204, 3, 185, 120, 180, 26, 216, 42, 140, 118, 224, 39, 176, 98, 250, 25, 44, 6, 212, 197, 117, 107, 17, 12, 19, 124, 252, 94, 222, 50, 73, 189, 24, 201, 163, 95, 141, 228, 187, 191, 221, 148, 123, 63, 221, 146, 202, 52, 52, 121, 26, 64, 78, 85, 197, 203, 148, 197, 207, 22, 165, 111, 125, 83, 242, 107, 75, 210, 179, 190, 36, 189, 27, 138, 26, 250, 54, 22, 164, 247, 148, 162, 166, 229, 215, 148, 225, 95, 51, 52, 36, 183, 162, 46, 177, 84, 65, 36, 89, 194, 182, 77, 241, 82, 32, 208, 120, 75, 198, 30, 106, 201, 149, 176, 8, 254, 243, 117, 45, 25, 189, 23, 138, 247, 24, 118, 24, 114, 148, 229, 140, 55, 196, 204, 203, 83, 217, 23, 64, 18, 32, 232, 93, 127, 0, 130, 62, 73, 8, 82, 32, 248, 93, 39, 45, 111, 29, 121, 209, 137, 98, 222, 158, 198, 2, 82, 99, 58, 132, 247, 58, 153, 12, 36, 215, 147, 147, 241, 145, 195, 76, 152, 206, 247, 246, 239, 51, 185, 39, 159, 156, 48, 4, 240, 179, 47, 125, 97, 232, 197, 189, 157, 51, 227, 211, 210, 168, 86, 117, 70, 224, 252, 237, 3, 107, 171, 221, 206, 219, 7, 112, 226, 46, 59, 83, 78, 59, 254, 52, 143, 110, 1, 203, 154, 123, 237, 216, 159, 210, 78, 103, 18, 175, 136, 62, 60, 132, 68, 110, 7, 75, 64, 71, 149, 8, 118, 31, 233, 252, 244, 149, 18, 128, 13, 72, 31, 190, 43, 146, 195, 119, 178, 134, 165, 23, 186, 27, 55, 255, 109, 40, 223, 252, 31, 77, 153, 124, 44, 84, 224, 123, 0, 109, 162, 183, 33, 177, 244, 156, 120, 189, 5, 104, 251, 130, 244, 111, 174, 73, 239, 70, 128, 126, 115, 81, 250, 183, 206, 33, 204, 154, 229, 150, 2, 210, 16, 54, 129, 20, 54, 130, 8, 64, 6, 131, 155, 155, 146, 28, 152, 133, 221, 59, 37, 65, 79, 85, 2, 184, 8, 126, 198, 16, 65, 179, 30, 202, 206, 239, 180, 228, 19, 151, 180, 228, 182, 15, 70, 199, 212, 221, 225, 67, 67, 231, 253, 130, 153, 8, 166, 83, 179, 117, 84, 192, 184, 3, 124, 58, 144, 46, 64, 132, 180, 168, 133, 52, 123, 152, 236, 232, 117, 65, 27, 4, 22, 166, 61, 48, 143, 138, 65, 233, 221, 172, 243, 135, 245, 76, 50, 45, 124, 7, 192, 225, 221, 187, 113, 191, 253, 225, 11, 158, 251, 252, 113, 91, 228, 164, 147, 19, 134, 0, 40, 233, 92, 223, 142, 82, 161, 32, 51, 147, 147, 210, 163, 47, 106, 180, 183, 78, 109, 121, 99, 186, 25, 153, 247, 249, 29, 206, 93, 89, 37, 8, 46, 177, 206, 190, 2, 205, 110, 151, 49, 75, 29, 31, 86, 65, 6, 26, 11, 31, 33, 231, 103, 176, 221, 231, 176, 249, 37, 92, 245, 179, 61, 44, 65, 2, 147, 187, 90, 242, 216, 55, 231, 235, 93, 42, 153, 222, 29, 201, 55, 222, 105, 204, 115, 30, 123, 28, 32, 165, 198, 78, 194, 207, 15, 250, 42, 18, 203, 79, 138, 223, 63, 45, 171, 182, 215, 165, 23, 0, 239, 223, 66, 240, 27, 224, 15, 156, 10, 82, 96, 252, 212, 89, 196, 185, 156, 49, 235, 32, 132, 254, 45, 21, 25, 216, 82, 151, 40, 59, 10, 2, 153, 5, 25, 212, 36, 232, 173, 139, 159, 107, 128, 96, 154, 18, 79, 242, 124, 91, 114, 237, 31, 179, 143, 192, 16, 222, 177, 146, 211, 94, 27, 215, 231, 46, 116, 18, 22, 3, 59, 4, 73, 2, 180, 4, 72, 2, 116, 3, 64, 14, 236, 3, 160, 208, 240, 227, 61, 113, 71, 204, 246, 176, 192, 244, 55, 17, 51, 66, 160, 125, 71, 34, 249, 92, 22, 22, 93, 67, 166, 198, 198, 1, 144, 248, 163, 95, 250, 200, 63, 195, 63, 58, 57, 229, 132, 34, 128, 100, 38, 243, 112, 189, 89, 151, 209, 145, 81, 201, 229, 179, 128, 231, 124, 67, 92, 24, 99, 48, 195, 126, 140, 59, 18, 208, 187, 187, 88, 152, 231, 242, 33, 44, 194, 87, 136, 232, 107, 68, 176, 97, 28, 166, 126, 156, 31, 174, 244, 209, 56, 18, 0, 61, 192, 16, 79, 66, 219, 34, 206, 64, 82, 160, 223, 252, 200, 215, 96, 50, 223, 207, 125, 117, 84, 118, 20, 229, 158, 79, 134, 170, 133, 247, 94, 139, 253, 131, 128, 8, 204, 4, 52, 117, 178, 191, 38, 105, 152, 240, 126, 223, 180, 120, 253, 19, 146, 89, 51, 37, 171, 207, 108, 169, 150, 239, 221, 52, 7, 77, 143, 37, 44, 129, 222, 141, 38, 174, 97, 179, 177, 4, 52, 88, 130, 88, 119, 78, 93, 18, 3, 163, 18, 102, 14, 73, 122, 117, 69, 146, 43, 64, 2, 253, 168, 183, 167, 14, 183, 130, 214, 0, 136, 0, 36, 240, 200, 215, 154, 242, 205, 95, 9, 101, 215, 119, 143, 112, 45, 151, 65, 86, 156, 17, 147, 117, 151, 226, 26, 19, 224, 234, 10, 0, 252, 180, 4, 108, 208, 52, 62, 188, 197, 153, 129, 173, 24, 204, 127, 67, 204, 166, 77, 28, 89, 120, 207, 218, 247, 13, 13, 32, 149, 78, 72, 185, 56, 39, 179, 211, 179, 226, 5, 254, 73, 235, 255, 83, 78, 40, 2, 136, 251, 254, 110, 220, 168, 198, 240, 129, 97, 125, 44, 216, 115, 239, 111, 195, 77, 83, 107, 157, 1, 103, 20, 143, 3, 152, 88, 49, 150, 158, 53, 255, 52, 170, 234, 0, 41, 49, 237, 56, 116, 99, 190, 108, 27, 36, 11, 93, 231, 100, 16, 253, 208, 31, 191, 91, 111, 158, 9, 39, 9, 72, 0, 107, 1, 0, 16, 128, 63, 74, 53, 68, 0, 136, 24, 123, 208, 85, 59, 134, 50, 181, 39, 148, 171, 255, 144, 219, 29, 93, 41, 12, 71, 242, 237, 95, 13, 229, 123, 191, 27, 74, 179, 74, 242, 9, 213, 60, 15, 122, 26, 146, 0, 64, 147, 43, 171, 146, 94, 11, 5, 213, 51, 44, 201, 161, 49, 25, 218, 90, 2, 160, 91, 240, 249, 11, 240, 245, 139, 146, 95, 139, 176, 166, 40, 57, 250, 252, 8, 249, 117, 88, 135, 217, 223, 195, 62, 1, 88, 9, 234, 14, 108, 153, 147, 53, 103, 215, 97, 5, 20, 164, 145, 217, 39, 137, 213, 179, 32, 146, 50, 234, 70, 24, 4, 17, 244, 213, 196, 131, 53, 64, 139, 131, 228, 51, 254, 64, 83, 190, 251, 158, 166, 60, 244, 197, 80, 90, 141, 229, 39, 130, 109, 175, 240, 112, 94, 184, 139, 32, 1, 62, 169, 73, 87, 32, 170, 3, 240, 124, 86, 163, 1, 226, 39, 1, 224, 158, 199, 104, 225, 69, 184, 79, 218, 78, 28, 17, 176, 9, 24, 83, 223, 201, 2, 210, 70, 52, 153, 74, 202, 248, 161, 131, 82, 169, 212, 196, 79, 231, 30, 176, 57, 39, 165, 88, 4, 156, 24, 50, 180, 102, 245, 65, 241, 100, 124, 108, 228, 160, 2, 214, 124, 6, 220, 220, 60, 158, 136, 187, 141, 29, 247, 246, 113, 98, 64, 110, 87, 156, 96, 253, 113, 155, 176, 12, 3, 125, 127, 118, 248, 193, 2, 16, 0, 62, 150, 172, 3, 8, 117, 137, 101, 97, 5, 96, 25, 7, 25, 168, 53, 0, 114, 216, 117, 117, 40, 215, 253, 89, 71, 143, 211, 83, 148, 221, 223, 143, 228, 43, 63, 31, 202, 3, 159, 111, 2, 104, 236, 228, 131, 230, 167, 217, 207, 161, 188, 129, 186, 164, 86, 214, 36, 179, 182, 38, 217, 117, 21, 137, 247, 141, 72, 124, 104, 90, 86, 159, 222, 0, 200, 217, 15, 80, 146, 172, 246, 248, 215, 37, 141, 178, 105, 90, 10, 48, 237, 51, 0, 116, 118, 69, 69, 71, 6, 178, 40, 67, 66, 96, 167, 96, 223, 166, 138, 172, 61, 15, 102, 112, 122, 191, 120, 3, 99, 146, 89, 95, 70, 221, 32, 128, 85, 101, 9, 6, 171, 176, 48, 170, 32, 1, 156, 175, 186, 4, 161, 204, 30, 2, 41, 253, 78, 83, 30, 254, 82, 180, 236, 36, 176, 238, 25, 49, 25, 218, 14, 77, 207, 233, 216, 116, 3, 160, 245, 227, 173, 0, 30, 25, 223, 216, 204, 145, 0, 14, 5, 122, 18, 234, 123, 2, 112, 19, 137, 125, 123, 136, 237, 182, 209, 113, 200, 52, 253, 41, 108, 75, 108, 31, 124, 251, 244, 244, 248, 4, 202, 134, 181, 92, 95, 255, 46, 205, 60, 73, 229, 132, 34, 128, 247, 253, 229, 63, 76, 198, 227, 254, 97, 206, 207, 110, 54, 106, 146, 72, 165, 112, 131, 57, 222, 223, 194, 253, 132, 137, 174, 165, 72, 10, 104, 0, 88, 211, 41, 194, 170, 241, 57, 108, 7, 205, 16, 71, 227, 176, 67, 135, 42, 154, 111, 162, 157, 156, 64, 27, 128, 157, 72, 198, 42, 64, 227, 128, 175, 207, 239, 216, 83, 227, 19, 244, 177, 44, 66, 174, 138, 80, 51, 241, 52, 180, 35, 243, 64, 2, 55, 255, 99, 83, 110, 253, 71, 248, 232, 104, 124, 63, 173, 240, 123, 251, 183, 253, 159, 80, 174, 129, 69, 113, 240, 214, 150, 154, 177, 113, 88, 28, 94, 6, 190, 62, 193, 63, 8, 211, 159, 224, 94, 83, 149, 28, 180, 127, 106, 229, 172, 132, 217, 195, 0, 245, 172, 172, 217, 30, 195, 178, 96, 135, 249, 96, 37, 228, 235, 18, 64, 123, 235, 184, 191, 142, 253, 51, 173, 169, 243, 1, 210, 3, 168, 3, 224, 206, 66, 211, 103, 87, 215, 100, 211, 185, 105, 152, 253, 112, 3, 210, 112, 3, 214, 149, 37, 181, 174, 36, 105, 88, 2, 233, 213, 101, 73, 12, 209, 37, 0, 17, 168, 75, 128, 243, 133, 37, 80, 158, 108, 201, 117, 127, 210, 146, 157, 223, 178, 7, 190, 76, 210, 183, 145, 147, 131, 8, 108, 4, 90, 0, 150, 4, 248, 132, 38, 193, 31, 11, 17, 34, 14, 1, 34, 15, 150, 128, 153, 23, 200, 219, 61, 239, 52, 46, 208, 250, 16, 150, 96, 219, 240, 125, 79, 195, 190, 221, 187, 113, 239, 99, 99, 253, 131, 3, 39, 213, 151, 128, 22, 203, 9, 69, 0, 111, 122, 206, 133, 184, 213, 177, 71, 71, 15, 13, 75, 97, 118, 74, 95, 14, 98, 58, 120, 56, 197, 147, 154, 23, 55, 149, 38, 60, 111, 126, 200, 142, 178, 249, 206, 31, 211, 31, 192, 6, 192, 50, 11, 111, 62, 133, 175, 132, 86, 70, 128, 232, 232, 31, 234, 209, 222, 98, 101, 2, 212, 205, 14, 64, 250, 252, 0, 187, 130, 63, 143, 208, 195, 142, 183, 178, 196, 25, 39, 49, 128, 4, 120, 28, 63, 248, 51, 250, 201, 45, 53, 223, 127, 18, 225, 252, 253, 177, 7, 34, 185, 249, 175, 1, 254, 247, 215, 101, 226, 145, 80, 60, 184, 24, 170, 245, 9, 98, 118, 246, 1, 180, 73, 104, 112, 250, 233, 153, 53, 208, 226, 176, 0, 188, 222, 73, 248, 238, 227, 146, 91, 89, 146, 129, 141, 113, 73, 246, 86, 37, 129, 242, 62, 180, 181, 110, 159, 136, 108, 64, 125, 8, 126, 138, 110, 68, 75, 2, 0, 57, 145, 107, 162, 60, 39, 10, 85, 101, 197, 102, 95, 122, 0, 246, 106, 180, 87, 82, 171, 8, 254, 146, 164, 52, 128, 12, 86, 85, 224, 98, 128, 0, 80, 55, 45, 1, 30, 19, 59, 67, 231, 14, 182, 228, 150, 191, 111, 201, 222, 107, 126, 178, 115, 125, 42, 146, 236, 137, 201, 41, 207, 52, 224, 214, 217, 129, 36, 1, 16, 128, 206, 1, 96, 159, 0, 45, 2, 144, 64, 100, 137, 128, 22, 0, 59, 118, 93, 231, 46, 193, 191, 152, 0, 216, 142, 194, 86, 75, 223, 60, 157, 128, 101, 57, 10, 37, 227, 249, 222, 161, 15, 124, 232, 195, 39, 221, 75, 64, 58, 229, 132, 34, 0, 74, 38, 151, 31, 110, 212, 234, 82, 131, 127, 150, 74, 37, 108, 170, 153, 237, 101, 180, 189, 147, 197, 32, 39, 248, 169, 217, 145, 238, 250, 4, 172, 232, 26, 26, 135, 241, 11, 81, 6, 214, 130, 27, 86, 52, 4, 128, 52, 186, 1, 104, 240, 106, 242, 83, 235, 3, 252, 210, 11, 240, 247, 193, 252, 238, 129, 37, 0, 80, 56, 107, 160, 37, 77, 185, 255, 115, 77, 249, 194, 107, 155, 48, 223, 67, 213, 232, 63, 202, 76, 158, 59, 24, 201, 93, 255, 26, 202, 21, 111, 106, 200, 61, 255, 86, 87, 210, 209, 33, 62, 128, 148, 90, 151, 29, 114, 9, 104, 235, 20, 52, 63, 193, 152, 94, 13, 223, 31, 36, 144, 134, 230, 142, 82, 227, 226, 231, 225, 215, 175, 68, 3, 238, 105, 2, 216, 0, 40, 204, 116, 5, 124, 192, 78, 76, 88, 64, 80, 144, 12, 140, 199, 65, 102, 92, 106, 126, 170, 14, 66, 192, 54, 56, 246, 220, 138, 72, 86, 108, 244, 164, 210, 58, 8, 178, 41, 74, 122, 21, 246, 5, 66, 160, 27, 160, 174, 192, 80, 89, 252, 254, 10, 172, 16, 144, 14, 172, 9, 118, 12, 114, 52, 100, 228, 174, 150, 220, 248, 87, 45, 153, 218, 241, 228, 231, 120, 52, 101, 197, 217, 49, 144, 150, 189, 151, 234, 10, 224, 158, 209, 10, 32, 17, 132, 236, 0, 196, 18, 121, 180, 238, 91, 122, 135, 77, 159, 80, 39, 238, 59, 137, 128, 207, 5, 240, 245, 96, 25, 88, 149, 117, 180, 173, 241, 209, 113, 110, 245, 240, 207, 93, 122, 254, 209, 239, 216, 57, 142, 228, 132, 35, 128, 152, 120, 7, 104, 219, 15, 239, 59, 40, 249, 124, 143, 50, 60, 31, 214, 229, 143, 131, 122, 109, 127, 142, 183, 15, 129, 29, 128, 45, 148, 81, 246, 199, 77, 166, 134, 230, 243, 224, 45, 44, 77, 73, 35, 154, 141, 77, 204, 199, 33, 184, 110, 126, 90, 6, 105, 17, 220, 0, 154, 189, 49, 104, 206, 40, 91, 21, 201, 211, 239, 70, 0, 32, 98, 253, 36, 130, 42, 136, 0, 1, 64, 138, 211, 74, 0, 56, 198, 30, 110, 202, 215, 222, 217, 144, 47, 188, 134, 211, 108, 67, 121, 244, 235, 161, 236, 187, 62, 148, 145, 187, 35, 25, 127, 40, 146, 209, 251, 34, 121, 232, 191, 66, 249, 214, 175, 54, 229, 19, 207, 40, 203, 117, 31, 168, 75, 113, 12, 251, 72, 214, 80, 15, 128, 207, 41, 187, 240, 221, 3, 248, 238, 9, 213, 250, 166, 195, 47, 189, 6, 228, 7, 243, 63, 181, 10, 121, 48, 205, 171, 241, 113, 128, 185, 32, 3, 107, 83, 170, 221, 227, 1, 129, 79, 208, 211, 245, 1, 113, 197, 176, 212, 192, 56, 174, 12, 59, 72, 73, 104, 36, 4, 142, 114, 4, 240, 114, 18, 32, 141, 84, 36, 171, 55, 100, 164, 17, 77, 224, 28, 96, 250, 15, 134, 208, 250, 53, 184, 27, 112, 51, 64, 58, 137, 21, 176, 44, 64, 66, 30, 8, 143, 157, 130, 94, 6, 117, 250, 56, 87, 184, 62, 7, 111, 195, 57, 126, 108, 249, 8, 128, 51, 3, 215, 94, 64, 237, 142, 243, 225, 236, 64, 245, 251, 141, 246, 55, 95, 20, 198, 249, 161, 157, 240, 73, 78, 114, 184, 105, 13, 38, 80, 28, 248, 13, 233, 91, 65, 52, 159, 207, 200, 228, 244, 140, 204, 205, 204, 74, 42, 157, 58, 105, 167, 0, 59, 57, 225, 8, 192, 15, 130, 219, 125, 223, 175, 221, 118, 245, 117, 210, 147, 73, 74, 111, 111, 94, 77, 125, 220, 77, 91, 194, 221, 84, 7, 97, 39, 76, 71, 170, 22, 53, 169, 238, 222, 183, 203, 160, 10, 86, 195, 237, 153, 166, 132, 128, 31, 123, 24, 56, 233, 135, 36, 16, 6, 70, 203, 107, 135, 24, 76, 231, 152, 146, 0, 8, 128, 161, 151, 214, 0, 72, 32, 15, 0, 211, 37, 64, 57, 186, 12, 227, 59, 27, 242, 253, 247, 55, 228, 139, 111, 169, 203, 149, 191, 219, 68, 104, 200, 87, 65, 12, 95, 252, 111, 117, 249, 206, 111, 53, 228, 222, 207, 212, 165, 52, 3, 48, 3, 84, 244, 205, 189, 108, 73, 146, 208, 248, 236, 124, 11, 96, 118, 107, 111, 60, 53, 49, 204, 241, 52, 128, 159, 134, 233, 79, 96, 38, 56, 94, 15, 127, 190, 25, 77, 67, 227, 87, 165, 111, 101, 82, 193, 175, 224, 182, 192, 55, 173, 223, 158, 152, 6, 158, 39, 211, 45, 33, 160, 5, 48, 196, 129, 159, 56, 192, 60, 116, 74, 31, 72, 175, 12, 92, 21, 205, 179, 4, 3, 176, 14, 250, 176, 47, 28, 11, 131, 63, 80, 17, 143, 231, 156, 51, 86, 128, 144, 56, 96, 5, 212, 171, 77, 121, 240, 243, 45, 57, 124, 103, 251, 106, 46, 169, 36, 114, 48, 192, 224, 238, 168, 69, 167, 29, 125, 214, 21, 80, 119, 192, 6, 130, 223, 156, 176, 246, 229, 82, 184, 198, 27, 207, 105, 211, 102, 220, 31, 119, 216, 6, 190, 59, 130, 29, 128, 52, 255, 155, 141, 90, 228, 167, 147, 39, 245, 16, 32, 5, 87, 233, 196, 146, 223, 249, 240, 135, 126, 152, 202, 102, 110, 218, 189, 227, 81, 249, 220, 191, 252, 139, 172, 26, 204, 234, 176, 13, 238, 31, 132, 255, 216, 23, 0, 200, 242, 134, 18, 190, 28, 10, 210, 116, 19, 244, 25, 0, 0, 65, 63, 0, 98, 137, 64, 155, 8, 254, 105, 99, 65, 163, 209, 90, 56, 127, 212, 84, 106, 202, 177, 80, 156, 230, 51, 234, 131, 121, 45, 4, 56, 128, 238, 17, 244, 0, 69, 124, 160, 36, 241, 33, 4, 198, 97, 21, 196, 225, 43, 147, 12, 148, 40, 104, 21, 104, 104, 200, 228, 222, 186, 28, 190, 31, 254, 253, 78, 104, 251, 137, 38, 192, 198, 206, 61, 16, 70, 138, 29, 119, 102, 158, 190, 223, 59, 167, 230, 189, 154, 222, 0, 189, 134, 213, 37, 29, 154, 35, 248, 83, 43, 0, 72, 128, 146, 132, 225, 167, 171, 210, 108, 21, 37, 72, 242, 43, 74, 73, 11, 104, 61, 35, 252, 153, 115, 94, 24, 104, 211, 112, 9, 177, 231, 101, 172, 3, 44, 113, 126, 249, 158, 172, 18, 65, 163, 57, 7, 183, 32, 210, 33, 199, 68, 47, 72, 0, 102, 63, 173, 145, 196, 0, 142, 15, 231, 231, 225, 252, 104, 161, 72, 162, 170, 163, 32, 81, 172, 41, 229, 153, 72, 238, 251, 180, 218, 76, 75, 46, 212, 222, 249, 213, 36, 45, 28, 184, 118, 6, 50, 224, 228, 25, 232, 2, 240, 57, 0, 62, 19, 128, 188, 118, 35, 215, 211, 53, 154, 159, 207, 251, 155, 199, 194, 205, 58, 11, 5, 73, 95, 242, 217, 172, 140, 29, 220, 207, 29, 20, 122, 6, 6, 31, 181, 91, 158, 180, 210, 190, 54, 39, 138, 188, 247, 13, 175, 107, 6, 153, 244, 215, 248, 66, 128, 59, 174, 187, 65, 190, 246, 233, 79, 203, 41, 235, 87, 73, 144, 72, 224, 6, 147, 201, 205, 140, 174, 150, 141, 147, 8, 120, 231, 249, 51, 141, 159, 13, 221, 222, 120, 93, 51, 162, 13, 67, 83, 16, 80, 212, 61, 43, 206, 182, 161, 169, 252, 71, 173, 138, 6, 199, 113, 127, 246, 5, 168, 150, 135, 57, 28, 103, 95, 0, 8, 192, 27, 44, 139, 183, 2, 75, 18, 1, 253, 101, 174, 15, 32, 88, 192, 48, 144, 24, 212, 132, 118, 128, 26, 4, 49, 244, 204, 33, 109, 90, 122, 215, 113, 156, 190, 4, 45, 59, 33, 169, 181, 0, 251, 186, 162, 1, 255, 90, 128, 31, 65, 173, 128, 65, 130, 191, 174, 61, 249, 58, 93, 55, 168, 73, 179, 89, 213, 233, 202, 169, 52, 71, 57, 216, 202, 177, 194, 227, 85, 49, 231, 63, 31, 22, 11, 79, 144, 219, 224, 188, 98, 45, 201, 15, 246, 72, 42, 155, 144, 74, 9, 86, 5, 59, 14, 225, 82, 4, 217, 150, 118, 66, 38, 122, 64, 56, 56, 110, 31, 228, 163, 75, 90, 64, 41, 156, 83, 170, 142, 253, 55, 80, 123, 40, 15, 127, 41, 212, 103, 21, 150, 67, 56, 23, 128, 47, 17, 85, 237, 79, 192, 235, 18, 46, 0, 72, 128, 47, 106, 161, 55, 200, 35, 225, 125, 197, 157, 83, 139, 174, 147, 158, 218, 224, 135, 176, 125, 100, 50, 41, 73, 39, 147, 112, 47, 247, 33, 33, 58, 176, 253, 156, 115, 71, 52, 243, 36, 150, 19, 142, 0, 40, 233, 129, 193, 43, 253, 32, 57, 203, 27, 123, 223, 93, 119, 201, 253, 183, 222, 34, 253, 253, 61, 184, 103, 182, 128, 21, 190, 4, 210, 53, 0, 189, 251, 140, 219, 27, 254, 164, 194, 226, 54, 74, 49, 155, 210, 92, 70, 77, 124, 18, 16, 174, 128, 192, 239, 213, 97, 193, 52, 0, 156, 7, 8, 96, 22, 123, 131, 0, 254, 96, 81, 188, 149, 0, 241, 202, 2, 200, 160, 136, 37, 2, 72, 193, 103, 218, 10, 144, 2, 2, 73, 34, 192, 50, 1, 211, 62, 5, 77, 239, 247, 79, 129, 8, 198, 117, 250, 110, 207, 198, 89, 73, 172, 158, 145, 244, 6, 152, 254, 107, 45, 1, 160, 92, 98, 168, 34, 9, 246, 7, 0, 252, 129, 157, 171, 31, 192, 103, 143, 195, 53, 105, 133, 117, 73, 195, 29, 34, 112, 121, 236, 230, 95, 103, 248, 81, 194, 179, 69, 128, 37, 16, 0, 244, 201, 68, 92, 234, 141, 138, 18, 158, 142, 126, 248, 77, 128, 188, 213, 238, 151, 240, 45, 129, 121, 32, 4, 62, 121, 40, 126, 69, 34, 31, 100, 0, 235, 168, 52, 21, 201, 190, 31, 128, 10, 158, 194, 48, 232, 143, 43, 185, 213, 49, 184, 76, 246, 252, 104, 238, 131, 0, 104, 13, 232, 151, 126, 225, 255, 179, 255, 135, 46, 2, 184, 220, 150, 153, 95, 56, 179, 223, 40, 8, 174, 163, 190, 76, 90, 26, 181, 154, 12, 239, 63, 200, 239, 0, 238, 249, 206, 103, 62, 123, 82, 190, 4, 164, 83, 78, 72, 2, 248, 205, 63, 125, 239, 158, 100, 220, 123, 236, 220, 103, 61, 79, 242, 185, 62, 121, 240, 135, 119, 75, 79, 38, 161, 62, 156, 42, 51, 189, 195, 188, 169, 236, 244, 163, 38, 231, 137, 34, 143, 140, 207, 135, 123, 8, 138, 69, 184, 88, 12, 19, 126, 70, 138, 219, 177, 237, 104, 251, 33, 113, 160, 226, 16, 166, 110, 220, 131, 246, 167, 233, 73, 18, 96, 79, 120, 166, 33, 30, 135, 2, 225, 14, 248, 208, 254, 30, 65, 14, 147, 221, 95, 3, 50, 208, 101, 65, 252, 213, 5, 9, 176, 12, 86, 207, 73, 2, 203, 36, 180, 123, 106, 125, 73, 114, 27, 1, 236, 149, 227, 48, 247, 71, 165, 127, 107, 73, 178, 107, 39, 17, 159, 150, 52, 44, 0, 18, 132, 6, 246, 5, 112, 90, 174, 213, 250, 62, 71, 7, 56, 148, 7, 2, 16, 28, 79, 216, 130, 31, 142, 179, 242, 3, 104, 63, 215, 202, 219, 75, 202, 147, 221, 102, 227, 50, 153, 242, 48, 141, 253, 148, 228, 178, 190, 148, 75, 101, 164, 218, 254, 4, 31, 1, 231, 202, 94, 127, 63, 215, 210, 126, 7, 18, 129, 151, 133, 27, 192, 225, 192, 128, 22, 72, 85, 231, 75, 112, 180, 100, 247, 213, 145, 84, 151, 97, 240, 44, 51, 4, 101, 224, 70, 2, 148, 165, 113, 207, 112, 42, 33, 39, 128, 64, 204, 61, 103, 234, 188, 152, 82, 70, 140, 133, 215, 94, 147, 108, 54, 45, 229, 114, 73, 38, 166, 38, 112, 29, 146, 247, 219, 140, 147, 90, 78, 72, 2, 248, 253, 55, 188, 165, 217, 172, 55, 134, 225, 163, 201, 166, 179, 207, 145, 67, 251, 15, 129, 185, 1, 36, 152, 111, 77, 178, 191, 45, 167, 99, 255, 88, 81, 16, 235, 205, 182, 167, 203, 2, 174, 16, 164, 35, 106, 4, 155, 153, 121, 3, 216, 14, 27, 183, 27, 17, 151, 32, 1, 99, 9, 96, 43, 104, 95, 99, 9, 128, 16, 232, 235, 43, 9, 192, 26, 232, 47, 194, 18, 128, 5, 64, 237, 191, 26, 254, 57, 180, 120, 0, 109, 30, 172, 5, 1, 172, 43, 72, 130, 166, 253, 250, 130, 100, 54, 130, 0, 54, 1, 68, 43, 39, 36, 179, 110, 66, 159, 220, 75, 173, 154, 132, 137, 61, 14, 203, 0, 219, 12, 1, 248, 170, 105, 81, 55, 59, 220, 56, 4, 137, 253, 169, 102, 214, 142, 62, 64, 183, 89, 199, 49, 194, 79, 79, 121, 122, 92, 70, 220, 9, 50, 240, 156, 185, 116, 121, 79, 36, 188, 74, 40, 135, 74, 83, 185, 180, 212, 107, 32, 0, 144, 39, 235, 228, 7, 153, 217, 185, 152, 72, 155, 249, 3, 250, 108, 0, 135, 26, 211, 56, 239, 44, 172, 133, 168, 36, 17, 72, 145, 163, 14, 36, 201, 201, 71, 34, 41, 47, 195, 243, 115, 94, 0, 227, 35, 133, 136, 59, 85, 61, 5, 156, 167, 146, 1, 206, 91, 181, 63, 2, 207, 157, 249, 204, 102, 64, 91, 88, 220, 1, 200, 235, 147, 66, 251, 25, 31, 30, 145, 74, 17, 86, 87, 54, 253, 144, 110, 112, 146, 203, 9, 73, 0, 42, 177, 216, 35, 124, 101, 211, 169, 231, 158, 35, 205, 122, 21, 55, 238, 176, 244, 246, 164, 113, 99, 205, 12, 64, 243, 141, 0, 7, 120, 189, 237, 10, 100, 141, 235, 58, 197, 130, 2, 13, 133, 109, 135, 14, 131, 110, 165, 217, 200, 195, 230, 10, 126, 22, 227, 82, 203, 115, 168, 209, 204, 60, 244, 84, 59, 26, 18, 136, 2, 99, 9, 232, 236, 64, 154, 200, 156, 53, 55, 88, 150, 0, 166, 127, 2, 96, 78, 66, 163, 27, 191, 30, 97, 125, 89, 114, 167, 148, 164, 103, 83, 89, 6, 78, 173, 65, 235, 195, 228, 95, 53, 37, 189, 52, 251, 161, 241, 115, 107, 27, 146, 30, 106, 73, 18, 224, 103, 111, 123, 44, 133, 144, 128, 214, 77, 0, 144, 0, 34, 193, 207, 99, 165, 149, 93, 111, 54, 209, 200, 81, 54, 1, 52, 8, 3, 133, 39, 160, 104, 176, 75, 138, 59, 231, 39, 18, 230, 155, 242, 41, 184, 19, 173, 42, 220, 21, 172, 235, 231, 177, 96, 237, 4, 1, 150, 28, 90, 36, 1, 5, 0, 62, 72, 79, 31, 140, 74, 115, 11, 16, 31, 39, 75, 241, 194, 225, 154, 85, 166, 69, 202, 147, 90, 233, 146, 74, 28, 199, 164, 47, 110, 166, 16, 236, 26, 112, 135, 176, 228, 143, 179, 190, 25, 52, 27, 129, 131, 69, 204, 239, 252, 40, 8, 51, 180, 44, 110, 50, 159, 2, 28, 57, 124, 8, 101, 162, 90, 58, 157, 57, 233, 135, 0, 41, 246, 242, 156, 120, 18, 4, 137, 199, 162, 86, 83, 122, 250, 123, 163, 161, 213, 171, 101, 199, 67, 15, 233, 24, 174, 121, 241, 7, 89, 29, 218, 145, 55, 220, 246, 4, 153, 155, 143, 127, 218, 20, 24, 32, 196, 179, 93, 154, 54, 65, 173, 74, 125, 97, 26, 147, 102, 185, 6, 196, 2, 248, 211, 60, 254, 248, 146, 73, 19, 131, 249, 139, 152, 90, 3, 244, 149, 105, 30, 179, 215, 28, 190, 178, 118, 242, 153, 241, 250, 44, 124, 253, 252, 186, 26, 2, 142, 249, 148, 166, 12, 108, 9, 101, 104, 11, 128, 148, 63, 40, 173, 228, 126, 89, 177, 21, 38, 232, 74, 14, 185, 177, 151, 31, 190, 120, 14, 199, 129, 186, 248, 150, 158, 16, 128, 163, 121, 205, 79, 93, 235, 241, 145, 120, 12, 23, 233, 87, 110, 120, 206, 190, 190, 253, 212, 17, 192, 98, 177, 231, 123, 68, 97, 30, 131, 35, 140, 8, 254, 175, 207, 247, 224, 235, 11, 85, 249, 1, 85, 3, 16, 228, 56, 255, 138, 231, 138, 99, 225, 146, 214, 65, 83, 223, 184, 129, 99, 211, 75, 22, 73, 163, 20, 73, 109, 150, 241, 165, 21, 31, 228, 99, 196, 222, 43, 222, 51, 13, 52, 254, 57, 202, 163, 78, 128, 230, 241, 246, 241, 208, 121, 134, 92, 113, 22, 128, 89, 5, 209, 249, 190, 100, 51, 105, 57, 176, 127, 47, 206, 41, 56, 188, 238, 148, 205, 39, 229, 107, 192, 23, 203, 9, 75, 0, 126, 224, 143, 214, 170, 240, 59, 97, 163, 174, 217, 180, 69, 14, 238, 218, 5, 55, 0, 13, 145, 173, 16, 55, 148, 63, 189, 209, 218, 7, 96, 134, 5, 117, 190, 128, 19, 70, 17, 180, 113, 235, 101, 48, 13, 133, 194, 44, 194, 156, 127, 228, 15, 102, 177, 62, 154, 196, 4, 155, 233, 75, 176, 13, 11, 63, 37, 9, 2, 130, 67, 132, 9, 227, 43, 243, 113, 221, 84, 79, 83, 114, 3, 117, 132, 166, 228, 87, 68, 210, 183, 54, 38, 67, 167, 136, 244, 174, 170, 72, 51, 126, 80, 30, 187, 255, 155, 50, 114, 224, 106, 57, 255, 185, 189, 114, 241, 75, 183, 169, 198, 79, 100, 249, 4, 26, 136, 129, 125, 11, 212, 246, 208, 172, 10, 122, 236, 35, 84, 144, 26, 49, 96, 155, 151, 121, 114, 235, 204, 56, 82, 218, 98, 113, 101, 230, 67, 58, 147, 130, 85, 69, 215, 130, 20, 135, 173, 113, 174, 172, 159, 230, 180, 214, 4, 128, 185, 221, 241, 213, 105, 117, 240, 83, 196, 239, 39, 232, 181, 199, 113, 210, 24, 224, 227, 24, 75, 44, 156, 11, 16, 114, 248, 175, 13, 124, 28, 159, 118, 4, 122, 48, 70, 204, 140, 64, 222, 91, 30, 53, 174, 32, 242, 237, 193, 89, 105, 143, 0, 32, 61, 147, 74, 130, 32, 66, 57, 188, 255, 160, 164, 178, 249, 253, 183, 95, 119, 213, 50, 216, 48, 199, 94, 78, 88, 2, 168, 214, 171, 7, 107, 245, 90, 131, 159, 122, 106, 54, 170, 146, 31, 88, 37, 149, 6, 0, 200, 251, 219, 106, 168, 230, 87, 224, 115, 21, 192, 55, 247, 154, 107, 243, 55, 190, 19, 23, 46, 141, 11, 198, 149, 64, 180, 217, 160, 78, 68, 105, 74, 198, 216, 45, 102, 201, 68, 1, 135, 12, 87, 47, 59, 203, 140, 75, 0, 173, 237, 213, 37, 153, 129, 133, 156, 245, 36, 221, 235, 75, 174, 63, 33, 153, 124, 36, 149, 234, 176, 60, 246, 240, 15, 228, 158, 155, 190, 40, 149, 241, 155, 229, 185, 47, 89, 37, 175, 122, 251, 197, 114, 198, 197, 167, 160, 28, 53, 90, 77, 130, 36, 172, 133, 44, 159, 116, 36, 240, 89, 55, 247, 107, 52, 112, 136, 253, 209, 248, 231, 161, 233, 185, 225, 28, 117, 166, 27, 39, 246, 96, 223, 230, 118, 210, 10, 48, 199, 62, 47, 157, 241, 39, 18, 158, 8, 9, 6, 22, 75, 154, 22, 64, 40, 149, 90, 133, 103, 143, 125, 33, 7, 161, 133, 125, 133, 237, 201, 54, 216, 79, 3, 224, 210, 183, 239, 4, 216, 51, 151, 180, 199, 81, 30, 247, 160, 119, 3, 235, 92, 90, 73, 230, 113, 180, 112, 55, 244, 244, 56, 23, 192, 6, 125, 16, 136, 195, 129, 81, 2, 89, 9, 220, 53, 206, 9, 32, 129, 207, 187, 135, 78, 168, 32, 72, 236, 124, 213, 60, 71, 0, 166, 135, 39, 37, 108, 212, 238, 181, 217, 39, 189, 216, 38, 118, 226, 73, 171, 81, 47, 180, 154, 141, 90, 185, 88, 144, 177, 3, 251, 229, 130, 231, 61, 79, 166, 199, 39, 165, 183, 191, 71, 146, 233, 20, 252, 85, 122, 176, 70, 148, 253, 173, 24, 77, 105, 69, 177, 100, 127, 104, 4, 20, 151, 173, 64, 195, 127, 183, 41, 23, 58, 202, 160, 228, 96, 192, 224, 222, 53, 65, 160, 210, 20, 166, 105, 78, 18, 72, 4, 158, 36, 146, 236, 57, 139, 100, 106, 98, 68, 30, 184, 237, 6, 185, 230, 43, 255, 46, 247, 94, 247, 57, 25, 200, 236, 147, 55, 190, 237, 12, 121, 237, 59, 158, 37, 27, 207, 88, 13, 191, 30, 214, 2, 204, 124, 143, 75, 104, 124, 126, 140, 146, 67, 141, 102, 102, 30, 136, 5, 251, 236, 124, 132, 153, 199, 175, 36, 133, 56, 251, 34, 226, 48, 209, 117, 165, 125, 94, 182, 160, 30, 229, 143, 146, 206, 50, 243, 241, 116, 22, 192, 65, 125, 230, 19, 91, 16, 100, 81, 251, 135, 77, 212, 221, 240, 244, 181, 91, 97, 29, 161, 26, 72, 196, 183, 240, 180, 18, 216, 43, 191, 214, 132, 124, 150, 131, 69, 48, 116, 134, 59, 142, 165, 147, 32, 43, 50, 189, 211, 30, 183, 30, 35, 151, 36, 0, 67, 2, 26, 103, 19, 159, 87, 250, 42, 11, 218, 0, 5, 171, 28, 70, 158, 28, 159, 144, 137, 201, 113, 73, 229, 178, 124, 13, 248, 211, 66, 78, 88, 2, 88, 181, 118, 125, 25, 139, 226, 174, 123, 238, 145, 117, 91, 54, 75, 239, 208, 10, 172, 134, 178, 247, 161, 123, 229, 219, 159, 253, 180, 76, 141, 28, 86, 224, 240, 94, 235, 13, 111, 3, 156, 67, 131, 116, 13, 200, 254, 154, 212, 150, 246, 170, 150, 165, 54, 51, 206, 129, 161, 8, 130, 16, 141, 154, 170, 216, 168, 125, 164, 25, 50, 48, 251, 128, 54, 140, 241, 109, 194, 8, 112, 158, 247, 60, 242, 128, 92, 243, 229, 207, 200, 21, 255, 250, 113, 16, 192, 213, 178, 125, 123, 82, 94, 251, 230, 243, 228, 103, 126, 246, 34, 89, 179, 62, 143, 237, 0, 116, 98, 151, 62, 54, 223, 113, 167, 254, 61, 172, 22, 104, 121, 106, 93, 84, 100, 128, 78, 243, 31, 102, 13, 247, 228, 33, 161, 109, 189, 64, 120, 30, 113, 118, 254, 97, 191, 13, 253, 252, 49, 91, 250, 79, 242, 236, 202, 124, 93, 70, 236, 58, 175, 27, 226, 58, 134, 14, 150, 227, 176, 90, 216, 64, 188, 30, 151, 86, 213, 147, 176, 12, 240, 151, 3, 105, 49, 84, 104, 9, 36, 113, 181, 82, 40, 107, 64, 119, 218, 235, 120, 237, 150, 94, 10, 135, 112, 151, 248, 60, 24, 175, 63, 22, 60, 122, 66, 222, 252, 16, 167, 197, 4, 146, 52, 233, 237, 179, 211, 107, 200, 54, 65, 23, 135, 215, 48, 145, 240, 101, 96, 160, 95, 14, 29, 56, 44, 205, 102, 163, 229, 197, 253, 174, 5, 112, 188, 203, 154, 205, 155, 235, 51, 99, 227, 245, 145, 93, 123, 228, 89, 47, 126, 177, 204, 205, 204, 161, 193, 182, 100, 231, 3, 247, 203, 57, 207, 122, 62, 76, 240, 156, 154, 254, 4, 39, 27, 2, 77, 61, 138, 1, 43, 1, 109, 26, 141, 6, 38, 82, 52, 207, 68, 201, 14, 108, 28, 134, 48, 184, 45, 154, 15, 181, 138, 107, 70, 4, 163, 137, 177, 40, 26, 19, 215, 160, 182, 9, 26, 228, 164, 123, 243, 178, 229, 236, 179, 228, 153, 175, 120, 25, 172, 147, 75, 100, 114, 46, 45, 215, 92, 59, 37, 255, 241, 169, 123, 228, 19, 31, 189, 89, 62, 251, 47, 215, 202, 181, 223, 184, 94, 30, 187, 255, 49, 57, 176, 27, 13, 175, 81, 51, 187, 224, 15, 117, 155, 87, 158, 163, 38, 16, 128, 161, 4, 133, 164, 254, 16, 69, 186, 30, 130, 18, 8, 201, 170, 217, 38, 0, 6, 119, 18, 63, 74, 22, 151, 51, 235, 58, 141, 150, 231, 161, 47, 212, 64, 26, 8, 32, 226, 215, 144, 107, 0, 126, 197, 151, 102, 17, 192, 47, 194, 180, 46, 37, 65, 0, 190, 248, 177, 156, 37, 0, 28, 16, 174, 195, 185, 111, 51, 0, 91, 106, 225, 211, 135, 1, 92, 45, 94, 18, 220, 124, 13, 56, 108, 37, 81, 134, 86, 216, 148, 86, 11, 132, 168, 121, 248, 99, 208, 45, 141, 80, 65, 100, 146, 9, 89, 49, 52, 40, 169, 76, 86, 118, 61, 170, 51, 127, 39, 214, 108, 57, 117, 191, 22, 120, 26, 200, 9, 75, 0, 167, 158, 117, 102, 177, 90, 42, 77, 151, 139, 69, 217, 187, 115, 167, 118, 92, 249, 137, 132, 228, 251, 7, 100, 96, 213, 90, 248, 210, 41, 180, 4, 115, 227, 249, 143, 29, 128, 12, 124, 219, 107, 167, 176, 109, 16, 98, 157, 205, 213, 172, 153, 20, 130, 223, 104, 10, 211, 159, 96, 122, 154, 77, 30, 163, 174, 69, 41, 169, 144, 40, 184, 196, 111, 229, 154, 13, 178, 237, 156, 139, 229, 236, 75, 159, 43, 167, 93, 246, 82, 185, 240, 165, 63, 43, 207, 120, 197, 27, 229, 252, 23, 191, 94, 206, 126, 193, 235, 100, 235, 179, 95, 47, 173, 158, 243, 100, 255, 88, 86, 110, 254, 193, 46, 185, 234, 75, 215, 179, 22, 84, 103, 42, 100, 125, 140, 121, 182, 35, 64, 53, 63, 19, 8, 124, 4, 14, 91, 153, 52, 88, 10, 176, 74, 234, 85, 16, 136, 22, 96, 160, 184, 229, 79, 34, 216, 6, 39, 213, 168, 55, 21, 28, 252, 40, 106, 136, 106, 91, 4, 62, 52, 127, 171, 4, 224, 23, 0, 252, 66, 82, 154, 179, 88, 130, 8, 154, 69, 95, 18, 94, 31, 10, 193, 18, 128, 139, 112, 250, 107, 99, 210, 127, 42, 106, 226, 177, 45, 177, 20, 14, 99, 63, 218, 247, 129, 96, 65, 207, 217, 140, 12, 236, 190, 228, 19, 159, 6, 252, 166, 76, 231, 17, 241, 219, 255, 233, 84, 90, 242, 185, 180, 156, 178, 126, 53, 206, 185, 46, 123, 119, 236, 192, 121, 123, 187, 223, 255, 193, 143, 142, 218, 98, 39, 189, 44, 68, 195, 9, 36, 255, 254, 193, 127, 170, 123, 169, 212, 253, 17, 26, 192, 248, 200, 132, 4, 49, 248, 162, 0, 205, 218, 205, 155, 100, 110, 236, 160, 4, 190, 5, 14, 3, 7, 139, 97, 111, 27, 205, 143, 134, 208, 217, 56, 217, 72, 144, 206, 192, 84, 40, 92, 109, 60, 10, 110, 166, 107, 26, 193, 64, 50, 128, 70, 65, 249, 208, 100, 106, 91, 99, 186, 10, 86, 162, 168, 14, 253, 75, 173, 195, 143, 81, 180, 164, 92, 169, 75, 169, 84, 70, 168, 75, 177, 84, 147, 66, 177, 44, 21, 164, 177, 255, 128, 179, 88, 6, 214, 109, 150, 13, 103, 94, 36, 167, 61, 243, 5, 178, 103, 247, 148, 84, 81, 54, 73, 226, 226, 33, 209, 148, 87, 83, 220, 19, 47, 158, 144, 6, 142, 153, 51, 243, 76, 207, 188, 89, 114, 223, 236, 3, 240, 125, 248, 226, 208, 214, 243, 95, 191, 209, 51, 49, 209, 39, 21, 150, 235, 16, 0, 62, 138, 82, 210, 40, 53, 36, 9, 179, 56, 194, 126, 155, 85, 144, 103, 37, 9, 147, 31, 218, 30, 224, 111, 206, 17, 252, 41, 93, 182, 10, 198, 34, 224, 187, 248, 244, 17, 92, 104, 255, 139, 126, 213, 147, 100, 126, 81, 189, 75, 36, 252, 114, 81, 121, 2, 17, 222, 52, 130, 92, 135, 38, 113, 241, 60, 206, 96, 228, 245, 195, 125, 164, 91, 133, 159, 235, 0, 116, 183, 139, 66, 165, 145, 239, 203, 203, 170, 21, 253, 114, 96, 231, 14, 57, 180, 123, 175, 36, 210, 169, 135, 223, 120, 225, 25, 52, 163, 158, 22, 114, 194, 18, 0, 5, 152, 120, 128, 67, 129, 245, 90, 77, 123, 112, 203, 0, 217, 186, 173, 167, 201, 150, 51, 207, 70, 166, 245, 67, 45, 216, 245, 63, 254, 169, 73, 251, 56, 97, 3, 50, 23, 227, 201, 154, 238, 252, 150, 172, 200, 196, 44, 71, 24, 225, 198, 8, 124, 223, 0, 124, 73, 16, 65, 36, 141, 134, 72, 181, 22, 74, 165, 218, 4, 33, 180, 64, 6, 12, 13, 144, 65, 77, 230, 230, 74, 50, 51, 195, 89, 103, 3, 226, 231, 86, 201, 174, 135, 247, 235, 119, 15, 27, 117, 227, 183, 242, 253, 247, 218, 21, 193, 21, 8, 15, 157, 36, 70, 215, 164, 97, 31, 103, 245, 161, 201, 146, 48, 95, 105, 1, 52, 106, 216, 153, 22, 230, 65, 113, 105, 55, 252, 113, 68, 193, 79, 162, 163, 53, 209, 130, 133, 145, 129, 246, 247, 164, 14, 63, 191, 9, 115, 191, 49, 155, 148, 250, 52, 142, 109, 50, 45, 205, 105, 16, 0, 214, 233, 6, 132, 149, 64, 66, 190, 144, 19, 4, 112, 238, 219, 60, 89, 129, 75, 191, 28, 194, 15, 177, 20, 15, 155, 107, 161, 154, 159, 115, 19, 124, 88, 105, 124, 118, 1, 36, 16, 241, 197, 176, 236, 59, 65, 30, 203, 240, 154, 240, 231, 132, 74, 128, 196, 57, 208, 223, 175, 95, 14, 186, 246, 107, 223, 68, 59, 170, 66, 113, 248, 59, 108, 145, 167, 133, 156, 208, 4, 144, 206, 231, 31, 8, 129, 178, 116, 42, 41, 94, 130, 90, 62, 6, 18, 168, 203, 212, 100, 193, 60, 206, 11, 161, 85, 192, 177, 95, 119, 235, 9, 34, 163, 39, 105, 9, 96, 69, 157, 105, 142, 21, 35, 5, 171, 44, 167, 107, 170, 229, 41, 108, 64, 212, 238, 174, 134, 14, 209, 6, 198, 218, 208, 180, 184, 45, 0, 196, 239, 209, 177, 36, 167, 36, 115, 130, 12, 201, 160, 222, 104, 73, 21, 102, 117, 173, 209, 212, 101, 165, 26, 74, 153, 132, 0, 160, 149, 171, 13, 169, 161, 173, 174, 220, 124, 134, 236, 124, 120, 47, 234, 168, 233, 254, 168, 253, 217, 223, 72, 143, 133, 28, 102, 173, 88, 21, 186, 26, 218, 156, 89, 14, 18, 224, 252, 43, 21, 16, 33, 204, 88, 148, 100, 9, 77, 55, 241, 31, 37, 40, 131, 115, 141, 56, 132, 135, 205, 112, 184, 82, 158, 171, 3, 208, 0, 55, 76, 255, 86, 33, 0, 248, 19, 82, 159, 74, 73, 125, 34, 45, 245, 73, 88, 8, 211, 32, 1, 16, 128, 246, 5, 84, 97, 1, 52, 60, 89, 121, 118, 92, 191, 226, 155, 93, 241, 227, 236, 243, 169, 203, 212, 46, 145, 153, 125, 184, 6, 78, 243, 83, 211, 43, 240, 65, 140, 0, 127, 203, 174, 211, 25, 96, 63, 138, 94, 68, 43, 106, 69, 225, 134, 241, 218, 102, 97, 5, 60, 122, 255, 67, 114, 207, 157, 119, 65, 251, 231, 30, 73, 245, 244, 124, 219, 22, 123, 90, 200, 137, 77, 0, 169, 204, 99, 240, 132, 139, 1, 144, 146, 76, 5, 50, 184, 162, 79, 86, 175, 29, 16, 142, 151, 27, 191, 157, 64, 64, 192, 146, 239, 123, 35, 39, 184, 233, 162, 164, 1, 3, 160, 142, 134, 129, 192, 60, 229, 5, 52, 24, 5, 153, 201, 82, 32, 235, 83, 102, 118, 221, 216, 233, 72, 211, 58, 76, 170, 130, 95, 137, 131, 193, 196, 185, 79, 190, 212, 147, 189, 209, 205, 102, 83, 26, 92, 106, 231, 84, 40, 117, 152, 237, 236, 189, 231, 92, 134, 193, 181, 235, 229, 224, 254, 89, 105, 54, 42, 208, 76, 220, 55, 159, 89, 7, 1, 160, 42, 29, 120, 208, 61, 184, 227, 210, 113, 130, 118, 34, 136, 16, 231, 140, 122, 106, 36, 0, 55, 15, 128, 226, 182, 162, 48, 222, 185, 110, 69, 175, 7, 130, 14, 243, 193, 231, 71, 21, 245, 50, 202, 181, 178, 234, 235, 55, 102, 3, 0, 31, 218, 127, 28, 224, 71, 112, 22, 0, 137, 129, 35, 0, 17, 172, 132, 116, 79, 92, 191, 219, 167, 239, 235, 95, 38, 105, 150, 69, 14, 221, 78, 224, 227, 92, 219, 224, 199, 210, 199, 181, 245, 96, 125, 217, 169, 201, 36, 4, 243, 101, 96, 3, 122, 157, 198, 141, 37, 201, 50, 149, 48, 110, 227, 93, 55, 221, 130, 235, 222, 152, 26, 90, 183, 241, 45, 35, 123, 247, 62, 45, 30, 2, 114, 114, 66, 19, 192, 153, 231, 157, 63, 156, 240, 18, 7, 38, 134, 71, 36, 1, 115, 174, 84, 170, 200, 236, 108, 209, 160, 24, 50, 239, 19, 99, 29, 64, 165, 86, 86, 147, 145, 254, 179, 5, 137, 211, 236, 92, 184, 230, 171, 48, 183, 218, 85, 251, 221, 169, 61, 152, 132, 117, 110, 74, 43, 194, 1, 106, 65, 127, 130, 170, 105, 214, 197, 165, 137, 235, 195, 67, 90, 20, 101, 227, 156, 86, 203, 201, 41, 102, 142, 130, 54, 72, 128, 143, 61, 248, 61, 131, 121, 105, 197, 82, 50, 118, 152, 115, 104, 121, 140, 8, 40, 196, 109, 217, 31, 231, 150, 148, 246, 30, 117, 23, 49, 16, 64, 15, 26, 112, 40, 133, 105, 142, 140, 250, 8, 44, 200, 76, 46, 25, 184, 133, 91, 218, 173, 245, 0, 240, 167, 224, 7, 89, 181, 226, 210, 106, 196, 213, 231, 47, 206, 248, 146, 244, 214, 74, 99, 46, 45, 181, 241, 148, 9, 99, 180, 0, 160, 253, 167, 76, 7, 32, 135, 1, 35, 104, 127, 62, 96, 117, 193, 127, 247, 228, 156, 183, 181, 143, 106, 201, 133, 215, 134, 254, 127, 173, 196, 11, 132, 64, 18, 128, 11, 64, 18, 80, 243, 31, 254, 63, 93, 128, 16, 22, 26, 239, 37, 173, 0, 119, 234, 238, 126, 179, 29, 212, 64, 2, 165, 50, 103, 147, 242, 93, 10, 233, 242, 218, 83, 54, 60, 109, 122, 255, 157, 216, 38, 117, 98, 202, 55, 191, 240, 153, 146, 159, 78, 236, 158, 28, 29, 195, 61, 132, 153, 13, 31, 120, 110, 174, 136, 70, 73, 173, 198, 27, 109, 0, 70, 254, 103, 83, 224, 48, 97, 219, 50, 48, 237, 0, 113, 163, 25, 184, 74, 200, 115, 52, 207, 100, 153, 50, 106, 142, 107, 148, 62, 50, 91, 17, 181, 8, 182, 9, 9, 52, 35, 182, 77, 161, 44, 234, 215, 125, 16, 188, 198, 255, 36, 104, 57, 47, 128, 115, 4, 104, 102, 155, 99, 163, 171, 128, 122, 104, 149, 64, 251, 211, 53, 160, 27, 210, 179, 122, 179, 236, 120, 100, 47, 242, 202, 220, 165, 150, 165, 176, 126, 117, 7, 80, 134, 83, 111, 57, 215, 128, 117, 104, 103, 34, 74, 38, 82, 25, 144, 135, 39, 197, 10, 9, 128, 98, 15, 104, 1, 248, 59, 196, 102, 179, 14, 190, 60, 147, 19, 124, 90, 240, 227, 155, 149, 184, 212, 230, 66, 41, 142, 121, 226, 135, 131, 82, 131, 230, 175, 66, 235, 87, 70, 73, 4, 176, 0, 224, 6, 52, 102, 232, 251, 195, 239, 103, 231, 31, 8, 227, 244, 215, 248, 114, 217, 239, 224, 184, 104, 170, 44, 147, 180, 26, 34, 15, 124, 193, 130, 218, 105, 127, 118, 250, 5, 0, 61, 180, 63, 193, 31, 197, 235, 106, 21, 177, 55, 197, 218, 75, 56, 95, 115, 226, 92, 54, 154, 117, 153, 46, 206, 72, 173, 86, 145, 173, 103, 108, 7, 9, 87, 87, 239, 121, 228, 190, 103, 105, 129, 167, 145, 156, 208, 4, 160, 18, 147, 123, 166, 38, 199, 165, 50, 55, 163, 29, 104, 78, 8, 30, 6, 163, 173, 153, 128, 8, 130, 25, 170, 67, 163, 105, 131, 4, 192, 37, 41, 168, 85, 96, 164, 157, 67, 237, 72, 176, 42, 53, 204, 167, 211, 203, 103, 220, 12, 45, 154, 60, 93, 32, 81, 171, 231, 82, 3, 19, 13, 25, 185, 198, 71, 32, 115, 128, 130, 23, 158, 219, 210, 21, 96, 104, 52, 90, 114, 202, 246, 51, 101, 248, 240, 28, 252, 111, 52, 102, 221, 222, 108, 99, 23, 38, 13, 251, 164, 85, 18, 192, 63, 96, 156, 36, 146, 202, 166, 0, 64, 31, 215, 160, 128, 66, 230, 120, 184, 223, 133, 194, 74, 144, 230, 234, 4, 211, 113, 220, 62, 108, 226, 252, 234, 62, 192, 239, 75, 29, 102, 125, 105, 194, 151, 50, 128, 238, 213, 86, 73, 117, 36, 37, 85, 130, 31, 218, 191, 54, 9, 119, 96, 154, 126, 191, 175, 99, 255, 236, 248, 219, 240, 28, 95, 158, 251, 190, 184, 164, 7, 22, 239, 107, 105, 101, 238, 160, 200, 158, 107, 112, 15, 109, 47, 191, 246, 252, 179, 3, 16, 224, 15, 125, 4, 144, 129, 209, 250, 128, 190, 181, 192, 212, 133, 179, 247, 65, 175, 43, 238, 109, 185, 84, 149, 122, 163, 46, 125, 3, 253, 32, 138, 150, 95, 42, 205, 109, 178, 187, 120, 218, 200, 9, 79, 0, 153, 190, 190, 71, 106, 245, 154, 204, 205, 206, 73, 50, 97, 166, 176, 58, 33, 76, 141, 22, 229, 132, 80, 194, 134, 13, 159, 13, 0, 13, 159, 0, 152, 47, 170, 194, 85, 109, 27, 26, 231, 143, 104, 6, 164, 72, 24, 4, 51, 1, 199, 6, 164, 237, 157, 118, 5, 235, 50, 149, 232, 54, 14, 123, 216, 147, 169, 7, 75, 155, 194, 218, 12, 76, 168, 197, 217, 193, 231, 139, 103, 63, 84, 194, 153, 118, 181, 26, 221, 128, 149, 210, 244, 251, 101, 247, 190, 57, 104, 57, 52, 98, 71, 74, 216, 144, 81, 243, 48, 18, 247, 139, 54, 15, 75, 32, 224, 151, 145, 80, 111, 144, 72, 74, 60, 145, 146, 153, 201, 57, 228, 112, 36, 128, 67, 7, 122, 68, 8, 102, 175, 42, 186, 61, 182, 160, 191, 175, 38, 191, 39, 173, 90, 92, 234, 165, 4, 52, 127, 66, 42, 0, 248, 212, 158, 152, 212, 166, 123, 37, 86, 94, 169, 154, 191, 58, 6, 2, 160, 233, 175, 224, 167, 230, 103, 175, 127, 92, 134, 78, 247, 228, 249, 127, 130, 229, 50, 76, 249, 93, 44, 247, 127, 6, 62, 124, 5, 231, 166, 224, 103, 128, 213, 69, 223, 31, 129, 203, 48, 142, 107, 64, 223, 159, 249, 32, 9, 247, 48, 19, 69, 149, 2, 46, 38, 159, 114, 204, 101, 249, 125, 201, 152, 140, 143, 140, 66, 55, 196, 202, 185, 222, 254, 27, 180, 208, 211, 72, 78, 120, 2, 8, 18, 137, 135, 113, 83, 235, 195, 7, 135, 37, 151, 203, 218, 84, 8, 218, 37, 111, 182, 78, 152, 33, 98, 181, 225, 27, 80, 56, 82, 176, 109, 66, 133, 160, 164, 145, 64, 204, 24, 176, 25, 10, 160, 168, 233, 175, 26, 196, 106, 17, 164, 81, 163, 176, 176, 89, 183, 229, 244, 63, 133, 251, 163, 153, 207, 165, 77, 162, 96, 191, 156, 229, 23, 226, 178, 211, 53, 161, 117, 225, 142, 75, 149, 62, 200, 96, 245, 230, 211, 101, 108, 100, 90, 154, 117, 104, 50, 28, 135, 30, 35, 131, 57, 100, 4, 186, 48, 38, 61, 160, 59, 128, 196, 68, 50, 43, 233, 124, 159, 76, 128, 0, 90, 32, 67, 67, 0, 188, 181, 110, 67, 19, 213, 243, 64, 8, 105, 246, 43, 248, 161, 245, 75, 1, 192, 159, 148, 50, 52, 124, 105, 52, 35, 35, 59, 113, 108, 197, 181, 210, 154, 30, 144, 26, 8, 128, 126, 127, 125, 198, 76, 248, 105, 113, 10, 48, 8, 163, 103, 189, 39, 151, 255, 127, 158, 156, 242, 108, 91, 247, 50, 74, 179, 22, 201, 142, 111, 227, 194, 240, 102, 217, 78, 63, 190, 139, 33, 244, 107, 210, 10, 170, 210, 242, 234, 210, 132, 5, 208, 132, 255, 79, 194, 36, 121, 179, 227, 207, 92, 6, 94, 103, 148, 69, 90, 58, 157, 146, 164, 159, 208, 137, 97, 7, 247, 238, 135, 5, 21, 60, 118, 193, 115, 46, 127, 90, 13, 1, 82, 78, 120, 2, 216, 188, 101, 235, 62, 0, 122, 100, 236, 224, 62, 201, 168, 11, 48, 223, 40, 169, 37, 201, 240, 132, 129, 1, 141, 3, 255, 66, 57, 82, 154, 81, 190, 76, 71, 32, 72, 185, 170, 255, 204, 162, 29, 72, 0, 136, 48, 206, 210, 10, 88, 39, 186, 50, 159, 96, 202, 154, 224, 38, 165, 112, 6, 47, 231, 11, 144, 0, 56, 116, 185, 118, 203, 118, 137, 7, 9, 41, 148, 203, 154, 102, 142, 3, 117, 227, 78, 241, 48, 221, 161, 42, 151, 65, 56, 101, 56, 240, 2, 233, 95, 177, 74, 10, 211, 115, 82, 100, 39, 168, 18, 128, 221, 80, 143, 159, 251, 38, 209, 0, 252, 240, 247, 249, 176, 78, 179, 234, 73, 29, 26, 189, 54, 155, 148, 10, 64, 94, 26, 73, 75, 225, 112, 70, 38, 119, 37, 196, 175, 174, 7, 240, 115, 218, 7, 80, 159, 78, 72, 125, 14, 4, 160, 224, 135, 214, 92, 227, 201, 171, 254, 197, 147, 109, 175, 178, 7, 178, 204, 242, 224, 127, 70, 250, 57, 118, 7, 126, 190, 175, 176, 21, 212, 36, 76, 32, 4, 117, 125, 55, 161, 186, 0, 124, 73, 41, 72, 64, 39, 111, 41, 89, 139, 246, 252, 215, 27, 13, 73, 39, 1, 124, 28, 62, 103, 3, 214, 65, 180, 187, 119, 238, 196, 54, 241, 187, 175, 252, 236, 39, 79, 218, 207, 128, 63, 145, 156, 240, 4, 240, 79, 159, 248, 210, 76, 44, 136, 239, 29, 29, 30, 134, 82, 8, 161, 21, 125, 88, 125, 30, 110, 48, 59, 203, 224, 39, 211, 27, 140, 248, 198, 90, 170, 88, 51, 167, 158, 175, 249, 54, 100, 192, 211, 159, 111, 200, 196, 84, 103, 179, 110, 1, 125, 156, 249, 23, 169, 214, 165, 214, 102, 162, 86, 3, 225, 58, 204, 104, 44, 181, 46, 53, 205, 13, 25, 208, 196, 228, 126, 89, 156, 126, 54, 127, 102, 110, 191, 17, 18, 14, 3, 193, 29, 90, 144, 115, 89, 46, 215, 36, 230, 167, 100, 211, 153, 23, 3, 164, 168, 155, 67, 114, 28, 38, 68, 93, 36, 12, 211, 195, 15, 183, 65, 143, 210, 206, 110, 179, 231, 210, 191, 106, 45, 140, 131, 152, 140, 29, 26, 182, 229, 176, 63, 213, 248, 204, 199, 49, 182, 123, 250, 225, 239, 87, 3, 105, 208, 236, 159, 77, 72, 25, 32, 47, 90, 240, 207, 237, 79, 203, 204, 222, 156, 196, 203, 171, 212, 236, 175, 77, 129, 0, 0, 254, 86, 153, 83, 130, 61, 89, 113, 86, 76, 222, 252, 101, 79, 54, 93, 222, 121, 149, 150, 79, 168, 253, 31, 190, 34, 148, 90, 17, 215, 150, 29, 127, 212, 254, 9, 128, 62, 89, 67, 192, 61, 78, 192, 2, 32, 17, 112, 20, 0, 224, 231, 60, 13, 92, 33, 141, 19, 252, 116, 21, 217, 89, 25, 231, 204, 73, 180, 141, 100, 202, 151, 210, 92, 89, 38, 70, 71, 216, 38, 110, 181, 187, 121, 90, 201, 9, 79, 0, 23, 174, 207, 134, 65, 50, 117, 255, 204, 228, 148, 212, 202, 37, 248, 197, 4, 8, 68, 219, 190, 162, 198, 130, 82, 163, 6, 156, 72, 32, 0, 185, 52, 101, 76, 57, 202, 124, 108, 126, 59, 6, 45, 229, 50, 221, 210, 226, 192, 204, 55, 96, 190, 91, 154, 34, 116, 63, 184, 116, 157, 133, 46, 159, 66, 13, 196, 205, 73, 50, 4, 121, 29, 218, 191, 94, 55, 125, 1, 125, 171, 79, 1, 134, 161, 157, 145, 167, 147, 137, 154, 8, 172, 3, 219, 243, 88, 152, 166, 253, 18, 216, 158, 253, 7, 45, 108, 159, 235, 27, 148, 32, 147, 151, 195, 123, 236, 123, 44, 116, 95, 220, 39, 162, 218, 217, 199, 158, 254, 184, 246, 244, 215, 11, 190, 84, 97, 214, 27, 240, 103, 12, 248, 15, 102, 101, 102, 95, 90, 138, 99, 189, 18, 21, 97, 254, 79, 129, 36, 20, 252, 70, 243, 111, 186, 60, 46, 111, 254, 138, 47, 171, 206, 179, 39, 125, 12, 228, 129, 207, 69, 114, 224, 86, 0, 218, 105, 127, 190, 45, 41, 9, 141, 15, 2, 8, 83, 21, 99, 5, 208, 2, 240, 65, 6, 62, 72, 27, 135, 74, 55, 160, 198, 153, 162, 156, 146, 137, 245, 4, 180, 63, 173, 40, 222, 155, 158, 124, 70, 166, 70, 199, 164, 82, 42, 85, 243, 189, 253, 119, 218, 221, 60, 173, 228, 132, 39, 0, 74, 54, 223, 247, 104, 165, 84, 149, 226, 204, 140, 36, 51, 104, 184, 212, 248, 218, 242, 173, 166, 71, 25, 6, 125, 134, 31, 193, 204, 208, 163, 143, 141, 134, 164, 192, 100, 81, 2, 198, 0, 215, 70, 97, 65, 96, 59, 110, 8, 161, 203, 137, 218, 144, 103, 253, 74, 2, 208, 248, 21, 90, 7, 123, 227, 221, 118, 20, 100, 171, 233, 57, 63, 44, 200, 253, 152, 227, 209, 76, 123, 80, 60, 58, 108, 138, 99, 130, 141, 130, 114, 117, 44, 43, 181, 38, 26, 44, 95, 116, 2, 83, 150, 47, 254, 163, 113, 97, 231, 250, 187, 137, 44, 186, 107, 173, 151, 150, 64, 40, 126, 34, 144, 60, 220, 128, 209, 225, 49, 169, 195, 125, 16, 73, 34, 31, 22, 16, 180, 190, 154, 253, 10, 126, 152, 253, 133, 4, 192, 159, 146, 242, 56, 192, 14, 240, 23, 45, 248, 25, 166, 246, 115, 226, 79, 159, 180, 138, 25, 5, 63, 71, 6, 216, 219, 127, 238, 219, 125, 121, 229, 199, 61, 248, 254, 246, 98, 28, 3, 225, 247, 21, 31, 251, 102, 75, 42, 179, 56, 113, 246, 250, 7, 212, 248, 0, 124, 18, 90, 63, 5, 119, 41, 89, 134, 246, 175, 74, 19, 4, 192, 97, 192, 86, 140, 95, 76, 106, 72, 181, 81, 195, 18, 215, 27, 87, 58, 153, 130, 223, 159, 74, 2, 248, 89, 89, 177, 162, 79, 227, 7, 246, 238, 230, 117, 220, 125, 234, 57, 231, 62, 102, 246, 244, 244, 146, 147, 130, 0, 18, 169, 220, 189, 208, 132, 225, 196, 240, 168, 100, 114, 41, 169, 227, 198, 19, 124, 243, 67, 63, 142, 0, 172, 217, 142, 117, 2, 73, 129, 137, 116, 5, 165, 93, 154, 40, 85, 62, 226, 102, 97, 210, 184, 142, 133, 235, 88, 210, 184, 5, 32, 245, 188, 41, 226, 64, 62, 175, 245, 73, 0, 20, 166, 153, 190, 6, 205, 52, 229, 176, 202, 30, 106, 206, 32, 228, 172, 64, 250, 252, 218, 39, 128, 99, 227, 82, 73, 1, 155, 179, 199, 158, 47, 184, 112, 68, 162, 117, 131, 12, 76, 156, 101, 91, 48, 255, 91, 50, 176, 122, 181, 76, 207, 20, 101, 236, 224, 56, 142, 61, 141, 50, 158, 237, 233, 135, 175, 107, 59, 251, 42, 211, 240, 247, 199, 0, 124, 106, 254, 67, 6, 252, 133, 67, 89, 41, 14, 35, 126, 56, 33, 82, 235, 129, 214, 79, 40, 89, 120, 65, 92, 158, 251, 62, 95, 94, 252, 183, 208, 150, 199, 16, 252, 148, 145, 187, 34, 217, 113, 37, 46, 10, 123, 247, 3, 154, 254, 32, 240, 20, 180, 61, 52, 191, 33, 128, 10, 192, 95, 133, 107, 80, 3, 65, 128, 4, 224, 56, 213, 26, 21, 105, 180, 26, 240, 245, 125, 89, 185, 178, 79, 250, 7, 250, 244, 205, 63, 131, 67, 189, 146, 207, 195, 237, 41, 20, 229, 177, 7, 238, 101, 71, 242, 77, 55, 127, 243, 155, 39, 253, 55, 0, 142, 36, 39, 5, 1, 12, 174, 89, 177, 43, 140, 53, 39, 14, 30, 62, 40, 249, 76, 70, 193, 78, 49, 96, 52, 224, 4, 102, 84, 92, 135, 159, 130, 148, 255, 32, 198, 31, 55, 192, 214, 194, 154, 200, 237, 56, 142, 140, 40, 86, 53, 217, 150, 103, 157, 140, 115, 205, 36, 25, 159, 158, 233, 90, 135, 77, 239, 12, 157, 194, 18, 154, 132, 127, 188, 1, 116, 7, 184, 153, 154, 246, 88, 182, 96, 178, 243, 69, 28, 166, 136, 169, 83, 205, 127, 148, 103, 190, 177, 4, 108, 26, 150, 230, 233, 195, 134, 244, 174, 222, 0, 75, 32, 45, 251, 30, 219, 139, 50, 56, 39, 246, 35, 212, 98, 0, 191, 175, 195, 124, 229, 73, 128, 127, 20, 154, 31, 96, 39, 232, 53, 28, 206, 105, 31, 0, 221, 129, 10, 204, 254, 88, 35, 171, 154, 63, 145, 141, 203, 243, 0, 254, 75, 223, 211, 241, 241, 141, 99, 36, 51, 123, 35, 185, 254, 127, 131, 208, 65, 136, 250, 176, 15, 9, 64, 205, 126, 163, 253, 91, 36, 1, 250, 255, 252, 74, 146, 95, 147, 58, 192, 223, 108, 129, 28, 96, 58, 13, 13, 245, 203, 5, 231, 158, 42, 231, 159, 179, 85, 206, 57, 115, 139, 156, 121, 250, 38, 89, 57, 216, 43, 89, 104, 255, 145, 189, 187, 100, 252, 224, 176, 36, 147, 233, 111, 217, 93, 61, 237, 36, 134, 6, 116, 108, 239, 238, 81, 144, 143, 92, 121, 165, 255, 91, 175, 125, 245, 13, 91, 182, 159, 246, 140, 119, 253, 207, 255, 37, 247, 221, 123, 63, 52, 181, 71, 100, 107, 62, 191, 3, 167, 75, 29, 59, 231, 227, 181, 140, 51, 133, 243, 237, 227, 250, 70, 88, 71, 12, 58, 28, 136, 56, 87, 57, 105, 71, 59, 239, 176, 228, 3, 37, 58, 27, 79, 253, 2, 62, 72, 98, 58, 25, 185, 26, 0, 193, 62, 223, 225, 197, 42, 60, 219, 17, 137, 12, 23, 116, 78, 63, 54, 230, 235, 181, 117, 123, 238, 135, 117, 218, 165, 38, 145, 17, 80, 175, 142, 89, 163, 225, 178, 247, 26, 6, 184, 240, 83, 100, 60, 70, 62, 222, 204, 144, 64, 96, 121, 15, 251, 100, 186, 135, 253, 113, 178, 11, 73, 198, 243, 2, 217, 121, 199, 205, 0, 204, 78, 121, 221, 219, 95, 39, 233, 84, 14, 224, 111, 26, 179, 31, 154, 191, 60, 145, 144, 210, 184, 33, 129, 18, 44, 128, 210, 24, 226, 156, 233, 55, 197, 57, 0, 158, 20, 167, 112, 142, 81, 86, 122, 55, 120, 114, 233, 111, 196, 229, 188, 119, 196, 36, 145, 227, 81, 30, 59, 105, 86, 35, 185, 250, 15, 67, 185, 253, 35, 102, 184, 79, 59, 252, 178, 0, 124, 190, 32, 245, 252, 180, 212, 123, 103, 164, 209, 59, 37, 141, 204, 156, 212, 147, 115, 32, 5, 190, 88, 213, 147, 92, 111, 78, 82, 32, 49, 15, 22, 192, 244, 232, 176, 28, 218, 127, 64, 70, 198, 70, 165, 52, 59, 39, 133, 217, 130, 212, 202, 85, 41, 21, 11, 82, 42, 20, 14, 110, 62, 239, 188, 115, 119, 220, 121, 231, 50, 124, 202, 228, 248, 147, 147, 130, 0, 40, 169, 124, 246, 83, 153, 116, 250, 23, 126, 255, 175, 254, 90, 246, 237, 63, 40, 179, 133, 42, 128, 65, 176, 226, 15, 104, 54, 64, 244, 1, 86, 0, 8, 192, 34, 136, 76, 26, 65, 108, 128, 106, 128, 111, 202, 146, 0, 218, 175, 224, 226, 95, 28, 219, 48, 79, 235, 228, 55, 1, 188, 54, 232, 9, 238, 4, 234, 214, 142, 126, 0, 210, 39, 49, 232, 40, 132, 37, 0, 144, 135, 15, 224, 106, 157, 0, 52, 235, 176, 213, 170, 144, 4, 180, 83, 16, 213, 49, 209, 140, 82, 96, 127, 17, 180, 24, 214, 125, 144, 86, 0, 115, 60, 64, 93, 36, 1, 44, 244, 129, 33, 30, 27, 137, 128, 236, 193, 109, 120, 190, 165, 169, 105, 185, 247, 91, 223, 144, 115, 47, 56, 75, 206, 189, 232, 121, 208, 252, 53, 213, 236, 229, 241, 172, 2, 190, 56, 98, 8, 128, 125, 0, 165, 137, 20, 136, 193, 78, 2, 154, 129, 187, 81, 243, 101, 213, 217, 9, 121, 246, 123, 61, 57, 243, 141, 172, 247, 216, 203, 163, 95, 139, 228, 191, 222, 0, 223, 72, 123, 252, 225, 247, 103, 0, 254, 108, 73, 26, 61, 51, 82, 239, 153, 150, 102, 207, 172, 18, 65, 53, 83, 144, 100, 95, 75, 50, 125, 129, 76, 142, 29, 144, 93, 15, 222, 15, 208, 239, 150, 169, 241, 49, 5, 61, 28, 7, 91, 227, 66, 73, 246, 100, 63, 82, 153, 41, 252, 166, 93, 125, 218, 9, 155, 236, 73, 33, 240, 227, 238, 46, 226, 70, 207, 78, 76, 72, 58, 203, 215, 129, 193, 86, 230, 61, 87, 179, 217, 152, 204, 10, 21, 213, 180, 28, 186, 163, 11, 96, 214, 53, 110, 253, 107, 231, 54, 112, 59, 46, 218, 40, 165, 0, 112, 76, 210, 73, 60, 244, 195, 213, 220, 71, 121, 253, 25, 95, 159, 5, 116, 123, 68, 204, 126, 169, 205, 89, 167, 205, 70, 132, 233, 166, 140, 17, 30, 135, 233, 151, 176, 9, 216, 41, 103, 170, 241, 205, 160, 4, 181, 30, 2, 205, 95, 238, 23, 81, 110, 202, 178, 12, 52, 139, 53, 145, 241, 6, 95, 233, 61, 36, 43, 215, 159, 37, 143, 220, 113, 64, 38, 15, 212, 160, 245, 123, 0, 248, 28, 76, 253, 180, 241, 247, 15, 50, 14, 159, 31, 22, 64, 133, 115, 253, 103, 18, 176, 16, 60, 104, 90, 95, 134, 78, 11, 228, 69, 127, 117, 252, 128, 191, 56, 18, 201, 119, 126, 3, 215, 213, 62, 232, 19, 5, 240, 251, 17, 180, 179, 47, 81, 81, 23, 32, 164, 11, 224, 87, 36, 55, 16, 72, 42, 23, 200, 245, 87, 126, 67, 174, 248, 196, 71, 228, 150, 107, 174, 146, 17, 40, 2, 190, 120, 101, 112, 227, 70, 57, 229, 244, 51, 100, 205, 246, 109, 146, 238, 239, 49, 247, 20, 247, 50, 145, 74, 127, 109, 195, 230, 83, 223, 111, 246, 246, 244, 148, 147, 135, 0, 82, 217, 251, 91, 205, 48, 28, 61, 112, 64, 250, 250, 251, 213, 47, 110, 182, 154, 186, 212, 78, 59, 11, 60, 245, 155, 209, 2, 34, 104, 104, 2, 168, 161, 192, 50, 117, 152, 78, 65, 246, 170, 195, 223, 196, 54, 168, 78, 203, 83, 116, 65, 172, 113, 201, 151, 141, 96, 169, 245, 218, 60, 142, 201, 171, 160, 78, 37, 7, 162, 90, 115, 145, 160, 75, 43, 136, 242, 88, 76, 15, 190, 77, 67, 131, 100, 41, 186, 253, 202, 41, 76, 210, 70, 26, 160, 12, 236, 21, 61, 70, 6, 36, 98, 27, 205, 131, 152, 206, 64, 4, 62, 86, 92, 143, 193, 172, 141, 75, 29, 97, 197, 250, 11, 165, 82, 236, 149, 187, 175, 221, 5, 240, 211, 207, 207, 3, 252, 240, 251, 17, 232, 255, 171, 5, 48, 9, 183, 96, 54, 33, 141, 146, 175, 143, 255, 174, 191, 44, 33, 111, 252, 130, 47, 91, 94, 98, 43, 63, 198, 82, 47, 69, 242, 205, 119, 135, 82, 28, 199, 5, 225, 163, 189, 176, 0, 66, 206, 248, 75, 212, 164, 149, 4, 240, 211, 36, 0, 88, 3, 201, 138, 100, 7, 61, 233, 233, 15, 228, 134, 111, 125, 77, 238, 189, 233, 6, 16, 65, 70, 182, 92, 116, 137, 156, 126, 249, 139, 228, 244, 231, 191, 80, 206, 122, 222, 229, 114, 193, 139, 95, 38, 3, 107, 215, 73, 171, 214, 196, 245, 243, 162, 84, 54, 247, 127, 158, 253, 210, 215, 190, 245, 209, 187, 239, 94, 134, 79, 152, 28, 191, 114, 210, 16, 64, 223, 138, 213, 15, 227, 198, 78, 28, 216, 189, 87, 167, 4, 211, 68, 39, 152, 85, 75, 91, 160, 105, 103, 26, 129, 7, 96, 187, 225, 63, 106, 115, 246, 20, 235, 116, 81, 10, 1, 102, 116, 174, 10, 73, 130, 194, 178, 180, 42, 104, 37, 240, 25, 114, 253, 41, 40, 93, 220, 128, 216, 148, 117, 233, 110, 93, 255, 107, 26, 73, 136, 226, 92, 139, 78, 97, 185, 206, 160, 217, 220, 150, 154, 159, 224, 103, 20, 235, 252, 22, 64, 173, 80, 210, 192, 105, 206, 205, 6, 130, 125, 99, 47, 181, 121, 188, 213, 39, 43, 87, 93, 44, 123, 239, 45, 200, 190, 123, 139, 82, 56, 148, 147, 162, 106, 125, 152, 253, 112, 3, 170, 240, 249, 107, 115, 156, 12, 228, 75, 163, 18, 151, 179, 223, 18, 200, 47, 92, 227, 29, 147, 121, 253, 71, 18, 146, 218, 181, 239, 231, 71, 70, 73, 176, 8, 252, 244, 90, 2, 65, 59, 254, 202, 112, 3, 74, 18, 129, 0, 20, 252, 3, 9, 89, 179, 174, 95, 174, 251, 214, 87, 228, 161, 31, 222, 42, 155, 206, 62, 75, 206, 121, 201, 171, 100, 229, 246, 237, 146, 232, 201, 233, 43, 191, 250, 251, 122, 228, 177, 219, 110, 149, 135, 110, 184, 14, 86, 82, 125, 42, 211, 223, 247, 206, 242, 220, 236, 239, 93, 253, 213, 207, 61, 237, 102, 254, 45, 150, 147, 134, 0, 222, 254, 7, 191, 61, 14, 100, 236, 222, 191, 119, 143, 250, 201, 169, 84, 18, 96, 33, 122, 230, 197, 0, 147, 224, 39, 56, 225, 83, 114, 118, 32, 77, 116, 96, 159, 166, 180, 25, 82, 67, 26, 65, 102, 77, 111, 29, 106, 211, 64, 178, 48, 0, 100, 26, 231, 250, 187, 184, 174, 178, 103, 129, 32, 133, 117, 192, 71, 135, 77, 89, 238, 207, 150, 209, 56, 183, 97, 48, 117, 154, 188, 249, 0, 55, 94, 89, 68, 235, 99, 156, 105, 236, 24, 80, 75, 192, 87, 50, 226, 156, 129, 58, 142, 109, 98, 114, 82, 246, 61, 242, 48, 210, 3, 51, 193, 7, 90, 156, 161, 54, 231, 73, 9, 62, 127, 46, 117, 134, 4, 209, 25, 242, 208, 141, 51, 50, 181, 7, 154, 116, 36, 171, 126, 191, 250, 252, 5, 51, 181, 151, 15, 2, 61, 251, 189, 190, 188, 226, 163, 32, 13, 237, 75, 56, 62, 228, 135, 31, 139, 228, 254, 207, 193, 10, 107, 226, 26, 105, 175, 63, 39, 252, 152, 94, 255, 48, 131, 37, 181, 63, 192, 159, 27, 240, 100, 205, 134, 126, 185, 246, 187, 95, 147, 7, 239, 188, 89, 182, 94, 124, 190, 156, 254, 236, 231, 74, 24, 199, 245, 168, 53, 116, 200, 111, 32, 159, 149, 187, 190, 251, 45, 217, 121, 199, 173, 240, 6, 188, 135, 250, 215, 174, 125, 101, 97, 108, 252, 211, 118, 87, 79, 123, 57, 105, 8, 224, 207, 126, 254, 109, 77, 47, 72, 60, 56, 57, 62, 6, 243, 177, 32, 185, 44, 191, 19, 72, 20, 17, 76, 102, 105, 124, 111, 51, 173, 182, 197, 73, 50, 108, 243, 236, 112, 195, 207, 204, 27, 48, 96, 54, 190, 187, 1, 45, 133, 11, 210, 129, 121, 168, 136, 155, 48, 93, 233, 65, 243, 180, 156, 93, 234, 211, 122, 168, 68, 31, 67, 213, 173, 153, 110, 194, 252, 58, 53, 58, 202, 114, 31, 88, 215, 155, 96, 170, 208, 114, 26, 199, 113, 176, 60, 59, 30, 105, 243, 235, 108, 3, 30, 27, 242, 72, 88, 65, 2, 166, 252, 116, 65, 154, 21, 28, 43, 252, 119, 206, 216, 107, 242, 245, 93, 0, 120, 117, 44, 41, 21, 128, 189, 39, 115, 182, 180, 10, 171, 101, 207, 61, 85, 153, 129, 59, 92, 153, 14, 164, 198, 55, 249, 114, 152, 47, 31, 151, 159, 249, 176, 47, 207, 252, 189, 184, 4, 105, 119, 100, 199, 94, 30, 251, 70, 36, 183, 253, 115, 75, 42, 51, 56, 89, 206, 246, 75, 214, 37, 74, 213, 84, 227, 135, 25, 4, 118, 0, 166, 74, 146, 24, 104, 201, 186, 173, 125, 114, 219, 141, 87, 202, 237, 55, 124, 79, 214, 159, 117, 166, 108, 191, 244, 18, 105, 54, 171, 112, 135, 106, 146, 239, 201, 74, 42, 145, 144, 27, 191, 114, 133, 28, 122, 108, 167, 164, 178, 201, 171, 86, 173, 59, 229, 101, 99, 187, 247, 222, 214, 190, 17, 93, 57, 121, 8, 128, 226, 7, 193, 221, 133, 153, 57, 153, 28, 29, 145, 129, 254, 94, 164, 16, 104, 4, 21, 0, 78, 213, 9, 49, 29, 114, 4, 31, 50, 136, 85, 93, 55, 233, 58, 115, 207, 130, 159, 229, 153, 231, 128, 222, 41, 132, 173, 66, 215, 1, 25, 105, 220, 54, 198, 84, 162, 19, 36, 64, 200, 234, 79, 55, 69, 14, 53, 63, 163, 16, 99, 206, 155, 109, 89, 192, 22, 209, 178, 60, 172, 182, 21, 128, 40, 123, 252, 57, 100, 201, 45, 20, 252, 156, 213, 7, 92, 164, 82, 253, 50, 55, 50, 35, 19, 251, 38, 160, 205, 19, 82, 135, 63, 175, 243, 247, 199, 51, 250, 252, 126, 121, 152, 111, 239, 233, 145, 108, 176, 21, 4, 49, 40, 211, 195, 145, 20, 38, 66, 169, 206, 137, 228, 214, 199, 228, 101, 31, 244, 229, 220, 95, 56, 246, 195, 124, 157, 178, 239, 186, 72, 174, 255, 139, 150, 76, 239, 37, 195, 217, 94, 127, 128, 63, 204, 84, 165, 149, 45, 75, 152, 43, 74, 11, 230, 127, 188, 183, 33, 235, 182, 245, 203, 67, 15, 223, 42, 215, 127, 255, 107, 178, 230, 244, 237, 178, 249, 226, 11, 37, 14, 203, 175, 84, 173, 72, 34, 157, 144, 94, 132, 91, 191, 242, 101, 25, 63, 120, 72, 146, 185, 204, 39, 182, 63, 243, 217, 111, 56, 240, 200, 174, 67, 118, 87, 93, 177, 114, 82, 17, 64, 38, 155, 125, 8, 102, 124, 107, 108, 100, 76, 134, 6, 123, 164, 7, 90, 128, 195, 111, 20, 3, 116, 23, 8, 110, 2, 149, 166, 56, 80, 165, 104, 51, 253, 5, 58, 11, 175, 101, 8, 128, 19, 115, 76, 121, 130, 143, 144, 199, 79, 135, 232, 76, 125, 243, 245, 146, 52, 80, 150, 229, 153, 6, 141, 173, 157, 141, 220, 134, 75, 106, 110, 160, 215, 117, 226, 241, 159, 214, 129, 152, 33, 164, 121, 154, 81, 94, 66, 192, 38, 26, 103, 30, 51, 96, 245, 35, 129, 86, 0, 66, 61, 38, 241, 102, 18, 38, 124, 66, 14, 63, 50, 44, 181, 41, 248, 245, 52, 239, 199, 50, 82, 97, 239, 254, 112, 70, 95, 228, 81, 29, 79, 74, 163, 144, 18, 63, 28, 16, 95, 122, 37, 106, 166, 100, 229, 57, 190, 188, 240, 127, 251, 114, 218, 107, 99, 48, 137, 143, 31, 240, 31, 188, 53, 146, 171, 223, 223, 146, 225, 187, 193, 110, 244, 249, 169, 249, 211, 102, 204, 63, 202, 17, 252, 37, 105, 33, 72, 190, 34, 235, 183, 247, 203, 193, 195, 143, 202, 55, 191, 252, 31, 210, 183, 233, 20, 89, 125, 238, 121, 146, 206, 165, 165, 82, 70, 57, 92, 236, 213, 131, 3, 114, 223, 181, 215, 200, 52, 20, 65, 34, 157, 250, 219, 43, 246, 22, 223, 125, 231, 55, 191, 95, 212, 214, 126, 252, 156, 242, 113, 33, 39, 21, 1, 244, 245, 15, 240, 121, 238, 233, 131, 251, 14, 232, 215, 94, 87, 12, 245, 74, 38, 147, 82, 16, 82, 156, 89, 207, 53, 213, 166, 10, 46, 254, 99, 171, 192, 165, 80, 63, 59, 14, 18, 160, 118, 70, 18, 74, 42, 17, 52, 13, 120, 153, 102, 171, 82, 97, 103, 33, 127, 243, 245, 179, 78, 157, 101, 160, 0, 54, 163, 9, 116, 45, 56, 26, 193, 62, 7, 214, 104, 90, 160, 219, 134, 194, 24, 83, 201, 85, 92, 154, 119, 216, 115, 244, 130, 150, 5, 103, 8, 162, 222, 6, 114, 120, 12, 180, 0, 170, 40, 8, 48, 231, 179, 27, 101, 98, 199, 28, 192, 238, 75, 157, 154, 127, 20, 97, 132, 111, 240, 65, 208, 183, 247, 242, 117, 222, 124, 121, 167, 143, 10, 2, 217, 242, 194, 64, 94, 246, 143, 129, 156, 254, 250, 56, 180, 165, 57, 142, 227, 65, 198, 30, 136, 228, 154, 247, 133, 114, 232, 118, 156, 40, 125, 126, 104, 126, 73, 213, 69, 8, 254, 124, 89, 90, 249, 162, 132, 249, 2, 8, 96, 86, 86, 156, 154, 145, 66, 125, 88, 190, 250, 197, 255, 39, 153, 149, 253, 178, 246, 156, 211, 101, 96, 32, 167, 55, 166, 80, 169, 200, 202, 161, 126, 217, 117, 215, 157, 178, 119, 199, 78, 9, 50, 217, 111, 61, 251, 55, 222, 247, 190, 151, 4, 184, 196, 102, 46, 88, 87, 22, 201, 73, 69, 0, 207, 124, 217, 107, 198, 129, 160, 221, 251, 247, 238, 86, 224, 166, 83, 41, 89, 187, 102, 165, 244, 195, 29, 200, 102, 51, 237, 25, 127, 74, 4, 40, 79, 160, 18, 216, 86, 183, 35, 5, 158, 126, 220, 180, 20, 67, 16, 22, 136, 10, 96, 32, 17, 105, 10, 92, 22, 181, 224, 118, 235, 92, 16, 226, 218, 17, 168, 27, 3, 204, 156, 210, 203, 56, 254, 140, 75, 97, 246, 196, 191, 249, 128, 20, 28, 143, 106, 125, 108, 107, 246, 207, 161, 63, 16, 17, 129, 175, 4, 16, 147, 6, 159, 230, 3, 9, 52, 26, 158, 52, 0, 104, 190, 171, 191, 39, 187, 21, 90, 63, 144, 194, 238, 150, 212, 71, 179, 82, 5, 248, 43, 10, 254, 140, 52, 102, 18, 218, 39, 64, 240, 243, 125, 127, 155, 94, 224, 201, 139, 255, 198, 147, 13, 207, 61, 126, 128, 79, 225, 67, 62, 223, 253, 237, 80, 246, 221, 8, 205, 79, 240, 243, 177, 94, 106, 126, 152, 253, 81, 142, 4, 0, 224, 231, 231, 164, 129, 48, 184, 41, 45, 73, 184, 2, 95, 253, 175, 127, 69, 126, 76, 54, 92, 120, 174, 164, 179, 73, 137, 227, 62, 20, 75, 101, 25, 232, 205, 203, 200, 142, 29, 209, 253, 55, 223, 38, 137, 100, 234, 254, 92, 79, 223, 187, 191, 251, 158, 247, 181, 132, 175, 137, 96, 75, 63, 190, 78, 253, 184, 144, 147, 138, 0, 254, 253, 31, 254, 119, 61, 59, 56, 240, 240, 248, 200, 176, 148, 138, 69, 104, 212, 56, 95, 247, 12, 75, 160, 15, 46, 65, 175, 244, 247, 229, 245, 61, 240, 156, 157, 71, 224, 58, 243, 219, 4, 34, 16, 13, 144, 75, 136, 174, 82, 141, 171, 153, 96, 242, 25, 51, 58, 159, 9, 252, 179, 63, 230, 33, 24, 51, 223, 109, 203, 141, 181, 144, 10, 243, 59, 73, 192, 204, 3, 96, 204, 228, 177, 56, 183, 35, 65, 49, 240, 171, 192, 204, 212, 239, 223, 35, 207, 188, 200, 195, 147, 168, 10, 2, 0, 176, 235, 51, 190, 164, 100, 189, 248, 181, 213, 50, 246, 72, 209, 188, 187, 15, 86, 0, 95, 219, 77, 205, 223, 44, 192, 69, 224, 135, 59, 91, 158, 108, 127, 133, 39, 175, 248, 136, 7, 243, 127, 222, 90, 57, 30, 100, 246, 64, 36, 95, 121, 107, 75, 246, 93, 15, 240, 243, 9, 63, 245, 249, 97, 250, 103, 64, 0, 208, 252, 81, 15, 204, 126, 90, 0, 185, 130, 228, 215, 138, 164, 7, 107, 242, 197, 207, 255, 95, 153, 171, 78, 203, 70, 128, 223, 75, 122, 210, 59, 208, 39, 143, 221, 125, 39, 92, 28, 95, 198, 247, 236, 147, 187, 175, 253, 65, 204, 243, 252, 29, 61, 131, 43, 222, 50, 122, 229, 129, 195, 240, 124, 64, 44, 8, 93, 240, 31, 81, 78, 42, 2, 160, 36, 130, 228, 61, 115, 211, 51, 50, 50, 60, 42, 158, 111, 39, 236, 180, 26, 250, 17, 200, 181, 107, 134, 100, 5, 136, 96, 37, 8, 161, 39, 159, 131, 133, 16, 72, 50, 225, 9, 95, 36, 66, 244, 41, 56, 81, 150, 125, 2, 10, 20, 139, 21, 5, 42, 16, 170, 0, 199, 186, 133, 182, 254, 51, 120, 50, 96, 229, 48, 158, 153, 249, 199, 116, 107, 93, 48, 77, 203, 64, 52, 157, 196, 195, 190, 2, 130, 30, 251, 177, 117, 58, 66, 96, 26, 96, 106, 203, 218, 96, 31, 231, 109, 193, 244, 231, 27, 121, 249, 78, 254, 218, 36, 192, 93, 236, 151, 188, 119, 170, 148, 15, 123, 82, 66, 104, 78, 103, 236, 23, 123, 168, 249, 249, 54, 164, 184, 92, 252, 171, 190, 188, 241, 191, 0, 148, 13, 6, 1, 58, 255, 224, 56, 144, 93, 223, 139, 228, 83, 207, 105, 201, 129, 91, 112, 53, 213, 236, 135, 253, 68, 179, 31, 218, 95, 224, 243, 71, 121, 130, 191, 0, 223, 191, 32, 201, 161, 134, 12, 110, 72, 200, 213, 223, 249, 47, 25, 25, 223, 39, 155, 46, 187, 72, 252, 76, 32, 121, 88, 118, 7, 119, 60, 42, 125, 43, 135, 100, 106, 248, 128, 220, 117, 245, 213, 28, 190, 125, 184, 119, 104, 229, 235, 15, 125, 98, 247, 67, 2, 210, 16, 126, 61, 184, 11, 254, 39, 148, 147, 142, 0, 112, 74, 15, 133, 245, 102, 52, 114, 224, 128, 228, 122, 114, 210, 211, 147, 145, 148, 23, 151, 217, 137, 113, 217, 243, 232, 99, 178, 247, 145, 135, 100, 231, 189, 119, 201, 206, 187, 111, 151, 201, 195, 251, 100, 229, 96, 14, 196, 48, 8, 96, 208, 220, 39, 88, 169, 110, 1, 80, 130, 147, 0, 230, 42, 192, 169, 157, 122, 88, 81, 179, 156, 229, 176, 39, 237, 23, 192, 146, 64, 83, 187, 64, 1, 203, 122, 144, 171, 121, 208, 216, 236, 19, 96, 186, 57, 54, 179, 173, 174, 163, 34, 252, 55, 228, 194, 92, 211, 33, 105, 246, 203, 125, 113, 191, 168, 3, 22, 64, 139, 238, 0, 9, 64, 77, 255, 64, 65, 222, 224, 151, 122, 166, 178, 146, 140, 214, 75, 170, 185, 66, 74, 195, 112, 21, 248, 189, 62, 126, 192, 147, 95, 235, 169, 251, 242, 162, 191, 12, 228, 37, 127, 107, 94, 69, 126, 60, 201, 15, 63, 30, 202, 215, 127, 169, 37, 133, 17, 92, 3, 157, 227, 207, 225, 62, 227, 247, 135, 240, 251, 217, 225, 167, 62, 127, 118, 86, 226, 125, 101, 89, 183, 45, 47, 247, 222, 245, 3, 121, 240, 129, 31, 202, 150, 103, 95, 44, 62, 200, 59, 153, 73, 75, 179, 92, 144, 70, 105, 78, 103, 124, 222, 117, 237, 119, 5, 222, 211, 61, 253, 67, 107, 95, 115, 240, 47, 118, 61, 24, 59, 29, 59, 202, 35, 208, 163, 234, 18, 192, 19, 202, 73, 243, 48, 144, 147, 83, 207, 62, 119, 35, 64, 254, 195, 179, 47, 185, 116, 96, 221, 166, 13, 178, 243, 225, 71, 133, 239, 9, 40, 205, 205, 232, 183, 223, 40, 62, 172, 1, 142, 173, 53, 177, 62, 184, 98, 165, 92, 254, 234, 87, 203, 233, 231, 95, 32, 227, 99, 147, 82, 169, 52, 236, 3, 55, 166, 63, 128, 166, 184, 14, 195, 169, 89, 110, 158, 28, 212, 52, 52, 44, 62, 4, 196, 97, 186, 56, 90, 25, 151, 74, 2, 76, 231, 131, 66, 49, 148, 229, 149, 69, 156, 214, 60, 93, 123, 190, 181, 88, 203, 199, 221, 60, 1, 214, 205, 22, 202, 52, 173, 192, 172, 131, 15, 20, 180, 188, 53, 8, 49, 53, 253, 97, 199, 194, 172, 143, 230, 82, 210, 156, 66, 227, 135, 185, 223, 156, 204, 66, 251, 131, 84, 38, 35, 41, 79, 211, 23, 238, 17, 105, 36, 64, 26, 113, 121, 213, 199, 205, 199, 58, 142, 167, 9, 62, 149, 169, 72, 110, 251, 96, 36, 247, 126, 42, 180, 224, 71, 208, 39, 252, 248, 144, 143, 241, 249, 9, 252, 176, 119, 14, 193, 248, 254, 107, 183, 231, 229, 192, 240, 3, 242, 149, 255, 252, 148, 108, 56, 247, 28, 201, 173, 95, 37, 53, 0, 190, 31, 150, 220, 195, 215, 127, 95, 210, 61, 189, 178, 227, 142, 59, 65, 154, 173, 187, 123, 7, 86, 191, 113, 215, 239, 238, 218, 19, 123, 41, 118, 182, 14, 33, 133, 128, 203, 202, 235, 218, 149, 35, 203, 73, 71, 0, 111, 254, 141, 247, 36, 190, 242, 127, 255, 229, 135, 173, 70, 227, 44, 155, 212, 150, 252, 224, 10, 57, 229, 236, 115, 176, 28, 146, 152, 231, 75, 185, 48, 37, 163, 143, 128, 32, 246, 238, 145, 139, 159, 255, 28, 121, 201, 155, 254, 155, 76, 79, 76, 74, 25, 36, 16, 15, 82, 10, 82, 223, 227, 147, 118, 243, 79, 11, 186, 167, 7, 249, 20, 30, 49, 75, 34, 208, 124, 182, 52, 190, 135, 30, 104, 87, 146, 64, 89, 246, 234, 235, 19, 132, 92, 98, 127, 137, 192, 71, 121, 230, 241, 227, 155, 56, 32, 228, 41, 177, 96, 63, 174, 126, 138, 167, 31, 17, 193, 54, 116, 69, 26, 129, 68, 53, 4, 126, 136, 19, 4, 16, 18, 252, 19, 0, 255, 104, 70, 90, 83, 240, 249, 105, 9, 76, 162, 92, 53, 41, 245, 178, 15, 64, 36, 229, 165, 255, 232, 201, 25, 111, 136, 73, 144, 57, 126, 110, 45, 253, 253, 91, 254, 54, 146, 251, 62, 219, 194, 113, 194, 188, 177, 51, 252, 162, 36, 8, 128, 224, 215, 30, 255, 146, 52, 1, 250, 168, 183, 8, 191, 127, 70, 250, 55, 194, 154, 73, 150, 228, 115, 255, 247, 67, 146, 223, 184, 74, 86, 159, 186, 85, 26, 97, 77, 6, 6, 7, 101, 223, 131, 247, 203, 190, 71, 30, 148, 242, 204, 140, 212, 154, 225, 29, 43, 87, 111, 120, 243, 163, 191, 116, 223, 190, 216, 43, 176, 179, 141, 8, 25, 132, 46, 248, 127, 164, 156, 116, 4, 64, 233, 91, 185, 234, 87, 74, 51, 211, 127, 215, 106, 54, 219, 239, 9, 223, 116, 214, 217, 178, 249, 194, 75, 245, 115, 80, 52, 245, 137, 64, 224, 84, 146, 241, 132, 12, 63, 246, 176, 236, 184, 243, 54, 217, 180, 109, 187, 188, 245, 215, 127, 77, 202, 181, 134, 76, 76, 76, 139, 7, 4, 83, 251, 155, 23, 140, 26, 128, 26, 18, 240, 117, 126, 1, 241, 74, 112, 251, 40, 132, 84, 196, 209, 168, 145, 102, 172, 4, 108, 139, 56, 131, 150, 241, 131, 249, 116, 45, 79, 95, 159, 121, 36, 8, 90, 4, 134, 100, 168, 253, 105, 27, 208, 170, 136, 193, 247, 151, 186, 7, 179, 30, 192, 167, 121, 63, 147, 150, 104, 18, 224, 31, 203, 74, 99, 44, 13, 2, 64, 224, 247, 251, 102, 80, 14, 68, 225, 165, 60, 121, 233, 63, 4, 114, 230, 155, 1, 254, 227, 104, 118, 223, 236, 254, 72, 110, 252, 203, 80, 238, 249, 52, 92, 43, 190, 172, 211, 249, 252, 52, 251, 225, 243, 115, 134, 159, 233, 244, 3, 240, 57, 228, 151, 43, 74, 118, 85, 40, 3, 27, 147, 114, 197, 23, 62, 41, 195, 227, 7, 100, 235, 51, 47, 197, 125, 107, 74, 190, 55, 47, 112, 130, 228, 187, 255, 254, 233, 168, 85, 111, 196, 64, 228, 183, 245, 111, 216, 254, 150, 71, 223, 124, 251, 222, 216, 43, 177, 51, 126, 218, 131, 119, 157, 134, 213, 241, 115, 9, 142, 91, 33, 71, 158, 116, 50, 51, 54, 250, 241, 108, 95, 223, 203, 189, 192, 127, 200, 53, 2, 154, 138, 27, 54, 157, 162, 195, 130, 124, 129, 135, 118, 214, 193, 161, 231, 196, 159, 181, 32, 135, 179, 47, 127, 145, 236, 219, 185, 83, 62, 251, 207, 255, 44, 125, 185, 164, 172, 89, 61, 32, 97, 3, 141, 19, 254, 57, 249, 66, 3, 125, 116, 13, 182, 67, 144, 96, 69, 48, 113, 250, 240, 186, 43, 172, 179, 135, 192, 0, 217, 192, 28, 196, 1, 160, 179, 166, 5, 219, 106, 25, 179, 29, 131, 14, 53, 134, 184, 37, 124, 143, 95, 3, 57, 246, 221, 253, 33, 167, 249, 178, 99, 15, 36, 208, 156, 73, 73, 139, 29, 125, 36, 4, 166, 33, 47, 106, 38, 181, 71, 252, 185, 127, 20, 200, 217, 111, 57, 190, 192, 63, 114, 143, 25, 230, 187, 251, 147, 0, 63, 95, 231, 69, 127, 63, 205, 49, 254, 26, 76, 50, 211, 211, 47, 208, 248, 212, 250, 36, 0, 118, 250, 5, 253, 13, 25, 218, 144, 151, 157, 251, 30, 146, 189, 8, 43, 182, 109, 146, 122, 163, 170, 68, 218, 151, 207, 202, 125, 55, 92, 39, 229, 98, 17, 156, 27, 220, 180, 122, 221, 150, 55, 63, 250, 70, 128, 255, 229, 216, 25, 53, 127, 23, 252, 63, 145, 156, 148, 4, 64, 153, 25, 27, 187, 113, 237, 230, 13, 63, 147, 76, 167, 63, 27, 247, 188, 240, 225, 91, 110, 146, 239, 252, 219, 39, 164, 60, 49, 42, 167, 111, 223, 44, 171, 86, 244, 19, 150, 48, 41, 155, 58, 119, 188, 127, 253, 6, 57, 227, 249, 47, 144, 253, 187, 118, 202, 167, 62, 244, 65, 233, 205, 38, 101, 237, 234, 21, 0, 33, 123, 245, 217, 163, 111, 192, 238, 72, 128, 32, 119, 105, 218, 121, 7, 243, 95, 95, 194, 201, 206, 63, 91, 134, 122, 28, 118, 134, 90, 13, 52, 249, 217, 38, 205, 54, 248, 135, 53, 19, 71, 64, 92, 235, 3, 11, 232, 7, 76, 161, 32, 249, 189, 253, 176, 134, 244, 10, 76, 127, 118, 236, 193, 247, 111, 205, 80, 219, 243, 197, 157, 246, 211, 220, 21, 144, 67, 141, 157, 124, 252, 64, 167, 47, 23, 253, 10, 48, 145, 56, 126, 90, 254, 225, 59, 34, 249, 214, 175, 182, 228, 177, 111, 155, 217, 125, 236, 232, 139, 1, 252, 49, 128, 63, 6, 240, 199, 0, 122, 233, 43, 72, 212, 111, 252, 254, 86, 14, 33, 83, 148, 204, 16, 174, 87, 174, 46, 143, 61, 118, 175, 164, 135, 250, 164, 84, 156, 148, 185, 201, 81, 201, 244, 246, 200, 228, 161, 125, 178, 239, 161, 135, 65, 114, 249, 251, 243, 235, 79, 123, 243, 125, 111, 188, 235, 128, 154, 253, 155, 17, 186, 224, 255, 137, 229, 164, 37, 0, 202, 190, 71, 118, 30, 172, 20, 75, 111, 239, 25, 90, 245, 186, 32, 157, 185, 117, 236, 208, 33, 249, 246, 103, 62, 43, 55, 124, 253, 235, 210, 151, 76, 202, 57, 167, 159, 33, 43, 6, 6, 209, 94, 60, 125, 3, 239, 224, 41, 27, 229, 140, 231, 94, 46, 123, 118, 236, 148, 255, 247, 193, 15, 233, 4, 162, 117, 235, 87, 171, 102, 166, 82, 39, 108, 41, 10, 110, 235, 175, 171, 246, 102, 158, 81, 250, 154, 103, 0, 206, 242, 110, 11, 2, 220, 144, 134, 221, 76, 183, 161, 104, 209, 136, 136, 231, 62, 104, 1, 96, 171, 102, 164, 239, 242, 227, 227, 189, 205, 34, 135, 254, 64, 2, 212, 254, 4, 62, 63, 206, 89, 160, 85, 16, 232, 67, 64, 124, 239, 223, 105, 175, 241, 228, 50, 62, 212, 115, 28, 249, 252, 59, 191, 19, 201, 87, 222, 214, 132, 5, 64, 115, 31, 33, 5, 224, 103, 0, 252, 92, 21, 26, 191, 44, 210, 15, 141, 15, 240, 135, 10, 126, 144, 0, 2, 123, 255, 99, 185, 154, 196, 179, 252, 178, 79, 77, 70, 135, 247, 193, 13, 88, 39, 57, 220, 35, 190, 245, 184, 55, 159, 145, 71, 239, 254, 33, 47, 81, 61, 223, 211, 247, 63, 119, 191, 250, 214, 97, 53, 251, 29, 248, 187, 227, 253, 63, 177, 156, 212, 4, 224, 100, 106, 248, 224, 55, 46, 126, 197, 107, 95, 156, 237, 239, 251, 181, 120, 194, 219, 243, 232, 221, 247, 200, 127, 126, 228, 35, 114, 247, 181, 223, 151, 13, 43, 123, 228, 252, 179, 78, 147, 161, 190, 62, 248, 209, 161, 172, 220, 184, 85, 206, 124, 230, 11, 100, 247, 195, 59, 229, 63, 63, 241, 41, 89, 181, 102, 181, 172, 26, 234, 129, 86, 230, 39, 183, 12, 106, 141, 25, 223, 208, 37, 49, 78, 16, 211, 10, 176, 115, 1, 13, 57, 48, 77, 1, 79, 211, 31, 32, 14, 169, 241, 57, 164, 103, 92, 15, 62, 139, 96, 172, 8, 0, 94, 243, 96, 101, 112, 26, 50, 135, 252, 0, 106, 106, 255, 86, 9, 0, 7, 248, 117, 120, 111, 54, 161, 166, 127, 72, 205, 79, 66, 0, 57, 16, 252, 107, 46, 240, 244, 137, 190, 220, 42, 99, 121, 28, 107, 225, 203, 73, 110, 254, 219, 80, 190, 240, 179, 13, 153, 217, 143, 235, 3, 240, 199, 172, 230, 23, 5, 63, 76, 254, 62, 130, 127, 206, 130, 31, 203, 30, 132, 108, 73, 248, 186, 47, 150, 77, 129, 200, 10, 51, 99, 82, 44, 151, 0, 254, 1, 201, 244, 15, 200, 202, 181, 107, 244, 253, 126, 99, 7, 14, 75, 44, 145, 248, 254, 222, 215, 236, 254, 174, 130, 127, 11, 66, 14, 161, 11, 254, 159, 74, 158, 22, 4, 64, 185, 233, 139, 159, 45, 23, 38, 167, 62, 190, 102, 235, 214, 231, 37, 243, 185, 143, 52, 27, 141, 198, 237, 63, 248, 129, 124, 234, 31, 62, 36, 119, 95, 247, 3, 89, 61, 144, 147, 83, 54, 174, 6, 144, 155, 178, 106, 243, 169, 114, 234, 5, 151, 200, 61, 55, 223, 34, 87, 252, 219, 167, 100, 221, 198, 83, 164, 119, 168, 95, 223, 16, 68, 173, 14, 28, 3, 230, 208, 210, 156, 47, 208, 9, 58, 27, 119, 86, 0, 93, 12, 254, 200, 6, 204, 34, 224, 93, 112, 101, 16, 197, 18, 128, 7, 240, 73, 0, 80, 125, 10, 108, 126, 131, 143, 190, 191, 250, 249, 14, 252, 156, 218, 203, 117, 130, 31, 166, 127, 118, 40, 46, 151, 190, 39, 46, 171, 47, 48, 224, 119, 86, 201, 177, 18, 190, 146, 236, 170, 223, 139, 228, 154, 15, 52, 37, 198, 143, 118, 194, 223, 87, 240, 91, 205, 31, 235, 169, 0, 252, 32, 0, 0, 63, 26, 64, 160, 249, 15, 66, 224, 195, 62, 28, 9, 224, 51, 255, 236, 35, 72, 231, 2, 125, 159, 95, 0, 127, 63, 158, 0, 9, 226, 58, 15, 14, 13, 233, 147, 125, 240, 253, 37, 17, 4, 95, 147, 215, 96, 135, 219, 16, 186, 224, 127, 74, 242, 180, 33, 0, 39, 7, 30, 124, 228, 80, 101, 118, 238, 55, 123, 86, 172, 120, 99, 144, 76, 60, 90, 152, 157, 149, 235, 191, 243, 29, 249, 236, 135, 62, 44, 213, 233, 73, 217, 176, 113, 173, 62, 188, 179, 254, 172, 179, 100, 195, 217, 231, 202, 205, 223, 251, 158, 92, 243, 181, 175, 203, 169, 91, 54, 72, 30, 13, 146, 175, 228, 6, 158, 21, 208, 166, 227, 15, 90, 95, 237, 127, 180, 64, 203, 5, 166, 31, 192, 36, 233, 196, 34, 21, 128, 93, 183, 49, 121, 6, 248, 8, 88, 154, 56, 19, 9, 126, 196, 235, 0, 127, 5, 64, 39, 1, 204, 1, 252, 12, 5, 4, 213, 252, 190, 68, 53, 180, 120, 144, 198, 214, 151, 198, 229, 172, 55, 155, 91, 120, 172, 193, 63, 181, 35, 146, 175, 254, 130, 233, 233, 39, 248, 99, 73, 128, 63, 141, 147, 1, 248, 141, 230, 135, 217, 15, 240, 199, 250, 73, 0, 236, 248, 43, 75, 152, 71, 200, 32, 240, 69, 31, 1, 192, 239, 215, 197, 75, 68, 226, 167, 227, 178, 115, 231, 99, 210, 187, 122, 21, 206, 19, 110, 125, 220, 147, 116, 54, 43, 7, 118, 238, 194, 158, 98, 181, 108, 174, 247, 230, 216, 153, 136, 246, 32, 116, 193, 255, 148, 228, 105, 71, 0, 78, 166, 135, 135, 191, 177, 230, 212, 173, 47, 73, 164, 211, 87, 113, 189, 48, 51, 43, 223, 255, 252, 21, 226, 195, 52, 95, 185, 106, 16, 88, 140, 100, 195, 5, 23, 72, 255, 250, 83, 228, 27, 159, 253, 156, 60, 242, 195, 123, 228, 244, 109, 91, 244, 1, 163, 168, 137, 6, 174, 224, 101, 135, 93, 83, 53, 20, 205, 120, 182, 196, 56, 90, 36, 63, 253, 29, 139, 155, 97, 67, 21, 91, 86, 77, 7, 237, 165, 2, 192, 177, 170, 92, 2, 224, 187, 145, 1, 157, 101, 168, 157, 127, 30, 64, 158, 148, 168, 196, 158, 126, 128, 158, 129, 29, 129, 214, 239, 103, 7, 97, 186, 55, 46, 207, 255, 83, 214, 117, 236, 133, 143, 242, 126, 231, 55, 66, 121, 244, 235, 124, 114, 146, 90, 159, 61, 253, 208, 252, 218, 211, 15, 173, 175, 102, 127, 1, 192, 159, 83, 211, 63, 234, 1, 224, 65, 10, 230, 205, 62, 53, 5, 191, 126, 215, 63, 222, 130, 249, 239, 75, 181, 86, 150, 241, 233, 89, 233, 229, 124, 141, 86, 36, 233, 116, 70, 226, 184, 62, 135, 246, 239, 225, 80, 234, 174, 109, 23, 95, 176, 71, 250, 177, 227, 0, 225, 105, 219, 130, 143, 142, 60, 173, 47, 223, 222, 251, 31, 60, 184, 254, 172, 115, 126, 49, 72, 165, 30, 230, 100, 158, 185, 233, 105, 185, 238, 203, 223, 144, 149, 131, 131, 210, 223, 219, 3, 0, 199, 101, 219, 101, 207, 148, 84, 62, 47, 95, 250, 228, 127, 32, 127, 70, 86, 175, 89, 97, 52, 55, 130, 17, 3, 114, 213, 224, 136, 18, 252, 58, 158, 111, 177, 175, 239, 4, 80, 21, 143, 0, 38, 152, 215, 248, 243, 65, 123, 253, 153, 78, 243, 31, 224, 214, 201, 63, 21, 79, 31, 230, 161, 27, 160, 224, 175, 80, 243, 131, 24, 224, 30, 112, 166, 223, 51, 127, 207, 147, 252, 90, 187, 147, 99, 36, 60, 231, 67, 183, 69, 242, 131, 15, 132, 178, 247, 58, 51, 204, 167, 154, 63, 83, 151, 120, 206, 12, 243, 197, 218, 90, 159, 38, 127, 17, 90, 187, 168, 254, 62, 95, 233, 165, 31, 246, 244, 169, 249, 177, 109, 12, 164, 17, 11, 37, 7, 43, 107, 122, 98, 92, 63, 219, 21, 4, 32, 64, 176, 36, 223, 233, 55, 61, 122, 88, 138, 211, 83, 226, 167, 18, 247, 126, 239, 147, 87, 148, 187, 115, 252, 143, 142, 60, 237, 249, 115, 231, 237, 183, 14, 231, 7, 7, 127, 59, 242, 165, 22, 228, 82, 178, 103, 199, 35, 114, 35, 92, 130, 158, 222, 62, 104, 158, 164, 36, 50, 25, 217, 118, 233, 51, 100, 244, 240, 33, 249, 250, 231, 63, 47, 61, 61, 57, 9, 146, 0, 38, 52, 18, 193, 75, 33, 16, 244, 89, 1, 128, 223, 194, 220, 180, 77, 5, 120, 76, 248, 61, 66, 243, 92, 0, 0, 46, 77, 125, 86, 64, 183, 37, 248, 181, 12, 73, 0, 91, 48, 95, 251, 0, 232, 79, 99, 89, 197, 118, 156, 7, 128, 208, 6, 63, 8, 98, 104, 123, 76, 46, 249, 205, 99, 219, 250, 233, 239, 239, 134, 237, 244, 237, 223, 104, 201, 190, 27, 0, 122, 118, 246, 165, 168, 245, 235, 218, 147, 47, 244, 247, 85, 243, 195, 199, 183, 38, 191, 228, 139, 210, 212, 151, 121, 150, 165, 233, 67, 251, 199, 81, 62, 142, 147, 7, 240, 69, 151, 145, 228, 178, 105, 57, 188, 111, 159, 164, 250, 251, 148, 60, 249, 74, 240, 30, 16, 192, 161, 189, 123, 165, 85, 111, 73, 220, 75, 222, 209, 157, 226, 123, 244, 228, 105, 79, 0, 148, 137, 3, 7, 174, 10, 82, 201, 79, 213, 155, 53, 217, 116, 250, 233, 242, 216, 237, 55, 203, 3, 55, 92, 39, 189, 125, 253, 240, 63, 227, 210, 183, 110, 189, 156, 114, 218, 153, 114, 199, 141, 55, 202, 248, 240, 97, 233, 237, 205, 43, 120, 41, 4, 175, 49, 227, 13, 33, 216, 100, 27, 231, 26, 193, 15, 48, 187, 117, 107, 9, 104, 25, 4, 166, 51, 80, 248, 54, 92, 87, 132, 79, 0, 82, 211, 71, 13, 79, 73, 65, 227, 8, 236, 39, 56, 237, 103, 225, 38, 64, 97, 30, 75, 121, 236, 107, 34, 95, 127, 103, 75, 198, 30, 128, 249, 226, 198, 248, 169, 249, 243, 53, 104, 125, 118, 246, 21, 37, 54, 0, 141, 63, 0, 211, 159, 29, 125, 61, 8, 89, 219, 209, 199, 239, 248, 7, 32, 13, 223, 0, 63, 212, 7, 163, 66, 190, 180, 83, 103, 88, 30, 30, 30, 145, 20, 71, 101, 112, 17, 19, 65, 90, 159, 234, 60, 188, 123, 15, 45, 171, 40, 221, 147, 125, 168, 11, 254, 163, 39, 93, 2, 176, 178, 97, 203, 169, 127, 19, 11, 195, 145, 210, 236, 140, 92, 248, 226, 151, 130, 4, 110, 149, 123, 190, 255, 61, 241, 57, 237, 23, 97, 205, 169, 219, 116, 174, 192, 125, 63, 252, 161, 244, 245, 231, 209, 92, 249, 250, 48, 6, 52, 96, 180, 99, 246, 236, 211, 255, 213, 30, 126, 164, 105, 31, 1, 73, 129, 37, 169, 241, 249, 179, 36, 65, 132, 171, 197, 224, 2, 1, 175, 36, 128, 124, 101, 10, 18, 6, 130, 142, 12, 32, 89, 131, 177, 16, 24, 182, 191, 242, 216, 190, 200, 243, 222, 79, 135, 242, 173, 95, 107, 73, 121, 210, 128, 159, 102, 127, 28, 154, 63, 158, 135, 246, 135, 230, 143, 89, 159, 159, 4, 192, 167, 249, 132, 179, 253, 248, 160, 79, 10, 228, 64, 43, 129, 243, 2, 248, 177, 143, 88, 11, 208, 39, 147, 241, 9, 74, 145, 68, 34, 37, 229, 82, 69, 138, 181, 166, 198, 9, 242, 60, 220, 175, 102, 165, 172, 239, 121, 244, 131, 96, 180, 103, 96, 240, 222, 46, 248, 143, 158, 116, 9, 192, 202, 163, 119, 223, 179, 55, 155, 206, 252, 203, 228, 129, 131, 146, 134, 217, 255, 226, 183, 189, 77, 166, 70, 134, 229, 222, 171, 190, 39, 17, 76, 207, 244, 208, 128, 244, 12, 174, 148, 31, 222, 112, 11, 154, 43, 26, 188, 47, 210, 104, 214, 205, 184, 62, 64, 171, 110, 0, 144, 220, 168, 55, 244, 123, 254, 6, 200, 248, 227, 36, 31, 176, 1, 73, 128, 73, 4, 54, 231, 5, 68, 80, 225, 236, 27, 136, 145, 44, 176, 127, 37, 1, 30, 8, 202, 162, 52, 35, 154, 206, 53, 77, 183, 255, 249, 238, 126, 143, 111, 184, 57, 70, 114, 231, 71, 67, 249, 222, 239, 133, 82, 43, 24, 240, 199, 1, 232, 120, 22, 160, 38, 248, 97, 230, 199, 250, 203, 218, 209, 71, 159, 159, 68, 16, 230, 1, 254, 44, 210, 116, 136, 15, 36, 161, 224, 231, 217, 224, 26, 233, 143, 228, 40, 130, 75, 44, 169, 84, 74, 248, 62, 199, 68, 42, 43, 230, 181, 236, 49, 233, 227, 55, 252, 97, 17, 148, 139, 5, 88, 2, 254, 189, 247, 220, 124, 219, 152, 57, 146, 174, 28, 13, 233, 18, 64, 135, 172, 61, 243, 236, 143, 121, 9, 127, 215, 125, 55, 221, 44, 249, 190, 94, 57, 251, 217, 207, 149, 222, 85, 107, 228, 81, 184, 4, 124, 52, 120, 53, 172, 128, 145, 253, 7, 228, 240, 158, 125, 208, 76, 57, 11, 106, 90, 1, 104, 204, 32, 0, 6, 190, 207, 143, 243, 5, 248, 197, 161, 6, 26, 181, 233, 233, 159, 135, 51, 225, 175, 130, 237, 204, 48, 34, 192, 143, 114, 20, 195, 25, 248, 209, 124, 232, 240, 139, 231, 71, 248, 34, 89, 117, 14, 240, 180, 194, 174, 46, 163, 180, 234, 145, 220, 252, 55, 0, 255, 31, 180, 164, 94, 194, 1, 171, 207, 207, 14, 63, 130, 191, 170, 154, 62, 14, 240, 199, 7, 236, 112, 95, 15, 223, 228, 3, 179, 31, 62, 63, 95, 244, 209, 162, 217, 207, 183, 254, 80, 243, 219, 9, 83, 60, 31, 10, 207, 47, 9, 82, 72, 37, 227, 50, 51, 59, 41, 137, 52, 251, 88, 248, 253, 198, 184, 100, 50, 105, 57, 184, 103, 175, 112, 74, 182, 23, 36, 110, 110, 111, 212, 149, 163, 34, 93, 2, 232, 144, 7, 111, 186, 121, 44, 145, 235, 255, 251, 209, 67, 135, 228, 224, 174, 221, 146, 131, 249, 217, 3, 2, 240, 252, 132, 76, 236, 223, 39, 3, 167, 156, 34, 17, 252, 209, 7, 238, 184, 75, 134, 56, 133, 24, 45, 87, 59, 170, 32, 142, 0, 56, 203, 143, 111, 19, 38, 41, 152, 89, 127, 112, 11, 90, 166, 15, 64, 9, 131, 20, 192, 241, 126, 118, 10, 50, 206, 109, 172, 54, 4, 204, 16, 39, 49, 216, 37, 159, 46, 116, 254, 174, 6, 83, 42, 104, 63, 227, 184, 60, 162, 224, 255, 91, 78, 240, 33, 209, 1, 248, 156, 224, 3, 240, 59, 205, 31, 239, 173, 26, 237, 15, 18, 208, 94, 255, 62, 128, 62, 7, 141, 159, 54, 143, 251, 182, 60, 196, 227, 56, 31, 156, 3, 9, 146, 215, 65, 59, 71, 121, 142, 252, 145, 235, 152, 135, 139, 52, 91, 44, 75, 58, 223, 171, 214, 81, 16, 4, 18, 247, 2, 220, 139, 157, 124, 152, 42, 74, 165, 51, 183, 218, 67, 234, 202, 81, 146, 46, 1, 44, 146, 83, 207, 189, 248, 179, 241, 132, 127, 247, 61, 215, 223, 40, 89, 104, 159, 176, 81, 145, 21, 91, 183, 200, 196, 193, 189, 18, 36, 147, 210, 183, 106, 165, 60, 124, 247, 93, 18, 131, 153, 79, 147, 21, 45, 89, 183, 115, 179, 251, 76, 220, 17, 130, 105, 212, 124, 47, 97, 147, 253, 3, 218, 248, 153, 206, 102, 79, 91, 128, 129, 63, 192, 157, 219, 224, 71, 18, 208, 87, 143, 171, 21, 192, 64, 224, 216, 117, 72, 109, 38, 146, 196, 50, 19, 192, 157, 31, 3, 1, 252, 35, 128, 175, 143, 241, 34, 40, 248, 233, 239, 27, 224, 155, 9, 62, 198, 252, 23, 104, 254, 88, 222, 188, 203, 159, 38, 127, 75, 199, 247, 113, 114, 158, 57, 7, 115, 13, 44, 240, 45, 121, 170, 229, 19, 243, 225, 62, 53, 165, 222, 140, 36, 160, 255, 15, 201, 102, 179, 82, 46, 20, 101, 234, 240, 176, 120, 137, 228, 84, 255, 234, 245, 143, 104, 70, 87, 142, 154, 116, 9, 96, 145, 220, 241, 253, 111, 21, 251, 86, 172, 248, 155, 169, 177, 49, 57, 176, 99, 135, 190, 59, 48, 147, 203, 74, 174, 127, 64, 230, 198, 71, 101, 245, 214, 109, 50, 58, 124, 88, 14, 236, 218, 133, 188, 30, 133, 44, 129, 78, 33, 9, 80, 140, 201, 142, 6, 78, 82, 80, 96, 3, 214, 180, 10, 224, 15, 16, 244, 106, 53, 232, 134, 182, 28, 150, 44, 107, 190, 61, 104, 147, 9, 122, 254, 116, 201, 122, 145, 138, 122, 103, 15, 138, 148, 150, 209, 11, 190, 231, 147, 161, 220, 252, 15, 176, 106, 106, 212, 252, 240, 249, 161, 213, 249, 164, 158, 1, 127, 197, 0, 127, 128, 90, 127, 190, 179, 143, 31, 239, 100, 79, 191, 120, 8, 113, 142, 239, 155, 115, 112, 126, 191, 17, 196, 16, 197, 101, 209, 156, 152, 239, 75, 113, 174, 168, 174, 22, 223, 151, 16, 143, 251, 146, 201, 230, 100, 236, 224, 65, 41, 151, 177, 15, 207, 127, 244, 221, 31, 251, 194, 97, 187, 113, 87, 142, 146, 116, 9, 224, 8, 242, 198, 255, 254, 171, 95, 245, 211, 137, 171, 239, 185, 237, 86, 104, 161, 28, 16, 87, 146, 85, 155, 182, 72, 113, 114, 66, 122, 87, 174, 17, 63, 149, 151, 123, 238, 186, 67, 122, 114, 121, 237, 172, 98, 67, 86, 19, 159, 32, 70, 139, 214, 134, 13, 204, 210, 180, 39, 106, 105, 238, 2, 201, 58, 118, 222, 68, 48, 175, 26, 39, 216, 81, 16, 100, 192, 215, 147, 183, 168, 233, 81, 38, 4, 123, 32, 85, 235, 48, 22, 0, 192, 193, 187, 196, 254, 0, 228, 148, 198, 67, 153, 59, 192, 248, 210, 11, 191, 205, 127, 195, 95, 153, 222, 126, 53, 251, 249, 40, 47, 193, 111, 129, 31, 239, 47, 34, 0, 252, 236, 245, 39, 248, 97, 21, 132, 124, 175, 159, 215, 0, 248, 233, 190, 96, 155, 24, 220, 4, 190, 4, 4, 231, 233, 30, 137, 54, 255, 236, 57, 240, 210, 32, 202, 23, 175, 148, 43, 21, 190, 206, 27, 70, 79, 76, 146, 65, 90, 71, 2, 14, 236, 217, 35, 213, 38, 182, 245, 130, 251, 255, 199, 185, 235, 120, 65, 187, 114, 20, 165, 75, 0, 71, 144, 143, 253, 201, 251, 235, 189, 253, 171, 255, 162, 56, 51, 215, 56, 176, 227, 49, 52, 82, 190, 123, 34, 33, 137, 116, 70, 234, 149, 178, 172, 216, 176, 73, 30, 186, 251, 30, 184, 7, 53, 73, 38, 19, 10, 104, 71, 2, 78, 220, 68, 33, 2, 189, 253, 228, 32, 164, 229, 70, 14, 240, 107, 134, 45, 237, 40, 52, 195, 134, 184, 21, 252, 236, 56, 17, 65, 141, 169, 62, 51, 151, 168, 132, 38, 180, 237, 16, 172, 205, 138, 236, 185, 186, 99, 71, 75, 36, 124, 145, 199, 247, 255, 176, 37, 115, 7, 141, 230, 231, 188, 254, 24, 125, 126, 29, 230, 3, 248, 213, 215, 167, 207, 15, 173, 77, 55, 128, 239, 240, 231, 27, 125, 225, 243, 71, 252, 166, 95, 28, 36, 192, 23, 128, 160, 46, 37, 58, 53, 139, 216, 220, 76, 191, 135, 73, 51, 224, 167, 240, 122, 53, 193, 116, 30, 252, 126, 230, 101, 82, 9, 37, 133, 137, 131, 251, 37, 237, 197, 64, 6, 254, 131, 166, 100, 87, 142, 166, 116, 9, 224, 9, 228, 182, 221, 123, 174, 75, 164, 83, 95, 125, 248, 238, 59, 36, 225, 199, 164, 90, 152, 149, 193, 83, 54, 72, 173, 56, 167, 203, 233, 241, 105, 217, 183, 123, 135, 12, 12, 246, 169, 249, 110, 252, 121, 85, 109, 182, 129, 179, 81, 115, 22, 32, 137, 129, 198, 47, 236, 129, 200, 60, 55, 64, 95, 87, 45, 5, 52, 122, 125, 6, 0, 233, 45, 37, 9, 187, 29, 97, 3, 173, 25, 249, 88, 242, 169, 58, 6, 18, 2, 252, 104, 150, 191, 231, 83, 168, 13, 190, 242, 82, 9, 95, 222, 249, 189, 223, 109, 201, 204, 30, 176, 19, 125, 126, 157, 228, 195, 14, 63, 107, 246, 211, 220, 87, 2, 128, 214, 199, 186, 100, 171, 218, 47, 192, 39, 255, 34, 104, 255, 16, 154, 159, 115, 150, 90, 32, 52, 210, 94, 20, 143, 235, 82, 93, 28, 92, 16, 62, 80, 197, 190, 17, 158, 43, 3, 153, 160, 14, 38, 164, 155, 65, 163, 137, 215, 33, 153, 12, 68, 26, 85, 153, 26, 159, 0, 47, 122, 81, 144, 78, 119, 253, 255, 37, 144, 46, 1, 60, 129, 108, 134, 34, 26, 92, 183, 230, 111, 170, 229, 114, 105, 255, 142, 29, 226, 161, 129, 114, 66, 80, 144, 201, 74, 50, 151, 149, 20, 92, 131, 135, 126, 120, 151, 164, 104, 178, 2, 247, 236, 237, 167, 152, 137, 61, 104, 232, 186, 52, 141, 156, 238, 0, 63, 47, 102, 214, 77, 104, 52, 57, 15, 192, 128, 152, 101, 8, 122, 243, 189, 0, 8, 231, 197, 3, 72, 145, 135, 37, 181, 105, 192, 96, 59, 225, 64, 6, 51, 251, 35, 249, 225, 191, 134, 210, 168, 152, 237, 143, 182, 112, 156, 127, 255, 205, 6, 252, 102, 110, 63, 192, 159, 171, 89, 237, 15, 208, 171, 214, 39, 248, 97, 1, 16, 252, 180, 14, 0, 126, 30, 99, 72, 115, 159, 179, 251, 112, 82, 10, 120, 92, 28, 215, 241, 233, 132, 167, 205, 85, 250, 255, 76, 39, 109, 242, 75, 190, 124, 109, 154, 10, 242, 252, 192, 151, 42, 200, 182, 82, 42, 128, 0, 226, 179, 233, 222, 254, 125, 38, 179, 43, 71, 83, 186, 4, 240, 36, 114, 224, 161, 71, 238, 74, 164, 211, 255, 182, 227, 129, 7, 160, 128, 91, 50, 125, 224, 128, 244, 13, 14, 74, 34, 149, 150, 129, 245, 235, 101, 247, 195, 143, 66, 73, 213, 244, 101, 159, 250, 85, 96, 136, 62, 1, 8, 115, 159, 38, 191, 33, 1, 234, 62, 211, 224, 21, 3, 54, 24, 237, 111, 242, 212, 235, 135, 121, 207, 71, 139, 1, 107, 99, 254, 251, 161, 196, 248, 214, 92, 126, 39, 143, 32, 164, 22, 166, 54, 6, 200, 72, 2, 55, 252, 69, 75, 166, 248, 37, 196, 163, 44, 215, 255, 121, 40, 15, 125, 25, 192, 39, 225, 180, 253, 126, 7, 126, 104, 126, 23, 242, 88, 231, 163, 190, 204, 199, 241, 145, 44, 216, 231, 65, 183, 198, 24, 66, 60, 39, 158, 51, 79, 150, 193, 44, 56, 161, 209, 116, 106, 96, 129, 115, 166, 240, 250, 101, 130, 184, 76, 14, 31, 68, 139, 52, 150, 84, 34, 153, 148, 194, 244, 180, 78, 172, 138, 251, 137, 177, 13, 219, 182, 13, 107, 225, 174, 28, 85, 233, 18, 192, 143, 144, 161, 53, 235, 254, 9, 154, 121, 116, 255, 206, 71, 164, 60, 57, 34, 62, 76, 245, 32, 149, 210, 78, 193, 137, 241, 97, 25, 63, 184, 79, 250, 123, 123, 1, 102, 0, 6, 26, 92, 193, 79, 125, 110, 59, 250, 218, 32, 215, 134, 175, 22, 46, 177, 128, 178, 158, 209, 128, 200, 214, 205, 128, 33, 69, 72, 204, 154, 208, 30, 172, 1, 29, 111, 71, 38, 223, 166, 3, 45, 203, 111, 229, 43, 40, 1, 206, 242, 84, 40, 215, 188, 191, 41, 35, 119, 27, 16, 29, 13, 185, 243, 99, 161, 220, 250, 79, 56, 110, 30, 152, 3, 63, 167, 248, 246, 64, 203, 247, 0, 244, 28, 226, 3, 17, 112, 190, 63, 59, 252, 120, 92, 17, 128, 31, 113, 114, 79, 12, 212, 165, 95, 49, 37, 202, 121, 30, 166, 78, 133, 51, 226, 154, 140, 117, 125, 220, 193, 18, 130, 153, 28, 5, 2, 240, 2, 41, 206, 204, 74, 121, 182, 128, 122, 224, 46, 192, 117, 72, 38, 211, 208, 254, 85, 88, 78, 40, 231, 5, 123, 191, 251, 185, 207, 195, 228, 232, 202, 209, 150, 46, 1, 252, 8, 57, 240, 200, 163, 123, 50, 249, 190, 143, 28, 222, 179, 95, 166, 70, 199, 101, 118, 108, 76, 135, 169, 178, 3, 253, 112, 3, 178, 114, 219, 181, 87, 75, 95, 127, 14, 90, 153, 166, 46, 3, 26, 58, 90, 186, 51, 247, 221, 59, 1, 180, 205, 27, 46, 80, 32, 52, 224, 23, 212, 1, 24, 157, 52, 4, 244, 243, 199, 167, 4, 117, 136, 144, 128, 130, 217, 223, 74, 212, 36, 76, 87, 205, 123, 243, 51, 244, 181, 161, 137, 181, 163, 205, 152, 219, 187, 174, 106, 193, 92, 111, 201, 222, 107, 81, 185, 21, 5, 215, 79, 33, 119, 253, 223, 16, 218, 191, 37, 245, 42, 181, 57, 193, 143, 125, 100, 64, 0, 240, 251, 37, 111, 123, 250, 105, 242, 179, 31, 128, 199, 145, 114, 224, 103, 224, 28, 7, 16, 7, 153, 140, 130, 83, 118, 199, 225, 248, 192, 164, 241, 26, 240, 186, 112, 38, 160, 185, 14, 204, 227, 107, 213, 249, 122, 244, 76, 79, 175, 41, 4, 9, 224, 2, 204, 78, 78, 74, 210, 211, 254, 0, 218, 58, 38, 163, 43, 71, 85, 186, 4, 240, 99, 200, 150, 179, 206, 253, 112, 24, 151, 7, 198, 71, 71, 117, 26, 112, 46, 157, 22, 31, 38, 234, 202, 77, 219, 228, 190, 59, 238, 145, 221, 15, 62, 44, 43, 6, 135, 116, 250, 175, 130, 31, 77, 213, 144, 128, 177, 0, 216, 166, 73, 3, 218, 182, 109, 96, 199, 33, 25, 65, 9, 0, 73, 244, 147, 213, 13, 96, 224, 16, 26, 159, 150, 35, 208, 9, 120, 2, 142, 143, 216, 230, 72, 4, 198, 26, 224, 67, 53, 36, 129, 253, 183, 52, 229, 59, 191, 213, 144, 251, 254, 61, 148, 234, 204, 60, 225, 252, 184, 194, 126, 4, 250, 252, 223, 254, 245, 38, 52, 176, 177, 46, 204, 107, 188, 16, 8, 126, 106, 254, 94, 190, 183, 31, 113, 238, 31, 199, 67, 183, 36, 66, 57, 2, 159, 179, 21, 213, 223, 39, 121, 241, 71, 247, 197, 98, 85, 125, 127, 141, 233, 169, 193, 186, 215, 171, 160, 215, 70, 99, 230, 18, 232, 208, 231, 92, 189, 41, 126, 38, 133, 109, 248, 178, 149, 22, 8, 33, 148, 82, 97, 22, 245, 194, 78, 136, 249, 75, 224, 236, 116, 133, 210, 37, 128, 31, 67, 238, 185, 238, 154, 153, 76, 111, 239, 255, 134, 246, 10, 71, 15, 236, 151, 102, 5, 26, 17, 141, 54, 191, 114, 181, 118, 240, 93, 119, 229, 247, 36, 147, 9, 36, 145, 72, 180, 65, 78, 13, 104, 130, 33, 0, 37, 4, 166, 115, 201, 134, 207, 114, 16, 78, 141, 213, 183, 9, 113, 174, 0, 81, 1, 164, 232, 43, 198, 125, 235, 255, 183, 191, 154, 3, 11, 128, 218, 151, 64, 228, 155, 118, 212, 247, 54, 36, 48, 185, 3, 0, 254, 141, 166, 92, 253, 222, 80, 118, 95, 21, 73, 173, 96, 43, 127, 18, 41, 142, 70, 242, 192, 231, 66, 249, 242, 91, 91, 114, 215, 255, 51, 157, 140, 218, 217, 104, 205, 126, 125, 141, 23, 247, 9, 147, 95, 3, 247, 139, 99, 145, 20, 142, 135, 150, 9, 191, 233, 7, 171, 69, 225, 142, 227, 54, 243, 30, 72, 104, 28, 209, 176, 150, 128, 19, 228, 145, 0, 12, 25, 98, 59, 196, 89, 222, 93, 3, 94, 203, 66, 169, 136, 214, 24, 87, 194, 96, 125, 190, 231, 73, 165, 88, 52, 46, 65, 36, 123, 76, 201, 174, 28, 109, 233, 18, 192, 143, 41, 255, 227, 143, 254, 252, 203, 201, 116, 250, 251, 149, 66, 17, 110, 192, 176, 36, 248, 125, 65, 8, 59, 176, 118, 62, 248, 144, 220, 118, 245, 213, 178, 98, 168, 7, 141, 218, 180, 106, 5, 59, 193, 109, 27, 185, 54, 108, 172, 16, 26, 174, 145, 187, 52, 237, 39, 208, 117, 130, 137, 1, 165, 248, 150, 156, 4, 39, 214, 88, 2, 224, 139, 51, 245, 35, 26, 198, 15, 239, 36, 1, 106, 227, 70, 35, 148, 187, 254, 173, 37, 223, 124, 119, 75, 190, 246, 11, 161, 220, 242, 15, 161, 236, 185, 38, 146, 122, 209, 28, 64, 117, 54, 210, 47, 244, 28, 184, 41, 146, 187, 254, 53, 148, 171, 160, 245, 175, 253, 0, 223, 217, 15, 223, 157, 147, 17, 172, 217, 79, 205, 47, 244, 241, 21, 248, 236, 236, 67, 200, 98, 159, 32, 132, 88, 10, 233, 212, 254, 28, 141, 160, 165, 2, 224, 234, 49, 235, 57, 224, 120, 121, 210, 16, 115, 126, 100, 51, 8, 138, 242, 26, 152, 28, 198, 89, 158, 17, 19, 103, 132, 63, 94, 87, 247, 190, 69, 118, 18, 70, 136, 207, 76, 207, 48, 94, 75, 103, 51, 135, 184, 109, 87, 142, 190, 156, 148, 159, 6, 91, 42, 89, 179, 110, 237, 27, 166, 70, 71, 255, 235, 180, 11, 47, 136, 157, 253, 194, 23, 201, 99, 247, 63, 36, 187, 238, 184, 85, 206, 184, 228, 50, 121, 244, 246, 91, 229, 23, 126, 247, 183, 1, 138, 164, 212, 107, 0, 38, 0, 18, 131, 234, 243, 188, 64, 31, 106, 33, 30, 8, 244, 56, 52, 155, 187, 224, 100, 95, 198, 57, 255, 39, 137, 50, 92, 227, 20, 88, 106, 75, 143, 183, 165, 225, 139, 212, 17, 106, 32, 155, 98, 90, 164, 144, 17, 153, 97, 200, 138, 204, 102, 37, 154, 69, 188, 148, 20, 169, 192, 242, 168, 113, 220, 28, 101, 249, 102, 161, 86, 92, 146, 61, 156, 188, 132, 40, 148, 185, 214, 159, 135, 130, 71, 241, 105, 232, 210, 86, 13, 59, 224, 206, 57, 191, 0, 96, 54, 15, 247, 208, 218, 48, 62, 191, 142, 245, 211, 231, 231, 44, 191, 190, 138, 196, 117, 150, 31, 253, 126, 106, 127, 104, 112, 125, 17, 39, 145, 74, 237, 76, 23, 166, 165, 239, 74, 80, 34, 131, 132, 4, 48, 207, 12, 238, 13, 211, 76, 42, 176, 13, 130, 160, 133, 64, 194, 97, 81, 146, 6, 115, 51, 153, 172, 236, 120, 224, 126, 201, 15, 172, 132, 107, 149, 146, 192, 247, 100, 235, 198, 53, 242, 209, 191, 254, 43, 153, 154, 24, 31, 221, 188, 109, 251, 121, 15, 220, 113, 119, 247, 49, 224, 37, 144, 174, 5, 240, 19, 200, 250, 211, 207, 191, 58, 244, 18, 187, 246, 237, 220, 205, 206, 122, 129, 69, 32, 233, 124, 94, 54, 108, 219, 46, 167, 158, 125, 150, 236, 223, 185, 11, 150, 1, 103, 243, 113, 218, 43, 193, 204, 215, 128, 161, 137, 179, 161, 59, 112, 48, 78, 63, 128, 157, 102, 170, 251, 40, 150, 18, 168, 253, 9, 151, 120, 12, 56, 6, 88, 60, 0, 139, 19, 107, 56, 175, 94, 251, 2, 172, 43, 160, 38, 57, 64, 154, 195, 18, 86, 0, 135, 9, 205, 240, 32, 130, 206, 28, 132, 27, 48, 23, 73, 121, 2, 220, 49, 11, 237, 63, 5, 190, 216, 39, 50, 241, 48, 193, 111, 119, 7, 31, 187, 77, 0, 220, 158, 218, 159, 90, 30, 96, 143, 245, 240, 139, 61, 208, 250, 124, 202, 143, 174, 0, 246, 29, 194, 210, 8, 237, 27, 124, 84, 227, 107, 167, 31, 200, 128, 199, 172, 235, 216, 111, 196, 115, 71, 147, 226, 73, 225, 124, 85, 219, 35, 170, 130, 115, 230, 100, 39, 150, 83, 50, 212, 60, 210, 68, 76, 106, 53, 212, 223, 108, 130, 40, 19, 234, 26, 240, 93, 140, 156, 87, 209, 168, 147, 72, 227, 135, 94, 244, 138, 87, 226, 44, 186, 178, 20, 210, 37, 128, 159, 64, 238, 184, 250, 219, 51, 169, 164, 255, 157, 234, 220, 172, 132, 149, 146, 172, 90, 187, 94, 134, 214, 111, 84, 80, 111, 62, 231, 92, 57, 204, 247, 214, 233, 132, 32, 66, 27, 45, 153, 49, 32, 160, 217, 156, 39, 0, 34, 194, 152, 190, 6, 244, 204, 103, 22, 63, 28, 162, 121, 64, 64, 216, 2, 80, 232, 95, 199, 16, 60, 248, 212, 124, 162, 46, 97, 128, 24, 115, 125, 2, 89, 46, 185, 142, 96, 251, 2, 196, 2, 84, 181, 179, 86, 239, 150, 86, 218, 105, 54, 128, 0, 98, 1, 182, 209, 201, 62, 244, 251, 193, 14, 121, 44, 123, 177, 36, 240, 117, 146, 15, 193, 143, 52, 237, 240, 227, 72, 63, 206, 140, 125, 20, 252, 185, 165, 158, 15, 235, 164, 62, 71, 9, 18, 3, 93, 2, 166, 243, 180, 176, 48, 222, 129, 126, 67, 217, 30, 146, 33, 13, 6, 2, 61, 72, 165, 205, 68, 32, 172, 243, 181, 96, 149, 114, 73, 42, 197, 50, 191, 148, 252, 240, 7, 255, 236, 127, 147, 182, 186, 178, 4, 210, 37, 128, 159, 80, 178, 249, 236, 55, 208, 70, 91, 19, 135, 14, 74, 127, 127, 159, 172, 221, 180, 69, 124, 52, 250, 51, 207, 58, 77, 82, 153, 140, 142, 161, 211, 252, 103, 227, 159, 239, 232, 122, 124, 239, 188, 166, 51, 32, 153, 67, 134, 77, 20, 166, 62, 84, 64, 97, 221, 109, 167, 83, 128, 217, 211, 174, 35, 3, 8, 4, 187, 78, 14, 2, 40, 185, 212, 89, 130, 0, 176, 157, 37, 168, 47, 18, 33, 184, 181, 114, 43, 220, 181, 219, 189, 139, 91, 75, 65, 183, 99, 29, 172, 151, 214, 132, 78, 238, 65, 32, 209, 240, 149, 221, 137, 154, 25, 230, 35, 17, 225, 56, 212, 110, 97, 199, 38, 234, 224, 49, 114, 52, 195, 145, 27, 211, 180, 114, 187, 107, 166, 106, 25, 4, 37, 0, 68, 204, 25, 26, 49, 15, 82, 241, 58, 181, 36, 219, 55, 128, 50, 186, 5, 8, 192, 211, 15, 128, 52, 107, 13, 184, 3, 137, 71, 77, 233, 174, 44, 133, 116, 9, 224, 39, 148, 83, 206, 191, 240, 246, 32, 240, 247, 28, 216, 185, 83, 122, 178, 73, 201, 247, 247, 75, 46, 151, 147, 24, 52, 127, 181, 82, 86, 95, 152, 95, 250, 49, 40, 211, 86, 175, 141, 220, 128, 196, 44, 205, 92, 120, 2, 126, 94, 248, 108, 128, 249, 242, 144, 21, 68, 28, 120, 156, 96, 235, 121, 240, 18, 228, 52, 199, 73, 16, 78, 163, 83, 20, 132, 79, 36, 174, 50, 91, 222, 214, 225, 92, 1, 213, 242, 126, 29, 166, 62, 3, 172, 14, 78, 71, 38, 249, 88, 178, 208, 99, 177, 248, 214, 99, 195, 102, 36, 46, 158, 11, 207, 206, 89, 51, 238, 32, 88, 142, 228, 70, 33, 240, 41, 230, 156, 76, 190, 186, 13, 16, 94, 31, 51, 13, 216, 92, 43, 190, 146, 141, 29, 128, 188, 38, 137, 100, 106, 183, 22, 234, 202, 146, 72, 151, 0, 126, 66, 185, 227, 91, 223, 158, 75, 37, 211, 215, 30, 60, 180, 23, 237, 181, 161, 190, 122, 223, 202, 1, 25, 27, 29, 133, 214, 154, 131, 63, 11, 48, 152, 94, 50, 4, 133, 133, 106, 54, 126, 109, 72, 123, 202, 209, 192, 29, 33, 52, 225, 247, 146, 48, 244, 233, 64, 174, 211, 23, 14, 249, 224, 12, 124, 105, 96, 131, 143, 197, 198, 176, 30, 107, 249, 112, 13, 248, 1, 83, 16, 5, 2, 63, 27, 206, 207, 135, 181, 223, 28, 204, 142, 63, 235, 66, 88, 156, 61, 129, 24, 224, 169, 88, 16, 42, 6, 25, 120, 168, 88, 154, 33, 73, 155, 6, 209, 100, 128, 24, 135, 142, 8, 18, 245, 233, 166, 249, 104, 19, 229, 67, 248, 254, 10, 106, 20, 82, 130, 179, 22, 140, 1, 61, 45, 6, 104, 121, 20, 54, 68, 97, 200, 81, 175, 1, 171, 196, 122, 165, 80, 150, 169, 209, 97, 235, 90, 132, 226, 7, 9, 153, 154, 152, 66, 153, 48, 76, 245, 100, 187, 239, 0, 88, 66, 233, 18, 192, 79, 33, 81, 46, 125, 85, 25, 141, 118, 116, 223, 97, 233, 205, 228, 101, 207, 158, 17, 121, 236, 161, 199, 164, 4, 179, 181, 86, 133, 233, 140, 6, 78, 112, 44, 20, 54, 122, 27, 181, 162, 192, 2, 40, 72, 0, 26, 64, 6, 205, 58, 252, 123, 128, 73, 191, 19, 168, 223, 10, 4, 184, 234, 88, 86, 65, 10, 252, 80, 72, 49, 144, 168, 20, 152, 158, 127, 4, 169, 34, 206, 145, 2, 144, 130, 62, 130, 71, 0, 51, 116, 10, 247, 187, 96, 223, 4, 57, 130, 29, 49, 48, 251, 96, 64, 61, 8, 92, 234, 58, 235, 4, 209, 180, 235, 85, 210, 176, 176, 38, 122, 41, 92, 161, 175, 195, 96, 247, 97, 128, 173, 123, 233, 16, 154, 251, 166, 147, 144, 224, 167, 104, 62, 220, 0, 14, 83, 120, 30, 243, 144, 14, 87, 41, 129, 248, 220, 228, 36, 178, 226, 37, 164, 31, 96, 177, 174, 44, 141, 224, 234, 119, 229, 39, 149, 254, 193, 21, 183, 193, 100, 157, 56, 240, 216, 99, 210, 211, 151, 147, 58, 52, 249, 228, 216, 152, 76, 28, 56, 8, 37, 88, 69, 9, 104, 251, 38, 65, 141, 168, 182, 117, 243, 153, 48, 243, 118, 31, 128, 158, 62, 52, 210, 245, 237, 192, 10, 3, 130, 2, 235, 0, 67, 163, 193, 78, 49, 90, 11, 62, 234, 32, 16, 1, 248, 106, 10, 160, 55, 195, 128, 177, 66, 86, 100, 46, 35, 209, 28, 214, 139, 41, 145, 50, 66, 13, 101, 8, 214, 38, 129, 138, 160, 218, 216, 128, 213, 236, 223, 68, 85, 184, 206, 160, 68, 129, 178, 180, 36, 248, 209, 145, 42, 8, 165, 108, 62, 71, 166, 1, 113, 126, 159, 48, 84, 34, 96, 175, 60, 52, 61, 182, 161, 69, 207, 99, 229, 16, 39, 131, 126, 160, 64, 167, 0, 227, 204, 236, 238, 76, 231, 30, 173, 30, 150, 229, 185, 113, 68, 132, 215, 192, 128, 223, 76, 20, 194, 153, 35, 157, 36, 200, 105, 191, 137, 12, 231, 80, 4, 18, 215, 231, 0, 60, 41, 22, 102, 57, 138, 50, 59, 180, 106, 237, 12, 10, 119, 101, 137, 164, 75, 0, 63, 133, 124, 253, 230, 251, 14, 121, 65, 112, 215, 129, 93, 187, 37, 157, 240, 161, 192, 98, 146, 237, 237, 135, 6, 175, 163, 17, 183, 0, 12, 162, 4, 64, 80, 83, 152, 144, 96, 67, 71, 18, 218, 189, 211, 126, 92, 152, 89, 130, 54, 16, 75, 10, 150, 184, 180, 234, 0, 79, 13, 32, 2, 48, 249, 105, 48, 41, 2, 156, 4, 60, 199, 253, 103, 179, 18, 99, 152, 3, 17, 112, 110, 64, 25, 22, 0, 173, 128, 182, 5, 96, 8, 192, 79, 197, 100, 232, 52, 145, 205, 47, 20, 217, 248, 60, 145, 245, 207, 16, 89, 125, 190, 200, 192, 214, 152, 228, 86, 192, 181, 64, 153, 24, 203, 115, 59, 206, 33, 0, 216, 165, 4, 139, 162, 64, 98, 177, 228, 66, 34, 0, 49, 132, 252, 84, 25, 173, 4, 144, 128, 249, 148, 185, 57, 15, 190, 254, 92, 15, 220, 158, 15, 1, 175, 150, 1, 86, 152, 111, 192, 206, 243, 226, 57, 242, 188, 141, 197, 195, 184, 14, 3, 50, 224, 26, 241, 69, 43, 201, 108, 15, 178, 65, 18, 28, 58, 109, 53, 101, 98, 124, 12, 165, 163, 195, 111, 248, 249, 119, 204, 233, 14, 186, 178, 36, 210, 37, 128, 159, 66, 206, 202, 73, 152, 201, 102, 175, 157, 24, 29, 147, 26, 204, 254, 84, 50, 41, 249, 193, 21, 146, 202, 245, 216, 73, 63, 80, 133, 218, 235, 79, 173, 105, 124, 95, 18, 130, 134, 182, 80, 93, 42, 146, 240, 7, 80, 112, 248, 140, 224, 32, 96, 160, 84, 155, 21, 44, 1, 254, 24, 205, 253, 66, 18, 128, 7, 40, 103, 178, 18, 217, 73, 64, 4, 106, 140, 147, 128, 170, 8, 156, 0, 4, 32, 247, 174, 247, 228, 204, 55, 198, 229, 21, 31, 137, 203, 207, 125, 51, 46, 255, 237, 27, 113, 121, 227, 21, 113, 121, 219, 213, 113, 249, 165, 91, 60, 249, 229, 187, 60, 121, 203, 183, 227, 242, 194, 191, 142, 203, 179, 254, 32, 46, 131, 167, 226, 24, 0, 108, 117, 33, 232, 78, 148, 96, 105, 20, 1, 122, 236, 43, 68, 253, 17, 214, 149, 8, 156, 155, 129, 178, 236, 147, 160, 245, 64, 107, 160, 217, 96, 255, 134, 61, 53, 252, 83, 197, 190, 64, 204, 57, 42, 224, 219, 192, 39, 67, 160, 225, 217, 81, 17, 79, 31, 255, 133, 216, 114, 156, 4, 196, 145, 148, 114, 169, 44, 126, 194, 31, 254, 253, 95, 124, 107, 67, 11, 118, 101, 73, 164, 75, 0, 63, 165, 164, 51, 217, 107, 42, 245, 74, 99, 248, 192, 62, 125, 55, 160, 151, 202, 73, 34, 219, 43, 165, 217, 146, 120, 104, 220, 106, 30, 163, 105, 235, 68, 75, 168, 126, 196, 240, 163, 198, 67, 178, 193, 128, 33, 9, 13, 6, 64, 154, 71, 165, 10, 159, 159, 102, 121, 12, 4, 16, 17, 128, 22, 252, 50, 147, 131, 246, 207, 153, 117, 59, 3, 80, 106, 190, 36, 211, 190, 108, 122, 158, 47, 47, 248, 19, 79, 126, 230, 131, 113, 185, 232, 87, 226, 208, 252, 49, 25, 0, 192, 83, 125, 48, 191, 125, 3, 54, 202, 192, 182, 152, 156, 247, 14, 144, 192, 95, 122, 242, 202, 143, 5, 114, 217, 111, 99, 31, 32, 16, 117, 53, 44, 9, 72, 33, 5, 194, 129, 181, 193, 192, 117, 184, 3, 36, 129, 24, 72, 32, 214, 0, 240, 235, 145, 212, 97, 161, 180, 64, 6, 45, 88, 5, 156, 73, 220, 116, 231, 68, 209, 115, 65, 2, 207, 91, 79, 204, 36, 147, 24, 57, 244, 71, 81, 50, 224, 133, 112, 35, 26, 38, 81, 167, 86, 87, 203, 21, 169, 22, 42, 32, 131, 100, 119, 10, 240, 18, 75, 151, 0, 126, 74, 57, 239, 178, 231, 240, 27, 117, 59, 247, 60, 182, 67, 242, 89, 248, 230, 158, 39, 185, 190, 65, 253, 130, 48, 17, 173, 189, 251, 64, 180, 142, 156, 67, 163, 25, 237, 78, 13, 8, 252, 107, 123, 231, 63, 34, 159, 113, 10, 205, 114, 104, 99, 142, 32, 180, 160, 113, 213, 44, 135, 230, 87, 223, 31, 38, 63, 67, 145, 1, 235, 21, 0, 20, 166, 185, 192, 52, 231, 152, 249, 139, 255, 50, 144, 183, 95, 229, 203, 185, 111, 143, 75, 118, 229, 60, 216, 127, 148, 108, 120, 110, 76, 158, 247, 199, 158, 188, 236, 239, 177, 47, 235, 10, 68, 216, 167, 90, 2, 32, 1, 18, 77, 68, 75, 0, 36, 20, 150, 124, 169, 151, 98, 82, 133, 101, 210, 0, 1, 88, 183, 127, 158, 195, 44, 136, 137, 123, 61, 39, 184, 24, 202, 1, 0, 181, 146, 128, 21, 183, 206, 20, 18, 2, 191, 140, 212, 168, 212, 120, 37, 52, 45, 157, 74, 72, 113, 118, 70, 154, 205, 6, 227, 221, 33, 192, 37, 150, 46, 1, 252, 148, 114, 229, 127, 126, 174, 148, 76, 164, 110, 26, 217, 187, 79, 50, 73, 62, 9, 24, 232, 55, 3, 166, 199, 198, 248, 2, 75, 180, 110, 211, 232, 105, 11, 240, 50, 171, 117, 108, 177, 73, 60, 24, 13, 73, 180, 32, 17, 96, 209, 214, 79, 133, 8, 51, 59, 14, 141, 28, 163, 217, 77, 205, 11, 2, 32, 232, 99, 0, 97, 140, 218, 152, 164, 80, 69, 253, 208, 198, 169, 188, 47, 111, 252, 124, 32, 23, 254, 242, 79, 127, 27, 19, 217, 152, 92, 252, 238, 184, 188, 254, 83, 9, 51, 141, 25, 117, 179, 3, 80, 232, 10, 192, 245, 144, 57, 128, 31, 203, 102, 129, 159, 40, 143, 75, 147, 125, 156, 236, 11, 104, 25, 32, 171, 73, 175, 154, 222, 156, 79, 28, 193, 76, 247, 65, 93, 56, 111, 237, 252, 116, 160, 183, 65, 221, 6, 61, 103, 144, 4, 226, 74, 136, 150, 73, 178, 217, 180, 206, 2, 228, 144, 168, 23, 36, 39, 121, 140, 93, 89, 58, 233, 18, 192, 83, 144, 158, 124, 239, 15, 248, 210, 202, 242, 204, 180, 126, 74, 188, 111, 213, 42, 153, 154, 152, 80, 156, 115, 98, 11, 27, 187, 2, 155, 128, 80, 213, 8, 209, 117, 211, 232, 77, 62, 0, 3, 118, 48, 190, 53, 181, 63, 125, 114, 106, 99, 154, 221, 0, 125, 25, 26, 191, 12, 141, 204, 222, 254, 10, 204, 112, 152, 252, 210, 244, 37, 59, 232, 203, 219, 174, 244, 101, 219, 43, 159, 250, 45, 164, 139, 112, 214, 155, 61, 185, 224, 29, 9, 73, 38, 81, 191, 37, 31, 18, 128, 9, 129, 132, 101, 88, 28, 116, 3, 56, 50, 209, 196, 25, 226, 4, 218, 157, 151, 56, 118, 14, 91, 50, 222, 62, 101, 70, 44, 227, 153, 17, 0, 123, 61, 84, 244, 10, 105, 224, 200, 0, 223, 184, 204, 44, 246, 11, 244, 245, 230, 100, 98, 116, 20, 174, 5, 236, 166, 48, 218, 169, 197, 187, 178, 100, 210, 37, 128, 167, 32, 125, 3, 253, 119, 214, 235, 245, 194, 190, 93, 59, 165, 135, 47, 10, 205, 228, 164, 82, 169, 232, 104, 0, 159, 234, 115, 26, 142, 222, 46, 205, 127, 3, 138, 121, 49, 38, 50, 129, 129, 114, 4, 21, 39, 246, 52, 0, 22, 248, 227, 49, 62, 1, 72, 208, 81, 227, 51, 208, 37, 64, 186, 154, 234, 176, 18, 232, 239, 175, 60, 219, 86, 116, 148, 228, 217, 127, 224, 201, 218, 243, 141, 175, 31, 145, 4, 56, 215, 128, 35, 3, 176, 60, 98, 218, 223, 192, 253, 147, 0, 216, 81, 23, 215, 227, 103, 31, 71, 251, 28, 108, 80, 235, 0, 245, 25, 248, 207, 11, 193, 30, 35, 25, 32, 78, 43, 129, 75, 118, 2, 234, 40, 1, 126, 217, 108, 82, 50, 169, 64, 232, 86, 249, 126, 172, 156, 233, 201, 116, 223, 3, 184, 196, 210, 37, 128, 167, 32, 111, 253, 165, 119, 239, 110, 69, 173, 29, 59, 119, 238, 148, 124, 54, 197, 105, 171, 146, 206, 164, 165, 48, 61, 41, 113, 104, 53, 3, 254, 152, 78, 241, 109, 0, 44, 124, 13, 24, 197, 241, 0, 23, 106, 24, 40, 128, 160, 65, 155, 166, 243, 143, 224, 35, 224, 76, 128, 37, 0, 224, 145, 20, 180, 199, 30, 22, 194, 89, 111, 242, 100, 219, 171, 249, 184, 239, 98, 136, 61, 53, 201, 174, 136, 201, 217, 176, 4, 178, 131, 158, 233, 240, 171, 99, 255, 236, 107, 64, 136, 211, 34, 225, 49, 112, 82, 146, 29, 250, 215, 89, 131, 60, 126, 181, 6, 140, 59, 160, 179, 3, 112, 98, 58, 23, 64, 207, 144, 231, 139, 56, 202, 153, 243, 166, 53, 128, 115, 228, 249, 106, 89, 110, 107, 200, 98, 213, 138, 62, 125, 13, 216, 222, 199, 118, 137, 151, 72, 135, 113, 143, 239, 69, 239, 202, 82, 74, 151, 0, 158, 130, 124, 224, 247, 223, 211, 140, 121, 222, 237, 195, 251, 247, 65, 227, 199, 36, 153, 74, 72, 207, 224, 160, 140, 28, 60, 36, 249, 92, 6, 141, 30, 16, 176, 13, 220, 104, 73, 224, 23, 64, 224, 44, 65, 51, 73, 198, 104, 78, 18, 128, 27, 147, 167, 166, 167, 246, 143, 113, 120, 143, 90, 151, 100, 96, 193, 31, 3, 248, 217, 179, 127, 193, 127, 143, 75, 126, 205, 209, 5, 191, 147, 205, 47, 138, 203, 150, 23, 24, 162, 49, 195, 139, 36, 2, 186, 36, 36, 5, 90, 31, 56, 78, 30, 43, 143, 25, 129, 32, 55, 28, 192, 255, 16, 215, 59, 200, 255, 230, 196, 245, 58, 184, 52, 247, 108, 0, 133, 224, 231, 247, 1, 152, 198, 62, 148, 193, 193, 1, 57, 180, 239, 128, 20, 231, 10, 116, 27, 34, 181, 162, 186, 178, 164, 210, 37, 128, 167, 40, 153, 116, 254, 158, 194, 216, 180, 76, 143, 142, 72, 79, 54, 7, 2, 88, 33, 99, 35, 195, 146, 134, 47, 173, 26, 81, 85, 31, 10, 170, 131, 172, 202, 178, 13, 10, 2, 68, 13, 98, 250, 255, 4, 21, 252, 107, 106, 89, 53, 255, 169, 121, 73, 4, 88, 231, 48, 157, 209, 254, 113, 217, 246, 114, 15, 32, 93, 26, 240, 83, 250, 54, 198, 100, 243, 11, 1, 246, 246, 49, 113, 38, 32, 2, 173, 19, 61, 6, 28, 75, 104, 52, 184, 158, 26, 131, 2, 221, 86, 176, 88, 216, 185, 7, 49, 215, 194, 116, 22, 106, 81, 108, 195, 126, 17, 62, 35, 193, 141, 251, 123, 210, 226, 249, 129, 220, 123, 199, 237, 200, 98, 90, 51, 138, 199, 248, 210, 130, 174, 44, 165, 116, 9, 224, 41, 74, 58, 157, 125, 176, 5, 219, 254, 192, 206, 93, 146, 203, 103, 164, 167, 175, 79, 10, 133, 89, 0, 133, 253, 0, 52, 115, 13, 50, 168, 237, 26, 77, 3, 122, 66, 2, 186, 159, 24, 208, 96, 30, 248, 161, 118, 165, 198, 133, 182, 39, 240, 213, 18, 0, 248, 117, 236, 29, 233, 0, 99, 207, 58, 128, 19, 26, 122, 169, 101, 221, 165, 113, 201, 12, 117, 16, 147, 30, 155, 9, 58, 37, 88, 71, 1, 204, 177, 255, 56, 162, 19, 163, 80, 216, 144, 128, 209, 248, 4, 61, 118, 160, 215, 133, 15, 2, 246, 107, 231, 223, 136, 126, 114, 45, 157, 205, 72, 34, 149, 225, 5, 234, 18, 192, 18, 75, 151, 0, 158, 162, 172, 218, 180, 113, 103, 220, 243, 70, 247, 62, 186, 67, 103, 177, 37, 51, 25, 53, 107, 249, 100, 32, 191, 27, 72, 192, 179, 189, 115, 84, 144, 237, 217, 188, 48, 196, 129, 2, 96, 178, 126, 52, 159, 234, 83, 115, 219, 18, 128, 6, 154, 254, 22, 252, 36, 137, 21, 103, 198, 101, 235, 75, 117, 243, 37, 149, 236, 42, 145, 21, 167, 3, 240, 122, 108, 56, 46, 4, 211, 203, 79, 224, 27, 87, 134, 199, 174, 65, 233, 236, 200, 194, 114, 198, 252, 167, 152, 78, 63, 227, 243, 179, 167, 128, 23, 5, 101, 112, 113, 248, 44, 64, 38, 155, 150, 7, 239, 188, 67, 98, 241, 132, 60, 231, 53, 111, 146, 116, 174, 119, 90, 194, 102, 247, 57, 128, 37, 150, 46, 1, 60, 69, 185, 249, 166, 235, 199, 35, 95, 30, 57, 184, 127, 159, 212, 74, 37, 73, 247, 228, 160, 193, 242, 50, 61, 49, 37, 217, 76, 82, 65, 224, 240, 162, 214, 48, 199, 204, 241, 83, 224, 67, 136, 15, 206, 166, 211, 161, 53, 106, 89, 237, 104, 131, 246, 7, 17, 80, 243, 199, 152, 6, 240, 249, 201, 152, 108, 120, 78, 124, 193, 172, 190, 165, 18, 190, 63, 48, 191, 26, 17, 28, 180, 233, 232, 51, 4, 164, 100, 0, 211, 223, 16, 2, 172, 1, 158, 155, 158, 156, 110, 166, 231, 210, 25, 52, 143, 51, 255, 108, 208, 98, 106, 222, 139, 20, 11, 83, 168, 131, 239, 78, 48, 22, 17, 95, 143, 254, 232, 253, 15, 202, 139, 95, 255, 58, 57, 235, 236, 51, 164, 60, 59, 115, 255, 131, 119, 223, 83, 208, 194, 93, 89, 50, 193, 157, 233, 202, 83, 145, 44, 218, 122, 144, 72, 222, 57, 59, 61, 45, 227, 227, 35, 146, 78, 165, 36, 221, 63, 40, 195, 135, 216, 17, 152, 86, 45, 103, 144, 96, 68, 241, 66, 203, 150, 105, 45, 104, 66, 162, 136, 32, 227, 236, 63, 53, 179, 45, 232, 185, 84, 127, 27, 183, 8, 22, 64, 220, 139, 201, 89, 63, 183, 244, 224, 167, 144, 100, 114, 107, 248, 4, 31, 14, 159, 204, 165, 199, 135, 21, 61, 22, 146, 21, 10, 181, 45, 128, 199, 11, 79, 109, 65, 192, 233, 206, 103, 152, 168, 199, 57, 255, 136, 147, 8, 249, 69, 33, 62, 1, 73, 134, 220, 118, 214, 25, 178, 119, 199, 99, 82, 41, 151, 239, 54, 37, 187, 178, 148, 210, 37, 128, 163, 32, 233, 92, 230, 6, 106, 178, 145, 253, 7, 36, 157, 8, 100, 96, 112, 72, 38, 96, 1, 208, 5, 8, 244, 57, 119, 130, 192, 249, 190, 92, 65, 176, 157, 99, 250, 84, 158, 2, 11, 201, 77, 104, 90, 88, 2, 113, 248, 220, 74, 2, 234, 127, 35, 13, 101, 122, 54, 196, 164, 127, 243, 145, 1, 183, 20, 66, 18, 240, 104, 109, 88, 11, 192, 29, 139, 246, 7, 168, 21, 128, 66, 200, 51, 174, 141, 57, 183, 54, 224, 145, 245, 184, 165, 50, 33, 200, 67, 103, 72, 198, 36, 157, 206, 235, 163, 191, 116, 5, 204, 71, 68, 227, 50, 184, 98, 165, 236, 221, 189, 87, 30, 184, 235, 46, 73, 165, 211, 163, 40, 216, 149, 37, 22, 220, 201, 174, 60, 85, 89, 179, 105, 227, 77, 49, 47, 190, 103, 120, 215, 30, 253, 160, 69, 62, 159, 147, 114, 169, 34, 141, 90, 195, 246, 3, 16, 45, 14, 32, 4, 0, 87, 13, 16, 244, 17, 88, 5, 148, 35, 2, 27, 24, 71, 80, 13, 140, 176, 254, 25, 208, 148, 101, 187, 237, 50, 136, 151, 48, 36, 96, 192, 222, 65, 2, 60, 38, 198, 121, 40, 56, 46, 243, 158, 0, 152, 247, 60, 206, 14, 225, 57, 155, 143, 128, 24, 151, 71, 69, 139, 32, 206, 63, 214, 171, 219, 128, 0, 90, 145, 52, 155, 161, 156, 113, 222, 121, 114, 199, 15, 174, 151, 113, 88, 79, 137, 116, 186, 251, 32, 208, 50, 8, 238, 66, 87, 158, 170, 220, 123, 227, 205, 211, 137, 100, 234, 234, 177, 195, 195, 0, 73, 168, 19, 130, 188, 32, 144, 210, 108, 65, 178, 25, 184, 1, 109, 49, 36, 208, 22, 7, 26, 164, 181, 77, 109, 14, 177, 17, 96, 10, 50, 172, 219, 50, 253, 155, 9, 50, 141, 46, 139, 120, 240, 62, 220, 144, 190, 35, 33, 37, 3, 27, 111, 191, 58, 12, 98, 141, 153, 14, 113, 7, 202, 50, 136, 107, 32, 33, 144, 8, 81, 7, 203, 199, 161, 253, 217, 253, 175, 219, 70, 82, 169, 214, 100, 237, 198, 13, 50, 55, 62, 38, 213, 82, 41, 170, 87, 42, 221, 14, 192, 101, 144, 46, 1, 28, 37, 73, 101, 179, 87, 77, 205, 77, 72, 161, 48, 13, 237, 153, 196, 122, 175, 28, 6, 33, 228, 114, 105, 243, 8, 44, 64, 224, 70, 4, 212, 10, 96, 128, 240, 191, 155, 68, 99, 128, 198, 4, 99, 90, 115, 221, 148, 130, 155, 49, 40, 18, 116, 114, 201, 18, 75, 173, 32, 176, 56, 236, 138, 5, 189, 51, 251, 237, 161, 91, 177, 121, 88, 114, 132, 159, 223, 11, 112, 22, 143, 233, 252, 99, 30, 226, 72, 167, 165, 16, 139, 5, 154, 206, 111, 38, 220, 123, 243, 245, 178, 251, 129, 123, 36, 136, 71, 82, 174, 84, 165, 119, 176, 71, 242, 125, 61, 82, 175, 243, 113, 67, 123, 81, 186, 178, 164, 210, 37, 128, 163, 36, 125, 67, 43, 127, 216, 170, 55, 102, 71, 15, 171, 249, 42, 153, 124, 175, 62, 25, 152, 74, 248, 218, 216, 9, 16, 157, 11, 79, 60, 168, 160, 125, 83, 197, 118, 180, 115, 243, 202, 48, 183, 36, 156, 12, 210, 248, 95, 191, 240, 67, 141, 185, 12, 194, 207, 137, 181, 221, 13, 197, 55, 226, 12, 124, 3, 177, 121, 174, 17, 98, 243, 245, 40, 241, 99, 207, 160, 154, 12, 29, 162, 76, 193, 10, 168, 253, 237, 42, 83, 112, 17, 106, 149, 138, 28, 218, 185, 67, 238, 189, 233, 122, 25, 57, 176, 15, 53, 196, 36, 157, 202, 72, 239, 192, 128, 180, 154, 205, 120, 72, 223, 162, 43, 75, 46, 93, 2, 56, 74, 114, 249, 75, 94, 127, 48, 22, 143, 239, 24, 221, 191, 95, 50, 169, 132, 36, 243, 89, 153, 158, 157, 211, 231, 0, 18, 169, 160, 13, 23, 10, 227, 58, 103, 158, 64, 39, 1, 180, 53, 38, 96, 192, 201, 111, 76, 119, 160, 211, 210, 208, 144, 147, 92, 46, 143, 148, 198, 69, 102, 247, 35, 162, 199, 128, 227, 33, 240, 61, 4, 125, 133, 56, 142, 141, 175, 8, 183, 196, 192, 227, 215, 47, 4, 233, 121, 96, 27, 69, 186, 61, 159, 118, 136, 155, 30, 127, 27, 72, 0, 165, 185, 25, 217, 122, 214, 57, 178, 113, 219, 233, 82, 152, 153, 33, 77, 104, 102, 50, 149, 98, 129, 102, 42, 207, 207, 31, 117, 101, 169, 165, 75, 0, 71, 73, 254, 237, 159, 254, 172, 30, 247, 131, 135, 14, 238, 217, 45, 30, 224, 237, 131, 4, 106, 141, 186, 148, 74, 37, 201, 100, 82, 0, 11, 154, 56, 91, 57, 0, 64, 72, 48, 232, 147, 115, 140, 57, 192, 19, 104, 36, 0, 190, 167, 159, 241, 246, 71, 62, 68, 63, 243, 181, 92, 66, 211, 127, 223, 77, 48, 229, 29, 240, 61, 196, 25, 124, 243, 129, 144, 40, 222, 192, 177, 33, 238, 142, 91, 207, 198, 158, 159, 158, 39, 77, 127, 214, 100, 142, 93, 53, 191, 50, 134, 89, 225, 148, 224, 124, 95, 159, 172, 219, 186, 77, 78, 217, 126, 186, 172, 222, 180, 69, 248, 105, 48, 157, 26, 172, 15, 76, 197, 42, 189, 43, 87, 21, 185, 109, 87, 150, 86, 186, 4, 112, 20, 37, 149, 206, 220, 53, 59, 61, 35, 97, 163, 33, 137, 32, 1, 87, 32, 43, 51, 211, 115, 146, 76, 6, 157, 120, 88, 0, 6, 167, 73, 219, 90, 86, 53, 45, 9, 128, 105, 22, 96, 8, 179, 123, 221, 198, 75, 43, 97, 51, 146, 241, 135, 66, 243, 137, 113, 61, 14, 0, 147, 159, 2, 15, 248, 85, 224, 134, 8, 227, 246, 51, 97, 40, 173, 199, 175, 104, 119, 190, 141, 90, 0, 60, 86, 174, 243, 228, 230, 143, 91, 31, 137, 230, 137, 131, 0, 124, 47, 46, 233, 108, 78, 6, 86, 173, 81, 151, 137, 233, 124, 187, 242, 220, 220, 28, 248, 35, 94, 237, 235, 31, 226, 171, 71, 186, 178, 196, 210, 37, 128, 163, 40, 217, 124, 207, 99, 229, 82, 41, 154, 156, 24, 147, 108, 34, 41, 249, 92, 78, 198, 70, 198, 37, 25, 164, 244, 101, 23, 243, 211, 98, 13, 64, 232, 211, 43, 108, 8, 116, 18, 0, 65, 5, 19, 155, 31, 249, 84, 141, 235, 136, 0, 4, 48, 6, 80, 46, 151, 220, 247, 239, 220, 175, 57, 30, 126, 10, 156, 192, 231, 23, 131, 76, 128, 175, 31, 135, 175, 207, 79, 152, 243, 185, 96, 246, 101, 24, 54, 67, 160, 216, 165, 37, 2, 46, 20, 247, 228, 8, 158, 11, 211, 208, 236, 248, 238, 228, 86, 204, 195, 18, 113, 142, 40, 176, 8, 167, 62, 212, 177, 143, 48, 44, 120, 9, 175, 59, 11, 112, 25, 164, 75, 0, 71, 81, 130, 132, 191, 51, 106, 69, 197, 18, 124, 255, 20, 92, 0, 126, 226, 170, 48, 51, 171, 224, 15, 124, 31, 109, 159, 230, 47, 0, 163, 224, 0, 184, 184, 174, 0, 34, 224, 8, 44, 106, 86, 128, 205, 18, 128, 18, 1, 192, 70, 43, 160, 56, 30, 201, 232, 125, 68, 210, 210, 202, 200, 221, 145, 236, 252, 62, 1, 14, 160, 242, 115, 97, 246, 219, 129, 106, 1, 112, 169, 22, 0, 210, 148, 4, 44, 248, 173, 246, 215, 199, 155, 209, 164, 120, 148, 10, 124, 100, 211, 205, 161, 240, 63, 161, 207, 103, 1, 248, 156, 132, 62, 20, 4, 115, 95, 39, 71, 225, 143, 111, 83, 166, 11, 192, 15, 171, 120, 190, 95, 218, 114, 218, 121, 110, 12, 162, 43, 75, 40, 93, 2, 56, 138, 178, 245, 204, 243, 39, 160, 211, 199, 199, 134, 71, 196, 15, 124, 137, 39, 18, 210, 104, 212, 164, 92, 41, 73, 2, 110, 0, 129, 66, 0, 212, 155, 13, 29, 242, 87, 224, 192, 10, 224, 231, 197, 72, 4, 33, 9, 128, 223, 227, 83, 141, 203, 56, 72, 193, 18, 65, 179, 26, 201, 157, 31, 39, 224, 150, 70, 220, 4, 165, 31, 254, 107, 168, 154, 218, 129, 92, 253, 126, 5, 190, 137, 147, 144, 216, 71, 225, 58, 2, 149, 188, 72, 102, 14, 226, 196, 51, 193, 111, 129, 175, 130, 168, 174, 162, 28, 191, 128, 4, 75, 73, 223, 4, 164, 219, 50, 25, 63, 78, 58, 98, 223, 97, 11, 110, 0, 92, 128, 198, 169, 103, 158, 185, 116, 39, 219, 149, 182, 116, 9, 224, 40, 202, 181, 95, 249, 175, 89, 128, 250, 80, 113, 110, 26, 26, 205, 67, 163, 143, 163, 49, 11, 8, 160, 172, 4, 192, 222, 111, 90, 0, 238, 131, 154, 122, 245, 145, 70, 112, 132, 48, 253, 67, 152, 220, 45, 213, 182, 53, 13, 97, 96, 62, 210, 233, 44, 130, 251, 191, 208, 148, 67, 183, 1, 46, 106, 83, 31, 93, 225, 177, 61, 252, 229, 72, 118, 124, 199, 124, 223, 207, 153, 255, 33, 65, 239, 44, 0, 37, 1, 18, 21, 9, 139, 176, 213, 13, 177, 36, 194, 21, 223, 134, 0, 184, 174, 73, 102, 169, 130, 116, 106, 249, 100, 50, 137, 147, 165, 225, 207, 199, 128, 77, 243, 35, 15, 106, 125, 56, 175, 102, 163, 65, 114, 40, 253, 233, 47, 191, 13, 76, 211, 149, 165, 150, 46, 1, 28, 93, 137, 98, 137, 248, 161, 194, 244, 52, 98, 212, 164, 124, 109, 87, 66, 170, 229, 42, 8, 192, 215, 2, 4, 154, 122, 254, 0, 4, 123, 188, 57, 85, 214, 248, 219, 86, 219, 2, 240, 173, 4, 128, 159, 4, 9, 96, 169, 129, 157, 111, 32, 128, 122, 41, 148, 107, 254, 184, 165, 195, 116, 71, 91, 230, 14, 69, 114, 251, 135, 91, 234, 106, 104, 71, 100, 39, 240, 25, 18, 136, 35, 141, 95, 16, 166, 101, 16, 211, 15, 121, 226, 71, 22, 96, 208, 133, 69, 187, 3, 189, 91, 118, 8, 193, 110, 72, 3, 4, 192, 247, 3, 178, 31, 4, 68, 208, 108, 240, 245, 233, 230, 131, 169, 72, 44, 161, 148, 173, 181, 43, 75, 41, 93, 2, 56, 202, 18, 107, 69, 143, 22, 11, 5, 73, 4, 41, 128, 26, 46, 116, 38, 45, 197, 98, 73, 65, 239, 121, 104, 240, 11, 80, 161, 16, 130, 246, 3, 248, 181, 183, 157, 128, 175, 130, 0, 106, 210, 74, 33, 164, 249, 141, 126, 243, 157, 254, 8, 214, 0, 1, 184, 231, 186, 134, 220, 244, 215, 161, 84, 166, 158, 58, 62, 156, 217, 207, 137, 63, 55, 252, 69, 40, 251, 110, 52, 238, 70, 27, 252, 150, 136, 66, 46, 65, 66, 252, 92, 120, 200, 225, 63, 252, 144, 219, 254, 209, 157, 81, 123, 93, 193, 205, 200, 34, 225, 110, 108, 158, 238, 146, 175, 250, 138, 121, 208, 244, 190, 36, 252, 132, 62, 63, 145, 242, 51, 170, 253, 107, 181, 42, 9, 161, 219, 1, 184, 76, 210, 37, 128, 163, 44, 189, 131, 3, 19, 133, 185, 89, 169, 20, 139, 104, 224, 113, 73, 165, 178, 18, 215, 137, 245, 161, 174, 91, 204, 65, 16, 33, 112, 212, 255, 39, 136, 140, 214, 85, 179, 63, 85, 1, 248, 75, 54, 84, 176, 94, 5, 16, 145, 78, 77, 12, 128, 222, 249, 137, 134, 124, 235, 215, 90, 50, 246, 96, 187, 178, 159, 74, 104, 141, 84, 103, 35, 185, 254, 207, 67, 185, 247, 179, 166, 191, 65, 193, 159, 192, 126, 0, 252, 22, 200, 39, 36, 17, 145, 148, 232, 146, 40, 9, 232, 167, 78, 20, 248, 45, 156, 204, 143, 58, 2, 235, 32, 104, 204, 132, 133, 226, 197, 61, 52, 194, 184, 186, 7, 234, 122, 192, 13, 136, 194, 176, 251, 61, 192, 101, 146, 46, 1, 28, 109, 9, 229, 80, 179, 222, 144, 102, 173, 34, 233, 36, 135, 255, 60, 64, 128, 67, 93, 17, 220, 1, 78, 9, 166, 174, 132, 123, 160, 182, 48, 132, 140, 192, 30, 119, 16, 128, 78, 178, 1, 200, 169, 117, 9, 252, 86, 182, 140, 0, 18, 192, 50, 116, 68, 0, 109, 220, 108, 53, 229, 161, 47, 55, 229, 27, 239, 106, 201, 67, 95, 130, 223, 92, 195, 246, 243, 204, 242, 99, 9, 203, 143, 61, 16, 201, 141, 127, 101, 8, 165, 89, 199, 190, 9, 112, 171, 245, 105, 125, 112, 159, 45, 144, 81, 152, 196, 254, 19, 136, 195, 66, 105, 209, 85, 129, 203, 226, 38, 40, 233, 110, 59, 195, 34, 140, 155, 126, 0, 100, 176, 96, 251, 24, 177, 164, 255, 15, 243, 159, 207, 65, 240, 227, 170, 125, 189, 25, 41, 206, 206, 74, 173, 90, 65, 86, 215, 2, 88, 46, 233, 18, 192, 81, 22, 180, 231, 3, 81, 43, 106, 70, 205, 154, 100, 82, 102, 130, 139, 233, 249, 111, 234, 228, 23, 67, 0, 4, 172, 89, 18, 31, 28, 230, 211, 78, 55, 128, 171, 165, 32, 132, 182, 5, 0, 91, 25, 16, 64, 174, 104, 73, 160, 164, 105, 142, 4, 72, 20, 135, 238, 106, 200, 151, 223, 214, 144, 79, 62, 187, 41, 143, 126, 53, 146, 226, 104, 164, 26, 253, 137, 132, 160, 111, 84, 34, 41, 12, 71, 242, 200, 151, 35, 249, 222, 239, 55, 228, 182, 143, 192, 236, 174, 192, 242, 128, 214, 15, 83, 0, 56, 234, 215, 253, 102, 12, 241, 180, 210, 101, 105, 166, 107, 210, 164, 85, 0, 183, 64, 71, 37, 148, 176, 120, 236, 79, 188, 175, 78, 233, 124, 84, 88, 99, 74, 6, 212, 246, 32, 19, 252, 248, 45, 197, 164, 239, 233, 231, 212, 216, 15, 0, 2, 232, 14, 1, 46, 147, 116, 9, 224, 40, 75, 118, 112, 160, 8, 160, 85, 166, 167, 38, 245, 85, 215, 4, 157, 2, 15, 26, 150, 67, 95, 71, 126, 160, 199, 128, 201, 117, 190, 169, 22, 166, 230, 37, 8, 243, 12, 5, 75, 4, 197, 5, 150, 0, 73, 160, 5, 111, 124, 248, 190, 150, 252, 215, 155, 27, 242, 169, 231, 54, 212, 151, 191, 251, 223, 66, 185, 255, 115, 161, 236, 188, 50, 148, 253, 55, 132, 114, 248, 78, 152, 37, 183, 153, 244, 107, 222, 223, 146, 47, 191, 189, 33, 95, 124, 107, 77, 118, 95, 215, 16, 254, 212, 207, 183, 192, 15, 115, 32, 26, 4, 221, 31, 227, 32, 1, 193, 177, 8, 73, 9, 22, 10, 71, 42, 116, 134, 160, 21, 125, 174, 223, 201, 145, 78, 173, 45, 204, 68, 224, 194, 18, 128, 138, 114, 8, 251, 71, 104, 29, 25, 66, 137, 251, 94, 119, 22, 224, 50, 73, 151, 0, 142, 178, 172, 92, 185, 118, 14, 234, 177, 88, 46, 20, 244, 115, 97, 250, 129, 208, 22, 95, 121, 85, 215, 215, 122, 73, 204, 215, 239, 2, 24, 36, 112, 129, 21, 222, 5, 206, 146, 163, 21, 64, 144, 5, 117, 105, 4, 85, 104, 94, 211, 23, 208, 204, 22, 164, 153, 155, 67, 0, 17, 208, 26, 32, 80, 73, 4, 0, 174, 142, 18, 176, 111, 0, 196, 49, 185, 191, 41, 55, 127, 176, 46, 95, 255, 213, 154, 92, 249, 123, 53, 249, 238, 31, 212, 229, 107, 239, 174, 203, 23, 222, 80, 147, 127, 255, 153, 154, 124, 227, 215, 107, 114, 235, 71, 107, 178, 247, 22, 154, 249, 212, 246, 198, 210, 136, 50, 216, 143, 3, 61, 200, 166, 217, 51, 167, 203, 48, 99, 9, 7, 101, 57, 25, 72, 231, 5, 128, 114, 120, 78, 14, 172, 186, 14, 223, 221, 60, 210, 140, 208, 145, 167, 130, 168, 177, 0, 152, 6, 107, 72, 113, 207, 117, 243, 165, 96, 214, 197, 57, 5, 126, 42, 144, 58, 45, 0, 212, 231, 7, 221, 89, 128, 203, 37, 93, 2, 56, 202, 178, 114, 195, 150, 57, 52, 238, 217, 74, 169, 162, 47, 5, 101, 191, 127, 19, 166, 46, 95, 120, 65, 75, 64, 71, 2, 156, 255, 223, 33, 234, 6, 104, 224, 56, 59, 59, 227, 172, 102, 134, 9, 174, 160, 119, 224, 204, 147, 8, 104, 13, 208, 60, 7, 56, 45, 17, 104, 160, 150, 214, 101, 93, 138, 197, 186, 140, 237, 173, 201, 196, 129, 154, 204, 78, 215, 164, 218, 50, 121, 58, 186, 160, 157, 140, 8, 36, 18, 88, 21, 205, 188, 169, 187, 213, 139, 186, 17, 20, 252, 118, 31, 180, 68, 204, 80, 100, 211, 184, 41, 214, 236, 167, 91, 211, 41, 92, 109, 79, 79, 224, 210, 242, 1, 129, 206, 211, 117, 197, 117, 97, 79, 159, 215, 195, 101, 240, 195, 42, 211, 147, 19, 90, 0, 86, 210, 148, 38, 118, 101, 201, 165, 75, 0, 71, 89, 126, 254, 183, 255, 39, 236, 229, 104, 118, 110, 174, 160, 147, 94, 180, 119, 31, 90, 174, 217, 132, 166, 108, 66, 211, 169, 11, 96, 64, 161, 141, 95, 1, 96, 64, 64, 64, 112, 146, 141, 206, 182, 163, 43, 224, 195, 247, 78, 210, 18, 112, 86, 0, 193, 10, 160, 230, 103, 205, 178, 135, 174, 129, 33, 135, 208, 90, 6, 244, 217, 233, 58, 132, 4, 183, 11, 36, 9, 230, 177, 31, 1, 128, 215, 242, 176, 38, 66, 108, 223, 100, 232, 69, 125, 125, 8, 88, 42, 9, 144, 104, 114, 150, 96, 232, 142, 248, 8, 58, 75, 17, 136, 166, 165, 66, 162, 162, 230, 198, 241, 186, 163, 167, 146, 231, 169, 80, 195, 235, 105, 217, 31, 59, 63, 201, 4, 250, 249, 112, 252, 153, 211, 102, 154, 174, 106, 224, 72, 128, 239, 251, 82, 41, 193, 221, 64, 66, 171, 86, 159, 214, 2, 93, 89, 114, 233, 18, 192, 81, 150, 183, 94, 120, 106, 232, 249, 254, 44, 63, 111, 229, 7, 9, 125, 251, 45, 199, 254, 9, 140, 38, 8, 128, 154, 142, 173, 156, 128, 208, 15, 101, 208, 100, 86, 64, 80, 187, 178, 6, 196, 9, 50, 118, 182, 41, 9, 192, 84, 79, 128, 4, 160, 137, 219, 4, 64, 160, 246, 76, 3, 188, 51, 6, 188, 48, 217, 213, 108, 71, 8, 25, 39, 128, 105, 41, 216, 244, 206, 252, 86, 15, 65, 206, 237, 16, 250, 16, 250, 103, 164, 213, 143, 52, 16, 128, 230, 43, 57, 128, 76, 216, 7, 65, 107, 130, 163, 18, 4, 63, 31, 254, 1, 153, 197, 17, 218, 26, 92, 135, 237, 12, 193, 41, 25, 32, 16, 239, 4, 61, 135, 8, 141, 149, 192, 115, 196, 146, 81, 72, 76, 211, 204, 42, 203, 219, 170, 212, 42, 42, 23, 138, 154, 225, 197, 60, 78, 4, 234, 202, 50, 72, 151, 0, 150, 64, 98, 158, 55, 81, 2, 1, 36, 130, 64, 210, 180, 2, 44, 72, 234, 252, 106, 48, 92, 0, 243, 197, 32, 148, 195, 213, 87, 32, 208, 86, 38, 17, 112, 73, 68, 32, 232, 92, 123, 117, 5, 64, 2, 212, 194, 236, 248, 227, 188, 0, 181, 4, 168, 173, 9, 224, 105, 105, 48, 244, 155, 101, 147, 75, 4, 46, 59, 67, 75, 151, 83, 40, 63, 37, 141, 129, 25, 148, 97, 64, 156, 235, 32, 129, 134, 18, 10, 77, 127, 154, 253, 216, 135, 78, 64, 130, 203, 2, 55, 68, 159, 71, 104, 155, 254, 122, 212, 246, 16, 219, 44, 128, 195, 6, 57, 56, 50, 96, 207, 62, 9, 1, 113, 231, 18, 16, 243, 140, 43, 15, 216, 52, 21, 128, 94, 103, 70, 162, 42, 181, 136, 52, 41, 214, 74, 228, 242, 93, 2, 88, 38, 233, 18, 192, 18, 8, 0, 48, 94, 173, 148, 57, 204, 45, 233, 116, 26, 235, 128, 56, 0, 81, 231, 88, 123, 100, 250, 0, 8, 8, 66, 222, 140, 10, 240, 54, 48, 48, 142, 12, 11, 54, 247, 6, 30, 9, 16, 79, 130, 8, 232, 195, 115, 88, 14, 230, 121, 29, 96, 109, 208, 92, 87, 45, 14, 224, 15, 76, 73, 29, 160, 214, 192, 56, 130, 130, 188, 127, 18, 105, 12, 211, 72, 35, 240, 73, 6, 70, 243, 55, 72, 34, 121, 16, 4, 172, 5, 2, 191, 201, 254, 134, 0, 154, 223, 7, 217, 112, 86, 34, 31, 76, 34, 9, 241, 120, 16, 220, 121, 40, 136, 137, 88, 18, 25, 207, 5, 199, 173, 79, 245, 97, 157, 226, 172, 1, 67, 106, 70, 184, 9, 59, 63, 57, 245, 89, 39, 7, 33, 207, 183, 207, 2, 144, 16, 105, 254, 20, 103, 57, 255, 39, 86, 79, 230, 250, 187, 111, 3, 90, 38, 49, 119, 160, 43, 71, 85, 224, 2, 204, 212, 107, 53, 248, 180, 37, 201, 229, 115, 22, 52, 244, 135, 217, 139, 78, 141, 7, 224, 16, 13, 138, 15, 166, 16, 30, 157, 130, 117, 213, 136, 36, 1, 108, 11, 243, 187, 21, 111, 72, 211, 99, 7, 94, 213, 116, 224, 193, 151, 111, 192, 26, 104, 228, 103, 165, 1, 87, 64, 3, 181, 185, 181, 4, 156, 121, 111, 180, 61, 3, 72, 162, 23, 233, 61, 176, 4, 52, 88, 141, 79, 115, 159, 125, 12, 52, 249, 1, 126, 78, 69, 142, 60, 190, 128, 144, 192, 231, 1, 242, 88, 120, 124, 122, 64, 70, 28, 176, 59, 84, 183, 158, 1, 206, 115, 129, 216, 211, 114, 197, 52, 88, 146, 208, 115, 67, 61, 116, 129, 216, 7, 64, 225, 20, 106, 72, 37, 153, 78, 119, 223, 6, 180, 76, 210, 37, 128, 37, 16, 63, 17, 76, 214, 235, 85, 125, 238, 125, 160, 175, 79, 135, 187, 72, 2, 212, 146, 205, 70, 13, 235, 22, 25, 88, 16, 75, 128, 56, 21, 160, 174, 171, 105, 205, 116, 254, 227, 221, 97, 128, 21, 16, 99, 7, 28, 159, 206, 131, 86, 230, 179, 2, 97, 146, 29, 123, 0, 110, 166, 104, 58, 8, 181, 115, 144, 160, 182, 254, 63, 59, 10, 233, 42, 104, 124, 14, 126, 189, 203, 67, 192, 54, 173, 20, 2, 59, 12, 9, 252, 4, 128, 207, 103, 13, 248, 192, 15, 167, 3, 147, 116, 16, 20, 248, 174, 133, 240, 248, 240, 207, 104, 118, 4, 30, 159, 21, 102, 169, 33, 99, 133, 81, 67, 106, 122, 22, 90, 92, 55, 129, 40, 1, 240, 205, 31, 118, 8, 80, 79, 19, 204, 64, 215, 161, 92, 130, 213, 20, 143, 149, 188, 176, 57, 107, 74, 119, 101, 169, 165, 75, 0, 75, 32, 48, 235, 71, 56, 246, 95, 43, 195, 2, 200, 100, 180, 135, 155, 66, 56, 132, 173, 6, 46, 58, 193, 133, 4, 123, 245, 157, 66, 101, 154, 49, 159, 13, 96, 104, 41, 152, 8, 81, 130, 5, 137, 192, 7, 56, 249, 80, 14, 59, 7, 57, 63, 63, 85, 151, 48, 141, 37, 173, 2, 154, 240, 118, 36, 64, 93, 5, 187, 84, 160, 35, 93, 199, 251, 105, 65, 164, 56, 186, 0, 192, 219, 78, 62, 99, 234, 51, 96, 223, 84, 198, 68, 115, 155, 164, 184, 212, 3, 195, 159, 73, 107, 119, 86, 186, 3, 7, 128, 233, 214, 44, 16, 107, 222, 107, 49, 23, 176, 170, 253, 2, 44, 11, 173, 175, 111, 73, 198, 137, 241, 163, 170, 204, 173, 85, 107, 76, 171, 228, 147, 241, 26, 55, 237, 202, 210, 75, 151, 0, 150, 64, 2, 241, 198, 98, 97, 44, 226, 251, 1, 83, 233, 164, 157, 229, 6, 1, 10, 154, 48, 253, 169, 241, 212, 29, 192, 186, 27, 9, 80, 55, 129, 16, 233, 192, 81, 8, 18, 209, 73, 54, 52, 173, 1, 72, 253, 46, 7, 251, 4, 168, 161, 219, 207, 234, 131, 12, 52, 128, 8, 116, 82, 144, 13, 140, 119, 172, 115, 42, 175, 155, 61, 168, 160, 231, 182, 28, 105, 96, 232, 212, 248, 220, 191, 3, 127, 251, 96, 244, 200, 116, 57, 159, 132, 242, 26, 218, 57, 86, 88, 192, 174, 81, 179, 35, 95, 3, 215, 59, 54, 103, 156, 240, 103, 158, 186, 68, 88, 242, 73, 192, 40, 230, 149, 47, 120, 209, 171, 155, 182, 84, 87, 150, 88, 186, 4, 176, 4, 146, 234, 27, 152, 3, 168, 171, 83, 19, 83, 146, 129, 246, 167, 134, 83, 156, 40, 240, 57, 119, 142, 235, 6, 36, 212, 156, 230, 195, 33, 4, 124, 8, 179, 152, 207, 218, 17, 248, 88, 34, 48, 195, 129, 136, 206, 130, 6, 206, 40, 212, 64, 20, 113, 29, 229, 96, 25, 48, 68, 1, 136, 69, 137, 1, 75, 141, 19, 236, 230, 125, 2, 156, 96, 196, 165, 113, 39, 128, 177, 24, 3, 234, 38, 63, 41, 232, 145, 175, 90, 221, 192, 148, 176, 53, 71, 9, 97, 146, 67, 47, 207, 3, 102, 0, 14, 215, 156, 23, 147, 204, 66, 243, 180, 87, 67, 143, 153, 235, 8, 142, 44, 236, 42, 133, 67, 131, 124, 31, 2, 63, 126, 226, 227, 250, 180, 154, 176, 152, 244, 117, 96, 222, 236, 199, 255, 252, 15, 113, 192, 93, 89, 14, 233, 18, 192, 18, 72, 190, 175, 119, 18, 192, 46, 112, 92, 155, 207, 186, 243, 251, 247, 10, 96, 5, 5, 59, 3, 57, 49, 198, 13, 129, 1, 18, 109, 244, 64, 88, 140, 75, 164, 17, 50, 10, 162, 197, 162, 154, 26, 48, 35, 111, 80, 123, 19, 196, 157, 129, 233, 139, 211, 108, 80, 192, 219, 201, 60, 237, 208, 33, 92, 51, 100, 131, 44, 27, 140, 201, 255, 196, 226, 178, 205, 182, 6, 232, 70, 144, 226, 170, 215, 14, 80, 91, 6, 36, 200, 64, 170, 99, 119, 64, 42, 29, 40, 248, 171, 149, 10, 251, 0, 56, 9, 168, 179, 146, 174, 44, 161, 116, 9, 96, 9, 36, 29, 36, 102, 0, 162, 194, 244, 244, 148, 2, 140, 175, 5, 95, 0, 12, 160, 128, 192, 119, 64, 163, 0, 18, 138, 36, 93, 90, 33, 55, 112, 93, 73, 162, 147, 9, 58, 162, 42, 157, 96, 94, 20, 20, 248, 186, 1, 246, 207, 89, 137, 110, 221, 162, 154, 57, 143, 23, 155, 202, 34, 166, 152, 17, 30, 7, 101, 17, 43, 233, 26, 207, 199, 197, 173, 152, 243, 67, 224, 118, 8, 157, 85, 81, 56, 252, 23, 183, 4, 89, 173, 86, 148, 4, 96, 245, 44, 193, 251, 142, 186, 242, 68, 210, 37, 128, 37, 144, 87, 188, 249, 93, 85, 180, 238, 105, 190, 30, 188, 217, 106, 72, 50, 145, 84, 204, 168, 47, 15, 113, 75, 2, 132, 65, 205, 125, 16, 132, 235, 23, 112, 72, 178, 184, 193, 63, 45, 190, 80, 108, 25, 251, 79, 165, 147, 80, 58, 197, 236, 7, 251, 68, 112, 245, 179, 36, 77, 245, 197, 162, 187, 235, 216, 167, 41, 103, 130, 138, 102, 218, 37, 2, 211, 217, 177, 199, 160, 85, 51, 32, 205, 236, 83, 75, 162, 28, 154, 89, 220, 55, 117, 235, 58, 255, 113, 243, 152, 78, 148, 202, 164, 82, 250, 176, 20, 223, 8, 228, 7, 65, 247, 65, 160, 101, 148, 46, 1, 44, 129, 252, 197, 111, 191, 163, 1, 219, 118, 164, 82, 44, 43, 16, 130, 68, 66, 193, 226, 196, 117, 250, 53, 208, 224, 29, 25, 56, 49, 192, 177, 40, 130, 56, 75, 65, 21, 246, 124, 21, 42, 156, 65, 200, 25, 119, 68, 26, 235, 113, 193, 89, 27, 46, 175, 99, 215, 144, 69, 149, 56, 81, 139, 129, 75, 179, 170, 32, 54, 81, 68, 160, 189, 221, 39, 193, 117, 245, 241, 117, 152, 99, 118, 129, 41, 44, 195, 248, 252, 249, 49, 89, 3, 203, 216, 165, 231, 197, 36, 1, 2, 224, 11, 73, 216, 233, 153, 8, 146, 221, 183, 1, 45, 163, 116, 9, 96, 137, 36, 72, 6, 135, 42, 197, 146, 154, 181, 233, 84, 192, 214, 142, 84, 59, 3, 144, 192, 132, 56, 32, 208, 56, 238, 52, 144, 1, 119, 189, 51, 78, 171, 154, 68, 163, 197, 21, 244, 240, 158, 25, 88, 210, 100, 89, 205, 174, 113, 146, 129, 37, 4, 198, 59, 234, 224, 82, 203, 1, 192, 58, 3, 81, 119, 201, 26, 185, 47, 126, 191, 143, 157, 123, 102, 191, 182, 35, 193, 110, 143, 186, 248, 179, 117, 235, 91, 137, 77, 101, 11, 67, 167, 176, 110, 246, 238, 35, 194, 49, 126, 18, 1, 207, 171, 243, 88, 89, 132, 211, 2, 82, 73, 95, 138, 197, 162, 118, 10, 198, 253, 160, 251, 32, 208, 50, 74, 151, 0, 150, 72, 188, 184, 55, 198, 153, 128, 149, 66, 73, 135, 253, 168, 201, 249, 32, 144, 155, 48, 227, 64, 208, 41, 74, 2, 54, 95, 69, 113, 245, 248, 114, 11, 68, 129, 134, 63, 222, 73, 27, 103, 80, 18, 209, 116, 252, 115, 96, 71, 80, 10, 177, 32, 36, 240, 21, 160, 186, 36, 56, 205, 190, 248, 191, 21, 53, 117, 68, 194, 89, 19, 38, 245, 199, 23, 221, 29, 255, 177, 78, 6, 146, 30, 93, 28, 18, 19, 150, 109, 119, 7, 101, 120, 93, 170, 21, 190, 3, 4, 199, 208, 170, 79, 114, 251, 174, 44, 143, 116, 9, 96, 137, 36, 153, 72, 236, 167, 246, 47, 20, 138, 106, 226, 242, 101, 32, 22, 95, 218, 240, 73, 8, 218, 185, 215, 33, 74, 18, 248, 41, 17, 184, 178, 246, 247, 163, 132, 219, 40, 232, 204, 106, 91, 244, 213, 219, 54, 174, 98, 87, 168, 205, 121, 60, 172, 217, 108, 199, 99, 34, 29, 216, 34, 204, 208, 2, 26, 177, 225, 199, 151, 199, 149, 230, 185, 42, 75, 217, 42, 41, 220, 17, 172, 12, 126, 62, 125, 118, 102, 70, 147, 64, 56, 93, 23, 96, 25, 165, 75, 0, 75, 36, 0, 216, 97, 206, 6, 156, 156, 154, 150, 56, 63, 123, 101, 123, 223, 219, 141, 31, 162, 224, 84, 96, 96, 69, 23, 243, 191, 159, 28, 115, 4, 48, 49, 70, 64, 205, 223, 86, 146, 77, 103, 53, 142, 120, 56, 247, 64, 247, 163, 50, 15, 124, 37, 2, 91, 151, 73, 161, 240, 184, 127, 156, 3, 66, 121, 108, 168, 37, 241, 79, 55, 209, 131, 178, 129, 249, 118, 222, 127, 167, 144, 0, 26, 252, 34, 16, 24, 40, 153, 205, 119, 223, 7, 184, 140, 210, 37, 128, 37, 146, 92, 174, 103, 26, 13, 186, 49, 55, 61, 173, 223, 5, 212, 198, 207, 255, 102, 161, 128, 114, 64, 116, 191, 39, 20, 7, 164, 31, 71, 80, 175, 235, 99, 160, 152, 111, 239, 177, 2, 35, 6, 246, 11, 235, 98, 54, 139, 49, 56, 209, 105, 186, 36, 168, 197, 100, 162, 238, 195, 124, 48, 7, 231, 4, 241, 142, 125, 81, 58, 235, 84, 65, 126, 155, 160, 108, 30, 143, 166, 82, 86, 220, 55, 83, 169, 84, 247, 81, 224, 101, 148, 46, 1, 44, 145, 4, 153, 220, 168, 136, 87, 40, 204, 78, 75, 34, 225, 171, 245, 203, 142, 52, 39, 70, 163, 18, 160, 236, 92, 179, 233, 71, 194, 56, 139, 33, 16, 180, 212, 205, 143, 3, 212, 17, 196, 185, 253, 38, 24, 162, 81, 64, 243, 32, 84, 108, 165, 12, 71, 0, 172, 217, 7, 95, 102, 206, 61, 66, 98, 94, 7, 17, 184, 186, 204, 92, 254, 133, 7, 205, 56, 2, 235, 116, 1, 194, 250, 244, 124, 121, 158, 54, 240, 216, 148, 211, 180, 76, 40, 133, 233, 41, 78, 153, 174, 164, 178, 185, 174, 11, 176, 140, 210, 37, 128, 37, 146, 100, 58, 61, 3, 115, 183, 52, 61, 49, 142, 134, 14, 77, 175, 175, 4, 55, 128, 160, 40, 40, 221, 207, 105, 247, 249, 236, 121, 65, 154, 155, 35, 240, 212, 196, 238, 227, 71, 136, 209, 242, 38, 152, 126, 2, 2, 151, 25, 102, 251, 249, 90, 58, 15, 214, 185, 8, 78, 230, 235, 160, 27, 164, 113, 77, 125, 188, 232, 181, 1, 27, 152, 145, 130, 168, 89, 153, 155, 237, 206, 3, 88, 70, 233, 18, 192, 18, 201, 153, 23, 95, 82, 133, 146, 156, 43, 21, 138, 226, 1, 0, 30, 180, 229, 2, 140, 80, 128, 38, 3, 141, 35, 72, 39, 94, 81, 196, 205, 29, 160, 246, 100, 61, 220, 170, 115, 75, 183, 190, 56, 168, 154, 237, 172, 235, 113, 242, 184, 45, 84, 116, 31, 46, 216, 116, 218, 3, 29, 37, 218, 193, 156, 87, 71, 78, 123, 125, 62, 173, 189, 37, 137, 175, 195, 114, 32, 49, 178, 252, 204, 212, 12, 75, 84, 125, 47, 232, 190, 18, 124, 25, 165, 75, 0, 75, 36, 87, 124, 252, 67, 213, 40, 106, 140, 240, 121, 128, 0, 13, 158, 111, 3, 86, 159, 217, 226, 192, 105, 76, 130, 203, 104, 72, 7, 16, 27, 40, 157, 113, 108, 58, 63, 182, 111, 65, 103, 55, 81, 65, 156, 105, 139, 3, 43, 48, 128, 211, 21, 35, 90, 175, 169, 216, 105, 106, 23, 140, 187, 48, 31, 40, 74, 60, 136, 234, 186, 77, 235, 220, 102, 190, 238, 206, 184, 145, 249, 122, 176, 212, 255, 4, 253, 188, 251, 160, 4, 128, 88, 189, 86, 199, 121, 73, 101, 227, 105, 103, 213, 177, 218, 149, 101, 146, 46, 1, 44, 157, 68, 126, 144, 152, 40, 204, 21, 36, 108, 241, 171, 64, 230, 129, 32, 231, 239, 207, 131, 199, 136, 198, 29, 66, 40, 46, 203, 173, 91, 97, 50, 182, 92, 80, 215, 147, 137, 214, 219, 177, 31, 149, 142, 213, 199, 251, 241, 139, 196, 102, 145, 4, 230, 193, 108, 42, 56, 114, 31, 192, 66, 113, 231, 104, 202, 106, 196, 44, 33, 204, 226, 247, 18, 13, 53, 104, 86, 117, 243, 246, 211, 187, 143, 2, 47, 163, 116, 9, 96, 9, 5, 154, 174, 80, 42, 115, 134, 91, 67, 82, 65, 66, 98, 33, 223, 157, 215, 1, 90, 0, 192, 53, 126, 21, 98, 197, 101, 51, 217, 174, 19, 68, 14, 72, 156, 84, 164, 67, 120, 88, 50, 137, 143, 23, 107, 39, 155, 230, 26, 113, 29, 128, 20, 206, 218, 227, 62, 231, 123, 237, 41, 200, 212, 58, 31, 223, 147, 207, 39, 21, 25, 218, 121, 40, 167, 29, 118, 90, 166, 115, 47, 70, 12, 41, 184, 227, 49, 19, 135, 12, 57, 225, 208, 109, 113, 246, 3, 232, 123, 13, 80, 139, 166, 51, 206, 125, 32, 207, 247, 18, 210, 108, 212, 165, 12, 215, 63, 153, 72, 22, 159, 243, 186, 215, 116, 31, 5, 94, 70, 233, 18, 192, 18, 138, 23, 243, 166, 234, 213, 154, 126, 243, 62, 153, 76, 28, 1, 62, 11, 133, 16, 81, 237, 238, 144, 99, 165, 77, 18, 4, 143, 37, 2, 138, 1, 221, 226, 178, 243, 210, 25, 119, 194, 250, 159, 76, 140, 134, 63, 18, 212, 33, 220, 151, 238, 239, 72, 53, 31, 41, 205, 136, 59, 78, 18, 193, 60, 248, 205, 30, 248, 178, 20, 94, 31, 62, 11, 128, 164, 233, 95, 124, 214, 197, 93, 11, 96, 25, 165, 75, 0, 75, 40, 48, 251, 167, 90, 13, 52, 110, 184, 0, 252, 252, 181, 1, 143, 17, 163, 57, 13, 104, 8, 112, 247, 107, 139, 195, 25, 3, 238, 146, 121, 123, 48, 68, 49, 104, 234, 225, 246, 157, 147, 126, 22, 139, 150, 194, 102, 243, 123, 229, 106, 199, 62, 40, 145, 209, 237, 102, 71, 180, 46, 98, 29, 102, 185, 11, 70, 204, 126, 105, 57, 184, 26, 205, 250, 124, 56, 178, 157, 192, 226, 4, 61, 35, 250, 234, 112, 183, 61, 150, 236, 27, 33, 73, 242, 113, 96, 164, 114, 14, 192, 226, 205, 187, 178, 132, 210, 37, 128, 37, 20, 104, 182, 105, 106, 55, 190, 235, 142, 4, 192, 134, 175, 218, 15, 75, 5, 174, 197, 151, 35, 3, 13, 252, 97, 185, 64, 220, 170, 93, 206, 3, 112, 161, 60, 81, 250, 19, 137, 238, 70, 247, 79, 19, 222, 4, 39, 157, 199, 212, 41, 102, 31, 102, 63, 140, 119, 134, 5, 157, 147, 243, 197, 32, 236, 92, 52, 251, 235, 172, 142, 101, 249, 193, 84, 189, 70, 124, 89, 106, 144, 232, 78, 2, 90, 102, 233, 18, 192, 82, 138, 159, 152, 227, 19, 110, 115, 115, 37, 9, 146, 73, 155, 200, 134, 111, 122, 219, 205, 138, 89, 180, 65, 190, 88, 152, 239, 92, 119, 39, 79, 84, 22, 210, 198, 92, 135, 116, 22, 7, 68, 109, 204, 136, 155, 255, 255, 196, 114, 164, 26, 159, 92, 22, 239, 227, 72, 194, 50, 90, 14, 59, 215, 119, 31, 194, 82, 130, 55, 208, 253, 30, 192, 50, 75, 151, 0, 150, 80, 18, 126, 188, 70, 147, 151, 175, 187, 78, 36, 225, 2, 216, 171, 205, 183, 4, 155, 185, 248, 20, 104, 78, 254, 80, 78, 199, 250, 109, 63, 128, 205, 50, 226, 16, 202, 117, 4, 78, 239, 101, 210, 34, 229, 220, 38, 21, 118, 33, 80, 187, 114, 85, 59, 240, 58, 203, 169, 150, 110, 153, 125, 216, 215, 115, 107, 1, 150, 209, 224, 10, 115, 103, 198, 90, 97, 93, 237, 114, 79, 38, 44, 167, 251, 237, 168, 239, 137, 4, 229, 120, 190, 172, 86, 191, 155, 8, 18, 240, 98, 221, 175, 2, 47, 183, 116, 9, 96, 9, 37, 145, 72, 233, 235, 173, 43, 149, 138, 164, 83, 41, 245, 227, 9, 142, 199, 249, 237, 4, 142, 91, 186, 248, 98, 233, 196, 165, 45, 196, 36, 214, 229, 128, 223, 182, 42, 32, 44, 241, 100, 248, 51, 68, 64, 211, 98, 81, 41, 69, 187, 70, 220, 110, 116, 225, 124, 248, 159, 90, 88, 157, 13, 243, 98, 44, 161, 90, 173, 174, 4, 128, 213, 174, 11, 176, 204, 210, 37, 128, 37, 20, 47, 153, 156, 67, 3, 111, 241, 85, 87, 201, 68, 160, 115, 1, 216, 224, 59, 129, 122, 36, 81, 173, 219, 249, 115, 168, 193, 102, 92, 119, 194, 100, 181, 6, 108, 157, 237, 114, 86, 184, 202, 148, 133, 169, 20, 238, 95, 43, 131, 116, 228, 46, 216, 30, 249, 122, 156, 168, 219, 172, 45, 18, 219, 225, 135, 127, 186, 89, 212, 81, 66, 211, 58, 235, 154, 47, 231, 146, 245, 60, 88, 63, 76, 0, 246, 145, 40, 193, 196, 162, 174, 5, 176, 204, 210, 37, 128, 37, 20, 207, 247, 249, 118, 155, 218, 212, 216, 168, 36, 147, 190, 36, 125, 184, 1, 16, 66, 202, 0, 219, 129, 194, 128, 124, 193, 207, 166, 169, 91, 96, 199, 214, 41, 124, 172, 88, 193, 206, 184, 73, 81, 240, 184, 252, 78, 209, 50, 248, 215, 153, 229, 200, 194, 133, 118, 69, 143, 219, 222, 192, 222, 148, 227, 118, 38, 213, 136, 73, 224, 38, 110, 222, 128, 126, 229, 199, 74, 251, 120, 219, 117, 35, 152, 234, 76, 128, 208, 5, 210, 233, 209, 226, 169, 5, 160, 231, 27, 143, 186, 15, 2, 45, 179, 116, 9, 96, 9, 5, 205, 155, 159, 184, 42, 23, 138, 124, 43, 144, 72, 0, 43, 128, 61, 237, 4, 184, 130, 98, 177, 184, 52, 102, 19, 64, 22, 44, 20, 243, 26, 46, 187, 210, 33, 79, 4, 254, 78, 233, 168, 230, 9, 5, 48, 215, 99, 124, 188, 116, 214, 109, 38, 242, 52, 195, 150, 14, 109, 106, 10, 137, 0, 63, 130, 30, 116, 128, 152, 233, 95, 208, 115, 236, 16, 71, 100, 90, 142, 89, 46, 27, 251, 108, 212, 106, 56, 135, 80, 26, 158, 215, 237, 4, 92, 102, 233, 18, 192, 18, 74, 239, 202, 53, 85, 180, 240, 74, 121, 118, 74, 129, 207, 47, 5, 83, 147, 62, 41, 96, 109, 150, 211, 206, 92, 234, 59, 3, 16, 52, 29, 183, 140, 96, 117, 226, 210, 219, 160, 90, 20, 88, 146, 85, 29, 73, 220, 108, 63, 118, 10, 134, 26, 184, 145, 21, 110, 132, 64, 192, 187, 64, 224, 235, 211, 125, 45, 192, 27, 9, 230, 9, 69, 219, 33, 137, 192, 50, 110, 159, 252, 110, 9, 187, 55, 77, 224, 23, 129, 231, 127, 40, 169, 129, 228, 69, 250, 168, 214, 205, 244, 255, 32, 138, 119, 39, 1, 45, 179, 116, 9, 96, 9, 229, 226, 231, 95, 14, 213, 239, 143, 55, 26, 117, 0, 53, 38, 169, 20, 95, 15, 14, 240, 116, 0, 173, 173, 41, 137, 139, 39, 19, 230, 35, 232, 98, 17, 162, 13, 168, 126, 82, 153, 63, 6, 39, 139, 137, 169, 109, 93, 80, 115, 51, 78, 132, 63, 110, 51, 147, 224, 146, 121, 36, 206, 146, 224, 182, 230, 225, 37, 179, 222, 134, 191, 238, 135, 174, 12, 202, 162, 112, 211, 18, 64, 34, 72, 118, 45, 128, 101, 150, 46, 1, 44, 161, 252, 191, 63, 127, 127, 21, 96, 157, 156, 28, 155, 208, 161, 192, 108, 54, 99, 115, 230, 197, 24, 203, 11, 81, 117, 68, 64, 51, 201, 6, 23, 157, 135, 221, 145, 229, 72, 117, 155, 85, 252, 59, 226, 166, 182, 86, 130, 182, 101, 192, 79, 147, 189, 133, 101, 155, 28, 230, 119, 174, 194, 100, 230, 48, 137, 141, 73, 117, 59, 18, 24, 212, 136, 176, 121, 243, 155, 224, 236, 144, 65, 187, 129, 203, 206, 55, 14, 249, 65, 208, 125, 14, 96, 153, 165, 75, 0, 75, 44, 65, 224, 141, 243, 251, 0, 51, 179, 115, 176, 0, 18, 166, 209, 35, 64, 167, 42, 184, 136, 30, 5, 60, 81, 4, 97, 94, 7, 90, 30, 47, 154, 71, 112, 234, 11, 52, 204, 122, 71, 121, 214, 169, 160, 71, 154, 198, 17, 117, 216, 165, 40, 37, 32, 65, 203, 44, 16, 108, 192, 227, 66, 158, 153, 209, 199, 13, 185, 173, 89, 106, 9, 20, 161, 118, 231, 82, 101, 62, 75, 151, 198, 33, 64, 96, 58, 78, 141, 36, 192, 242, 139, 251, 22, 140, 27, 195, 166, 167, 103, 46, 181, 90, 133, 231, 29, 121, 137, 160, 107, 1, 44, 179, 116, 9, 96, 137, 5, 26, 110, 15, 191, 122, 195, 161, 174, 100, 50, 105, 198, 237, 181, 249, 155, 75, 111, 32, 96, 101, 17, 80, 22, 136, 113, 155, 53, 184, 119, 247, 19, 108, 139, 69, 9, 198, 86, 228, 233, 39, 184, 159, 172, 82, 43, 218, 49, 137, 58, 23, 117, 40, 146, 36, 58, 223, 85, 200, 154, 152, 187, 96, 191, 186, 161, 93, 118, 138, 221, 45, 143, 213, 101, 25, 218, 89, 88, 144, 239, 3, 224, 48, 41, 164, 21, 120, 221, 207, 130, 47, 183, 116, 9, 96, 137, 37, 157, 78, 239, 170, 84, 75, 50, 59, 49, 38, 217, 148, 33, 0, 213, 252, 16, 157, 13, 216, 9, 208, 197, 32, 122, 18, 97, 209, 182, 166, 166, 176, 26, 132, 39, 178, 32, 76, 249, 133, 129, 155, 154, 165, 157, 75, 192, 13, 153, 6, 34, 208, 207, 149, 145, 16, 58, 14, 202, 109, 211, 145, 244, 56, 177, 135, 97, 196, 150, 211, 253, 161, 46, 158, 183, 177, 74, 204, 249, 115, 159, 252, 120, 106, 163, 166, 125, 0, 77, 176, 65, 119, 34, 208, 50, 75, 151, 0, 150, 88, 18, 233, 212, 193, 176, 17, 70, 179, 147, 211, 250, 17, 76, 118, 6, 18, 12, 139, 133, 32, 212, 159, 3, 180, 19, 183, 234, 80, 181, 0, 220, 128, 108, 39, 129, 28, 65, 88, 39, 199, 232, 89, 173, 198, 1, 108, 183, 15, 87, 53, 215, 59, 181, 191, 46, 25, 197, 97, 62, 238, 120, 58, 133, 89, 139, 178, 31, 151, 100, 19, 52, 205, 102, 240, 56, 248, 223, 29, 122, 173, 162, 150, 127, 179, 222, 168, 117, 9, 96, 153, 165, 75, 0, 75, 44, 126, 34, 185, 23, 139, 226, 248, 200, 168, 106, 66, 146, 128, 211, 220, 6, 8, 70, 22, 196, 23, 131, 206, 224, 229, 113, 226, 0, 180, 152, 4, 184, 189, 106, 91, 106, 93, 11, 108, 173, 243, 8, 117, 116, 10, 74, 131, 44, 232, 201, 91, 89, 196, 45, 11, 86, 93, 93, 76, 92, 84, 238, 113, 226, 202, 116, 150, 227, 246, 118, 189, 56, 167, 243, 127, 234, 32, 199, 238, 251, 0, 151, 89, 186, 4, 176, 196, 178, 241, 140, 179, 38, 209, 210, 199, 167, 38, 198, 213, 85, 230, 11, 48, 28, 64, 219, 192, 132, 168, 59, 176, 0, 33, 243, 66, 114, 80, 183, 129, 69, 45, 240, 180, 115, 205, 68, 219, 117, 80, 180, 110, 130, 158, 108, 131, 194, 44, 231, 129, 32, 220, 207, 227, 235, 188, 241, 235, 220, 198, 9, 211, 117, 137, 133, 78, 47, 224, 170, 219, 39, 227, 63, 34, 112, 241, 100, 210, 62, 94, 4, 61, 127, 108, 192, 126, 138, 122, 83, 251, 0, 106, 185, 124, 111, 119, 30, 192, 50, 75, 151, 0, 150, 88, 110, 252, 250, 87, 102, 226, 94, 252, 192, 244, 228, 164, 126, 2, 219, 135, 5, 0, 244, 217, 220, 39, 146, 121, 40, 41, 80, 109, 113, 106, 104, 2, 199, 226, 13, 178, 208, 116, 87, 173, 79, 162, 176, 50, 95, 206, 72, 167, 149, 65, 89, 252, 170, 113, 151, 223, 110, 20, 139, 42, 88, 176, 53, 87, 92, 128, 56, 190, 120, 156, 48, 191, 195, 227, 113, 117, 227, 168, 219, 15, 50, 217, 237, 234, 189, 43, 87, 116, 135, 1, 151, 89, 186, 4, 176, 244, 18, 197, 130, 212, 158, 194, 108, 73, 194, 6, 125, 93, 34, 162, 3, 42, 136, 46, 6, 166, 73, 49, 63, 205, 143, 217, 124, 187, 25, 49, 78, 5, 111, 122, 216, 105, 29, 224, 191, 35, 2, 22, 181, 197, 231, 119, 195, 4, 51, 235, 78, 235, 132, 56, 107, 192, 149, 85, 141, 172, 189, 124, 11, 123, 238, 181, 14, 6, 38, 116, 134, 118, 221, 16, 172, 51, 233, 113, 226, 182, 181, 173, 204, 149, 113, 100, 145, 244, 57, 203, 49, 148, 58, 39, 2, 121, 177, 234, 224, 138, 181, 93, 11, 96, 153, 165, 75, 0, 203, 32, 190, 23, 223, 49, 55, 51, 35, 5, 4, 63, 224, 183, 241, 0, 67, 5, 205, 17, 97, 115, 68, 209, 178, 174, 56, 163, 11, 54, 157, 7, 118, 27, 112, 22, 160, 218, 3, 175, 89, 252, 103, 92, 9, 5, 187, 253, 105, 142, 18, 136, 221, 126, 177, 60, 65, 114, 91, 76, 181, 186, 232, 80, 244, 237, 116, 186, 19, 238, 112, 180, 177, 33, 226, 202, 113, 22, 32, 215, 192, 93, 92, 171, 174, 90, 179, 182, 107, 1, 44, 179, 116, 9, 96, 25, 36, 147, 77, 239, 107, 53, 106, 218, 217, 149, 73, 167, 52, 141, 192, 112, 254, 120, 219, 8, 126, 18, 33, 72, 157, 121, 175, 208, 181, 218, 154, 75, 13, 154, 115, 100, 97, 158, 211, 234, 52, 38, 88, 158, 174, 131, 219, 111, 167, 155, 209, 22, 174, 115, 119, 139, 211, 41, 76, 235, 76, 119, 135, 239, 210, 184, 110, 211, 22, 20, 237, 56, 77, 147, 198, 132, 184, 68, 45, 28, 75, 232, 85, 47, 184, 236, 178, 46, 1, 44, 179, 116, 9, 96, 25, 196, 139, 251, 251, 98, 176, 209, 11, 179, 51, 184, 224, 230, 146, 255, 168, 225, 187, 54, 114, 108, 80, 154, 192, 54, 132, 186, 246, 5, 240, 215, 161, 201, 93, 63, 0, 162, 237, 64, 205, 170, 156, 129, 184, 155, 141, 199, 206, 61, 159, 47, 2, 194, 250, 252, 23, 139, 81, 96, 241, 225, 48, 31, 63, 238, 135, 219, 107, 62, 3, 227, 20, 87, 222, 165, 59, 178, 224, 18, 194, 250, 245, 20, 145, 230, 248, 69, 57, 139, 17, 183, 140, 249, 210, 106, 70, 82, 46, 20, 36, 136, 73, 253, 215, 95, 251, 51, 11, 59, 37, 186, 178, 228, 210, 37, 128, 101, 16, 63, 147, 154, 4, 152, 42, 163, 135, 135, 117, 24, 144, 109, 223, 137, 2, 204, 138, 118, 136, 217, 64, 80, 57, 141, 191, 88, 148, 12, 88, 160, 163, 34, 93, 119, 32, 116, 226, 226, 44, 138, 56, 3, 65, 232, 106, 85, 75, 224, 9, 246, 177, 160, 30, 10, 139, 185, 162, 168, 175, 45, 44, 215, 89, 5, 243, 184, 47, 179, 104, 139, 77, 110, 167, 113, 157, 231, 201, 89, 128, 245, 70, 85, 162, 68, 200, 89, 128, 157, 155, 116, 101, 25, 164, 75, 0, 203, 32, 253, 171, 86, 205, 162, 105, 207, 242, 59, 129, 124, 43, 144, 123, 132, 87, 193, 199, 38, 175, 104, 64, 20, 8, 117, 129, 162, 229, 108, 158, 10, 146, 149, 28, 52, 106, 127, 110, 27, 252, 22, 148, 117, 98, 215, 219, 200, 66, 68, 45, 3, 44, 213, 141, 80, 240, 50, 183, 93, 162, 45, 36, 21, 181, 88, 22, 215, 233, 164, 51, 125, 241, 190, 23, 85, 185, 32, 219, 70, 56, 36, 218, 104, 214, 37, 108, 128, 6, 91, 49, 253, 62, 120, 87, 150, 87, 186, 4, 176, 12, 178, 126, 219, 233, 179, 64, 238, 92, 181, 92, 85, 19, 220, 98, 88, 197, 1, 122, 177, 44, 38, 131, 78, 112, 113, 254, 124, 231, 118, 79, 84, 135, 222, 93, 102, 177, 10, 11, 250, 199, 137, 110, 138, 12, 230, 117, 230, 51, 206, 188, 163, 214, 66, 216, 227, 207, 247, 26, 216, 99, 197, 130, 157, 128, 74, 130, 32, 162, 88, 204, 59, 210, 209, 117, 101, 137, 165, 75, 0, 203, 32, 191, 248, 39, 31, 169, 226, 82, 207, 22, 139, 37, 224, 202, 184, 185, 79, 214, 218, 23, 104, 245, 35, 72, 39, 49, 16, 252, 238, 1, 163, 159, 94, 58, 182, 61, 210, 46, 153, 237, 138, 44, 222, 77, 103, 222, 147, 138, 41, 104, 126, 70, 248, 1, 146, 86, 179, 5, 43, 160, 1, 82, 235, 90, 0, 199, 66, 186, 4, 176, 12, 242, 115, 155, 146, 161, 231, 123, 179, 165, 98, 89, 248, 108, 61, 77, 95, 170, 227, 246, 236, 63, 130, 14, 129, 218, 144, 97, 49, 240, 29, 108, 156, 242, 228, 179, 250, 44, 194, 52, 53, 209, 237, 246, 237, 205, 58, 151, 46, 126, 36, 144, 182, 211, 24, 233, 40, 192, 109, 220, 106, 103, 29, 108, 45, 157, 235, 46, 238, 214, 41, 140, 115, 91, 151, 222, 174, 154, 243, 20, 204, 235, 194, 220, 7, 72, 216, 49, 89, 175, 213, 164, 84, 43, 73, 204, 143, 119, 31, 5, 62, 6, 210, 37, 128, 101, 146, 184, 231, 205, 84, 43, 101, 105, 214, 154, 0, 2, 128, 206, 137, 59, 138, 16, 226, 228, 73, 58, 227, 22, 8, 16, 211, 1, 42, 183, 221, 130, 109, 77, 149, 42, 36, 12, 40, 217, 54, 113, 60, 169, 116, 150, 57, 82, 171, 232, 4, 52, 165, 99, 63, 109, 113, 105, 139, 242, 184, 9, 143, 208, 28, 165, 121, 242, 144, 83, 128, 249, 153, 112, 94, 7, 37, 195, 120, 188, 251, 28, 192, 49, 144, 46, 1, 44, 151, 132, 209, 40, 191, 130, 219, 130, 185, 171, 157, 111, 76, 178, 36, 208, 57, 157, 215, 9, 65, 226, 102, 0, 106, 158, 205, 110, 199, 93, 112, 194, 178, 49, 66, 108, 62, 81, 139, 46, 76, 106, 19, 134, 146, 143, 5, 179, 214, 201, 56, 91, 131, 3, 184, 91, 58, 89, 180, 174, 199, 109, 33, 173, 121, 157, 249, 140, 187, 186, 236, 190, 213, 90, 81, 139, 39, 142, 253, 153, 99, 224, 33, 243, 163, 160, 156, 145, 28, 134, 241, 238, 27, 129, 143, 129, 116, 9, 96, 153, 36, 22, 143, 79, 55, 27, 108, 236, 208, 128, 118, 80, 158, 192, 83, 13, 8, 113, 126, 188, 254, 0, 126, 247, 115, 162, 128, 181, 96, 106, 139, 205, 110, 119, 2, 46, 122, 55, 191, 91, 116, 110, 198, 58, 221, 126, 29, 240, 117, 63, 22, 203, 140, 182, 23, 29, 213, 181, 227, 237, 122, 77, 196, 45, 221, 98, 65, 57, 4, 238, 131, 81, 38, 51, 44, 120, 227, 16, 215, 173, 59, 227, 139, 199, 87, 168, 119, 101, 153, 165, 75, 0, 203, 36, 126, 34, 152, 83, 11, 0, 13, 62, 14, 243, 119, 177, 180, 193, 104, 193, 79, 161, 127, 223, 73, 2, 212, 184, 10, 39, 135, 38, 39, 138, 37, 151, 104, 51, 176, 88, 80, 86, 203, 216, 52, 43, 238, 115, 221, 110, 19, 37, 1, 102, 35, 104, 169, 249, 162, 243, 121, 186, 232, 204, 128, 112, 117, 81, 146, 19, 61, 47, 253, 179, 239, 37, 176, 5, 213, 194, 65, 30, 191, 41, 64, 254, 10, 252, 120, 247, 163, 32, 199, 64, 186, 4, 176, 76, 18, 79, 250, 147, 141, 102, 77, 39, 189, 80, 219, 83, 218, 154, 27, 98, 52, 165, 1, 7, 151, 109, 223, 30, 69, 76, 14, 126, 44, 99, 131, 161, 9, 179, 189, 214, 195, 168, 51, 187, 41, 4, 29, 39, 250, 0, 96, 26, 88, 159, 211, 250, 86, 220, 211, 120, 11, 100, 62, 219, 196, 17, 116, 219, 142, 31, 143, 229, 9, 197, 110, 99, 130, 141, 80, 201, 119, 166, 91, 225, 187, 7, 66, 221, 125, 76, 26, 244, 141, 186, 178, 236, 210, 37, 128, 229, 146, 40, 152, 100, 19, 175, 87, 13, 1, 116, 2, 209, 129, 144, 128, 94, 12, 46, 150, 115, 32, 158, 23, 148, 239, 44, 230, 226, 11, 55, 85, 113, 219, 59, 233, 36, 13, 119, 28, 142, 32, 218, 199, 196, 133, 219, 100, 17, 63, 180, 133, 101, 16, 184, 77, 123, 59, 150, 237, 44, 175, 196, 196, 96, 215, 59, 196, 109, 167, 155, 34, 63, 30, 248, 157, 39, 216, 149, 101, 146, 46, 1, 44, 147, 228, 210, 169, 73, 168, 228, 144, 15, 190, 180, 39, 195, 88, 220, 56, 0, 205, 131, 98, 62, 116, 130, 151, 235, 54, 54, 223, 65, 136, 159, 90, 10, 248, 163, 198, 215, 192, 159, 45, 75, 160, 51, 216, 84, 253, 241, 143, 245, 242, 187, 252, 154, 31, 55, 101, 8, 196, 206, 125, 51, 104, 221, 172, 202, 84, 215, 22, 18, 9, 127, 63, 74, 142, 88, 198, 158, 82, 19, 215, 162, 94, 175, 234, 49, 123, 126, 162, 251, 58, 176, 99, 32, 93, 2, 88, 38, 137, 249, 177, 74, 20, 54, 235, 252, 8, 134, 231, 119, 92, 246, 69, 192, 250, 81, 226, 128, 77, 81, 56, 99, 93, 223, 243, 199, 138, 8, 96, 77, 163, 153, 110, 76, 117, 87, 158, 128, 111, 111, 107, 49, 233, 214, 149, 4, 204, 198, 198, 220, 215, 237, 205, 143, 127, 71, 146, 197, 196, 209, 22, 150, 239, 216, 70, 235, 237, 204, 119, 2, 18, 168, 55, 154, 74, 66, 60, 14, 216, 35, 221, 121, 0, 199, 64, 186, 4, 176, 76, 146, 204, 101, 75, 104, 232, 213, 169, 201, 73, 125, 19, 46, 49, 162, 218, 245, 39, 16, 5, 29, 2, 1, 211, 25, 152, 166, 111, 241, 93, 92, 159, 3, 35, 130, 2, 181, 67, 184, 174, 46, 64, 167, 213, 160, 133, 159, 64, 142, 4, 98, 10, 235, 118, 32, 231, 230, 92, 62, 65, 217, 118, 178, 150, 129, 223, 111, 193, 15, 137, 188, 192, 235, 118, 2, 30, 3, 233, 18, 192, 50, 73, 42, 145, 159, 139, 34, 175, 84, 169, 54, 181, 241, 119, 154, 246, 63, 174, 16, 44, 110, 59, 7, 254, 246, 108, 66, 151, 214, 129, 66, 71, 24, 12, 156, 125, 168, 113, 251, 227, 8, 3, 211, 244, 129, 35, 110, 107, 144, 108, 203, 107, 210, 2, 209, 17, 9, 14, 51, 178, 122, 4, 181, 20, 112, 44, 92, 186, 87, 136, 171, 40, 158, 59, 164, 179, 46, 196, 57, 2, 202, 250, 217, 15, 218, 106, 180, 100, 122, 114, 22, 245, 134, 117, 207, 107, 241, 67, 170, 93, 89, 102, 233, 18, 192, 50, 201, 138, 21, 67, 197, 88, 204, 43, 150, 139, 112, 117, 1, 84, 130, 192, 106, 191, 182, 40, 48, 9, 232, 35, 33, 240, 201, 68, 65, 69, 224, 62, 241, 118, 28, 242, 235, 20, 5, 60, 119, 127, 196, 77, 76, 93, 142, 92, 88, 150, 61, 246, 134, 92, 22, 9, 183, 95, 92, 71, 103, 49, 198, 23, 237, 199, 37, 181, 96, 121, 40, 181, 120, 126, 195, 111, 121, 69, 230, 117, 101, 121, 165, 75, 0, 203, 36, 231, 94, 118, 73, 3, 90, 175, 196, 151, 130, 212, 160, 249, 64, 6, 228, 1, 0, 129, 32, 48, 226, 180, 176, 18, 3, 147, 231, 179, 218, 178, 24, 228, 206, 236, 119, 56, 92, 152, 139, 117, 235, 167, 243, 237, 67, 14, 208, 14, 220, 20, 110, 239, 142, 193, 89, 16, 237, 31, 95, 24, 98, 59, 27, 41, 46, 157, 194, 58, 92, 61, 174, 94, 155, 245, 120, 177, 173, 140, 167, 165, 152, 239, 40, 231, 39, 2, 241, 18, 126, 37, 22, 36, 186, 95, 5, 58, 6, 210, 37, 128, 101, 146, 15, 253, 241, 251, 72, 0, 51, 4, 25, 39, 3, 25, 32, 51, 116, 162, 6, 240, 34, 8, 137, 148, 31, 83, 158, 168, 188, 2, 213, 178, 1, 227, 74, 4, 46, 224, 167, 199, 193, 206, 67, 183, 45, 139, 235, 49, 89, 193, 186, 230, 233, 114, 97, 154, 214, 109, 197, 212, 134, 31, 183, 237, 108, 77, 29, 85, 105, 241, 197, 129, 194, 165, 110, 23, 171, 231, 122, 123, 187, 157, 128, 199, 64, 186, 4, 176, 124, 2, 20, 122, 163, 4, 83, 179, 217, 0, 62, 8, 74, 38, 207, 35, 5, 41, 198, 151, 118, 0, 233, 144, 5, 224, 236, 144, 35, 77, 230, 113, 0, 37, 88, 23, 244, 53, 48, 138, 44, 157, 139, 239, 198, 226, 40, 110, 127, 139, 247, 107, 215, 59, 171, 39, 216, 153, 238, 58, 29, 59, 201, 96, 129, 116, 38, 187, 56, 235, 113, 65, 133, 15, 1, 121, 146, 235, 27, 184, 229, 5, 151, 191, 120, 202, 38, 118, 101, 25, 165, 75, 0, 203, 40, 190, 231, 31, 174, 23, 10, 6, 16, 64, 149, 121, 44, 118, 30, 93, 196, 106, 8, 27, 153, 176, 34, 60, 29, 68, 9, 240, 78, 144, 171, 22, 182, 106, 217, 197, 185, 170, 1, 105, 170, 145, 53, 119, 62, 159, 128, 213, 153, 119, 172, 221, 102, 106, 29, 166, 154, 199, 137, 230, 45, 18, 119, 28, 26, 184, 7, 22, 233, 40, 166, 29, 131, 79, 240, 83, 162, 224, 126, 93, 128, 112, 244, 33, 153, 206, 200, 229, 175, 127, 235, 63, 127, 228, 239, 255, 162, 251, 62, 192, 99, 32, 93, 2, 88, 70, 137, 226, 241, 225, 154, 14, 125, 17, 218, 68, 1, 97, 209, 9, 52, 27, 119, 73, 22, 188, 38, 218, 89, 206, 8, 211, 24, 22, 104, 121, 200, 226, 178, 157, 235, 143, 171, 199, 130, 113, 177, 116, 18, 142, 198, 185, 218, 89, 246, 137, 226, 20, 238, 98, 81, 224, 49, 46, 216, 183, 30, 50, 236, 32, 48, 94, 60, 10, 187, 230, 255, 49, 146, 46, 1, 44, 163, 248, 94, 252, 177, 152, 237, 84, 51, 120, 194, 15, 171, 10, 12, 252, 209, 45, 0, 218, 76, 208, 66, 244, 213, 145, 197, 158, 51, 252, 169, 112, 233, 226, 29, 226, 52, 172, 106, 102, 252, 185, 34, 243, 26, 91, 147, 53, 56, 153, 7, 185, 171, 148, 78, 136, 253, 113, 27, 219, 129, 168, 85, 187, 33, 64, 150, 178, 199, 171, 113, 254, 168, 249, 233, 14, 32, 93, 131, 253, 233, 182, 186, 153, 41, 76, 18, 112, 68, 64, 171, 64, 79, 30, 130, 243, 115, 7, 210, 149, 101, 150, 46, 1, 44, 163, 36, 18, 193, 78, 47, 22, 211, 9, 47, 22, 27, 6, 104, 26, 227, 186, 139, 205, 139, 3, 169, 130, 238, 71, 200, 226, 173, 221, 182, 157, 226, 246, 247, 248, 61, 45, 148, 78, 144, 183, 151, 148, 199, 29, 6, 224, 173, 192, 159, 7, 183, 19, 71, 10, 157, 194, 117, 45, 135, 184, 30, 3, 57, 199, 120, 60, 93, 57, 6, 210, 37, 128, 101, 148, 190, 92, 126, 119, 10, 65, 87, 128, 0, 130, 192, 132, 133, 32, 1, 74, 76, 88, 44, 71, 72, 162, 40, 168, 59, 192, 206, 184, 123, 208, 167, 13, 64, 230, 43, 248, 77, 167, 161, 14, 223, 161, 12, 131, 230, 89, 113, 199, 226, 128, 106, 192, 58, 95, 207, 2, 93, 173, 219, 153, 122, 181, 71, 83, 151, 166, 128, 30, 143, 77, 247, 124, 95, 3, 223, 0, 212, 238, 180, 68, 133, 176, 254, 249, 30, 192, 195, 210, 125, 31, 224, 49, 147, 24, 110, 176, 185, 99, 93, 89, 22, 121, 249, 59, 127, 229, 202, 124, 95, 223, 75, 9, 44, 92, 126, 4, 0, 11, 224, 82, 157, 12, 96, 16, 102, 6, 108, 8, 11, 58, 9, 13, 0, 185, 100, 57, 166, 42, 152, 92, 62, 22, 186, 189, 173, 139, 226, 192, 220, 30, 167, 119, 210, 17, 53, 27, 206, 247, 35, 208, 52, 215, 89, 127, 220, 134, 229, 244, 159, 17, 30, 22, 221, 2, 253, 14, 33, 63, 53, 196, 61, 240, 69, 127, 34, 244, 225, 203, 208, 248, 37, 36, 112, 66, 15, 1, 61, 133, 125, 78, 99, 251, 169, 184, 23, 77, 227, 40, 102, 17, 166, 81, 219, 140, 120, 50, 25, 107, 133, 115, 177, 152, 87, 246, 125, 191, 116, 234, 198, 245, 35, 239, 123, 247, 127, 239, 118, 2, 30, 3, 233, 18, 192, 50, 203, 207, 253, 206, 123, 191, 17, 243, 188, 87, 182, 254, 255, 246, 174, 29, 167, 97, 32, 10, 122, 189, 198, 73, 36, 16, 72, 57, 0, 162, 143, 68, 71, 193, 13, 40, 40, 160, 0, 4, 23, 160, 116, 26, 130, 56, 1, 5, 71, 161, 225, 98, 27, 127, 146, 216, 113, 176, 151, 153, 181, 205, 167, 71, 150, 32, 111, 154, 253, 219, 43, 173, 102, 230, 109, 180, 222, 84, 239, 142, 211, 12, 147, 137, 134, 212, 10, 244, 35, 177, 184, 36, 93, 202, 44, 247, 203, 93, 30, 4, 167, 115, 163, 236, 220, 155, 221, 8, 52, 187, 50, 225, 134, 182, 253, 145, 240, 254, 189, 79, 1, 112, 239, 97, 218, 236, 201, 249, 117, 162, 85, 110, 18, 27, 144, 122, 141, 249, 100, 149, 87, 47, 180, 181, 75, 204, 105, 105, 61, 63, 198, 16, 222, 214, 99, 106, 107, 13, 234, 80, 86, 115, 104, 70, 2, 75, 143, 49, 99, 150, 151, 16, 153, 220, 247, 85, 62, 222, 63, 88, 189, 204, 34, 249, 182, 255, 143, 64, 4, 160, 103, 92, 69, 179, 55, 132, 195, 231, 78, 0, 128, 206, 121, 233, 210, 157, 24, 124, 145, 180, 89, 26, 38, 221, 77, 186, 141, 51, 163, 253, 251, 170, 65, 20, 248, 28, 212, 243, 161, 107, 180, 150, 240, 102, 56, 177, 74, 225, 215, 41, 58, 44, 48, 46, 131, 104, 24, 104, 3, 92, 88, 25, 180, 25, 188, 36, 105, 136, 172, 18, 136, 71, 140, 60, 221, 187, 8, 2, 93, 140, 247, 118, 139, 231, 135, 72, 254, 173, 247, 159, 67, 4, 160, 103, 64, 0, 94, 195, 209, 240, 146, 103, 243, 93, 40, 221, 58, 56, 23, 129, 2, 80, 55, 103, 246, 121, 44, 182, 68, 101, 1, 55, 103, 56, 157, 144, 172, 240, 253, 12, 253, 152, 119, 161, 53, 234, 99, 164, 115, 223, 218, 185, 85, 42, 13, 180, 78, 32, 2, 169, 214, 58, 211, 218, 207, 71, 131, 193, 122, 114, 116, 88, 222, 223, 221, 180, 202, 34, 16, 252, 132, 8, 64, 207, 184, 158, 62, 62, 41, 237, 159, 128, 252, 166, 174, 43, 132, 207, 220, 23, 59, 50, 27, 40, 66, 74, 34, 99, 65, 50, 236, 181, 179, 157, 48, 72, 195, 32, 88, 141, 194, 225, 230, 116, 114, 188, 185, 189, 56, 235, 2, 126, 129, 224, 87, 32, 2, 32, 16, 108, 49, 218, 95, 141, 4, 2, 193, 54, 66, 4, 64, 32, 216, 90, 120, 222, 7, 87, 101, 10, 206, 60, 141, 174, 148, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
            spriteTexture.LoadImage(medsDefaultSprite);
            Plugin.medsSprites["medsDefaultCard"] = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            Plugin.Log.LogInfo("Default sprites loaded!");

            // collect vanilla AudioClips, noting that there are two walk_stone entries and this will just use the latter
            Plugin.Log.LogInfo("Loading AudioClips...");
            AudioClip[] foundAudioClips = Resources.FindObjectsOfTypeAll<UnityEngine.AudioClip>();
            foreach (AudioClip ac in foundAudioClips)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla AudioClip: " + ac.name); };
                Plugin.medsAudioClips[ac.name] = ac;
            }
            // #TODO: custom AudioClips? :D
            Plugin.Log.LogInfo(foundAudioClips.Length + " AudioClips found, " + Plugin.medsAudioClips.Count + " loaded (" + (foundAudioClips.Length - Plugin.medsAudioClips.Count) + (foundAudioClips.Length - Plugin.medsAudioClips.Count == 1 ? " duplicate)" : " duplicates)"));

            // collect vanilla sprites...
            Plugin.Log.LogInfo("Loading sprites...");
            Sprite[] foundSprites = Resources.FindObjectsOfTypeAll<UnityEngine.Sprite>();
            foreach (Sprite spr in foundSprites)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla Sprite: " + spr.name); };
                Plugin.medsVanillaSprites[spr.name] = spr;
            }
            Plugin.Log.LogInfo(foundSprites.Length + " Sprites found, " + Plugin.medsVanillaSprites.Count + " loaded (" + (foundSprites.Length - Plugin.medsVanillaSprites.Count) + (foundSprites.Length - Plugin.medsVanillaSprites.Count == 1 ? " duplicate)" : " duplicates)"));


            // #TODO: probably use try...catch for loading custom content? so you don't have to deal with 8.4 million bug reports as users manually maul cards :D

            /*
             *    88      a8P   88888888888  8b        d8  888b      88    ,ad8888ba,  888888888888  88888888888  ad88888ba   
             *    88    ,88'    88            Y8,    ,8P   8888b     88   d8"'    `"8b      88       88          d8"     "8b  
             *    88  ,88"      88             Y8,  ,8P    88 `8b    88  d8'        `8b     88       88          Y8,          
             *    88,d88'       88aaaaa         "8aa8"     88  `8b   88  88          88     88       88aaaaa     `Y8aaaaa,    
             *    8888"88,      88"""""          `88'      88   `8b  88  88          88     88       88"""""       `"""""8b,  
             *    88P   Y8b     88                88       88    `8b 88  Y8,        ,8P     88       88                  `8b  
             *    88     "88,   88                88       88     `8888   Y8a.    .a8P      88       88          Y8a     a8P  
             *    88       Y8b  88888888888       88       88      `888    `"Y8888Y"'       88       88888888888  "Y88888P"                                                                                                                  
             */
            Plugin.Log.LogInfo("Loading keynotes...");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "keyNote");
            // vanilla keynotes
            for (int index = 0; index < keyNotesDataArray.Length; ++index)
            {
                string lower = keyNotesDataArray[index].KeynoteName.Replace(" ", "").ToLower();
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla keynote: " + lower); };
                keyNotesDataArray[index].Id = lower;
                Plugin.medsKeyNotesDataSource[lower] = UnityEngine.Object.Instantiate<KeyNotesData>(keyNotesDataArray[index]);
                Plugin.medsKeyNotesDataSource[lower].KeynoteName = Texts.Instance.GetText(Plugin.medsKeyNotesDataSource[lower].KeynoteName);
                string text1 = Texts.Instance.GetText(lower + "_description", "keynotes");
                if (text1 != "")
                    Plugin.medsKeyNotesDataSource[lower].Description = text1;
                string text2 = Texts.Instance.GetText(lower + "_descriptionExtended", "keynotes");
                if (text2 != "")
                    Plugin.medsKeyNotesDataSource[lower].DescriptionExtended = text2;
            }
            // custom keynotes
            FileInfo[] medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "keyNote"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom keynote: " + f.Name); };
                KeyNotesData medsKeyNote = DataTextConvert.ToData(JsonUtility.FromJson<KeyNotesDataText>(File.ReadAllText(f.ToString())));
                Plugin.medsKeyNotesDataSource[medsKeyNote.Id] = UnityEngine.Object.Instantiate<KeyNotesData>(medsKeyNote);
            }
            // save vanilla+custom
            Globals.Instance.KeyNotes = Plugin.medsKeyNotesDataSource;
            Plugin.Log.LogInfo("Keynotes loaded!");

            /*
             *           db        88        88  88888888ba          db         ad88888ba         ,adba,          ,ad8888ba,   88        88  88888888ba    ad88888ba   88888888888  ad88888ba   
             *          d88b       88        88  88      "8b        d88b       d8"     "8b        8I  I8         d8"'    `"8b  88        88  88      "8b  d8"     "8b  88          d8"     "8b  
             *         d8'`8b      88        88  88      ,8P       d8'`8b      Y8,                "8bdP'        d8'            88        88  88      ,8P  Y8,          88          Y8,          
             *        d8'  `8b     88        88  88aaaaaa8P'      d8'  `8b     `Y8aaaaa,         ,d8"8b  88     88             88        88  88aaaaaa8P'  `Y8aaaaa,    88aaaaa     `Y8aaaaa,    
             *       d8YaaaaY8b    88        88  88""""88'       d8YaaaaY8b      `"""""8b,     .dP'   Yb,8I     88             88        88  88""""88'      `"""""8b,  88"""""       `"""""8b,  
             *      d8""""""""8b   88        88  88    `8b      d8""""""""8b           `8b     8P      888'     Y8,            88        88  88    `8b            `8b  88                  `8b  
             *     d8'        `8b  Y8a.    .a8P  88     `8b    d8'        `8b  Y8a     a8P     8b,   ,dP8b       Y8a.    .a8P  Y8a.    .a8P  88     `8b   Y8a     a8P  88          Y8a     a8P  
             *    d8'          `8b  `"Y8888Y"'   88      `8b  d8'          `8b  "Y88888P"      `Y8888P"  Yb       `"Y8888Y"'    `"Y8888Y"'   88      `8b   "Y88888P"   88888888888  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading auras & curses...");
            Plugin.RecursiveFolderCreate("Obeliskial_importing", "auraCurse");
            // vanilla auras & curses; this is basically CreateAuraCurses, but occurring earlier?
            List<string> medsACIndex = new();
            for (int index = 0; index < auraCurseDataArray1.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla auraCurse: " + auraCurseDataArray1[index].ACName); };
                auraCurseDataArray1[index].Init();
                Plugin.medsAurasCursesSource.Add(auraCurseDataArray1[index].Id, UnityEngine.Object.Instantiate<AuraCurseData>(auraCurseDataArray1[index]));
                Plugin.medsAurasCursesSource[auraCurseDataArray1[index].Id].Init();
                Plugin.medsAurasCursesSource[auraCurseDataArray1[index].Id].ACName = Texts.Instance.GetText(Plugin.medsAurasCursesSource[auraCurseDataArray1[index].Id].Id);
                string text = Texts.Instance.GetText(auraCurseDataArray1[index].Id + "_description", "auracurse");
                if (text != "")
                    Plugin.medsAurasCursesSource[auraCurseDataArray1[index].Id].Description = text;
                medsACIndex.Add(auraCurseDataArray1[index].Id.ToLower());
            }
            for (int index = 0; index < auraCurseDataArray2.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla auraCurse: " + auraCurseDataArray2[index].ACName); };
                auraCurseDataArray2[index].Init();
                Plugin.medsAurasCursesSource[auraCurseDataArray2[index].Id] = UnityEngine.Object.Instantiate<AuraCurseData>(auraCurseDataArray2[index]);
                Plugin.medsAurasCursesSource[auraCurseDataArray2[index].Id].Init();
                Plugin.medsAurasCursesSource[auraCurseDataArray2[index].Id].ACName = Texts.Instance.GetText(Plugin.medsAurasCursesSource[auraCurseDataArray2[index].Id].Id);
                string text = Texts.Instance.GetText(auraCurseDataArray2[index].Id + "_description", "auracurse");
                if (text != "")
                    Plugin.medsAurasCursesSource[auraCurseDataArray2[index].Id].Description = text;
                medsACIndex.Add(auraCurseDataArray2[index].Id.ToLower());
            }
            // custom auras & curses
            Dictionary<string, AuraCurseData> medsImportAuraCurse = new();
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "auraCurse"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom auraCurse: " + f.Name); };
                AuraCurseData medsACSingle = DataTextConvert.ToData(JsonUtility.FromJson<AuraCurseDataText>(File.ReadAllText(f.ToString())));
                medsACSingle.Init();
                Plugin.medsAurasCursesSource[medsACSingle.Id] = UnityEngine.Object.Instantiate<AuraCurseData>(medsACSingle);
                if (!medsACIndex.Contains(medsACSingle.Id.ToLower()))
                    medsACIndex.Add(medsACSingle.Id.ToLower());
            }
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_AurasCursesSource").SetValue(Plugin.medsAurasCursesSource);
            Traverse.Create(Globals.Instance).Field("_AurasCursesIndex").SetValue(medsACIndex);
            Traverse.Create(Globals.Instance).Field("_AurasCurses").SetValue(Plugin.medsAurasCursesSource);
            Plugin.Log.LogInfo("Auras & curses loaded!");

            /*
             *      ,ad8888ba,         db         88888888ba   88888888ba,     ad88888ba   
             *     d8"'    `"8b       d88b        88      "8b  88      `"8b   d8"     "8b  
             *    d8'                d8'`8b       88      ,8P  88        `8b  Y8,          
             *    88                d8'  `8b      88aaaaaa8P'  88         88  `Y8aaaaa,    
             *    88               d8YaaaaY8b     88""""88'    88         88    `"""""8b,  
             *    Y8,             d8""""""""8b    88    `8b    88         8P          `8b  
             *     Y8a.    .a8P  d8'        `8b   88     `8b   88      .a8P   Y8a     a8P  
             *      `"Y8888Y"'  d8'          `8b  88      `8b  88888888Y"'     "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading cards...");
            // vanilla cards
            for (int index = 0; index < cardDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla card: " + cardDataArray[index].name); };
                cardDataArray[index].Id = cardDataArray[index].name.ToLower();
                Plugin.medsCardsSource[cardDataArray[index].Id] = UnityEngine.Object.Instantiate<CardData>(cardDataArray[index]);
            }
            // custom cards
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            Plugin.medsSecondRunImport = new();
            Plugin.medsSecondRunImport2 = new();
            Dictionary<string, CardData> medsCardsCustom = new();
            foreach (FileInfo f in medsFI)
            {
                try
                {
                    if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom card: " + f.Name); };
                    CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                    // #TODO: UNLOCKS?
                    if (!Plugin.medsCustomUnlocks.Contains(medsCard.Id))
                        Plugin.medsCustomUnlocks.Add(medsCard.Id);
                    Plugin.medsCardsSource[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
                }
                catch (Exception err)
                {
                    Plugin.Log.LogInfo("ERROR LOADING CUSTOM CARD " + f.Name + ": " + err.Message);
                }
            }
            SortedDictionary<string, CardData> medsCardsTemp = new SortedDictionary<string, CardData>(Plugin.medsCardsSource);
            Plugin.medsCardsSource = new Dictionary<string, CardData>(medsCardsTemp);
            /*int a = 0;
            foreach (string key in Plugin.medsCardsSource.Keys)
            {
                medsCardsTemp[key] = medsCardsSource[a]
            }*/
            // do a second run to link AddCardList
            Plugin.Log.LogInfo("Loading custom card component: AddCardList...");
            foreach (string key in Plugin.medsSecondRunImport.Keys)
            {
                Plugin.medsCardsSource[key].AddCardList = new CardData[Plugin.medsSecondRunImport[key].Length];
                for (int a = 0; a < Plugin.medsSecondRunImport[key].Length; a++)
                {
                    if (Plugin.medsCardsSource.ContainsKey(Plugin.medsSecondRunImport[key][a]))
                        Plugin.medsCardsSource[key].AddCardList[a] = Plugin.medsCardsSource[Plugin.medsSecondRunImport[key][a]];
                    else
                        Plugin.medsCardsSource[key].AddCardList[a] = (CardData)null;
                }
            }
            // do a second run to link UpgradesToRare
            Plugin.Log.LogInfo("Loading custom card component: UpgradesToRare...");
            foreach (string key in Plugin.medsSecondRunImport2.Keys)
            {
                if (Plugin.medsCardsSource.ContainsKey(Plugin.medsSecondRunImport2[key]))
                    Plugin.medsCardsSource[key].UpgradesToRare = Plugin.medsCardsSource[Plugin.medsSecondRunImport2[key]];
            }
            /* #MISSINGCARDDATA #TODO
             data.PetFront = text.PetFront;
             data.PetInvert = text.PetInvert;
             data.PetModel = ""; // no clue, not worth it?
             data.PetOffset = ""; // no clue, not worth it?
             data.PetSize = ""; // no clue, not worth it?
             data.SummonUnit = ((UnityEngine.Object)text.SummonUnit != (UnityEngine.Object)null) ? text.SummonUnit.Id : ""; // maybe later :)
             data.SummonUnitNum = text.SummonUnitNum; // maybe later :)*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CardsSource").SetValue(Plugin.medsCardsSource);
            Plugin.Log.LogInfo("Cards loaded!");

            /*
             *    888888888888  88888888ba          db         88  888888888888  ad88888ba   
             *         88       88      "8b        d88b        88       88      d8"     "8b  
             *         88       88      ,8P       d8'`8b       88       88      Y8,          
             *         88       88aaaaaa8P'      d8'  `8b      88       88      `Y8aaaaa,    
             *         88       88""""88'       d8YaaaaY8b     88       88        `"""""8b,  
             *         88       88    `8b      d8""""""""8b    88       88              `8b  
             *         88       88     `8b    d8'        `8b   88       88      Y8a     a8P  
             *         88       88      `8b  d8'          `8b  88       88       "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading traits...");
            Dictionary<string, TraitData> medsTraitsCopy = new();
            // vanilla traits
            for (int index = 0; index < traitDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla trait: " + traitDataArray[index].TraitName); };
                traitDataArray[index].Init();
                Plugin.medsTraitsSource[traitDataArray[index].Id] = UnityEngine.Object.Instantiate<TraitData>(traitDataArray[index]);
                Plugin.medsTraitsSource[traitDataArray[index].Id].SetNameAndDescription();
                medsTraitsCopy[traitDataArray[index].Id] = UnityEngine.Object.Instantiate<TraitData>(Plugin.medsTraitsSource[traitDataArray[index].Id]);
            }
            // custom traits
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "trait"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom trait: " + f.Name); };
                TraitData medsTrait = DataTextConvert.ToData(JsonUtility.FromJson<TraitDataText>(File.ReadAllText(f.ToString())));
                Plugin.medsTraitsSource[medsTrait.Id] = UnityEngine.Object.Instantiate<TraitData>(medsTrait);
                medsTraitsCopy[medsTrait.Id] = UnityEngine.Object.Instantiate<TraitData>(Plugin.medsTraitsSource[medsTrait.Id]);
            }
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_TraitsSource").SetValue(Plugin.medsTraitsSource);
            Traverse.Create(Globals.Instance).Field("_Traits").SetValue(medsTraitsCopy);
            Plugin.Log.LogInfo("Traits loaded!");

            /*
             *    88        88  88888888888  88888888ba     ,ad8888ba,    88888888888  ad88888ba   
             *    88        88  88           88      "8b   d8"'    `"8b   88          d8"     "8b  
             *    88        88  88           88      ,8P  d8'        `8b  88          Y8,          
             *    88aaaaaaaa88  88aaaaa      88aaaaaa8P'  88          88  88aaaaa     `Y8aaaaa,    
             *    88""""""""88  88"""""     a 88""""88'    88          88  88"""""       `"""""8b,  
             *    88        88  88           88    `8b    Y8,        ,8P  88                  `8b  
             *    88        88  88           88     `8b    Y8a.    .a8P   88          Y8a     a8P  
             *    88        88  88888888888  88      `8b    `"Y8888Y"'    88888888888  "Y88888P"   
             * 
             *    I think these are actually classes? (not subclasses)
             */
            Plugin.Log.LogInfo("Loading vanilla heroes...");
            Dictionary<string, HeroData> medsHeroes = new();
            for (int index = 0; index < heroDataArray.Length; ++index)
                medsHeroes.Add(heroDataArray[index].Id, UnityEngine.Object.Instantiate<HeroData>(heroDataArray[index]));
            Traverse.Create(Globals.Instance).Field("_Heroes").SetValue(medsHeroes);
            Plugin.Log.LogInfo("Heroes loaded!");

            /*
             *    88888888ba   88888888888  88888888ba   88      a8P   ad88888ba   
             *    88      "8b  88           88      "8b  88    ,88'   d8"     "8b  
             *    88      ,8P  88           88      ,8P  88  ,88"     Y8,          
             *    88aaaaaa8P'  88aaaaa      88aaaaaa8P'  88,d88'      `Y8aaaaa,    
             *    88""""""'    88"""""      88""""88'    8888"88,       `"""""8b,  
             *    88           88           88    `8b    88P   Y8b            `8b  
             *    88           88           88     `8b   88     "88,  Y8a     a8P  
             *    88           88888888888  88      `8b  88       Y8b  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading perks...");
            // vanilla perks
            for (int index = 0; index < perkDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla perk: " + perkDataArray[index].Id); };
                perkDataArray[index].Init();
                Plugin.medsPerksSource[perkDataArray[index].Id] = UnityEngine.Object.Instantiate<PerkData>(perkDataArray[index]);
            }
            // custom perks
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "perk"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom perk: " + f.Name); };
                PerkData medsPerk = DataTextConvert.ToData(JsonUtility.FromJson<PerkDataText>(File.ReadAllText(f.ToString())));
                Plugin.medsPerksSource[medsPerk.Id] = UnityEngine.Object.Instantiate<PerkData>(medsPerk);
            }
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_PerksSource").SetValue(Plugin.medsPerksSource);
            Plugin.Log.LogInfo("Perks loaded!");



            /*
             *    88888888ba      db         ,ad8888ba,   88      a8P   88888888ba,         db    888888888888    db         
             *    88      "8b    d88b       d8"'    `"8b  88    ,88'    88      `"8b       d88b        88        d88b        
             *    88      ,8P   d8'`8b     d8'            88  ,88"      88        `8b     d8'`8b       88       d8'`8b       
             *    88aaaaaa8P'  d8'  `8b    88             88,d88'       88         88    d8'  `8b      88      d8'  `8b      
             *    88""""""'   d8YaaaaY8b   88             8888"88,      88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    88         d8""""""""8b  Y8,            88P   Y8b     88         8P  d8""""""""8b    88    d8""""""""8b    
             *    88        d8'        `8b  Y8a.    .a8P  88     "88,   88      .a8P  d8'        `8b   88   d8'        `8b   
             *    88       d8'          `8b  `"Y8888Y"'   88       Y8b  88888888Y"'  d8'          `8b  88  d8'          `8b  
             */
            Plugin.Log.LogInfo("Loading PackData...");
            // vanilla 
            for (int index = 0; index < packDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla PackData: " + packDataArray[index].PackId); };
                Plugin.medsPackDataSource[packDataArray[index].PackId.ToLower()] = UnityEngine.Object.Instantiate<PackData>(packDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_PackDataSource").SetValue(Plugin.medsPackDataSource);
            Plugin.Log.LogInfo("PackData loaded!");

            /*
             *     ad88888ba   88        88  88888888ba     ,ad8888ba,   88                  db         ad88888ba    ad88888ba   88888888888  ad88888ba   
             *    d8"     "8b  88        88  88      "8b   d8"'    `"8b  88                 d88b       d8"     "8b  d8"     "8b  88          d8"     "8b  
             *    Y8,          88        88  88      ,8P  d8'            88                d8'`8b      Y8,          Y8,          88          Y8,          
             *    `Y8aaaaa,    88        88  88aaaaaa8P'  88             88               d8'  `8b     `Y8aaaaa,    `Y8aaaaa,    88aaaaa     `Y8aaaaa,    
             *      `"""""8b,  88        88  88""""""8b,  88             88              d8YaaaaY8b      `"""""8b,    `"""""8b,  88"""""       `"""""8b,  
             *            `8b  88        88  88      `8b  Y8,            88             d8""""""""8b           `8b          `8b  88                  `8b  
             *    Y8a     a8P  Y8a.    .a8P  88      a8P   Y8a.    .a8P  88            d8'        `8b  Y8a     a8P  Y8a     a8P  88          Y8a     a8P  
             *     "Y88888P"    `"Y8888Y"'   88888888P"     `"Y8888Y"'   88888888888  d8'          `8b  "Y88888P"    "Y88888P"   88888888888  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading subclasses...");
            // vanilla subclasses
            for (int index = 0; index < subClassDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla subclass: " + subClassDataArray[index].SubClassName); };
                Plugin.medsSubClassesSource[subClassDataArray[index].SubClassName.Replace(" ", "").ToLower()] = UnityEngine.Object.Instantiate<SubClassData>(subClassDataArray[index]);
            }
            // custom subclasses
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "subclass"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom subclass: " + f.Name); };
                SubClassData medsSubClass = DataTextConvert.ToData(JsonUtility.FromJson<SubClassDataText>(File.ReadAllText(f.ToString())));
                Plugin.medsSubClassesSource[medsSubClass.Id] = UnityEngine.Object.Instantiate<SubClassData>(medsSubClass);
            }
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_SubClassSource").SetValue(Plugin.medsSubClassesSource);
            
            Plugin.Log.LogInfo("Subclasses loaded!");

            Plugin.Log.LogInfo("Creating subclass clones...");
            Dictionary<string, SubClassData> medsSubClassCopy = new();
            foreach (string key in Plugin.medsSubClassesSource.Keys)
            {
                medsSubClassCopy[key] = UnityEngine.Object.Instantiate<SubClassData>(Plugin.medsSubClassesSource[key]);
                medsSubClassCopy[key].CharacterName = Texts.Instance.GetText(key + "_name", "class");
                medsSubClassCopy[key].CharacterDescription = Texts.Instance.GetText(key + "_description", "class");
                medsSubClassCopy[key].CharacterDescriptionStrength = Texts.Instance.GetText(key + "_strength", "class");
            }
            Traverse.Create(Globals.Instance).Field("_SubClass").SetValue(medsSubClassCopy);
            Plugin.Log.LogInfo("Subclass clones created!");

            /*
             *    888b      88  88888888ba     ,ad8888ba,              
             *    8888b     88  88      "8b   d8"'    `"8b             
             *    88 `8b    88  88      ,8P  d8'                       
             *    88  `8b   88  88aaaaaa8P'  88             ,adPPYba,  
             *    88   `8b  88  88""""""'    88             I8[    ""  
             *    88    `8b 88  88           Y8,             `"Y8ba,   
             *    88     `8888  88            Y8a.    .a8P  aa    ]8I  
             *    88      `888  88             `"Y8888Y"'   `"YbbdP"'  
             */
            Plugin.Log.LogInfo("Loading NPCs...");
            // vanilla 
            for (int index = 0; index < npcDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla NPC: " + npcDataArray[index].Id); };
                Plugin.medsNPCsSource[npcDataArray[index].Id] = UnityEngine.Object.Instantiate<NPCData>(npcDataArray[index]);
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_NPCsSource").SetValue(Plugin.medsNPCsSource);
            Plugin.Log.LogInfo("NPCs loaded!");

            Plugin.Log.LogInfo("Creating NPC clones...");
            Dictionary<string, NPCData> medsNPCs = new();
            Dictionary<string, NPCData> medsNPCsNamed = new();
            SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
            foreach (string key in Plugin.medsNPCsSource.Keys)
            {
                if (!sortedDictionary.ContainsKey(key))
                    sortedDictionary.Add(key, Plugin.medsNPCsSource[key].NPCName);
                medsNPCs.Add(key, UnityEngine.Object.Instantiate<NPCData>(Plugin.medsNPCsSource[key]));
                string text1 = Texts.Instance.GetText(key + "_name", "monsters");
                if (text1 != "")
                    medsNPCs[key].NPCName = text1;
                if (Plugin.medsNPCsSource[key].IsNamed && Plugin.medsNPCsSource[key].Difficulty > -1)
                {
                    medsNPCsNamed.Add(key, UnityEngine.Object.Instantiate<NPCData>(Plugin.medsNPCsSource[key]));
                    string text2 = Texts.Instance.GetText(key + "_name", "monsters");
                    if (text2 != "")
                        medsNPCsNamed[key].NPCName = text2;
                }
            }
            Traverse.Create(Globals.Instance).Field("_NPCs").SetValue(medsNPCs);
            Traverse.Create(Globals.Instance).Field("_NPCsNamed").SetValue(medsNPCsNamed);
            Plugin.Log.LogInfo("NPC clones created!");

            /*
             *    88  888888888888  88888888888  88b           d88   ad88888ba   
             *    88       88       88           888b         d888  d8"     "8b  
             *    88       88       88           88`8b       d8'88  Y8,          
             *    88       88       88aaaaa      88 `8b     d8' 88  `Y8aaaaa,    
             *    88       88       88"""""      88  `8b   d8'  88    `"""""8b,  
             *    88       88       88           88   `8b d8'   88          `8b  
             *    88       88       88           88    `888'    88  Y8a     a8P  
             *    88       88       88888888888  88     `8'     88   "Y88888P"   
             *                                                                   
             *                                                                   
             */
            Plugin.Log.LogInfo("Loading item data...");
            // vanilla 
            for (int index = 0; index < itemDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla item data: " + itemDataArray[index].name); };
                itemDataArray[index].Id = itemDataArray[index].name.ToLower();
                if (Plugin.medsDropShop.Value && !Plugin.medsDoNotDropList.Contains(itemDataArray[index].Id))
                {
                    itemDataArray[index].DropOnly = false;
                    if (!Plugin.medsDropOnlyItems.Contains(itemDataArray[index].Id))
                        Plugin.medsDropOnlyItems.Add(itemDataArray[index].Id);
                }
                Plugin.medsItemDataSource[itemDataArray[index].Id] = UnityEngine.Object.Instantiate<ItemData>(itemDataArray[index]);
                if (Plugin.medsAllThePets.Value && Plugin.medsItemDataSource[itemDataArray[index].Id].QuestItem == true) { Plugin.medsItemDataSource[itemDataArray[index].Id].QuestItem = false; };
            }

            /*medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "item"))).GetFiles("*.json");
            Dictionary<string, ItemData> medsItemsCustom = new();
            foreach (FileInfo f in medsFI)
            {
                try
                {
                    if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom item: " + f.Name); };
                    ItemData medsItem = DataTextConvert.ToData(JsonUtility.FromJson<ItemDataText>(File.ReadAllText(f.ToString())));
                    if (!Plugin.medsCustomUnlocks.Contains(medsItem.Id))
                        Plugin.medsCustomUnlocks.Add(medsItem.Id);
                    Plugin.medsItemDataSource[medsItem.Id] = UnityEngine.Object.Instantiate<ItemData>(medsItem);
                }
                catch (Exception err)
                {
                    Plugin.Log.LogInfo("ERROR LOADING CUSTOM ITEM " + f.Name + ": " + err.Message);
                }
            }*/

            // second run through cards to connect items...
            Plugin.Log.LogInfo("Loading custom card component: Item...");
            foreach (string key in Plugin.medsCardsNeedingItems.Keys)
            {
                ItemData newItem = (ItemData)null;
                try
                {
                    newItem = DataTextConvert.ToData(JsonUtility.FromJson<ItemDataText>(Plugin.medsCardsNeedingItems[key]));
                    if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom item: " + newItem.Id); };
                    Plugin.medsItemDataSource[newItem.Id] = UnityEngine.Object.Instantiate<ItemData>(newItem);
                    Plugin.medsCardsSource[key].Item = Plugin.medsItemDataSource[newItem.Id];
                }
                catch
                {
                    Plugin.Log.LogError("ERROR LOADING CUSTOM ITEM FROM CARD: " + key);
                }
            }
            Plugin.Log.LogInfo("Loading custom card component: ItemEnchantment...");
            foreach (string key in Plugin.medsCardsNeedingItemEnchants.Keys)
            {
                ItemData newItem = (ItemData)null;
                try
                {
                    newItem = DataTextConvert.ToData(JsonUtility.FromJson<ItemDataText>(Plugin.medsCardsNeedingItemEnchants[key]));
                    if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom enchantment: " + newItem.Id); };
                    Plugin.medsItemDataSource[newItem.Id] = UnityEngine.Object.Instantiate<ItemData>(newItem);
                    Plugin.medsCardsSource[key].ItemEnchantment = Plugin.medsItemDataSource[newItem.Id];
                }
                catch
                {
                    Plugin.Log.LogError("ERROR LOADING CUSTOM ENCHANTMENT FROM CARD: " + key);
                }
            }


            if (Plugin.medsAllThePets.Value)
            {
                foreach (string key in Plugin.medsCardsSource.Keys)
                {
                    if (!(Plugin.medsCardsSource[key].ShowInTome) && Plugin.medsCardsSource[key].CardClass == Enums.CardClass.Item)
                    {
                        Plugin.medsCardsSource[key].ShowInTome = true;
                    }
                    if ((UnityEngine.Object)Plugin.medsCardsSource[key].Item != (UnityEngine.Object)null && Plugin.medsCardsSource[key].Item.QuestItem)
                    {
                        Plugin.medsCardsSource[key].Item.QuestItem = false;
                    }
                }
            } 

            Traverse.Create(Globals.Instance).Field("_ItemDataSource").SetValue(Plugin.medsItemDataSource);
            Plugin.Log.LogInfo("Item data loaded!");
            Traverse.Create(Globals.Instance).Field("_CardsSource").SetValue(Plugin.medsCardsSource);

            // setvalue cards

            Plugin.Log.LogInfo("Loading card clones...");
            Globals.Instance.CreateCardClones();
            // this.CreateAuraCurses(); I don't see why we can't do this earlier?


            /*
             *    88888888ba   88888888888  88888888ba   88      a8P   888b      88    ,ad8888ba,    88888888ba,    88888888888  ad88888ba   
             *    88      "8b  88           88      "8b  88    ,88'    8888b     88   d8"'    `"8b   88      `"8b   88          d8"     "8b  
             *    88      ,8P  88           88      ,8P  88  ,88"      88 `8b    88  d8'        `8b  88        `8b  88          Y8,          
             *    88aaaaaa8P'  88aaaaa      88aaaaaa8P'  88,d88'       88  `8b   88  88          88  88         88  88aaaaa     `Y8aaaaa,    
             *    88""""""'    88"""""      88""""88'    8888"88,      88   `8b  88  88          88  88         88  88"""""       `"""""8b,  
             *    88           88           88    `8b    88P   Y8b     88    `8b 88  Y8,        ,8P  88         8P  88                  `8b  
             *    88           88           88     `8b   88     "88,   88     `8888   Y8a.    .a8P   88      .a8P   88          Y8a     a8P  
             *    88           88888888888  88      `8b  88       Y8b  88      `888    `"Y8888Y"'    88888888Y"'    88888888888  "Y88888P"   
             */

            Plugin.Log.LogInfo("Loading perknodes...");
            // vanilla 
            for (int index = 0; index < perkNodeDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla perknode: " + perkNodeDataArray[index].Id); };
                Plugin.medsPerksNodesSource[perkNodeDataArray[index].Id] = UnityEngine.Object.Instantiate<PerkNodeData>(perkNodeDataArray[index]);
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_PerksNodesSource").SetValue(Plugin.medsPerksNodesSource);
            Plugin.Log.LogInfo("Perknodes loaded!");

            /*
             *    88888888888  8b           d8  88888888888  888b      88  888888888888  ad88888ba   
             *    88           `8b         d8'  88           8888b     88       88      d8"     "8b  
             *    88            `8b       d8'   88           88 `8b    88       88      Y8,          
             *    88aaaaa        `8b     d8'    88aaaaa      88  `8b   88       88      `Y8aaaaa,    
             *    88"""""         `8b   d8'     88"""""      88   `8b  88       88        `"""""8b,  
             *    88               `8b d8'      88           88    `8b 88       88              `8b  
             *    88                `888'       88           88     `8888       88      Y8a     a8P  
             *    88888888888        `8'        88888888888  88      `888       88       "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading events...");
            // vanilla 
            for (int index = 0; index < eventDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla event: " + eventDataArray[index].EventId); };
                EventData eventData = UnityEngine.Object.Instantiate<EventData>(eventDataArray[index]);
                eventData.Init();
                Plugin.medsEventDataSource[eventData.EventId.ToLower()] = eventData;
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_Events").SetValue(Plugin.medsEventDataSource);
            Plugin.Log.LogInfo("Events loaded!");

            /*
             *    88888888888  8b           d8  88888888888  888b      88  888888888888     88888888ba   88888888888  ,ad8888ba,     ad88888ba   
             *    88           `8b         d8'  88           8888b     88       88          88      "8b  88          d8"'    `"8b   d8"     "8b  
             *    88            `8b       d8'   88           88 `8b    88       88          88      ,8P  88         d8'        `8b  Y8,          
             *    88aaaaa        `8b     d8'    88aaaaa      88  `8b   88       88          88aaaaaa8P'  88aaaaa    88          88  `Y8aaaaa,    
             *    88"""""         `8b   d8'     88"""""      88   `8b  88       88          88""""88'    88"""""    88          88    `"""""8b,  
             *    88               `8b d8'      88           88    `8b 88       88          88    `8b    88         Y8,    "88,,8P          `8b  
             *    88                `888'       88           88     `8888       88          88     `8b   88          Y8a.    Y88P   Y8a     a8P  
             *    88888888888        `8'        88888888888  88      `888       88          88      `8b  88888888888  `"Y8888Y"Y8a   "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading event requirements...");
            // vanilla 
            for (int index = 0; index < eventRequirementDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla event requirement: " + eventRequirementDataArray[index].RequirementId); };
                string lower = eventRequirementDataArray[index].RequirementId.ToLower();
                Plugin.medsEventRequirementDataSource[lower] = UnityEngine.Object.Instantiate<EventRequirementData>(eventRequirementDataArray[index]);
                if (Plugin.medsEventRequirementDataSource[lower].ItemTrack || Plugin.medsEventRequirementDataSource[lower].RequirementTrack)
                {
                    string text3 = Texts.Instance.GetText(lower + "_name", "requirements");
                    if (text3 != "")
                        Plugin.medsEventRequirementDataSource[lower].RequirementName = text3;
                    string text4 = Texts.Instance.GetText(lower + "_description", "requirements");
                    if (text4 != "")
                        Plugin.medsEventRequirementDataSource[lower].Description = text4;
                }
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_Requirements").SetValue(Plugin.medsEventRequirementDataSource);
            Plugin.Log.LogInfo("Event requirements loaded!");

            /*
             *      ,ad8888ba,    ,ad8888ba,    88b           d88  88888888ba         db    888888888888     88888888ba,         db    888888888888    db         
             *     d8"'    `"8b  d8"'    `"8b   888b         d888  88      "8b       d88b        88          88      `"8b       d88b        88        d88b        
             *    d8'           d8'        `8b  88`8b       d8'88  88      ,8P      d8'`8b       88          88        `8b     d8'`8b       88       d8'`8b       
             *    88            88          88  88 `8b     d8' 88  88aaaaaa8P'     d8'  `8b      88          88         88    d8'  `8b      88      d8'  `8b      
             *    88            88          88  88  `8b   d8'  88  88""""""8b,    d8YaaaaY8b     88          88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    Y8,           Y8,        ,8P  88   `8b d8'   88  88      `8b   d8""""""""8b    88          88         8P  d8""""""""8b    88    d8""""""""8b    
             *     Y8a.    .a8P  Y8a.    .a8P   88    `888'    88  88      a8P  d8'        `8b   88          88      .a8P  d8'        `8b   88   d8'        `8b   
             *      `"Y8888Y"'    `"Y8888Y"'    88     `8'     88  88888888P"  d8'          `8b  88          88888888Y"'  d8'          `8b  88  d8'          `8b  
             */
            Plugin.Log.LogInfo("Loading combat data...");
            // vanilla 
            for (int index = 0; index < combatDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla combat data: " + combatDataArray[index].CombatId); };
                Plugin.medsCombatDataSource[combatDataArray[index].CombatId.Replace(" ", "").ToLower()] = UnityEngine.Object.Instantiate<CombatData>(combatDataArray[index]);
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CombatDataSource").SetValue(Plugin.medsCombatDataSource);
            Plugin.Log.LogInfo("Combat data loaded!");


            /*
             *    888b      88    ,ad8888ba,    88888888ba,    88888888888     88888888ba,         db    888888888888    db         
             *    8888b     88   d8"'    `"8b   88      `"8b   88              88      `"8b       d88b        88        d88b        
             *    88 `8b    88  d8'        `8b  88        `8b  88              88        `8b     d8'`8b       88       d8'`8b       
             *    88  `8b   88  88          88  88         88  88aaaaa         88         88    d8'  `8b      88      d8'  `8b      
             *    88   `8b  88  88          88  88         88  88"""""         88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    88    `8b 88  Y8,        ,8P  88         8P  88              88         8P  d8""""""""8b    88    d8""""""""8b    
             *    88     `8888   Y8a.    .a8P   88      .a8P   88              88      .a8P  d8'        `8b   88   d8'        `8b   
             *    88      `888    `"Y8888Y"'    88888888Y"'    88888888888     88888888Y"'  d8'          `8b  88  d8'          `8b  
             */
            Plugin.Log.LogInfo("Loading node data...");
            // vanilla 
            for (int index = 0; index < nodeDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla node data: " + nodeDataArray[index].NodeId); };
                string lower = nodeDataArray[index].NodeId.ToLower();
                Plugin.medsNodeDataSource[lower] = UnityEngine.Object.Instantiate<NodeData>(nodeDataArray[index]);
                Plugin.medsNodeDataSource[lower].NodeName = Texts.Instance.GetText(Plugin.medsNodeDataSource[lower].NodeId + "_name", "nodes");
                Plugin.medsNodeCombatEventRelation[lower] = lower;
                for (int index4 = 0; index4 < nodeDataArray[index].NodeCombat.Length; ++index4)
                {
                    if ((UnityEngine.Object)nodeDataArray[index].NodeCombat[index4] != (UnityEngine.Object)null && !Plugin.medsNodeCombatEventRelation.ContainsKey(nodeDataArray[index].NodeCombat[index4].CombatId))
                        Plugin.medsNodeCombatEventRelation.Add(nodeDataArray[index].NodeCombat[index4].CombatId, lower);
                }
                for (int index5 = 0; index5 < nodeDataArray[index].NodeEvent.Length; ++index5)
                {
                    if ((UnityEngine.Object)nodeDataArray[index].NodeEvent[index5] != (UnityEngine.Object)null && !Plugin.medsNodeCombatEventRelation.ContainsKey(nodeDataArray[index].NodeEvent[index5].EventId))
                        Plugin.medsNodeCombatEventRelation.Add(nodeDataArray[index].NodeEvent[index5].EventId, lower);
                }
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_NodeDataSource").SetValue(Plugin.medsNodeDataSource);
            Traverse.Create(Globals.Instance).Field("_NodeCombatEventRelation").SetValue(Plugin.medsNodeCombatEventRelation);
            Plugin.Log.LogInfo("Node data loaded!");

            /*
             *    88888888ba      db         88  88888888ba    ad88888ba      88888888ba      db         ,ad8888ba,   88      a8P   ad88888ba   
             *    88      "8b    d88b        88  88      "8b  d8"     "8b     88      "8b    d88b       d8"'    `"8b  88    ,88'   d8"     "8b  
             *    88      ,8P   d8'`8b       88  88      ,8P  Y8,             88      ,8P   d8'`8b     d8'            88  ,88"     Y8,          
             *    88aaaaaa8P'  d8'  `8b      88  88aaaaaa8P'  `Y8aaaaa,       88aaaaaa8P'  d8'  `8b    88             88,d88'      `Y8aaaaa,    
             *    88""""""'   d8YaaaaY8b     88  88""""88'      `"""""8b,     88""""""'   d8YaaaaY8b   88             8888"88,       `"""""8b,  
             *    88         d8""""""""8b    88  88    `8b            `8b     88         d8""""""""8b  Y8,            88P   Y8b            `8b  
             *    88        d8'        `8b   88  88     `8b   Y8a     a8P     88        d8'        `8b  Y8a.    .a8P  88     "88,  Y8a     a8P  
             *    88       d8'          `8b  88  88      `8b   "Y88888P"      88       d8'          `8b  `"Y8888Y"'   88       Y8b  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading pairs packs...");
            // vanilla 
            for (int index = 0; index < playerPairsPackDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla pairs pack data: " + playerPairsPackDataArray[index].PackId); };
                Plugin.medsCardPlayerPairsPackDataSource[playerPairsPackDataArray[index].PackId.ToLower()] = UnityEngine.Object.Instantiate<CardPlayerPairsPackData>(playerPairsPackDataArray[index]);
            }
            // custom #TODO
            //
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CardPlayerPairsPackDataSource").SetValue(Plugin.medsCardPlayerPairsPackDataSource);
            Plugin.Log.LogInfo("Pairs packs loaded!");


            /*
             *    88           ,ad8888ba,      ,ad8888ba,  888888888888  88888888ba,         db    888888888888    db         
             *    88          d8"'    `"8b    d8"'    `"8b      88       88      `"8b       d88b        88        d88b        
             *    88         d8'        `8b  d8'        `8b     88       88        `8b     d8'`8b       88       d8'`8b       
             *    88         88          88  88          88     88       88         88    d8'  `8b      88      d8'  `8b      
             *    88         88          88  88          88     88       88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    88         Y8,        ,8P  Y8,        ,8P     88       88         8P  d8""""""""8b    88    d8""""""""8b    
             *    88          Y8a.    .a8P    Y8a.    .a8P      88       88      .a8P  d8'        `8b   88   d8'        `8b   
             *    88888888888  `"Y8888Y"'      `"Y8888Y"'       88       88888888Y"'  d8'          `8b  88  d8'          `8b  
             */

            /*
             *    888888888888  88  88888888888  88888888ba         88888888ba   88888888888  I8,        8        ,8I    db         88888888ba   88888888ba,     ad88888ba   
             *         88       88  88           88      "8b        88      "8b  88           `8b       d8b       d8'   d88b        88      "8b  88      `"8b   d8"     "8b  
             *         88       88  88           88      ,8P        88      ,8P  88            "8,     ,8"8,     ,8"   d8'`8b       88      ,8P  88        `8b  Y8,          
             *         88       88  88aaaaa      88aaaaaa8P'        88aaaaaa8P'  88aaaaa        Y8     8P Y8     8P   d8'  `8b      88aaaaaa8P'  88         88  Y8aaaaa,    
             *         88       88  88"""""      88""""88'          88""""88'    88"""""        `8b   d8' `8b   d8'  d8YaaaaY8b     88""""88'    88         88   `"""""8b,  
             *         88       88  88           88    `8b          88    `8b    88              `8a a8'   `8a a8'  d8""""""""8b    88    `8b    88         8P          `8b  
             *         88       88  88           88     `8b         88     `8b   88               `8a8'     `8a8'  d8'        `8b   88     `8b   88      .a8P   Y8a     a8P  
             *         88       88  88888888888  88      `8b        88      `8b  88888888888       `8'       `8'  d8'          `8b  88      `8b  88888888Y"'     "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading tier reward data...");
            // vanilla 
            for (int index = 0; index < tierRewardDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla tier reward data: " + tierRewardDataArray[index].name); };
                Plugin.medsTierRewardDataSource[tierRewardDataArray[index].TierNum] = UnityEngine.Object.Instantiate<TierRewardData>(tierRewardDataArray[index]);
            }
            /*/ custom #TODO
            medsFI = (new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", "card"))).GetFiles("*.json");
            foreach (FileInfo f in medsFI)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading custom : " + f.Name); };
                CardData medsCard = DataTextConvert.ToData(JsonUtility.FromJson<CardDataText>(File.ReadAllText(f.ToString())));
                medsCards[medsCard.Id] = UnityEngine.Object.Instantiate<CardData>(medsCard);
            }*/
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_TierRewardDataSource").SetValue(Plugin.medsTierRewardDataSource);
            Plugin.Log.LogInfo("Tier reward data loaded!");


            /*
             *    88           ,ad8888ba,      ,ad8888ba,  888888888888  88888888ba,         db    888888888888    db         
             *    88          d8"'    `"8b    d8"'    `"8b      88       88      `"8b       d88b        88        d88b        
             *    88         d8'        `8b  d8'        `8b     88       88        `8b     d8'`8b       88       d8'`8b       
             *    88         88          88  88          88     88       88         88    d8'  `8b      88      d8'  `8b      
             *    88         88          88  88          88     88       88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    88         Y8,        ,8P  Y8,        ,8P     88       88         8P  d8""""""""8b    88    d8""""""""8b    
             *    88          Y8a.    .a8P    Y8a.    .a8P      88       88      .a8P  d8'        `8b   88   d8'        `8b   
             *    88888888888  `"Y8888Y"'      `"Y8888Y"'       88       88888888Y"'  d8'          `8b  88  d8'          `8b  
             */
            Plugin.Log.LogInfo("Loading LootData...");
            // vanilla 
            for (int index = 0; index < lootDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla LootData: " + lootDataArray[index].Id); };
                Plugin.medsLootDataSource[lootDataArray[index].Id.ToLower()] = UnityEngine.Object.Instantiate<LootData>(lootDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_LootDataSource").SetValue(Plugin.medsLootDataSource);
            Plugin.Log.LogInfo("LootData loaded!");

            /*
             *     ad88888ba   88      a8P   88  888b      88   ad88888ba   
             *    d8"     "8b  88    ,88'    88  8888b     88  d8"     "8b  
             *    Y8,          88  ,88"      88  88 `8b    88  Y8,          
             *    `Y8aaaaa,    88,d88'       88  88  `8b   88  `Y8aaaaa,    
             *      `"""""8b,  8888"88,      88  88   `8b  88    `"""""8b,  
             *            `8b  88P   Y8b     88  88    `8b 88          `8b  
             *    Y8a     a8P  88     "88,   88  88     `8888  Y8a     a8P  
             *     "Y88888P"   88       Y8b  88  88      `888   "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading SkinData...");
            // vanilla 
            for (int index = 0; index < skinDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla SkinData: " + skinDataArray[index].SkinId); };
                Plugin.medsSkinsSource[skinDataArray[index].SkinId.ToLower()] = UnityEngine.Object.Instantiate<SkinData>(skinDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_SkinDataSource").SetValue(Plugin.medsSkinsSource);
            Plugin.Log.LogInfo("SkinData loaded!");


            /*
             *      ,ad8888ba,         db         88888888ba   88888888ba,    88888888ba         db         ,ad8888ba,   88      a8P   ad88888ba   
             *     d8"'    `"8b       d88b        88      "8b  88      `"8b   88      "8b       d88b       d8"'    `"8b  88    ,88'   d8"     "8b  
             *    d8'                d8'`8b       88      ,8P  88        `8b  88      ,8P      d8'`8b     d8'            88  ,88"     Y8,          
             *    88                d8'  `8b      88aaaaaa8P'  88         88  88aaaaaa8P'     d8'  `8b    88             88,d88'      `Y8aaaaa,    
             *    88               d8YaaaaY8b     88""""88'    88         88  88""""""8b,    d8YaaaaY8b   88             8888"88,       `"""""8b,  
             *    Y8,             d8""""""""8b    88    `8b    88         8P  88      `8b   d8""""""""8b  Y8,            88P   Y8b            `8b  
             *     Y8a.    .a8P  d8'        `8b   88     `8b   88      .a8P   88      a8P  d8'        `8b  Y8a.    .a8P  88     "88,  Y8a     a8P  
             *      `"Y8888Y"'  d8'          `8b  88      `8b  88888888Y"'    88888888P"  d8'          `8b  `"Y8888Y"'   88       Y8b  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading CardbackData...");
            // vanilla 
            for (int index = 0; index < cardbackDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla CardbackData: " + cardbackDataArray[index].CardbackId); };
                Plugin.medsCardbacksSource[cardbackDataArray[index].CardbackId.ToLower()] = UnityEngine.Object.Instantiate<CardbackData>(cardbackDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CardbackDataSource").SetValue(Plugin.medsCardbacksSource);
            Plugin.Log.LogInfo("CardbackData loaded!");


            /*
             *      ,ad8888ba,    ,ad8888ba,    88888888ba   88888888ba        88888888ba      db         ,ad8888ba,   88      a8P   88888888ba,         db    888888888888    db         
             *     d8"'    `"8b  d8"'    `"8b   88      "8b  88      "8b       88      "8b    d88b       d8"'    `"8b  88    ,88'    88      `"8b       d88b        88        d88b        
             *    d8'           d8'        `8b  88      ,8P  88      ,8P       88      ,8P   d8'`8b     d8'            88  ,88"      88        `8b     d8'`8b       88       d8'`8b       
             *    88            88          88  88aaaaaa8P'  88aaaaaa8P'       88aaaaaa8P'  d8'  `8b    88             88,d88'       88         88    d8'  `8b      88      d8'  `8b      
             *    88            88          88  88""""88'    88""""88'         88""""""'   d8YaaaaY8b   88             8888"88,      88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    Y8,           Y8,        ,8P  88    `8b    88    `8b         88         d8""""""""8b  Y8,            88P   Y8b     88         8P  d8""""""""8b    88    d8""""""""8b    
             *     Y8a.    .a8P  Y8a.    .a8P   88     `8b   88     `8b   888  88        d8'        `8b  Y8a.    .a8P  88     "88,   88      .a8P  d8'        `8b   88   d8'        `8b   
             *      `"Y8888Y"'    `"Y8888Y"'    88      `8b  88      `8b  888  88       d8'          `8b  `"Y8888Y"'   88       Y8b  88888888Y"'  d8'          `8b  88  d8'          `8b  
             */
            Plugin.Log.LogInfo("Loading CorruptionPackData...");
            // vanilla 
            for (int index = 0; index < corruptionPackDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla CorruptionPackData: " + corruptionPackDataArray[index].name); };
                Plugin.medsCorruptionPackDataSource[corruptionPackDataArray[index].name] = UnityEngine.Object.Instantiate<CorruptionPackData>(corruptionPackDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CorruptionPackDataSource").SetValue(Plugin.medsCorruptionPackDataSource);
            Plugin.Log.LogInfo("CorruptionPackData loaded!");


            /*
             *      ,ad8888ba,         db         88888888ba   88888888ba,    88888888ba   88                  db    8b        d8  88888888888  88888888ba   88888888ba      db         ,ad8888ba,   88      a8P   
             *     d8"'    `"8b       d88b        88      "8b  88      `"8b   88      "8b  88                 d88b    Y8,    ,8P   88           88      "8b  88      "8b    d88b       d8"'    `"8b  88    ,88'    
             *    d8'                d8'`8b       88      ,8P  88        `8b  88      ,8P  88                d8'`8b    Y8,  ,8P    88           88      ,8P  88      ,8P   d8'`8b     d8'            88  ,88"      
             *    88                d8'  `8b      88aaaaaa8P'  88         88  88aaaaaa8P'  88               d8'  `8b    "8aa8"     88aaaaa      88aaaaaa8P'  88aaaaaa8P'  d8'  `8b    88             88,d88'       
             *    88               d8YaaaaY8b     88""""88'    88         88  88""""""'    88              d8YaaaaY8b    `88'      88"""""      88""""88'    88""""""'   d8YaaaaY8b   88             8888"88,      
             *    Y8,             d8""""""""8b    88    `8b    88         8P  88           88             d8""""""""8b    88       88           88    `8b    88         d8""""""""8b  Y8,            88P   Y8b     
             *     Y8a.    .a8P  d8'        `8b   88     `8b   88      .a8P   88           88            d8'        `8b   88       88           88     `8b   88        d8'        `8b  Y8a.    .a8P  88     "88,   
             *      `"Y8888Y"'  d8'          `8b  88      `8b  88888888Y"'    88           88888888888  d8'          `8b  88       88888888888  88      `8b  88       d8'          `8b  `"Y8888Y"'   88       Y8b  
             */
            Plugin.Log.LogInfo("Loading CardPlayerPackData...");
            // vanilla 
            for (int index = 0; index < cardPlayerPackDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla CardPlayerPackData: " + cardPlayerPackDataArray[index].PackId); };
                Plugin.medsCardPlayerPackDataSource[cardPlayerPackDataArray[index].PackId.ToLower()] = UnityEngine.Object.Instantiate<CardPlayerPackData>(cardPlayerPackDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_CardPlayerPackDataSource").SetValue(Plugin.medsCardPlayerPackDataSource);
            Plugin.Log.LogInfo("CardPlayerPackData loaded!");


            /*
             *      ,ad8888ba,   88  888b      88  88888888888  88b           d88         db    888888888888  88    ,ad8888ba,    ad88888ba   
             *     d8"'    `"8b  88  8888b     88  88           888b         d888        d88b        88       88   d8"'    `"8b  d8"     "8b  
             *    d8'            88  88 `8b    88  88           88`8b       d8'88       d8'`8b       88       88  d8'            Y8,          
             *    88             88  88  `8b   88  88aaaaa      88 `8b     d8' 88      d8'  `8b      88       88  88             `Y8aaaaa,    
             *    88             88  88   `8b  88  88"""""      88  `8b   d8'  88     d8YaaaaY8b     88       88  88               `"""""8b,  
             *    Y8,            88  88    `8b 88  88           88   `8b d8'   88    d8""""""""8b    88       88  Y8,                    `8b  
             *     Y8a.    .a8P  88  88     `8888  88           88    `888'    88   d8'        `8b   88       88   Y8a.    .a8P  Y8a     a8P  
             *      `"Y8888Y"'   88  88      `888  88888888888  88     `8'     88  d8'          `8b  88       88    `"Y8888Y"'    "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading cinematic data...");
            // vanilla 
            for (int index = 0; index < cinematicDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading cinematic data: " + cinematicDataArray[index].CinematicId); };
                Plugin.medsCinematicDataSource[cinematicDataArray[index].CinematicId.Replace(" ", "").ToLower()] = UnityEngine.Object.Instantiate<CinematicData>(cinematicDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_Cinematics").SetValue(Plugin.medsCinematicDataSource);
            Plugin.Log.LogInfo("Cinematic data loaded!");

            /*
             *    888888888888   ,ad8888ba,    888b      88  88888888888  ad88888ba   
             *             ,88  d8"'    `"8b   8888b     88  88          d8"     "8b  
             *           ,88"  d8'        `8b  88 `8b    88  88          Y8,          
             *         ,88"    88          88  88  `8b   88  88aaaaa     `Y8aaaaa,    
             *       ,88"      88          88  88   `8b  88  88"""""       `"""""8b,  
             *     ,88"        Y8,        ,8P  88    `8b 88  88                  `8b  
             *    88"           Y8a.    .a8P   88     `8888  88          Y8a     a8P  
             *    888888888888   `"Y8888Y"'    88      `888  88888888888  "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading zone data...");
            // vanilla 
            for (int index = 0; index < zoneDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla zone data: " + zoneDataArray[index].ZoneId); };
                Plugin.medsZoneDataSource[zoneDataArray[index].ZoneId.ToLower()] = UnityEngine.Object.Instantiate<ZoneData>(zoneDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_ZoneDataSource").SetValue(Plugin.medsZoneDataSource);
            Plugin.Log.LogInfo("Zone data loaded!");

            /*
             *      ,ad8888ba,   88        88         db         88           88                      888888888888  88888888ba          db         88  888888888888  ad88888ba   
             *     d8"'    `"8b  88        88        d88b        88           88                           88       88      "8b        d88b        88       88      d8"     "8b  
             *    d8'            88        88       d8'`8b       88           88                           88       88      ,8P       d8'`8b       88       88      Y8,          
             *    88             88aaaaaaaa88      d8'  `8b      88           88                           88       88aaaaaa8P'      d8'  `8b      88       88      `Y8aaaaa,    
             *    88             88""""""""88     d8YaaaaY8b     88           88                           88       88""""88'       d8YaaaaY8b     88       88        `"""""8b,  
             *    Y8,            88        88    d8""""""""8b    88           88                           88       88    `8b      d8""""""""8b    88       88              `8b  
             *     Y8a.    .a8P  88        88   d8'        `8b   88           88           888             88       88     `8b    d8'        `8b   88       88      Y8a     a8P  
             *      `"Y8888Y"'   88        88  d8'          `8b  88888888888  88888888888  888             88       88      `8b  d8'          `8b  88       88       "Y88888P"   
             */
            Plugin.Log.LogInfo("Loading challenge traits...");
            // vanilla 
            for (int index = 0; index < challengeTraitArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla challenge trait: " + challengeTraitArray[index].Id); };
                Plugin.medsChallengeTraitsSource[challengeTraitArray[index].Id.ToLower()] = UnityEngine.Object.Instantiate<ChallengeTrait>(challengeTraitArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_ChallengeTraitsSource").SetValue(Plugin.medsChallengeTraitsSource);
            Plugin.Log.LogInfo("Challenge traits loaded!");

            /*
             *      ,ad8888ba,   88        88         db         88           88                      88888888ba,         db    888888888888    db         
             *     d8"'    `"8b  88        88        d88b        88           88                      88      `"8b       d88b        88        d88b        
             *    d8'            88        88       d8'`8b       88           88                      88        `8b     d8'`8b       88       d8'`8b       
             *    88             88aaaaaaaa88      d8'  `8b      88           88                      88         88    d8'  `8b      88      d8'  `8b      
             *    88             88""""""""88     d8YaaaaY8b     88           88                      88         88   d8YaaaaY8b     88     d8YaaaaY8b     
             *    Y8,            88        88    d8""""""""8b    88           88                      88         8P  d8""""""""8b    88    d8""""""""8b    
             *     Y8a.    .a8P  88        88   d8'        `8b   88           88           888        88      .a8P  d8'        `8b   88   d8'        `8b   
             *      `"Y8888Y"'   88        88  d8'          `8b  88888888888  88888888888  888        88888888Y"'  d8'          `8b  88  d8'          `8b  
             */

            Plugin.Log.LogInfo("Loading challenge data...");
            // vanilla 
            for (int index = 0; index < challengeDataArray.Length; ++index)
            {
                if (Plugin.medsVerbose.Value) { Plugin.Log.LogInfo("Loading vanilla challenge data: " + challengeDataArray[index].Id); };
                Plugin.medsChallengeDataSource[challengeDataArray[index].Id.ToLower()] = UnityEngine.Object.Instantiate<ChallengeData>(challengeDataArray[index]);
            }
            /*/ custom #TODO */
            // save vanilla+custom
            Traverse.Create(Globals.Instance).Field("_WeeklyDataSource").SetValue(Plugin.medsChallengeDataSource);
            Plugin.Log.LogInfo("Challenge data loaded!");
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(HeroSelectionManager), "Start")]
        public static bool HSMStartPrefix(ref HeroSelectionManager __instance)
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
            if (Plugin.medsCustomContent.Value || (Plugin.IsHost() ? Plugin.medsDLCClones.Value : Plugin.medsMPDLCClones))
            {
                // replace StartCo with your own... :)
                photonView = Traverse.Create(__instance).Field("photonView").GetValue<PhotonView>();
                ngValueMaster = Traverse.Create(__instance).Field("ngValueMaster").GetValue<int>();
                ngValue = Traverse.Create(__instance).Field("ngValue").GetValue<int>();
                ngCorruptors = Traverse.Create(__instance).Field("ngCorruptors").GetValue<string>();
                obeliskMadnessValue = Traverse.Create(__instance).Field("obeliskMadnessValue").GetValue<int>();
                obeliskMadnessValueMaster = Traverse.Create(__instance).Field("obeliskMadnessValueMaster").GetValue<int>();
                boxSelection = Traverse.Create(__instance).Field("boxSelection").GetValue<BoxSelection[]>();
                boxHero = Traverse.Create(__instance).Field("boxHero").GetValue< Dictionary<GameObject, HeroSelection>>();
                boxFilled = Traverse.Create(__instance).Field("boxFilled").GetValue<Dictionary<GameObject, bool>>();
                subclassDictionary = Traverse.Create(__instance).Field("subclassDictionary").GetValue<Dictionary<string, SubClassData[]>>();
                nonHistorySubclassDictionary = Traverse.Create(__instance).Field("nonHistorySubclassDictionary").GetValue<Dictionary<string, SubClassData>>();
                SubclassByName = Traverse.Create(__instance).Field("SubclassByName").GetValue<Dictionary<string, string>>();
                __instance.StartCoroutine(medsHeroSelectionStartCo());
                return false;
            }
            return true;
        }


        private static IEnumerator medsHeroSelectionStartCo()
        {
            ngValueMaster = ngValue = 0;
            ngCorruptors = "";
            obeliskMadnessValue = obeliskMadnessValueMaster = 0;
            Traverse.Create(HeroSelectionManager.Instance).Field("ngValueMaster").SetValue(ngValueMaster);
            Traverse.Create(HeroSelectionManager.Instance).Field("ngValue").SetValue(ngValue);
            Traverse.Create(HeroSelectionManager.Instance).Field("ngCorruptors").SetValue(ngCorruptors);
            Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValue").SetValue(obeliskMadnessValue);
            Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValueMaster").SetValue(obeliskMadnessValueMaster);
            HeroSelectionManager.Instance.madnessLevel.text = string.Format(Texts.Instance.GetText("madnessNumber"), (object)0);
            if (GameManager.Instance.IsMultiplayer())
            {
                Debug.Log((object)"WaitingSyncro heroSelection");
                if (NetworkManager.Instance.IsMaster())
                {
                    NetworkManager.Instance.PlayerSkuList.Clear();
                    while (!NetworkManager.Instance.AllPlayersReady("heroSelection"))
                        yield return (object)Globals.Instance.WaitForSeconds(0.01f);
                    if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("Game ready, Everybody checked heroSelection");
                    if (GameManager.Instance.IsLoadingGame())
                        photonView.RPC("NET_SetLoadingGame", RpcTarget.Others);
                    NetworkManager.Instance.PlayersNetworkContinue("heroSelection", AtOManager.Instance.GetWeekly().ToString());
                    yield return (object)Globals.Instance.WaitForSeconds(0.1f);
                }
                else
                {
                    GameManager.Instance.SetGameStatus(Enums.GameStatus.NewGame);
                    NetworkManager.Instance.SetWaitingSyncro("heroSelection", true);
                    NetworkManager.Instance.SetStatusReady("heroSelection");
                    while (NetworkManager.Instance.WaitingSyncro["heroSelection"])
                        yield return (object)Globals.Instance.WaitForSeconds(0.01f);
                    if (NetworkManager.Instance.netAuxValue != "")
                        AtOManager.Instance.SetWeekly(int.Parse(NetworkManager.Instance.netAuxValue));
                    if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("heroSelection, we can continue!");
                }
            }
            if (GameManager.Instance.IsMultiplayer())
            {
                List<string> stringList = new List<string>();
                for (int index = 0; index < Globals.Instance.SkuAvailable.Count; ++index)
                {
                    if (SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[index]))
                        stringList.Add(Globals.Instance.SkuAvailable[index]);
                }
                string str = "";
                if (stringList.Count > 0)
                    str = JsonHelper.ToJson<string>(stringList.ToArray());
                if (NetworkManager.Instance.IsMaster())
                {
                    photonView.RPC("NET_SetSku", RpcTarget.All, (object)NetworkManager.Instance.GetPlayerNick(), (object)str);
                }
                else
                {
                    string roomName = NetworkManager.Instance.GetRoomName();
                    if (roomName != "")
                    {
                        SaveManager.SaveIntoPrefsString("coopRoomId", roomName);
                        SaveManager.SavePrefs();
                    }
                    NetworkManager.Instance.SetWaitingSyncro("skuWait", true);
                    photonView.RPC("NET_SetSku", RpcTarget.All, (object)NetworkManager.Instance.GetPlayerNick(), (object)str);
                }
                Debug.Log((object)"WaitingSyncro skuWait");
                if (NetworkManager.Instance.IsMaster())
                {
                    while (!NetworkManager.Instance.AllPlayersHaveSkuList())
                        yield return (object)Globals.Instance.WaitForSeconds(0.01f);
                    if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("Game ready, Everybody checked skuWait");
                    NetworkManager.Instance.PlayersNetworkContinue("skuWait");
                    yield return (object)Globals.Instance.WaitForSeconds(0.1f);
                }
                else
                {
                    while (NetworkManager.Instance.WaitingSyncro["skuWait"])
                        yield return (object)Globals.Instance.WaitForSeconds(0.01f);
                    if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("skuWait, we can continue!");
                }
            }
            // Plugin.Log.LogDebug("about to show madness");
            MadnessManager.Instance.ShowMadness();
            MadnessManager.Instance.RefreshValues();
            MadnessManager.Instance.ShowMadness();
            HeroSelectionManager.Instance.playerHeroSkinsDict = new Dictionary<string, string>();
            HeroSelectionManager.Instance.playerHeroCardbackDict = new Dictionary<string, string>();
            boxSelection = new BoxSelection[HeroSelectionManager.Instance.boxGO.Length];
            for (int index = 0; index < HeroSelectionManager.Instance.boxGO.Length; ++index)
            {
                boxHero[HeroSelectionManager.Instance.boxGO[index]] = (HeroSelection)null;
                boxFilled[HeroSelectionManager.Instance.boxGO[index]] = false;
                boxSelection[index] = HeroSelectionManager.Instance.boxGO[index].GetComponent<BoxSelection>();
            }
            Traverse.Create(HeroSelectionManager.Instance).Field("boxSelection").SetValue(boxSelection);
            Traverse.Create(HeroSelectionManager.Instance).Field("boxFilled").SetValue(boxFilled);
            Traverse.Create(HeroSelectionManager.Instance).Field("boxHero").SetValue(boxHero);
            HeroSelectionManager.Instance.ShowDrag(false, Vector3.zero);
            // Plugin.Log.LogDebug("about to begin looping through subclasses");
            foreach (KeyValuePair<string, SubClassData> keyValuePair in Globals.Instance.SubClass)
            {
                if (!keyValuePair.Value.MainCharacter)
                {
                    if (!nonHistorySubclassDictionary.ContainsKey(keyValuePair.Key))
                        nonHistorySubclassDictionary.Add(keyValuePair.Key, Globals.Instance.SubClass[keyValuePair.Key]);
                }
                else if (keyValuePair.Value.ExpansionCharacter)
                {
                    string key = "dlc";
                    // wouldn't everything just be SO much easier if the subclassdictionary was composed of string, List<string> pairs instead?
                    if (!subclassDictionary.ContainsKey(key))
                        subclassDictionary.Add(key, new SubClassData[4]);
                    if (Globals.Instance.SubClass[keyValuePair.Key].OrderInList >= subclassDictionary[key].Length)
                    {
                        SubClassData[] tempSCD = new SubClassData[Globals.Instance.SubClass[keyValuePair.Key].OrderInList + 1];
                        for (int a = 0; a < subclassDictionary[key].Length; a++)
                        {
                            // Plugin.Log.LogDebug("loop 1." + a);
                            if ((UnityEngine.Object)subclassDictionary[key][a] != (UnityEngine.Object)null)
                                tempSCD[a] = subclassDictionary[key][a];
                        }
                        subclassDictionary[key] = tempSCD;
                    }
                    subclassDictionary[key][Globals.Instance.SubClass[keyValuePair.Key].OrderInList] = Globals.Instance.SubClass[keyValuePair.Key];
                }
                else
                {
                    string key = Enum.GetName(typeof(Enums.HeroClass), (object)Globals.Instance.SubClass[keyValuePair.Key].HeroClass).ToLower().Replace(" ", "");
                    if (!subclassDictionary.ContainsKey(key))
                        subclassDictionary.Add(key, new SubClassData[4]);
                    if (Globals.Instance.SubClass[keyValuePair.Key].OrderInList >= subclassDictionary[key].Length)
                    {
                        SubClassData[] tempSCD = new SubClassData[Globals.Instance.SubClass[keyValuePair.Key].OrderInList + 1];
                        // Plugin.Log.LogDebug("SCDict length: " + subclassDictionary[key].Length + "\ntempSCD length: " + tempSCD.Length);
                        for (int a = 0; a < subclassDictionary[key].Length; a++)
                        {
                            // Plugin.Log.LogDebug("loop 2." + a);
                            if ((UnityEngine.Object)subclassDictionary[key][a] != (UnityEngine.Object)null)
                            {
                                tempSCD[a] = subclassDictionary[key][a];
                                // Plugin.Log.LogDebug("adding subclass " + subclassDictionary[key][a]);
                            }
                        }
                        Plugin.Log.LogDebug("made it through the loop! original length: ");
                        subclassDictionary[key] = tempSCD;
                    }
                    // Plugin.Log.LogDebug("made it ALL THE WAY through the loop!");
                    // Plugin.Log.LogDebug("NEWLEN: " + subclassDictionary[key].Length);
                    // Plugin.Log.LogDebug(Globals.Instance.SubClass[keyValuePair.Key]);
                    // Plugin.Log.LogDebug("NEWOIL: " + Globals.Instance.SubClass[keyValuePair.Key].OrderInList);

                    subclassDictionary[key][Globals.Instance.SubClass[keyValuePair.Key].OrderInList] = Globals.Instance.SubClass[keyValuePair.Key];
                    // Plugin.Log.LogDebug("NEWOIL: " + Globals.Instance.SubClass[keyValuePair.Key].OrderInList);
                }
                // Plugin.Log.LogDebug("end of subclass loop!");
            }
            // Plugin.Log.LogDebug("finished looping through subclasses");
            Traverse.Create(HeroSelectionManager.Instance).Field("nonHistorySubclassDictionary").SetValue(nonHistorySubclassDictionary);
            Traverse.Create(HeroSelectionManager.Instance).Field("subclassDictionary").SetValue(subclassDictionary);
            HeroSelectionManager.Instance._ClassWarriors.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
            HeroSelectionManager.Instance._ClassHealers.color = Functions.HexToColor(Globals.Instance.ClassColor["healer"]);
            HeroSelectionManager.Instance._ClassMages.color = Functions.HexToColor(Globals.Instance.ClassColor["mage"]);
            HeroSelectionManager.Instance._ClassScouts.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
            HeroSelectionManager.Instance._ClassMagicKnights.color = Functions.HexToColor(Globals.Instance.ClassColor["magicknight"]);
            // Plugin.Log.LogDebug("about to begin looping through subclassDictionary");
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int num1 = 4; // loop through ALL entries in each subclass, not just 4 :D
                switch (index1)
                {
                    case 0:
                        num1 = subclassDictionary["warrior"].Length;
                        break;
                    case 1:
                        num1 = subclassDictionary["scout"].Length;
                        break;
                    case 2:
                        num1 = subclassDictionary["mage"].Length;
                        break;
                    case 3:
                        num1 = subclassDictionary["healer"].Length;
                        break;
                    case 4:
                        if (subclassDictionary.ContainsKey("dlc"))
                        {
                            num1 = subclassDictionary["dlc"].Length;
                            break;
                        }
                        break;
                }
                for (int index2 = 0; index2 < num1; ++index2)
                {
                    SubClassData _subclassdata = (SubClassData)null;
                    GameObject gameObject1 = (GameObject)null;
                    switch (index1)
                    {
                        case 0:
                            _subclassdata = subclassDictionary["warrior"][index2];
                            gameObject1 = HeroSelectionManager.Instance.warriorsGO;
                            break;
                        case 1:
                            _subclassdata = subclassDictionary["scout"][index2];
                            gameObject1 = HeroSelectionManager.Instance.scoutsGO;
                            break;
                        case 2:
                            _subclassdata = subclassDictionary["mage"][index2];
                            gameObject1 = HeroSelectionManager.Instance.magesGO;
                            break;
                        case 3:
                            _subclassdata = subclassDictionary["healer"][index2];
                            gameObject1 = HeroSelectionManager.Instance.healersGO;
                            break;
                        case 4:
                            if (subclassDictionary.ContainsKey("dlc"))
                            {
                                _subclassdata = subclassDictionary["dlc"][index2];
                                gameObject1 = HeroSelectionManager.Instance.dlcsGO;
                                break;
                            }
                            break;
                    }
                    if (!((UnityEngine.Object)_subclassdata == (UnityEngine.Object)null))
                    {
                        GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(HeroSelectionManager.Instance.heroSelectionPrefab, Vector3.zero, Quaternion.identity, gameObject1.transform);
                        gameObject2.transform.localPosition = new Vector3((float)(1.6500000238418579 * (double)index2), -0.65f, 0.0f);
                        gameObject2.name = _subclassdata.SubClassName.ToLower();
                        HeroSelection component = gameObject2.transform.Find("Portrait").transform.GetComponent<HeroSelection>();
                        HeroSelectionManager.Instance.heroSelectionDictionary.Add(gameObject2.name, component);
                        component.blocked = !PlayerManager.Instance.IsHeroUnlocked(_subclassdata.Id);
                        if (component.blocked && GameManager.Instance.IsObeliskChallenge() && !GameManager.Instance.IsWeeklyChallenge())
                            component.blocked = false;
                        if (_subclassdata.Id == "mercenary" || _subclassdata.Id == "ranger" || _subclassdata.Id == "elementalist" || _subclassdata.Id == "cleric")
                            component.blocked = false;
                        if (!(Plugin.medsSubclassList.Contains(_subclassdata.Id)))
                            component.blocked = false;
                        if (component.blocked && GameManager.Instance.IsWeeklyChallenge())
                        {
                            ChallengeData weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
                            if ((UnityEngine.Object)weeklyData != (UnityEngine.Object)null && (_subclassdata.Id == weeklyData.Hero1.Id || _subclassdata.Id == weeklyData.Hero2.Id || _subclassdata.Id == weeklyData.Hero3.Id || _subclassdata.Id == weeklyData.Hero4.Id))
                                component.blocked = false;
                        }
                        component.SetSubclass(_subclassdata);
                        component.SetSprite(_subclassdata.SpriteSpeed, _subclassdata.SpriteBorderSmall, _subclassdata.SpriteBorderLocked);
                        string activeSkin = PlayerManager.Instance.GetActiveSkin(_subclassdata.Id);
                        if (activeSkin != "")
                        {
                            SkinData skinData = Globals.Instance.GetSkinData(activeSkin);
                            string lower = _subclassdata.Id.ToLower();
                            // this.AddToPlayerHeroSkin(_subclassdata.Id, activeSkin);
                            if (!HeroSelectionManager.Instance.playerHeroSkinsDict.ContainsKey(lower))
                                HeroSelectionManager.Instance.playerHeroSkinsDict.Add(lower, activeSkin);
                            else
                                HeroSelectionManager.Instance.playerHeroSkinsDict[lower] = activeSkin;
                            // end
                            component.SetSprite(skinData.SpritePortrait, skinData.SpriteSilueta, _subclassdata.SpriteBorderLocked);
                        }
                        component.SetName(_subclassdata.CharacterName);
                        component.Init();
                        if ((UnityEngine.Object)_subclassdata.SpriteBorderLocked != (UnityEngine.Object)null && _subclassdata.SpriteBorderLocked.name == "regularBorderSmall")
                            component.ShowComingSoon();
                        SubclassByName.Add(_subclassdata.Id, _subclassdata.SubClassName);
                        if (GameManager.Instance.IsWeeklyChallenge())
                            component.blocked = true;
                        HeroSelectionManager.Instance.menuController.Add(component.transform);
                    }
                }
            }
            // Plugin.Log.LogDebug("FINISHED looping through subclassDictionary");
            foreach (KeyValuePair<string, SubClassData> nonHistorySubclass in nonHistorySubclassDictionary)
            {
                SubClassData _subclassdata = nonHistorySubclass.Value;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HeroSelectionManager.Instance.heroSelectionPrefab, Vector3.zero, Quaternion.identity);
                gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, -100f);
                gameObject.name = _subclassdata.SubClassName.ToLower();
                HeroSelection component = gameObject.transform.Find("Portrait").transform.GetComponent<HeroSelection>();
                HeroSelectionManager.Instance.heroSelectionDictionary.Add(gameObject.name, component);
                component.blocked = true;
                component.SetSubclass(_subclassdata);
                component.SetSprite(_subclassdata.SpriteSpeed, _subclassdata.SpriteBorderSmall, _subclassdata.SpriteBorderLocked);
                component.SetName(_subclassdata.CharacterName);
                component.Init();
                SubclassByName.Add(_subclassdata.Id, _subclassdata.SubClassName);
            }
            Traverse.Create(HeroSelectionManager.Instance).Field("SubclassByName").SetValue(SubclassByName);
            if (!GameManager.Instance.IsObeliskChallenge() && AtOManager.Instance.IsFirstGame() && !GameManager.Instance.IsMultiplayer())
            {
                AtOManager.Instance.SetGameId("cban29t");
                HeroSelectionManager.Instance.heroSelectionDictionary["mercenary"].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[0]);
                HeroSelectionManager.Instance.heroSelectionDictionary["ranger"].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[1]);
                HeroSelectionManager.Instance.heroSelectionDictionary["elementalist"].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[2]);
                HeroSelectionManager.Instance.heroSelectionDictionary["cleric"].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[3]);
                SandboxManager.Instance.DisableSandbox();
                yield return (object)Globals.Instance.WaitForSeconds(1f);
                // #TODO: reflections set all values [but this is only for first game, so should be fine?]
                HeroSelectionManager.Instance.BeginAdventure();
            }
            else
            {
                HeroSelectionManager.Instance.charPopupGO = UnityEngine.Object.Instantiate<GameObject>(HeroSelectionManager.Instance.charPopupPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity);
                HeroSelectionManager.Instance.charPopup = HeroSelectionManager.Instance.charPopupGO.GetComponent<CharPopup>();
                HeroSelectionManager.Instance.charPopup.HideNow();
                HeroSelectionManager.Instance.separator.gameObject.SetActive(true);
                if (!GameManager.Instance.IsWeeklyChallenge())
                {
                    HeroSelectionManager.Instance.titleGroupDefault.gameObject.SetActive(true);
                    HeroSelectionManager.Instance.titleWeeklyDefault.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.weeklyModifiersButton.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.weeklyT.gameObject.SetActive(false);
                }
                else
                {
                    HeroSelectionManager.Instance.titleGroupDefault.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.titleWeeklyDefault.gameObject.SetActive(true);
                    HeroSelectionManager.Instance.weeklyModifiersButton.gameObject.SetActive(true);
                    HeroSelectionManager.Instance.weeklyT.gameObject.SetActive(true);
                    Traverse.Create(HeroSelectionManager.Instance).Field("setWeekly").SetValue(true);
                    if (!GameManager.Instance.IsLoadingGame())
                        AtOManager.Instance.SetWeekly(Functions.GetCurrentWeeklyWeek());
                    HeroSelectionManager.Instance.weeklyNumber.text = AtOManager.Instance.GetWeeklyName(AtOManager.Instance.GetWeekly());
                }
                if (!GameManager.Instance.IsObeliskChallenge())
                {
                    HeroSelectionManager.Instance.madnessButton.gameObject.SetActive(true);
                    if (GameManager.Instance.IsMultiplayer())
                    {
                        if (NetworkManager.Instance.IsMaster())
                        {
                            if (GameManager.Instance.IsLoadingGame())
                            {
                                ngValueMaster = ngValue = AtOManager.Instance.GetNgPlus();
                                ngCorruptors = AtOManager.Instance.GetMadnessCorruptors();
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngValueMaster").SetValue(ngValueMaster);
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngValue").SetValue(ngValue);
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngCorruptors").SetValue(ngCorruptors);
                                HeroSelectionManager.Instance.SetMadnessLevel();
                            }
                            else if (SaveManager.PrefsHasKey("madnessLevelCoop") && SaveManager.PrefsHasKey("madnessCorruptorsCoop"))
                            {
                                int num2 = SaveManager.LoadPrefsInt("madnessLevelCoop");
                                string str = SaveManager.LoadPrefsString("madnessCorruptorsCoop");
                                if (PlayerManager.Instance.NgLevel >= num2)
                                {
                                    ngValueMaster = ngValue = num2;
                                    if (str != "")
                                        ngCorruptors = str;
                                }
                                else
                                {
                                    ngValueMaster = ngValue = 0;
                                    ngCorruptors = "";
                                }
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngValueMaster").SetValue(ngValueMaster);
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngValue").SetValue(ngValue);
                                Traverse.Create(HeroSelectionManager.Instance).Field("ngCorruptors").SetValue(ngCorruptors);
                                HeroSelectionManager.Instance.SetMadnessLevel();
                            }
                        }
                    }
                    else if (SaveManager.PrefsHasKey("madnessLevel") && SaveManager.PrefsHasKey("madnessCorruptors"))
                    {
                        int num3 = SaveManager.LoadPrefsInt("madnessLevel");
                        string str = SaveManager.LoadPrefsString("madnessCorruptors");
                        if (PlayerManager.Instance.NgLevel >= num3)
                        {
                            ngValueMaster = ngValue = num3;
                            if (str != "")
                                ngCorruptors = str;
                        }
                        else
                        {
                            ngValueMaster = ngValue = 0;
                            ngCorruptors = "";
                        }
                        Traverse.Create(HeroSelectionManager.Instance).Field("ngValueMaster").SetValue(ngValueMaster);
                        Traverse.Create(HeroSelectionManager.Instance).Field("ngValue").SetValue(ngValue);
                        Traverse.Create(HeroSelectionManager.Instance).Field("ngCorruptors").SetValue(ngCorruptors);
                        HeroSelectionManager.Instance.SetMadnessLevel();
                    }
                }
                else if (!GameManager.Instance.IsWeeklyChallenge())
                {
                    HeroSelectionManager.Instance.madnessButton.gameObject.SetActive(true);
                    if (GameManager.Instance.IsMultiplayer())
                    {
                        if (NetworkManager.Instance.IsMaster())
                        {
                            if (GameManager.Instance.IsLoadingGame())
                            {
                                obeliskMadnessValue = obeliskMadnessValueMaster = AtOManager.Instance.GetObeliskMadness();
                                Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValue").SetValue(obeliskMadnessValue);
                                Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValueMaster").SetValue(obeliskMadnessValueMaster);
                                HeroSelectionManager.Instance.SetObeliskMadnessLevel();
                            }
                            else if (SaveManager.PrefsHasKey("obeliskMadnessCoop"))
                            {
                                int num4 = SaveManager.LoadPrefsInt("obeliskMadnessCoop");
                                obeliskMadnessValue = PlayerManager.Instance.ObeliskMadnessLevel < num4 ? (obeliskMadnessValueMaster = 0) : (obeliskMadnessValueMaster = num4);
                                Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValue").SetValue(obeliskMadnessValue);
                                Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValueMaster").SetValue(obeliskMadnessValueMaster);
                                HeroSelectionManager.Instance.SetObeliskMadnessLevel();
                            }
                        }
                    }
                    else if (SaveManager.PrefsHasKey("obeliskMadness"))
                    {
                        int num5 = SaveManager.LoadPrefsInt("obeliskMadness");
                        obeliskMadnessValue = PlayerManager.Instance.ObeliskMadnessLevel < num5 ? (obeliskMadnessValueMaster = 0) : (obeliskMadnessValueMaster = num5);
                        Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValue").SetValue(obeliskMadnessValue);
                        Traverse.Create(HeroSelectionManager.Instance).Field("obeliskMadnessValueMaster").SetValue(obeliskMadnessValueMaster);
                        HeroSelectionManager.Instance.SetObeliskMadnessLevel();
                    }
                }
                else
                    HeroSelectionManager.Instance.madnessButton.gameObject.SetActive(false);
                HeroSelectionManager.Instance.Resize();
                if (GameManager.Instance.IsWeeklyChallenge() && !GameManager.Instance.IsLoadingGame())
                {
                    HeroSelectionManager.Instance.gameSeedModify.gameObject.SetActive(false);
                    ChallengeData weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
                    if ((UnityEngine.Object)weeklyData != (UnityEngine.Object)null)
                    {
                        HeroSelectionManager.Instance.heroSelectionDictionary[weeklyData.Hero1.Id].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[0]);
                        HeroSelectionManager.Instance.heroSelectionDictionary[weeklyData.Hero2.Id].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[1]);
                        HeroSelectionManager.Instance.heroSelectionDictionary[weeklyData.Hero3.Id].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[2]);
                        HeroSelectionManager.Instance.heroSelectionDictionary[weeklyData.Hero4.Id].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[3]);
                    }
                    if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
                    {
                        if ((UnityEngine.Object)weeklyData != (UnityEngine.Object)null)
                            AtOManager.Instance.SetGameId(weeklyData.Seed);
                        else
                            AtOManager.Instance.SetGameId();
                    }
                    GameManager.Instance.SceneLoaded();
                }
                else if (GameManager.Instance.IsLoadingGame() || AtOManager.Instance.IsFirstGame() && !GameManager.Instance.IsMultiplayer() && !GameManager.Instance.IsObeliskChallenge())
                {
                    HeroSelectionManager.Instance.gameSeedModify.gameObject.SetActive(false);
                    if (AtOManager.Instance.IsFirstGame())
                        AtOManager.Instance.SetGameId("cban29t");
                }
                else
                {
                    if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
                        AtOManager.Instance.SetGameId();
                    HeroSelectionManager.Instance.gameSeed.gameObject.SetActive(true);
                }
                if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
                {
                    HeroSelectionManager.Instance.gameSeedTxt.text = AtOManager.Instance.GetGameId();
                    if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
                        photonView.RPC("NET_SetSeed", RpcTarget.Others, (object)AtOManager.Instance.GetGameId());
                }
                if (GameManager.Instance.IsWeeklyChallenge() || GameManager.Instance.IsObeliskChallenge() && obeliskMadnessValue > 7)
                    HeroSelectionManager.Instance.gameSeed.gameObject.SetActive(false);
                Traverse.Create(HeroSelectionManager.Instance).Field("playerHeroPerksDict").SetValue(new Dictionary<string, List<string>>());
                if (GameManager.Instance.IsMultiplayer())
                {
                    HeroSelectionManager.Instance.masterDescription.gameObject.SetActive(true);
                    if (NetworkManager.Instance.IsMaster())
                    {
                        int num = 0;
                        foreach (Player player in NetworkManager.Instance.PlayerList)
                        {
                            for (int index = 0; index < 4; ++index)
                            {
                                boxSelection[index].ShowPlayer(num);
                                boxSelection[index].SetPlayerPosition(num, player.NickName);
                            }
                            ++num;
                        }
                        for (int position = num; position < 4; ++position)
                        {
                            for (int index = 0; index < 4; ++index)
                                boxSelection[index].SetPlayerPosition(position, "");
                        }
                        Traverse.Create(HeroSelectionManager.Instance).Field("boxSelection").SetValue(boxSelection);
                        foreach (Player player in NetworkManager.Instance.PlayerList)
                        {
                            string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(player.NickName);
                            if (playerNickReal == NetworkManager.Instance.Owner0)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 0);
                            if (playerNickReal == NetworkManager.Instance.Owner1)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 1);
                            if (playerNickReal == NetworkManager.Instance.Owner2)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 2);
                            if (playerNickReal == NetworkManager.Instance.Owner3)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 3);
                        }
                        //this.DrawBoxSelectionNames();
                        // custom DrawBoxSelectionNames
                        int drawboxNum = 0;
                        foreach (Player player in NetworkManager.Instance.PlayerList)
                        {
                            for (int index = 0; index < 4; ++index)
                            {
                                boxSelection[index].ShowPlayer(drawboxNum);
                                boxSelection[index].SetPlayerPosition(drawboxNum, player.NickName);
                            }
                            ++drawboxNum;
                        }
                        for (int position = drawboxNum; position < 4; ++position)
                        {
                            for (int index = 0; index < 4; ++index)
                                boxSelection[index].SetPlayerPosition(position, "");
                        }
                        foreach (Player player in NetworkManager.Instance.PlayerList)
                        {
                            string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(player.NickName);
                            if (playerNickReal == NetworkManager.Instance.Owner0)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 0);
                            if (playerNickReal == NetworkManager.Instance.Owner1)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 1);
                            if (playerNickReal == NetworkManager.Instance.Owner2)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 2);
                            if (playerNickReal == NetworkManager.Instance.Owner3)
                                HeroSelectionManager.Instance.AssignPlayerToBox(player.NickName, 3);
                        }
                        // end custom DrawBoxSelectionNames
                        HeroSelectionManager.Instance.botonBegin.gameObject.SetActive(true);
                        HeroSelectionManager.Instance.botonBegin.Disable();
                        HeroSelectionManager.Instance.botonFollow.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        HeroSelectionManager.Instance.gameSeedModify.gameObject.SetActive(false);
                        HeroSelectionManager.Instance.botonBegin.gameObject.SetActive(false);
                        HeroSelectionManager.Instance.botonFollow.transform.parent.gameObject.SetActive(true);
                        HeroSelectionManager.Instance.ShowFollowStatus();
                    }
                    if (NetworkManager.Instance.IsMaster() && GameManager.Instance.IsLoadingGame())
                    {
                        for (int index = 0; index < 4; ++index)
                        {
                            Hero hero = AtOManager.Instance.GetHero(index);
                            if (hero != null && !((UnityEngine.Object)hero.HeroData == (UnityEngine.Object)null))
                            {
                                string subclassName = hero.SubclassName;
                                int perkRank = hero.PerkRank;
                                string skinUsed = hero.SkinUsed;
                                string cardbackUsed = hero.CardbackUsed;
                                // Plugin.Log.LogDebug("second AddToPlayerHeroSkin! SCDID: " + subclassName + " activeSkin: " + skinUsed);
                                string lower = subclassName.ToLower();
                                // custom AddToPlayerHeroSkin
                                // this.AddToPlayerHeroSkin(subclassName, skinUsed);
                                if (!HeroSelectionManager.Instance.playerHeroSkinsDict.ContainsKey(lower))
                                    HeroSelectionManager.Instance.playerHeroSkinsDict.Add(lower, skinUsed);
                                else
                                    HeroSelectionManager.Instance.playerHeroSkinsDict[lower] = skinUsed;
                                // custom AddToPlayerHeroCardback
                                // this.AddToPlayerHeroCardback(subclassName, cardbackUsed);
                                if (!HeroSelectionManager.Instance.playerHeroCardbackDict.ContainsKey(lower))
                                    HeroSelectionManager.Instance.playerHeroCardbackDict.Add(lower, cardbackUsed);
                                else
                                    HeroSelectionManager.Instance.playerHeroCardbackDict[lower] = cardbackUsed;
                                // end custom
                                if (HeroSelectionManager.Instance.heroSelectionDictionary.ContainsKey(subclassName))
                                {
                                    HeroSelectionManager.Instance.heroSelectionDictionary[subclassName].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[index]);
                                    if (hero.HeroData.HeroSubClass.MainCharacter)
                                    {
                                        HeroSelectionManager.Instance.heroSelectionDictionary[subclassName].SetRankBox(perkRank);
                                        HeroSelectionManager.Instance.heroSelectionDictionary[subclassName].SetSkin(skinUsed);
                                    }
                                }
                                photonView.RPC("NET_AssignHeroToBox", RpcTarget.Others, (object)hero.SubclassName.ToLower(), (object)index, (object)perkRank, (object)skinUsed, (object)cardbackUsed);
                            }
                        }
                    }
                }
                else
                {
                    HeroSelectionManager.Instance.masterDescription.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.botonFollow.transform.parent.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.botonBegin.gameObject.SetActive(true);
                    HeroSelectionManager.Instance.botonBegin.Disable();
                    if (!GameManager.Instance.IsWeeklyChallenge())
                    {
                        //this.PreAssign();
                        // custom PreAssign
                        if (!(PlayerManager.Instance.LastUsedTeam == null || PlayerManager.Instance.LastUsedTeam.Length != 4))
                        {
                            for (int index = 0; index < 4; ++index)
                            {
                                if (HeroSelectionManager.Instance.heroSelectionDictionary.ContainsKey(PlayerManager.Instance.LastUsedTeam[index]) && (GameManager.Instance.IsObeliskChallenge() || PlayerManager.Instance.IsHeroUnlocked(PlayerManager.Instance.LastUsedTeam[index])))
                                    HeroSelectionManager.Instance.heroSelectionDictionary[PlayerManager.Instance.LastUsedTeam[index]].AssignHeroToBox(HeroSelectionManager.Instance.boxGO[index]);
                            }
                        }
                        // end
                    }
                }
                yield return (object)Globals.Instance.WaitForSeconds(0.1f);
                HeroSelectionManager.Instance.RefreshSandboxButton();
                if (!GameManager.Instance.IsObeliskChallenge())
                {
                    HeroSelectionManager.Instance.sandboxButton.gameObject.SetActive(true);
                    HeroSelectionManager.Instance.madnessButton.localPosition = new Vector3(2.43f, HeroSelectionManager.Instance.madnessButton.localPosition.y, HeroSelectionManager.Instance.madnessButton.localPosition.z);
                    HeroSelectionManager.Instance.sandboxButton.localPosition = new Vector3(5.23f, HeroSelectionManager.Instance.sandboxButton.localPosition.y, HeroSelectionManager.Instance.sandboxButton.localPosition.z);
                    if (!GameManager.Instance.IsMultiplayer() || GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
                    {
                        string sandboxMods;
                        if (GameManager.Instance.GameStatus != Enums.GameStatus.LoadGame)
                        {
                            if ((!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()) && PlayerManager.Instance.NgLevel == 0)
                            {
                                SandboxManager.Instance.DisableSandbox();
                                AtOManager.Instance.ClearSandbox();
                            }
                            else
                                AtOManager.Instance.SetSandboxMods(SaveManager.LoadPrefsString("sandboxSettings"));
                            SandboxManager.Instance.LoadValuesFromAtOManager();
                            SandboxManager.Instance.AdjustTotalHeroesBoxToCoop();
                            SandboxManager.Instance.SaveValuesToAtOManager();
                            sandboxMods = AtOManager.Instance.GetSandboxMods();
                        }
                        else
                        {
                            sandboxMods = AtOManager.Instance.GetSandboxMods();
                            SandboxManager.Instance.LoadValuesFromAtOManager();
                        }
                        if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
                            photonView.RPC("NET_ShareSandbox", RpcTarget.Others, (object)Functions.CompressString(sandboxMods));
                        HeroSelectionManager.Instance.RefreshCharBoxesBySandboxHeroes();
                    }
                }
                else
                {
                    HeroSelectionManager.Instance.sandboxButton.gameObject.SetActive(false);
                    HeroSelectionManager.Instance.madnessButton.localPosition = new Vector3(3.8f, HeroSelectionManager.Instance.madnessButton.localPosition.y, HeroSelectionManager.Instance.madnessButton.localPosition.z);
                    SandboxManager.Instance.DisableSandbox();
                }
                HeroSelectionManager.Instance.readyButtonText.gameObject.SetActive(false);
                HeroSelectionManager.Instance.readyButton.gameObject.SetActive(false);
                if (GameManager.Instance.IsMultiplayer())
                {
                    if (NetworkManager.Instance.IsMaster())
                    {
                        NetworkManager.Instance.ClearAllPlayerManualReady();
                        NetworkManager.Instance.SetManualReady(true);
                    }
                    else
                    {
                        HeroSelectionManager.Instance.readyButtonText.gameObject.SetActive(true);
                        HeroSelectionManager.Instance.readyButton.gameObject.SetActive(true);
                    }
                }
                GameManager.Instance.SceneLoaded();
                if (!GameManager.Instance.TutorialWatched("characterPerks"))
                {
                    foreach (KeyValuePair<string, HeroSelection> heroSelection in HeroSelectionManager.Instance.heroSelectionDictionary)
                    {
                        if (heroSelection.Value.perkPointsT.gameObject.activeSelf)
                        {
                            GameManager.Instance.ShowTutorialPopup("characterPerks", heroSelection.Value.perkPointsT.gameObject.transform.position, Vector3.zero);
                            break;
                        }
                    }
                }
                if (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame() && NetworkManager.Instance.IsMaster())
                {
                    bool flag = true;
                    List<string> stringList1 = new List<string>();
                    List<string> stringList2 = new List<string>();
                    for (int index = 0; index < 4; ++index)
                    {
                        Hero hero = AtOManager.Instance.GetHero(index);
                        if (hero.OwnerOriginal != null)
                        {
                            string lower = hero.OwnerOriginal.ToLower();
                            if (!stringList1.Contains(lower))
                                stringList1.Add(lower);
                        }
                        else
                            break;
                    }
                    foreach (Player player in NetworkManager.Instance.PlayerList)
                    {
                        string lower = NetworkManager.Instance.GetPlayerNickReal(player.NickName).ToLower();
                        if (!stringList2.Contains(lower))
                            stringList2.Add(lower);
                    }
                    if (stringList1.Count != stringList2.Count)
                    {
                        flag = false;
                    }
                    else
                    {
                        for (int index = 0; index < stringList2.Count; ++index)
                        {
                            if (!stringList1.Contains(stringList2[index]))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (!flag)
                        photonView.RPC("NET_SetNotOriginal", RpcTarget.All);
                }
            }
        }

    }
}

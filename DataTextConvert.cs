using System;
using static Enums;
using UnityEngine;
using System.Data.Common;
using UnityEngine.InputSystem;
using System.Linq;

namespace Obeliskial_Options
{
    public class DataTextConvert
    {
        /*
         *                                                                                                           
         *    888888888888  ,ad8888ba,        ad88888ba  888888888888  88888888ba   88  888b      88    ,ad8888ba,   
         *         88      d8"'    `"8b      d8"     "8b      88       88      "8b  88  8888b     88   d8"'    `"8b  
         *         88     d8'        `8b     Y8,              88       88      ,8P  88  88 `8b    88  d8'            
         *         88     88          88     `Y8aaaaa,        88       88aaaaaa8P'  88  88  `8b   88  88             
         *         88     88          88       `"""""8b,      88       88""""88'    88  88   `8b  88  88      88888  
         *         88     Y8,        ,8P             `8b      88       88    `8b    88  88    `8b 88  Y8,        88  
         *         88      Y8a.    .a8P      Y8a     a8P      88       88     `8b   88  88     `8888   Y8a.    .a88  
         *         88       `"Y8888Y"'        "Y88888P"       88       88      `8b  88  88      `888    `"Y88888P"   
         *
         *   Returns a standardised identification string.
         */
        public static string ToString(AuraCurseData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(CardData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(CardbackData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.CardbackId : "";
        }
        public static string ToString(SubClassData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(PerkData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(PerkNodeData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(KeyNotesData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(NPCData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(TraitData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(ItemData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(NodeData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.NodeId : "";
        }
        public static string ToString(LootData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(LootItem data)
        {
            return JsonUtility.ToJson(ToText(data));
        }
        public static string ToString(CombatData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.CombatId : "";
        }
        public static string ToString(EventData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.EventId : "";
        }
        public static string ToString(ZoneData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.ZoneId : "";
        }
        public static string ToString(EventRequirementData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.RequirementId : "";
        }
        public static string ToString(Sprite sprite)
        {
            if ((UnityEngine.Object)sprite != (UnityEngine.Object)null)
                return sprite.name;
            return "";
        }
        public static string ToString(SkinData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.SkinId : "";
        }
        public static string ToString(PackData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.PackId : "";
        }
        public static string ToString(ChallengeData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(ChallengeTrait data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(CinematicData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.CinematicId : "";
        }
        public static string ToString(ThermometerTierData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.ThermometerTierId : "";
        }
        public static string ToString(CardPlayerPackData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.PackId : "";
        }
        public static string ToString(UnityEngine.Object data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString(AudioClip data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString(GameObject data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString(NodesConnectedRequirement data)
        {
            return JsonUtility.ToJson(ToText(data));
        }
        public static string ToString(CombatEffect data)
        {
            return JsonUtility.ToJson(ToText(data));
        }
        public static string ToString(CorruptionPackData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.PackName : "";
        }
        public static string ToString(Vector2 data)
        {
            return data.ToString();
        }
        public static string ToString(TierRewardData data)
        {
            return JsonUtility.ToJson(data);
        }
        public static string ToString<T>(T data)
        {
            if (typeof(T).BaseType == typeof(System.Enum))
                return Enum.GetName(data.GetType(), data);
            Plugin.Log.LogError("ToString<T> is capturing a type that it isn't set up for!!! " + typeof(T));
            return "";
        }
        // I'm pretty sure these can be simplified? probably with the var keyword? f!L pls lmk
        public static string[] ToString(SubClassData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(CardData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(PerkData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(HeroCards[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = JsonUtility.ToJson(ToText(data[a]));
            return text;
        }
        public static string[] ToString(EventData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(CombatData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(CombatEffect[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(NPCData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(ChallengeData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(ChallengeTrait[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(NodeData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(PerkNodeData[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(LootItem[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToString(data[a]);
            return text;
        }
        public static string[] ToString(NodesConnectedRequirement[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = JsonUtility.ToJson(ToText(data[a]));
            return text;
        }
        public static string[] ToString(AICards[] data)
        {
            string[] text = new string[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = JsonUtility.ToJson(ToText(data[a]));
            return text;
        }
        /* not actually needed?
         * public static string[] ToString(AuraCurseData[] data)
        {
            if (data.Length > 0)
            {
                string[] text = new string[data.Length];
                for (int a = 0; a < data.Length; a++)
                    text[a] = ((UnityEngine.Object)data[a] != (UnityEngine.Object)null) ? data[a].Id : "";
                return text;
            }
            return new string[0];
        }*/
        public static string[] ToString<T>(T[] data)
        {
            if (data.Length > 0)
            {
                if (data[0].GetType().BaseType == typeof(System.Enum))
                {
                    string[] text = new string[data.Length];
                    for (int a = 0; a < data.Length; a++)
                        text[a] = Enum.GetName(data[a].GetType(), data[a]);
                    return text;
                }
                Plugin.Log.LogError("[] ToString<T> is capturing a type that it isn't set up for!!! " + typeof(T));
            }
            return new string[0];
        }
        /*
         *                                                                                                           
         *    888888888888  ,ad8888ba,          888888888888  88888888888  8b        d8  888888888888  
         *         88      d8"'    `"8b              88       88            Y8,    ,8P        88       
         *         88     d8'        `8b             88       88             `8b  d8'         88       
         *         88     88          88             88       88aaaaa          Y88P           88       
         *         88     88          88             88       88"""""          d88b           88       
         *         88     Y8,        ,8P             88       88             ,8P  Y8,         88       
         *         88      Y8a.    .a8P              88       88            d8'    `8b        88       
         *         88       `"Y8888Y"'               88       88888888888  8P        Y8       88       
         *
         *   Converts input AtO type to corresponding DataText type.
         */
        public static CardDataText ToText(CardData data)
        {
            CardDataText text = new();
            text.ID = data.Id;
            text.AcEnergyBonus = DataTextConvert.ToString(data.AcEnergyBonus);
            text.AcEnergyBonus2 = DataTextConvert.ToString(data.AcEnergyBonus2);
            text.AcEnergyBonusQuantity = data.AcEnergyBonusQuantity;
            text.AcEnergyBonus2Quantity = data.AcEnergyBonus2Quantity;
            text.AddCard = data.AddCard;
            text.AddCardChoose = data.AddCardChoose;
            text.AddCardCostTurn = data.AddCardCostTurn;
            text.AddCardFrom = DataTextConvert.ToString(data.AddCardFrom);
            text.AddCardId = data.AddCardId;
            text.AddCardList = DataTextConvert.ToString(data.AddCardList);
            text.AddCardPlace = DataTextConvert.ToString(data.AddCardPlace);
            text.AddCardReducedCost = data.AddCardReducedCost;
            text.AddCardType = DataTextConvert.ToString(data.AddCardType);
            text.AddCardTypeAux = DataTextConvert.ToString(data.AddCardTypeAux);
            text.AddCardVanish = data.AddCardVanish;
            text.Aura = DataTextConvert.ToString(data.Aura);
            text.Aura2 = DataTextConvert.ToString(data.Aura2);
            text.Aura3 = DataTextConvert.ToString(data.Aura3);
            text.AuraCharges = data.AuraCharges;
            text.AuraChargesSpecialValue1 = data.AuraChargesSpecialValue1;
            text.AuraChargesSpecialValue2 = data.AuraChargesSpecialValue2;
            text.AuraChargesSpecialValueGlobal = data.AuraChargesSpecialValueGlobal;
            text.AuraCharges2 = data.AuraCharges2;
            text.AuraCharges2SpecialValue1 = data.AuraCharges2SpecialValue1;
            text.AuraCharges2SpecialValue2 = data.AuraCharges2SpecialValue2;
            text.AuraCharges2SpecialValueGlobal = data.AuraCharges2SpecialValueGlobal;
            text.AuraCharges3 = data.AuraCharges3;
            text.AuraCharges3SpecialValue1 = data.AuraCharges3SpecialValue1;
            text.AuraCharges3SpecialValue2 = data.AuraCharges3SpecialValue2;
            text.AuraCharges3SpecialValueGlobal = data.AuraCharges3SpecialValueGlobal;
            text.AuraSelf = DataTextConvert.ToString(data.AuraSelf);
            text.AuraSelf2 = DataTextConvert.ToString(data.AuraSelf2);
            text.AuraSelf3 = DataTextConvert.ToString(data.AuraSelf3);
            text.AutoplayDraw = data.AutoplayDraw;
            text.AutoplayEndTurn = data.AutoplayEndTurn;
            text.BaseCard = data.BaseCard;
            text.CardClass = DataTextConvert.ToString(data.CardClass);
            text.CardName = data.CardName;
            text.CardNumber = data.CardNumber;
            text.CardRarity = DataTextConvert.ToString(data.CardRarity);
            text.CardType = DataTextConvert.ToString(data.CardType);
            text.CardTypeAux = DataTextConvert.ToString(data.CardTypeAux);
            /* CardTypeList is built from CardType and CardTypeAux when GetCardTypes() is called, so we don't need to use it. */
            text.CardUpgraded = DataTextConvert.ToString(data.CardUpgraded);
            text.Corrupted = data.Corrupted;
            text.Curse = DataTextConvert.ToString(data.Curse);
            text.Curse2 = DataTextConvert.ToString(data.Curse2);
            text.Curse3 = DataTextConvert.ToString(data.Curse3);
            text.CurseCharges = data.CurseCharges;
            text.CurseChargesSpecialValue1 = data.CurseChargesSpecialValue1;
            text.CurseChargesSpecialValue2 = data.CurseChargesSpecialValue2;
            text.CurseChargesSpecialValueGlobal = data.CurseChargesSpecialValueGlobal;
            text.CurseCharges2 = data.CurseCharges2;
            text.CurseCharges2SpecialValue1 = data.CurseCharges2SpecialValue1;
            text.CurseCharges2SpecialValue2 = data.CurseCharges2SpecialValue2;
            text.CurseCharges2SpecialValueGlobal = data.CurseCharges2SpecialValueGlobal;
            text.CurseCharges3 = data.CurseCharges3;
            text.CurseCharges3SpecialValue1 = data.CurseCharges3SpecialValue1;
            text.CurseCharges3SpecialValue2 = data.CurseCharges3SpecialValue2;
            text.CurseCharges3SpecialValueGlobal = data.CurseCharges3SpecialValueGlobal;
            text.CurseSelf = DataTextConvert.ToString(data.CurseSelf);
            text.CurseSelf2 = DataTextConvert.ToString(data.CurseSelf2);
            text.CurseSelf3 = DataTextConvert.ToString(data.CurseSelf3);
            text.Damage = data.Damage;
            text.DamageSpecialValue1 = data.DamageSpecialValue1;
            text.DamageSpecialValue2 = data.DamageSpecialValue2;
            text.DamageSpecialValueGlobal = data.DamageSpecialValueGlobal;
            text.Damage2 = data.Damage2;
            text.Damage2SpecialValue1 = data.Damage2SpecialValue1;
            text.Damage2SpecialValue2 = data.Damage2SpecialValue2;
            text.Damage2SpecialValueGlobal = data.Damage2SpecialValueGlobal;
            text.DamageEnergyBonus = data.DamageEnergyBonus;
            text.DamageSelf = data.DamageSelf;
            text.DamageSelf2 = data.DamageSelf2;
            text.DamageSides = data.DamageSides;
            text.DamageSides2 = data.DamageSides2;
            text.DamageType = DataTextConvert.ToString(data.DamageType);
            text.DamageType2 = DataTextConvert.ToString(data.DamageType2);
            text.Description = data.Description;
            // text.DescriptionID = data.descriptionid
            text.DiscardCard = data.DiscardCard;
            text.DiscardCardAutomatic = data.DiscardCardAutomatic;
            text.DiscardCardPlace = DataTextConvert.ToString(data.DiscardCardPlace);
            text.DiscardCardType = DataTextConvert.ToString(data.DiscardCardType);
            text.DiscardCardTypeAux = DataTextConvert.ToString(data.DiscardCardTypeAux);
            text.DispelAuras = data.DispelAuras;
            text.DrawCard = data.DrawCard;
            text.EffectCastCenter = data.EffectCastCenter;
            text.EffectCaster = data.EffectCaster;
            text.EffectCasterRepeat = data.EffectCasterRepeat;
            text.EffectPostCastDelay = data.EffectPostCastDelay;
            text.EffectPostTargetDelay = data.EffectPostTargetDelay;
            text.EffectPreAction = data.EffectPreAction;
            text.EffectRepeat = data.EffectRepeat;
            text.EffectRepeatDelay = data.EffectRepeatDelay;
            text.EffectRepeatEnergyBonus = data.EffectRepeatEnergyBonus;
            text.EffectRepeatMaxBonus = data.EffectRepeatMaxBonus;
            text.EffectRepeatModificator = data.EffectRepeatModificator;
            text.EffectRepeatTarget = DataTextConvert.ToString(data.EffectRepeatTarget);
            text.EffectRequired = data.EffectRequired;
            text.EffectTarget = data.EffectTarget;
            text.EffectTrail = data.EffectTrail;
            text.EffectTrailAngle = DataTextConvert.ToString(data.EffectTrailAngle);
            text.EffectTrailRepeat = data.EffectTrailRepeat;
            text.EffectTrailSpeed = data.EffectTrailSpeed;
            text.EndTurn = data.EndTurn;
            text.EnergyCost = data.EnergyCost;
            text.EnergyCostForShow = data.EnergyCostForShow;
            text.EnergyRecharge = data.EnergyRecharge;
            text.EnergyReductionPermanent = data.EnergyReductionPermanent;
            text.EnergyReductionTemporal = data.EnergyReductionTemporal;
            text.EnergyReductionToZeroPermanent = data.EnergyReductionToZeroPermanent;
            text.EnergyReductionToZeroTemporal = data.EnergyReductionToZeroTemporal;
            text.ExhaustCounter = data.ExhaustCounter;
            text.FlipSprite = data.FlipSprite;
            text.Fluff = data.Fluff;
            text.FluffPercent = data.FluffPercent;
            text.GoldGainQuantity = data.GoldGainQuantity;
            text.Heal = data.Heal;
            text.HealAuraCurseName = DataTextConvert.ToString(data.HealAuraCurseName);
            text.HealAuraCurseName2 = DataTextConvert.ToString(data.HealAuraCurseName2);
            text.HealAuraCurseName3 = DataTextConvert.ToString(data.HealAuraCurseName3);
            text.HealAuraCurseName4 = DataTextConvert.ToString(data.HealAuraCurseName4);
            text.HealAuraCurseSelf = DataTextConvert.ToString(data.HealAuraCurseSelf);
            text.HealCurses = data.HealCurses;
            text.HealEnergyBonus = data.HealEnergyBonus;
            text.HealSelf = data.HealSelf;
            text.HealSelfPerDamageDonePercent = data.HealSelfPerDamageDonePercent;
            text.HealSelfSpecialValue1 = data.HealSelfSpecialValue1;
            text.HealSelfSpecialValue2 = data.HealSelfSpecialValue2;
            text.HealSelfSpecialValueGlobal = data.HealSelfSpecialValueGlobal;
            text.HealSides = data.HealSides;
            text.HealSpecialValue1 = data.HealSpecialValue1;
            text.HealSpecialValue2 = data.HealSpecialValue2;
            text.HealSpecialValueGlobal = data.HealSpecialValueGlobal;
            text.IgnoreBlock = data.IgnoreBlock;
            text.IgnoreBlock2 = data.IgnoreBlock2;
            text.IncreaseAuras = data.IncreaseAuras;
            text.IncreaseCurses = data.IncreaseCurses;
            text.Innate = data.Innate;
            text.IsPetAttack = data.IsPetAttack;
            text.IsPetCast = data.IsPetCast;
            text.Item = DataTextConvert.ToString(data.Item);

            if (text.Item.Length > 0 && Plugin.medsItemDataSource.ContainsKey(text.Item))
                text.Item = JsonUtility.ToJson(ToText(Plugin.medsItemDataSource[text.Item]));
            text.ItemEnchantment = DataTextConvert.ToString(data.ItemEnchantment);
            if (text.ItemEnchantment.Length > 0 && Plugin.medsItemDataSource.ContainsKey(text.ItemEnchantment))
                text.ItemEnchantment = JsonUtility.ToJson(ToText(Plugin.medsItemDataSource[text.ItemEnchantment]));
            text.KillPet = data.KillPet;
            text.Lazy = data.Lazy;
            text.LookCards = data.LookCards;
            text.LookCardsDiscardUpTo = data.LookCardsDiscardUpTo;
            text.LookCardsVanishUpTo = data.LookCardsVanishUpTo;
            text.MaxInDeck = data.MaxInDeck;
            text.ModifiedByTrait = data.ModifiedByTrait;
            text.MoveToCenter = data.MoveToCenter;
            text.OnlyInWeekly = data.OnlyInWeekly;
            text.PetFront = data.PetFront;
            text.PetInvert = data.PetInvert;
            text.PetModel = DataTextConvert.ToString(data.PetModel);
            text.PetOffset = DataTextConvert.ToString(data.PetOffset);
            text.PetSize = DataTextConvert.ToString(data.PetSize);
            text.Playable = data.Playable;
            text.PullTarget = data.PullTarget;
            text.PushTarget = data.PushTarget;
            text.ReduceAuras = data.ReduceAuras;
            text.ReduceCurses = data.ReduceCurses;
            text.RelatedCard = data.RelatedCard;
            text.RelatedCard2 = data.RelatedCard2;
            text.RelatedCard3 = data.RelatedCard3;
            text.SelfHealthLoss = data.SelfHealthLoss;
            text.SelfHealthLossSpecialGlobal = data.SelfHealthLossSpecialGlobal;
            text.SelfHealthLossSpecialValue1 = data.SelfHealthLossSpecialValue1;
            text.SelfHealthLossSpecialValue2 = data.SelfHealthLossSpecialValue2;
            text.ShardsGainQuantity = data.ShardsGainQuantity;
            text.ShowInTome = data.ShowInTome;
            text.Sku = data.Sku;
            text.Sound = DataTextConvert.ToString(data.Sound);
            text.SoundPreAction = DataTextConvert.ToString(data.SoundPreAction);
            text.SoundPreActionFemale = DataTextConvert.ToString(data.SoundPreActionFemale);
            text.SpecialAuraCurseName1 = DataTextConvert.ToString(data.SpecialAuraCurseName1);
            text.SpecialAuraCurseName2 = DataTextConvert.ToString(data.SpecialAuraCurseName2);
            text.SpecialAuraCurseNameGlobal = DataTextConvert.ToString(data.SpecialAuraCurseNameGlobal);
            text.SpecialValue1 = DataTextConvert.ToString(data.SpecialValue1);
            text.SpecialValue2 = DataTextConvert.ToString(data.SpecialValue2);
            text.SpecialValueGlobal = DataTextConvert.ToString(data.SpecialValueGlobal);
            text.SpecialValueModifier1 = data.SpecialValueModifier1;
            text.SpecialValueModifier2 = data.SpecialValueModifier2;
            text.SpecialValueModifierGlobal = data.SpecialValueModifierGlobal;
            text.Sprite = DataTextConvert.ToString(data.Sprite);
            if ((UnityEngine.Object)data.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Sprite, "card/" + text.CardClass);
            text.Starter = data.Starter;
            text.StealAuras = data.StealAuras;
            text.SummonAura = DataTextConvert.ToString(data.SummonAura);
            text.SummonAura2 = DataTextConvert.ToString(data.SummonAura2);
            text.SummonAura3 = DataTextConvert.ToString(data.SummonAura3);
            text.SummonAuraCharges = data.SummonAuraCharges;
            text.SummonAuraCharges2 = data.SummonAuraCharges2;
            text.SummonAuraCharges3 = data.SummonAuraCharges3;
            text.SummonUnit = DataTextConvert.ToString(data.SummonUnit);
            text.SummonUnitNum = data.SummonUnitNum;
            text.TargetPosition = DataTextConvert.ToString(data.TargetPosition);
            text.TargetSide = DataTextConvert.ToString(data.TargetSide);
            text.TargetType = DataTextConvert.ToString(data.TargetType);
            text.TransferCurses = data.TransferCurses;
            text.UpgradedFrom = data.UpgradedFrom;
            text.UpgradesTo1 = data.UpgradesTo1;
            text.UpgradesTo2 = data.UpgradesTo2;
            text.UpgradesToRare = DataTextConvert.ToString(data.UpgradesToRare);
            text.Vanish = data.Vanish;
            text.Visible = data.Visible;
            return text;
        }

        public static TraitDataText ToText(TraitData data)
        {
            TraitDataText text = new();
            text.ID = data.Id;
            text.Activation = DataTextConvert.ToString(data.Activation);
            text.AuraCurseBonus1 = DataTextConvert.ToString(data.AuracurseBonus1);
            text.AuraCurseBonus2 = DataTextConvert.ToString(data.AuracurseBonus2);
            text.AuraCurseBonus3 = DataTextConvert.ToString(data.AuracurseBonus3);
            text.AuraCurseBonusValue1 = data.AuracurseBonusValue1;
            text.AuraCurseBonusValue2 = data.AuracurseBonusValue2;
            text.AuraCurseBonusValue3 = data.AuracurseBonusValue3;
            text.AuraCurseImmune1 = data.AuracurseImmune1;
            text.AuraCurseImmune2 = data.AuracurseImmune2;
            text.AuraCurseImmune3 = data.AuracurseImmune3;
            text.CharacterStatModified = DataTextConvert.ToString(data.CharacterStatModified);
            text.CharacterStatModifiedValue = data.CharacterStatModifiedValue;
            text.DamageBonusFlat = DataTextConvert.ToString(data.DamageBonusFlat);
            text.DamageBonusFlat2 = DataTextConvert.ToString(data.DamageBonusFlat2);
            text.DamageBonusFlat3 = DataTextConvert.ToString(data.DamageBonusFlat3);
            text.DamageBonusFlatValue = data.DamageBonusFlatValue;
            text.DamageBonusFlatValue2 = data.DamageBonusFlatValue2;
            text.DamageBonusFlatValue3 = data.DamageBonusFlatValue3;
            text.DamageBonusPercent = DataTextConvert.ToString(data.DamageBonusPercent);
            text.DamageBonusPercent2 = DataTextConvert.ToString(data.DamageBonusPercent2);
            text.DamageBonusPercent3 = DataTextConvert.ToString(data.DamageBonusPercent3);
            text.DamageBonusPercentValue = data.DamageBonusPercentValue;
            text.DamageBonusPercentValue2 = data.DamageBonusPercentValue2;
            text.DamageBonusPercentValue3 = data.DamageBonusPercentValue3;
            text.Description = data.Description;
            text.HealFlatBonus = data.HealFlatBonus;
            text.HealPercentBonus = data.HealPercentBonus;
            text.HealReceivedFlatBonus = data.HealReceivedFlatBonus;
            text.HealReceivedPercentBonus = data.HealReceivedPercentBonus;
            text.ResistModified1 = DataTextConvert.ToString(data.ResistModified1);
            text.ResistModified2 = DataTextConvert.ToString(data.ResistModified2);
            text.ResistModified3 = DataTextConvert.ToString(data.ResistModified3);
            text.ResistModifiedValue1 = data.ResistModifiedValue1;
            text.ResistModifiedValue2 = data.ResistModifiedValue2;
            text.ResistModifiedValue3 = data.ResistModifiedValue3;
            text.TimesPerRound = data.TimesPerRound;
            text.TimesPerTurn = data.TimesPerTurn;
            text.TraitCard = DataTextConvert.ToString(data.TraitCard);
            text.TraitCardForAllHeroes = DataTextConvert.ToString(data.TraitCardForAllHeroes);
            text.TraitName = data.TraitName;
            return text;
        }

        public static SubClassDataText ToText(SubClassData data)
        {
            SubClassDataText text = new();
            text.ID = data.Id;
            text.ActionSound = DataTextConvert.ToString(data.ActionSound);
            text.Cards = DataTextConvert.ToString(data.Cards);
            text.ChallengePack0 = DataTextConvert.ToString(data.ChallengePack0);
            text.ChallengePack1 = DataTextConvert.ToString(data.ChallengePack1);
            text.ChallengePack2 = DataTextConvert.ToString(data.ChallengePack2);
            text.ChallengePack3 = DataTextConvert.ToString(data.ChallengePack3);
            text.ChallengePack4 = DataTextConvert.ToString(data.ChallengePack4);
            text.ChallengePack5 = DataTextConvert.ToString(data.ChallengePack5);
            text.ChallengePack6 = DataTextConvert.ToString(data.ChallengePack6);
            text.CharacterDescription = data.CharacterDescription;
            text.CharacterDescriptionStrength = data.CharacterDescriptionStrength;
            text.CharacterName = data.CharacterName;
            text.Energy = data.Energy;
            text.EnergyTurn = data.EnergyTurn;
            text.Female = data.Female;
            text.FluffOffsetX = data.FluffOffsetX;
            text.FluffOffsetY = data.FluffOffsetY;
            text.GameObjectAnimated = DataTextConvert.ToString(data.GameObjectAnimated);
            text.HeroClass = DataTextConvert.ToString(data.HeroClass);
            text.HitSound = DataTextConvert.ToString(data.HitSound);
            text.HP = data.Hp;
            text.MaxHP = data.MaxHp;
            text.Item = ToString(data.Item);
            text.ResistSlashing = data.ResistSlashing;
            text.ResistBlunt = data.ResistBlunt;
            text.ResistPiercing = data.ResistPiercing;
            text.ResistFire = data.ResistFire;
            text.ResistCold = data.ResistCold;
            text.ResistLightning = data.ResistLightning;
            text.ResistHoly = data.ResistHoly;
            text.ResistShadow = data.ResistShadow;
            text.ResistMind = data.ResistMind;
            text.Speed = data.Speed;
            text.Sprite = DataTextConvert.ToString(data.Sprite);
            if ((UnityEngine.Object)data.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Sprite, "subclass");
            text.SpriteBorder = DataTextConvert.ToString(data.SpriteBorder);
            if ((UnityEngine.Object)data.SpriteBorder != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteBorder, "subclass");
            text.SpriteBorderLocked = DataTextConvert.ToString(data.SpriteBorderLocked);
            if ((UnityEngine.Object)data.SpriteBorderLocked != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteBorderLocked, "subclass");
            text.SpriteBorderSmall = DataTextConvert.ToString(data.SpriteBorderSmall);
            if ((UnityEngine.Object)data.SpriteBorderSmall != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteBorderSmall, "subclass");
            text.SpritePortrait = DataTextConvert.ToString(data.SpritePortrait);
            if ((UnityEngine.Object)data.SpritePortrait != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpritePortrait, "subclass");
            text.SpriteSpeed = DataTextConvert.ToString(data.SpriteSpeed);
            if ((UnityEngine.Object)data.SpriteSpeed != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteSpeed, "subclass");
            text.StickerAngry = DataTextConvert.ToString(data.StickerAngry);
            if ((UnityEngine.Object)data.StickerAngry != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.StickerAngry, "subclass");
            text.StickerBase = DataTextConvert.ToString(data.StickerBase);
            if ((UnityEngine.Object)data.StickerBase != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.StickerBase, "subclass");
            text.StickerIndifferent = DataTextConvert.ToString(data.StickerIndiferent);
            if ((UnityEngine.Object)data.StickerIndiferent != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.StickerIndiferent, "subclass");
            text.StickerLove = DataTextConvert.ToString(data.StickerLove);
            if ((UnityEngine.Object)data.StickerLove != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.StickerLove, "subclass");
            text.StickerSurprise = DataTextConvert.ToString(data.StickerSurprise);
            if ((UnityEngine.Object)data.StickerSurprise != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.StickerSurprise, "subclass");
            text.StickerOffsetX = data.StickerOffsetX;
            text.SubclassName = data.SubClassName;
            text.Trait0 = DataTextConvert.ToString(data.Trait0);
            text.Trait1A = DataTextConvert.ToString(data.Trait1A);
            text.Trait1B = DataTextConvert.ToString(data.Trait1B);
            text.Trait2A = DataTextConvert.ToString(data.Trait2A);
            text.Trait2B = DataTextConvert.ToString(data.Trait2B);
            text.Trait3A = DataTextConvert.ToString(data.Trait3A);
            text.Trait3B = DataTextConvert.ToString(data.Trait3B);
            text.Trait4A = DataTextConvert.ToString(data.Trait4A);
            text.Trait4B = DataTextConvert.ToString(data.Trait4B);
            return text;
        }

        public static CardDataText[] ToText(CardData[] data)
        {
            CardDataText[] text = new CardDataText[data.Length];
            for (int a = 0; a < data.Length; a++)
                text[a] = ToText(data[a]);
            return text;
        }

        public static HeroCardsText ToText(HeroCards data)
        {
            HeroCardsText text = new();
            text.UnitsInDeck = data.UnitsInDeck;
            text.Card = DataTextConvert.ToString(data.Card);
            return text;
        }

        public static ItemDataText ToText(ItemData data)
        {
            ItemDataText text = new();
            text.ACG1MultiplyByEnergyUsed = data.Acg1MultiplyByEnergyUsed;
            text.ACG2MultiplyByEnergyUsed = data.Acg2MultiplyByEnergyUsed;
            text.ACG3MultiplyByEnergyUsed = data.Acg3MultiplyByEnergyUsed;
            text.Activation = ToString(data.Activation);
            text.ActivationOnlyOnHeroes = data.ActivationOnlyOnHeroes;
            text.AuraCurseBonus1 = ToString(data.AuracurseBonus1);
            text.AuraCurseBonus2 = ToString(data.AuracurseBonus2);
            text.AuraCurseBonusValue1 = data.AuracurseBonusValue1;
            text.AuraCurseBonusValue2 = data.AuracurseBonusValue2;
            text.AuraCurseCustomAC = ToString(data.AuracurseCustomAC);
            text.AuraCurseCustomModValue1 = data.AuracurseCustomModValue1;
            text.AuraCurseCustomModValue2 = data.AuracurseCustomModValue2;
            text.AuraCurseCustomString = data.AuracurseCustomString;
            text.AuraCurseGain1 = ToString(data.AuracurseGain1);
            text.AuraCurseGain2 = ToString(data.AuracurseGain2);
            text.AuraCurseGain3 = ToString(data.AuracurseGain3);
            text.AuraCurseGainValue1 = data.AuracurseGainValue1;
            text.AuraCurseGainValue2 = data.AuracurseGainValue2;
            text.AuraCurseGainValue3 = data.AuracurseGainValue3;
            text.AuraCurseGainSelf1 = ToString(data.AuracurseGainSelf1);
            text.AuraCurseGainSelf2 = ToString(data.AuracurseGainSelf2);
            text.AuraCurseGainSelfValue1 = data.AuracurseGainSelfValue1;
            text.AuraCurseGainSelfValue2 = data.AuracurseGainSelfValue2;
            text.AuraCurseImmune1 = ToString(data.AuracurseImmune1);
            text.AuraCurseImmune2 = ToString(data.AuracurseImmune2);
            text.AuraCurseNumForOneEvent = data.AuraCurseNumForOneEvent;
            text.AuraCurseSetted = ToString(data.AuraCurseSetted);
            text.CardNum = data.CardNum;
            text.CardPlace = ToString(data.CardPlace);
            text.CardsReduced = data.CardsReduced;
            text.CardToGain = ToString(data.CardToGain);
            text.CardToGainList = ToString(data.CardToGainList.ToArray());
            text.CardToGainType = ToString(data.CardToGainType);
            text.CardToReduceType = ToString(data.CardToReduceType);
            text.CastedCardType = ToString(data.CastedCardType);
            text.CastEnchantmentOnFinishSelfCast = data.CastEnchantmentOnFinishSelfCast;
            text.ChanceToDispel = data.ChanceToDispel;
            text.ChanceToDispelNum = data.ChanceToDispelNum;
            text.CharacterStatModified = ToString(data.CharacterStatModified);
            text.CharacterStatModified2 = ToString(data.CharacterStatModified2);
            text.CharacterStatModified3 = ToString(data.CharacterStatModified3);
            text.CharacterStatModifiedValue = data.CharacterStatModifiedValue;
            text.CharacterStatModifiedValue2 = data.CharacterStatModifiedValue2;
            text.CharacterStatModifiedValue3 = data.CharacterStatModifiedValue3;
            text.CostReducePermanent = data.CostReducePermanent;
            text.CostReduceReduction = data.CostReduceReduction;
            text.CostReduction = data.CostReduction;
            text.CostZero = data.CostZero;
            text.CursedItem = data.CursedItem;
            text.DamageFlatBonus = ToString(data.DamageFlatBonus);
            text.DamageFlatBonus2 = ToString(data.DamageFlatBonus2);
            text.DamageFlatBonus3 = ToString(data.DamageFlatBonus3);
            text.DamageFlatBonusValue = data.DamageFlatBonusValue;
            text.DamageFlatBonusValue2 = data.DamageFlatBonusValue2;
            text.DamageFlatBonusValue3 = data.DamageFlatBonusValue3;
            text.DamagePercentBonus = ToString(data.DamagePercentBonus);
            text.DamagePercentBonus2 = ToString(data.DamagePercentBonus2);
            text.DamagePercentBonus3 = ToString(data.DamagePercentBonus3);
            text.DamagePercentBonusValue = data.DamagePercentBonusValue;
            text.DamagePercentBonusValue2 = data.DamagePercentBonusValue2;
            text.DamagePercentBonusValue3 = data.DamagePercentBonusValue3;
            text.DamageToTarget = data.DamageToTarget;
            text.DamageToTargetType = ToString(data.DamageToTargetType);
            text.DestroyAfterUse = data.DestroyAfterUse;
            text.DestroyAfterUses = data.DestroyAfterUses;
            text.DestroyEndOfTurn = data.DestroyEndOfTurn;
            text.DestroyStartOfTurn = data.DestroyStartOfTurn;
            text.DrawCards = data.DrawCards;
            text.DrawMultiplyByEnergyUsed = data.DrawMultiplyByEnergyUsed;
            text.DropOnly = data.DropOnly;
            text.DTTMultiplyByEnergyUsed = data.DttMultiplyByEnergyUsed;
            text.DuplicateActive = data.DuplicateActive;
            text.EffectCaster = data.EffectCaster;
            text.EffectItemOwner = data.EffectItemOwner;
            text.EffectTarget = data.EffectTarget;
            text.EmptyHand = data.EmptyHand;
            text.EnergyQuantity = data.EnergyQuantity;
            text.ExactRound = data.ExactRound;
            text.HealFlatBonus = data.HealFlatBonus;
            text.HealPercentBonus = data.HealPercentBonus;
            text.HealPercentQuantity = data.HealPercentQuantity;
            text.HealQuantity = data.HealQuantity;
            text.HealReceivedFlatBonus = data.HealReceivedFlatBonus;
            text.HealReceivedPercentBonus = data.HealReceivedPercentBonus;
            text.ID = data.Id;
            text.IsEnchantment = data.IsEnchantment;
            text.ItemSound = ToString(data.ItemSound);
            text.ItemTarget = ToString(data.ItemTarget);
            text.LowerOrEqualPercentHP = data.LowerOrEqualPercentHP;
            text.MaxHealth = data.MaxHealth;
            text.ModifiedDamageType = ToString(data.ModifiedDamageType);
            text.NotShowCharacterBonus = data.NotShowCharacterBonus;
            text.OnlyAddItemToNPCs = data.OnlyAddItemToNPCs;
            text.PassSingleAndCharacterRolls = data.PassSingleAndCharacterRolls;
            text.PercentDiscountShop = data.PercentDiscountShop;
            text.PercentRetentionEndGame = data.PercentRetentionEndGame;
            text.Permanent = data.Permanent;
            text.QuestItem = data.QuestItem;
            text.ReduceHighestCost = data.ReduceHighestCost;
            text.ResistModified1 = ToString(data.ResistModified1);
            text.ResistModified2 = ToString(data.ResistModified2);
            text.ResistModified3 = ToString(data.ResistModified3);
            text.ResistModifiedValue1 = data.ResistModifiedValue1;
            text.ResistModifiedValue2 = data.ResistModifiedValue2;
            text.ResistModifiedValue3 = data.ResistModifiedValue3;
            text.RoundCycle = data.RoundCycle;
            text.SpriteBossDrop = ToString(data.SpriteBossDrop);
            if ((UnityEngine.Object)data.SpriteBossDrop != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteBossDrop, "item");
            text.TimesPerCombat = data.TimesPerCombat;
            text.TimesPerTurn = data.TimesPerTurn;
            text.UsedEnergy = data.UsedEnergy;
            text.UseTheNextInsteadWhenYouPlay = data.UseTheNextInsteadWhenYouPlay;
            text.Vanish = data.Vanish;
            return text;
        }

        public static AICardsText ToText(AICards data)
        {
            AICardsText text = new();
            text.AddCardRound = data.AddCardRound;
            text.AuraCurseCastIf = DataTextConvert.ToString(data.AuracurseCastIf);
            text.Card = DataTextConvert.ToString(data.Card);
            text.OnlyCastIf = DataTextConvert.ToString(data.OnlyCastIf);
            text.PercentToCast = data.PercentToCast;
            text.Priority = data.Priority;
            text.TargetCast = DataTextConvert.ToString(data.TargetCast);
            text.UnitsInDeck = data.UnitsInDeck;
            text.ValueCastIf = data.ValueCastIf;
            return text;
        }
        public static NPCDataText ToText(NPCData data)
        {
            NPCDataText text = new();
            text.AICards = DataTextConvert.ToString(data.AICards);
            text.AuraCurseImmune = data.AuracurseImmune.ToArray();
            text.BaseMonster = DataTextConvert.ToString(data.BaseMonster);
            text.BigModel = data.BigModel;
            text.CardsInHand = data.CardsInHand;
            text.Description = data.Description;
            text.Difficulty = data.Difficulty;
            text.Energy = data.Energy;
            text.EnergyTurn = data.EnergyTurn;
            text.ExperienceReward = data.ExperienceReward;
            text.Female = data.Female;
            text.FinishCombatOnDead = data.FinishCombatOnDead;
            text.FluffOffsetX = data.FluffOffsetX;
            text.FluffOffsetY = data.FluffOffsetY;
            text.GameObjectAnimated = DataTextConvert.ToString(data.GameObjectAnimated);
            text.GoldReward = data.GoldReward;
            text.HellModeMob = DataTextConvert.ToString(data.HellModeMob);
            text.HitSound = DataTextConvert.ToString(data.HitSound);
            text.HP = data.Hp;
            text.ID = data.Id;
            text.IsBoss = data.IsBoss;
            text.IsNamed = data.IsNamed;
            text.NgPlusMob = DataTextConvert.ToString(data.NgPlusMob);
            text.NPCName = data.NPCName;
            text.PosBottom = data.PosBottom;
            text.PreferredPosition = DataTextConvert.ToString(data.PreferredPosition);
            text.ResistBlunt = data.ResistBlunt;
            text.ResistCold = data.ResistCold;
            text.ResistFire = data.ResistFire;
            text.ResistHoly = data.ResistHoly;
            text.ResistLightning = data.ResistLightning;
            text.ResistMind = data.ResistMind;
            text.ResistPiercing = data.ResistPiercing;
            text.ResistShadow = data.ResistShadow;
            text.ResistSlashing = data.ResistSlashing;
            text.ScriptableObjectName = data.ScriptableObjectName;
            text.Speed = data.Speed;
            text.Sprite = DataTextConvert.ToString(data.Sprite);
            if ((UnityEngine.Object)data.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Sprite, "NPC");
            text.SpritePortrait = DataTextConvert.ToString(data.SpritePortrait);
            if ((UnityEngine.Object)data.SpritePortrait != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpritePortrait, "NPC");
            text.SpriteSpeed = DataTextConvert.ToString(data.SpriteSpeed);
            if ((UnityEngine.Object)data.SpriteSpeed != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteSpeed, "NPC");
            text.TierMob = DataTextConvert.ToString(data.TierMob);
            text.TierReward = DataTextConvert.ToString(data.TierReward);
            text.UpgradedMob = DataTextConvert.ToString(data.UpgradedMob);
            return text;
        }
        public static PerkDataText ToText(PerkData data)
        {
            PerkDataText text = new();
            text.AdditionalCurrency = data.AdditionalCurrency;
            text.AdditionalShards = data.AdditionalShards;
            text.AuraCurseBonus = DataTextConvert.ToString(data.AuracurseBonus);
            text.AuraCurseBonusValue = data.AuracurseBonusValue;
            text.CardClass = DataTextConvert.ToString(data.CardClass);
            text.CustomDescription = data.CustomDescription;
            text.DamageFlatBonus = DataTextConvert.ToString(data.DamageFlatBonus);
            text.DamageFlatBonusValue = data.DamageFlatBonusValue;
            text.EnergyBegin = data.EnergyBegin;
            text.HealQuantity = data.HealQuantity;
            text.Icon = DataTextConvert.ToString(data.Icon);
            if ((UnityEngine.Object)data.Icon != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Icon, "perk");
            text.IconTextValue = data.IconTextValue;
            text.ID = data.Id;
            text.Level = data.Level;
            text.MainPerk = data.MainPerk;
            text.MaxHealth = data.MaxHealth;
            text.ObeliskPerk = data.ObeliskPerk;
            text.ResistModified = DataTextConvert.ToString(data.ResistModified);
            text.ResistModifiedValue = data.ResistModifiedValue;
            text.Row = data.Row;
            text.SpeedQuantity = data.SpeedQuantity;
            return text;
        }
        public static AuraCurseDataText ToText(AuraCurseData data)
        {
            AuraCurseDataText text = new();
            text.ACName = data.ACName;
            text.AuraConsumed = data.AuraConsumed;
            text.AuraDamageIncreasedPercent = data.AuraDamageIncreasedPercent;
            text.AuraDamageIncreasedPercent2 = data.AuraDamageIncreasedPercent2;
            text.AuraDamageIncreasedPercent3 = data.AuraDamageIncreasedPercent3;
            text.AuraDamageIncreasedPercent4 = data.AuraDamageIncreasedPercent4;
            text.AuraDamageIncreasedPercentPerStack = data.AuraDamageIncreasedPercentPerStack;
            text.AuraDamageIncreasedPercentPerStack2 = data.AuraDamageIncreasedPercentPerStack2;
            text.AuraDamageIncreasedPercentPerStack3 = data.AuraDamageIncreasedPercentPerStack3;
            text.AuraDamageIncreasedPercentPerStack4 = data.AuraDamageIncreasedPercentPerStack4;
            text.AuraDamageIncreasedPercentPerStackPerEnergy = data.AuraDamageIncreasedPercentPerStackPerEnergy;
            text.AuraDamageIncreasedPercentPerStackPerEnergy2 = data.AuraDamageIncreasedPercentPerStackPerEnergy2;
            text.AuraDamageIncreasedPercentPerStackPerEnergy3 = data.AuraDamageIncreasedPercentPerStackPerEnergy3;
            text.AuraDamageIncreasedPercentPerStackPerEnergy4 = data.AuraDamageIncreasedPercentPerStackPerEnergy4;
            text.AuraDamageIncreasedPerStack = data.AuraDamageIncreasedPerStack;
            text.AuraDamageIncreasedPerStack2 = data.AuraDamageIncreasedPerStack2;
            text.AuraDamageIncreasedPerStack3 = data.AuraDamageIncreasedPerStack3;
            text.AuraDamageIncreasedPerStack4 = data.AuraDamageIncreasedPerStack4;
            text.AuraDamageIncreasedTotal = data.AuraDamageIncreasedTotal;
            text.AuraDamageIncreasedTotal2 = data.AuraDamageIncreasedTotal2;
            text.AuraDamageIncreasedTotal3 = data.AuraDamageIncreasedTotal3;
            text.AuraDamageIncreasedTotal4 = data.AuraDamageIncreasedTotal4;
            text.AuraDamageType = ToString(data.AuraDamageType);
            text.AuraDamageType2 = ToString(data.AuraDamageType2);
            text.AuraDamageType3 = ToString(data.AuraDamageType3);
            text.AuraDamageType4 = ToString(data.AuraDamageType4);
            text.BlockChargesGainedPerStack = data.BlockChargesGainedPerStack;
            text.CardsDrawPerStack = data.CardsDrawPerStack;
            text.CharacterStatAbsolute = data.CharacterStatAbsolute;
            text.CharacterStatAbsoluteValue = data.CharacterStatAbsoluteValue;
            text.CharacterStatAbsoluteValuePerStack = data.CharacterStatAbsoluteValuePerStack;
            text.CharacterStatChargesMultiplierNeededForOne = data.CharacterStatChargesMultiplierNeededForOne;
            text.CharacterStatModified = ToString(data.CharacterStatModified);
            text.CharacterStatModifiedValue = data.CharacterStatModifiedValue;
            text.CharacterStatModifiedValuePerStack = data.CharacterStatModifiedValuePerStack;
            text.ChargesAuxNeedForOne1 = data.ChargesAuxNeedForOne1;
            text.ChargesAuxNeedForOne2 = data.ChargesAuxNeedForOne2;
            text.ChargesMultiplierDescription = data.ChargesMultiplierDescription;
            text.CombatlogShow = data.CombatlogShow;
            text.ConsumeAll = data.ConsumeAll;
            text.ConsumedAtCast = data.ConsumedAtCast;
            text.ConsumedAtRound = data.ConsumedAtRound;
            text.ConsumedAtRoundBegin = data.ConsumedAtRoundBegin;
            text.ConsumedAtTurn = data.ConsumedAtTurn;
            text.ConsumedAtTurnBegin = data.ConsumedAtTurnBegin;
            text.CursePreventedPerStack = data.CursePreventedPerStack;
            text.DamagePreventedPerStack = data.DamagePreventedPerStack;
            text.DamageReflectedConsumeCharges = data.DamageReflectedConsumeCharges;
            text.DamageReflectedType = ToString(data.DamageReflectedType);
            text.DamageSidesWhenConsumed = data.DamageSidesWhenConsumed;
            text.DamageSidesWhenConsumedPerCharge = data.DamageSidesWhenConsumedPerCharge;
            text.DamageTypeWhenConsumed = ToString(data.DamageTypeWhenConsumed);
            text.DamageWhenConsumed = data.DamageWhenConsumed;
            text.DamageWhenConsumedPerCharge = data.DamageWhenConsumedPerCharge;
            text.Description = data.Description;
            text.DieWhenConsumedAll = data.DieWhenConsumedAll;
            text.DisabledCardTypes = ToString(data.DisabledCardTypes);
            text.DoubleDamageIfCursesLessThan = data.DoubleDamageIfCursesLessThan;
            text.EffectTick = data.EffectTick;
            text.EffectTickSides = data.EffectTickSides;
            text.ExplodeAtStacks = data.ExplodeAtStacks;
            text.GainAuraCurseConsumption = ToString(data.GainAuraCurseConsumption);
            text.GainAuraCurseConsumption2 = ToString(data.GainAuraCurseConsumption2);
            text.GainAuraCurseConsumptionPerCharge = data.GainAuraCurseConsumptionPerCharge;
            text.GainAuraCurseConsumptionPerCharge2 = data.GainAuraCurseConsumptionPerCharge2;
            text.GainCharges = data.GainCharges;
            text.GainChargesFromThisAuraCurse = ToString(data.GainChargesFromThisAuraCurse);
            text.GainChargesFromThisAuraCurse2 = ToString(data.GainChargesFromThisAuraCurse2);
            text.HealAttackerConsumeCharges = data.HealAttackerConsumeCharges;
            text.HealAttackerPerStack = data.HealAttackerPerStack;
            text.HealDonePercent = data.HealDonePercent;
            text.HealDonePercentPerStack = data.HealDonePercentPerStack;
            text.HealDonePercentPerStackPerEnergy = data.HealDonePercentPerStackPerEnergy;
            text.HealDonePerStack = data.HealDonePerStack;
            text.HealReceivedTotal = data.HealReceivedTotal;
            text.HealSidesWhenConsumed = data.HealSidesWhenConsumed;
            text.HealSidesWhenConsumedPerCharge = data.HealSidesWhenConsumedPerCharge;
            text.HealWhenConsumed = data.HealWhenConsumed;
            text.HealWhenConsumedPerCharge = data.HealWhenConsumedPerCharge;
            text.IconShow = data.IconShow;
            text.ID = data.Id;
            text.IncreasedDamageReceivedType = ToString(data.IncreasedDamageReceivedType);
            text.IncreasedDamageReceivedType2 = ToString(data.IncreasedDamageReceivedType2);
            text.IncreasedDirectDamageChargesMultiplierNeededForOne = data.IncreasedDirectDamageChargesMultiplierNeededForOne;
            text.IncreasedDirectDamageChargesMultiplierNeededForOne2 = data.IncreasedDirectDamageChargesMultiplierNeededForOne2;
            text.IncreasedDirectDamageReceivedPerStack = data.IncreasedDirectDamageReceivedPerStack;
            text.IncreasedDirectDamageReceivedPerStack2 = data.IncreasedDirectDamageReceivedPerStack2;
            text.IncreasedDirectDamageReceivedPerTurn = data.IncreasedDirectDamageReceivedPerTurn;
            text.IncreasedDirectDamageReceivedPerTurn2 = data.IncreasedDirectDamageReceivedPerTurn2;
            text.IncreasedPercentDamageReceivedPerStack = data.IncreasedPercentDamageReceivedPerStack;
            text.IncreasedPercentDamageReceivedPerStack2 = data.IncreasedPercentDamageReceivedPerStack2;
            text.IncreasedPercentDamageReceivedPerTurn = data.IncreasedPercentDamageReceivedPerTurn;
            text.IncreasedPercentDamageReceivedPerTurn2 = data.IncreasedPercentDamageReceivedPerTurn2;
            text.Invulnerable = data.Invulnerable;
            text.IsAura = data.IsAura;
            text.MaxCharges = data.MaxCharges;
            text.MaxMadnessCharges = data.MaxMadnessCharges;
            text.ModifyCardCostPerChargeNeededForOne = data.ModifyCardCostPerChargeNeededForOne;
            text.NoRemoveBlockAtTurnEnd = data.NoRemoveBlockAtTurnEnd;
            text.Preventable = data.Preventable;
            text.PreventedAuraCurse = ToString(data.PreventedAuraCurse);
            text.PreventedAuraCurseStackPerStack = data.PreventedAuraCurseStackPerStack;
            text.PreventedDamagePerStack = data.PreventedDamagePerStack;
            text.PreventedDamageTypePerStack = ToString(data.PreventedDamageTypePerStack);
            text.PriorityOnConsumption = data.PriorityOnConsumption;
            text.ProduceDamageWhenConsumed = data.ProduceDamageWhenConsumed;
            text.ProduceHealWhenConsumed = data.ProduceHealWhenConsumed;
            text.Removable = data.Removable;
            text.RemoveAuraCurse = ToString(data.RemoveAuraCurse);
            text.RemoveAuraCurse2 = ToString(data.RemoveAuraCurse2);
            text.ResistModified = ToString(data.ResistModified);
            text.ResistModified2 = ToString(data.ResistModified2);
            text.ResistModified3 = ToString(data.ResistModified3);
            text.ResistModifiedPercentagePerStack = data.ResistModifiedPercentagePerStack;
            text.ResistModifiedPercentagePerStack2 = data.ResistModifiedPercentagePerStack2;
            text.ResistModifiedPercentagePerStack3 = data.ResistModifiedPercentagePerStack3;
            text.ResistModifiedValue = data.ResistModifiedValue;
            text.ResistModifiedValue2 = data.ResistModifiedValue2;
            text.ResistModifiedValue3 = data.ResistModifiedValue3;
            text.RevealCardsPerCharge = data.RevealCardsPerCharge;
            text.SkipsNextTurn = data.SkipsNextTurn;
            text.Sound = ToString(data.Sound);
            text.Sprite = ToString(data.Sprite);
            if ((UnityEngine.Object)data.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Sprite, "auraCurse");
            text.Stealth = data.Stealth;
            text.Taunt = data.Taunt;
            return text;
        }
        public static NodeDataText ToText(NodeData data)
        {
            NodeDataText text = new();
            text.CombatPercent = data.CombatPercent;
            text.Description = data.Description;
            text.DisableCorruption = data.DisableCorruption;
            text.DisableRandom = data.DisableRandom;
            text.EventPercent = data.EventPercent;
            text.ExistsPercent = data.ExistsPercent;
            text.ExistsSku = data.ExistsSku;
            text.GoToTown = data.GoToTown;
            text.NodeBackgroundImg = ToString(data.NodeBackgroundImg);
            if ((UnityEngine.Object)data.NodeBackgroundImg != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.NodeBackgroundImg, "node");
            text.NodeCombat = ToString(data.NodeCombat);
            text.NodeCombatTier = ToString(data.NodeCombatTier);
            text.NodeEvent = ToString(data.NodeEvent);
            text.NodeEventPercent = data.NodeEventPercent;
            text.NodeEventPriority = data.NodeEventPriority;
            text.NodeEventTier = ToString(data.NodeEventTier);
            text.NodeGround = ToString(data.NodeGround);
            text.NodeId = data.NodeId;
            text.NodeName = data.NodeName;
            text.NodeRequirement = ToString(data.NodeRequirement);
            text.NodesConnected = ToString(data.NodesConnected);
            text.NodesConnectedRequirement = ToString(data.NodesConnectedRequirement);
            text.NodeZone = ToString(data.NodeZone);
            text.TravelDestination = data.TravelDestination;
            text.VisibleIfNotRequirement = data.VisibleIfNotRequirement;
            return text;
        }
        public static KeyNotesDataText ToText(KeyNotesData data)
        {
            KeyNotesDataText text = new();
            text.ID = data.Id;
            text.KeynoteName = data.KeynoteName;
            text.Description = data.Description;
            text.DescriptionExtended = data.DescriptionExtended;
            return text;
        }
        public static LootDataText ToText(LootData data)
        {
            LootDataText text = new();
            text.DefaultPercentEpic = data.DefaultPercentEpic;
            text.DefaultPercentMythic = data.DefaultPercentMythic;
            text.DefaultPercentRare = data.DefaultPercentRare;
            text.DefaultPercentUncommon = data.DefaultPercentUncommon;
            text.GoldQuantity = data.GoldQuantity;
            text.ID = data.Id;
            text.LootItemTable = ToString(data.LootItemTable);
            text.NumItems = data.NumItems;
            return text;
        }
        public static LootItemText ToText(LootItem data)
        {
            LootItemText text = new();
            text.LootCard = ToString(data.LootCard);
            text.LootPercent = data.LootPercent;
            text.LootRarity = ToString(data.LootRarity);
            text.LootType = ToString(data.LootType);
            return text;
        }
        public static PerkNodeDataText ToText(PerkNodeData data)
        {
            PerkNodeDataText text = new();
            text.Column = data.Column;
            text.Cost = ToString(data.Cost);
            text.ID = data.Id;
            text.LockedInTown = data.LockedInTown;
            text.NotStack = data.NotStack;
            text.Perk = ToString(data.Perk);
            text.PerkRequired = ToString(data.PerkRequired);
            text.PerksConnected = ToString(data.PerksConnected);
            text.Row = data.Row;
            text.Sprite = ToString(data.Sprite);
            if ((UnityEngine.Object)data.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Sprite, "perkNode");
            text.Type = data.Type;
            return text;
        }
        public static ChallengeDataText ToText(ChallengeData data)
        {
            ChallengeDataText text = new();
            text.Boss1 = ToString(data.Boss1);
            text.Boss2 = ToString(data.Boss2);
            text.BossCombat = ToString(data.BossCombat);
            text.CorruptionList = ToString(data.CorruptionList.ToArray());
            text.Hero1 = ToString(data.Hero1);
            text.Hero2 = ToString(data.Hero2);
            text.Hero3 = ToString(data.Hero3);
            text.Hero4 = ToString(data.Hero4);
            text.ID = data.Id;
            text.IDSteam = data.IdSteam;
            text.Loot = ToString(data.Loot);
            text.Seed = data.Seed;
            text.Traits = ToString(data.Traits.ToArray());
            text.Week = data.Week;
            return text;
        }
        public static ChallengeTraitText ToText(ChallengeTrait data)
        {
            ChallengeTraitText text = new();
            text.Icon = ToString(data.Icon);
            if ((UnityEngine.Object)data.Icon != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.Icon, "challengeTrait");
            text.ID = data.Id;
            text.IsMadnessTrait = data.IsMadnessTrait;
            text.Name = data.Name;
            text.Order = data.Order;
            return text;
        }
        public static CombatDataText ToText(CombatData data)
        {
            CombatDataText text = new();
            text.CinematicData = ToString(data.CinematicData);
            text.CombatBackground = ToString(data.CombatBackground);
            text.CombatEffect = ToString(data.CombatEffect);
            text.CombatID = data.CombatId;
            text.CombatMusic = ToString(data.CombatMusic);
            text.CombatTier = ToString(data.CombatTier);
            text.Description = data.Description;
            text.EventData = ToString(data.EventData);
            text.EventRequirementData = ToString(data.EventRequirementData);
            text.HealHeroes = data.HealHeroes;
            text.NPCList = ToString(data.NPCList);
            text.NPCRemoveInMadness0Index = data.NpcRemoveInMadness0Index;
            text.ThermometerTierData = ToString(data.ThermometerTierData);
            return text;
        }
        public static CombatEffectText ToText(CombatEffect data)
        {
            CombatEffectText text = new();
            text.AuraCurse = ToString(data.AuraCurse);
            text.AuraCurseCharges = data.AuraCurseCharges;
            text.AuraCurseTarget = ToString(data.AuraCurseTarget);
            return text;
        }
        public static EventDataText ToText(EventData data)
        {
            EventDataText text = new();
            text.Description = data.Description;
            text.DescriptionAction = data.DescriptionAction;
            text.EventIconShader = ToString(data.EventIconShader);
            text.EventID = data.EventId;
            text.EventName = data.EventName;
            text.EventSpriteBook = ToString(data.EventSpriteBook);
            if ((UnityEngine.Object)data.EventSpriteBook != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.EventSpriteBook, "event");
            text.EventSpriteDecor = ToString(data.EventSpriteDecor);
            if ((UnityEngine.Object)data.EventSpriteDecor != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.EventSpriteDecor, "event");
            text.EventSpriteMap = ToString(data.EventSpriteMap);
            if ((UnityEngine.Object)data.EventSpriteMap != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.EventSpriteMap, "event");
            text.EventTier = ToString(data.EventTier);
            text.EventUniqueID = data.EventUniqueId;
            text.HistoryMode = data.HistoryMode;
            text.ReplyRandom = data.ReplyRandom;
            text.Replies = new string[data.Replys.Length];
            for (int a = 0; a < data.Replys.Length; a++)
            {
                text.Replies[a] = data.EventId + a.ToString();
                Plugin.medsEventReplyData[data.EventId + a.ToString()] = data.Replys[a];
            }
            text.RequiredClass = ToString(data.RequiredClass);
            text.Requirement = ToString(data.Requirement);
            return text;
        }
        public static EventRequirementDataText ToText(EventRequirementData data)
        {
            EventRequirementDataText text = new();
            text.AssignToPlayerAtBegin = data.AssignToPlayerAtBegin;
            text.Description = data.Description;
            text.ItemSprite = ToString(data.ItemSprite);
            if ((UnityEngine.Object)data.ItemSprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.ItemSprite, "eventRequirement");
            text.RequirementID = data.RequirementId;
            text.RequirementName = data.RequirementName;
            text.RequirementTrack = data.RequirementTrack;
            text.TrackSprite = ToString(data.TrackSprite);
            if ((UnityEngine.Object)data.TrackSprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.TrackSprite, "eventRequirement");
            return text;
        }
        public static EventReplyDataText ToText(EventReplyData data)
        {
            EventReplyDataText text = new();
            text.DustCost = data.DustCost;
            text.GoldCost = data.GoldCost;
            text.IndexForAnswerTranslation = data.IndexForAnswerTranslation;
            text.RepeatForAllCharacters = data.RepeatForAllCharacters;
            text.ReplyActionText = ToString(data.ReplyActionText);
            text.ReplyShowCard = ToString(data.ReplyShowCard);
            text.ReplyText = data.ReplyText;
            text.RequiredClass = ToString(data.RequiredClass);
            text.Requirement = ToString(data.Requirement);
            text.RequirementBlocked = ToString(data.RequirementBlocked);
            text.RequirementItem = ToString(data.RequirementItem);
            text.RequirementMultiplayer = data.RequirementMultiplayer;
            text.RequirementSku = data.RequirementSku;

            text.SSAddCard1 = ToString(data.SsAddCard1);
            text.SSAddCard2 = ToString(data.SsAddCard2);
            text.SSAddCard3 = ToString(data.SsAddCard3);
            text.SSAddItem = ToString(data.SsAddItem);
            text.SSCardPlayerGame = data.SsCardPlayerGame;
            text.SSCardPlayerGamePackData = ToString(data.SsCardPlayerGamePackData);
            text.SSCharacterReplacement = ToString(data.SsCharacterReplacement);
            text.SSCharacterReplacementPosition = data.SsCharacterReplacementPosition;
            text.SSCombat = ToString(data.SsCombat);
            text.SSCorruptionUI = data.SsCorruptionUI;
            text.SSCorruptItemSlot = ToString(data.SsCorruptItemSlot);
            text.SSCraftUI = data.SsCraftUI;
            text.SSCraftUIMaxType = ToString(data.SsCraftUIMaxType);
            text.SSDiscount = data.SsDiscount;
            text.SSDustReward = data.SsDustReward;
            text.SSEvent = ToString(data.SsEvent);
            text.SSExperienceReward = data.SsExperienceReward;
            text.SSFinishEarlyAccess = data.SsFinishEarlyAccess;
            text.SSFinishGame = data.SsFinishGame;
            text.SSFinishObeliskMap = data.SsFinishObeliskMap;
            text.SSGoldReward = data.SsGoldReward;
            text.SSHealerUI = data.SsHealerUI;
            text.SSLootList = ToString(data.SsLootList);
            text.SSMaxQuantity = data.SsMaxQuantity;
            text.SSMerchantUI = data.SsMerchantUI;
            text.SSNodeTravel = ToString(data.SsNodeTravel);
            text.SSPerkData = ToString(data.SsPerkData);
            text.SSPerkData1 = ToString(data.SsPerkData1);
            text.SSRemoveItemSlot = ToString(data.SsRemoveItemSlot);
            text.SSRequirementLock = ToString(data.SsRequirementLock);
            text.SSRequirementLock2 = ToString(data.SsRequirementLock2);
            text.SSRequirementUnlock = ToString(data.SsRequirementUnlock);
            text.SSRequirementUnlock2 = ToString(data.SsRequirementUnlock2);
            text.SSRewardHealthFlat = data.SsRewardHealthFlat;
            text.SSRewardHealthPercent = data.SsRewardHealthPercent;
            text.SSRewardText = data.SsRewardText;
            text.SSRewardTier = ToString(data.SsRewardTier);
            text.SSRoll = data.SsRoll;
            text.SSRollCard = ToString(data.SsRollCard);
            text.SSRollMode = ToString(data.SsRollMode);
            text.SSRollNumber = data.SsRollNumber;
            text.SSRollNumberCritical = data.SsRollNumberCritical;
            text.SSRollNumberCriticalFail = data.SsRollNumberCriticalFail;
            text.SSRollTarget = ToString(data.SsRollTarget);
            text.SSShopList = ToString(data.SsShopList);
            text.SSSteamStat = data.SsSteamStat;
            text.SSSupplyReward = data.SsSupplyReward;
            text.SSUnlockClass = ToString(data.SsUnlockClass);
            text.SSUnlockSkin = ToString(data.SsUnlockSkin);
            text.SSUnlockSteamAchievement = data.SsUnlockSteamAchievement;
            text.SSUpgradeRandomCard = data.SsUpgradeRandomCard;
            text.SSUpgradeUI = data.SsUpgradeUI;

            text.SSCAddCard1 = ToString(data.SscAddCard1);
            text.SSCAddCard2 = ToString(data.SscAddCard2);
            text.SSCAddCard3 = ToString(data.SscAddCard3);
            text.SSCAddItem = ToString(data.SscAddItem);
            text.SSCCardPlayerGame = data.SscCardPlayerGame;
            text.SSCCardPlayerGamePackData = ToString(data.SscCardPlayerGamePackData);
            text.SSCCombat = ToString(data.SscCombat);
            text.SSCCorruptionUI = data.SscCorruptionUI;
            text.SSCCorruptItemSlot = ToString(data.SscCorruptItemSlot);
            text.SSCCraftUI = data.SscCraftUI;
            text.SSCCraftUIMaxType = ToString(data.SscCraftUIMaxType);
            text.SSCDiscount = data.SscDiscount;
            text.SSCDustReward = data.SscDustReward;
            text.SSCEvent = ToString(data.SscEvent);
            text.SSCExperienceReward = data.SscExperienceReward;
            text.SSCFinishEarlyAccess = data.SscFinishEarlyAccess;
            text.SSCFinishGame = data.SscFinishGame;
            text.SSCGoldReward = data.SscGoldReward;
            text.SSCHealerUI = data.SscHealerUI;
            text.SSCLootList = ToString(data.SscLootList);
            text.SSCMaxQuantity = data.SscMaxQuantity;
            text.SSCMerchantUI = data.SscMerchantUI;
            text.SSCNodeTravel = ToString(data.SscNodeTravel);
            text.SSCRemoveItemSlot = ToString(data.SscRemoveItemSlot);
            text.SSCRequirementLock = ToString(data.SscRequirementLock);
            text.SSCRequirementUnlock = ToString(data.SscRequirementUnlock);
            text.SSCRequirementUnlock2 = ToString(data.SscRequirementUnlock2);
            text.SSCRewardHealthFlat = data.SscRewardHealthFlat;
            text.SSCRewardHealthPercent = data.SscRewardHealthPercent;
            text.SSCRewardText = data.SscRewardText;
            text.SSCRewardTier = ToString(data.SscRewardTier);
            text.SSCShopList = ToString(data.SscShopList);
            text.SSCSupplyReward = data.SscSupplyReward;
            text.SSCUnlockClass = ToString(data.SscUnlockClass);
            text.SSCUnlockSteamAchievement = data.SscUnlockSteamAchievement;
            text.SSCUpgradeRandomCard = data.SscUpgradeRandomCard;
            text.SSCUpgradeUI = data.SscUpgradeUI;

            text.FLAddCard1 = ToString(data.FlAddCard1);
            text.FLAddCard2 = ToString(data.FlAddCard2);
            text.FLAddCard3 = ToString(data.FlAddCard3);
            text.FLAddItem = ToString(data.FlAddItem);
            text.FLCardPlayerGame = data.FlCardPlayerGame;
            text.FLCardPlayerGamePackData = ToString(data.FlCardPlayerGamePackData);
            text.FLCombat = ToString(data.FlCombat);
            text.FLCorruptionUI = data.FlCorruptionUI;
            text.FLCorruptItemSlot = ToString(data.FlCorruptItemSlot);
            text.FLCraftUI = data.FlCraftUI;
            text.FLCraftUIMaxType = ToString(data.FlCraftUIMaxType);
            text.FLDiscount = data.FlDiscount;
            text.FLDustReward = data.FlDustReward;
            text.FLEvent = ToString(data.FlEvent);
            text.FLExperienceReward = data.FlExperienceReward;
            text.FLGoldReward = data.FlGoldReward;
            text.FLHealerUI = data.FlHealerUI;
            text.FLLootList = ToString(data.FlLootList);
            text.FLMaxQuantity = data.FlMaxQuantity;
            text.FLMerchantUI = data.FlMerchantUI;
            text.FLNodeTravel = ToString(data.FlNodeTravel);
            text.FLRemoveItemSlot = ToString(data.FlRemoveItemSlot);
            text.FLRequirementLock = ToString(data.FlRequirementLock);
            text.FLRequirementUnlock = ToString(data.FlRequirementUnlock);
            text.FLRequirementUnlock2 = ToString(data.FlRequirementUnlock2);
            text.FLRewardHealthFlat = data.FlRewardHealthFlat;
            text.FLRewardHealthPercent = data.FlRewardHealthPercent;
            text.FLRewardText = data.FlRewardText;
            text.FLRewardTier = ToString(data.FlRewardTier);
            text.FLShopList = ToString(data.FlShopList);
            text.FLSupplyReward = data.FlSupplyReward;
            text.FLUnlockClass = ToString(data.FlUnlockClass);
            text.FLUnlockSteamAchievement = data.FlUnlockSteamAchievement;
            text.FLUpgradeRandomCard = data.FlUpgradeRandomCard;
            text.FLUpgradeUI = data.FlUpgradeUI;

            text.FLCAddCard1 = ToString(data.FlcAddCard1);
            text.FLCAddCard2 = ToString(data.FlcAddCard2);
            text.FLCAddCard3 = ToString(data.FlcAddCard3);
            text.FLCAddItem = ToString(data.FlcAddItem);
            text.FLCCardPlayerGame = data.FlcCardPlayerGame;
            text.FLCCardPlayerGamePackData = ToString(data.FlcCardPlayerGamePackData);
            text.FLCCombat = ToString(data.FlcCombat);
            text.FLCCorruptionUI = data.FlcCorruptionUI;
            text.FLCCorruptItemSlot = ToString(data.FlcCorruptItemSlot);
            text.FLCCraftUI = data.FlcCraftUI;
            text.FLCCraftUIMaxType = ToString(data.FlcCraftUIMaxType);
            text.FLCDiscount = data.FlcDiscount;
            text.FLCDustReward = data.FlcDustReward;
            text.FLCEvent = ToString(data.FlcEvent);
            text.FLCExperienceReward = data.FlcExperienceReward;
            text.FLCGoldReward = data.FlcGoldReward;
            text.FLCHealerUI = data.FlcHealerUI;
            text.FLCLootList = ToString(data.FlcLootList);
            text.FLCMaxQuantity = data.FlcMaxQuantity;
            text.FLCMerchantUI = data.FlcMerchantUI;
            text.FLCNodeTravel = ToString(data.FlcNodeTravel);
            text.FLCRemoveItemSlot = ToString(data.FlcRemoveItemSlot);
            text.FLCRequirementLock = ToString(data.FlcRequirementLock);
            text.FLCRequirementUnlock = ToString(data.FlcRequirementUnlock);
            text.FLCRequirementUnlock2 = ToString(data.FlcRequirementUnlock2);
            text.FLCRewardHealthFlat = data.FlcRewardHealthFlat;
            text.FLCRewardHealthPercent = data.FlcRewardHealthPercent;
            text.FLCRewardText = data.FlcRewardText;
            text.FLCRewardTier = ToString(data.FlcRewardTier);
            text.FLCShopList = ToString(data.FlcShopList);
            text.FLCSupplyReward = data.FlcSupplyReward;
            text.FLCUnlockClass = ToString(data.FlcUnlockClass);
            text.FLCUnlockSteamAchievement = data.FlcUnlockSteamAchievement;
            text.FLCUpgradeRandomCard = data.FlcUpgradeRandomCard;
            text.FLCUpgradeUI = data.FlcUpgradeUI;

            return text;
        }
        public static ZoneDataText ToText(ZoneData data)
        {
            ZoneDataText text = new();
            text.ChangeTeamOnEntrance = data.ChangeTeamOnEntrance;
            text.DisableExperienceOnThisZone = data.DisableExperienceOnThisZone;
            text.DisableMadnessOnThisZone = data.DisableMadnessOnThisZone;
            text.NewTeam = ToString(data.NewTeam.ToArray());
            text.ObeliskFinal = data.ObeliskFinal;
            text.ObeliskHigh = data.ObeliskHigh;
            text.ObeliskLow = data.ObeliskLow;
            text.RestoreTeamOnExit = data.RestoreTeamOnExit;
            text.ZoneID = data.ZoneId;
            text.ZoneName = data.ZoneName;
            return text;
        }
        /* KeyNotesData already appropriately serializable?
        public static KeyNotesDataText ToText(KeyNotesData data)
        {
            KeyNotesDataText text = new();

            return text;
        }
        */
        public static PackDataText ToText(PackData data)
        {
            PackDataText text = new();
            text.Card0 = ToString(data.Card0);
            text.Card1 = ToString(data.Card1);
            text.Card2 = ToString(data.Card2);
            text.Card3 = ToString(data.Card3);
            text.Card4 = ToString(data.Card4);
            text.Card5 = ToString(data.Card5);
            text.CardSpecial0 = ToString(data.CardSpecial0);
            text.CardSpecial1 = ToString(data.CardSpecial1);
            text.PackClass = ToString(data.PackClass);
            text.PackID = data.PackId;
            text.PackName = data.PackName;
            text.PerkList = ToString(data.PerkList.ToArray());
            text.RequiredClass = ToString(data.RequiredClass);
            return text;
        }
        public static NodesConnectedRequirementText ToText(NodesConnectedRequirement data)
        {
            NodesConnectedRequirementText text = new();
            text.NodeData = ToString(data.NodeData);
            text.ConnectionRequirement = ToString(data.ConectionRequeriment);
            text.ConnectionIfNotNode = ToString(data.ConectionIfNotNode);
            return text;
        }
        public static CorruptionPackDataText ToText(CorruptionPackData data)
        {
            CorruptionPackDataText text = new();
            text.HighPack = ToString(data.HighPack.ToArray());
            text.LowPack = ToString(data.LowPack.ToArray());
            text.PackClass = ToString(data.PackClass);
            text.PackName = data.PackName;
            text.PackTier = data.PackTier;
            return text;
        }
        public static CardPlayerPackDataText ToText(CardPlayerPackData data)
        {
            CardPlayerPackDataText text = new();
            text.Card0 = ToString(data.Card0);
            text.Card0RandomBoon = data.Card0RandomBoon;
            text.Card0RandomInjury = data.Card0RandomInjury;
            text.Card1 = ToString(data.Card1);
            text.Card1RandomBoon = data.Card1RandomBoon;
            text.Card1RandomInjury = data.Card1RandomInjury;
            text.Card2 = ToString(data.Card2);
            text.Card2RandomBoon = data.Card2RandomBoon;
            text.Card2RandomInjury = data.Card2RandomInjury;
            text.Card3 = ToString(data.Card3);
            text.Card3RandomBoon = data.Card3RandomBoon;
            text.Card3RandomInjury = data.Card3RandomInjury;
            text.ModIterations = data.ModIterations;
            text.ModSpeed = data.ModSpeed;
            text.PackId = data.PackId;
            return text;
        }
        public static CardbackDataText ToText(CardbackData data)
        {
            CardbackDataText text = new();
            text.AdventureLevel = data.AdventureLevel;
            text.BaseCardback = data.BaseCardback;
            text.CardbackID = data.CardbackId;
            text.CardbackName = data.CardbackName;
            text.CardbackOrder = data.CardbackOrder;
            text.CardbackSprite = ToString(data.CardbackSprite);
            if ((UnityEngine.Object)data.CardbackSprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.CardbackSprite, "cardback");
            text.CardbackSubclass = ToString(data.CardbackSubclass);
            text.Locked = data.Locked;
            text.ObeliskLevel = data.ObeliskLevel;
            text.RankLevel = data.RankLevel;
            text.ShowIfLocked = data.ShowIfLocked;
            text.Sku = data.Sku;
            text.SteamStat = data.SteamStat;
            return text;
        }
        public static SkinDataText ToText(SkinData data)
        {
            SkinDataText text = new();
            text.BaseSkin = data.BaseSkin;
            text.PerkLevel = data.PerkLevel;
            text.SkinGo = ToString(data.SkinGo);
            text.SkinID = data.SkinId;
            text.SkinName = data.SkinName;
            text.SkinOrder = data.SkinOrder;
            text.SkinSubclass = ToString(data.SkinSubclass);
            text.Sku = data.Sku;
            text.SpritePortrait = ToString(data.SpritePortrait);
            if ((UnityEngine.Object)data.SpritePortrait != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpritePortrait, "skin");
            text.SpritePortraitGrande = ToString(data.SpritePortraitGrande);
            if ((UnityEngine.Object)data.SpritePortraitGrande != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpritePortraitGrande, "skin");
            text.SpriteSilueta = ToString(data.SpriteSilueta);
            if ((UnityEngine.Object)data.SpriteSilueta != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteSilueta, "skin");
            text.SpriteSiluetaGrande = ToString(data.SpriteSiluetaGrande);
            if ((UnityEngine.Object)data.SpriteSiluetaGrande != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(data.SpriteSiluetaGrande, "skin");
            text.SteamStat = data.SteamStat;
            return text;
        }
        public static CinematicDataText ToText(CinematicData data)
        {
            CinematicDataText text = new();
            text.CinematicBSO = ToString(data.CinematicBSO);
            text.CinematicCombat = ToString(data.CinematicCombat);
            text.CinematicEndAdventure = data.CinematicEndAdventure;
            text.CinematicEvent = ToString(data.CinematicEvent);
            text.CinematicGo = ToString(data.CinematicGo);
            text.CinematicID = data.CinematicId;
            return text;
        }

        /*
         *                                                                                           
         *    888888888888  ,ad8888ba,          88888888ba,         db    888888888888    db         
         *         88      d8"'    `"8b         88      `"8b       d88b        88        d88b        
         *         88     d8'        `8b        88        `8b     d8'`8b       88       d8'`8b       
         *         88     88          88        88         88    d8'  `8b      88      d8'  `8b      
         *         88     88          88        88         88   d8YaaaaY8b     88     d8YaaaaY8b     
         *         88     Y8,        ,8P        88         8P  d8""""""""8b    88    d8""""""""8b    
         *         88      Y8a.    .a8P         88      .a8P  d8'        `8b   88   d8'        `8b   
         *         88       `"Y8888Y"'          88888888Y"'  d8'          `8b  88  d8'          `8b  
         *
         *   Converts to corresponding AtO type (including string->Enums).
         */


        public static int ToData<T>(string text)
        {
            // I wanted to use this to convert enum arrays in a single line, but it's more effect than it's worth.
            // if (typeof(T).IsArray && typeof(T).GetElementType().BaseType == typeof(System.Enum))
            if (typeof(T).BaseType == typeof(System.Enum))
            {
                try
                {
                    return (int)Enum.Parse(typeof(T), text, true);
                }
                catch
                {
                    Plugin.Log.LogError("ToData<T> has captured a value of type " + typeof(T) + " that it cannot parse: " + text);
                    return 0;
                }
            }
            Plugin.Log.LogError("ToData<T> is capturing a type that it isn't set up for!!! " + typeof(T));
            return 0;
        }

        public static AuraCurseData ToData(AuraCurseDataText text)
        {
            AuraCurseData data = ScriptableObject.CreateInstance<AuraCurseData>();
            /*if (Plugin.medsAurasCursesSource.ContainsKey(text.ID))
                data = Plugin.medsAurasCursesSource[text.ID];
            else
                data = Plugin.medsAurasCursesSource["bless"];*/
            
            data.ACName = text.ACName;
            data.name = text.ACName;
            data.AuraConsumed = text.AuraConsumed;
            data.AuraDamageIncreasedPercent = text.AuraDamageIncreasedPercent;
            data.AuraDamageIncreasedPercent2 = text.AuraDamageIncreasedPercent2;
            data.AuraDamageIncreasedPercent3 = text.AuraDamageIncreasedPercent3;
            data.AuraDamageIncreasedPercent4 = text.AuraDamageIncreasedPercent4;
            data.AuraDamageIncreasedPercentPerStack = text.AuraDamageIncreasedPercentPerStack;
            data.AuraDamageIncreasedPercentPerStack2 = text.AuraDamageIncreasedPercentPerStack2;
            data.AuraDamageIncreasedPercentPerStack3 = text.AuraDamageIncreasedPercentPerStack3;
            data.AuraDamageIncreasedPercentPerStack4 = text.AuraDamageIncreasedPercentPerStack4;
            data.AuraDamageIncreasedPercentPerStackPerEnergy = text.AuraDamageIncreasedPercentPerStackPerEnergy;
            data.AuraDamageIncreasedPercentPerStackPerEnergy2 = text.AuraDamageIncreasedPercentPerStackPerEnergy2;
            data.AuraDamageIncreasedPercentPerStackPerEnergy3 = text.AuraDamageIncreasedPercentPerStackPerEnergy3;
            data.AuraDamageIncreasedPercentPerStackPerEnergy4 = text.AuraDamageIncreasedPercentPerStackPerEnergy4;
            data.AuraDamageIncreasedPerStack = text.AuraDamageIncreasedPerStack;
            data.AuraDamageIncreasedPerStack2 = text.AuraDamageIncreasedPerStack2;
            data.AuraDamageIncreasedPerStack3 = text.AuraDamageIncreasedPerStack3;
            data.AuraDamageIncreasedPerStack4 = text.AuraDamageIncreasedPerStack4;
            data.AuraDamageIncreasedTotal = text.AuraDamageIncreasedTotal;
            data.AuraDamageIncreasedTotal2 = text.AuraDamageIncreasedTotal2;
            data.AuraDamageIncreasedTotal3 = text.AuraDamageIncreasedTotal3;
            data.AuraDamageIncreasedTotal4 = text.AuraDamageIncreasedTotal4;
            data.AuraDamageType = (DamageType)ToData<DamageType>(text.AuraDamageType);
            data.AuraDamageType2 = (DamageType)ToData<DamageType>(text.AuraDamageType2);
            data.AuraDamageType3 = (DamageType)ToData<DamageType>(text.AuraDamageType3);
            data.AuraDamageType4 = (DamageType)ToData<DamageType>(text.AuraDamageType4);
            data.BlockChargesGainedPerStack = text.BlockChargesGainedPerStack;
            data.CardsDrawPerStack = text.CardsDrawPerStack;
            data.CharacterStatAbsolute = text.CharacterStatAbsolute;
            data.CharacterStatAbsoluteValue = text.CharacterStatAbsoluteValue;
            data.CharacterStatAbsoluteValuePerStack = text.CharacterStatAbsoluteValuePerStack;
            data.CharacterStatChargesMultiplierNeededForOne = text.CharacterStatChargesMultiplierNeededForOne;
            data.CharacterStatModified = (CharacterStat)ToData<CharacterStat>(text.CharacterStatModified);
            data.CharacterStatModifiedValue = text.CharacterStatModifiedValue;
            data.CharacterStatModifiedValuePerStack = text.CharacterStatModifiedValuePerStack;
            data.ChargesAuxNeedForOne1 = text.ChargesAuxNeedForOne1;
            data.ChargesAuxNeedForOne2 = text.ChargesAuxNeedForOne2;
            data.ChargesMultiplierDescription = text.ChargesMultiplierDescription;
            data.CombatlogShow = text.CombatlogShow;
            data.ConsumeAll = text.ConsumeAll;
            data.ConsumedAtCast = text.ConsumedAtCast;
            data.ConsumedAtRound = text.ConsumedAtRound;
            data.ConsumedAtRoundBegin = text.ConsumedAtRoundBegin;
            data.ConsumedAtTurn = text.ConsumedAtTurn;
            data.ConsumedAtTurnBegin = text.ConsumedAtTurnBegin;
            data.CursePreventedPerStack = text.CursePreventedPerStack;
            data.DamagePreventedPerStack = text.DamagePreventedPerStack;
            data.DamageReflectedConsumeCharges = text.DamageReflectedConsumeCharges;
            data.DamageReflectedType = (DamageType)ToData<DamageType>(text.DamageReflectedType);
            data.DamageSidesWhenConsumed = text.DamageSidesWhenConsumed;
            data.DamageSidesWhenConsumedPerCharge = text.DamageSidesWhenConsumedPerCharge;
            data.DamageTypeWhenConsumed = (DamageType)ToData<DamageType>(text.DamageTypeWhenConsumed);
            data.DamageWhenConsumed = text.DamageWhenConsumed;
            data.DamageWhenConsumedPerCharge = text.DamageWhenConsumedPerCharge;
            data.Description = text.Description;
            data.DieWhenConsumedAll = text.DieWhenConsumedAll;
            data.DisabledCardTypes = new CardType[text.DisabledCardTypes.Length];
            for (int a = 0; a < text.DisabledCardTypes.Length; a++)
                data.DisabledCardTypes[a] = (CardType)ToData<CardType>(text.DisabledCardTypes[a]);
            data.DoubleDamageIfCursesLessThan = text.DoubleDamageIfCursesLessThan;
            data.EffectTick = text.EffectTick;
            data.EffectTickSides = text.EffectTickSides;
            data.ExplodeAtStacks = text.ExplodeAtStacks;
            data.GainAuraCurseConsumption = Plugin.medsAurasCursesSource.ContainsKey(text.GainAuraCurseConsumption) ? Plugin.medsAurasCursesSource[text.GainAuraCurseConsumption] : (AuraCurseData)null;
            data.GainAuraCurseConsumption2 = Plugin.medsAurasCursesSource.ContainsKey(text.GainAuraCurseConsumption2) ? Plugin.medsAurasCursesSource[text.GainAuraCurseConsumption2] : (AuraCurseData)null;
            data.GainAuraCurseConsumptionPerCharge = text.GainAuraCurseConsumptionPerCharge;
            data.GainAuraCurseConsumptionPerCharge2 = text.GainAuraCurseConsumptionPerCharge2;
            data.GainCharges = text.GainCharges;
            data.GainChargesFromThisAuraCurse = Plugin.medsAurasCursesSource.ContainsKey(text.GainChargesFromThisAuraCurse) ? Plugin.medsAurasCursesSource[text.GainChargesFromThisAuraCurse] : (AuraCurseData)null;
            data.GainChargesFromThisAuraCurse2 = Plugin.medsAurasCursesSource.ContainsKey(text.GainChargesFromThisAuraCurse2) ? Plugin.medsAurasCursesSource[text.GainChargesFromThisAuraCurse2] : (AuraCurseData)null;
            data.HealAttackerConsumeCharges = text.HealAttackerConsumeCharges;
            data.HealAttackerPerStack = text.HealAttackerPerStack;
            data.HealDonePercent = text.HealDonePercent;
            data.HealDonePercentPerStack = text.HealDonePercentPerStack;
            data.HealDonePercentPerStackPerEnergy = text.HealDonePercentPerStackPerEnergy;
            data.HealDonePerStack = text.HealDonePerStack;
            data.HealReceivedTotal = text.HealReceivedTotal;
            data.HealSidesWhenConsumed = text.HealSidesWhenConsumed;
            data.HealSidesWhenConsumedPerCharge = text.HealSidesWhenConsumedPerCharge;
            data.HealWhenConsumed = text.HealWhenConsumed;
            data.HealWhenConsumedPerCharge = text.HealWhenConsumedPerCharge;
            data.IconShow = text.IconShow;
            data.Id = text.ID;
            data.IncreasedDamageReceivedType = (DamageType)ToData<DamageType>(text.IncreasedDamageReceivedType);
            data.IncreasedDamageReceivedType2 = (DamageType)ToData<DamageType>(text.IncreasedDamageReceivedType2);
            data.IncreasedDirectDamageChargesMultiplierNeededForOne = text.IncreasedDirectDamageChargesMultiplierNeededForOne;
            data.IncreasedDirectDamageChargesMultiplierNeededForOne2 = text.IncreasedDirectDamageChargesMultiplierNeededForOne2;
            data.IncreasedDirectDamageReceivedPerStack = text.IncreasedDirectDamageReceivedPerStack;
            data.IncreasedDirectDamageReceivedPerStack2 = text.IncreasedDirectDamageReceivedPerStack2;
            data.IncreasedDirectDamageReceivedPerTurn = text.IncreasedDirectDamageReceivedPerTurn;
            data.IncreasedDirectDamageReceivedPerTurn2 = text.IncreasedDirectDamageReceivedPerTurn2;
            data.IncreasedPercentDamageReceivedPerStack = text.IncreasedPercentDamageReceivedPerStack;
            data.IncreasedPercentDamageReceivedPerStack2 = text.IncreasedPercentDamageReceivedPerStack2;
            data.IncreasedPercentDamageReceivedPerTurn = text.IncreasedPercentDamageReceivedPerTurn;
            data.IncreasedPercentDamageReceivedPerTurn2 = text.IncreasedPercentDamageReceivedPerTurn2;
            data.Invulnerable = text.Invulnerable;
            data.IsAura = text.IsAura;
            data.MaxCharges = text.MaxCharges;
            data.MaxMadnessCharges = text.MaxMadnessCharges;
            data.ModifyCardCostPerChargeNeededForOne = text.ModifyCardCostPerChargeNeededForOne;
            data.NoRemoveBlockAtTurnEnd = text.NoRemoveBlockAtTurnEnd;
            data.Preventable = text.Preventable;
            data.PreventedAuraCurse = Plugin.medsAurasCursesSource.ContainsKey(text.PreventedAuraCurse) ? Plugin.medsAurasCursesSource[text.PreventedAuraCurse] : (AuraCurseData)null;
            data.PreventedAuraCurseStackPerStack = text.PreventedAuraCurseStackPerStack;
            data.PreventedDamagePerStack = text.PreventedDamagePerStack;
            data.PreventedDamageTypePerStack = (DamageType)ToData<DamageType>(text.PreventedDamageTypePerStack);
            data.PriorityOnConsumption = text.PriorityOnConsumption;
            data.ProduceDamageWhenConsumed = text.ProduceDamageWhenConsumed;
            data.ProduceHealWhenConsumed = text.ProduceHealWhenConsumed;
            data.Removable = text.Removable;
            data.RemoveAuraCurse = Plugin.medsAurasCursesSource.ContainsKey(text.RemoveAuraCurse) ? Plugin.medsAurasCursesSource[text.RemoveAuraCurse] : (AuraCurseData)null;
            data.RemoveAuraCurse2 = Plugin.medsAurasCursesSource.ContainsKey(text.RemoveAuraCurse2) ? Plugin.medsAurasCursesSource[text.RemoveAuraCurse2] : (AuraCurseData)null;
            data.ResistModified = (DamageType)ToData<DamageType>(text.ResistModified);
            data.ResistModified2 = (DamageType)ToData<DamageType>(text.ResistModified2);
            data.ResistModified3 = (DamageType)ToData<DamageType>(text.ResistModified3);
            data.ResistModifiedPercentagePerStack = text.ResistModifiedPercentagePerStack;
            data.ResistModifiedPercentagePerStack2 = text.ResistModifiedPercentagePerStack2;
            data.ResistModifiedPercentagePerStack3 = text.ResistModifiedPercentagePerStack3;
            data.ResistModifiedValue = text.ResistModifiedValue;
            data.ResistModifiedValue2 = text.ResistModifiedValue2;
            data.ResistModifiedValue3 = text.ResistModifiedValue3;
            data.RevealCardsPerCharge = text.RevealCardsPerCharge;
            data.SkipsNextTurn = text.SkipsNextTurn;
            if (!Plugin.medsAurasCursesSource.ContainsKey(text.ID)) // #LOADSOUNDS #TODO
                data.Sound = (UnityEngine.AudioClip)null;
            else
                data.Sound = Plugin.medsAurasCursesSource[text.ID].Sound;
            try
            {
                data.Sprite = Plugin.ImportSprite(text.Sprite);
                Plugin.AddTMPFallbackSprite(text.Sprite);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError(ex.Message);
                if (!Plugin.medsAurasCursesSource.ContainsKey(text.ID))
                {
                    data.Sprite = Plugin.medsSprites["medsDefaultAuraCurse"];
                    Plugin.AddTMPFallbackSprite("medsDefaultAuraCurse");
                    Plugin.Log.LogInfo("using default AuraCurse sprite instead!");
                }
                else
                {
                    data.Sprite = Plugin.medsAurasCursesSource[text.ID].Sprite;
                    Plugin.Log.LogInfo("using vanilla sprite " + data.Sprite.name + " instead!");
                }
            }
            data.Stealth = text.Stealth;
            data.Taunt = text.Taunt;
            return data;
        }

        public static CardData ToData(CardDataText text)
        {
            CardData data = ScriptableObject.CreateInstance<CardData>();
            /*if (Plugin.medsCardsSource.ContainsKey(text.ID))
                data = UnityEngine.Object.Instantiate<CardData>(Plugin.medsCardsSource[text.ID]); // we need to learn more about instantiating and when we do it? :( Plugin.medsCardsSource[text.ID]; // 
            else
                data = UnityEngine.Object.Instantiate<CardData>(Plugin.medsCardsSource["defend"]); */
            data.name = text.CardName;
            data.Id = text.ID;
            data.InternalId = text.ID;
            data.AcEnergyBonus = Globals.Instance.GetAuraCurseData(text.AcEnergyBonus);
            data.AcEnergyBonus2 = Globals.Instance.GetAuraCurseData(text.AcEnergyBonus2);
            data.AcEnergyBonusQuantity = text.AcEnergyBonusQuantity;
            data.AcEnergyBonus2Quantity = text.AcEnergyBonus2Quantity;
            data.AddCard = text.AddCard;
            data.AddCardChoose = text.AddCardChoose;
            data.AddCardCostTurn = text.AddCardCostTurn;
            data.AddCardFrom = (CardFrom)ToData<CardFrom>(text.AddCardFrom);
            data.AddCardId = text.AddCardId;
            if (text.AddCardList.Length > 0)
                Plugin.medsSecondRunImport[text.ID] = text.AddCardList;
            data.AddCardPlace = (CardPlace)ToData<CardPlace>(text.AddCardPlace);
            data.AddCardReducedCost = text.AddCardReducedCost;
            data.AddCardType = (CardType)ToData<CardType>(text.AddCardType);
            data.AddCardTypeAux = new CardType[text.AddCardTypeAux.Length];
            for (int a = 0; a < text.AddCardTypeAux.Length; a++)
                data.AddCardTypeAux[a] = (CardType)ToData<CardType>(text.AddCardTypeAux[a]);
            data.AddCardVanish = text.AddCardVanish;
            data.Aura = Globals.Instance.GetAuraCurseData(text.Aura);
            data.Aura2 = Globals.Instance.GetAuraCurseData(text.Aura2);
            data.Aura3 = Globals.Instance.GetAuraCurseData(text.Aura3);
            data.AuraCharges = text.AuraCharges;
            data.AuraChargesSpecialValue1 = text.AuraChargesSpecialValue1;
            data.AuraChargesSpecialValue2 = text.AuraChargesSpecialValue2;
            data.AuraChargesSpecialValueGlobal = text.AuraChargesSpecialValueGlobal;
            data.AuraCharges2 = text.AuraCharges2;
            data.AuraCharges2SpecialValue1 = text.AuraCharges2SpecialValue1;
            data.AuraCharges2SpecialValue2 = text.AuraCharges2SpecialValue2;
            data.AuraCharges2SpecialValueGlobal = text.AuraCharges2SpecialValueGlobal;
            data.AuraCharges3 = text.AuraCharges3;
            data.AuraCharges3SpecialValue1 = text.AuraCharges3SpecialValue1;
            data.AuraCharges3SpecialValue2 = text.AuraCharges3SpecialValue2;
            data.AuraCharges3SpecialValueGlobal = text.AuraCharges3SpecialValueGlobal;
            data.AuraSelf = Globals.Instance.GetAuraCurseData(text.AuraSelf);
            data.AuraSelf2 = Globals.Instance.GetAuraCurseData(text.AuraSelf2);
            data.AuraSelf3 = Globals.Instance.GetAuraCurseData(text.AuraSelf3);
            data.AutoplayDraw = text.AutoplayDraw;
            data.AutoplayEndTurn = text.AutoplayEndTurn;
            data.BaseCard = text.BaseCard;
            data.CardClass = (CardClass)ToData<CardClass>(text.CardClass);
            data.CardName = text.CardName;
            data.CardNumber = text.CardNumber;
            data.CardRarity = (CardRarity)ToData<CardRarity>(text.CardRarity);
            data.CardType = (CardType)ToData<CardType>(text.CardType);
            data.CardTypeAux = new CardType[text.CardTypeAux.Length];
            for (int a = 0; a < text.CardTypeAux.Length; a++)
                data.CardTypeAux[a] = (CardType)ToData<CardType>(text.CardTypeAux[a]);
            data.CardUpgraded = (CardUpgraded)ToData<CardUpgraded>(text.CardUpgraded);
            data.Corrupted = text.Corrupted;
            data.Curse = Globals.Instance.GetAuraCurseData(text.Curse);
            data.Curse2 = Globals.Instance.GetAuraCurseData(text.Curse2);
            data.Curse3 = Globals.Instance.GetAuraCurseData(text.Curse3);
            data.CurseCharges = text.CurseCharges;
            data.CurseChargesSpecialValue1 = text.CurseChargesSpecialValue1;
            data.CurseChargesSpecialValue2 = text.CurseChargesSpecialValue2;
            data.CurseChargesSpecialValueGlobal = text.CurseChargesSpecialValueGlobal;
            data.CurseCharges2 = text.CurseCharges2;
            data.CurseCharges2SpecialValue1 = text.CurseCharges2SpecialValue1;
            data.CurseCharges2SpecialValue2 = text.CurseCharges2SpecialValue2;
            data.CurseCharges2SpecialValueGlobal = text.CurseCharges2SpecialValueGlobal;
            data.CurseCharges3 = text.CurseCharges3;
            data.CurseCharges3SpecialValue1 = text.CurseCharges3SpecialValue1;
            data.CurseCharges3SpecialValue2 = text.CurseCharges3SpecialValue2;
            data.CurseCharges3SpecialValueGlobal = text.CurseCharges3SpecialValueGlobal;
            data.CurseSelf = Globals.Instance.GetAuraCurseData(text.CurseSelf);
            data.CurseSelf2 = Globals.Instance.GetAuraCurseData(text.CurseSelf2);
            data.CurseSelf3 = Globals.Instance.GetAuraCurseData(text.CurseSelf3);
            data.Damage = text.Damage;
            data.DamageSpecialValue1 = text.DamageSpecialValue1;
            data.DamageSpecialValue2 = text.DamageSpecialValue2;
            data.DamageSpecialValueGlobal = text.DamageSpecialValueGlobal;
            data.Damage2 = text.Damage2;
            data.Damage2SpecialValue1 = text.Damage2SpecialValue1;
            data.Damage2SpecialValue2 = text.Damage2SpecialValue2;
            data.Damage2SpecialValueGlobal = text.Damage2SpecialValueGlobal;
            data.DamageEnergyBonus = text.DamageEnergyBonus;
            data.DamageSelf = text.DamageSelf;
            data.DamageSelf2 = text.DamageSelf2;
            data.DamageSides = text.DamageSides;
            data.DamageSides2 = text.DamageSides2;
            data.DamageType = (DamageType)ToData<DamageType>(text.DamageType);
            data.DamageType2 = (DamageType)ToData<DamageType>(text.DamageType2);
            data.Description = ""; // text.Description; // isn't this generated in-game? SetDescriptionNew #TODO: probably remove it?
            // data.DescriptionID = text.descriptionid
            data.DiscardCard = text.DiscardCard;
            data.DiscardCardAutomatic = text.DiscardCardAutomatic;
            data.DiscardCardPlace = (CardPlace)ToData<CardPlace>(text.DiscardCardPlace);
            data.DiscardCardType = (CardType)ToData<CardType>(text.DiscardCardType);
            data.DiscardCardTypeAux = new CardType[text.DiscardCardTypeAux.Length];
            for (int a = 0; a < text.DiscardCardTypeAux.Length; a++)
                data.DiscardCardTypeAux[a] = (CardType)ToData<CardType>(text.DiscardCardTypeAux[a]);
            data.DispelAuras = text.DispelAuras;
            data.DrawCard = text.DrawCard;
            data.EffectCastCenter = text.EffectCastCenter;
            data.EffectCaster = text.EffectCaster;
            data.EffectCasterRepeat = text.EffectCasterRepeat;
            data.EffectPostCastDelay = text.EffectPostCastDelay;
            data.EffectPostTargetDelay = text.EffectPostTargetDelay;
            data.EffectPreAction = text.EffectPreAction;
            data.EffectRepeat = text.EffectRepeat;
            data.EffectRepeatDelay = text.EffectRepeatDelay;
            data.EffectRepeatEnergyBonus = text.EffectRepeatEnergyBonus;
            data.EffectRepeatMaxBonus = text.EffectRepeatMaxBonus;
            data.EffectRepeatModificator = text.EffectRepeatModificator;
            data.EffectRepeatTarget = (EffectRepeatTarget)ToData<EffectRepeatTarget>(text.EffectRepeatTarget);
            data.EffectRequired = text.EffectRequired;
            data.EffectTarget = text.EffectTarget;
            data.EffectTrail = text.EffectTrail;
            data.EffectTrailAngle = (EffectTrailAngle)ToData<EffectTrailAngle>(text.EffectTrailAngle);
            data.EffectTrailRepeat = text.EffectTrailRepeat;
            data.EffectTrailSpeed = text.EffectTrailSpeed;
            data.EndTurn = text.EndTurn;
            data.EnergyCost = text.EnergyCost;
            data.EnergyCostForShow = text.EnergyCostForShow;
            data.EnergyRecharge = text.EnergyRecharge;
            data.EnergyReductionPermanent = text.EnergyReductionPermanent;
            data.EnergyReductionTemporal = text.EnergyReductionTemporal;
            data.EnergyReductionToZeroPermanent = text.EnergyReductionToZeroPermanent;
            data.EnergyReductionToZeroTemporal = text.EnergyReductionToZeroTemporal;
            data.ExhaustCounter = text.ExhaustCounter;
            data.FlipSprite = text.FlipSprite;
            data.Fluff = text.Fluff;
            data.FluffPercent = text.FluffPercent;
            data.GoldGainQuantity = text.GoldGainQuantity;
            data.Heal = text.Heal;
            data.HealAuraCurseName = Globals.Instance.GetAuraCurseData(text.HealAuraCurseName);
            data.HealAuraCurseName2 = Globals.Instance.GetAuraCurseData(text.HealAuraCurseName2);
            data.HealAuraCurseName3 = Globals.Instance.GetAuraCurseData(text.HealAuraCurseName3);
            data.HealAuraCurseName4 = Globals.Instance.GetAuraCurseData(text.HealAuraCurseName4);
            data.HealAuraCurseSelf = Globals.Instance.GetAuraCurseData(text.HealAuraCurseSelf);
            data.HealCurses = text.HealCurses;
            data.HealEnergyBonus = text.HealEnergyBonus;
            data.HealSelf = text.HealSelf;
            data.HealSelfPerDamageDonePercent = text.HealSelfPerDamageDonePercent;
            data.HealSelfSpecialValue1 = text.HealSelfSpecialValue1;
            data.HealSelfSpecialValue2 = text.HealSelfSpecialValue2;
            data.HealSelfSpecialValueGlobal = text.HealSelfSpecialValueGlobal;
            data.HealSides = text.HealSides;
            data.HealSpecialValue1 = text.HealSpecialValue1;
            data.HealSpecialValue2 = text.HealSpecialValue2;
            data.HealSpecialValueGlobal = text.HealSpecialValueGlobal;
            data.IgnoreBlock = text.IgnoreBlock;
            data.IgnoreBlock2 = text.IgnoreBlock2;
            data.IncreaseAuras = text.IncreaseAuras;
            data.IncreaseCurses = text.IncreaseCurses;
            data.Innate = text.Innate;
            data.IsPetAttack = text.IsPetAttack;
            data.IsPetCast = text.IsPetCast;
            data.KillPet = text.KillPet;
            data.Lazy = text.Lazy;
            data.LookCards = text.LookCards;
            data.LookCardsDiscardUpTo = text.LookCardsDiscardUpTo;
            data.LookCardsVanishUpTo = text.LookCardsVanishUpTo;
            data.MaxInDeck = text.MaxInDeck;
            data.ModifiedByTrait = text.ModifiedByTrait;
            data.MoveToCenter = text.MoveToCenter;
            data.OnlyInWeekly = text.OnlyInWeekly;
            data.Playable = text.Playable;
            data.PullTarget = text.PullTarget;
            data.PushTarget = text.PushTarget;
            data.ReduceAuras = text.ReduceAuras;
            data.ReduceCurses = text.ReduceCurses;
            data.RelatedCard = text.RelatedCard;
            data.RelatedCard2 = text.RelatedCard2;
            data.RelatedCard3 = text.RelatedCard3;
            data.SelfHealthLoss = text.SelfHealthLoss;
            data.SelfHealthLossSpecialGlobal = text.SelfHealthLossSpecialGlobal;
            data.SelfHealthLossSpecialValue1 = text.SelfHealthLossSpecialValue1;
            data.SelfHealthLossSpecialValue2 = text.SelfHealthLossSpecialValue2;
            data.ShardsGainQuantity = text.ShardsGainQuantity;
            data.ShowInTome = text.ShowInTome;
            data.Sku = text.Sku;
            data.Sound = ToData(text.Sound);
            data.SoundPreAction = ToData(text.SoundPreAction);
            data.SoundPreActionFemale = ToData(text.SoundPreActionFemale);
            data.SpecialAuraCurseName1 = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseName1);
            data.SpecialAuraCurseName2 = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseName2);
            data.SpecialAuraCurseNameGlobal = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseNameGlobal);
            data.SpecialValue1 = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValue1);
            data.SpecialValue2 = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValue2);
            data.SpecialValueGlobal = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValueGlobal);
            data.SpecialValueModifier1 = text.SpecialValueModifier1;
            data.SpecialValueModifier2 = text.SpecialValueModifier2;
            data.SpecialValueModifierGlobal = text.SpecialValueModifierGlobal;
            try
            {
                data.Sprite = Plugin.ImportSprite(text.Sprite);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError(ex.Message);
                if (!Plugin.medsCardsSource.ContainsKey(text.ID))
                {
                    data.Sprite = Plugin.medsSprites["medsDefaultCard"];
                    Plugin.Log.LogInfo("using default card sprite instead!");
                }
                else
                {
                    data.Sprite = Plugin.medsCardsSource[text.ID].Sprite;
                    Plugin.Log.LogInfo("using vanilla sprite " + data.Sprite.name + " instead!");
                }
            }
            data.Starter = text.Starter;
            data.StealAuras = text.StealAuras;
            data.SummonAura = Globals.Instance.GetAuraCurseData(text.SummonAura);
            data.SummonAura2 = Globals.Instance.GetAuraCurseData(text.SummonAura2);
            data.SummonAura3 = Globals.Instance.GetAuraCurseData(text.SummonAura3);
            data.SummonAuraCharges = text.SummonAuraCharges;
            data.SummonAuraCharges2 = text.SummonAuraCharges2;
            data.SummonAuraCharges3 = text.SummonAuraCharges3;
            data.TargetPosition = (CardTargetPosition)ToData<CardTargetPosition>(text.TargetPosition);
            data.TargetSide = (CardTargetSide)ToData<CardTargetSide>(text.TargetSide);
            data.TargetType = (CardTargetType)ToData<CardTargetType>(text.TargetType);
            data.TransferCurses = text.TransferCurses;
            data.UpgradedFrom = text.UpgradedFrom;
            data.UpgradesTo1 = text.UpgradesTo1;
            data.UpgradesTo2 = text.UpgradesTo2;
            data.UpgradesToRare = (CardData)null;
            if (!String.IsNullOrWhiteSpace(text.UpgradesToRare))
                Plugin.medsSecondRunImport2[text.ID] = text.UpgradesToRare;
            data.Vanish = text.Vanish;
            data.Visible = text.Visible;
            data.Item = (ItemData)null;
            data.ItemEnchantment = (ItemData)null;
            if (!String.IsNullOrWhiteSpace(text.Item))
                Plugin.medsCardsNeedingItems[text.ID] = text.Item;
            if (!String.IsNullOrWhiteSpace(text.ItemEnchantment))
                Plugin.medsCardsNeedingItemEnchants[text.ID] = text.ItemEnchantment;
            return data;
        }

        public static TraitData ToData(TraitDataText text)
        {
            TraitData data = ScriptableObject.CreateInstance<TraitData>();
            data.Id = text.ID;
            data.Activation = (EventActivation)ToData<EventActivation>(text.Activation);
            data.AuracurseBonus1 = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus1);
            data.AuracurseBonus2 = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus2);
            data.AuracurseBonus3 = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus3);
            data.AuracurseBonusValue1 = text.AuraCurseBonusValue1;
            data.AuracurseBonusValue2 = text.AuraCurseBonusValue2;
            data.AuracurseBonusValue3 = text.AuraCurseBonusValue3;
            data.AuracurseImmune1 = text.AuraCurseImmune1;
            data.AuracurseImmune2 = text.AuraCurseImmune2;
            data.AuracurseImmune3 = text.AuraCurseImmune3;
            data.CharacterStatModified = (CharacterStat)ToData<CharacterStat>(text.CharacterStatModified);
            data.CharacterStatModifiedValue = text.CharacterStatModifiedValue;
            data.DamageBonusFlat = (DamageType)ToData<DamageType>(text.DamageBonusFlat);
            data.DamageBonusFlat2 = (DamageType)ToData<DamageType>(text.DamageBonusFlat2);
            data.DamageBonusFlat3 = (DamageType)ToData<DamageType>(text.DamageBonusFlat3);
            data.DamageBonusFlatValue = text.DamageBonusFlatValue;
            data.DamageBonusFlatValue2 = text.DamageBonusFlatValue2;
            data.DamageBonusFlatValue3 = text.DamageBonusFlatValue3;
            data.DamageBonusPercent = (DamageType)ToData<DamageType>(text.DamageBonusPercent);
            data.DamageBonusPercent2 = (DamageType)ToData<DamageType>(text.DamageBonusPercent2);
            data.DamageBonusPercent3 = (DamageType)ToData<DamageType>(text.DamageBonusPercent3);
            data.DamageBonusPercentValue = text.DamageBonusPercentValue;
            data.DamageBonusPercentValue2 = text.DamageBonusPercentValue2;
            data.DamageBonusPercentValue3 = text.DamageBonusPercentValue3;
            data.Description = text.Description;
            data.HealFlatBonus = text.HealFlatBonus;
            data.HealPercentBonus = text.HealPercentBonus;
            data.HealReceivedFlatBonus = text.HealReceivedFlatBonus;
            data.HealReceivedPercentBonus = text.HealReceivedPercentBonus;
            data.ResistModified1 = (DamageType)ToData<DamageType>(text.ResistModified1);
            data.ResistModified2 = (DamageType)ToData<DamageType>(text.ResistModified2);
            data.ResistModified3 = (DamageType)ToData<DamageType>(text.ResistModified3);
            data.ResistModifiedValue1 = text.ResistModifiedValue1;
            data.ResistModifiedValue2 = text.ResistModifiedValue2;
            data.ResistModifiedValue3 = text.ResistModifiedValue3;
            data.TimesPerRound = text.TimesPerRound;
            data.TimesPerTurn = text.TimesPerTurn;
            data.TraitCard = Globals.Instance.GetCardData(text.TraitCard);
            data.TraitCardForAllHeroes = Globals.Instance.GetCardData(text.TraitCardForAllHeroes);
            data.TraitName = text.TraitName;
            return data;
        }
        public static HeroCards ToData(HeroCardsText text)
        {
            HeroCards data = new();
            if (Plugin.medsCardsSource.ContainsKey(text.Card))
            {
                data.UnitsInDeck = text.UnitsInDeck;
                data.Card = Plugin.medsCardsSource[text.Card];
            }
            return data;
        }

        public static SubClassData ToData(SubClassDataText text)
        {
            SubClassData data = ScriptableObject.CreateInstance<SubClassData>();
            data.Id = text.ID;
            if (!Plugin.medsSubClassesSource.ContainsKey(text.ID))
            {
                data.ActionSound = (UnityEngine.AudioClip)null; // #TODO #LOADSOUNDS
                data.HitSound = (UnityEngine.AudioClip)null; // #TODO #LOADSOUNDS
                data.GameObjectAnimated = (GameObject)null; // #TODO #CHARACTERSPRITES
                data.ExpansionCharacter = false;
                data.OrderInList = 0;
            }
            else
            {
                data.ActionSound = Plugin.medsSubClassesSource[text.ID].ActionSound;
                data.HitSound = Plugin.medsSubClassesSource[text.ID].HitSound;
                data.GameObjectAnimated = Plugin.medsSubClassesSource[text.ID].GameObjectAnimated;
                data.ExpansionCharacter = Plugin.medsSubClassesSource[text.ID].ExpansionCharacter;
                data.OrderInList = Plugin.medsSubClassesSource[text.ID].OrderInList;
            }
            data.Blocked = false;
            data.Cards = new HeroCards[text.Cards.Length];
            for (int a = 0; a < text.Cards.Length; a++)
                data.Cards[a] = ToData(JsonUtility.FromJson<HeroCardsText>(text.Cards[a]));
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack0))
                data.ChallengePack0 = Plugin.medsPackDataSource[text.ChallengePack0];
            else
                data.ChallengePack0 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack1))
                data.ChallengePack1 = Plugin.medsPackDataSource[text.ChallengePack1];
            else
                data.ChallengePack1 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack2))
                data.ChallengePack2 = Plugin.medsPackDataSource[text.ChallengePack2];
            else
                data.ChallengePack2 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack3))
                data.ChallengePack3 = Plugin.medsPackDataSource[text.ChallengePack3];
            else
                data.ChallengePack3 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack4))
                data.ChallengePack4 = Plugin.medsPackDataSource[text.ChallengePack4];
            else
                data.ChallengePack4 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack5))
                data.ChallengePack5 = Plugin.medsPackDataSource[text.ChallengePack5];
            else
                data.ChallengePack5 = (PackData)null;
            if (Plugin.medsPackDataSource.ContainsKey(text.ChallengePack6))
                data.ChallengePack6 = Plugin.medsPackDataSource[text.ChallengePack6];
            else
                data.ChallengePack6 = (PackData)null;
            data.CharacterDescription = text.CharacterDescription;
            data.CharacterDescriptionStrength = text.CharacterDescriptionStrength;
            data.CharacterName = text.CharacterName;
            data.Energy = text.Energy;
            data.EnergyTurn = text.EnergyTurn;
            data.Female = text.Female;
            data.FluffOffsetX = text.FluffOffsetX; // #CHARACTERSPRITES
            data.FluffOffsetY = text.FluffOffsetY; // #CHARACTERSPRITES
            data.HeroClass = (HeroClass)ToData<HeroClass>(text.HeroClass);
            data.Hp = text.HP;
            data.Item = (CardData)null;
            if (Plugin.medsCardsSource.ContainsKey(text.Item))
            {
                data.Item = Plugin.medsCardsSource[text.Item];
            }
            data.MainCharacter = true;
            data.MaxHp = text.MaxHP;
            data.ResistSlashing = text.ResistSlashing;
            data.ResistBlunt = text.ResistBlunt;
            data.ResistPiercing = text.ResistPiercing;
            data.ResistFire = text.ResistFire;
            data.ResistCold = text.ResistCold;
            data.ResistLightning = text.ResistLightning;
            data.ResistHoly = text.ResistHoly;
            data.ResistShadow = text.ResistShadow;
            data.ResistMind = text.ResistMind;
            data.Speed = text.Speed;
            data.Sprite = (Sprite)null;
            if (text.Sprite.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : null in combat
                {
                    data.Sprite = Plugin.ImportSprite(text.Sprite);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].Sprite != (UnityEngine.Object)null)
                    {
                        data.Sprite = Plugin.medsSubClassesSource[text.ID].Sprite;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.Sprite.name + " instead!");
                    }
                }
            }
            data.SpriteBorder = (Sprite)null;
            if (text.SpriteBorder.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : malukahsiluetaGrandePro in combat
                {
                    data.SpriteBorder = Plugin.ImportSprite(text.SpriteBorder);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].SpriteBorder != (UnityEngine.Object)null)
                    {
                        data.SpriteBorder = Plugin.medsSubClassesSource[text.ID].SpriteBorder;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.SpriteBorder.name + " instead!");
                    }
                }
            }
            data.SpriteBorderLocked = (Sprite)null;
            if (text.SpriteBorderLocked.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : malukahBorderSmallBN in combat
                {
                    data.SpriteBorderLocked = Plugin.ImportSprite(text.SpriteBorderLocked);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].SpriteBorderLocked != (UnityEngine.Object)null)
                    {
                        data.SpriteBorderLocked = Plugin.medsSubClassesSource[text.ID].SpriteBorderLocked;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.SpriteBorderLocked.name + " instead!");
                    }
                }
            }
            data.SpriteBorderSmall = (Sprite)null;
            if (text.SpriteBorderSmall.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : malukahsiluetaPro in combat
                {
                    data.SpriteBorderSmall = Plugin.ImportSprite(text.SpriteBorderSmall);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].SpriteBorderSmall != (UnityEngine.Object)null)
                    {
                        data.SpriteBorderSmall = Plugin.medsSubClassesSource[text.ID].SpriteBorderSmall;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.SpriteBorderSmall.name + " instead!");
                    }
                }
            }
            Plugin.Log.LogInfo("TESTB");
            data.SpritePortrait = (Sprite)null;
            if (text.SpritePortrait.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : malukahportraitGrandePro in combat
                {
                    data.SpritePortrait = Plugin.ImportSprite(text.SpritePortrait);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].SpritePortrait != (UnityEngine.Object)null)
                    {
                        data.SpritePortrait = Plugin.medsSubClassesSource[text.ID].SpritePortrait;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.SpritePortrait.name + " instead!");
                    }
                }
            }
            data.SpriteSpeed = (Sprite)null;
            if (text.SpriteSpeed.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : malukahportraitPro in combat
                {
                    data.SpriteSpeed = Plugin.ImportSprite(text.SpriteSpeed);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].SpriteSpeed != (UnityEngine.Object)null)
                    {
                        data.SpriteSpeed = Plugin.medsSubClassesSource[text.ID].SpriteSpeed;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.SpriteSpeed.name + " instead!");
                    }
                }
            }
            Plugin.Log.LogInfo("TESTC");
            data.StickerAngry = (Sprite)null;
            if (text.StickerAngry.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : sticker_malukah_angry
                {
                    data.StickerAngry = Plugin.ImportSprite(text.StickerAngry);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].StickerAngry != (UnityEngine.Object)null)
                    {
                        data.StickerAngry = Plugin.medsSubClassesSource[text.ID].StickerAngry;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.StickerAngry.name + " instead!");
                    }
                }
            }
            data.StickerBase = (Sprite)null;
            if (text.StickerBase.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : sticker_malukah_base
                {
                    data.StickerBase = Plugin.ImportSprite(text.StickerBase);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].StickerBase != (UnityEngine.Object)null)
                    {
                        data.StickerBase = Plugin.medsSubClassesSource[text.ID].StickerBase;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.StickerBase.name + " instead!");
                    }
                }
            }
            data.StickerIndiferent = (Sprite)null;
            if (text.StickerIndifferent.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : sticker_malukah_indiferent
                {
                    data.StickerIndiferent = Plugin.ImportSprite(text.StickerIndifferent);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].StickerIndiferent != (UnityEngine.Object)null)
                    {
                        data.StickerIndiferent = Plugin.medsSubClassesSource[text.ID].StickerIndiferent;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.StickerIndiferent.name + " instead!");
                    }
                }
            }
            data.StickerLove = (Sprite)null;
            if (text.StickerLove.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : sticker_malukah_love
                {
                    data.StickerLove = Plugin.ImportSprite(text.StickerLove);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].StickerLove != (UnityEngine.Object)null)
                    {
                        data.StickerLove = Plugin.medsSubClassesSource[text.ID].StickerLove;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.StickerLove.name + " instead!");
                    }
                }
            }
            data.StickerSurprise = (Sprite)null;
            if (text.StickerSurprise.Length > 0)
            {
                try  // #TODO #CHARACTERSPRITES : sticker_malukah_surprise
                {
                    data.StickerSurprise = Plugin.ImportSprite(text.StickerSurprise);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex.Message);
                    if (Plugin.medsSubClassesSource.ContainsKey(text.ID) && (UnityEngine.Object)Plugin.medsSubClassesSource[text.ID].StickerSurprise != (UnityEngine.Object)null)
                    {
                        data.StickerSurprise = Plugin.medsSubClassesSource[text.ID].StickerSurprise;
                        Plugin.Log.LogInfo("using vanilla sprite " + data.StickerSurprise.name + " instead!");
                    }
                }
            }
            Plugin.Log.LogInfo("TESTE");
            data.StickerOffsetX = text.StickerOffsetX;
            data.SubClassName = text.SubclassName;
            data.Trait0 = (TraitData)null;
            data.Trait1A = (TraitData)null;
            data.Trait1B = (TraitData)null;
            data.Trait2A = (TraitData)null;
            data.Trait2B = (TraitData)null;
            data.Trait3A = (TraitData)null;
            data.Trait3B = (TraitData)null;
            data.Trait4A = (TraitData)null;
            data.Trait4B = (TraitData)null;
            data.Trait1ACard = (CardData)null;
            data.Trait1BCard = (CardData)null;
            data.Trait3ACard = (CardData)null;
            data.Trait3BCard = (CardData)null;
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait0))
                data.Trait0 = Plugin.medsTraitsSource[text.Trait0];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait1A))
                data.Trait1A = Plugin.medsTraitsSource[text.Trait1A];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait1B))
                data.Trait1B = Plugin.medsTraitsSource[text.Trait1B];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait2A))
                data.Trait2A = Plugin.medsTraitsSource[text.Trait2A];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait2B))
                data.Trait2B = Plugin.medsTraitsSource[text.Trait2B];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait3A))
                data.Trait3A = Plugin.medsTraitsSource[text.Trait3A];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait3B))
                data.Trait3B = Plugin.medsTraitsSource[text.Trait3B];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait4A))
                data.Trait4A = Plugin.medsTraitsSource[text.Trait4A];
            if (Plugin.medsTraitsSource.ContainsKey(text.Trait4B))
                data.Trait4B = Plugin.medsTraitsSource[text.Trait4B];
            if (Plugin.medsCardsSource.ContainsKey(text.Trait1A))
                data.Trait1ACard = Plugin.medsCardsSource[text.Trait1A];
            if (Plugin.medsCardsSource.ContainsKey(text.Trait1B))
                data.Trait1BCard = Plugin.medsCardsSource[text.Trait1B];
            if (Plugin.medsCardsSource.ContainsKey(text.Trait3A))
                data.Trait3ACard = Plugin.medsCardsSource[text.Trait3A];
            if (Plugin.medsCardsSource.ContainsKey(text.Trait3B))
                data.Trait3BCard = Plugin.medsCardsSource[text.Trait3B];
            return data;
        }

        public static PerkData ToData(PerkDataText text)
        {
            PerkData data = ScriptableObject.CreateInstance<PerkData>();
            data.AdditionalCurrency = text.AdditionalCurrency;
            data.AdditionalShards = text.AdditionalShards;
            data.AuracurseBonus = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus);
            data.AuracurseBonusValue = text.AuraCurseBonusValue;
            data.CardClass = (CardClass)ToData<CardClass>(text.CardClass);
            data.CustomDescription = text.CustomDescription;
            data.DamageFlatBonus = (DamageType)ToData<DamageType>(text.DamageFlatBonus);
            data.DamageFlatBonusValue = text.DamageFlatBonusValue;
            data.EnergyBegin = text.EnergyBegin;
            data.HealQuantity = text.HealQuantity;
            try
            {
                data.Icon = Plugin.ImportSprite(text.Icon);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError(ex.Message);
                if (!Plugin.medsPerksSource.ContainsKey(text.ID))
                {
                    data.Icon = Plugin.medsSprites["medsDefaultAuraCurse"];
                    Plugin.Log.LogInfo("using default AuraCurse sprite instead!");
                }
                else
                {
                    data.Icon = Plugin.medsPerksSource[text.ID].Icon;
                    Plugin.Log.LogInfo("using vanilla sprite " + data.Icon.name + " instead!");
                }
            }
            data.IconTextValue = text.IconTextValue;
            data.Id = text.ID;
            data.Level = text.Level;
            data.MainPerk = text.MainPerk;
            data.MaxHealth = text.MaxHealth;
            data.ObeliskPerk = text.ObeliskPerk;
            data.ResistModified = (DamageType)ToData<DamageType>(text.ResistModified);
            data.ResistModifiedValue = text.ResistModifiedValue;
            data.Row = text.Row;
            data.SpeedQuantity = text.SpeedQuantity;
            return data;
        }

        public static AICards ToData(AICardsText text)
        {
            AICards data = new();
            data.AddCardRound = text.AddCardRound;
            data.AuracurseCastIf = Globals.Instance.GetAuraCurseData(text.AuraCurseCastIf);
            data.Card = Globals.Instance.GetCardData(text.Card);
            data.OnlyCastIf = (OnlyCastIf)ToData<OnlyCastIf>(text.OnlyCastIf);
            data.PercentToCast = text.PercentToCast;
            data.Priority = text.Priority;
            data.TargetCast = (TargetCast)ToData<TargetCast>(text.TargetCast);
            data.UnitsInDeck = text.UnitsInDeck;
            data.ValueCastIf = text.ValueCastIf;
            return data;
        }

        public static NPCData ToData(NPCDataText text)
        {
            NPCData data = ScriptableObject.CreateInstance<NPCData>();
            data.AICards = new AICards[text.AICards.Length];
            for (int a = 0; a < text.AICards.Length; a++)
            {
                data.AICards[a] = ToData(JsonUtility.FromJson<AICardsText>(text.AICards[a]));
            }
            data.AuracurseImmune = new();
            for (int a = 0; a < text.AuraCurseImmune.Length; a++)
            {
                if (!(data.AuracurseImmune.Contains(text.AuraCurseImmune[a])))
                    data.AuracurseImmune.Add(text.AuraCurseImmune[a]);
            }
            data.BigModel = text.BigModel;
            data.CardsInHand = text.CardsInHand;
            data.Description = text.Description;
            data.Difficulty = text.Difficulty;
            data.Energy = text.Energy;
            data.EnergyTurn = text.EnergyTurn;
            data.ExperienceReward = text.ExperienceReward;
            data.Female = text.Female;
            data.FinishCombatOnDead = text.FinishCombatOnDead;
            data.FluffOffsetX = text.FluffOffsetX;
            data.FluffOffsetY = text.FluffOffsetY;
            // #TODO data.GameObjectAnimated = DataTextConvert.ToString(text.GameObjectAnimated); // #TODO #CHARACTERSPRITE #GAMEOBJECTANIMATED
            data.GoldReward = text.GoldReward;
            // #TODO data.HitSound = DataTextConvert.ToString(text.HitSound);
            // do we really have to set hp/id/speed with reflections? ugh.
            // #TODO #NPC
            // data.Hp = text.HP;
            // data.Id = text.ID;
            // data.Speed = text.Speed;
            data.IsBoss = text.IsBoss;
            data.IsNamed = text.IsNamed;
            data.NPCName = text.NPCName;
            data.PosBottom = text.PosBottom;
            data.PreferredPosition = (CardTargetPosition)ToData<CardTargetPosition>(text.PreferredPosition);
            data.ResistBlunt = text.ResistBlunt;
            data.ResistCold = text.ResistCold;
            data.ResistFire = text.ResistFire;
            data.ResistHoly = text.ResistHoly;
            data.ResistLightning = text.ResistLightning;
            data.ResistMind = text.ResistMind;
            data.ResistPiercing = text.ResistPiercing;
            data.ResistShadow = text.ResistShadow;
            data.ResistSlashing = text.ResistSlashing;
            data.ScriptableObjectName = text.ScriptableObjectName;
            /* #TODO data.Sprite = DataTextConvert.ToString(text.Sprite);
            if ((UnityEngine.Object)text.Sprite != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(text.Sprite, "NPC");
            data.SpritePortrait = DataTextConvert.ToString(text.SpritePortrait);
            if ((UnityEngine.Object)text.SpritePortrait != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(text.SpritePortrait, "NPC");
            data.SpriteSpeed = DataTextConvert.ToString(text.SpriteSpeed);
            if ((UnityEngine.Object)text.SpriteSpeed != (UnityEngine.Object)null && Plugin.medsExportSprites.Value)
                Plugin.ExportSprite(text.SpriteSpeed, "NPC");
            data.TierMob = DataTextConvert.ToString(text.TierMob);
            data.TierReward = DataTextConvert.ToString(text.TierReward);
            // probably have to do these separately/after :(
            data.BaseMonster = DataTextConvert.ToString(text.BaseMonster);
            data.HellModeMob = DataTextConvert.ToString(text.HellModeMob);
            data.NgPlusMob = DataTextConvert.ToString(text.NgPlusMob);
            data.UpgradedMob = DataTextConvert.ToString(text.UpgradedMob);*/
            return data;
        }


        public static NodeData ToData(NodeDataText text)
        {
            NodeData data = ScriptableObject.CreateInstance<NodeData>();

            return data;
        }


        public static LootData ToData(LootDataText text)
        {
            LootData data = ScriptableObject.CreateInstance<LootData>();

            return data;
        }


        public static PerkNodeData ToData(PerkNodeDataText text)
        {
            PerkNodeData data = ScriptableObject.CreateInstance<PerkNodeData>();

            return data;
        }

        public static ChallengeData ToData(ChallengeDataText text)
        {
            ChallengeData data = ScriptableObject.CreateInstance<ChallengeData>();

            return data;
        }

        public static ChallengeTrait ToData(ChallengeTraitText text)
        {
            ChallengeTrait data = ScriptableObject.CreateInstance<ChallengeTrait>();

            return data;
        }

        public static CombatData ToData(CombatDataText text)
        {
            CombatData data = ScriptableObject.CreateInstance<CombatData>();

            return data;
        }
        public static EventData ToData(EventDataText text)
        {
            EventData data = ScriptableObject.CreateInstance<EventData>();

            return data;
        }
        public static EventRequirementData ToData(EventRequirementDataText text)
        {
            EventRequirementData data = ScriptableObject.CreateInstance<EventRequirementData>();
            data.AssignToPlayerAtBegin = text.AssignToPlayerAtBegin;
            data.Description = text.Description;
            data.ItemSprite = null; // do we have a ToSprite equiv? #TODO #EVENTREQUIREMENT #TOSPRITE
            data.RequirementId = text.RequirementID;
            data.RequirementName = text.RequirementName;
            data.RequirementTrack = text.RequirementTrack;
            data.TrackSprite = null; // ToSprite #TODO #EVENTREQUIREMENT #TOSPRITE
            return data;
        }
        public static ZoneData ToData(ZoneDataText text)
        {
            ZoneData data = ScriptableObject.CreateInstance<ZoneData>();

            return data;
        }
        public static PackData ToData(PackDataText text)
        {
            PackData data = ScriptableObject.CreateInstance<PackData>();
            if (Plugin.medsCardsSource.ContainsKey(text.Card0))
                data.Card0 = Plugin.medsCardsSource[text.Card0];
            if (Plugin.medsCardsSource.ContainsKey(text.Card1))
                data.Card1 = Plugin.medsCardsSource[text.Card1];
            if (Plugin.medsCardsSource.ContainsKey(text.Card2))
                data.Card2 = Plugin.medsCardsSource[text.Card2];
            if (Plugin.medsCardsSource.ContainsKey(text.Card3))
                data.Card3 = Plugin.medsCardsSource[text.Card3];
            if (Plugin.medsCardsSource.ContainsKey(text.Card4))
                data.Card4 = Plugin.medsCardsSource[text.Card4];
            if (Plugin.medsCardsSource.ContainsKey(text.Card5))
                data.Card5 = Plugin.medsCardsSource[text.Card5];
            if (Plugin.medsCardsSource.ContainsKey(text.CardSpecial0))
                data.CardSpecial0 = Plugin.medsCardsSource[text.CardSpecial0];
            if (Plugin.medsCardsSource.ContainsKey(text.CardSpecial1))
                data.CardSpecial1 = Plugin.medsCardsSource[text.CardSpecial1];
            data.PackClass = (CardClass)ToData<CardClass>(text.PackClass);
            data.PackId = text.PackID;
            data.PackName = text.PackName;
            data.PerkList = new System.Collections.Generic.List<PerkData>();
            foreach (string perkID in text.PerkList)
            {
                if (Plugin.medsPerksSource.ContainsKey(perkID))
                    data.PerkList.Add(Plugin.medsPerksSource[perkID]);
            }
            if (Plugin.medsCardsSource.ContainsKey(text.CardSpecial1))
                data.CardSpecial1 = Plugin.medsCardsSource[text.CardSpecial1];
            if (text.RequiredClass.Length > 0)
                Plugin.medsSecondRunImport2[text.PackID] = text.RequiredClass;
            return data;
        }
        public static CardPlayerPackData ToData(CardPlayerPackDataText text)
        {
            CardPlayerPackData data = ScriptableObject.CreateInstance<CardPlayerPackData>();

            return data;
        }
        public static ItemData ToData(ItemDataText text)
        {
            ItemData data = ScriptableObject.CreateInstance<ItemData>();

            data.Acg1MultiplyByEnergyUsed = text.ACG1MultiplyByEnergyUsed;
            data.Acg2MultiplyByEnergyUsed = text.ACG2MultiplyByEnergyUsed;
            data.Acg3MultiplyByEnergyUsed = text.ACG3MultiplyByEnergyUsed;
            data.Activation = (EventActivation)ToData<EventActivation>(text.Activation);
            data.ActivationOnlyOnHeroes = text.ActivationOnlyOnHeroes;
            data.AuracurseBonus1 = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus1);
            data.AuracurseBonus2 = Globals.Instance.GetAuraCurseData(text.AuraCurseBonus2);
            data.AuracurseBonusValue1 = text.AuraCurseBonusValue1;
            data.AuracurseBonusValue2 = text.AuraCurseBonusValue2;
            data.AuracurseCustomAC = Globals.Instance.GetAuraCurseData(text.AuraCurseCustomAC);
            data.AuracurseCustomModValue1 = text.AuraCurseCustomModValue1;
            data.AuracurseCustomModValue2 = text.AuraCurseCustomModValue2;
            data.AuracurseCustomString = text.AuraCurseCustomString;
            data.AuracurseGain1 = Globals.Instance.GetAuraCurseData(text.AuraCurseGain1);
            data.AuracurseGain2 = Globals.Instance.GetAuraCurseData(text.AuraCurseGain2);
            data.AuracurseGain3 = Globals.Instance.GetAuraCurseData(text.AuraCurseGain3);
            data.AuracurseGainValue1 = text.AuraCurseGainValue1;
            data.AuracurseGainValue2 = text.AuraCurseGainValue2;
            data.AuracurseGainValue3 = text.AuraCurseGainValue3;
            data.AuracurseGainSelf1 = Globals.Instance.GetAuraCurseData(text.AuraCurseGainSelf1);
            data.AuracurseGainSelf2 = Globals.Instance.GetAuraCurseData(text.AuraCurseGainSelf2);
            data.AuracurseGainSelfValue1 = text.AuraCurseGainSelfValue1;
            data.AuracurseGainSelfValue2 = text.AuraCurseGainSelfValue2;
            data.AuracurseImmune1 = Globals.Instance.GetAuraCurseData(text.AuraCurseImmune1);
            data.AuracurseImmune2 = Globals.Instance.GetAuraCurseData(text.AuraCurseImmune2);
            data.AuraCurseNumForOneEvent = text.AuraCurseNumForOneEvent;
            data.AuraCurseSetted = Globals.Instance.GetAuraCurseData(text.AuraCurseSetted);
            data.CardNum = text.CardNum;
            data.CardPlace = (CardPlace)ToData<CardPlace>(text.CardPlace);
            data.CardsReduced = text.CardsReduced;
            if (Plugin.medsCardsSource.ContainsKey(text.CardToGain))
                data.CardToGain = Plugin.medsCardsSource[text.CardToGain];
            else
                data.CardToGain = (CardData)null;
            data.CardToGainList = new();
            for (int a = 0; a < text.CardToGainList.Length; a++)
            {
                if (Plugin.medsCardsSource.ContainsKey(text.CardToGainList[a]))
                {
                    CardData medsCardData = Plugin.medsCardsSource[text.CardToGainList[a]];
                    if (!(data.CardToGainList.Contains(medsCardData)))
                        data.CardToGainList.Add(medsCardData);
                }
            }
            data.CardToGainType = (CardType)ToData<CardType>(text.CardToGainType);
            data.CardToReduceType = (CardType)ToData<CardType>(text.CardToReduceType);
            data.CastedCardType = (CardType)ToData<CardType>(text.CastedCardType);
            data.CastEnchantmentOnFinishSelfCast = text.CastEnchantmentOnFinishSelfCast;
            data.ChanceToDispel = text.ChanceToDispel;
            data.ChanceToDispelNum = text.ChanceToDispelNum;
            data.CharacterStatModified = (CharacterStat)ToData<CharacterStat>(text.CharacterStatModified);
            data.CharacterStatModified2 = (CharacterStat)ToData<CharacterStat>(text.CharacterStatModified2);
            data.CharacterStatModified3 = (CharacterStat)ToData<CharacterStat>(text.CharacterStatModified3);
            data.CharacterStatModifiedValue = text.CharacterStatModifiedValue;
            data.CharacterStatModifiedValue2 = text.CharacterStatModifiedValue2;
            data.CharacterStatModifiedValue3 = text.CharacterStatModifiedValue3;
            data.CostReducePermanent = text.CostReducePermanent;
            data.CostReduceReduction = text.CostReduceReduction;
            data.CostReduction = text.CostReduction;
            data.CostZero = text.CostZero;
            data.CursedItem = text.CursedItem;
            data.DamageFlatBonus = (DamageType)ToData<DamageType>(text.DamageFlatBonus);
            data.DamageFlatBonus2 = (DamageType)ToData<DamageType>(text.DamageFlatBonus2);
            data.DamageFlatBonus3 = (DamageType)ToData<DamageType>(text.DamageFlatBonus3);
            data.DamageFlatBonusValue = text.DamageFlatBonusValue;
            data.DamageFlatBonusValue2 = text.DamageFlatBonusValue2;
            data.DamageFlatBonusValue3 = text.DamageFlatBonusValue3;
            data.DamagePercentBonus = (DamageType)ToData<DamageType>(text.DamagePercentBonus);
            data.DamagePercentBonus2 = (DamageType)ToData<DamageType>(text.DamagePercentBonus2);
            data.DamagePercentBonus3 = (DamageType)ToData<DamageType>(text.DamagePercentBonus3);
            data.DamagePercentBonusValue = text.DamagePercentBonusValue;
            data.DamagePercentBonusValue2 = text.DamagePercentBonusValue2;
            data.DamagePercentBonusValue3 = text.DamagePercentBonusValue3;
            data.DamageToTarget = text.DamageToTarget;
            data.DamageToTargetType = (DamageType)ToData<DamageType>(text.DamageToTargetType);
            data.DestroyAfterUse = text.DestroyAfterUse;
            data.DestroyAfterUses = text.DestroyAfterUses;
            data.DestroyEndOfTurn = text.DestroyEndOfTurn;
            data.DestroyStartOfTurn = text.DestroyStartOfTurn;
            data.DrawCards = text.DrawCards;
            data.DrawMultiplyByEnergyUsed = text.DrawMultiplyByEnergyUsed;
            data.DropOnly = text.DropOnly;
            data.DttMultiplyByEnergyUsed = text.DTTMultiplyByEnergyUsed;
            data.DuplicateActive = text.DuplicateActive;
            data.EffectCaster = text.EffectCaster;
            data.EffectItemOwner = text.EffectItemOwner;
            data.EffectTarget = text.EffectTarget;
            data.EmptyHand = text.EmptyHand;
            data.EnergyQuantity = text.EnergyQuantity;
            data.ExactRound = text.ExactRound;
            data.HealFlatBonus = text.HealFlatBonus;
            data.HealPercentBonus = text.HealPercentBonus;
            data.HealPercentQuantity = text.HealPercentQuantity;
            data.HealQuantity = text.HealQuantity;
            data.HealReceivedFlatBonus = text.HealReceivedFlatBonus;
            data.HealReceivedPercentBonus = text.HealReceivedPercentBonus;
            data.Id = text.ID;
            data.IsEnchantment = text.IsEnchantment;
            data.ItemSound = ToData(text.ItemSound);
            data.ItemTarget = (ItemTarget)ToData<ItemTarget>(text.ItemTarget);
            data.LowerOrEqualPercentHP = text.LowerOrEqualPercentHP;
            data.MaxHealth = text.MaxHealth;
            data.ModifiedDamageType = (DamageType)ToData<DamageType>(text.ModifiedDamageType);
            data.NotShowCharacterBonus = text.NotShowCharacterBonus;
            data.OnlyAddItemToNPCs = text.OnlyAddItemToNPCs;
            data.PassSingleAndCharacterRolls = text.PassSingleAndCharacterRolls;
            data.PercentDiscountShop = text.PercentDiscountShop;
            data.PercentRetentionEndGame = text.PercentRetentionEndGame;
            data.Permanent = text.Permanent;
            data.QuestItem = text.QuestItem;
            data.ReduceHighestCost = text.ReduceHighestCost;
            data.ResistModified1 = (DamageType)ToData<DamageType>(text.ResistModified1);
            data.ResistModified2 = (DamageType)ToData<DamageType>(text.ResistModified2);
            data.ResistModified3 = (DamageType)ToData<DamageType>(text.ResistModified3);
            data.ResistModifiedValue1 = text.ResistModifiedValue1;
            data.ResistModifiedValue2 = text.ResistModifiedValue2;
            data.ResistModifiedValue3 = text.ResistModifiedValue3;
            data.RoundCycle = text.RoundCycle;
            data.SpriteBossDrop = (Sprite)null; // #TODO: SpriteBossDrop
            data.TimesPerCombat = text.TimesPerCombat;
            data.TimesPerTurn = text.TimesPerTurn;
            data.UsedEnergy = text.UsedEnergy;
            data.UseTheNextInsteadWhenYouPlay = text.UseTheNextInsteadWhenYouPlay;
            data.Vanish = text.Vanish;
            return data;
        }
        public static CardbackData ToData(CardbackDataText text)
        {
            CardbackData data = ScriptableObject.CreateInstance<CardbackData>();

            return data;
        }
        public static SkinData ToData(SkinDataText text)
        {
            SkinData data = ScriptableObject.CreateInstance<SkinData>();

            return data;
        }
        public static CinematicData ToData(CinematicDataText text)
        {
            CinematicData data = ScriptableObject.CreateInstance<CinematicData>();

            return data;
        }
        public static CorruptionPackData ToData(CorruptionPackDataText text)
        {
            CorruptionPackData data = ScriptableObject.CreateInstance<CorruptionPackData>();

            return data;
        }
        public static KeyNotesData ToData(KeyNotesDataText text)
        {
            KeyNotesData data = ScriptableObject.CreateInstance<KeyNotesData>();
            data.Id = text.ID;
            data.KeynoteName = text.KeynoteName;
            data.DescriptionExtended = text.DescriptionExtended;
            data.Description = text.Description;
            return data;
        }

        public static UnityEngine.AudioClip ToData(string audioClipName)
        {
            return Plugin.medsAudioClips.ContainsKey(audioClipName) ? Plugin.medsAudioClips[audioClipName] : (UnityEngine.AudioClip)null;
        }
        /*
         *                                                                                   
         *    888888888888  ,ad8888ba,          88           88  888b      88  88      a8P   
         *         88      d8"'    `"8b         88           88  8888b     88  88    ,88'    
         *         88     d8'        `8b        88           88  88 `8b    88  88  ,88"      
         *         88     88          88        88           88  88  `8b   88  88,d88'       
         *         88     88          88        88           88  88   `8b  88  8888"88,      
         *         88     Y8,        ,8P        88           88  88    `8b 88  88P   Y8b     
         *         88      Y8a.    .a8P         88           88  88     `8888  88     "88,   
         *         88       `"Y8888Y"'          88888888888  88  88      `888  88       Y8b  
         *
         *   Utilities for linking to AtO objects?
         */
    }
}

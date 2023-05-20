using System;
using static Enums;
using UnityEngine;

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
            {
                if (Plugin.medsExportSprites.Value)
                    Plugin.ExportSprite(sprite);
                return sprite.name;
            }
            return "";
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
            return JsonUtility.ToJson(ToText(data));
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
            text.ItemEnchantment = DataTextConvert.ToString(data.ItemEnchantment);
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
            text.Female = data.Female;
            text.GameObjectAnimated = DataTextConvert.ToString(data.GameObjectAnimated);
            text.HeroClass = DataTextConvert.ToString(data.HeroClass);
            text.HitSound = DataTextConvert.ToString(data.HitSound);
            text.HP = data.Hp;
            text.MaxHP = data.MaxHp;
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
            text.SpriteBorder = DataTextConvert.ToString(data.SpriteBorder);
            text.SpriteBorderLocked = DataTextConvert.ToString(data.SpriteBorderLocked);
            text.SpriteBorderSmall = DataTextConvert.ToString(data.SpriteBorderSmall);
            text.SpritePortrait = DataTextConvert.ToString(data.SpritePortrait);
            text.SpriteSpeed = DataTextConvert.ToString(data.SpriteSpeed);
            text.StickerAngry = DataTextConvert.ToString(data.StickerAngry);
            text.StickerBase = DataTextConvert.ToString(data.StickerBase);
            text.StickerIndifferent = DataTextConvert.ToString(data.StickerIndiferent);
            text.StickerLove = DataTextConvert.ToString(data.StickerLove);
            text.StickerSurprise = DataTextConvert.ToString(data.StickerSurprise);
            text.StickerOffsetX = data.StickerOffsetX;
            text.SubclassName = data.SubClassName;
            text.Trait0 = DataTextConvert.ToString(data.Trait0);
            text.Trait1A = DataTextConvert.ToString(data.Trait0);
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
            text.SpritePortrait = DataTextConvert.ToString(data.SpritePortrait);
            text.SpriteSpeed = DataTextConvert.ToString(data.SpriteSpeed);
            text.TierMob = DataTextConvert.ToString(data.TierMob);
            text.TierReward = DataTextConvert.ToString(data.TierReward);
            text.UpgradedMob = DataTextConvert.ToString(data.UpgradedMob);
            return text;
        }
        public static PerkDataText ToText(PerkData data)
        {
            var text = new PerkDataText();
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
            text.EventSpriteDecor = ToString(data.EventSpriteDecor);
            text.EventSpriteMap = ToString(data.EventSpriteMap);
            text.EventTier = ToString(data.EventTier);
            text.EventUniqueID = data.EventUniqueId;
            text.HistoryMode = data.HistoryMode;
            text.ReplyRandom = data.ReplyRandom;
            text.Replies = new string[0]; // = ToString(data.Replys); //eventreplydata[] #TODO (no unique id, so need to pull in entirety! so big :()
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
            text.RequirementID = data.RequirementId;
            text.RequirementName = data.RequirementName;
            text.RequirementTrack = data.RequirementTrack;
            text.TrackSprite = ToString(data.TrackSprite);
            return text;
        }
        public static EventReplyDataText ToText(EventReplyData data)
        {
            EventReplyDataText text = new();
            // #TODO
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
        /* maybe later?
        public static ItemDataText ToText(ItemData data)
        {
            ItemDataText text = new();

            return text;
        }
        */

        /* generic
        public static Text ToText( data)
        {
            Text text = new();

            return text;
        }
        */

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

        public static CardData ToData(CardDataText text)
        {
            CardData data;
            if (Plugin.medsCardsSource.ContainsKey(text.ID))
                data = UnityEngine.Object.Instantiate<CardData>(Plugin.medsCardsSource[text.ID]); // we need to learn more about instantiating and when we do it? :( Plugin.medsCardsSource[text.ID]; // 
            else
                data = UnityEngine.Object.Instantiate<CardData>(Plugin.medsCardsSource["defend"]);
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
            data.AddCardPlace = (CardPlace)ToData<CardPlace>(text.AddCardPlace);
            data.AddCardReducedCost = text.AddCardReducedCost;
            data.AddCardType = (CardType)ToData<CardType>(text.AddCardType);
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
            for (int a = 0; a < text.CardTypeAux.Length; a++)
                data.CardTypeAux[a] = (CardType)ToData<CardType>(text.CardTypeAux[a]);
            // CardTypeList isn't needed, because it's automatically generated ingame?
            //if (text.CardTypeList.Length > 0)
            //{
            //data.CardTypeAux = new string[text.CardTypeAux.Length];
            //for (int a = 0; a < text.CardTypeAux.Length; a++)
            //data.CardTypeAux[a] = Convert.ToString(text.CardTypeAux[a]);
            //}
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
            data.Description = text.Description;
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
            data.PetFront = text.PetFront;
            data.PetInvert = text.PetInvert;
            // data.PetModel = ""; // no clue, not worth it?
            // data.PetOffset = ""; // no clue, not worth it?
            // data.PetSize = ""; // no clue, not worth it?
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
            data.SpecialAuraCurseName1 = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseName1);
            data.SpecialAuraCurseName2 = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseName2);
            data.SpecialAuraCurseNameGlobal = Globals.Instance.GetAuraCurseData(text.SpecialAuraCurseNameGlobal);
            data.SpecialValue1 = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValue1);
            data.SpecialValue2 = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValue2);
            data.SpecialValueGlobal = (CardSpecialValue)ToData<CardSpecialValue>(text.SpecialValueGlobal);
            data.SpecialValueModifier1 = text.SpecialValueModifier1;
            data.SpecialValueModifier2 = text.SpecialValueModifier2;
            data.SpecialValueModifierGlobal = text.SpecialValueModifierGlobal;
            data.Starter = text.Starter;
            data.StealAuras = text.StealAuras;
            data.SummonAura = Globals.Instance.GetAuraCurseData(text.SummonAura);
            data.SummonAura2 = Globals.Instance.GetAuraCurseData(text.SummonAura2);
            data.SummonAura3 = Globals.Instance.GetAuraCurseData(text.SummonAura3);
            data.SummonAuraCharges = text.SummonAuraCharges;
            data.SummonAuraCharges2 = text.SummonAuraCharges2;
            data.SummonAuraCharges3 = text.SummonAuraCharges3;
            // data.SummonUnit = ((UnityEngine.Object)text.SummonUnit != (UnityEngine.Object)null) ? text.SummonUnit.Id : ""; // maybe later :)
            // data.SummonUnitNum = text.SummonUnitNum; // maybe later :)
            data.TargetPosition = (CardTargetPosition)ToData<CardTargetPosition>(text.TargetPosition);
            data.TargetSide = (CardTargetSide)ToData<CardTargetSide>(text.TargetSide);
            data.TargetType = (CardTargetType)ToData<CardTargetType>(text.TargetType);
            data.TransferCurses = text.TransferCurses;
            data.UpgradedFrom = text.UpgradedFrom;
            data.UpgradesTo1 = text.UpgradesTo1;
            data.UpgradesTo2 = text.UpgradesTo2;
            data.Vanish = text.Vanish;
            data.Visible = text.Visible;
            return data;
        }


        public static TraitData ToData(TraitDataText text)
        {
            TraitData data = new();
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

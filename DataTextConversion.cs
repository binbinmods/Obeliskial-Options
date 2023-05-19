using System;
using static Enums;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Obeliskial_Options
{
    public class Data2Text
    {
        public static CardDataText CardData(CardData c)
        {
            // c: incoming card
            // t: exporting text
            CardDataText t = new();
            t.ID = c.Id;
            t.AcEnergyBonus = Convert.ToString(c.AcEnergyBonus);
            t.AcEnergyBonus2 = Convert.ToString(c.AcEnergyBonus2);
            t.AcEnergyBonusQuantity = c.AcEnergyBonusQuantity;
            t.AcEnergyBonus2Quantity = c.AcEnergyBonus2Quantity;
            t.AddCard = c.AddCard;
            t.AddCardChoose = c.AddCardChoose;
            t.AddCardCostTurn = c.AddCardCostTurn;
            t.AddCardFrom = Convert.ToString(c.AddCardFrom);
            t.AddCardId = c.AddCardId;
            t.AddCardList = Convert.ToString(c.AddCardList);
            t.AddCardPlace = Convert.ToString(c.AddCardPlace);
            t.AddCardReducedCost = c.AddCardReducedCost;
            t.AddCardType = Convert.ToString(c.AddCardType);
            t.AddCardTypeAux = Convert.ToString(c.AddCardTypeAux);
            t.AddCardVanish = c.AddCardVanish;
            t.Aura = Convert.ToString(c.Aura);
            t.Aura2 = Convert.ToString(c.Aura2);
            t.Aura3 = Convert.ToString(c.Aura3);
            t.AuraCharges = c.AuraCharges;
            t.AuraChargesSpecialValue1 = c.AuraChargesSpecialValue1;
            t.AuraChargesSpecialValue2 = c.AuraChargesSpecialValue2;
            t.AuraChargesSpecialValueGlobal = c.AuraChargesSpecialValueGlobal;
            t.AuraCharges2 = c.AuraCharges2;
            t.AuraCharges2SpecialValue1 = c.AuraCharges2SpecialValue1;
            t.AuraCharges2SpecialValue2 = c.AuraCharges2SpecialValue2;
            t.AuraCharges2SpecialValueGlobal = c.AuraCharges2SpecialValueGlobal;
            t.AuraCharges3 = c.AuraCharges3;
            t.AuraCharges3SpecialValue1 = c.AuraCharges3SpecialValue1;
            t.AuraCharges3SpecialValue2 = c.AuraCharges3SpecialValue2;
            t.AuraCharges3SpecialValueGlobal = c.AuraCharges3SpecialValueGlobal;
            t.AuraSelf = Convert.ToString(c.AuraSelf);
            t.AuraSelf2 = Convert.ToString(c.AuraSelf2);
            t.AuraSelf3 = Convert.ToString(c.AuraSelf3);
            t.AutoplayDraw = c.AutoplayDraw;
            t.AutoplayEndTurn = c.AutoplayEndTurn;
            t.BaseCard = c.BaseCard;
            t.CardClass = Convert.ToString(c.CardClass);
            t.CardName = c.CardName;
            t.CardNumber = c.CardNumber;
            t.CardRarity = Convert.ToString(c.CardRarity);
            t.CardType = Convert.ToString(c.CardType);
            t.CardTypeAux = Convert.ToString(c.CardTypeAux);
            /* CardTypeList
            if (c.CardTypeList.Length > 0)
            {
                t.CardTypeAux = new string[c.CardTypeAux.Length];
                for (int a = 0; a < c.CardTypeAux.Length; a++)
                    t.CardTypeAux[a] = Convert.ToString(c.CardTypeAux[a]);
            }
            */
            t.CardUpgraded = Convert.ToString(c.CardUpgraded);
            t.Corrupted = c.Corrupted;
            t.Curse = Convert.ToString(c.Curse);
            t.Curse2 = Convert.ToString(c.Curse2);
            t.Curse3 = Convert.ToString(c.Curse3);
            t.CurseCharges = c.CurseCharges;
            t.CurseChargesSpecialValue1 = c.CurseChargesSpecialValue1;
            t.CurseChargesSpecialValue2 = c.CurseChargesSpecialValue2;
            t.CurseChargesSpecialValueGlobal = c.CurseChargesSpecialValueGlobal;
            t.CurseCharges2 = c.CurseCharges2;
            t.CurseCharges2SpecialValue1 = c.CurseCharges2SpecialValue1;
            t.CurseCharges2SpecialValue2 = c.CurseCharges2SpecialValue2;
            t.CurseCharges2SpecialValueGlobal = c.CurseCharges2SpecialValueGlobal;
            t.CurseCharges3 = c.CurseCharges3;
            t.CurseCharges3SpecialValue1 = c.CurseCharges3SpecialValue1;
            t.CurseCharges3SpecialValue2 = c.CurseCharges3SpecialValue2;
            t.CurseCharges3SpecialValueGlobal = c.CurseCharges3SpecialValueGlobal;
            t.CurseSelf = Convert.ToString(c.CurseSelf);
            t.CurseSelf2 = Convert.ToString(c.CurseSelf2);
            t.CurseSelf3 = Convert.ToString(c.CurseSelf3);
            t.Damage = c.Damage;
            t.DamageSpecialValue1 = c.DamageSpecialValue1;
            t.DamageSpecialValue2 = c.DamageSpecialValue2;
            t.DamageSpecialValueGlobal = c.DamageSpecialValueGlobal;
            t.Damage2 = c.Damage2;
            t.Damage2SpecialValue1 = c.Damage2SpecialValue1;
            t.Damage2SpecialValue2 = c.Damage2SpecialValue2;
            t.Damage2SpecialValueGlobal = c.Damage2SpecialValueGlobal;
            t.DamageEnergyBonus = c.DamageEnergyBonus;
            t.DamageSelf = c.DamageSelf;
            t.DamageSelf2 = c.DamageSelf2;
            t.DamageSides = c.DamageSides;
            t.DamageSides2 = c.DamageSides2;
            t.DamageType = Convert.ToString(c.DamageType);
            t.DamageType2 = Convert.ToString(c.DamageType2);
            t.Description = c.Description;
            // t.DescriptionID = c.descriptionid
            t.DiscardCard = c.DiscardCard;
            t.DiscardCardAutomatic = c.DiscardCardAutomatic;
            t.DiscardCardPlace = Convert.ToString(c.DiscardCardPlace);
            t.DiscardCardType = Convert.ToString(c.DiscardCardType);
            t.DiscardCardTypeAux = Convert.ToString(c.DiscardCardTypeAux);
            t.DispelAuras = c.DispelAuras;
            t.DrawCard = c.DrawCard;
            t.EffectCastCenter = c.EffectCastCenter;
            t.EffectCaster = c.EffectCaster;
            t.EffectCasterRepeat = c.EffectCasterRepeat;
            t.EffectPostCastDelay = c.EffectPostCastDelay;
            t.EffectPostTargetDelay = c.EffectPostTargetDelay;
            t.EffectPreAction = c.EffectPreAction;
            t.EffectRepeat = c.EffectRepeat;
            t.EffectRepeatDelay = c.EffectRepeatDelay;
            t.EffectRepeatEnergyBonus = c.EffectRepeatEnergyBonus;
            t.EffectRepeatMaxBonus = c.EffectRepeatMaxBonus;
            t.EffectRepeatModificator = c.EffectRepeatModificator;
            t.EffectRepeatTarget = Convert.ToString(c.EffectRepeatTarget);
            t.EffectRequired = c.EffectRequired;
            t.EffectTarget = c.EffectTarget;
            t.EffectTrail = c.EffectTrail;
            t.EffectTrailAngle = Convert.ToString(c.EffectTrailAngle);
            t.EffectTrailRepeat = c.EffectTrailRepeat;
            t.EffectTrailSpeed = c.EffectTrailSpeed;
            t.EndTurn = c.EndTurn;
            t.EnergyCost = c.EnergyCost;
            t.EnergyCostForShow = c.EnergyCostForShow;
            t.EnergyRecharge = c.EnergyRecharge;
            t.EnergyReductionPermanent = c.EnergyReductionPermanent;
            t.EnergyReductionTemporal = c.EnergyReductionTemporal;
            t.EnergyReductionToZeroPermanent = c.EnergyReductionToZeroPermanent;
            t.EnergyReductionToZeroTemporal = c.EnergyReductionToZeroTemporal;
            t.ExhaustCounter = c.ExhaustCounter;
            t.FlipSprite = c.FlipSprite;
            t.Fluff = c.Fluff;
            t.FluffPercent = c.FluffPercent;
            t.GoldGainQuantity = c.GoldGainQuantity;
            t.Heal = c.Heal;
            t.HealAuraCurseName = Convert.ToString(c.HealAuraCurseName);
            t.HealAuraCurseName2 = Convert.ToString(c.HealAuraCurseName2);
            t.HealAuraCurseName3 = Convert.ToString(c.HealAuraCurseName3);
            t.HealAuraCurseName4 = Convert.ToString(c.HealAuraCurseName4);
            t.HealAuraCurseSelf = Convert.ToString(c.HealAuraCurseSelf);
            t.HealCurses = c.HealCurses;
            t.HealEnergyBonus = c.HealEnergyBonus;
            t.HealSelf = c.HealSelf;
            t.HealSelfPerDamageDonePercent = c.HealSelfPerDamageDonePercent;
            t.HealSelfSpecialValue1 = c.HealSelfSpecialValue1;
            t.HealSelfSpecialValue2 = c.HealSelfSpecialValue2;
            t.HealSelfSpecialValueGlobal = c.HealSelfSpecialValueGlobal;
            t.HealSides = c.HealSides;
            t.HealSpecialValue1 = c.HealSpecialValue1;
            t.HealSpecialValue2 = c.HealSpecialValue2;
            t.HealSpecialValueGlobal = c.HealSpecialValueGlobal;
            t.IgnoreBlock = c.IgnoreBlock;
            t.IgnoreBlock2 = c.IgnoreBlock2;
            t.IncreaseAuras = c.IncreaseAuras;
            t.IncreaseCurses = c.IncreaseCurses;
            t.Innate = c.Innate;
            t.IsPetAttack = c.IsPetAttack;
            t.IsPetCast = c.IsPetCast;
            t.Item = Convert.ToString(c.Item);
            t.ItemEnchantment = Convert.ToString(c.ItemEnchantment);
            t.KillPet = c.KillPet;
            t.Lazy = c.Lazy;
            t.LookCards = c.LookCards;
            t.LookCardsDiscardUpTo = c.LookCardsDiscardUpTo;
            t.LookCardsVanishUpTo = c.LookCardsVanishUpTo;
            t.MaxInDeck = c.MaxInDeck;
            t.ModifiedByTrait = c.ModifiedByTrait;
            t.MoveToCenter = c.MoveToCenter;
            t.OnlyInWeekly = c.OnlyInWeekly;
            t.PetFront = c.PetFront;
            t.PetInvert = c.PetInvert;
            t.PetModel = ""; // no clue, not worth it?
            t.PetOffset = ""; // no clue, not worth it?
            t.PetSize = ""; // no clue, not worth it?
            t.Playable = c.Playable;
            t.PullTarget = c.PullTarget;
            t.PushTarget = c.PushTarget;
            t.ReduceAuras = c.ReduceAuras;
            t.ReduceCurses = c.ReduceCurses;
            t.RelatedCard = c.RelatedCard;
            t.RelatedCard2 = c.RelatedCard2;
            t.RelatedCard3 = c.RelatedCard3;
            t.SelfHealthLoss = c.SelfHealthLoss;
            t.SelfHealthLossSpecialGlobal = c.SelfHealthLossSpecialGlobal;
            t.SelfHealthLossSpecialValue1 = c.SelfHealthLossSpecialValue1;
            t.SelfHealthLossSpecialValue2 = c.SelfHealthLossSpecialValue2;
            t.ShardsGainQuantity = c.ShardsGainQuantity;
            t.ShowInTome = c.ShowInTome;
            t.Sku = c.Sku;
            t.Sound = Convert.ToString(c.Sound);
            t.SoundPreAction = Convert.ToString(c.SoundPreAction);
            t.SoundPreActionFemale = Convert.ToString(c.SoundPreActionFemale);
            t.SpecialAuraCurseName1 = Convert.ToString(c.SpecialAuraCurseName1);
            t.SpecialAuraCurseName2 = Convert.ToString(c.SpecialAuraCurseName2);
            t.SpecialAuraCurseNameGlobal = Convert.ToString(c.SpecialAuraCurseNameGlobal);
            t.SpecialValue1 = Convert.ToString(c.SpecialValue1);
            t.SpecialValue2 = Convert.ToString(c.SpecialValue2);
            t.SpecialValueGlobal = Convert.ToString(c.SpecialValueGlobal);
            t.SpecialValueModifier1 = c.SpecialValueModifier1;
            t.SpecialValueModifier2 = c.SpecialValueModifier2;
            t.SpecialValueModifierGlobal = c.SpecialValueModifierGlobal;
            t.Sprite = Convert.ToString(c.Sprite);
            t.Starter = c.Starter;
            t.StealAuras = c.StealAuras;
            t.SummonAura = Convert.ToString(c.SummonAura);
            t.SummonAura2 = Convert.ToString(c.SummonAura2);
            t.SummonAura3 = Convert.ToString(c.SummonAura3);
            t.SummonAuraCharges = c.SummonAuraCharges;
            t.SummonAuraCharges2 = c.SummonAuraCharges2;
            t.SummonAuraCharges3 = c.SummonAuraCharges3;
            t.SummonUnit = Convert.ToString(c.SummonUnit);
            t.SummonUnitNum = c.SummonUnitNum;
            t.TargetPosition = Convert.ToString(c.TargetPosition);
            t.TargetSide = Convert.ToString(c.TargetSide);
            t.TargetType = Convert.ToString(c.TargetType);
            t.TransferCurses = c.TransferCurses;
            t.UpgradedFrom = c.UpgradedFrom;
            t.UpgradesTo1 = c.UpgradesTo1;
            t.UpgradesTo2 = c.UpgradesTo2;
            t.UpgradesToRare = Convert.ToString(c.UpgradesToRare);
            t.Vanish = c.Vanish;
            t.Visible = c.Visible;
            return t;
        }

        public static TraitDataText TraitData(TraitData c)
        {
            TraitDataText t = new();
            t.ID = c.Id;
            t.Activation = Convert.ToString(c.Activation);
            t.AuraCurseBonus1 = Convert.ToString(c.AuracurseBonus1);
            t.AuraCurseBonus2 = Convert.ToString(c.AuracurseBonus2);
            t.AuraCurseBonus3 = Convert.ToString(c.AuracurseBonus3);
            t.AuraCurseBonusValue1 = c.AuracurseBonusValue1;
            t.AuraCurseBonusValue2 = c.AuracurseBonusValue2;
            t.AuraCurseBonusValue3 = c.AuracurseBonusValue3;
            t.AuraCurseImmune1 = c.AuracurseImmune1;
            t.AuraCurseImmune2 = c.AuracurseImmune2;
            t.AuraCurseImmune3 = c.AuracurseImmune3;
            t.CharacterStatModified = Convert.ToString(c.CharacterStatModified);
            t.CharacterStatModifiedValue = c.CharacterStatModifiedValue;
            t.DamageBonusFlat = Convert.ToString(c.DamageBonusFlat);
            t.DamageBonusFlat2 = Convert.ToString(c.DamageBonusFlat2);
            t.DamageBonusFlat3 = Convert.ToString(c.DamageBonusFlat3);
            t.DamageBonusFlatValue = c.DamageBonusFlatValue;
            t.DamageBonusFlatValue2 = c.DamageBonusFlatValue2;
            t.DamageBonusFlatValue3 = c.DamageBonusFlatValue3;
            t.DamageBonusPercent = Convert.ToString(c.DamageBonusPercent);
            t.DamageBonusPercent2 = Convert.ToString(c.DamageBonusPercent2);
            t.DamageBonusPercent3 = Convert.ToString(c.DamageBonusPercent3);
            t.DamageBonusPercentValue = c.DamageBonusPercentValue;
            t.DamageBonusPercentValue2 = c.DamageBonusPercentValue2;
            t.DamageBonusPercentValue3 = c.DamageBonusPercentValue3;
            t.Description = c.Description;
            t.HealFlatBonus = c.HealFlatBonus;
            t.HealPercentBonus = c.HealPercentBonus;
            t.HealReceivedFlatBonus = c.HealReceivedFlatBonus;
            t.HealReceivedPercentBonus = c.HealReceivedPercentBonus;
            t.ResistModified1 = Convert.ToString(c.ResistModified1);
            t.ResistModified2 = Convert.ToString(c.ResistModified2);
            t.ResistModified3 = Convert.ToString(c.ResistModified3);
            t.ResistModifiedValue1 = c.ResistModifiedValue1;
            t.ResistModifiedValue2 = c.ResistModifiedValue2;
            t.ResistModifiedValue3 = c.ResistModifiedValue3;
            t.TimesPerRound = c.TimesPerRound;
            t.TimesPerTurn = c.TimesPerTurn;
            t.TraitCard = Convert.ToString(c.TraitCard);
            t.TraitCardForAllHeroes = Convert.ToString(c.TraitCardForAllHeroes);
            t.TraitName = c.TraitName;
            return t;
        }

        public static SubClassDataText SubClassData(SubClassData c)
        {
            SubClassDataText t = new();
            t.ID = c.Id;
            if (c.Cards.Length > 0) // consider making a HeroCardsText that you can ToJson?
            {
                t.Cards = new string[c.Cards.Length];
                for (int a = 0; a < c.Cards.Length; a++)
                {
                    if ((HeroCards)c.Cards[a] != (HeroCards)null && (CardData)c.Cards[a].Card != (CardData)null)
                        t.Cards[a] = c.Cards[a].UnitsInDeck + "|" + c.Cards[a].Card.Id;
                }
            }
            t.ChallengePack0 = Convert.ToString(c.ChallengePack0);
            t.ChallengePack1 = Convert.ToString(c.ChallengePack1);
            t.ChallengePack2 = Convert.ToString(c.ChallengePack2);
            t.ChallengePack3 = Convert.ToString(c.ChallengePack3);
            t.ChallengePack4 = Convert.ToString(c.ChallengePack4);
            t.ChallengePack5 = Convert.ToString(c.ChallengePack5);
            t.ChallengePack6 = Convert.ToString(c.ChallengePack6);
            t.CharacterDescription = c.CharacterDescription;
            t.CharacterDescriptionStrength = c.CharacterDescriptionStrength;
            t.CharacterName = c.CharacterName;
            t.Female = c.Female;
            t.GameObjectAnimated = Convert.ToString(c.GameObjectAnimated);
            t.HeroClass = Convert.ToString(c.HeroClass);
            t.HitSound = Convert.ToString(c.HitSound);
            t.HP = c.Hp;
            t.MaxHP = c.MaxHp;
            t.ResistSlashing = c.ResistSlashing;
            t.ResistBlunt = c.ResistBlunt;
            t.ResistPiercing = c.ResistPiercing;
            t.ResistFire = c.ResistFire;
            t.ResistCold = c.ResistCold;
            t.ResistLightning = c.ResistLightning;
            t.ResistHoly = c.ResistHoly;
            t.ResistShadow = c.ResistShadow;
            t.ResistMind = c.ResistMind;
            t.Speed = c.Speed;
            t.Sprite = Convert.ToString(c.Sprite);
            t.SpriteBorder = Convert.ToString(c.SpriteBorder);
            t.SpriteBorderLocked = Convert.ToString(c.SpriteBorderLocked);
            t.SpriteBorderSmall = Convert.ToString(c.SpriteBorderSmall);
            t.SpritePortrait = Convert.ToString(c.SpritePortrait);
            t.SpriteSpeed = Convert.ToString(c.SpriteSpeed);
            t.StickerAngry = Convert.ToString(c.StickerAngry);
            t.StickerBase = Convert.ToString(c.StickerBase);
            t.StickerIndifferent = Convert.ToString(c.StickerIndiferent);
            t.StickerLove = Convert.ToString(c.StickerLove);
            t.StickerSurprise = Convert.ToString(c.StickerSurprise);
            t.StickerOffsetX = c.StickerOffsetX;
            t.SubclassName = c.SubClassName;
            t.Trait0 = Convert.ToString(c.Trait0);
            t.Trait1A = Convert.ToString(c.Trait0);
            t.Trait1B = Convert.ToString(c.Trait1B);
            t.Trait2A = Convert.ToString(c.Trait2A);
            t.Trait2B = Convert.ToString(c.Trait2B);
            t.Trait3A = Convert.ToString(c.Trait3A);
            t.Trait3B = Convert.ToString(c.Trait3B);
            t.Trait4A = Convert.ToString(c.Trait4A);
            t.Trait4B = Convert.ToString(c.Trait4B);
            return t;
        }

        public static NPCDataText NPCData(NPCData data)
        {
            NPCDataText text = new();
            text.AICards = new string[data.AICards.Length];
            for (int a = 0; a < data.AICards.Length; a++)
                text.AICards[a] = JsonUtility.ToJson(AICards(data.AICards[a]));
            
            return text;
        }

        public static AICardsText AICards(AICards data)
        {
            AICardsText text = new();
            text.AddCardRound = data.AddCardRound;
            text.AuraCurseCastIf = Convert.ToString(data.AuracurseCastIf);
            text.Card = Convert.ToString(data.Card);
            text.OnlyCastIf = Convert.ToString(data.OnlyCastIf);
            text.PercentToCast = data.PercentToCast;
            text.Priority = data.Priority;
            text.TargetCast = Convert.ToString(data.TargetCast);
            text.UnitsInDeck = data.UnitsInDeck;
            text.ValueCastIf = data.ValueCastIf;
            return text;
        }
    }
    public class Text2Data
    {
        public static CardData CardData(CardDataText t, bool overWrite = false)
        {
            CardData c;
            if (Plugin.medsCardsSource.ContainsKey(t.ID))
                c = Plugin.medsCardsSource[t.ID]; // UnityEngine.Object.Instantiate<CardData>(medsCardsSource[t.ID]);
            else
                c = UnityEngine.Object.Instantiate<CardData>(Plugin.medsCardsSource["defend"]);
            AuraCurseData auraCurse;
            c.name = t.CardName;
            c.Id = t.ID;
            c.InternalId = t.ID;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AcEnergyBonus);
            c.AcEnergyBonus = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AcEnergyBonus2);
            c.AcEnergyBonus2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.AcEnergyBonusQuantity = t.AcEnergyBonusQuantity;
            c.AcEnergyBonus2Quantity = t.AcEnergyBonus2Quantity;
            c.AddCard = t.AddCard;
            c.AddCardChoose = t.AddCardChoose;
            c.AddCardCostTurn = t.AddCardCostTurn;
            try
            {
                c.AddCardFrom = (CardFrom)Enum.Parse(typeof(CardFrom), t.AddCardFrom, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data AddCardFrom value: " + t.AddCardFrom);
            }
            c.AddCardId = t.AddCardId;
            try
            {
                c.AddCardPlace = (CardPlace)Enum.Parse(typeof(CardPlace), t.AddCardPlace, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data AddCardPlace value: " + t.AddCardPlace);
            }
            c.AddCardReducedCost = t.AddCardReducedCost;
            try
            {
                c.AddCardType = (CardType)Enum.Parse(typeof(CardType), t.AddCardType, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data AddCardType value: " + t.AddCardType);
            }
            for (int a = 0; a < t.AddCardTypeAux.Length; a++)
            {
                c.AddCardTypeAux = new CardType[a];
                try
                {
                    c.AddCardTypeAux[a] = (CardType)Enum.Parse(typeof(CardType), t.AddCardTypeAux[a], true);
                }
                catch
                {
                    Plugin.Log.LogError("Unable to parse CardText2Data AddCardTypeAux value " + a.ToString() + ": " + t.AddCardTypeAux[a]);
                }
            }
            c.AddCardVanish = t.AddCardVanish;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Aura);
            c.Aura = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Aura2);
            c.Aura2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Aura3);
            c.Aura3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.AuraCharges = t.AuraCharges;
            c.AuraChargesSpecialValue1 = t.AuraChargesSpecialValue1;
            c.AuraChargesSpecialValue2 = t.AuraChargesSpecialValue2;
            c.AuraChargesSpecialValueGlobal = t.AuraChargesSpecialValueGlobal;
            c.AuraCharges2 = t.AuraCharges2;
            c.AuraCharges2SpecialValue1 = t.AuraCharges2SpecialValue1;
            c.AuraCharges2SpecialValue2 = t.AuraCharges2SpecialValue2;
            c.AuraCharges2SpecialValueGlobal = t.AuraCharges2SpecialValueGlobal;
            c.AuraCharges3 = t.AuraCharges3;
            c.AuraCharges3SpecialValue1 = t.AuraCharges3SpecialValue1;
            c.AuraCharges3SpecialValue2 = t.AuraCharges3SpecialValue2;
            c.AuraCharges3SpecialValueGlobal = t.AuraCharges3SpecialValueGlobal;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraSelf);
            c.AuraSelf = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraSelf2);
            c.AuraSelf2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraSelf3);
            c.AuraSelf3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.AutoplayDraw = t.AutoplayDraw;
            c.AutoplayEndTurn = t.AutoplayEndTurn;
            c.BaseCard = t.BaseCard;
            try
            {
                c.CardClass = (CardClass)Enum.Parse(typeof(CardClass), t.CardClass, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data CardClass value: " + t.CardClass);
            }
            c.CardName = t.CardName;
            c.CardNumber = t.CardNumber;
            try
            {
                c.CardRarity = (CardRarity)Enum.Parse(typeof(CardRarity), t.CardRarity, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data CardRarity value: " + t.CardRarity);
            }
            try
            {
                c.CardType = (CardType)Enum.Parse(typeof(CardType), t.CardType, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data CardType value: " + t.CardType);
            }
            for (int a = 0; a < t.CardTypeAux.Length; a++)
            {
                c.CardTypeAux = new CardType[a];
                try
                {
                    c.CardTypeAux[a] = (CardType)Enum.Parse(typeof(CardType), t.CardTypeAux[a], true);
                }
                catch
                {
                    Plugin.Log.LogError("Unable to parse CardText2Data CardTypeAux value " + a.ToString() + ": " + t.CardTypeAux[a]);
                }
            }
            // CardTypeList
            //if (t.CardTypeList.Length > 0)
            //{
            //c.CardTypeAux = new string[t.CardTypeAux.Length];
            //for (int a = 0; a < t.CardTypeAux.Length; a++)
            //c.CardTypeAux[a] = Convert.ToString(t.CardTypeAux[a]);
            //}
            try
            {
                c.CardUpgraded = (CardUpgraded)Enum.Parse(typeof(CardUpgraded), t.CardUpgraded, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data CardUpgraded value: " + t.CardUpgraded);
            }
            c.Corrupted = t.Corrupted;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Curse);
            c.Curse = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Curse2);
            c.Curse2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.Curse3);
            c.Curse3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.CurseCharges = t.CurseCharges;
            c.CurseChargesSpecialValue1 = t.CurseChargesSpecialValue1;
            c.CurseChargesSpecialValue2 = t.CurseChargesSpecialValue2;
            c.CurseChargesSpecialValueGlobal = t.CurseChargesSpecialValueGlobal;
            c.CurseCharges2 = t.CurseCharges2;
            c.CurseCharges2SpecialValue1 = t.CurseCharges2SpecialValue1;
            c.CurseCharges2SpecialValue2 = t.CurseCharges2SpecialValue2;
            c.CurseCharges2SpecialValueGlobal = t.CurseCharges2SpecialValueGlobal;
            c.CurseCharges3 = t.CurseCharges3;
            c.CurseCharges3SpecialValue1 = t.CurseCharges3SpecialValue1;
            c.CurseCharges3SpecialValue2 = t.CurseCharges3SpecialValue2;
            c.CurseCharges3SpecialValueGlobal = t.CurseCharges3SpecialValueGlobal;
            auraCurse = Globals.Instance.GetAuraCurseData(t.CurseSelf);
            c.CurseSelf = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.CurseSelf2);
            c.CurseSelf2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.CurseSelf3);
            c.CurseSelf3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.Damage = t.Damage;
            c.DamageSpecialValue1 = t.DamageSpecialValue1;
            c.DamageSpecialValue2 = t.DamageSpecialValue2;
            c.DamageSpecialValueGlobal = t.DamageSpecialValueGlobal;
            c.Damage2 = t.Damage2;
            c.Damage2SpecialValue1 = t.Damage2SpecialValue1;
            c.Damage2SpecialValue2 = t.Damage2SpecialValue2;
            c.Damage2SpecialValueGlobal = t.Damage2SpecialValueGlobal;
            c.DamageEnergyBonus = t.DamageEnergyBonus;
            c.DamageSelf = t.DamageSelf;
            c.DamageSelf2 = t.DamageSelf2;
            c.DamageSides = t.DamageSides;
            c.DamageSides2 = t.DamageSides2;
            try
            {
                c.DamageType = (DamageType)Enum.Parse(typeof(DamageType), t.DamageType, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data DamageType value: " + t.DamageType);
            }
            try
            {
                c.DamageType2 = (DamageType)Enum.Parse(typeof(DamageType), t.DamageType2, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data DamageType2 value: " + t.DamageType2);
            }
            c.Description = t.Description;
            // c.DescriptionID = t.descriptionid
            c.DiscardCard = t.DiscardCard;
            c.DiscardCardAutomatic = t.DiscardCardAutomatic;
            try
            {
                c.DiscardCardPlace = (CardPlace)Enum.Parse(typeof(CardPlace), t.DiscardCardPlace, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data DiscardCardPlace value: " + t.DiscardCardPlace);
            }
            try
            {
                c.DiscardCardType = (CardType)Enum.Parse(typeof(CardType), t.DiscardCardType, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data DiscardCardType value: " + t.DiscardCardType);
            }
            for (int a = 0; a < t.DiscardCardTypeAux.Length; a++)
            {
                c.DiscardCardTypeAux = new CardType[a];
                try
                {
                    c.DiscardCardTypeAux[a] = (CardType)Enum.Parse(typeof(CardType), t.DiscardCardTypeAux[a], true);
                }
                catch
                {
                    Plugin.Log.LogError("Unable to parse CardText2Data DiscardCardTypeAux value " + a.ToString() + ": " + t.DiscardCardTypeAux[a]);
                }
            }
            c.DispelAuras = t.DispelAuras;
            c.DrawCard = t.DrawCard;
            c.EffectCastCenter = t.EffectCastCenter;
            c.EffectCaster = t.EffectCaster;
            c.EffectCasterRepeat = t.EffectCasterRepeat;
            c.EffectPostCastDelay = t.EffectPostCastDelay;
            c.EffectPostTargetDelay = t.EffectPostTargetDelay;
            c.EffectPreAction = t.EffectPreAction;
            c.EffectRepeat = t.EffectRepeat;
            c.EffectRepeatDelay = t.EffectRepeatDelay;
            c.EffectRepeatEnergyBonus = t.EffectRepeatEnergyBonus;
            c.EffectRepeatMaxBonus = t.EffectRepeatMaxBonus;
            c.EffectRepeatModificator = t.EffectRepeatModificator;
            try
            {
                c.EffectRepeatTarget = (EffectRepeatTarget)Enum.Parse(typeof(EffectRepeatTarget), t.EffectRepeatTarget, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data EffectRepeatTarget value: " + t.EffectRepeatTarget);
            }
            c.EffectRequired = t.EffectRequired;
            c.EffectTarget = t.EffectTarget;
            c.EffectTrail = t.EffectTrail;
            try
            {
                c.EffectTrailAngle = (EffectTrailAngle)Enum.Parse(typeof(EffectTrailAngle), t.EffectTrailAngle, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data EffectTrailAngle value: " + t.EffectTrailAngle);
            }
            c.EffectTrailRepeat = t.EffectTrailRepeat;
            c.EffectTrailSpeed = t.EffectTrailSpeed;
            c.EndTurn = t.EndTurn;
            c.EnergyCost = t.EnergyCost;
            c.EnergyCostForShow = t.EnergyCostForShow;
            c.EnergyRecharge = t.EnergyRecharge;
            c.EnergyReductionPermanent = t.EnergyReductionPermanent;
            c.EnergyReductionTemporal = t.EnergyReductionTemporal;
            c.EnergyReductionToZeroPermanent = t.EnergyReductionToZeroPermanent;
            c.EnergyReductionToZeroTemporal = t.EnergyReductionToZeroTemporal;
            c.ExhaustCounter = t.ExhaustCounter;
            c.FlipSprite = t.FlipSprite;
            c.Fluff = t.Fluff;
            c.FluffPercent = t.FluffPercent;
            c.GoldGainQuantity = t.GoldGainQuantity;
            c.Heal = t.Heal;
            auraCurse = Globals.Instance.GetAuraCurseData(t.HealAuraCurseName);
            c.HealAuraCurseName = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.HealAuraCurseName2);
            c.HealAuraCurseName2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.HealAuraCurseName3);
            c.HealAuraCurseName3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.HealAuraCurseName4);
            c.HealAuraCurseName4 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.HealAuraCurseSelf);
            c.HealAuraCurseSelf = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.HealCurses = t.HealCurses;
            c.HealEnergyBonus = t.HealEnergyBonus;
            c.HealSelf = t.HealSelf;
            c.HealSelfPerDamageDonePercent = t.HealSelfPerDamageDonePercent;
            c.HealSelfSpecialValue1 = t.HealSelfSpecialValue1;
            c.HealSelfSpecialValue2 = t.HealSelfSpecialValue2;
            c.HealSelfSpecialValueGlobal = t.HealSelfSpecialValueGlobal;
            c.HealSides = t.HealSides;
            c.HealSpecialValue1 = t.HealSpecialValue1;
            c.HealSpecialValue2 = t.HealSpecialValue2;
            c.HealSpecialValueGlobal = t.HealSpecialValueGlobal;
            c.IgnoreBlock = t.IgnoreBlock;
            c.IgnoreBlock2 = t.IgnoreBlock2;
            c.IncreaseAuras = t.IncreaseAuras;
            c.IncreaseCurses = t.IncreaseCurses;
            c.Innate = t.Innate;
            c.IsPetAttack = t.IsPetAttack;
            c.IsPetCast = t.IsPetCast;
            c.KillPet = t.KillPet;
            c.Lazy = t.Lazy;
            c.LookCards = t.LookCards;
            c.LookCardsDiscardUpTo = t.LookCardsDiscardUpTo;
            c.LookCardsVanishUpTo = t.LookCardsVanishUpTo;
            c.MaxInDeck = t.MaxInDeck;
            c.ModifiedByTrait = t.ModifiedByTrait;
            c.MoveToCenter = t.MoveToCenter;
            c.OnlyInWeekly = t.OnlyInWeekly;
            c.PetFront = t.PetFront;
            c.PetInvert = t.PetInvert;
            // c.PetModel = ""; // no clue, not worth it?
            // c.PetOffset = ""; // no clue, not worth it?
            // c.PetSize = ""; // no clue, not worth it?
            c.Playable = t.Playable;
            c.PullTarget = t.PullTarget;
            c.PushTarget = t.PushTarget;
            c.ReduceAuras = t.ReduceAuras;
            c.ReduceCurses = t.ReduceCurses;
            c.RelatedCard = t.RelatedCard;
            c.RelatedCard2 = t.RelatedCard2;
            c.RelatedCard3 = t.RelatedCard3;
            c.SelfHealthLoss = t.SelfHealthLoss;
            c.SelfHealthLossSpecialGlobal = t.SelfHealthLossSpecialGlobal;
            c.SelfHealthLossSpecialValue1 = t.SelfHealthLossSpecialValue1;
            c.SelfHealthLossSpecialValue2 = t.SelfHealthLossSpecialValue2;
            c.ShardsGainQuantity = t.ShardsGainQuantity;
            c.ShowInTome = t.ShowInTome;
            c.Sku = t.Sku;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SpecialAuraCurseName1);
            c.SpecialAuraCurseName1 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SpecialAuraCurseName2);
            c.SpecialAuraCurseName2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SpecialAuraCurseNameGlobal);
            c.SpecialAuraCurseNameGlobal = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            try
            {
                c.SpecialValue1 = (CardSpecialValue)Enum.Parse(typeof(CardSpecialValue), t.SpecialValue1, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data SpecialValue1 value: " + t.SpecialValue1);
            }
            try
            {
                c.SpecialValue2 = (CardSpecialValue)Enum.Parse(typeof(CardSpecialValue), t.SpecialValue2, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data SpecialValue2 value: " + t.SpecialValue2);
            }
            try
            {
                c.SpecialValueGlobal = (CardSpecialValue)Enum.Parse(typeof(CardSpecialValue), t.SpecialValueGlobal, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data SpecialValueGlobal value: " + t.SpecialValueGlobal);
            }
            c.SpecialValueModifier1 = t.SpecialValueModifier1;
            c.SpecialValueModifier2 = t.SpecialValueModifier2;
            c.SpecialValueModifierGlobal = t.SpecialValueModifierGlobal;
            c.Starter = t.Starter;
            c.StealAuras = t.StealAuras;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SummonAura);
            c.SummonAura = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SummonAura2);
            c.SummonAura2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.SummonAura3);
            c.SummonAura3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.SummonAuraCharges = t.SummonAuraCharges;
            c.SummonAuraCharges2 = t.SummonAuraCharges2;
            c.SummonAuraCharges3 = t.SummonAuraCharges3;
            // c.SummonUnit = ((UnityEngine.Object)t.SummonUnit != (UnityEngine.Object)null) ? t.SummonUnit.Id : ""; // maybe later :)
            // c.SummonUnitNum = t.SummonUnitNum; // maybe later :)
            try
            {
                c.TargetPosition = (CardTargetPosition)Enum.Parse(typeof(CardTargetPosition), t.TargetPosition, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data TargetPosition value: " + t.TargetPosition);
            }
            try
            {
                c.TargetSide = (CardTargetSide)Enum.Parse(typeof(CardTargetSide), t.TargetSide, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data TargetSide value: " + t.TargetSide);
            }
            try
            {
                c.TargetType = (CardTargetType)Enum.Parse(typeof(CardTargetType), t.TargetType, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse CardText2Data TargetType value: " + t.TargetType);
            }
            c.TransferCurses = t.TransferCurses;
            c.UpgradedFrom = t.UpgradedFrom;
            c.UpgradesTo1 = t.UpgradesTo1;
            c.UpgradesTo2 = t.UpgradesTo2;
            c.Vanish = t.Vanish;
            c.Visible = t.Visible;


            return c;
        }

        public static TraitData TraitData(TraitDataText t)
        {
            TraitData c = new();
            AuraCurseData auraCurse;
            CardData traitCard;
            c.Id = t.ID;
            try
            {
                c.Activation = (EventActivation)Enum.Parse(typeof(EventActivation), t.Activation, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data Activation value: " + t.Activation);
            }
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraCurseBonus1);
            c.AuracurseBonus1 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraCurseBonus2);
            c.AuracurseBonus2 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            auraCurse = Globals.Instance.GetAuraCurseData(t.AuraCurseBonus3);
            c.AuracurseBonus3 = ((UnityEngine.Object)auraCurse != (UnityEngine.Object)null) ? auraCurse : (AuraCurseData)null;
            c.AuracurseBonusValue1 = t.AuraCurseBonusValue1;
            c.AuracurseBonusValue2 = t.AuraCurseBonusValue2;
            c.AuracurseBonusValue3 = t.AuraCurseBonusValue3;
            c.AuracurseImmune1 = t.AuraCurseImmune1;
            c.AuracurseImmune2 = t.AuraCurseImmune2;
            c.AuracurseImmune3 = t.AuraCurseImmune3;
            try
            {
                c.CharacterStatModified = (CharacterStat)Enum.Parse(typeof(CharacterStat), t.CharacterStatModified, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data CharacterStatModified value: " + t.CharacterStatModified);
            }
            c.CharacterStatModifiedValue = t.CharacterStatModifiedValue;
            try
            {
                c.DamageBonusFlat = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusFlat, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusFlat value: " + t.DamageBonusFlat);
            }
            try
            {
                c.DamageBonusFlat2 = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusPercent, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusFlat2 value: " + t.DamageBonusFlat2);
            }
            try
            {
                c.DamageBonusFlat3 = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusFlat3, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusFlat3 value: " + t.DamageBonusFlat3);
            }
            c.DamageBonusFlatValue = t.DamageBonusFlatValue;
            c.DamageBonusFlatValue2 = t.DamageBonusFlatValue2;
            c.DamageBonusFlatValue3 = t.DamageBonusFlatValue3;
            try
            {
                c.DamageBonusPercent = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusPercent, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusPercent value: " + t.DamageBonusPercent);
            }
            try
            {
                c.DamageBonusPercent2 = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusPercent2, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusPercent2 value: " + t.DamageBonusPercent2);
            }
            try
            {
                c.DamageBonusPercent3 = (DamageType)Enum.Parse(typeof(DamageType), t.DamageBonusPercent3, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data DamageBonusPercent3 value: " + t.DamageBonusPercent3);
            }
            c.DamageBonusPercentValue = t.DamageBonusPercentValue;
            c.DamageBonusPercentValue2 = t.DamageBonusPercentValue2;
            c.DamageBonusPercentValue3 = t.DamageBonusPercentValue3;
            c.Description = t.Description;
            c.HealFlatBonus = t.HealFlatBonus;
            c.HealPercentBonus = t.HealPercentBonus;
            c.HealReceivedFlatBonus = t.HealReceivedFlatBonus;
            c.HealReceivedPercentBonus = t.HealReceivedPercentBonus;
            try
            {
                c.ResistModified1 = (DamageType)Enum.Parse(typeof(DamageType), t.ResistModified1, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data ResistModified1 value: " + t.ResistModified1);
            }
            try
            {
                c.ResistModified2 = (DamageType)Enum.Parse(typeof(DamageType), t.ResistModified2, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data ResistModified2 value: " + t.ResistModified2);
            }
            try
            {
                c.ResistModified3 = (DamageType)Enum.Parse(typeof(DamageType), t.ResistModified3, true);
            }
            catch
            {
                Plugin.Log.LogError("Unable to parse TraitText2Data ResistModified3 value: " + t.ResistModified3);
            }
            c.ResistModifiedValue1 = t.ResistModifiedValue1;
            c.ResistModifiedValue2 = t.ResistModifiedValue2;
            c.ResistModifiedValue3 = t.ResistModifiedValue3;
            c.TimesPerRound = t.TimesPerRound;
            c.TimesPerTurn = t.TimesPerTurn;
            traitCard = Globals.Instance.GetCardData(t.TraitCard);
            c.TraitCard = ((UnityEngine.Object)traitCard != (UnityEngine.Object)null) ? traitCard : (CardData)null;
            traitCard = Globals.Instance.GetCardData(t.TraitCardForAllHeroes);
            c.TraitCardForAllHeroes = ((UnityEngine.Object)traitCard != (UnityEngine.Object)null) ? traitCard : (CardData)null;
            c.TraitName = t.TraitName;
            return c;
        }
    }

    public class Convert
    {
        public static string ToString(AuraCurseData data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.Id : "";
        }
        public static string ToString(CardData data)
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
        public static string ToString(UnityEngine.Object data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString(UnityEngine.AudioClip data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString(UnityEngine.GameObject data)
        {
            return ((UnityEngine.Object)data != (UnityEngine.Object)null) ? data.name : "";
        }
        public static string ToString<T>(T data)
        {
            if (typeof(T).BaseType == typeof(System.Enum))
                return Enum.GetName(data.GetType(), data);
            Plugin.Log.LogError("ToString<T> is capturing a type that it isn't set up for!!! " + typeof(T));
            return "";
        }
        public static string[] ToString(CardData[] data)
        {
            if (data.Length > 0)
            {
                string[] text = new string[data.Length];
                for (int a = 0; a < data.Length; a++)
                    text[a] = ToString(data[a]);
                return text;
            }
            return new string[0];
        }
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
                Plugin.Log.LogError("ToString<T[]> is capturing a type that it isn't set up for!!! " + typeof(T));
            }
            return new string[0];
        }

    }
}

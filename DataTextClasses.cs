using System;
using static Enums;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine;

namespace Obeliskial_Options
{
    [Serializable]
    public class DataText { }

    [Serializable]
    public class SubClassDataText : DataText
    {
        public string ActionSound;
        public string[] Cards; // quantity|name
        public string ChallengePack0;
        public string ChallengePack1;
        public string ChallengePack2;
        public string ChallengePack3;
        public string ChallengePack4;
        public string ChallengePack5;
        public string ChallengePack6;
        public string CharacterDescription;
        public string CharacterDescriptionStrength;
        public string CharacterName;
        public int Energy;
        public int EnergyTurn;
        public bool Female;
        public float FluffOffsetX;
        public float FluffOffsetY;
        public string GameObjectAnimated; // ???
        public string HeroClass;
        public string HitSound; // ???
        public int HP;
        public string ID;
        public string Item;
        public int[] MaxHP; // e.g. 0|5|5|5|5 for Malunah?
        public int ResistSlashing;
        public int ResistBlunt;
        public int ResistPiercing;
        public int ResistFire;
        public int ResistCold;
        public int ResistLightning;
        public int ResistHoly;
        public int ResistShadow;
        public int ResistMind;
        public int Speed;
        public string Sprite; // ???
        public string SpriteBorder; // ???
        public string SpriteBorderLocked; // ???
        public string SpriteBorderSmall; // ???
        public string SpritePortrait; // ???
        public string SpriteSpeed; // ???
        public string StickerBase; // ???
        public string StickerAngry; // ???
        public string StickerIndifferent; // ???
        public string StickerLove; // ???
        public string StickerSurprise; // ???
        public float StickerOffsetX;
        public string SubclassName;
        public string Trait0;
        public string Trait1A; // assumes card with same name
        public string Trait1B; // assumes card with same name
        public string Trait2A;
        public string Trait2B;
        public string Trait3A; // assumes card with same name
        public string Trait3B; // assumes card with same name
        public string Trait4A;
        public string Trait4B;
    }

    [Serializable]
    public class HeroCardsText : DataText
    {
        public int UnitsInDeck;
        public string Card; // CardData
    }

    [Serializable]
    public class TraitDataText : DataText
    {
        public string Activation; // Enums.EventActivation // PreBeginCombat for Piss's L3 Sacred
        public string AuraCurseBonus1; // "Sanctify" (AuraCurseData)
        public string AuraCurseBonus2; // null (AuraCurseData)
        public string AuraCurseBonus3; // null (AuraCurseData)
        public int AuraCurseBonusValue1; // 0
        public int AuraCurseBonusValue2; // 0
        public int AuraCurseBonusValue3; // 0
        public string AuraCurseImmune1; // "fatigue" for Magna's L5 Tireless 
        public string AuraCurseImmune2; // ""
        public string AuraCurseImmune3; // ""
        public string CharacterStatModified; // enums.characterStat None/Hp/Speed/Energy/EnergyTurn [Energy: starting?]
        public int CharacterStatModifiedValue; // 0
        public string DamageBonusFlat; // Enums.DamageType
        public string DamageBonusFlat2;
        public string DamageBonusFlat3;
        public int DamageBonusFlatValue; // 0
        public int DamageBonusFlatValue2;
        public int DamageBonusFlatValue3;
        public string DamageBonusPercent; // Enums.DamageType
        public string DamageBonusPercent2;
        public string DamageBonusPercent3;
        public float DamageBonusPercentValue; // 0
        public float DamageBonusPercentValue2;
        public float DamageBonusPercentValue3;
        public string Description;
        public int HealFlatBonus;
        public float HealPercentBonus;
        public int HealReceivedFlatBonus;
        public float HealReceivedPercentBonus;
        public string ID; // filename?
        // ignore keynotes public string[] KeyNotes; // [ "Energy" (KeyNotesData), "Fatigue" (KeyNotesData) ] // the help text?
        public string ResistModified1; // Enums.DamageType
        public string ResistModified2;
        public string ResistModified3;
        public int ResistModifiedValue1;
        public int ResistModifiedValue2;
        public int ResistModifiedValue3;
        public int TimesPerRound; // e.g. rat on book use?
        public int TimesPerTurn; // e.g. rat on book use?
        public string TraitCard; // Globals.Instance.GetCardData("vaccine") ?? (actually uses the least-cloned; is this from source or?)
        public string TraitCardForAllHeroes; // e.g. Nezglekt's Friendly Tadpole // Globals.Instance.GetCardData("vaccine") ?? (actually uses the least-cloned; is this from source or?)
        public string TraitName; // "nice" name
    }

    [Serializable]
    public class CardDataText : DataText
    {
        // #TODO: enchant damage? enchantDamage, enchantDamagePreCalculated? e.g. profisherface?


        // public IGNORE DamagePreCalculated; // IGNORE; InitClone automatically sets
        // public IGNORE DamagePreCalculated2; // IGNORE; InitClone automatically sets
        // public IGNORE DamageSelfPreCalculated; // IGNORE; InitClone automatically sets
        // public IGNORE DamageSelfPreCalculated2; // IGNORE; InitClone automatically sets
        // public IGNORE DamageSidesPreCalculated; // IGNORE; InitClone automatically sets
        // public IGNORE DamageSidesPreCalculated2; // IGNORE; InitClone automatically sets
        // public IGNORE DescriptionNormalized; // built by CardData.SetDescriptionNew
        // public IGNORE EnergyCostOriginal; // can probably always set to 0, given InitClone will set it
        // public IGNORE InternalID; // InitClone automatically sets to ID
        // public IGNORE KeyNotes; // automatically built by CreateCardClones-->CardData.InitClone, so no need to worry about it?
        // public IGNORE Target; // probably set by CardData.SetTarget?

        public string AcEnergyBonus; // AuraCurseData
        public string AcEnergyBonus2; // AuraCurseData
        public int AcEnergyBonusQuantity;
        public int AcEnergyBonus2Quantity;
        public int AddCard;
        public int AddCardChoose;
        public bool AddCardCostTurn;
        public string AddCardFrom; // Enums.CardFrom (Mass Invis: Deck?)
        public string AddCardId;
        public string[] AddCardList; // then Globals.Instance.GetCardData each?
        public string AddCardPlace; // Enums.CardPlace (Mass Invis: Discard?)
        public int AddCardReducedCost;
        public string AddCardType; // Enums.CardType (Mass Invis: None)
        public string[] AddCardTypeAux; // ???
        public bool AddCardVanish;
        public string Aura; // Mass Invis: "Stealth" (AuraCurseData)
        public string Aura2;
        public string Aura3;
        public int AuraCharges;
        public bool AuraChargesSpecialValue1;
        public bool AuraChargesSpecialValue2;
        public bool AuraChargesSpecialValueGlobal;
        public int AuraCharges2;
        public bool AuraCharges2SpecialValue1;
        public bool AuraCharges2SpecialValue2;
        public bool AuraCharges2SpecialValueGlobal;
        public int AuraCharges3;
        public bool AuraCharges3SpecialValue1;
        public bool AuraCharges3SpecialValue2;
        public bool AuraCharges3SpecialValueGlobal;
        public string AuraSelf; // null (AuraCurseData)
        public string AuraSelf2; // null (AuraCurseData)
        public string AuraSelf3; // null (AuraCurseData)
        public bool AutoplayDraw;
        public bool AutoplayEndTurn;
        public string BaseCard; // note: MassInvisibility (i.e., not lowercase), actual string
        public string CardClass; // Enums.CardClass
        public string CardName; // "nice" name
        public int CardNumber; // Mass Invis: 0
        public string CardRarity; // Enums.CardRarity
        public string CardType; // Enums.CardType
        public string[] CardTypeAux; // ???
        public string[] CardTypeList; // List<Enums.CardType> // when would this be used? Check pls
        public string CardUpgraded; // Enums.CardUpgraded
        public bool Corrupted;
        public string Curse; // null (AuraCurseData)
        public string Curse2; // null (AuraCurseData)
        public string Curse3; // null (AuraCurseData)
        public int CurseCharges;
        public bool CurseChargesSpecialValue1; // ???
        public bool CurseChargesSpecialValue2; // ???
        public bool CurseChargesSpecialValueGlobal; // ???
        public int CurseCharges2;
        public bool CurseCharges2SpecialValue1; // ???
        public bool CurseCharges2SpecialValue2; // ???
        public bool CurseCharges2SpecialValueGlobal; // ???
        public int CurseCharges3;
        public bool CurseCharges3SpecialValue1; // ???
        public bool CurseCharges3SpecialValue2; // ???
        public bool CurseCharges3SpecialValueGlobal; // ???
        public string CurseSelf; // null (AuraCurseData)
        public string CurseSelf2; // null (AuraCurseData)
        public string CurseSelf3; // null (AuraCurseData)
        public int Damage;
        public bool DamageSpecialValue1; // ???
        public bool DamageSpecialValue2; // ???
        public bool DamageSpecialValueGlobal; // ???
        public int Damage2;
        public bool Damage2SpecialValue1; // ???
        public bool Damage2SpecialValue2; // ???
        public bool Damage2SpecialValueGlobal; // ???
        public int DamageEnergyBonus;
        public int DamageSelf;
        public int DamageSelf2;
        public int DamageSides;
        public int DamageSides2;
        public string DamageType; // Enums.DamageType
        public string DamageType2; // Enums.DamageType
        public string Description; // "<Grant><br2><Aura_AuraCharges>"
        public string DescriptionID; // ???
        public int DiscardCard;
        public bool DiscardCardAutomatic;
        public string DiscardCardPlace; // Enums.CardPlace
        public string DiscardCardType; // Enums.CardType
        public string[] DiscardCardTypeAux; // List<Enums.CardType> // when would this be used? Check pls
        public int DispelAuras;
        public int DrawCard;
        public bool EffectCastCenter;
        public string EffectCaster;
        public bool EffectCasterRepeat;
        public float EffectPostCastDelay;
        public float EffectPostTargetDelay;
        public string EffectPreAction;
        public int EffectRepeat; // Mass Invis: 1
        public float EffectRepeatDelay;
        public int EffectRepeatEnergyBonus;
        public int EffectRepeatMaxBonus;
        public int EffectRepeatModificator;
        public string EffectRepeatTarget; // Enums.EffectRepeatTarget
        public string EffectRequired; // note that InitClone2 calls GetKeyNotesData(EffectRequired), so depending on how this is used you need to add a keynote?
        public string EffectTarget; // Mass Invis: "invis"
        public string EffectTrail;
        public string EffectTrailAngle; // Enums.EffectTrailAngle
        public bool EffectTrailRepeat;
        public float EffectTrailSpeed; // Mass Invis: 1
        public bool EndTurn;
        public int EnergyCost; // Mass Invis: 2
        public int EnergyCostForShow; // Mass Invis: 0
        public int EnergyRecharge;
        public int EnergyReductionPermanent;
        public int EnergyReductionTemporal;
        public bool EnergyReductionToZeroPermanent;
        public bool EnergyReductionToZeroTemporal;
        public int ExhaustCounter;
        public bool FlipSprite;
        public string Fluff;
        public float FluffPercent;
        public int GoldGainQuantity;
        public int Heal;
        public string HealAuraCurseName; // Mass Invis: "Mark" (AuraCurseData)
        public string HealAuraCurseName2;
        public string HealAuraCurseName3;
        public string HealAuraCurseName4;
        public string HealAuraCurseSelf;
        public int HealCurses;
        public int HealEnergyBonus;
        public int HealSelf;
        public float HealSelfPerDamageDonePercent;
        public bool HealSelfSpecialValue1;
        public bool HealSelfSpecialValue2;
        public bool HealSelfSpecialValueGlobal;
        public int HealSides;
        public bool HealSpecialValue1;
        public bool HealSpecialValue2;
        public bool HealSpecialValueGlobal;
        public string ID;
        public bool IgnoreBlock;
        public bool IgnoreBlock2;
        public int IncreaseAuras;
        public int IncreaseCurses;
        public bool Innate;
        public bool IsPetAttack;
        public bool IsPetCast;
        public string Item; // then GetItemData? Is that a thing?
        public string ItemEnchantment; // null (ItemData) too btw
        public bool KillPet;
        public bool Lazy;
        public int LookCards;
        public int LookCardsDiscardUpTo;
        public int LookCardsVanishUpTo;
        public int MaxInDeck;
        public bool ModifiedByTrait;
        public bool MoveToCenter;
        public bool OnlyInWeekly;
        public bool PetFront; // Mass Invis: true
        public bool PetInvert; // Mass Invis: true
        public string PetModel; // null (UnityEngine.GameObject)
        public string PetOffset; // Vector2
        public string PetSize; // Vector2
        public bool Playable;
        public int PullTarget;
        public int PushTarget;
        public int ReduceAuras;
        public int ReduceCurses;
        public string RelatedCard; // looks like actual string?
        public string RelatedCard2; // looks like actual string?
        public string RelatedCard3; // looks like actual string?
        public int SelfHealthLoss;
        public bool SelfHealthLossSpecialGlobal;
        public bool SelfHealthLossSpecialValue1;
        public bool SelfHealthLossSpecialValue2;
        public int ShardsGainQuantity;
        public bool ShowInTome;
        public string Sku; // DLC?
        public string Sound; // Mass Invis: sparkle2 (UnityEngine.AudioClip)
        public string SoundPreAction; // null (UnityEngine.AudioClip)
        public string SoundPreActionFemale; // null (UnityEngine.AudioClip)
        public string SpecialAuraCurseName1; // null (AuraCurseData)
        public string SpecialAuraCurseName2; // null (AuraCurseData)
        public string SpecialAuraCurseNameGlobal; // null (AuraCurseData)
        public string SpecialValue1; // Enums.CardSpecialValue
        public string SpecialValue2; // Enums.CardSpecialValue
        public string SpecialValueGlobal; // Enums.CardSpecialValue
        public float SpecialValueModifier1;
        public float SpecialValueModifier2;
        public float SpecialValueModifierGlobal;
        public string Sprite; // "MassInvisibility" (UnityEngine.Sprite)
        public bool Starter;
        public int StealAuras;
        public string SummonAura; // null (AuraCurseData)
        public string SummonAura2; // null (AuraCurseData)
        public string SummonAura3; // null (AuraCurseData)
        public int SummonAuraCharges;
        public int SummonAuraCharges2;
        public int SummonAuraCharges3;
        public string SummonUnit; // null (NPCData)
        public int SummonUnitNum;
        public string TargetPosition; // Enums.CardTargetPosition
        public string TargetSide; // Enums.CardTargetSide
        public string TargetType; // Enums.CardTargetType
        public int TransferCurses;
        public string UpgradedFrom; // actual string
        public string UpgradesTo1; // "MassInvisibilityA"
        public string UpgradesTo2; // "MassInvisibilityB"
        public string UpgradesToRare; // CARDDATA, NOT STRING! MassInvisibilityRare (CardData)
        public bool Vanish; // exhaust
        public bool Visible; // ???
    }

    [Serializable]
    public class PerkDataText : DataText
    {
        public int AdditionalCurrency;
        public int AdditionalShards;
        public string AuraCurseBonus;
        public int AuraCurseBonusValue;
        public string CardClass;
        public string CustomDescription; // check if made by cloning function?
        public string DamageFlatBonus;
        public int DamageFlatBonusValue;
        public int EnergyBegin;
        public int HealQuantity;
        public string Icon; // "fast" (Unityengine.Sprite); just set based on relevant aura/etc?
        public string IconTextValue; // "+1"
        public string ID;
        public int Level;
        public bool MainPerk;
        public int MaxHealth;
        public bool ObeliskPerk;
        public string ResistModified;
        public int ResistModifiedValue;
        public int Row;
        public int SpeedQuantity;
    }

    [Serializable]
    public class AuraCurseDataText : DataText
    {
        public string ACName;
        public int AuraConsumed;
        public int AuraDamageIncreasedPercent;
        public int AuraDamageIncreasedPercent2;
        public int AuraDamageIncreasedPercent3;
        public int AuraDamageIncreasedPercent4;
        public float AuraDamageIncreasedPercentPerStack;
        public float AuraDamageIncreasedPercentPerStack2;
        public float AuraDamageIncreasedPercentPerStack3;
        public float AuraDamageIncreasedPercentPerStack4;
        public float AuraDamageIncreasedPercentPerStackPerEnergy;
        public float AuraDamageIncreasedPercentPerStackPerEnergy2;
        public float AuraDamageIncreasedPercentPerStackPerEnergy3;
        public float AuraDamageIncreasedPercentPerStackPerEnergy4;
        public float AuraDamageIncreasedPerStack;
        public float AuraDamageIncreasedPerStack2;
        public float AuraDamageIncreasedPerStack3;
        public float AuraDamageIncreasedPerStack4;
        public int AuraDamageIncreasedTotal; // check clones?
        public int AuraDamageIncreasedTotal2;
        public int AuraDamageIncreasedTotal3;
        public int AuraDamageIncreasedTotal4;
        public string AuraDamageType;
        public string AuraDamageType2;
        public string AuraDamageType3;
        public string AuraDamageType4;
        public int BlockChargesGainedPerStack;
        public int CardsDrawPerStack;
        public bool CharacterStatAbsolute;
        public int CharacterStatAbsoluteValue;
        public int CharacterStatAbsoluteValuePerStack;
        public int CharacterStatChargesMultiplierNeededForOne;
        public string CharacterStatModified;
        public int CharacterStatModifiedValue;
        public int CharacterStatModifiedValuePerStack;
        public int ChargesAuxNeedForOne1;
        public int ChargesAuxNeedForOne2;
        public int ChargesMultiplierDescription;
        public bool CombatlogShow;
        public bool ConsumeAll;
        public bool ConsumedAtCast;
        public bool ConsumedAtRound;
        public bool ConsumedAtRoundBegin;
        public bool ConsumedAtTurn;
        public bool ConsumedAtTurnBegin;
        public int CursePreventedPerStack;
        public int DamagePreventedPerStack;
        public int DamageReflectedConsumeCharges;
        public string DamageReflectedType;
        public int DamageSidesWhenConsumed;
        public int DamageSidesWhenConsumedPerCharge;
        public string DamageTypeWhenConsumed;
        public int DamageWhenConsumed;
        public float DamageWhenConsumedPerCharge;
        public string Description;
        public bool DieWhenConsumedAll;
        public string[] DisabledCardTypes; // I assume for silence/disarm?
        public int DoubleDamageIfCursesLessThan;
        public string EffectTick;
        public string EffectTickSides;
        public int ExplodeAtStacks;
        public string GainAuraCurseConsumption;
        public string GainAuraCurseConsumption2;
        public int GainAuraCurseConsumptionPerCharge;
        public int GainAuraCurseConsumptionPerCharge2;
        public bool GainCharges;
        public string GainChargesFromThisAuraCurse;
        public string GainChargesFromThisAuraCurse2;
        public int HealAttackerConsumeCharges;
        public int HealAttackerPerStack;
        public int HealDonePercent;
        public int HealDonePercentPerStack;
        public int HealDonePercentPerStackPerEnergy;
        public int HealDonePerStack;
        public int HealReceivedTotal;
        public int HealSidesWhenConsumed;
        public float HealSidesWhenConsumedPerCharge;
        public int HealWhenConsumed;
        public float HealWhenConsumedPerCharge;
        public bool IconShow;
        public string ID;
        public string IncreasedDamageReceivedType;
        public string IncreasedDamageReceivedType2;
        public int IncreasedDirectDamageChargesMultiplierNeededForOne;
        public int IncreasedDirectDamageChargesMultiplierNeededForOne2;
        public int IncreasedDirectDamageReceivedPerStack;
        public int IncreasedDirectDamageReceivedPerStack2;
        public int IncreasedDirectDamageReceivedPerTurn;
        public int IncreasedDirectDamageReceivedPerTurn2;
        public int IncreasedPercentDamageReceivedPerStack;
        public int IncreasedPercentDamageReceivedPerStack2;
        public int IncreasedPercentDamageReceivedPerTurn;
        public int IncreasedPercentDamageReceivedPerTurn2;
        public bool Invulnerable;
        public bool IsAura;
        public int MaxCharges;
        public int MaxMadnessCharges;
        public int ModifyCardCostPerChargeNeededForOne;
        public bool NoRemoveBlockAtTurnEnd;
        public bool Preventable;
        public string PreventedAuraCurse;
        public int PreventedAuraCurseStackPerStack;
        public int PreventedDamagePerStack;
        public string PreventedDamageTypePerStack;
        public int PriorityOnConsumption;
        public bool ProduceDamageWhenConsumed;
        public bool ProduceHealWhenConsumed;
        public bool Removable;
        public string RemoveAuraCurse;
        public string RemoveAuraCurse2;
        public string ResistModified;
        public string ResistModified2;
        public string ResistModified3;
        public float ResistModifiedPercentagePerStack;
        public float ResistModifiedPercentagePerStack2;
        public float ResistModifiedPercentagePerStack3;
        public int ResistModifiedValue;
        public int ResistModifiedValue2;
        public int ResistModifiedValue3;
        public int RevealCardsPerCharge;
        public bool SkipsNextTurn;
        public string Sound; // noting that Weak has null
        public string Sprite; // "weak" (UnityEngine.Sprite)
        public bool Stealth;
        public bool Taunt;
    }

    [Serializable]
    public class NPCDataText  : DataText
    {
        public string[] AICards; // AICards[]
        public string[] AuraCurseImmune;
        public string BaseMonster; // NPCData
        public bool BigModel;
        public int CardsInHand;
        public string Description;
        public int Difficulty;
        public int Energy;
        public int EnergyTurn;
        public int ExperienceReward;
        public bool Female;
        public bool FinishCombatOnDead;
        public float FluffOffsetX; // ??
        public float FluffOffsetY; // ??
        public string GameObjectAnimated; // UnityEngine.GameObject
        public int GoldReward;
        public string HellModeMob; // NPCData
        public string HitSound; // UnityEngine.Sprite
        public int HP;
        public string ID;
        public bool IsBoss;
        public bool IsNamed;
        public string NgPlusMob; // NPCData
        public string NPCName; // "nice" name
        public float PosBottom;
        public string PreferredPosition; //CardTargetPosition
        public int ResistBlunt;
        public int ResistCold;
        public int ResistFire;
        public int ResistHoly;
        public int ResistLightning;
        public int ResistMind;
        public int ResistPiercing;
        public int ResistShadow;
        public int ResistSlashing;
        public string ScriptableObjectName; // actual string
        public int Speed;
        public string Sprite; // UnityEngine.Sprite
        public string SpritePortrait; // UnityEngine.Sprite;
        public string SpriteSpeed; // UnityEngine.Sprite;
        public string TierMob; // CombatTier
        public string TierReward; // TierRewardData
        public string UpgradedMob; // NPCData
    }

    [Serializable]
    public class AICardsText : DataText
    {
        public int AddCardRound;
        public string AuraCurseCastIf; // AuraCurseData
        public string Card; // CardData
        public string OnlyCastIf; // OnlyCastIf
        public float PercentToCast;
        public int Priority;
        public string TargetCast; // TargetCast
        public int UnitsInDeck;
        public float ValueCastIf;
    }

    [Serializable]
    public class NodeDataText : DataText
    {
        public int CombatPercent;
        public string Description;
        public bool DisableCorruption;
        public bool DisableRandom;
        public int EventPercent;
        public int ExistsPercent;
        public string ExistsSku; // DLC?
        public bool GoToTown;
        public string NodeBackgroundImg; // UnityEngine.Sprite, can be null
        public string[] NodeCombat; // CombatData
        public string NodeCombatTier; // CombatTier
        public string[] NodeEvent; // EventData
        public int[] NodeEventPercent; // corresponding to the NodeEvents above? Why didn't they do AICards like this? because it's more complicated?
        public int[] NodeEventPriority; // likewise
        public string NodeEventTier; // CombatTier
        public string NodeGround; // CombatTier
        public string NodeId;
        public string NodeName; // "nice" name
        public string NodeRequirement; // EventRequirementData
        public string[] NodesConnected; // NodeData
        public string[] NodesConnectedRequirement; // NodesConnectedRequirement
        public string NodeZone; // ZoneData
        public bool TravelDestination;
        public bool VisibleIfNotRequirement;
    }

    [Serializable]
    public class LootDataText : DataText
    {
        public float DefaultPercentEpic;
        public float DefaultPercentMythic;
        public float DefaultPercentRare;
        public float DefaultPercentUncommon;
        public int GoldQuantity;
        public string ID;
        public string[] LootItemTable; // LootItem
        public int NumItems;
    }

    [Serializable]
    public class LootItemText : DataText
    {
        public string LootCard; // CardData
        public float LootPercent;
        public string LootRarity; // CardRarity
        public string LootType; // CardType
    }

    [Serializable]
    public class PerkNodeDataText : DataText
    {
        public int Column;
        public string Cost; // PerkCost
        public string ID;
        public bool LockedInTown;
        public bool NotStack;
        public string Perk; // PerkData
        public string PerkRequired; // PerkNodeData
        public string[] PerksConnected; // PerkNodeData; none for any of the Crack ones, though? maybe only used when they're actually _connected_ actively?
        public int Row;
        public string Sprite; // UnityEngine.Sprite
        public int Type; // Crack: 1; Sanctify: 3. 0/1/2/3 general/physical/elemental/mystical, or w/e?
    }

    [Serializable]
    public class ChallengeDataText : DataText
    {
        public string Boss1; // NPCData
        public string Boss2; // NPCData
        public string BossCombat; // CombatData
        public string[] CorruptionList; // CardData, a la DeathGrip, Hexproof, ThornProliferation
        public string Hero1; // SubClassData
        public string Hero2; // SubClassData
        public string Hero3; // SubClassData
        public string Hero4; // SubClassData
        public string ID;
        public string IDSteam;
        public string Loot; // LootData
        public string Seed;
        public string[] Traits; // ChallengeTrait
        public int Week;
    }

    [Serializable]
    public class ChallengeTraitText : DataText
    {
        public string Icon; // UnityEngine.Sprite
        public string ID;
        public bool IsMadnessTrait;
        public string Name; // "nice" name
        public int Order;
    }

    /* TierRewardData is already appropriately serializable!
    [Serializable]
    public class TierRewardDataText : DataText
    {
        public int Common;
        public int Uncommon;
        public int Rare;
        public int Epic;
        public int Mythic;
        public int Dust;
        public int TierNum;
    }*/

    [Serializable]
    public class CombatDataText : DataText
    {
        public string CinematicData; // CinematicData
        public string CombatBackground; // CombatBackground
        public string[] CombatEffect; // CombatEffect .AuraCurse, .AuraCurseCharges, .AuraCurseTarget (CombatUnit)
        public string CombatID;
        public string CombatMusic; // "16_Nowhere Land - Alexander Nakarada" (UnityEngine.AudioClip)
        public string CombatTier; // CombatTier
        public string Description; // in... Spanish, I think?
        public string EventData; // EventData
        public string EventRequirementData; // EventRequirementData
        public bool HealHeroes;
        public string[] NPCList; // NPCData
        public int NPCRemoveInMadness0Index;
        public string ThermometerTierData; // ThermometerTierData
    }

    [Serializable]
    public class CombatEffectText : DataText
    {
        public string AuraCurse;
        public int AuraCurseCharges;
        public string AuraCurseTarget;
    }

    [Serializable]
    public class EventDataText : DataText
    {
        public string Description;
        public string DescriptionAction;
        public string EventIconShader; // MapIconShader
        public string EventID;
        public string EventName; // "nice" name
        public string EventSpriteBook; // UnityEngine.Sprite
        public string EventSpriteDecor; // UnityEngine.Sprite
        public string EventSpriteMap; // UnityEngine.Sprite
        public string EventTier; // CombatTier
        public string EventUniqueID; // e_sen38_b was blank :)
        public bool HistoryMode;
        public int ReplyRandom;
        public string[] Replies; // EventReplyData
        public string RequiredClass; // SubClassData
        public string Requirement; // EventRequirementData
    }

    [Serializable]
    public class EventRequirementDataText : DataText
    {
        public bool AssignToPlayerAtBegin;
        public string Description;
        public string ItemSprite; // UnityEngine.Sprite
        public string RequirementID;
        public string RequirementName;
        public bool RequirementTrack;
        public string TrackSprite; // UnityEngine.Sprite
    }

    [Serializable]
    public class EventReplyDataText : DataText
    {
        public int DustCost;
        public string FLAddCard1; // CardData
        public string FLAddCard2; // CardData
        public string FLAddCard3; // CardData
        public string FLAddItem; // CardData
        public string FLCAddCard1; // CardData
        public string FLCAddCard2; // CardData
        public string FLCAddCard3; // CardData
        public string FLCAddItem; // CardData
        public bool FLCardPlayerGame;
        public string FLCardPlayerGamePackData; // CardPlayerPackData
        public bool FLCCardPlayerGame;
        public string FLCCardPlayerGamePackData; // CardPlayerPackData
        // UNFINISHED! #TODO

    }

    [Serializable]
    public class ZoneDataText : DataText
    {
        public bool ChangeTeamOnEntrance;
        public bool DisableExperienceOnThisZone;
        public bool DisableMadnessOnThisZone;
        public string[] NewTeam; // SubClassData
        public bool ObeliskFinal;
        public bool ObeliskHigh;
        public bool ObeliskLow;
        public bool RestoreTeamOnExit;
        public string ZoneID;
        public string ZoneName;
    }

    [Serializable]
    public class KeyNotesDataText : DataText
    {
        public string Description;
        public string DescriptionExtended;
        public string ID;
        public string KeynoteName;
    }

    [Serializable]
    public class PackDataText : DataText
    {
        public string Card0; // CardData
        public string Card1; // CardData
        public string Card2; // CardData
        public string Card3; // CardData
        public string Card4; // CardData
        public string Card5; // CardData
        public string CardSpecial0; // CardData
        public string CardSpecial1; // CardData
        public string PackClass; // CardClass
        public string PackID;
        public string PackName; // "nice" name
        public string[] PerkList; // PerkData
        public string RequiredClass; // SubClassData
    }

    [Serializable]
    public class ItemDataText : DataText
    {

    }

    [Serializable]
    public class CorruptionPackDataText : DataText
    {
        public string[] HighPack; // CardData
        public string[] LowPack; // CardData
        public string PackClass; // Enums.CardClass
        public string PackName;
        public int PackTier;
    }

    [Serializable]
    public class CardPlayerPackDataText : DataText
    {
        public string Card0; // CardData
        public bool Card0RandomBoon;
        public bool Card0RandomInjury;
        public string Card1; // CardData
        public bool Card1RandomBoon;
        public bool Card1RandomInjury;
        public string Card2; // CardData
        public bool Card2RandomBoon;
        public bool Card2RandomInjury;
        public string Card3; // CardData
        public bool Card3RandomBoon;
        public bool Card3RandomInjury;
        public int ModIterations;
        public int ModSpeed;
        public string PackId;
    }

    [Serializable]
    public class NodesConnectedRequirementText : DataText
    {
        public string NodeData;
        public string ConnectionRequirement;
        public string ConnectionIfNotNode;
    }

}

# Obeliskial Options

(updated for 1.1.02)

This is an **Across the Obelisk** mod that contains a number of options to alter gameplay.

Broadly speaking, this mod makes the game easier; I encourage you to try harder madness levels, corruption modifiers and personal challenges to re-balance the game.

Should be multiplayer-friendly. The host’s settings will override other players’ for options in the below tables with MP Override = true.

## Debug

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **All Key Items**                | false   | true        | Give all key items in Adventure Mode. Items are added when you load into town.                                                                                                 |
| **Gold ++**                      | false   | true        | Many cash.                                                                                                                                                                     |
| **Gems ++**                      | false   | true        | Many cryttals.                                                                                                                                                                 |
| **Supplies ++**                  | false   | true        | Many supplies.                                                                                                                                                                 |
| **Developer Mode**               | false   | true        | (IN TESTING) Turns on AtO devs’ developer mode. Back up your save!                                                                                                             |
| **Export Settings**              | n.a.    | n.a.        | (IN TESTING) Copy this string to export your settings!                                                                                                                         |
| **Import Settings**              | n.a.    | n.a.        | (IN TESTING) Paste a string in here to import settings!                                                                                                                        |

## Cards & Decks

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Ignore Minimum Deck Size**     | true    | true        | (IN TESTING) Allow you to remove cards even when deck contains less than 15.                                                                                                   |
| **Card Removal**                 | true    | true        | (IN TESTING) Allow removal of any cards (vanilla), only curses, no curses, or no cards at all.                                                                                                   |
| **Craft Corrupted Cards**        | false   | true        | Allow crafting of corrupted cards. Also allows crafting of higher rarity cards in any town (which I intend to separate out, but haven't yet :D)                                |
| **Craft Infinite Cards**         | false   | true        | Infinite card crafts (set available card count to 99).                                                                                                                         |

## Loot

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Corrupted Card Rewards**       | false   | true        | Card rewards are always corrupted.                                                                                                                                             |
| **Corrupted Loot Rewards**       | false   | true        | Make item loot rewards always corrupted.                                                                                                                                       |

## Corruption & Madness

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **High Madness - Sell Supplies** | true    | true        | Allows you to sell supplies on high madness.                                                                                                                                   |
| **High Madness - Shop Rerolls**  | true    | true        | Allows you to reroll the shop more than once on high madness (despite what the relevant text says...).                                                                         |
| **High Madness - Use Claims**    | true    | true        | (IN TESTING) Use claims on any madness.                                                                                                                                        |

## Events & Nodes

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Travel Anywhere**              | false   | true        | (IN TESTING) Travel to any node.                                                                                                                                               |
| **No Travel Requirements**       | false   | true        | (IN TESTING) Can travel to nodes that are normally invisible.                                                                                                                  |
| **Always Fail Event Rolls**      | false   | true        | Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible.                                                        |
| **Always Succeed Event Rolls**   | false   | true        | Always succeed event rolls, though event text might not match. Critically succeeds if possible.                                                                                |

## Perks

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Many Perk Points**             | false   | true        | (MILDLY BUGGY) Set maximum perk points to 1000.                                                                                                                                |
| **Modify Perks Whenever**        | false   | true        | (IN TESTING) Change perks whenever you want.                                                                                                                                   |
| **No Perk Requirements**         | false   | true        | (IN TESTING) Can select perk without selecting its precursor perks.                                                                                                            |

## Shop

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Adjusted Shop Rarity**         | false   | true        | Modifies shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt.                                         |
| **Bad Luck Protection**          | 100     | true        | Increases rarity of shop items based on number of shops seen without acquiring new items (scales with town tier). Value*ShopsSeen*ActNumber/100000=increased % mythic items    |
| **Corrupted Map Shops**          | true    | true        | Allow shops on the map (e.g. werewolf shop in Senenthia) to have corrupted goods for sale.                                                                                     |
| **Corrupted Obelisk Shops**      | true    | true        | Allow obelisk corruption shops to have corrupted goods for sale.                                                                                                               |
| **Corrupted Town Shops**         | true    | true        | Allow town shops to have corrupted goods for sale.                                                                                                                             |
| **Discount Divination**          | true    | true        | Discounts are applied to divinations.                                                                                                                                          |
| **Discount Doomroll**            | true    | true        | Discounts are applied to shop rerolls.                                                                                                                                         |
| **Plentiful Pet Purchases**      | true    | true        | Buy more than one of each pet.                                                                                                                                                 |
| **Individual Player Shops**      | true    | true        | Does not send shop purchase records in multiplayer. Does not include pets!                                                                                                     |
| **Post-Scarcity Shops**          | true    | true        | Does not record who purchased what in the shop. Does not include pets!                                                                                                         |

## Should Be Vanilla

| Option                           | Default | MP Override | Description                                                                                                                                                                    |
|:---------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Allow Profanities**            | true    | false       | Ignores the profanity filter.                                                                                                                                                  |
| **Emotional**                    | true    | false       | Use more emotes during combat (i.e., all party members rather than just those you control, with no cooldown between emote use. Pro tip: use hotkeys (e.g. R) to spam faster!!) |
| **Force Select Server**          | false   | false       | Force server selection to location of your choice. The game is kind of inconsistent about remembering this and it irritates my friends.                                        |
| **Server To Force**              | au      | false       | Which server should be forced if the above option is true?                                                                                                                     |
| **Max Multiplayer Members**      | true    | false       | Default to 4 players in multiplayer.                                                                                                                                           |
| **Overly Tenergetic**            | true    | true        | Allow characters to have more than 10 energy.                                                                                                                                  |
| **Bugfix: Equipment HP**         | true    | true        | (IN TESTING) Fixes a vanilla bug that allows infinite stacking of HP by buying the same item repeatedly.                                                                       |
| **Skip Cinematics**              | true    | false       | (IN TESTING) Automatically skip cinematics.                                                                                                                                    |
| **Auto Continue**                | true    | false       | (IN TESTING) Automatically 'continue' events.                                                                                                                                  |

## Installation

Install [BepInEx](https://across-the-obelisk.thunderstore.io/package/BepInEx/BepInExPack_AcrossTheObelisk/).

Download the latest [release](https://github.com/stiffmeds/Obeliskial-Options/releases) and put it in your BepInEx _plugins_ folder.

Change settings in BepInEx\config\com.meds.obeliskialoptions.cfg (appears after first run) OR use a configuration manager (I use [this one - you want the Mono version](https://github.com/sinai-dev/BepInExConfigManager)) to set them ingame. If you use a config manager you need to change BepInEx\config\BepInEx.cfg: HideManagerGameObject = true.

## Support

I guess open a github issue? :)

...or feel free to ask in the #modding channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701); I am very active there.
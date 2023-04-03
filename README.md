# Obeliskial Options

(updated for 1.1.02)

This is an **Across the Obelisk** mod that contains a number of options to alter gameplay.

Broadly speaking, this mod makes the game easier; I encourage you to try harder madness levels, corruption modifiers and personal challenges to re-balance the game.

MULTIPLAYER: Ensure that all players have the same settings or things are likely to break.

## Regular Options

| Option                           | Default | Description                                                                                                                                                                   |
|:---------------------------------|:-------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Adjusted Shop Rarity**         | true    | Modifies shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt.                                        |
| **Allow Profanities**            | true    | Ignores the profanity filter.                                                                                                                                                 |
| **Corrupted Card Rewards**       | false   | Card rewards are always corrupted.                                                                                                                                            |
| **Corrupted Item Rewards**       | false   | Make item rewards always corrupted.                                                                                                                                           |
| **Corrupted Items in Town Shop** | true    | Allow town shops to have corrupted goods.                                                                                                                                     |
| **Discount Divination**          | true    | Discounts are applied to divinations.                                                                                                                                         |
| **Discount Doomroll**            | true    | Discounts are applied to shop rerolls.                                                                                                                                        |
| **Emotional**                    | true    | Use more emotes during combat (i.e., all party members rather than just those you control, with no cooldown between emote use. Pro tip: use hotkeys (e.g. R) to spam faster!!)|
| **High Madness - Sell Supplies** | true    | Allows you to sell supplies on high madness.                                                                                                                                  |
| **High Madness - Shop Rerolls**  | true    | Allows you to reroll the shop more than once on high madness (despite what the relevant text says...).                                                                        |
| **High Madness - Use Claims**    | true    | (IN TESTING) Use claims on any madness.                                                                                                                                       |
| **Individual Player Shops**      | true    | Does not send shop purchase records in multiplayer. Does not include pets!                                                                                                    |
| **Max Multiplayer Members**      | true    | Default to 4 players in multiplayer.                                                                                                                                          |
| **Plentiful Pet Purchases**      | true    | Buy more than one of each pet.                                                                                                                                                |
| **Post-Scarcity Shops**          | true    | Does not record who purchased what in the shop. Does not include pets!                                                                                                        |
| **Strayan**                      | false   | Default server selection to Australia. The game is kind of inconsistent about remembering this and it irritates my friends.                                                   |


## Debug Options

These options are for debugging and testing purposes. Most users will not care about them.

| Option                            | Default | Description                                                                                                                                     |
|:----------------------------------|:-------:|:------------------------------------------------------------------------------------------------------------------------------------------------|
| **All Key Items**                 | false   | Give all key items in Adventure Mode. Items are added when you load into town.                                                                  |
| **Always Fail Event Rolls**       | false   | Always fail event rolls (unless Always Succeed is on), though event text might not match. Critically fails if possible.                         |
| **Always Succeed Event Rolls**    | false   | Always succeed event rolls, though event text might not match. Critically succeeds if possible.                                                 |
| **Become Rich**                   | false   | Many cash, cryttals, supplies.                                                                                                                  |
| **Craft Corrupted Cards**         | false   | Allow crafting of corrupted cards. Also allows crafting of higher rarity cards in any town (which I intend to separate out, but haven't yet :D) |
| **Craft Infinite Cards**          | false   | Infinite card crafts (set available card count to 99).                                                                                          |
| **Developer Mode**                | false   | (IN TESTING) Turns on AtO devs’ developer mode. Back up your save!                                                                              |
| **Many Perk Points**              | false   | (MILDLY BUGGY) Set maximum perk points to 1000.                                                                                                 |
| **Modify Perks Whenever**         | false   | (IN TESTING) Change perks whenever you want.                                                                                                    |
| **Travel Anywhere**               | false   | (IN TESTING) Travel to any node.                                                                                                                |
| **No Perk Requirements**          | false   | (IN TESTING) Can select perk without selecting its precursor perks.                                                                             |
| **No Travel Requirements**        | false   | (IN TESTING) Can travel to nodes that are normally invisible.                                                                                   |

## Installation

Install [BepInEx](https://across-the-obelisk.thunderstore.io/package/BepInEx/BepInExPack_AcrossTheObelisk/).
Download the latest [release](https://github.com/stiffmeds/Obeliskial-Options/releases) and put it in your BepInEx _plugins_ folder.
Change settings in the BepInEx\config\com.meds.obeliskialoptions.cfg (appears after first run) OR use a configuration manager (I use [this one](https://github.com/sinai-dev/BepInExConfigManager)) to set them ingame. If you use a config manager you need to change BepInEx\config\BepInEx.cfg to have HideManagerGameObject = true rather than HideManagerGameObject = false.

## Support

I guess open a github issue? :)

...or feel free to ping me in the #modding channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701): Stiff Meds#9105
# Obeliskial Options

This is an **Across the Obelisk** mod that contains a number of options to alter gameplay.

Broadly speaking, this mod makes the game easier; I encourage you to try harder madness levels, corruption modifiers and personal challenges to re-balance the game.

Should be multiplayer-friendly, though all players must have the mod. The host’s settings will override other players’ for options in the below tables with MP Override = true.

## Custom Content

[Custom Card/Item Creator](https://code.secretsisters.gay/AtO_Custom): Ensure that 'Custom Content' is enabled in the Debug section of Obeliskial Options settings and extract your downloaded custom card/item to *Across the Obelisk\BepInEx\config*.

<details>
	<summary>Default Custom Content</summary>

![Dark Princess Chalice (formerly Chalice of Queens)](https://i.imgur.com/OHMJNHJ.png) ![Dark Princess Chalice Rare](https://i.imgur.com/9RdQ3BA.png)

![Divine Presence](https://i.imgur.com/oFfwbYQ.png) ![Divine Presence Rare](https://i.imgur.com/GIQOLO0.png)

![Glass Ring](https://i.imgur.com/ICqGlc6.png) ![Glass Ring Rare](https://i.imgur.com/4F3r32C.png)

![Golden Apple](https://i.imgur.com/aQxeruX.png) ![Golden Apple Rare](https://i.imgur.com/hiqhTZv.png)

![Hellfire Earrings](https://i.imgur.com/bcaSajU.png) ![Hellfire Earrings Rare](https://i.imgur.com/EqlyxjT.png)

![Lucky Dice](https://i.imgur.com/wj0W8zP.png) ![Lucky Dice Rare](https://i.imgur.com/eEhKCon.png)

![Nature's Blessing](https://i.imgur.com/Yf0FMRB.png) ![Nature's Blessing Rare](https://i.imgur.com/UOt2Eyi.png)

![Righteous Rod](https://i.imgur.com/DVUcayD.png) ![Righteous Rod Rare](https://i.imgur.com/lJUchM6.png)

![Ring of Stone](https://i.imgur.com/mk8yoWc.png) ![Ring of Stone Rare](https://i.imgur.com/UKfDZN1.png)

![Rod of Blasting](https://i.imgur.com/abnCBAp.png) ![Rod of Blasting Rare](https://i.imgur.com/3ZcEO2g.png)

![Runic Shackles](https://i.imgur.com/key0DGB.png) ![Runic Shackles Rare](https://i.imgur.com/4QqIMMq.png)

![Sanguine Scroll](https://i.imgur.com/s3f3O2t.png) ![Sanguine Scroll Rare](https://i.imgur.com/Vm9mHKb.png)

![Spicy Pasta](https://i.imgur.com/2RH7y3V.png) ![Extra Spicy Pasta](https://i.imgur.com/8afOBDw.png)

![Void Stone](https://i.imgur.com/6BgkBoL.png) ![Void Stone Rare](https://i.imgur.com/aB6bjjD.png)

![Wand of Warding](https://i.imgur.com/aJ87aM1.png) ![Wand of Warding Rare](https://i.imgur.com/ugusIXS.png)

![Zealot's Cross](https://i.imgur.com/eWlzn18.png) ![Zealot's Cross Rare](https://i.imgur.com/Sottxnj.png)

Charls

Hanshek

</details>



## Debug

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **All Key Items**                   | false   | true        | Give all key items in Adventure Mode. Items are added when you load into town.                                                                                                 |
| **Gold ++**                         | false   | true        | Many cash.                                                                                                                                                                     |
| **Gems ++**                         | false   | true        | Many cryttals.                                                                                                                                                                 |
| **Supplies ++**                     | false   | false       | Many supplies.                                                                                                                                                                 |
| **Developer Mode**                  | false   | true        | Turns on AtO devs’ developer mode. Back up your save!                                                                                                                          |
| **Export Settings**                 | n.a.    | n.a.        | Copy this string to export your settings!                                                                                                                                      |
| **Import Settings**                 | n.a.    | n.a.        | Paste a string in here to import settings!                                                                                                                                     |
| **Enable Custom Content**           | true    | false       | Loads custom cards/items/sprites/events/etc.                                                                                                                                 |
| **Export Vanilla Content**          | false   | false       | Export vanilla data to Custom Content-compatible JSON files.                                                                                                                   |
| **Export Sprites**                  | true    | false       | Export sprites when exporting vanilla content.                                                                                                                                 |

## Cards & Decks

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Minimum Deck Size**               | 1       | true        | Set the minimum deck size.                                                                                                                                                     |
| **Card Removal**                    | any     | true        | Allow removal of any cards (vanilla), only curses, no curses, or no cards at all.                                                                                              |
| **Craft Corrupted Cards**           | false   | true        | Allow crafting of corrupted cards. Also allows crafting of higher rarity cards in any town (which I intend to separate out, but haven't yet :D)                                |
| **Craft Infinite Cards**            | false   | true        | Infinite card crafts (set available card count to 99).                                                                                                                         |

## Characters

| Option                              | Default     | MP Override | Description                                                                                                                                                                |
|:------------------------------------|:-----------:|:-----------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Enable Clones**                   | true        | true        | Adds three clone characters to the DLC section of Hero Selection.                                                                                                          |
| **Clone 1**                         | loremaster  | true        | Which subclass should be cloned into DLC slot 4?                                                                                                                           |
| **Clone 1 Name**                    | Clone       | false       | What should the character in DLC slot 4 be called?                                                                                                                         |
| **Clone 2**                         | loremaster  | true        | Which subclass should be cloned into DLC slot 5?                                                                                                                           |
| **Clone 2 Name**                    | Copy        | false       | What should the character in DLC slot 5 be called?                                                                                                                         |
| **Clone 3**                         | loremaster  | true        | Which subclass should be cloned into DLC slot 6?                                                                                                                           |
| **Clone 3 Name**                    | Counterfeit | false       | What should the character in DLC slot 6 be called?                                                                                                                         |
| **Level Past 50**                   | true        | false       | Allows characters to be raised up to rank 500.                                                                                                                             |

Subclasses as at Sands of Ulminin release: mercenary, sentinel, berserker, warden, ranger, assassin, archer, minstrel, elementalist, pyromancer, loremaster, warlock, cleric, priest, voodoowitch, prophet, bandit, paladin, fallen.
If you’re already in the hero selection screen, you need to leave and re-enter it to update the clones. Clones do not count for events (i.e., do not get dialogue options). The perk selection screen will sometimes show the wrong skin in the top right, but appears to be functional.

## Corruption & Madness

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **High Madness - Sell Supplies**    | true    | true        | Allows you to sell supplies on high madness.                                                                                                                                   |
| **High Madness - Shop Rerolls**     | true    | true        | Allows you to reroll the shop more than once on high madness (despite what the relevant text says...).                                                                         |
| **High Madness - Use Claims**       | true    | true        | Use claims on any madness.                                                                                                                                                     |

## Events & Nodes

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Travel Anywhere**                 | false   | true        | Travel to any node.                                                                                                                                                            |
| **No Class Requirements**           | false   | true        | (IN TESTING) Events and replies ignore class requirements (note: does not include events requiring the character to roll a specific card e.g. healers at campsites).           |
| **No Equipment Requirements**       | false   | true        | (IN TESTING) Events and replies ignore equipment/pet requirements.                                                                                                             |
| **No Key Item Requirements**        | false   | true        | (IN TESTING) Events and replies ignore key item / quest requirements.                                                                                                          |
| **Visit All Zones**                 | false   | true        | You can choose any location to visit from the obelisk (e.g. can go to the Void early, can visit all locations before going, etc.).                                             |

## Loot

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Corrupted Card Rewards**          | false   | true        | Card rewards are always corrupted.                                                                                                                                             |
| **Corrupted Loot Rewards**          | false   | true        | Make item loot rewards always corrupted.                                                                                                                                       |

## Perks

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Many Perk Points**                | false   | true        | (VISUALLY BUGGY) Set maximum perk points to 1000.                                                                                                                              |
| **Modify Perks Whenever**           | false   | true        | (IN TESTING) Change perks whenever you want.                                                                                                                                   |
| **No Perk Requirements**            | false   | true        | Can select perk without selecting its precursor perks.                                                                                                                         |

## Shop

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Adjusted Shop Rarity**            | false   | true        | Modifies shop rarity based on current madness/corruption. This also makes the change in rarity from act 1 to 4 _slightly_ less abrupt.                                         |
| **Bad Luck Protection**             | 10      | true        | Increases rarity of shop items based on number of shops seen without acquiring new items.                                                                                      |
| **Corrupted Map Shops**             | true    | true        | Allow shops on the map (e.g. werewolf shop in Senenthia) to have corrupted goods for sale.                                                                                     |
| **Corrupted Obelisk Shops**         | true    | true        | Allow obelisk corruption shops to have corrupted goods for sale.                                                                                                               |
| **Corrupted Town Shops**            | true    | true        | Allow town shops to have corrupted goods for sale.                                                                                                                             |
| **Discount Divination**             | true    | true        | Discounts are applied to divinations.                                                                                                                                          |
| **Discount Doomroll**               | true    | true        | Discounts are applied to shop rerolls.                                                                                                                                         |
| **Plentiful Pet Purchases**         | true    | true        | Buy more than one of each pet.                                                                                                                                                 |
| **Individual Player Shops**         | true    | true        | Does not send shop purchase records in multiplayer. Does not include pets!                                                                                                     |
| **Post-Scarcity Shops**             | true    | true        | Does not record who purchased what in the shop. Does not include pets!                                                                                                         |
| **Drop-Only Items Appear In Shops** | true    | true        | Items that would normally not appear in shops, such as the Yggdrasil Root or Yogger's Cleaver, will appear.                                                                    |

## Should Be Vanilla

| Option                              | Default | MP Override | Description                                                                                                                                                                    |
|:------------------------------------|:-------:|:-----------:|:-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Allow Profanities**               | true    | false       | Ignores the profanity filter.                                                                                                                                                  |
| **Emotional**                       | true    | false       | Use more emotes during combat (i.e., all heroes rather than just those you control, with no cooldown between emote use. Pro tip: use hotkeys (e.g. R) to spam faster!!)        |
| **Force Select Server**             | false   | false       | Force server selection to location of your choice. The game is kind of inconsistent about remembering this and it irritates my friends.                                        |
| **Server To Force**                 | au      | false       | Which server should be forced if the above option is true?                                                                                                                     |
| **Max Multiplayer Members**         | true    | false       | Default to 4 players in multiplayer.                                                                                                                                           |
| **Overly Tenergetic**               | true    | true        | (VISUALLY BUGGY) Allow characters to have more than 10 energy.                                                                                                                 |
| **Bugfix: Equipment HP**            | true    | true        | (VISUALLY BUGGY) Fixes a vanilla bug that allows infinite stacking of HP by buying the same item repeatedly.                                                                   |
| **Skip Cinematics**                 | false   | false       | Automatically skip cinematics.                                                                                                                                                 |
| **Auto Continue**                   | false   | false       | (VISUALLY BUGGY?) Automatically 'continue' events.                                                                                                                             |
| **Auto Create Room on MP Load**     | true    | false       | Use previous settings to automatically create lobby room when loading multiplayer game.                                                                                        |
| **Auto Ready on MP Load**           | true    | false       | Automatically readies up non-host players when loading multiplayer game.                                                                                                       |
| **Spacebar to Continue**            | true    | false       | Spacebar clicks the 'Continue' button in events for you.                                                                                                                       |
| **Conflict Resolution**             | 4       | true        | Automatically select (1) lowest card; (2) closest to 2; (3) highest card; or (4) random to determine multiplayer conflicts.                                                    |
| **All The Pets**                    | true    | true        | (IN TESTING) Shows blob pets and Harley in the Tome of Knowledge and shop.                                                                                                     |

## Installation

Install [BepInEx](https://across-the-obelisk.thunderstore.io/package/BepInEx/BepInExPack_AcrossTheObelisk/).

Download the latest [release](https://github.com/stiffmeds/Obeliskial-Options/releases) and put it in your BepInEx _plugins_ folder.

Change settings in BepInEx\config\com.meds.obeliskialoptions.cfg (appears after first run) OR use a configuration manager (I use [this one - you want the Mono version](https://github.com/sinai-dev/BepInExConfigManager)) to set them ingame. If you use a config manager you need to change BepInEx\config\BepInEx.cfg: HideManagerGameObject = true.

## Support

I guess open a github issue? :)

...or feel free to ask in the **#modding** channel of the [official Across the Obelisk Discord](https://discord.gg/across-the-obelisk-679706811108163701); I am very active there.

## Donations

I make mods because I love it, not because I want to make money from it. If you really, really want to donate to me, my preferred non-profit organisations are [Greyhound Adoptions WA](https://greyhoundadoptionswa.com.au/donation/) ([paypal](https://www.paypal.com/donate?token=m8DwEGGEH0FFsS6PS-5p4MX9_5g8_ocMMrNFjaELN-xcG6Ok-KCFabu5xtB-57QBiOM7QLSuKVUepvL_)) or [Headache Australia](https://headacheaustralia.org.au/donate/).
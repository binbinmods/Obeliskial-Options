# 2.0.7

Fixed a _Drop-Only Items Appear In Shops_ desync if some players owned Sands of Ulminin items and others didn't (thanks @Falo Rowi!)

# 2.0.6

Tentatively fixed a bug with _Drop-Only Items Appear In Shops_ resulting from a change to how DLCs are checked.

# 2.0.5

Fixed a bug where _Emotional_ would break in Sandbox Mode with less than 4 heroes.

# 2.0.4

Fixed a bug where Corrupt Card Rewards would cause an infinite loop while loading into Obelisk Challenge card selection (thanks @Sara!)

# 2.0.3

Fix for if Level Past 50 is disabled after characters have passed normal max XP.
Fix for _Emotional_ breaking Wolf Wars.
Fix for Developer Mode remaining enabled.

# 2.0.2

Fix desync in loot rewards.
Fix not being able to select cards to vanish/discard when it's not your turn.

# 2.0.1

Visit All Zones fix (thanks @Randomality for noticing the issue!).
Developer Mode fix.
Bad Luck Protection should no longer give mythic rarity items in Act 1.

# 2.0.0

Now requires Obeliskial Essentials as a dependency.
Split custom content into Obeliskial Content.
_Visit All Zones_ now allows you to go through as many portals as you want.
Removed _Always Fail_/_Succeed Event Rolls_, which have been incorporated into Sandbox Mode.
Clones now get their own event replies.
Fixed Overly Tenergetic energy display.
Various minor bugfixes.

# 1.5.3

Fix to allow All The Pets and Drop-Only Items Appear In Shops to work without custom content enabled. Pets aren't in alphabetical order in the shop when added this way, but that's a problem for future me :)

# 1.5.2

Made clones compatible with the SkinExtender mod.
Updated DoNotDropList with SoU items that should not appear in shops.
Further fix for custom content importing (cards+items should work fine; site still needs an update).

# 1.5.1

Re-enabled the other two clone slots.
Added WilburCardJSONExport method for exporting JSON files for the Wilbur Discord bot (but you have to call it yourself :D).
Partial fix for custom content importing (cards+items still need a review; site still needs an update).

# 1.5.0

Initial update for Sands of Ulminin. Note only one clone slot working until I add more.

# 1.4.2

I WAS BORN STUPID BUT I'LL LIVE FOREVER.

(removed duplicate/old dll)

# 1.4.1

## Other Changes

Fixed Enchantments (thanks to @Dead in the AtO Discord for identifying that they were broken!)

Fixed an issue where Bree clones wouldn't be able to activate Queen of Thorns.

Added FullCardSpriteOutput() method, which creates images of every card. Beware that this is a RAM hog!

Some behind-the-scenes prep for custom subclasses.

# 1.4.0

## New Settings

Custom Content: Custom cards and items can now be imported. Card/Item Creator: https://code.secretsisters.gay/AtO_Custom. Includes 16 custom items: Chalice of Queens, Divine Presence, Glass Ring, Golden Apple, Hellfire Earrings, Lucky Dice, Nature's Blessing, Righteous Rod, Ring of Stone, Rod of Blasting, Runic Shackles, Sanguine Scroll, Spicy Pasta, Void Stone, Wand of Warding, Zealot's Cross.

Export Vanilla JSON: Export vanilla data to Custom Content-compatible JSON files.

Export Sprites: Export sprites when exporting vanilla content.

Drop-Only Items Appear In Shops: Items that would normally not appear in shops, such as the Yggdrasil Root or Yogger's Cleaver, will appear.

Visit All Zones: You can visit all three of Aquarfall, Faeborg and Velkarath before going to the Void.

Conflict Resolution: Automatically select (1) lowest card; (2) closest to 2; (3) highest card; or (4) random to determine multiplayer conflicts.

All The Pets: Shows blob pets and Harley in the Tome of Knowledge and shop.

## Other Changes

Fixed Card Removal, so it now works consistently.

Ignore Minimum Deck Size has been removed in favor of Minimum Deck Size, where you can choose your own :)

Adjusted Bad Luck Protection scaling for the umpteenth time. I recommend setting this to ~10 if you make many rerolls (>50) in a town, and to ~300 if you make few (<5).

Emotional now allows you to use any character's emotes, not just those in the party.

Emotional now allows you to put a character's emote on multiple cards at once.

Emotional now allows you to put emotes on cards in the Look screen.

Leaderboard entries now send version number, seed hash and team. These can't yet be easily viewed by clients; this is just prepwork for making full 'Past Games'-style leaderboard entries!

# 1.3.0

Internal only.

# 1.2.1

## New Settings

Level Over 50: Allows characters to be raised up to rank 500.

## Other Changes

Clone skin now resets if invalid, rather than just refusing to load the hero selection screen.

Clones now add any rank progress gained to the original.

_Actually_ reduced default Bad Luck Protection from 10 to 5 (but made it scale better with Act).

# 1.2.0

## New Settings

Enable Clones: Adds three clone characters to the DLC section of hero selection. VERY EARLY STAGES, VERY LITTLE TESTING!

Very little multiplayer testing. If you're already in the hero selection screen, you need to leave and re-enter it to update the clones.

Clones do not count for events (i.e., do not get dialogue options). I'll see what I can do about this in the future.

The perk selection screen will sometimes show the wrong skin in the top right, but appears to be functional.

# 1.1.3

Fixed Overly Tenergetic breaking Overcharge/Repeat Up To X cards (thanks WeizheXu!)

# 1.1.2

Minor text fix. I'm on a roll :)

# 1.1.1

Fixed Bad Luck Protection (oops).

# 1.1.0

## New Settings

Bad Luck Protection: Every new shop you see without purchasing increases the chance of higher rarity items.

Corrupted Map Shops: Allow shops on the map (e.g. werewolf shop in Senenthia) to have corrupted goods for sale.

Corrupted Obelisk Shops: Allow obelisk corruption shops to have corrupted goods for sale.

Corrupted Town Shops: Allow town shops to have corrupted goods for sale.

Gold ++: Gives 500k+ gold.

Dust ++: Gives 500k+ gems.

Supplies ++: Gives 500+ supplies.

Card Removal: Allow removal of any cards (vanilla), only curses, no curses, or no cards at all.

Bugfix: Equipment HP: fixes a vanilla bug that allows infinite stacking of HP by purchasing the same item you already have equipped.

Skip Cinematics: Automatically skip cinematics.

Auto Continue: Automatically 'continue' events.

Export Settings: Copy this string to back up settings.

Import Settings: Paste a string in to import settings.

Auto Create Room on MP Load: Use previous settings to automatically create lobby room when loading multiplayer game.

Auto Ready on MP Load: Automatically readies up non-host players when loading multiplayer game.

Spacebar to Continue: Spacebar clicks the 'Continue' button in events for you.

No Player Class Requirements: (IN TESTING - BUGGY AF) ignore class requirements? e.g. pretend you have a healer? might let you ignore specific character requirements.

No Player Item Requirements: (IN TESTING - BUGGY AF) ignore equipment/pet requirements? e.g. should let you 'drop off the crate' @ Tsnemo's ship?

No Player Requirements: (IN TESTING - BUGGY AF) ignore key item???? requirements.

## Other Changes

Removed Corrupted Shops setting (replaced with Map/Obelisk/Town).

Removed Become Rich setting (replaced with Gold/Dust/Supplies ++).

Multiplayer: Settings will now automatically copy the host (updated when hero selection screen opens and when host changes settings).

# 1.0.5

Added Ignore Minimum Deck Size option.

# 1.0.4

Added Force Server Select for locations other than Australia; added Overly Tenergetic (allows characters to go above 10 energy).

# 1.0.2

Made compatible with AtO 1.1.02.
Added Travel Anywhere, No Perk Requirements, No Travel Requirements. Started fixing the Modify Perks exceptions.

# 1.0.0

Initial release.
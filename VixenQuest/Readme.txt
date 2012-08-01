To Do:
 Core:
 - add graf of planes and portal points in places to trawel between planes
 - random name generator for main hero
 - toys
   - which sex actions affects
   - which gender opponent could be
   - skill bonus
 - curses
   - exhibitionism - temporary disables clothes
   - zoophilia - animals are preferred opponents
   - strighten orientation - temporary change homosexual orientation to bi and bi to stright
   - strictly strighten orientation - temporary set orientation to stright
   - alter orientation - temporary change stright orientation to bi and bi to homosexual
   - strongly alter orientation - temporary change orientation to homosexual
   - transgendering - changes male to female and female/shemale to male
   - clitoromegaly - applyable only to females, temporary transforms female to shemale
   - ambygender - applyable only to males, temporary transforms male to shemale
   - weakness - temporary sets force to 0
   - monstrosity - temporary sets beauty to 0
   - unlucky - temporary sets luck to 0
 - powers
   - ?
 - use parametrized actions' description patterns (?)
 - add automatic merging of liqids vessels in inventory (vials merged to flasks, flasks to bottles)
 - add named encounters (with named NPCs - like heroes of well-known games, books or video)
 - starting equipment - cheap and without any bonuses, only clothes
 - rest actions could bring or lost gold
 - make it so males got loot and xp from active sex, while females from passive (?)
 - building a strong hierarchy of ranks in locations (?) 

 Misc.:
 - store static const data in outer data files

 Interface:
 - add visualization for plane's map and hero's travels across it
 - add minimized mode, when only current location, opponent name, action description and progress are shown
 - add "hide to system tray" function
 - make highligting when something had changed in vixen's stats, skills or inventory...
 - saving events log to file (?)
 - manual generation of main hero(-ine).
 - save/load




What's new:

ver. 0.12.0
 - fixed loot names to not contain victim's name - only race, gender, orientation and profession
 - new algorithm of locations grid generation - now it uses Voronoi diagrams
 - generating of events journal of hero adventures
 - added new skill - foreplay, used for dildo play and fisting
 - skills are now assimetrical - for example, ass-fucking now not anal vs. anal, but anal vs. traditional
 - sex encounter now always contains at least one sex action, except failed pursue attempts
 - sex encounters again could contain more then one sex action
 - active anal sex now not "fuck into ass", but "fuck by ass"

ver. 0.11.1
 - added connecting grid of lands inside plane, now travelling from one land to another is calculated according to that grid

ver. 0.11.0
 - added types of locations and sets of races (sacred animal, domestic animal, deity, common people, rare races...)
 - builded a quite strong hierarchy of ranks in locations according their types
 - random names generated for solo encounters and all locations
 - added hierarchy of locations according their types (e.g. setlements containsbuildings, states consists of lands, etc.)
 - adventure structure changed: now each encounter contains only 1 sex action (but also contains move actionn and rest action), and each quest contains a sequense of encounters according hero's path to quest target.

ver. 0.10.4
 - now direction of action's progress bar increasing is determined by is action initiated by vixen or by target.
 - average actions' length decreased.
 - action description now contains only action description, without opponents name.
 - actions order now isn't determined strictly by previous action success. Only second action in encounter works this way. So, success of first action - evade, pursue or seducing - determins, who will initiate second (fuck itself) action. After that, action actor changes each turn, while both have enough potency for next action. When one of lovers exhausts, other will continue acting continuously, until consume all his potency. In general, it looks like some kind of ping-pong.

ver. 0.10.3
 - when losing an encounter, now there is a chance (determined by vixen's and opponent's Luck) that vixen will lose one of equipped items.
 - now it needs a less xp to levelup, so total leveling speed is more like before 0.10.2.
 - changed algorithm of choosing opponents, so now it oriented on average vixen effective (altered by equipment) stats instead of pure level.

ver. 0.10.2
 - now if vixen wins encounter, he/she receives revard, experience and plot progression, while after losing an encounter, vixen loses some plot progression.
 - winning and losing conditions now are better displayed in interface.

ver. 0.10.1
 - altered interface to show current values of vixen's and target's potency.

ver. 0.10.0
 - added new stat - "Potency".
 - clothes now gives bonuses to Potency, jevelry - to all other stats.
 - redesigned encounter generation to win/lose conception, potency used as consumable resource, determining the ability to continue the encounter.

ver. 0.9.0
 - at Superior Marketplace it's possible now not only to sell loot, but also to buy new equipment.

ver. 0.8.0
 - different vixen classes now gots different bonuses to stats when levelup.

ver. 0.7.3
 - generation of clothes and jevelry redesigned: now clothes have quality, material and type, while jevelry have 2 materials and type.
 - jevelry now giving an absolute bonus to one of the stats, according it's level.
 - clothes now improving one of the stats by percent value

ver. 0.7.2
 - jevelry now have a type (finger, neck, bracelet of pierceing) and could be equipped according to it.

ver. 0.7.1
 - now equiping only 1 item if there are several of them looted.
 - unequipped items are returned to inventory.

ver. 0.7.0
 - divided rewards by types with different rules of name generation:
   - clothes
   - sex toys
   - jevelry
   - other trash
 - clothes now have gender and body part properties.
 - looted clothes could be equipped.

ver. 0.6.5
 - each race now have own preferred primary skill and preferred orientation, choosen by random at game start.

ver. 0.6.4
 - all opponents now have stats and skills - both are completely random level/2+rnd(level).

ver. 0.6.3
 - strengthen the limitations of mobs levels in location. Now it uses Gauss distribution.
 - encounter length too.
 - added a lot of sexy encounter rewards names.

ver. 0.6.2
 - loot generation - gifts only for completing non-animal encounter with history > 1, level of gift is linked to encounter level.
 - altered generation of plot, so higher acts contains more quests - like in PQ.
 - made it so if opponent is much higher level and NOT interested in hero - encounter ends.

ver. 0.6.1
 - new opponents subtype - someone's relative (son/doughter, sister/brother, cousin, lover...)
 - vixen's stats
   - Force - instead of outdoor skill
   - Beauty - instead of exhibitionism skill
   - Luck - improves chances for good loot

ver. 0.6.0
 - Added vixen's orientation. Now opponents are choosen according vixen's orientation. Actions selections in encounters also use it.

ver. 0.5.1
 - Encounter generation improved, now it uses vixen skills values and opponent level for selection of actions.

ver. 0.5.0
 - Vixen skills now improving after completing proper action if opponent level was higher than current skill value.

ver. 0.4.2
 - Now new encounter generating after previous encounter finished instead of pregenerating on quest creation.

ver. 0.4.1
 - Added new mob types - animal and god.

ver. 0.4.0
 - Action chains transformed to encounters.
 - Quest progress now counted in encounters instead of actions.

ver. 0.3.5
 - Now new quest generating after previous quest finished instead of pregenerating on act creation.

ver. 0.3.4
 - Added new quest types - item and location.

ver. 0.3.2
 - Added "rest" action

ver. 0.3.1
 - Added generations of action chains, united by a common target mob

ver. 0.3.0
 - Loot added.
 - Added travelling to market for selling the loot

ver. 0.2.1
 - Separated races from moods. Now each mob characterized by race, mood and profession.

ver. 0.2.0
 - Locations added.

ver. 0.1.0
 - Basic plot and quests generation.
 - Plot progression.

ver. 0.0.1
 - Started work on project. Main form and basic interface.
 - Vixen stats and skills randomly generating, but not using at all.
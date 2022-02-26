
# The Game
 * There's a hexagonal board full of tiles.
 * There's a number of players that are in the game.
 * There are the following hexagon types:
  * Star
  * Empty
 * Stars explode after a pre-defined number of turns, destroying everything on that tile.
 * The last player standing, wins.
 * Each player starts with 1 colony.
 * There are 2 resources: Metals, and Organics.
 * Each colony draws a card at the start of each turn.
 * The cards are as follows:
  * TODO, but these cards allow you to do actions.

## Foreseen Consequences of The Game
 * Games should snowball to victory for 1 player very quickly after some initial stalemate is broken.
 * Games can only last so long until all the stars explode.

# Build Log
## 2022-01-20
As of this point, I've had a very difficult time determining how I want to store events in the database.
For example:
 - What goes in the Star Placement Event?
   - What happens if the placement event needs new fields in the future?
     - The old code won't see those fields, which might be ok, but new code would see the old events and have a problem.
   - What happens if the requirements of placing a star change in the future?
   - What if I find a bug in one of the basic model objects where fixing it breaks all the old versions of the game?

The above problems are caused by a few things:
 1. The desire to be able to replay any old game from the past and have it still play out the same.
 2. The desire to push out a server update and old games can finish on the same ruleset.

I think part of the problem is that the individual model objects are determining if they are valid.
Instead, shouldn't the game rules determine if they are valid? If version 1 and 2 of the game have different needs on star lifetime calculations, then stars shouldn't do anything other than be dumb data containers.

One solution is to make every model object dumb, and create a masssive GameRules class that is given every single object in the match and then operates on that.

## 2022-01-24
### Idea 1: Reflect to find Game Rules
The big problem with this solution is that it isn't aware of data structure changes.
Protobuf's design principles might come in handy here.
I could make every field optional and mark old items with the deprecated property.
Interface: IGameRules
Getter: Version of type int

## 2022-02-26
Instead of finding the game rules, each game should just point to the game rules id it uses.

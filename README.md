# Project Overview
The current version of the game includes a pirate player and a monkey enemy. The player cannot take damage or be killed, but the monkey can be damaged and killed by the player.

### Player functionality:
- Walk and rotate using ASDW keys.
- Attack using the left mouse button.
- Idle, rotate, walk, and attack animations have been added.
- When the player attacks, Unity's SphereCastNonAlloc method is used to detect collisions along rays extending from the sword. These rays are drawn in the scene view in Unity when the player attacks. 

### Monkey functionality:
- The monkey will pursue the player once the player enters the monkey's detection range. The detection range can be visualized in the scene view in Unity by selecting the monkey.
- Once the player has been out of the monkey's detection range for 2 seconds, the monkey will stop pursuing the player and return to its starting position.
- Idle, walk, attack, take damage, and death animations have been added (although the attack animation currently does nothing).
- The monkey will die after being hit twice by the player.
- The monkey game object will be removed from the game (destroyed) 3 seconds after getting defeated and will not respawn.

# Setup Instructions
The game can be run by loading the project into Unity.

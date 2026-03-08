# Project Overview
The current version of the game includes a pirate player and a monkey enemy. The player can take damage, be killed, and respawn. The monkey can take damage, be killed, but will not respawn.

### Player features:
- Walk and rotate using ASDW or arrow keys.
- Attack using the left mouse button.
- Idle, walk, attack, take damage, and death animations have been added.
- The player can walk off the platform but will fall forever. The game needs to be stopped and restarted to return to the platform.
- The player will respawn in their original position with max HP after being defeated.

### Monkey features:
- The monkey will pursue the player once the player enters the monkey's detection range. 
- Once the player has been out of the monkey's detection range for 2 seconds, the monkey will stop pursuing the player and return to its starting position.
- Idle, walk, attack, take damage, and death animations have been added.
- The monkey will die after being hit twice by the player and will not respawn.
- After getting defeated, the monkey slowly becomes invisible and emits particles for 2 seconds before disappearing.
- If the monkey kills the player, the monkey will stop pursuing the player and return to its original position.

### Scene View Features:
- When the player/monkey attacks, Unity's SphereCastNonAlloc method is used to detect collisions along rays extending from the player's sword/monkey's paws. These rays are drawn in the scene view in Unity when the player/monkey attacks.
- The monkey's detection range can be visualized in the scene view in Unity by selecting the monkey.

# Setup Instructions
The game can be run by loading the project into Unity.

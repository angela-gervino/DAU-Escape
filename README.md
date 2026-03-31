# Project Overview
In this island escape game, you play as a pirate who has just found the burried treasure on an island and must now find the way back to their ship. Navigate through the island's maze of hills and trees, fighting off wild monkeys along the way, and return to the ship to successfully escape and win the game.

### Player features:
- Walk and rotate using ASDW or arrow keys.
- Attack using the left mouse button.
- The player will respawn in their original position with max HP after being defeated.

### Monkey features:
- The monkey will pursue the player once the player enters the monkey's detection range. 
- Once the player has been out of the monkey's detection range for 2 seconds, the monkey will stop pursuing the player and return to its starting position.
- The monkey will die after being hit twice by the player and will not respawn.
- After getting defeated, the monkey slowly becomes invisible and emits particles for 2 seconds before disappearing.
- If the monkey kills the player, the monkey will stop pursuing the player and return to its original position.

### Scene View Features:
- When the player/monkey attacks, Unity's SphereCastNonAlloc method is used to detect collisions along rays extending from the player's sword/monkey's paws. These rays are drawn in the scene view in Unity when the player/monkey attacks.
- The monkey's detection range can be visualized in the scene view in Unity by selecting the monkey.

# Setup Instructions
The game can be run by loading the project into Unity.

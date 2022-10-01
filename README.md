# Unity-Endless-Climb-Game
A casual game made with unity.
## Description
A game where character is bouncy and sticky ball. Purpose is climbing the platforms and running from the saw which slowly follows the character.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/GP.gif)

### Combo Ability
If player light up 6 platforms quickly, they get a boost bonus.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/combo.gif)

### Optimaztion
Whenever player reaches critical levels, game generates more playable environment and player always goes up so they slowly moving away from the zero point. This might cause performance issues. To deal with it, game deletes old/unused playable environment and moves the current movable environment to the position of deleted playable environment. As a result, player's distance from origin point and object count in the scene can never be greater then a constant value.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/Perf.gif)

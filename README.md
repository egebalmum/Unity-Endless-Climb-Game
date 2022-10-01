# Unity-Endless-Climb-Game
A casual game made with unity.
## Description
A game where character is bouncy and sticky ball and the purpose is running from the saw by climbing platforms.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/GP.gif)

### Combo Ability
Activated when character lights up six platforms quickly.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/combo.gif)

### Optimization
Whenever player reaches critical levels, game generates more playable environment and player slowly moving away from the origin point. These designs might cause performance issues. To deal with it, game deletes old/unused playable environment and moves the current movable environment to the position of deleted playable environment. As a result, player's distance from origin point and object count in the scene can never be greater then a constant value.
- ![](https://github.com/Pika10/Unity-Endless-Climb-Game/blob/main/ReadmeGifs/Perf.gif)

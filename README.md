# Chase
## Gameplay
The chased team player controls one hero, the chasing team player controls three heroes. The purpose of the chased team player is to lead his hero to the top edge of the map (to one of the green cells). The purpose of the chasing team player is to surround the opponent's hero before he reaches his destination. Each hero has some amount of move points, which indicates what distance the hero can pass per turn. The chased team hero can kill the opponent's heroes. In order to capture the chased team hero, the opponent's heroes should surround him from at least two directions. The information about opponent's heroes is for both teams inaccurate unless an opponent's hero is very close to one of your heroes. The inaccurate information is displayed as an area in which the opponent's hero is. If the opponent's hero is far away, it is possible that no information (including inaccurate) is displayed.


The map contains three types of cells - road, mountain and destination cells. The road cells are walkable, the mountain cells are not. The destination cells are walkable and if the chased team's hero reaches one of these, the chased team wins. The map is read from `Assets/Data/map3.txt` in the `Assets/Scripts/Map.cs` script. You can change the MAP_FILE_PATH in order to change the map being read. In the txt file, the "R" symbol indicates a road cell, the "M" symbol a mountain cell and the "D" symbol a destination cell. You can use e.g. [Ascii Paint](https://kirilllive.github.io/ASCII_Art_Paint/ascii_paint.html) to paint your own map.

## Controls
* `Scroll up/down` - zoom
* `Middle button` - drag camera
* `1/2/3/4` - choose one of your team's heroes
* `Left button` - calculate path and then confirm it
* `I` - show/hide information
* `F` - finish your turn

## Technical details
The game is made in Unity using the C# programming language. In order to build the game, you should open it with Unity and press `File`->`Build Profiles`->`Build`.
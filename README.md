# Procedurally-Generated-Terrain
Personal research project into how to procedurally generate terrain in unity using shaders. This project was done in Unity 2020.1.1f1 and the goal was learn about methods that are used to generate terrain as the user walks around a world without the whole world being loaded when the player launches the game. 

Using vertices, the terrain is completely generated using shaders and you can upscale or downscale the number of triangles used to build the terrain, which will then determine how detailed the terrain is. Also, as you wander around it will unload tiles that the user can't see to decrease the strain on user memory.

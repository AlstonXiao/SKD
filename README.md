# SKD Game Develop Team
<p style="color:grey">Note: We are not using github to do version control so the the version on Github may not be the latest version</p>

![Chapter one of the game (In development)](./Github_ReadME_Source/Game_Overview.png)

## Background Story
This game takes place in the far future. At that time, humans have run out of resources and you, the main character was sent to find more energy sources. One day, you suddenly found a strong signal of energy underneath the ground. You decided to explore, but after you enter the underground, you discovered that there is a ruin of a huge world, and the signal is emitted from the deepest part of the ruin. Unfortunately, you lost all your advanced tools when you get to the underground world, and the path to the deepest part is blocked. So you have to use the power of the artifact to get to the resource that emits the signal. But, who knows what will happen when you get there.

The history of this ruin can be traced back to ancient Roma. People living at that time got an alien artifact, and it has enormous power. A few people stole this artifact and used it to build this underground utopia. However, due to some reasons, all people in this underground world has died and here became a ruin. The main character will solve the puzzles when he approaches the artifact also experience the story happened in the past.

## Game Mechanism 
This game is a combination of puzzle-solving and storytelling. The alien technology you will use is called "duplicate space." You can duplicate a part of space, and place it wherever needed, and the copied object will do the same thing as the original object. You need to use this superpower from the artifact to remove obstacles and get to the energy source. As the player solving the puzzles, The environment will tell the story in the past, and the player can experience the epic story by the details in the environment.

### Example

On the right we have a rotating receiver. We apply the artifact's power to "capture" the space and put it on the left. The copied object now rotates with the old one.


![Duplicated object moves as the original object moves](./Github_ReadME_Source/Connected_Object.gif)

# Current Game Status
## Development
* Story : Finished the outline and currently writing scripts for level one.
* Game play : Finished the core mechanism and now building UI for the player.
* Level Design: Currently half way on designing the chapter one.
## Demo 
We include a very short demo of the game to show the core game mechanism. To run the demo, you have to install unity. Open the **DEMO** scene in the Asset folder, click run and the demo will start.

This demo is very simple. You need to open the door by pushing two buttons at the same time. However, you only have one box and can only push the button once. So what you should do right now?

**Note: please clone this project instead of download it since Github will ignore large files when forming the zip for download**
### Tips for control the character
1. To move around, use `WASD`.
2. To use the artifact's power, press `Q` then a cube will emerge for you to choose the space to copy.
![Selecting space to duplicate](./Github_ReadME_Source/Space_Selecting.png)
3. You can use `scroll wheel` to adjust the distance of the selection box and `z` or `x` to adjust its size. 
4. When you finish selecting, press `left click` to confirm. You will automatically hold the duplicated object on your hand.
![Duplicated a part of space and holding it](./Github_ReadME_Source/Space_Selected.png)
5. To place the object, press `E`
![Put it at the right place](./Github_ReadME_Source/Place_Space.png)
# About the team
We are students at University of Wisconsin - Madison and this game is one of our side projects.  
## Team Member
* **Team Leader:** Yan Xiao, yxiao65@wisc.edu
* **Team Member:** Yuzhe Gu 
* **Team Member:** Tianyiao Ren (Rae) 
* **Team Member:** Roland Jiang
### Special Thanks To: Xiao Fei, Yuxuan Shao, Zhaoqing CUi

# Disclaimer 

The material embodied in this software is provided to you "as-is" and without warranty of any kind, express, implied or otherwise, including without limitation, any warranty of fitness for a particular purpose. The use of this code is your responsibility and at your own risk.

This repository is for nonprofit, educational purposes only. All the files in this repository should not be reused for other purposes outside this repository.
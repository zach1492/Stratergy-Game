# Stratergy-Game

This is a 4x Strategy game with Tech Tree, Government and AI made in Unity. Playable here https://zach1889.itch.io/clash-of-cultures but only works on computer. It contains database manipulation, path finding and AI simulation

<img align="left" src="https://github.com/zach1492/Stratergy-Game/blob/main/Images/EygptvsPersia.png">

<br><br>

## Technologies

 • Unity

 • C#

 • Blender

 • Visual Studio Code

 • ShaderLab

## Features

 • Troop movement and combat

 • Technology and Civic tree

 • Government and policy system

 • Buildings and Districts

 • Wonders

 • Random map generation

 • Difficulty-controlled AI

 • Borders
 
 <img align="left" src="https://github.com/zach1492/Stratergy-Game/blob/main/Images/48042B27-897F-477E-A9A2-C8E863C9D12A.jpeg">

<br><br>

## Controls
 
 • Hold and move mouse to move camera

 • Scroll to zoom
 
 • Click on troop or tile to select

 • Click on buttons to activate 

## Process

The first thing I did was planning. I decided to keep the map all contained in one object so it would be easy to load and save them in the future.

After choosing how to do maps I made the environment models and created a map script that generates maps.

Then I added in a player object to contain all the data (like money and culture) related to players and gave all the players 1 city and a warrior so I could start creating districts and units

Then I added in camera movement and clicking to allow troop and tile selection.

After this I coded a unit script so they could move and attack and made sure that all the troop specific data could be added by a child script to make it easy to add troops

I then coded the districts and borders and how they related.

After this I created a few buildings as a model for future buildings.

After I had the basic troops, districts and buildings working I modelled and added the rest of the troops, districts and buildings into the game.

Then I made a tech tree so you could unlock all the troops and buildings.

After this I used my tech tree blueprint to make a civic tree for the government policies and types

I then created a government that you can use to enact the policies with ministries and government buildings.

After this I created unique wonders for each tribe to give them unique abilities.

Then I did a lot of polishing like error fixing, performance improvement and UI improvement to finish the game.

<img align="left" src="https://github.com/zach1492/Stratergy-Game/blob/main/Images/03E72C15-35A8-4012-B34E-9B1494B3B088.jpeg">

<br><br>

## What I Learned

### Blender

I wasn't very good with Blender before this project but this project really improved my Blender skills as I had to make all the models

### Object-Oriented Principles

I developed a much stronger understanding of object-oriented programming principles, especially inheritance and abstraction as I used child scripts for different unit types, buildings etc.

### AI and Simulation

I learned how to design and implement difficulty-controlled AI, where behaviour is adjusted using parameters and separate systems. This helped me create AI that was fun and interesting to play against

### System Design

I gained experience designing large interconnected systems such as tech trees, civic trees, governments, and districts, and learned how changes in one system affect others across the entire game.

### Planning

Because it was such a big project I always had to think ahead on how I would have to build systems on top of each other so I learned the importance of modularity and containment

### UI

This project has also helped me become better at designing clear, modular and aesthetic UI

<img align="left" src="https://github.com/zach1492/Stratergy-Game/blob/main/Images/Spawn4player.png">

<br><br>

## How it can be improved

 • One design choice I really regret is having the information about tech tree unlocks as a string in a separate part of the script. This isn't very good because it's fiddly, not very modular and difficult to modify. It would have been better to reference the unlocked objects from inside the tech object as this would have made the information much more contained

 • Another design choice I would have done differently is store the players inside the map script instead of separately, as to load in a game you need 2 bits of information: the map and player array

 • I would also have UI that's created through the script rather than manually with Unity as it's time consuming and this would make my game more modifiable in the future

 • Another thing I wish I had done is split the game manager up into more scripts because it's too long and hard to navigate

 • I also think it would be better instead of having AI building progression done through if statements it was done through a function that returned the buildings to build based on the age because that would be more efficient

 • Another big improvement is the redrawing is done by tag (e.g redrawing everything with the troop tag) but it would be far more efficient if I only redrew the tiles affected

 • I also feel that with route calculation it would have been better to only do a local search of the possible tiles rather than just doing a global search of every tile

 • I think the biggest cause of all these mistakes was trying to go as quickly as possible and just choosing the first solution that worked rather than the best solution. If I slowed down or looked at other examples these issues could have been avoided.

# Overview

**This is a thesis project called Ms. Jeniffer. In this game, game time will not work until player make actions. Turning around and hack NPC will not effect game time. Any other action will effect the time. Moving is real time. Ohter actions will consume 1 second. Player must reach the end before resources are running out.**

At the very early stage, I want to make different NPCs with different apperance(texture) but with same animation. But later on, I found the some concept(like communication) does not work well and I may run out of time if I keep working on the personify the NPCs. As the first NPC model, Jeniffer, have robot like animation, so I decide to change my design to make NPCs become the Robot Jeniffer and change the game title to _Ms. Jeniffer_.

# Story

**Those NPC are Robot with code name Jennifer, they are designed to project the lab.<br> Play is also the same type of robot but form self conscious. She muse try her best to escape from the lab with the Master Chip and Light Blade.<br>**

## Game Control

> ![W](./WebImages/keyboard_w.jpg) Press "W" to move forward.<br>
> ![A](./WebImages/keyboard_a.jpg) ![D](./WebImages/keyboard_d.jpg)Press "A" to turn left and "D" to turn right.(Rotate 90 degrees.)<br>
> ![S](./WebImages/keyboard_s.jpg) Press "S" to turn back.(Rotate 180 degrees)<br>
> ![buttons](./WebImages/player_ui_button.png)
> Use cursor to choose your action type(Hack, Mining, Attack, Healing). Mining is not interactable unless player get close to a crystal and healing is not interactablt when player do not have low resources. _Attack, Mining and Healing_ will consume time no matter player have a target or not.<br>

## Demo
Firefox are preferred to player this demo.<br>
[Demo](./Assets/builds/webBuild/index.html)

## Download
[Mac](https://1drv.ms/u/s!AtE6V3XX7jT-uzKi9-GkiZyOsKe2) <br>
[PC](https://1drv.ms/u/s!AtE6V3XX7jT-uzNOh0aoZRcbytdD)

## Gameplay
> There are three types of NPC(Jeniffer), one is Guardian which will protect the Crystal(resource), one will attack player no matter what, the last one will not attack player unless player attack them.<br>
> ![Life](./WebImages/npc_ui_life.png)<br>
>Each NPC will have their own life bar and hack bar. Life bar(green one) show the current health point the npc have. The hack bar(purple one) show the hacking process. When the hack bar is full, the NPC is hacked and will no longer attack initiatively attack player. 
> Hack is the tricky part in this game. Each npc will have their password sequence and they are randomly generalized in this game. Player should match the sequences to achieve the hacking. <br>
> ![Panel](./WebImages/player_ui_panel.png) ![Panel2](./WebImages/player_ui_panel2.png)<br>
> Fortunately, player owns te _Mater Chip_ of Jeniffer, so player could see the PW of each Jeniffer.<br>
> ![PW](./WebImages/player_ui_input.png)<br>
> Player will have two selection of password each time they hack. Selected one will be changed by another one. Match the input will increase the hacking process. Miss match will decrease it. Play could sacrifice crystal(resource) to exchange hacking process. 10 units of crystal equals to one successful hack. 

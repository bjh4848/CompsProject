# CompsMaster

## Code Overview
Features:
- Kinematic Player Controller
- Raycast Hit Detection
- Advanced Gun System
- Procedural Recoil & Procedural Camera Shake
- Basic Enemy AI
- Progressive Enemy Spawner

The code can be a bit difficult to understand at first. It's best to start with the **Player** object in the scene and the **PlayerMovement** script. This script is especially simple. The Input method grabs keystrokes and assigns them to the corresponding variable 'x' or 'z'. In Unity whenever variables are initialized, the variables themselves can be assigned from the main view by dragging objects into the corresponding category, allowing us to assign variables to game objects outside of the script. If you're ever confused as to where a variable was assigned, it could be through this method.

The player character's position is then transformed with the x and z variables. Similarly, if the spacebar is hit, the player's position is transformed by our jumpHeight and gravity variable. All these variables allow us to fine tune things like the acceleration and the deceleration of our player character.

A quick overview of a more advanced script can be found in *GunLogic* script, which corresponds to most of the movement and behavior of our gun.

It first starts (in the start() function if you can believe it) with initializing our gun sound effect, and and our AdvancedCamScript. We also initialize the bullets remaining in our magazine along with setting the state of our gun state machine to ready. Within Unity there are a couple of functions which are standard, things like Start, Awake, Update, FixedUpdate, etc. all have corresponding behaviors within the framework. Start() always plays whenever the script starts, and fixedupdate occurs at a similar rate to Update(), except unlike Update(), which is called at every frame. FixedUpdate is called indepedently of frames and instead is called in league with Unity's physics engine, meaning the physics in this case aren't dependent on frame rate.

In FixedUpdate we're essentially attempting to return to our starting positions that we outlined in the code, in this case since we haven't fired yet there's no positions to return to, but if we had fired we would be lerping between the origin of the gun's position represented with (Vector3.zero) and the positional recoil which changes the gun on the X, Y, Z axis and the rotational recoil which rotates the gun up and away with a Quaternion. A Quaternion is used by Unity as a means of implementing an object rotational position in 3D, even though it's technically incorrect I think of it as the Vector3 equivalent but for angles. You'll also notice it's multiplied by time.deltaTime, which calculates the time and the time between frames to once again ensure that it's not tied to frame rate. This theoretically should be redundant as we're doing this in FixedUpdate() but I found that it looked choppy and weird without so I'm playing it safe. You'll also notice the funnily named Slerp method, which is the same as a Lerp except we're spherically interpolating between separate points. While both are similar in that they provide a smooth looking transition between points, the slerp method also utilizes the direction and thus will interpolate upon an angle and not just a straight line between two points.

In our update function we simply call the MyInput() function. MyInput checks every frame for the state of our gun and if we've pressed a button that qualifies as a shot. There's also an allowedButtonHold boolean which is for if our gun can simply shoot by holding left click or if it requires a mousepress for every shot. The R key which reloads our gun only if we've shot at some point, and then the Shoot() function is called only when a number of conditions are met.

ShotReady, which is only true when not doing any other action, we're not reloading, and we have bullets to shoot. The ```bulletsShot = bulletsPerShot;``` is simply in the case if we want to shoot multiple bullets on each click so we'd set bulletsPerShot to be something like 7.

In Reload() we use the Invoke() method which calls another function after a set amount of time. Therefore after *reloadTime* we then call the ReloadFinished() function below it. Which resets the amount of bullets we we have, plays the reload sound effect, and resets are reloading variable.

In the shoot function we do a lot of different things. We play a gunshot sound, determine x & Y bullet spread by selecting a random number between two bounds we set earlier, and then project a raycast. This is also where we add rotational recoil to our gun. We have two separate Vectors here, both of which add to the recoil instead of getting assigned to it allowing the recoil to look smoth and realistic incase we shoot multiple times in quick succession. The first Vector is rotational recoil, which rotates the gun along the X and Z, the second (even though its still named rotational recoil) attempts to mimic kickback by moving the gun along the Y and Z. Both of these combine to offer realistic movement to that of a gun recoiling from being fired. Back to the raycast, this raycast needs to be where we're actually aiming, and thus we create a new Vector3 in front us, we then project that raycast forward and collect information on what we hit. If the gameobject we collided with has the 'tag' enemy, then we get the script which controls enemy AI health and make it take damage. In Unity we can use functions from other scripts by using the 'GetComponent' method allowing us to communicate with multiple scripts at once. We then instantiate a Muzzle Flash particle at the tip of our gun, before decreasing the amount of bullets in our magazine. Lastly, we have another Invoke method. This one is so we can't shoot all the bullets in our magazine by clicking the mouse really quick, instead there's an artifical cooldown. 



---

## Replication Instructions

1. This project was made with Unity, and thus requires Unity Hub in order to function. Which can be downloaded [here](https://unity.com/download).
2. This project was developed with Unity LTS *2021.3.14f1* and will work best in the corresponding version. Using a version earlier or later could cause it to break.
3. Clone this repository or download it as a zip.
4. Open up Unity and under the Projects tab, select *Open*, select *add project from disk*, then select where you installed this repository.
5. The project should now open and initialize.

**Note**, upon opening the project if it asks for 'Script Updating Consent' in regards to it refering to an API that has been updated select 'No'. Updating the script API will cause the project to break. 



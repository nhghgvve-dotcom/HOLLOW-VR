# HOLLOW

A standalone VR multiplayer horror game for Meta Quest 3S, built with Unity.

## Build 0.1 — movement prototype

The first milestone is a small offline test room with tracked VR hands and arm-powered ape locomotion.

### Planned for this milestone
- Meta Quest 3S target (Android)
- OpenXR controller tracking
- Gorilla-style arm locomotion: push against the floor and walls to move
- No joystick walking
- A simple test room and visible hands/arms

### Unity setup
1. Install a current Unity LTS release with **Android Build Support** (Android SDK & NDK tools and OpenJDK).
2. Create a 3D URP or 3D Core project named `HOLLOW`.
3. In Package Manager, install **XR Plugin Management**, **OpenXR Plugin**, and **XR Interaction Toolkit**.
4. In Project Settings → XR Plug-in Management, enable OpenXR for Android. Enable the Meta Quest Touch Controller profile in OpenXR interaction profiles.
5. Add an XR Origin (Action-based) to the scene and configure its left/right controller transforms to follow the Quest controllers.
6. Add a Rigidbody to the player root (freeze rotation) and attach `HollowGorillaLocomotion.cs`. Assign the XR Origin root and tracked left/right hand transforms.
7. Create a floor and wall colliders. Put the player root slightly above the floor and test with the headset guardian enabled.
8. Build for Android/ARM64. Use the Android build settings recommended by your installed Unity/OpenXR versions.

### Important
This repository currently contains the starter source and setup notes, not a compiled APK or a complete Unity scene. Controller tracking and locomotion must be tested in Unity on the Quest 3S. Multiplayer, monster AI, maps, cosmetics, and final character art are later milestones.

## Controls
Move naturally by placing/pushing your tracked hands against solid surfaces. There is no artificial joystick locomotion in this prototype.

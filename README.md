# HOLLOW

**HOLLOW** is an original VR multiplayer horror game prototype for Meta Quest 3S, developed in Unity by Living Room Studios.

## Build 0.1 — Quest movement prototype
The first playable milestone is a greybox test room with real OpenXR head/controller tracking, visible ape hands, and early arm-push locomotion. There is no joystick walking.

### Project
- Unity: 6000.0.40f1
- Target: Meta Quest 3S / Android
- XR: OpenXR
- Architecture: ARM64
- Package manifest: `Packages/manifest.json`

### Open in Unity
1. Install Unity 6.0.40f1 with Android Build Support, Android SDK & NDK Tools, and OpenJDK.
2. Open this repository in Unity Hub.
3. Allow Package Manager to resolve dependencies.
4. Enable OpenXR under Project Settings → XR Plug-in Management → Android, and enable the Meta Quest Touch Controller Profile.
5. Choose **HOLLOW → Build Prototype Scene** from the Unity menu.
6. Press Play with an OpenXR headset, or build to Android for Quest testing.

### GitHub APK build
The workflow `.github/workflows/build-quest-apk.yml` builds an ARM64 APK. Configure the repository Actions secrets `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD`, then run **Build HOLLOW for Meta Quest** under Actions.

See [Build 0.1 notes](Docs/BUILD_0_1.md).

This is an early prototype. Multiplayer, monster AI, full ape character art, audio, and finished maps are future milestones.

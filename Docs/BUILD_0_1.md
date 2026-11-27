# HOLLOW — Build 0.1 checklist

- [ ] Create Unity 3D project and add this repository's Assets folder.
- [ ] Install XR Plug-in Management, OpenXR Plugin, and XR Interaction Toolkit.
- [ ] Configure Android OpenXR and Quest Touch Controller profile.
- [ ] Set up XR Origin with tracked left/right controllers.
- [ ] Add a Rigidbody player root and assign both hand transforms to HollowGorillaLocomotion.
- [ ] Build a safe test room with floor and wall colliders.
- [ ] Test controller tracking and arm locomotion on a Meta Quest 3S.
- [ ] Tune hand radius, damping, and push speed based on headset testing.

This is a source-code prototype. It is not yet a finished Unity project or installable APK.


## Set up a free Unity Personal license for GitHub Actions

The APK workflow cannot compile until Unity provides a license file. Do not share your Unity password or license file in chat.

1. In GitHub, open **Actions** and run **Request Unity Personal License**.
2. When it finishes, open that run and download the **HOLLOW-Unity-Activation-Request** artifact. Extract the `.alf` file.
3. Sign in to your Unity account and upload the `.alf` file at https://license.unity3d.com/manual.
4. Choose the Unity Personal license option if eligible, complete Unity's activation steps, and download the resulting `.ulf` license file.
5. In this repository, open Settings → Secrets and variables → Actions → New repository secret. Name it exactly `UNITY_LICENSE` and paste the full contents of the `.ulf` file as its value.
6. Run **Build HOLLOW for Meta Quest** from Actions. The workflow is manual so it will not repeatedly fail before the license is configured.

If Unity does not offer a Personal license for the account, follow Unity's displayed licensing instructions; do not attempt to bypass activation.

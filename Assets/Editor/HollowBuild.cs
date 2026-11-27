#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class HollowBuild
{
    public static void BuildAndroid()
    {
        Directory.CreateDirectory("Assets/Scenes");
        HollowSceneBuilder.BuildScene();

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.productName = "HOLLOW";
        PlayerSettings.companyName = "Living Room Studios";
        PlayerSettings.applicationIdentifier = "com.livingroomstudios.hollow";
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.virtualRealitySupported = true;

        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/HOLLOW_TestRoom.unity" },
            locationPathName = "build/HOLLOW-Quest.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception("HOLLOW Android build failed: " + report.summary.result);
        Debug.Log("HOLLOW APK built: " + options.locationPathName);
    }
}
#endif

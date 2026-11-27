#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class HollowSceneBuilder
{
    [MenuItem("HOLLOW/Build Prototype Scene")]
    public static void BuildScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var bootstrap = new GameObject("HOLLOW Prototype");
        bootstrap.AddComponent<HollowPrototypeBootstrap>();
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/HOLLOW_TestRoom.unity");
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/HOLLOW_TestRoom.unity", true)
        };
        AssetDatabase.SaveAssets();
        Debug.Log("HOLLOW test room scene created.");
    }
}
#endif

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep20Demo
{
    public static void Apply()
    {
        const string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        GameObject old =
            GameObject.Find(
                "GameDemoController"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        AudioClip clueSound =
            AssetDatabase.LoadAssetAtPath<
                AudioClip
            >(
                "Assets/Audio/fx_clue.wav"
            );

        GameObject demo =
            new GameObject(
                "GameDemoController"
            );

        CollegeDemoController controller =
            demo.AddComponent<
                CollegeDemoController
            >();

        controller.clueSound =
            clueSound;

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 20 TERMINEE - DEMO CINEMATIQUE AJOUTEE"
        );
    }
}

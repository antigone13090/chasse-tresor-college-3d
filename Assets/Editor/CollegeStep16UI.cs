using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep16UI
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
                "ModernGameUI"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        GameObject ui =
            new GameObject(
                "ModernGameUI"
            );

        CollegeModernUI controller =
            ui.AddComponent<
                CollegeModernUI
            >();

        controller.showStartScreen =
            true;

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 16 TERMINEE - ACCUEIL + HUD + PAUSE + VICTOIRE"
        );
    }
}

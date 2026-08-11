using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep21Trailer
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
                "GameTrailerController"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        GameObject trailer =
            new GameObject(
                "GameTrailerController"
            );

        trailer.AddComponent<
            CollegeTrailerController
        >();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 21 TERMINEE - BANDE-ANNONCE CINEMATIQUE"
        );
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeAddMiniMap
{
    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        GameObject old =
            GameObject.Find("MiniMapHUD");

        if (old != null)
            Object.DestroyImmediate(old);

        GameObject minimap =
            new GameObject("MiniMapHUD");

        minimap.AddComponent<MiniMapHUD>();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "MINI-CARTE AJOUTEE"
        );
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class RemoveLabels
{
    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Remove("LABEL_VIE_SCOLAIRE");
        Remove("LABEL_CDI");
        Remove("LABEL_VERS_CDI");
        Remove("LABEL_ESCALIER");
        Remove("CDI_BEACON");

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();

        Debug.Log(
            "Pancartes supprimees - jeu restaure"
        );
    }

    static void Remove(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

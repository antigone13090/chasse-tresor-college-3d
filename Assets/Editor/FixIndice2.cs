using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class FixIndice2
{
    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        GameObject clue =
            GameObject.Find("Indice_2_CDI");

        if (clue == null)
        {
            Debug.LogError("Indice_2_CDI introuvable !");
            return;
        }

        // JUSTE DERRIERE LA PORTE DU CDI
        // CDI : R+1 = 3.2 m
        clue.transform.position =
            new Vector3(
                -1.0f,   // x = 31.5 sur l'ancienne carte
                 4.0f,   // hauteur R+1
                 2.8f    // juste après la porte
            );

        // Beaucoup plus visible
        clue.transform.localScale =
            new Vector3(
                1.2f,
                1.2f,
                1.2f
            );

        // Supprime ancienne lumière éventuelle
        Transform oldLight =
            clue.transform.Find("IndiceLight");

        if (oldLight != null)
            Object.DestroyImmediate(oldLight.gameObject);

        // Petite lumière autour de l'indice
        GameObject lightObject =
            new GameObject("IndiceLight");

        lightObject.transform.SetParent(clue.transform);
        lightObject.transform.localPosition =
            Vector3.zero;

        Light light =
            lightObject.AddComponent<Light>();

        light.type = LightType.Point;
        light.range = 5f;
        light.intensity = 3f;
        light.color =
            new Color(0.1f, 1f, 0.6f);

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "Indice 2 repositionne juste derriere la porte du CDI."
        );
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public static class CollegeStep11
{
    const float C = 2f;

    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Delete("Tresor_Final");

        Material wood =
            Mat(
                "Treasure_Wood",
                new Color(.35f,.14f,.04f)
            );

        Material dark =
            Mat(
                "Treasure_Metal",
                new Color(.12f,.12f,.10f)
            );

        Material gold =
            Mat(
                "Treasure_Gold",
                new Color(1f,.72f,.05f)
            );

        GameObject root =
            new GameObject("Tresor_Final");

        // Trésor original :
        // grille (43,27)
        // positionné au centre de cette cellule

        root.transform.position =
            new Vector3(
                X(43.5f),
                0f,
                Z(27.5f)
            );

        root.AddComponent<TreasureObject>();

        // Coffre
        Cube(
            "Chest_Base",
            root.transform.position
                + new Vector3(0f,.55f,0f),
            new Vector3(3f,1.1f,2f),
            wood,
            root.transform
        );

        Cube(
            "Chest_Lid",
            root.transform.position
                + new Vector3(0f,1.35f,0f),
            new Vector3(3.2f,.55f,2.1f),
            wood,
            root.transform
        );

        // Bandes métalliques
        Cube(
            "Chest_Band_1",
            root.transform.position
                + new Vector3(-.85f,.65f,0f),
            new Vector3(.18f,1.4f,2.12f),
            dark,
            root.transform
        );

        Cube(
            "Chest_Band_2",
            root.transform.position
                + new Vector3(.85f,.65f,0f),
            new Vector3(.18f,1.4f,2.12f),
            dark,
            root.transform
        );

        // Serrure
        Cube(
            "Chest_Lock",
            root.transform.position
                + new Vector3(0f,.85f,-1.08f),
            new Vector3(.40f,.55f,.15f),
            gold,
            root.transform
        );

        // Or visible
        Cube(
            "Gold_1",
            root.transform.position
                + new Vector3(-.45f,1.7f,0f),
            new Vector3(.7f,.22f,.65f),
            gold,
            root.transform
        );

        Cube(
            "Gold_2",
            root.transform.position
                + new Vector3(.35f,1.75f,.1f),
            new Vector3(.8f,.22f,.65f),
            gold,
            root.transform
        );

        // Lumière dorée
        GameObject lightObj =
            new GameObject("TreasureLight");

        lightObj.transform.SetParent(
            root.transform
        );

        lightObj.transform.localPosition =
            new Vector3(0f,2.2f,0f);

        Light light =
            lightObj.AddComponent<Light>();

        light.type = LightType.Point;
        light.range = 8f;
        light.intensity = 4f;
        light.color =
            new Color(1f,.72f,.15f);

        // Ajoute la scène aux Build Settings
        // pour permettre R = recommencer.

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes
            );

        bool found = false;

        foreach (
            EditorBuildSettingsScene s
            in scenes)
        {
            if (s.path == scenePath)
            {
                s.enabled = true;
                found = true;
            }
        }

        if (!found)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    scenePath,
                    true
                )
            );
        }

        EditorBuildSettings.scenes =
            scenes.ToArray();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 11 TERMINEE - TRESOR + VICTOIRE"
        );
    }

    static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material,
        Transform parent)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;
        obj.transform.position =
            position;
        obj.transform.localScale =
            scale;

        obj.transform.SetParent(parent);

        obj.GetComponent<Renderer>()
            .sharedMaterial = material;

        return obj;
    }

    static Material Mat(
        string name,
        Color color)
    {
        string path =
            "Assets/Materials/"
            + name
            + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (mat != null)
        {
            mat.color = color;
            return mat;
        }

        Shader shader =
            Shader.Find("Standard");

        if (shader == null)
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

        mat = new Material(shader);
        mat.color = color;

        AssetDatabase.CreateAsset(
            mat,
            path
        );

        return mat;
    }

    static float X(float value)
    {
        return (value-32f)*C;
    }

    static float Z(float value)
    {
        return (value-22f)*C;
    }

    static void Delete(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

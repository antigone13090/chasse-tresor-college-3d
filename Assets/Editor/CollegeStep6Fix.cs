using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep6Fix
{
    const float CELL = 2f;
    const float R1 = 3.2f;

    public static void Fix()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        // Indices un peu plus gros
        ResizeClue("Indice_1_Vie_Scolaire");
        ResizeClue("Indice_2_CDI");

        // Ancienne balise éventuelle
        DestroyIfExists("CDI_BEACON");

        // Balise jaune très visible devant le CDI
        Material yellow =
            GetMaterial(
                "CDI_Beacon_Yellow",
                new Color(1f, 0.8f, 0.05f)
            );

        GameObject beacon =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        beacon.name = "CDI_BEACON";

        beacon.transform.position =
            new Vector3(
                MapX(31.5f),
                R1 + 2.5f,
                MapZ(24.3f)
            );

        beacon.transform.localScale =
            new Vector3(
                0.30f,
                5f,
                0.30f
            );

        beacon.GetComponent<Renderer>()
            .sharedMaterial = yellow;

        Collider col =
            beacon.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();

        Debug.Log(
            "ETAPE 6 FIX : interaction indices + balise CDI"
        );
    }

    static void ResizeClue(string name)
    {
        GameObject clue =
            GameObject.Find(name);

        if (clue != null)
        {
            clue.transform.localScale =
                new Vector3(
                    0.85f,
                    0.85f,
                    0.85f
                );
        }
    }

    static float MapX(float x)
    {
        return (x - 32f) * CELL;
    }

    static float MapZ(float z)
    {
        return (z - 22f) * CELL;
    }

    static Material GetMaterial(
        string name,
        Color color
    )
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

    static void DestroyIfExists(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

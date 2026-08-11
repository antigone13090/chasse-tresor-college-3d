using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep7
{
    const float C = 2f;

    public static void Create()
    {
        string path = "Assets/Scenes/CollegePrototype.unity";
        var scene = EditorSceneManager.OpenScene(path);

        Delete("[SPECIAL] Auditorium (auditorium)");
        Delete("Auditorium_Visitable");
        Delete("Indice_3_Auditorium");

        Material wall = Mat("AudWall", new Color(.72f,.69f,.65f));
        Material floor = Mat("AudFloor", new Color(.30f,.24f,.22f));
        Material doorMat = Mat("AudDoor", new Color(.32f,.14f,.07f));
        Material green = Mat("Clue_Green", new Color(.10f,.90f,.70f));

        GameObject root = new GameObject("Auditorium_Visitable");

        float x1 = X(47);
        float x2 = X(52);
        float z1 = Z(19);
        float z2 = Z(25);

        float cx = (x1 + x2) / 2f;
        float cz = (z1 + z2) / 2f;

        Cube(
            "Auditorium_Floor",
            new Vector3(cx, -.1f, cz),
            new Vector3(x2-x1, .2f, z2-z1),
            floor,
            root.transform
        );

        Cube(
            "Aud_North",
            new Vector3(cx, 1.5f, z1),
            new Vector3(x2-x1, 3f, .18f),
            wall,
            root.transform
        );

        Cube(
            "Aud_South",
            new Vector3(cx, 1.5f, z2),
            new Vector3(x2-x1, 3f, .18f),
            wall,
            root.transform
        );

        Cube(
            "Aud_East",
            new Vector3(x2, 1.5f, cz),
            new Vector3(.18f, 3f, z2-z1),
            wall,
            root.transform
        );

        float dz = Z(22);

        Cube(
            "Aud_West_1",
            new Vector3(x1, 1.5f, (z1 + dz - 1f) / 2f),
            new Vector3(.18f, 3f, (dz - 1f) - z1),
            wall,
            root.transform
        );

        Cube(
            "Aud_West_2",
            new Vector3(x1, 1.5f, (dz + 1f + z2) / 2f),
            new Vector3(.18f, 3f, z2 - (dz + 1f)),
            wall,
            root.transform
        );

        GameObject pivot = new GameObject("Auditorium_Door");
        pivot.transform.SetParent(root.transform);
        pivot.transform.position = new Vector3(x1, 0f, dz - 1f);

        DoorController dc = pivot.AddComponent<DoorController>();
        dc.openAngle = 95f;
        dc.speed = 4f;

        GameObject panel =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        panel.name = "Auditorium_Door_Panel";
        panel.transform.SetParent(pivot.transform);

        panel.transform.localPosition =
            new Vector3(0f, 1.25f, 1f);

        panel.transform.localScale =
            new Vector3(.12f, 2.5f, 2f);

        panel.GetComponent<Renderer>().sharedMaterial =
            doorMat;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                Cube(
                    "Seat_" + r + "_" + c,
                    new Vector3(
                        X(48.3f + c * .8f),
                        .35f,
                        Z(20.2f + r * 1.1f)
                    ),
                    new Vector3(.55f,.7f,.55f),
                    floor,
                    root.transform
                );
            }
        }

        Cube(
            "Auditorium_Stage",
            new Vector3(
                X(49.5f),
                .20f,
                Z(24f)
            ),
            new Vector3(
                5f,
                .4f,
                1.8f
            ),
            floor,
            root.transform
        );

        GameObject clue =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        clue.name = "Indice_3_Auditorium";

        clue.transform.position =
            new Vector3(
                X(49.5f),
                .9f,
                Z(22f)
            );

        clue.transform.localScale =
            Vector3.one;

        clue.GetComponent<Renderer>().sharedMaterial =
            green;

        ClueObject data =
            clue.AddComponent<ClueObject>();

        data.order = 3;
        data.clueTitle =
            "Indice 3 - Auditorium";

        data.clueText =
            "Le prochain indice aime les lieux où l'on écoute.";

        data.nextObjective =
            "Va vers le gymnase pour trouver l'indice 4.";

        GameObject lightObject =
            new GameObject("IndiceLight");

        lightObject.transform.SetParent(clue.transform);
        lightObject.transform.localPosition =
            Vector3.zero;

        Light li =
            lightObject.AddComponent<Light>();

        li.type = LightType.Point;
        li.range = 6f;
        li.intensity = 3f;
        li.color = new Color(.1f, 1f, .6f);

        EditorSceneManager.SaveScene(scene, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("ETAPE 7 TERMINEE");
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
        obj.transform.position = position;
        obj.transform.localScale = scale;

        if (parent != null)
            obj.transform.SetParent(parent);

        obj.GetComponent<Renderer>().sharedMaterial =
            material;

        return obj;
    }

    static Material Mat(string name, Color color)
    {
        string path =
            "Assets/Materials/" + name + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (mat != null)
        {
            mat.color = color;
            return mat;
        }

        Shader shader = Shader.Find("Standard");

        if (shader == null)
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

        mat = new Material(shader);
        mat.color = color;

        AssetDatabase.CreateAsset(mat, path);

        return mat;
    }

    static float X(float value)
    {
        return (value - 32f) * C;
    }

    static float Z(float value)
    {
        return (value - 22f) * C;
    }

    static void Delete(string name)
    {
        GameObject obj = GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

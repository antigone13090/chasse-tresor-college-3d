using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep6DoorLabels
{
    const float CELL = 2f;
    const float R1 = 3.2f;

    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        DestroyIfExists("LABEL_VIE_SCOLAIRE");
        DestroyIfExists("LABEL_CDI");
        DestroyIfExists("LABEL_VERS_CDI");

        CreateSign(
            "LABEL_VIE_SCOLAIRE",
            new Vector3(
                MapX(23.5f),
                2.7f,
                MapZ(30f) + 0.35f
            ),
            "VIE SCOLAIRE",
            new Color(0.88f, 0.35f, 0.12f),
            4.8f
        );

        CreateSign(
            "LABEL_CDI",
            new Vector3(
                MapX(31.5f),
                R1 + 2.7f,
                MapZ(24f) + 0.35f
            ),
            "CDI",
            new Color(0.96f, 0.82f, 0.08f),
            2.4f
        );

        CreateSign(
            "LABEL_VERS_CDI",
            new Vector3(
                MapX(28.5f),
                R1 + 1.9f,
                MapZ(24.5f) + 1.15f
            ),
            "VERS CDI",
            new Color(0.96f, 0.82f, 0.08f),
            3.2f
        );

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 6 : panneaux VIE SCOLAIRE / CDI ajoutes"
        );
    }

    static void CreateSign(
        string rootName,
        Vector3 position,
        string text,
        Color panelColor,
        float width
    )
    {
        GameObject root =
            new GameObject(rootName);

        root.transform.position = position;

        // panneau
        GameObject panel =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        panel.name = rootName + "_Panel";
        panel.transform.SetParent(root.transform);
        panel.transform.localPosition =
            Vector3.zero;
        panel.transform.localScale =
            new Vector3(width, 0.9f, 0.08f);

        var panelRenderer =
            panel.GetComponent<Renderer>();

        panelRenderer.sharedMaterial =
            CreateMaterial(
                rootName + "_Mat",
                panelColor
            );

        var panelCollider =
            panel.GetComponent<Collider>();

        if (panelCollider != null)
            panelCollider.enabled = false;

        // texte face 1
        CreateText(
            root.transform,
            rootName + "_Text_Front",
            text,
            new Vector3(0f, 0f, 0.06f),
            Quaternion.Euler(0f, 180f, 0f)
        );

        // texte face 2
        CreateText(
            root.transform,
            rootName + "_Text_Back",
            text,
            new Vector3(0f, 0f, -0.06f),
            Quaternion.identity
        );
    }

    static void CreateText(
        Transform parent,
        string name,
        string content,
        Vector3 localPos,
        Quaternion localRot
    )
    {
        GameObject textObj =
            new GameObject(name);

        textObj.transform.SetParent(parent);
        textObj.transform.localPosition = localPos;
        textObj.transform.localRotation = localRot;

        TextMesh tm =
            textObj.AddComponent<TextMesh>();

        tm.text = content;
        tm.fontSize = 72;
        tm.characterSize = 0.10f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.black;

        Font font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        if (font != null)
        {
            tm.font = font;
            MeshRenderer mr =
                textObj.GetComponent<MeshRenderer>();

            if (mr != null)
                mr.sharedMaterial = font.material;
        }
    }

    static Material CreateMaterial(
        string name,
        Color color
    )
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

        Shader shader =
            Shader.Find("Standard");

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

    static float MapX(float x)
    {
        return (x - 32f) * CELL;
    }

    static float MapZ(float z)
    {
        return (z - 22f) * CELL;
    }

    static void DestroyIfExists(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

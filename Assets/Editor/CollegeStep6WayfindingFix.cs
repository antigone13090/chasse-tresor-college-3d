using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep6WayfindingFix
{
    const float CELL = 2f;
    const float R1 = 3.2f;

    public static void Apply()
    {
        string scenePath = "Assets/Scenes/CollegePrototype.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        // Suppression anciens panneaux / balises
        DestroyIfExists("LABEL_VIE_SCOLAIRE");
        DestroyIfExists("LABEL_CDI");
        DestroyIfExists("LABEL_VERS_CDI");
        DestroyIfExists("LABEL_ESCALIER");
        DestroyIfExists("CDI_BEACON");

        // -----------------------------
        // VIE SCOLAIRE
        // -----------------------------
        CreateSign(
            "LABEL_VIE_SCOLAIRE",
            new Vector3(
                MapX(23.5f),
                2.55f,
                MapZ(30f) + 0.22f
            ),
            "VIE SCOLAIRE",
            new Color(0.92f, 0.42f, 0.10f),
            2.8f
        );

        // -----------------------------
        // ESCALIER
        // -----------------------------
        CreateSign(
            "LABEL_ESCALIER",
            new Vector3(
                MapX(24f),
                1.85f,
                MapZ(27.8f)
            ),
            "ESCALIER",
            new Color(0.15f, 0.65f, 1.0f),
            2.4f
        );

        // -----------------------------
        // VERS CDI
        // -----------------------------
        CreateSign(
            "LABEL_VERS_CDI",
            new Vector3(
                MapX(28.2f),
                R1 + 1.75f,
                MapZ(24.6f) + 0.85f
            ),
            "VERS CDI  >",
            new Color(0.98f, 0.85f, 0.08f),
            3.0f
        );

        // -----------------------------
        // CDI
        // -----------------------------
        CreateSign(
            "LABEL_CDI",
            new Vector3(
                MapX(31.5f),
                R1 + 2.55f,
                MapZ(24f) + 0.22f
            ),
            "CDI",
            new Color(0.98f, 0.85f, 0.08f),
            1.8f
        );

        // -----------------------------
        // Balise jaune devant le CDI
        // -----------------------------
        Material yellow =
            GetMaterial(
                "CDI_Beacon_Yellow",
                new Color(1f, 0.85f, 0.05f)
            );

        GameObject beacon =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        beacon.name = "CDI_BEACON";
        beacon.transform.position =
            new Vector3(
                MapX(31.5f),
                R1 + 2.3f,
                MapZ(24.4f)
            );

        beacon.transform.localScale =
            new Vector3(0.22f, 4.6f, 0.22f);

        beacon.GetComponent<Renderer>().sharedMaterial = yellow;

        Collider bc = beacon.GetComponent<Collider>();
        if (bc != null)
            bc.enabled = false;

        // -----------------------------
        // Repositionnement joueur
        // -----------------------------
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            CharacterController cc =
                player.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.transform.position =
                new Vector3(
                    MapX(23.5f),
                    0.10f,
                    MapZ(31f)
                );

            player.transform.rotation =
                Quaternion.Euler(0f, 180f, 0f);

            if (cc != null)
                cc.enabled = true;
        }

        EditorSceneManager.SaveScene(scene, scenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("ETAPE 6 FIX : panneaux corriges et chemin CDI balise");
    }

    static void CreateSign(
        string rootName,
        Vector3 position,
        string text,
        Color panelColor,
        float width
    )
    {
        GameObject root = new GameObject(rootName);
        root.transform.position = position;

        // panneau
        GameObject panel =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        panel.name = rootName + "_Panel";
        panel.transform.SetParent(root.transform);
        panel.transform.localPosition = Vector3.zero;
        panel.transform.localScale =
            new Vector3(width, 0.55f, 0.08f);

        var pr = panel.GetComponent<Renderer>();
        pr.sharedMaterial =
            CreateMaterial(rootName + "_Mat", panelColor);

        Collider pc = panel.GetComponent<Collider>();
        if (pc != null)
            pc.enabled = false;

        // texte côté 1
        CreateText(
            root.transform,
            rootName + "_Front",
            text,
            new Vector3(0f, 0f, 0.051f),
            Quaternion.identity
        );

        // texte côté 2
        CreateText(
            root.transform,
            rootName + "_Back",
            text,
            new Vector3(0f, 0f, -0.051f),
            Quaternion.Euler(0f, 180f, 0f)
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
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent);
        textObj.transform.localPosition = localPos;
        textObj.transform.localRotation = localRot;

        TextMesh tm = textObj.AddComponent<TextMesh>();
        tm.text = content;
        tm.fontSize = 72;
        tm.characterSize = 0.10f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.black;

        Font font =
            Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        if (font != null)
        {
            tm.font = font;
            MeshRenderer mr = textObj.GetComponent<MeshRenderer>();
            if (mr != null)
                mr.sharedMaterial = font.material;
        }
    }

    static Material CreateMaterial(string name, Color color)
    {
        string path = "Assets/Materials/" + name + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(path);

        if (mat != null)
        {
            mat.color = color;
            return mat;
        }

        Shader shader = Shader.Find("Standard");
        if (shader == null)
            shader = Shader.Find("Universal Render Pipeline/Lit");

        mat = new Material(shader);
        mat.color = color;

        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static Material GetMaterial(string name, Color color)
    {
        return CreateMaterial(name, color);
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
        GameObject obj = GameObject.Find(name);
        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

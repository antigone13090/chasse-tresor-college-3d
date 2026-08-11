using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep13Decor
{
    static GameObject root;

    static Material glass;
    static Material frame;
    static Material wood;
    static Material metal;
    static Material trunk;
    static Material leaves;
    static Material stone;
    static Material lampGlass;
    static Material planter;
    static Material soil;

    static float X(float mapX)
    {
        return (mapX - 32f) * 2f;
    }

    static float Z(float mapY)
    {
        return (mapY - 22f) * 2f;
    }

    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        GameObject old =
            GameObject.Find("Decor_Step13");

        if (old != null)
            Object.DestroyImmediate(old);

        root =
            new GameObject("Decor_Step13");

        CreateMaterials();

        AddWindows();
        AddDoorFrames();
        AddTrees();
        AddBenches();
        AddLampPosts();
        AddPlanters();
        AddCourtyardBorders();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 13 TERMINEE - FENETRES + ARBRES + BANCS + LAMPADAIRES + DECOR"
        );
    }

    // =====================================================
    // MATERIAUX
    // =====================================================

    static void CreateMaterials()
    {
        glass =
            MakeMaterial(
                "DECOR_Glass",
                new Color(
                    .20f,.48f,.66f,1f
                ),
                .05f,
                .82f
            );

        frame =
            MakeMaterial(
                "DECOR_WindowFrame",
                new Color(
                    .12f,.13f,.14f,1f
                ),
                .45f,
                .38f
            );

        wood =
            MakeMaterial(
                "DECOR_BenchWood",
                new Color(
                    .34f,.17f,.065f,1f
                ),
                0f,
                .30f
            );

        metal =
            MakeMaterial(
                "DECOR_Metal",
                new Color(
                    .12f,.13f,.14f,1f
                ),
                .72f,
                .45f
            );

        trunk =
            MakeMaterial(
                "DECOR_Trunk",
                new Color(
                    .25f,.12f,.045f,1f
                ),
                0f,
                .12f
            );

        leaves =
            MakeMaterial(
                "DECOR_Leaves",
                new Color(
                    .16f,.39f,.14f,1f
                ),
                0f,
                .13f
            );

        stone =
            MakeMaterial(
                "DECOR_Stone",
                new Color(
                    .55f,.56f,.53f,1f
                ),
                0f,
                .16f
            );

        planter =
            MakeMaterial(
                "DECOR_Planter",
                new Color(
                    .39f,.30f,.22f,1f
                ),
                0f,
                .20f
            );

        soil =
            MakeMaterial(
                "DECOR_Soil",
                new Color(
                    .15f,.08f,.035f,1f
                ),
                0f,
                .05f
            );

        lampGlass =
            MakeMaterial(
                "DECOR_LampGlass",
                new Color(
                    1f,.86f,.52f,1f
                ),
                0f,
                .70f
            );

        if (lampGlass.HasProperty("_EmissionColor"))
        {
            lampGlass.EnableKeyword(
                "_EMISSION"
            );

            lampGlass.SetColor(
                "_EmissionColor",
                new Color(
                    .70f,.48f,.12f
                ) * 1.3f
            );
        }
    }

    // =====================================================
    // FENETRES
    // =====================================================

    static void AddWindows()
    {
        GameObject group =
            NewGroup("Fenêtres");

        // Gymnase
        AddHorizontalWindowRow(
            group,
            8.2f,
            15.8f,
            34.02f,
            1.75f,
            5
        );

        // Self
        AddHorizontalWindowRow(
            group,
            50f,
            58f,
            14.02f,
            1.65f,
            5
        );

        // Auditorium
        AddVerticalWindowRow(
            group,
            52.02f,
            20f,
            24f,
            1.8f,
            3
        );

        // Vie scolaire
        AddHorizontalWindowRow(
            group,
            21.6f,
            25.4f,
            27.02f,
            1.7f,
            3
        );

        // CDI R+1
        AddHorizontalWindowRow(
            group,
            27f,
            35f,
            21.02f,
            4.75f,
            5
        );

        // Arts / Musique R+1
        AddHorizontalWindowRow(
            group,
            11f,
            19f,
            12.02f,
            4.75f,
            5
        );

        // Salles hautes
        AddHorizontalWindowRow(
            group,
            5f,
            21f,
            5.02f,
            1.8f,
            8
        );

        // Salles droite
        AddVerticalWindowRow(
            group,
            61.98f,
            4f,
            12f,
            1.8f,
            5
        );
    }

    static void AddHorizontalWindowRow(
        GameObject parent,
        float minX,
        float maxX,
        float mapZ,
        float worldY,
        int count)
    {
        for (int i=0; i<count; i++)
        {
            float t =
                (i + 1f)
                / (count + 1f);

            float mapX =
                Mathf.Lerp(
                    minX,
                    maxX,
                    t
                );

            CreateWindow(
                parent,
                new Vector3(
                    X(mapX),
                    worldY,
                    Z(mapZ)
                ),
                Quaternion.identity,
                false
            );
        }
    }

    static void AddVerticalWindowRow(
        GameObject parent,
        float mapX,
        float minZ,
        float maxZ,
        float worldY,
        int count)
    {
        for (int i=0; i<count; i++)
        {
            float t =
                (i + 1f)
                / (count + 1f);

            float mapZ =
                Mathf.Lerp(
                    minZ,
                    maxZ,
                    t
                );

            CreateWindow(
                parent,
                new Vector3(
                    X(mapX),
                    worldY,
                    Z(mapZ)
                ),
                Quaternion.Euler(
                    0f,
                    90f,
                    0f
                ),
                false
            );
        }
    }

    static void CreateWindow(
        GameObject parent,
        Vector3 position,
        Quaternion rotation,
        bool upper)
    {
        GameObject w =
            new GameObject("Fenetre");

        w.transform.SetParent(
            parent.transform
        );

        w.transform.position =
            position;

        w.transform.rotation =
            rotation;

        CreateBox(
            "Vitre",
            w.transform,
            Vector3.zero,
            new Vector3(
                1.35f,
                1.05f,
                .055f
            ),
            glass
        );

        CreateBox(
            "Cadre_Gauche",
            w.transform,
            new Vector3(
                -.72f,
                0f,
                -.01f
            ),
            new Vector3(
                .08f,
                1.18f,
                .09f
            ),
            frame
        );

        CreateBox(
            "Cadre_Droit",
            w.transform,
            new Vector3(
                .72f,
                0f,
                -.01f
            ),
            new Vector3(
                .08f,
                1.18f,
                .09f
            ),
            frame
        );

        CreateBox(
            "Cadre_Haut",
            w.transform,
            new Vector3(
                0f,
                .57f,
                -.01f
            ),
            new Vector3(
                1.52f,
                .08f,
                .09f
            ),
            frame
        );

        CreateBox(
            "Cadre_Bas",
            w.transform,
            new Vector3(
                0f,
                -.57f,
                -.01f
            ),
            new Vector3(
                1.52f,
                .08f,
                .09f
            ),
            frame
        );

        CreateBox(
            "Montant_Central",
            w.transform,
            new Vector3(
                0f,
                0f,
                -.015f
            ),
            new Vector3(
                .055f,
                1.05f,
                .095f
            ),
            frame
        );
    }

    // =====================================================
    // ENCADREMENTS DE PORTES
    // =====================================================

    static void AddDoorFrames()
    {
        GameObject group =
            NewGroup(
                "Encadrements_Portes"
            );

        DoorController[] doors =
            Object.FindObjectsByType<
                DoorController
            >(
                FindObjectsSortMode.None
            );

        int number = 0;

        foreach (
            DoorController door
            in doors)
        {
            Renderer renderer =
                door.GetComponent<Renderer>();

            if (renderer == null)
                renderer =
                    door.GetComponentInChildren<
                        Renderer
                    >();

            if (renderer == null)
                continue;

            Transform d =
                door.transform;

            float width =
                Mathf.Abs(
                    d.lossyScale.x
                );

            float height =
                Mathf.Abs(
                    d.lossyScale.y
                );

            float depth =
                Mathf.Abs(
                    d.lossyScale.z
                );

            if (width < .5f)
                width = 1.5f;

            if (height < 1f)
                height = 2.2f;

            if (depth > .5f)
                depth = .18f;

            GameObject frameRoot =
                new GameObject(
                    "Cadre_Porte_"
                    + (++number)
                );

            frameRoot.transform.SetParent(
                group.transform
            );

            frameRoot.transform.position =
                d.position;

            frameRoot.transform.rotation =
                d.rotation;

            CreateBox(
                "Montant_Gauche",
                frameRoot.transform,
                new Vector3(
                    -(width/2f + .07f),
                    0f,
                    0f
                ),
                new Vector3(
                    .12f,
                    height + .18f,
                    depth + .12f
                ),
                frame
            );

            CreateBox(
                "Montant_Droit",
                frameRoot.transform,
                new Vector3(
                    width/2f + .07f,
                    0f,
                    0f
                ),
                new Vector3(
                    .12f,
                    height + .18f,
                    depth + .12f
                ),
                frame
            );

            CreateBox(
                "Linteau",
                frameRoot.transform,
                new Vector3(
                    0f,
                    height/2f + .08f,
                    0f
                ),
                new Vector3(
                    width + .26f,
                    .13f,
                    depth + .12f
                ),
                frame
            );
        }

        Debug.Log(
            "Encadrements portes : "
            + number
        );
    }

    // =====================================================
    // ARBRES
    // =====================================================

    static void AddTrees()
    {
        GameObject group =
            NewGroup("Arbres");

        Vector2[] positions =
        {
            new Vector2(11f,30f),
            new Vector2(16f,37f),
            new Vector2(22f,40f),

            new Vector2(28f,41f),
            new Vector2(38f,41f),
            new Vector2(47f,41f),

            new Vector2(57f,37f),
            new Vector2(60f,30f),
            new Vector2(61f,21f),

            new Vector2(61f,14f),
            new Vector2(48f,6f),

            new Vector2(35f,7f),
            new Vector2(24f,7f),

            new Vector2(8f,20f),
            new Vector2(6f,27f),
            new Vector2(5f,34f)
        };

        int i = 0;

        foreach (Vector2 p in positions)
        {
            CreateTree(
                group,
                "Arbre_" + (++i),
                X(p.x),
                Z(p.y)
            );
        }
    }

    static void CreateTree(
        GameObject parent,
        string name,
        float x,
        float z)
    {
        GameObject tree =
            new GameObject(name);

        tree.transform.SetParent(
            parent.transform
        );

        tree.transform.position =
            new Vector3(
                x,
                0f,
                z
            );

        CreateCylinder(
            "Tronc",
            tree.transform,
            new Vector3(
                0f,
                1.4f,
                0f
            ),
            .38f,
            2.8f,
            trunk
        );

        CreateSphere(
            "Feuillage_1",
            tree.transform,
            new Vector3(
                0f,
                3.4f,
                0f
            ),
            new Vector3(
                2.7f,
                2.4f,
                2.7f
            ),
            leaves
        );

        CreateSphere(
            "Feuillage_2",
            tree.transform,
            new Vector3(
                -.75f,
                3.1f,
                .15f
            ),
            new Vector3(
                1.7f,
                1.8f,
                1.7f
            ),
            leaves
        );

        CreateSphere(
            "Feuillage_3",
            tree.transform,
            new Vector3(
                .75f,
                3.2f,
                -.15f
            ),
            new Vector3(
                1.8f,
                1.9f,
                1.8f
            ),
            leaves
        );
    }

    // =====================================================
    // BANCS
    // =====================================================

    static void AddBenches()
    {
        GameObject group =
            NewGroup("Bancs");

        CreateBench(
            group,
            X(25.5f),
            Z(19.2f),
            0f
        );

        CreateBench(
            group,
            X(31f),
            Z(19.2f),
            0f
        );

        CreateBench(
            group,
            X(37f),
            Z(19.2f),
            0f
        );

        CreateBench(
            group,
            X(41f),
            Z(23f),
            90f
        );

        CreateBench(
            group,
            X(24.8f),
            Z(26.5f),
            180f
        );

        CreateBench(
            group,
            X(18f),
            Z(32.5f),
            0f
        );
    }

    static void CreateBench(
        GameObject parent,
        float x,
        float z,
        float rotationY)
    {
        GameObject bench =
            new GameObject("Banc");

        bench.transform.SetParent(
            parent.transform
        );

        bench.transform.position =
            new Vector3(
                x,
                0f,
                z
            );

        bench.transform.rotation =
            Quaternion.Euler(
                0f,
                rotationY,
                0f
            );

        CreateBox(
            "Assise",
            bench.transform,
            new Vector3(
                0f,.52f,0f
            ),
            new Vector3(
                2.4f,.16f,.48f
            ),
            wood
        );

        CreateBox(
            "Dossier",
            bench.transform,
            new Vector3(
                0f,1.0f,.22f
            ),
            new Vector3(
                2.4f,.65f,.12f
            ),
            wood
        );

        CreateBox(
            "Pied_Gauche",
            bench.transform,
            new Vector3(
                -.85f,.27f,0f
            ),
            new Vector3(
                .12f,.55f,.38f
            ),
            metal
        );

        CreateBox(
            "Pied_Droit",
            bench.transform,
            new Vector3(
                .85f,.27f,0f
            ),
            new Vector3(
                .12f,.55f,.38f
            ),
            metal
        );
    }

    // =====================================================
    // LAMPADAIRES
    // =====================================================

    static void AddLampPosts()
    {
        GameObject group =
            NewGroup("Lampadaires");

        Vector2[] positions =
        {
            new Vector2(24f,18f),
            new Vector2(42f,18f),
            new Vector2(24f,28f),
            new Vector2(42f,28f),

            new Vector2(17f,31f),
            new Vector2(25f,35f),

            new Vector2(48f,29f),
            new Vector2(56f,29f)
        };

        foreach (Vector2 p in positions)
        {
            CreateLamp(
                group,
                X(p.x),
                Z(p.y)
            );
        }
    }

    static void CreateLamp(
        GameObject parent,
        float x,
        float z)
    {
        GameObject lamp =
            new GameObject("Lampadaire");

        lamp.transform.SetParent(
            parent.transform
        );

        lamp.transform.position =
            new Vector3(
                x,
                0f,
                z
            );

        CreateCylinder(
            "Poteau",
            lamp.transform,
            new Vector3(
                0f,
                1.7f,
                0f
            ),
            .12f,
            3.4f,
            metal
        );

        CreateBox(
            "Bras",
            lamp.transform,
            new Vector3(
                .28f,
                3.25f,
                0f
            ),
            new Vector3(
                .65f,
                .10f,
                .10f
            ),
            metal
        );

        CreateSphere(
            "Lampe",
            lamp.transform,
            new Vector3(
                .57f,
                3.18f,
                0f
            ),
            new Vector3(
                .32f,
                .25f,
                .32f
            ),
            lampGlass
        );
    }

    // =====================================================
    // JARDINIERES
    // =====================================================

    static void AddPlanters()
    {
        GameObject group =
            NewGroup("Jardinieres");

        CreatePlanter(
            group,
            X(20f),
            Z(31f)
        );

        CreatePlanter(
            group,
            X(26.5f),
            Z(31f)
        );

        CreatePlanter(
            group,
            X(46f),
            Z(30f)
        );

        CreatePlanter(
            group,
            X(43f),
            Z(17f)
        );
    }

    static void CreatePlanter(
        GameObject parent,
        float x,
        float z)
    {
        GameObject p =
            new GameObject("Jardiniere");

        p.transform.SetParent(
            parent.transform
        );

        p.transform.position =
            new Vector3(
                x,
                0f,
                z
            );

        CreateBox(
            "Bac",
            p.transform,
            new Vector3(
                0f,.28f,0f
            ),
            new Vector3(
                1.5f,.55f,.72f
            ),
            planter
        );

        CreateBox(
            "Terre",
            p.transform,
            new Vector3(
                0f,.57f,0f
            ),
            new Vector3(
                1.32f,.06f,.56f
            ),
            soil
        );

        for (int i=-2; i<=2; i++)
        {
            CreateSphere(
                "Plante",
                p.transform,
                new Vector3(
                    i*.23f,
                    .82f,
                    0f
                ),
                new Vector3(
                    .34f,.50f,.34f
                ),
                leaves
            );
        }
    }

    // =====================================================
    // BORDURES DE COUR
    // =====================================================

    static void AddCourtyardBorders()
    {
        GameObject group =
            NewGroup("Bordures_Cour");

        float left =
            X(24f);

        float right =
            X(42f);

        float top =
            Z(18f);

        float bottom =
            Z(28f);

        float centerX =
            (left + right) / 2f;

        float centerZ =
            (top + bottom) / 2f;

        CreateBoxWorld(
            "Bordure_Nord",
            group.transform,
            new Vector3(
                centerX,
                .07f,
                top
            ),
            new Vector3(
                Mathf.Abs(right-left),
                .14f,
                .16f
            ),
            stone
        );

        CreateBoxWorld(
            "Bordure_Sud",
            group.transform,
            new Vector3(
                centerX,
                .07f,
                bottom
            ),
            new Vector3(
                Mathf.Abs(right-left),
                .14f,
                .16f
            ),
            stone
        );

        CreateBoxWorld(
            "Bordure_Ouest",
            group.transform,
            new Vector3(
                left,
                .07f,
                centerZ
            ),
            new Vector3(
                .16f,
                .14f,
                Mathf.Abs(bottom-top)
            ),
            stone
        );

        CreateBoxWorld(
            "Bordure_Est",
            group.transform,
            new Vector3(
                right,
                .07f,
                centerZ
            ),
            new Vector3(
                .16f,
                .14f,
                Mathf.Abs(bottom-top)
            ),
            stone
        );
    }

    // =====================================================
    // HELPERS
    // =====================================================

    static GameObject NewGroup(
        string name)
    {
        GameObject g =
            new GameObject(name);

        g.transform.SetParent(
            root.transform
        );

        return g;
    }

    static GameObject CreateBox(
        string name,
        Transform parent,
        Vector3 localPosition,
        Vector3 scale,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;

        obj.transform.SetParent(
            parent,
            false
        );

        obj.transform.localPosition =
            localPosition;

        obj.transform.localScale =
            scale;

        Renderer r =
            obj.GetComponent<Renderer>();

        if (r != null)
            r.sharedMaterial = material;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateBoxWorld(
        string name,
        Transform parent,
        Vector3 worldPosition,
        Vector3 scale,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;

        obj.transform.SetParent(
            parent
        );

        obj.transform.position =
            worldPosition;

        obj.transform.localScale =
            scale;

        Renderer r =
            obj.GetComponent<Renderer>();

        if (r != null)
            r.sharedMaterial = material;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateCylinder(
        string name,
        Transform parent,
        Vector3 localPosition,
        float diameter,
        float height,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder
            );

        obj.name = name;

        obj.transform.SetParent(
            parent,
            false
        );

        obj.transform.localPosition =
            localPosition;

        obj.transform.localScale =
            new Vector3(
                diameter,
                height / 2f,
                diameter
            );

        Renderer r =
            obj.GetComponent<Renderer>();

        if (r != null)
            r.sharedMaterial = material;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateSphere(
        string name,
        Transform parent,
        Vector3 localPosition,
        Vector3 scale,
        Material material)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        obj.name = name;

        obj.transform.SetParent(
            parent,
            false
        );

        obj.transform.localPosition =
            localPosition;

        obj.transform.localScale =
            scale;

        Renderer r =
            obj.GetComponent<Renderer>();

        if (r != null)
            r.sharedMaterial = material;

        RemoveCollider(obj);

        return obj;
    }

    static void RemoveCollider(
        GameObject obj)
    {
        Collider c =
            obj.GetComponent<Collider>();

        if (c != null)
            Object.DestroyImmediate(c);
    }

    static Material MakeMaterial(
        string name,
        Color color,
        float metallic,
        float smoothness)
    {
        string path =
            "Assets/Materials/"
            + name
            + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<
                Material
            >(path);

        if (mat == null)
        {
            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader =
                    Shader.Find("Standard");

            mat =
                new Material(shader);

            AssetDatabase.CreateAsset(
                mat,
                path
            );
        }

        mat.color = color;

        if (mat.HasProperty("_BaseColor"))
            mat.SetColor(
                "_BaseColor",
                color
            );

        if (mat.HasProperty("_Color"))
            mat.SetColor(
                "_Color",
                color
            );

        if (mat.HasProperty("_Metallic"))
            mat.SetFloat(
                "_Metallic",
                metallic
            );

        if (mat.HasProperty("_Smoothness"))
            mat.SetFloat(
                "_Smoothness",
                smoothness
            );

        if (mat.HasProperty("_Glossiness"))
            mat.SetFloat(
                "_Glossiness",
                smoothness
            );

        EditorUtility.SetDirty(mat);

        return mat;
    }
}

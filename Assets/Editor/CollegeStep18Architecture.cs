using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;

public static class CollegeStep18Architecture
{
    static GameObject root;

    static Material roof;
    static Material fascia;
    static Material facade;
    static Material glass;
    static Material canopy;
    static Material grass;
    static Material shrub;
    static Material stone;

    static float X(float mapX)
    {
        return (mapX - 32f) * 2f;
    }

    static float Z(float mapY)
    {
        return (mapY - 22f) * 2f;
    }

    struct RoofSpec
    {
        public string name;
        public float x;
        public float z;
        public float w;
        public float d;
        public float y;

        public RoofSpec(
            string name,
            float x,
            float z,
            float w,
            float d,
            float y)
        {
            this.name = name;
            this.x = x;
            this.z = z;
            this.w = w;
            this.d = d;
            this.y = y;
        }
    }

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
                "Decor_Step18_Architecture"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        root =
            new GameObject(
                "Decor_Step18_Architecture"
            );

        CreateMaterials();

        UpgradeExistingWindows();

        CreateRoofs();

        CreateFacadeDetails();

        CreateEntranceCanopies();

        CreateWindowSills();

        CreateLandscaping();

        ImproveEnvironment();

        int colliders =
            root.GetComponentsInChildren<
                Collider
            >(true).Length;

        Debug.Log(
            "Decor Step18 colliders : "
            + colliders
        );

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 18 TERMINEE - TOITURES + FACADES + VITRAGES + RELIEF"
        );
    }

    // =====================================================
    // MATERIAUX
    // =====================================================

    static void CreateMaterials()
    {
        roof =
            MakeMaterial(
                "ARCH_Roof",
                new Color(
                    .12f,.14f,.16f,1f
                ),
                .05f,
                .22f
            );

        fascia =
            MakeMaterial(
                "ARCH_Fascia",
                new Color(
                    .25f,.27f,.28f,1f
                ),
                .15f,
                .28f
            );

        facade =
            MakeMaterial(
                "ARCH_FacadeTrim",
                new Color(
                    .82f,.80f,.74f,1f
                ),
                0f,
                .18f
            );

        canopy =
            MakeMaterial(
                "ARCH_Canopy",
                new Color(
                    .16f,.20f,.24f,1f
                ),
                .55f,
                .48f
            );

        grass =
            MakeMaterial(
                "ARCH_Grass",
                new Color(
                    .20f,.42f,.17f,1f
                ),
                0f,
                .08f
            );

        shrub =
            MakeMaterial(
                "ARCH_Shrub",
                new Color(
                    .10f,.31f,.10f,1f
                ),
                0f,
                .08f
            );

        stone =
            MakeMaterial(
                "ARCH_Stone",
                new Color(
                    .42f,.43f,.41f,1f
                ),
                0f,
                .14f
            );

        glass =
            MakeTransparentGlass();
    }

    // =====================================================
    // TOITURES
    // =====================================================

    static void CreateRoofs()
    {
        GameObject group =
            NewGroup("Toitures");

        RoofSpec[] roofs =
        {
            new RoofSpec(
                "Gymnase",
                7f,34f,10f,7f,3.25f
            ),

            new RoofSpec(
                "Vie_Scolaire",
                21f,27f,5f,3f,3.25f
            ),

            new RoofSpec(
                "CDI",
                26f,21f,10f,3f,6.45f
            ),

            new RoofSpec(
                "Auditorium",
                47f,19f,5f,6f,4.05f
            ),

            new RoofSpec(
                "Self",
                49f,14f,10f,3f,3.25f
            ),

            new RoofSpec(
                "Arts_Musique",
                10f,12f,10f,5f,6.45f
            ),

            new RoofSpec(
                "Salles_12_14",
                26f,16f,10f,4f,9.65f
            ),

            new RoofSpec(
                "Conseils",
                44f,25f,6f,4f,6.45f
            ),

            new RoofSpec(
                "Multi_1",
                40f,24f,3f,5f,3.25f
            ),

            new RoofSpec(
                "Salles_5_8",
                53f,8f,9f,5f,3.25f
            ),

            new RoofSpec(
                "Salles_1_4",
                55f,3f,7f,4f,3.25f
            ),

            new RoofSpec(
                "Infirmerie",
                52f,5f,3f,3f,3.25f
            ),

            new RoofSpec(
                "Salles_29_32",
                3f,5f,20f,4f,3.25f
            ),

            new RoofSpec(
                "Salles_17_28",
                4f,12f,8f,5f,3.25f
            ),

            new RoofSpec(
                "Salles_10_11",
                22f,10f,6f,4f,3.25f
            ),

            new RoofSpec(
                "Aumonerie",
                25f,36f,6f,4f,3.25f
            ),

            new RoofSpec(
                "Salles_23_26",
                31f,34f,8f,5f,3.25f
            ),

            new RoofSpec(
                "Salles_19_22",
                39f,35f,10f,5f,3.25f
            )
        };

        foreach (
            RoofSpec spec
            in roofs)
        {
            CreateRoof(
                group,
                spec
            );
        }
    }

    static void CreateRoof(
        GameObject parent,
        RoofSpec spec)
    {
        float cx =
            X(spec.x + spec.w / 2f);

        float cz =
            Z(spec.z + spec.d / 2f);

        float width =
            spec.w * 2f + .45f;

        float depth =
            spec.d * 2f + .45f;

        GameObject roofRoot =
            new GameObject(
                "Toit_" + spec.name
            );

        roofRoot.transform.SetParent(
            parent.transform
        );

        CreateBoxWorld(
            "Dalle_Toiture",
            roofRoot.transform,
            new Vector3(
                cx,
                spec.y,
                cz
            ),
            new Vector3(
                width,
                .22f,
                depth
            ),
            roof
        );

        float fasciaY =
            spec.y - .08f;

        CreateBoxWorld(
            "Fascia_Nord",
            roofRoot.transform,
            new Vector3(
                cx,
                fasciaY,
                cz - depth/2f
            ),
            new Vector3(
                width,
                .30f,
                .12f
            ),
            fascia
        );

        CreateBoxWorld(
            "Fascia_Sud",
            roofRoot.transform,
            new Vector3(
                cx,
                fasciaY,
                cz + depth/2f
            ),
            new Vector3(
                width,
                .30f,
                .12f
            ),
            fascia
        );

        CreateBoxWorld(
            "Fascia_Ouest",
            roofRoot.transform,
            new Vector3(
                cx - width/2f,
                fasciaY,
                cz
            ),
            new Vector3(
                .12f,
                .30f,
                depth
            ),
            fascia
        );

        CreateBoxWorld(
            "Fascia_Est",
            roofRoot.transform,
            new Vector3(
                cx + width/2f,
                fasciaY,
                cz
            ),
            new Vector3(
                .12f,
                .30f,
                depth
            ),
            fascia
        );
    }

    // =====================================================
    // FACADES
    // =====================================================

    static void CreateFacadeDetails()
    {
        GameObject group =
            NewGroup(
                "Details_Facades"
            );

        // Bandeau long de la façade haute
        CreateBoxWorld(
            "Bandeau_Nord",
            group.transform,
            new Vector3(
                X(13f),
                2.45f,
                Z(5f) - .08f
            ),
            new Vector3(
                40f,
                .20f,
                .16f
            ),
            facade
        );

        // Gymnase
        CreateBoxWorld(
            "Bandeau_Gymnase",
            group.transform,
            new Vector3(
                X(12f),
                2.35f,
                Z(34f) - .08f
            ),
            new Vector3(
                20f,
                .22f,
                .18f
            ),
            facade
        );

        // Self
        CreateBoxWorld(
            "Bandeau_Self",
            group.transform,
            new Vector3(
                X(54f),
                2.35f,
                Z(14f) - .08f
            ),
            new Vector3(
                20f,
                .22f,
                .18f
            ),
            facade
        );

        // CDI
        CreateBoxWorld(
            "Bandeau_CDI",
            group.transform,
            new Vector3(
                X(31f),
                5.55f,
                Z(21f) - .08f
            ),
            new Vector3(
                20f,
                .20f,
                .18f
            ),
            facade
        );

        // Arts/Musique
        CreateBoxWorld(
            "Bandeau_Arts",
            group.transform,
            new Vector3(
                X(15f),
                5.55f,
                Z(12f) - .08f
            ),
            new Vector3(
                20f,
                .20f,
                .18f
            ),
            facade
        );

        // Piliers décoratifs entrée
        CreateFacadePillar(
            group,
            20.9f,
            29f
        );

        CreateFacadePillar(
            group,
            25.1f,
            29f
        );

        // Auditorium
        CreateFacadePillar(
            group,
            47f,
            20f
        );

        CreateFacadePillar(
            group,
            47f,
            24f
        );
    }

    static void CreateFacadePillar(
        GameObject parent,
        float mapX,
        float mapZ)
    {
        CreateBoxWorld(
            "Pilier_Facade",
            parent.transform,
            new Vector3(
                X(mapX),
                1.55f,
                Z(mapZ)
            ),
            new Vector3(
                .30f,
                3.1f,
                .30f
            ),
            facade
        );
    }

    // =====================================================
    // AUVENTS
    // =====================================================

    static void CreateEntranceCanopies()
    {
        GameObject group =
            NewGroup("Auvents");

        CreateCanopy(
            group,
            X(23f),
            2.45f,
            Z(29.8f),
            4.8f,
            1.8f,
            0f
        );

        CreateCanopy(
            group,
            X(16.9f),
            2.45f,
            Z(36f),
            3.5f,
            1.5f,
            90f
        );

        CreateCanopy(
            group,
            X(51.5f),
            2.45f,
            Z(16.8f),
            3.6f,
            1.5f,
            0f
        );

        CreateCanopy(
            group,
            X(47.3f),
            2.45f,
            Z(22f),
            3.3f,
            1.45f,
            90f
        );
    }

    static void CreateCanopy(
        GameObject parent,
        float x,
        float y,
        float z,
        float width,
        float depth,
        float rotation)
    {
        GameObject canopyRoot =
            new GameObject("Auvent");

        canopyRoot.transform.SetParent(
            parent.transform
        );

        canopyRoot.transform.position =
            new Vector3(
                x,
                y,
                z
            );

        canopyRoot.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Toit",
            canopyRoot.transform,
            Vector3.zero,
            new Vector3(
                width,
                .16f,
                depth
            ),
            canopy
        );

        CreateBox(
            "Support_G",
            canopyRoot.transform,
            new Vector3(
                -width/2f + .15f,
                -1.15f,
                depth/2f - .15f
            ),
            new Vector3(
                .10f,
                2.25f,
                .10f
            ),
            canopy
        );

        CreateBox(
            "Support_D",
            canopyRoot.transform,
            new Vector3(
                width/2f - .15f,
                -1.15f,
                depth/2f - .15f
            ),
            new Vector3(
                .10f,
                2.25f,
                .10f
            ),
            canopy
        );
    }

    // =====================================================
    // FENETRES
    // =====================================================

    static void UpgradeExistingWindows()
    {
        Renderer[] renderers =
            Object.FindObjectsByType<
                Renderer
            >(
                FindObjectsSortMode.None
            );

        int count = 0;

        foreach (
            Renderer r
            in renderers)
        {
            if (
                r.gameObject.name
                != "Vitre"
            )
                continue;

            r.sharedMaterial =
                glass;

            count++;
        }

        Debug.Log(
            "Vitres améliorées : "
            + count
        );
    }

    static void CreateWindowSills()
    {
        GameObject group =
            NewGroup(
                "Appuis_Fenetres"
            );

        GameObject decor =
            GameObject.Find(
                "Decor_Step13"
            );

        if (decor == null)
        {
            Debug.LogWarning(
                "Decor_Step13 absent : appuis de fenêtres non générés."
            );

            return;
        }

        Renderer[] renderers =
            decor.GetComponentsInChildren<
                Renderer
            >(true);

        int number = 0;

        foreach (
            Renderer r
            in renderers)
        {
            if (
                r.gameObject.name
                != "Vitre"
            )
                continue;

            Bounds b =
                r.bounds;

            bool wideX =
                b.size.x >= b.size.z;

            Vector3 scale;

            if (wideX)
            {
                scale =
                    new Vector3(
                        b.size.x + .30f,
                        .10f,
                        .34f
                    );
            }
            else
            {
                scale =
                    new Vector3(
                        .34f,
                        .10f,
                        b.size.z + .30f
                    );
            }

            CreateBoxWorld(
                "Appui_" + (++number),
                group.transform,
                new Vector3(
                    b.center.x,
                    b.min.y - .07f,
                    b.center.z
                ),
                scale,
                facade
            );
        }

        Debug.Log(
            "Appuis fenêtres : "
            + number
        );
    }

    // =====================================================
    // RELIEF EXTERIEUR
    // =====================================================

    static void CreateLandscaping()
    {
        GameObject group =
            NewGroup(
                "Relief_Exterieur"
            );

        CreateMound(
            group,
            8f,27f,
            5f,2.2f
        );

        CreateMound(
            group,
            19f,40f,
            7f,2.5f
        );

        CreateMound(
            group,
            53f,38f,
            6f,2.4f
        );

        CreateMound(
            group,
            59f,20f,
            4.5f,2f
        );

        CreateMound(
            group,
            37f,6f,
            6f,2.2f
        );

        CreateShrubCluster(
            group,
            13f,31f
        );

        CreateShrubCluster(
            group,
            28f,40f
        );

        CreateShrubCluster(
            group,
            47f,40f
        );

        CreateShrubCluster(
            group,
            60f,28f
        );

        CreateRockCluster(
            group,
            7f,22f
        );

        CreateRockCluster(
            group,
            55f,33f
        );
    }

    static void CreateMound(
        GameObject parent,
        float mapX,
        float mapZ,
        float width,
        float depth)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere
            );

        obj.name =
            "Butte_Vegetale";

        obj.transform.SetParent(
            parent.transform
        );

        obj.transform.position =
            new Vector3(
                X(mapX),
                -.38f,
                Z(mapZ)
            );

        obj.transform.localScale =
            new Vector3(
                width,
                1.05f,
                depth
            );

        obj.GetComponent<Renderer>()
            .sharedMaterial = grass;

        RemoveCollider(obj);
    }

    static void CreateShrubCluster(
        GameObject parent,
        float mapX,
        float mapZ)
    {
        for (int i=0; i<5; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 5f;

            GameObject obj =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            obj.name =
                "Arbuste";

            obj.transform.SetParent(
                parent.transform
            );

            obj.transform.position =
                new Vector3(
                    X(mapX)
                    + Mathf.Cos(angle)
                    * .85f,

                    .48f,

                    Z(mapZ)
                    + Mathf.Sin(angle)
                    * .65f
                );

            obj.transform.localScale =
                new Vector3(
                    1.1f,
                    .85f,
                    1.1f
                );

            obj.GetComponent<Renderer>()
                .sharedMaterial =
                    shrub;

            RemoveCollider(obj);
        }
    }

    static void CreateRockCluster(
        GameObject parent,
        float mapX,
        float mapZ)
    {
        for (int i=0; i<4; i++)
        {
            GameObject obj =
                GameObject.CreatePrimitive(
                    PrimitiveType.Sphere
                );

            obj.name =
                "Pierre";

            obj.transform.SetParent(
                parent.transform
            );

            obj.transform.position =
                new Vector3(
                    X(mapX)
                    + i*.45f,

                    .18f,

                    Z(mapZ)
                    + Mathf.Sin(i*2f)
                    *.35f
                );

            obj.transform.localScale =
                new Vector3(
                    .65f + i*.08f,
                    .32f,
                    .55f
                );

            obj.transform.rotation =
                Quaternion.Euler(
                    0f,
                    i*37f,
                    i*6f
                );

            obj.GetComponent<Renderer>()
                .sharedMaterial =
                    stone;

            RemoveCollider(obj);
        }
    }

    // =====================================================
    // ENVIRONNEMENT
    // =====================================================

    static void ImproveEnvironment()
    {
        RenderSettings.ambientIntensity =
            .92f;

        RenderSettings.reflectionIntensity =
            .88f;

        RenderSettings.fogColor =
            new Color(
                .69f,.75f,.80f
            );

        RenderSettings.fogStartDistance =
            85f;

        RenderSettings.fogEndDistance =
            170f;

        if (RenderSettings.skybox != null)
        {
            Material sky =
                RenderSettings.skybox;

            if (
                sky.HasProperty(
                    "_Exposure"
                )
            )
            {
                sky.SetFloat(
                    "_Exposure",
                    1.22f
                );
            }

            if (
                sky.HasProperty(
                    "_AtmosphereThickness"
                )
            )
            {
                sky.SetFloat(
                    "_AtmosphereThickness",
                    1.10f
                );
            }

            EditorUtility.SetDirty(
                sky
            );
        }

        DynamicGI.UpdateEnvironment();
    }

    // =====================================================
    // VERRE
    // =====================================================

    static Material MakeTransparentGlass()
    {
        string path =
            "Assets/Materials/ARCH_Glass.mat";

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

        Color color =
            new Color(
                .22f,
                .52f,
                .72f,
                .42f
            );

        if (
            mat.shader.name.Contains(
                "Universal Render Pipeline"
            )
        )
        {
            if (
                mat.HasProperty("_BaseColor")
            )
                mat.SetColor(
                    "_BaseColor",
                    color
                );

            if (
                mat.HasProperty("_Surface")
            )
                mat.SetFloat(
                    "_Surface",
                    1f
                );

            if (
                mat.HasProperty("_Blend")
            )
                mat.SetFloat(
                    "_Blend",
                    0f
                );

            mat.SetOverrideTag(
                "RenderType",
                "Transparent"
            );

            mat.renderQueue =
                (int)RenderQueue.Transparent;

            mat.SetInt(
                "_SrcBlend",
                (int)BlendMode.SrcAlpha
            );

            mat.SetInt(
                "_DstBlend",
                (int)BlendMode.OneMinusSrcAlpha
            );

            mat.SetInt(
                "_ZWrite",
                0
            );

            mat.EnableKeyword(
                "_SURFACE_TYPE_TRANSPARENT"
            );
        }
        else
        {
            if (
                mat.HasProperty("_Color")
            )
                mat.SetColor(
                    "_Color",
                    color
                );

            mat.SetOverrideTag(
                "RenderType",
                "Transparent"
            );

            mat.SetInt(
                "_SrcBlend",
                (int)BlendMode.SrcAlpha
            );

            mat.SetInt(
                "_DstBlend",
                (int)BlendMode.OneMinusSrcAlpha
            );

            mat.SetInt(
                "_ZWrite",
                0
            );

            mat.DisableKeyword(
                "_ALPHATEST_ON"
            );

            mat.EnableKeyword(
                "_ALPHABLEND_ON"
            );

            mat.renderQueue = 3000;
        }

        if (
            mat.HasProperty(
                "_Metallic"
            )
        )
            mat.SetFloat(
                "_Metallic",
                .08f
            );

        if (
            mat.HasProperty(
                "_Smoothness"
            )
        )
            mat.SetFloat(
                "_Smoothness",
                .88f
            );

        if (
            mat.HasProperty(
                "_Glossiness"
            )
        )
            mat.SetFloat(
                "_Glossiness",
                .88f
            );

        EditorUtility.SetDirty(
            mat
        );

        return mat;
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
            r.sharedMaterial =
                material;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateBoxWorld(
        string name,
        Transform parent,
        Vector3 position,
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
            position;

        obj.transform.localScale =
            scale;

        Renderer r =
            obj.GetComponent<Renderer>();

        if (r != null)
            r.sharedMaterial =
                material;

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

        if (
            mat.HasProperty(
                "_BaseColor"
            )
        )
            mat.SetColor(
                "_BaseColor",
                color
            );

        if (
            mat.HasProperty(
                "_Color"
            )
        )
            mat.SetColor(
                "_Color",
                color
            );

        if (
            mat.HasProperty(
                "_Metallic"
            )
        )
            mat.SetFloat(
                "_Metallic",
                metallic
            );

        if (
            mat.HasProperty(
                "_Smoothness"
            )
        )
            mat.SetFloat(
                "_Smoothness",
                smoothness
            );

        if (
            mat.HasProperty(
                "_Glossiness"
            )
        )
            mat.SetFloat(
                "_Glossiness",
                smoothness
            );

        EditorUtility.SetDirty(mat);

        return mat;
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep14Interiors
{
    static GameObject root;

    static Material wood;
    static Material darkWood;
    static Material metal;
    static Material white;
    static Material blue;
    static Material green;
    static Material red;
    static Material yellow;
    static Material black;
    static Material bookRed;
    static Material bookBlue;
    static Material bookGreen;
    static Material screen;
    static Material curtain;
    static Material lightMat;

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
            GameObject.Find("Decor_Step14_Interieurs");

        if (old != null)
            Object.DestroyImmediate(old);

        root =
            new GameObject(
                "Decor_Step14_Interieurs"
            );

        CreateMaterials();

        CreateVieScolaire();
        CreateCDI();
        CreateGymnase();
        CreateAuditorium();
        CreateArtsMusique();
        CreateSelf();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 14 TERMINEE - INTERIEURS ENRICHIS"
        );
    }

    // =====================================================
    // VIE SCOLAIRE
    // =====================================================

    static void CreateVieScolaire()
    {
        GameObject g =
            NewGroup("Vie_Scolaire_Decor");

        // comptoir d'accueil
        CreateBoxWorld(
            "Comptoir",
            g.transform,
            new Vector3(
                X(22.7f),
                .65f,
                Z(28.0f)
            ),
            new Vector3(
                3.2f,
                1.15f,
                .65f
            ),
            darkWood
        );

        CreateBoxWorld(
            "Dessus_Comptoir",
            g.transform,
            new Vector3(
                X(22.7f),
                1.25f,
                Z(28.0f)
            ),
            new Vector3(
                3.35f,
                .12f,
                .82f
            ),
            wood
        );

        // deux bureaux
        CreateDesk(
            g,
            X(24.2f),
            Z(28.4f),
            90f
        );

        CreateDesk(
            g,
            X(24.2f),
            Z(27.5f),
            90f
        );

        // écrans
        CreateMonitor(
            g,
            X(24.0f),
            .95f,
            Z(28.4f),
            90f
        );

        CreateMonitor(
            g,
            X(24.0f),
            .95f,
            Z(27.5f),
            90f
        );

        // casiers muraux
        for (int i = 0; i < 5; i++)
        {
            CreateBoxWorld(
                "Casier_" + i,
                g.transform,
                new Vector3(
                    X(25.35f),
                    .75f,
                    Z(27.35f + i * .38f)
                ),
                new Vector3(
                    .45f,
                    1.3f,
                    .58f
                ),
                blue
            );
        }

        AddCeilingLight(
            g,
            X(23.5f),
            2.55f,
            Z(28.2f),
            4.0f,
            2.1f
        );
    }

    // =====================================================
    // CDI
    // =====================================================

    static void CreateCDI()
    {
        GameObject g =
            NewGroup("CDI_Decor");

        float floorY = 3.2f;

        // rayonnages au fond
        for (int i = 0; i < 4; i++)
        {
            float x =
                X(27.2f + i * 2.2f);

            CreateBookshelf(
                g,
                x,
                floorY,
                Z(21.45f),
                0f
            );
        }

        // rayonnages latéraux
        CreateBookshelf(
            g,
            X(26.5f),
            floorY,
            Z(22.6f),
            90f
        );

        CreateBookshelf(
            g,
            X(35.4f),
            floorY,
            Z(22.6f),
            90f
        );

        // tables centrales
        CreateReadingTable(
            g,
            X(29.5f),
            floorY,
            Z(22.4f)
        );

        CreateReadingTable(
            g,
            X(33.0f),
            floorY,
            Z(22.4f)
        );

        // ordinateur CDI
        CreateDesk(
            g,
            X(34.5f),
            Z(23.2f),
            180f,
            floorY
        );

        CreateMonitor(
            g,
            X(34.5f),
            floorY + .95f,
            Z(23.1f),
            180f
        );

        AddCeilingLight(
            g,
            X(29.0f),
            floorY + 2.55f,
            Z(22.4f),
            5f,
            1.8f
        );

        AddCeilingLight(
            g,
            X(33.5f),
            floorY + 2.55f,
            Z(22.4f),
            5f,
            1.8f
        );
    }

    // =====================================================
    // GYMNASE
    // =====================================================

    static void CreateGymnase()
    {
        GameObject g =
            NewGroup("Gymnase_Decor");

        // ligne centrale
        CreateBoxWorld(
            "Ligne_Centrale",
            g.transform,
            new Vector3(
                X(12f),
                .025f,
                Z(37.5f)
            ),
            new Vector3(
                .08f,
                .035f,
                11.0f
            ),
            white
        );

        // lignes latérales
        CreateBoxWorld(
            "Ligne_Nord",
            g.transform,
            new Vector3(
                X(12f),
                .026f,
                Z(35.0f)
            ),
            new Vector3(
                15f,
                .035f,
                .08f
            ),
            white
        );

        CreateBoxWorld(
            "Ligne_Sud",
            g.transform,
            new Vector3(
                X(12f),
                .026f,
                Z(40.0f)
            ),
            new Vector3(
                15f,
                .035f,
                .08f
            ),
            white
        );

        // cercle central simplifié
        for (int i = 0; i < 16; i++)
        {
            float angle =
                i * Mathf.PI * 2f / 16f;

            float cx =
                X(12f)
                + Mathf.Cos(angle) * 1.8f;

            float cz =
                Z(37.5f)
                + Mathf.Sin(angle) * 1.8f;

            GameObject marker =
                CreateBoxWorld(
                    "Cercle_" + i,
                    g.transform,
                    new Vector3(
                        cx,
                        .028f,
                        cz
                    ),
                    new Vector3(
                        .45f,
                        .035f,
                        .08f
                    ),
                    white
                );

            marker.transform.rotation =
                Quaternion.Euler(
                    0f,
                    -i * 22.5f,
                    0f
                );
        }

        CreateBasket(
            g,
            X(8.5f),
            Z(37.5f),
            90f
        );

        CreateBasket(
            g,
            X(15.5f),
            Z(37.5f),
            -90f
        );

        AddCeilingLight(
            g,
            X(10f),
            4.2f,
            Z(37.5f),
            7f,
            2.3f
        );

        AddCeilingLight(
            g,
            X(14f),
            4.2f,
            Z(37.5f),
            7f,
            2.3f
        );
    }

    // =====================================================
    // AUDITORIUM
    // =====================================================

    static void CreateAuditorium()
    {
        GameObject g =
            NewGroup("Auditorium_Decor");

        // rideau de scène
        for (int i = 0; i < 8; i++)
        {
            CreateBoxWorld(
                "Rideau_" + i,
                g.transform,
                new Vector3(
                    X(51.3f),
                    1.7f,
                    Z(19.8f + i * .55f)
                ),
                new Vector3(
                    .12f,
                    3.2f,
                    .75f
                ),
                curtain
            );
        }

        // écran / mur de projection
        CreateBoxWorld(
            "Ecran_Projection",
            g.transform,
            new Vector3(
                X(51.15f),
                2.0f,
                Z(22f)
            ),
            new Vector3(
                .08f,
                2.7f,
                4.4f
            ),
            white
        );

        // pupitre
        CreateBoxWorld(
            "Pupitre",
            g.transform,
            new Vector3(
                X(49.4f),
                .65f,
                Z(22.0f)
            ),
            new Vector3(
                .65f,
                1.25f,
                .75f
            ),
            darkWood
        );

        // spots scène
        AddSpotLight(
            g,
            X(48.3f),
            3.5f,
            Z(20.3f),
            new Vector3(
                X(50.5f),
                1.2f,
                Z(21.5f)
            )
        );

        AddSpotLight(
            g,
            X(48.3f),
            3.5f,
            Z(23.7f),
            new Vector3(
                X(50.5f),
                1.2f,
                Z(22.5f)
            )
        );
    }

    // =====================================================
    // ARTS ET MUSIQUE
    // =====================================================

    static void CreateArtsMusique()
    {
        GameObject g =
            NewGroup("Arts_Musique_Decor");

        float floorY = 3.2f;

        // peinture : chevalets supplémentaires
        for (int i = 0; i < 3; i++)
        {
            CreateEasel(
                g,
                X(16.3f + i * .8f),
                floorY,
                Z(14.4f)
            );
        }

        // palettes / blocs de couleurs
        Material[] colors =
        {
            red,
            yellow,
            green,
            blue
        };

        for (int i = 0; i < 4; i++)
        {
            CreateBoxWorld(
                "Peinture_" + i,
                g.transform,
                new Vector3(
                    X(18.0f + i * .28f),
                    floorY + .82f,
                    Z(13.4f)
                ),
                new Vector3(
                    .35f,
                    .20f,
                    .35f
                ),
                colors[i]
            );
        }

        // musique : clavier
        CreateBoxWorld(
            "Clavier",
            g.transform,
            new Vector3(
                X(12.5f),
                floorY + .85f,
                Z(14f)
            ),
            new Vector3(
                2.0f,
                .18f,
                .65f
            ),
            black
        );

        for (int i = 0; i < 12; i++)
        {
            CreateBoxWorld(
                "Touche_" + i,
                g.transform,
                new Vector3(
                    X(12.08f + i * .075f),
                    floorY + .96f,
                    Z(14f)
                ),
                new Vector3(
                    .12f,
                    .035f,
                    .52f
                ),
                white
            );
        }

        // batterie simplifiée
        CreateCylinderWorld(
            "Grosse_Caisse",
            g.transform,
            new Vector3(
                X(11.3f),
                floorY + .55f,
                Z(15.2f)
            ),
            .75f,
            .7f,
            red
        );

        CreateCylinderWorld(
            "Tom",
            g.transform,
            new Vector3(
                X(12.1f),
                floorY + .75f,
                Z(15.2f)
            ),
            .45f,
            .45f,
            blue
        );

        AddCeilingLight(
            g,
            X(13f),
            floorY + 2.55f,
            Z(14.5f),
            5f,
            1.8f
        );

        AddCeilingLight(
            g,
            X(17.5f),
            floorY + 2.55f,
            Z(14.5f),
            5f,
            1.8f
        );
    }

    // =====================================================
    // SELF
    // =====================================================

    static void CreateSelf()
    {
        GameObject g =
            NewGroup("Self_Decor");

        // comptoir de service
        CreateBoxWorld(
            "Comptoir_Service",
            g.transform,
            new Vector3(
                X(56.5f),
                .70f,
                Z(15.1f)
            ),
            new Vector3(
                7.2f,
                1.15f,
                .75f
            ),
            metal
        );

        CreateBoxWorld(
            "Dessus_Comptoir",
            g.transform,
            new Vector3(
                X(56.5f),
                1.30f,
                Z(15.1f)
            ),
            new Vector3(
                7.4f,
                .12f,
                .90f
            ),
            white
        );

        // plateaux de nourriture
        Material[] food =
        {
            green,
            red,
            yellow,
            bookGreen
        };

        for (int i = 0; i < 4; i++)
        {
            CreateBoxWorld(
                "Bac_Repas_" + i,
                g.transform,
                new Vector3(
                    X(54.5f + i * .75f),
                    1.43f,
                    Z(15.1f)
                ),
                new Vector3(
                    1.15f,
                    .12f,
                    .58f
                ),
                food[i]
            );
        }

        // distributeur
        CreateBoxWorld(
            "Distributeur",
            g.transform,
            new Vector3(
                X(58.3f),
                1.15f,
                Z(16.2f)
            ),
            new Vector3(
                1.0f,
                2.2f,
                .75f
            ),
            black
        );

        CreateBoxWorld(
            "Ecran_Distributeur",
            g.transform,
            new Vector3(
                X(58.05f),
                1.5f,
                Z(16.2f)
            ),
            new Vector3(
                .05f,
                .55f,
                .43f
            ),
            screen
        );

        AddCeilingLight(
            g,
            X(52f),
            2.6f,
            Z(15.5f),
            5f,
            1.8f
        );

        AddCeilingLight(
            g,
            X(56f),
            2.6f,
            Z(15.5f),
            5f,
            1.8f
        );
    }

    // =====================================================
    // MEUBLES
    // =====================================================

    static void CreateBookshelf(
        GameObject parent,
        float worldX,
        float floorY,
        float worldZ,
        float rotation)
    {
        GameObject shelf =
            new GameObject("Bibliotheque");

        shelf.transform.SetParent(
            parent.transform
        );

        shelf.transform.position =
            new Vector3(
                worldX,
                floorY,
                worldZ
            );

        shelf.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Fond",
            shelf.transform,
            new Vector3(
                0f,
                1.15f,
                0f
            ),
            new Vector3(
                1.7f,
                2.25f,
                .20f
            ),
            darkWood
        );

        for (int y = 0; y < 4; y++)
        {
            CreateBox(
                "Etagere_" + y,
                shelf.transform,
                new Vector3(
                    0f,
                    .35f + y * .55f,
                    -.16f
                ),
                new Vector3(
                    1.85f,
                    .08f,
                    .52f
                ),
                wood
            );

            for (int b = 0; b < 7; b++)
            {
                Material m;

                switch ((b + y) % 3)
                {
                    case 0:
                        m = bookRed;
                        break;

                    case 1:
                        m = bookBlue;
                        break;

                    default:
                        m = bookGreen;
                        break;
                }

                CreateBox(
                    "Livre",
                    shelf.transform,
                    new Vector3(
                        -.70f + b * .22f,
                        .56f + y * .55f,
                        -.32f
                    ),
                    new Vector3(
                        .15f,
                        .38f,
                        .20f
                    ),
                    m
                );
            }
        }
    }

    static void CreateReadingTable(
        GameObject parent,
        float x,
        float floorY,
        float z)
    {
        CreateBoxWorld(
            "Table_CDI",
            parent.transform,
            new Vector3(
                x,
                floorY + .74f,
                z
            ),
            new Vector3(
                2.7f,
                .12f,
                1.25f
            ),
            wood
        );

        float[,] legs =
        {
            {-.95f,-.40f},
            {.95f,-.40f},
            {-.95f,.40f},
            {.95f,.40f}
        };

        for (int i=0; i<4; i++)
        {
            CreateBoxWorld(
                "Pied_Table",
                parent.transform,
                new Vector3(
                    x + legs[i,0],
                    floorY + .37f,
                    z + legs[i,1]
                ),
                new Vector3(
                    .10f,
                    .72f,
                    .10f
                ),
                metal
            );
        }

        CreateChair(
            parent,
            x,
            floorY,
            z - 1.0f,
            0f
        );

        CreateChair(
            parent,
            x,
            floorY,
            z + 1.0f,
            180f
        );
    }

    static void CreateDesk(
        GameObject parent,
        float x,
        float z,
        float rotation,
        float floorY = 0f)
    {
        GameObject desk =
            new GameObject("Bureau");

        desk.transform.SetParent(
            parent.transform
        );

        desk.transform.position =
            new Vector3(
                x,
                floorY,
                z
            );

        desk.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Plateau",
            desk.transform,
            new Vector3(
                0f,
                .78f,
                0f
            ),
            new Vector3(
                1.7f,
                .12f,
                .8f
            ),
            wood
        );

        CreateBox(
            "Pied1",
            desk.transform,
            new Vector3(
                -.65f,
                .39f,
                0f
            ),
            new Vector3(
                .10f,
                .76f,
                .55f
            ),
            metal
        );

        CreateBox(
            "Pied2",
            desk.transform,
            new Vector3(
                .65f,
                .39f,
                0f
            ),
            new Vector3(
                .10f,
                .76f,
                .55f
            ),
            metal
        );
    }

    static void CreateMonitor(
        GameObject parent,
        float x,
        float y,
        float z,
        float rotation)
    {
        GameObject monitor =
            new GameObject("Ecran");

        monitor.transform.SetParent(
            parent.transform
        );

        monitor.transform.position =
            new Vector3(
                x,
                y,
                z
            );

        monitor.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Moniteur",
            monitor.transform,
            Vector3.zero,
            new Vector3(
                .75f,
                .48f,
                .08f
            ),
            black
        );

        CreateBox(
            "Surface_Ecran",
            monitor.transform,
            new Vector3(
                0f,
                0f,
                -.05f
            ),
            new Vector3(
                .64f,
                .37f,
                .02f
            ),
            screen
        );

        CreateBox(
            "Pied",
            monitor.transform,
            new Vector3(
                0f,
                -.35f,
                .05f
            ),
            new Vector3(
                .10f,
                .30f,
                .10f
            ),
            metal
        );
    }

    static void CreateChair(
        GameObject parent,
        float x,
        float floorY,
        float z,
        float rotation)
    {
        GameObject chair =
            new GameObject("Chaise");

        chair.transform.SetParent(
            parent.transform
        );

        chair.transform.position =
            new Vector3(
                x,
                floorY,
                z
            );

        chair.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Assise",
            chair.transform,
            new Vector3(
                0f,
                .48f,
                0f
            ),
            new Vector3(
                .65f,
                .10f,
                .65f
            ),
            blue
        );

        CreateBox(
            "Dossier",
            chair.transform,
            new Vector3(
                0f,
                .90f,
                .28f
            ),
            new Vector3(
                .65f,
                .72f,
                .10f
            ),
            blue
        );
    }

    static void CreateBasket(
        GameObject parent,
        float x,
        float z,
        float rotation)
    {
        GameObject basket =
            new GameObject("Panier_Basket");

        basket.transform.SetParent(
            parent.transform
        );

        basket.transform.position =
            new Vector3(
                x,
                0f,
                z
            );

        basket.transform.rotation =
            Quaternion.Euler(
                0f,
                rotation,
                0f
            );

        CreateBox(
            "Poteau",
            basket.transform,
            new Vector3(
                0f,
                1.7f,
                0f
            ),
            new Vector3(
                .14f,
                3.4f,
                .14f
            ),
            metal
        );

        CreateBox(
            "Panneau",
            basket.transform,
            new Vector3(
                0f,
                3.0f,
                -.55f
            ),
            new Vector3(
                1.8f,
                1.1f,
                .10f
            ),
            white
        );

        CreateCylinder(
            "Cercle",
            basket.transform,
            new Vector3(
                0f,
                2.65f,
                -1.05f
            ),
            .65f,
            .08f,
            red
        );
    }

    static void CreateEasel(
        GameObject parent,
        float x,
        float floorY,
        float z)
    {
        GameObject e =
            new GameObject("Chevalet");

        e.transform.SetParent(
            parent.transform
        );

        e.transform.position =
            new Vector3(
                x,
                floorY,
                z
            );

        CreateBox(
            "Pied_G",
            e.transform,
            new Vector3(
                -.25f,
                .75f,
                0f
            ),
            new Vector3(
                .08f,
                1.5f,
                .08f
            ),
            wood
        );

        CreateBox(
            "Pied_D",
            e.transform,
            new Vector3(
                .25f,
                .75f,
                0f
            ),
            new Vector3(
                .08f,
                1.5f,
                .08f
            ),
            wood
        );

        CreateBox(
            "Toile",
            e.transform,
            new Vector3(
                0f,
                1.1f,
                -.03f
            ),
            new Vector3(
                .85f,
                .95f,
                .06f
            ),
            white
        );
    }

    // =====================================================
    // ECLAIRAGE
    // =====================================================

    static void AddCeilingLight(
        GameObject parent,
        float x,
        float y,
        float z,
        float range,
        float intensity)
    {
        GameObject fixture =
            CreateBoxWorld(
                "Plafonnier",
                parent.transform,
                new Vector3(
                    x,
                    y,
                    z
                ),
                new Vector3(
                    1.0f,
                    .08f,
                    .35f
                ),
                lightMat
            );

        GameObject lightObject =
            new GameObject(
                "Lumiere"
            );

        lightObject.transform.SetParent(
            fixture.transform
        );

        lightObject.transform.localPosition =
            new Vector3(
                0f,
                -.12f,
                0f
            );

        Light light =
            lightObject.AddComponent<Light>();

        light.type =
            LightType.Point;

        light.color =
            new Color(
                1f,
                .91f,
                .75f
            );

        light.range =
            range;

        light.intensity =
            intensity;

        light.shadows =
            LightShadows.Soft;
    }

    static void AddSpotLight(
        GameObject parent,
        float x,
        float y,
        float z,
        Vector3 target)
    {
        GameObject obj =
            new GameObject("Spot");

        obj.transform.SetParent(
            parent.transform
        );

        obj.transform.position =
            new Vector3(
                x,
                y,
                z
            );

        obj.transform.LookAt(target);

        Light light =
            obj.AddComponent<Light>();

        light.type =
            LightType.Spot;

        light.color =
            new Color(
                1f,
                .78f,
                .48f
            );

        light.range = 12f;
        light.intensity = 4f;
        light.spotAngle = 35f;
        light.shadows = LightShadows.Soft;
    }

    // =====================================================
    // GROUPES
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

    // =====================================================
    // CREATION PRIMITIVES
    // =====================================================

    static GameObject CreateBox(
        string name,
        Transform parent,
        Vector3 localPosition,
        Vector3 scale,
        Material mat)
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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = mat;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateBoxWorld(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material mat)
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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = mat;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateCylinder(
        string name,
        Transform parent,
        Vector3 localPosition,
        float diameter,
        float height,
        Material mat)
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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = mat;

        RemoveCollider(obj);

        return obj;
    }

    static GameObject CreateCylinderWorld(
        string name,
        Transform parent,
        Vector3 position,
        float diameter,
        float height,
        Material mat)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder
            );

        obj.name = name;

        obj.transform.SetParent(
            parent
        );

        obj.transform.position =
            position;

        obj.transform.localScale =
            new Vector3(
                diameter,
                height / 2f,
                diameter
            );

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = mat;

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

    // =====================================================
    // MATERIAUX
    // =====================================================

    static void CreateMaterials()
    {
        wood =
            MakeMaterial(
                "INT_Wood",
                new Color(
                    .42f,.23f,.09f,1f
                ),
                0f,
                .30f
            );

        darkWood =
            MakeMaterial(
                "INT_DarkWood",
                new Color(
                    .23f,.11f,.045f,1f
                ),
                0f,
                .25f
            );

        metal =
            MakeMaterial(
                "INT_Metal",
                new Color(
                    .20f,.22f,.24f,1f
                ),
                .65f,
                .42f
            );

        white =
            MakeMaterial(
                "INT_White",
                new Color(
                    .92f,.91f,.86f,1f
                ),
                0f,
                .28f
            );

        blue =
            MakeMaterial(
                "INT_Blue",
                new Color(
                    .15f,.35f,.62f,1f
                ),
                0f,
                .24f
            );

        green =
            MakeMaterial(
                "INT_Green",
                new Color(
                    .18f,.48f,.20f,1f
                ),
                0f,
                .20f
            );

        red =
            MakeMaterial(
                "INT_Red",
                new Color(
                    .68f,.10f,.08f,1f
                ),
                0f,
                .25f
            );

        yellow =
            MakeMaterial(
                "INT_Yellow",
                new Color(
                    .92f,.64f,.08f,1f
                ),
                0f,
                .25f
            );

        black =
            MakeMaterial(
                "INT_Black",
                new Color(
                    .035f,.04f,.045f,1f
                ),
                .15f,
                .35f
            );

        bookRed =
            MakeMaterial(
                "INT_BookRed",
                new Color(
                    .55f,.08f,.08f,1f
                ),
                0f,
                .18f
            );

        bookBlue =
            MakeMaterial(
                "INT_BookBlue",
                new Color(
                    .08f,.22f,.50f,1f
                ),
                0f,
                .18f
            );

        bookGreen =
            MakeMaterial(
                "INT_BookGreen",
                new Color(
                    .12f,.40f,.16f,1f
                ),
                0f,
                .18f
            );

        screen =
            MakeMaterial(
                "INT_Screen",
                new Color(
                    .12f,.40f,.62f,1f
                ),
                0f,
                .75f
            );

        curtain =
            MakeMaterial(
                "INT_Curtain",
                new Color(
                    .45f,.025f,.035f,1f
                ),
                0f,
                .12f
            );

        lightMat =
            MakeMaterial(
                "INT_Light",
                new Color(
                    1f,.87f,.58f,1f
                ),
                0f,
                .70f
            );

        SetEmission(
            screen,
            new Color(
                .05f,.20f,.40f
            ) * 1.2f
        );

        SetEmission(
            lightMat,
            new Color(
                1f,.62f,.20f
            ) * 1.4f
        );
    }

    static void SetEmission(
        Material mat,
        Color color)
    {
        if (
            mat.HasProperty(
                "_EmissionColor"
            )
        )
        {
            mat.EnableKeyword(
                "_EMISSION"
            );

            mat.SetColor(
                "_EmissionColor",
                color
            );

            EditorUtility.SetDirty(mat);
        }
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

        mat.color = color;

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

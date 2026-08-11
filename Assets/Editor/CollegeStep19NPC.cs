using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep19NPC
{
    static GameObject root;

    static Material skin1;
    static Material skin2;
    static Material skin3;

    static Material hairDark;
    static Material hairBrown;
    static Material hairLight;

    static Material shirtBlue;
    static Material shirtRed;
    static Material shirtGreen;
    static Material shirtYellow;
    static Material shirtPurple;
    static Material shirtGrey;

    static Material trousersDark;
    static Material trousersBlue;

    static Material shoes;

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
        const string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        GameObject old =
            GameObject.Find(
                "NPC_Step19"
            );

        if (old != null)
            Object.DestroyImmediate(old);

        root =
            new GameObject(
                "NPC_Step19"
            );

        CreateMaterials();

        CreateCourtyardNPCs();
        CreateCDINPCs();
        CreateSelfNPCs();
        CreateAdultNPCs();

        int colliders =
            root.GetComponentsInChildren<
                Collider
            >(true).Length;

        CollegeNPC[] npcs =
            root.GetComponentsInChildren<
                CollegeNPC
            >(true);

        Debug.Log(
            "Personnages créés : "
            + npcs.Length
        );

        Debug.Log(
            "Colliders NPC : "
            + colliders
        );

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 19 TERMINEE - PERSONNAGES 3D ANIMES"
        );
    }

    // =====================================================
    // COUR
    // =====================================================

    static void CreateCourtyardNPCs()
    {
        GameObject group =
            NewGroup("Eleves_Cour");

        CreateNPC(
            group,
            "Eleve_Cour_01",
            new Vector3(
                X(27f),0f,Z(20f)
            ),
            0.92f,
            shirtBlue,
            skin1,
            hairDark,
            true,
            new Vector3[]
            {
                new Vector3(X(27f),0f,Z(20f)),
                new Vector3(X(31f),0f,Z(20f)),
                new Vector3(X(34f),0f,Z(22f)),
                new Vector3(X(31f),0f,Z(24f)),
                new Vector3(X(27f),0f,Z(23f))
            }
        );

        CreateNPC(
            group,
            "Eleve_Cour_02",
            new Vector3(
                X(37f),0f,Z(20f)
            ),
            0.96f,
            shirtRed,
            skin2,
            hairBrown,
            true,
            new Vector3[]
            {
                new Vector3(X(37f),0f,Z(20f)),
                new Vector3(X(39f),0f,Z(22f)),
                new Vector3(X(37f),0f,Z(25f)),
                new Vector3(X(34f),0f,Z(24f))
            }
        );

        CreateNPC(
            group,
            "Eleve_Cour_03",
            new Vector3(
                X(28f),0f,Z(26f)
            ),
            0.90f,
            shirtGreen,
            skin3,
            hairDark,
            true,
            new Vector3[]
            {
                new Vector3(X(28f),0f,Z(26f)),
                new Vector3(X(31f),0f,Z(25f)),
                new Vector3(X(34f),0f,Z(26f)),
                new Vector3(X(32f),0f,Z(27f))
            }
        );

        CreateNPC(
            group,
            "Eleve_Cour_04",
            new Vector3(
                X(40f),0f,Z(20f)
            ),
            0.94f,
            shirtYellow,
            skin1,
            hairLight,
            true,
            new Vector3[]
            {
                new Vector3(X(40f),0f,Z(20f)),
                new Vector3(X(39f),0f,Z(24f)),
                new Vector3(X(36f),0f,Z(26f)),
                new Vector3(X(35f),0f,Z(21f))
            }
        );

        CreateNPC(
            group,
            "Eleve_Cour_05",
            new Vector3(
                X(30f),0f,Z(22f)
            ),
            0.88f,
            shirtPurple,
            skin2,
            hairBrown,
            false,
            null
        );

        CreateNPC(
            group,
            "Eleve_Cour_06",
            new Vector3(
                X(30.8f),0f,Z(22.6f)
            ),
            0.91f,
            shirtGrey,
            skin3,
            hairDark,
            false,
            null
        );
    }

    // =====================================================
    // CDI
    // =====================================================

    static void CreateCDINPCs()
    {
        GameObject group =
            NewGroup("Eleves_CDI");

        float y = 3.2f;

        CreateNPC(
            group,
            "Eleve_CDI_01",
            new Vector3(
                X(29f),y,Z(22.4f)
            ),
            .88f,
            shirtBlue,
            skin2,
            hairBrown,
            false,
            null
        );

        CreateNPC(
            group,
            "Eleve_CDI_02",
            new Vector3(
                X(33f),y,Z(22.5f)
            ),
            .90f,
            shirtGreen,
            skin1,
            hairDark,
            false,
            null
        );
    }

    // =====================================================
    // SELF
    // =====================================================

    static void CreateSelfNPCs()
    {
        GameObject group =
            NewGroup("Eleves_Self");

        CreateNPC(
            group,
            "Eleve_Self_01",
            new Vector3(
                X(52.3f),0f,Z(15.7f)
            ),
            .90f,
            shirtRed,
            skin1,
            hairDark,
            true,
            new Vector3[]
            {
                new Vector3(
                    X(52.3f),0f,Z(15.7f)
                ),
                new Vector3(
                    X(54f),0f,Z(15.7f)
                ),
                new Vector3(
                    X(55.5f),0f,Z(16.2f)
                ),
                new Vector3(
                    X(53f),0f,Z(16.2f)
                )
            }
        );

        CreateNPC(
            group,
            "Eleve_Self_02",
            new Vector3(
                X(50.5f),0f,Z(15.5f)
            ),
            .92f,
            shirtYellow,
            skin3,
            hairLight,
            false,
            null
        );
    }

    // =====================================================
    // ADULTES
    // =====================================================

    static void CreateAdultNPCs()
    {
        GameObject group =
            NewGroup("Adultes");

        CreateNPC(
            group,
            "Adulte_Vie_Scolaire",
            new Vector3(
                X(24.3f),0f,Z(28.1f)
            ),
            1.05f,
            shirtGrey,
            skin2,
            hairDark,
            false,
            null
        );

        CreateNPC(
            group,
            "Adulte_Cour",
            new Vector3(
                X(25.5f),0f,Z(25f)
            ),
            1.08f,
            shirtBlue,
            skin1,
            hairBrown,
            true,
            new Vector3[]
            {
                new Vector3(
                    X(25.5f),0f,Z(25f)
                ),
                new Vector3(
                    X(27f),0f,Z(23f)
                ),
                new Vector3(
                    X(26f),0f,Z(20f)
                )
            }
        );
    }

    // =====================================================
    // CREATION PERSONNAGE
    // =====================================================

    static GameObject CreateNPC(
        GameObject parent,
        string name,
        Vector3 position,
        float scale,
        Material shirt,
        Material skin,
        Material hair,
        bool walking,
        Vector3[] waypoints)
    {
        GameObject npc =
            new GameObject(name);

        npc.transform.SetParent(
            parent.transform
        );

        npc.transform.position =
            position;

        npc.transform.localScale =
            Vector3.one * scale;

        // Corps
        GameObject body =
            CreateBox(
                "Corps",
                npc.transform,
                new Vector3(
                    0f,
                    1.25f,
                    0f
                ),
                new Vector3(
                    .60f,
                    .82f,
                    .34f
                ),
                shirt
            );

        // Tête
        GameObject head =
            CreateSphere(
                "Tete",
                npc.transform,
                new Vector3(
                    0f,
                    1.94f,
                    0f
                ),
                new Vector3(
                    .46f,
                    .50f,
                    .44f
                ),
                skin
            );

        // Cheveux
        CreateSphere(
            "Cheveux",
            head.transform,
            new Vector3(
                0f,
                .14f,
                .02f
            ),
            new Vector3(
                1.03f,
                .52f,
                1.03f
            ),
            hair
        );

        // yeux
        CreateSphere(
            "Oeil_G",
            head.transform,
            new Vector3(
                -.12f,
                .02f,
                .205f
            ),
            new Vector3(
                .055f,
                .055f,
                .035f
            ),
            hairDark
        );

        CreateSphere(
            "Oeil_D",
            head.transform,
            new Vector3(
                .12f,
                .02f,
                .205f
            ),
            new Vector3(
                .055f,
                .055f,
                .035f
            ),
            hairDark
        );

        // bras
        GameObject leftArm =
            CreateBox(
                "Bras_G",
                npc.transform,
                new Vector3(
                    -.39f,
                    1.25f,
                    0f
                ),
                new Vector3(
                    .18f,
                    .78f,
                    .20f
                ),
                shirt
            );

        GameObject rightArm =
            CreateBox(
                "Bras_D",
                npc.transform,
                new Vector3(
                    .39f,
                    1.25f,
                    0f
                ),
                new Vector3(
                    .18f,
                    .78f,
                    .20f
                ),
                shirt
            );

        // jambes
        GameObject leftLeg =
            CreateBox(
                "Jambe_G",
                npc.transform,
                new Vector3(
                    -.17f,
                    .55f,
                    0f
                ),
                new Vector3(
                    .22f,
                    .78f,
                    .25f
                ),
                trousersBlue
            );

        GameObject rightLeg =
            CreateBox(
                "Jambe_D",
                npc.transform,
                new Vector3(
                    .17f,
                    .55f,
                    0f
                ),
                new Vector3(
                    .22f,
                    .78f,
                    .25f
                ),
                trousersDark
            );

        // chaussures
        CreateBox(
            "Chaussure_G",
            leftLeg.transform,
            new Vector3(
                0f,
                -.52f,
                .08f
            ),
            new Vector3(
                1.10f,
                .20f,
                1.35f
            ),
            shoes
        );

        CreateBox(
            "Chaussure_D",
            rightLeg.transform,
            new Vector3(
                0f,
                -.52f,
                .08f
            ),
            new Vector3(
                1.10f,
                .20f,
                1.35f
            ),
            shoes
        );

        CollegeNPC controller =
            npc.AddComponent<CollegeNPC>();

        controller.walking =
            walking;

        controller.waypoints =
            waypoints;

        controller.speed =
            Random.Range(
                1.05f,
                1.45f
            );

        controller.waitTime =
            Random.Range(
                .6f,
                1.5f
            );

        controller.body =
            body.transform;

        controller.head =
            head.transform;

        controller.leftArm =
            leftArm.transform;

        controller.rightArm =
            rightArm.transform;

        controller.leftLeg =
            leftLeg.transform;

        controller.rightLeg =
            rightLeg.transform;

        return npc;
    }

    // =====================================================
    // MATERIAUX
    // =====================================================

    static void CreateMaterials()
    {
        skin1 =
            MakeMaterial(
                "NPC_Skin1",
                new Color(
                    .88f,.66f,.50f,1f
                )
            );

        skin2 =
            MakeMaterial(
                "NPC_Skin2",
                new Color(
                    .66f,.42f,.28f,1f
                )
            );

        skin3 =
            MakeMaterial(
                "NPC_Skin3",
                new Color(
                    .96f,.78f,.62f,1f
                )
            );

        hairDark =
            MakeMaterial(
                "NPC_HairDark",
                new Color(
                    .035f,.025f,.02f,1f
                )
            );

        hairBrown =
            MakeMaterial(
                "NPC_HairBrown",
                new Color(
                    .20f,.09f,.035f,1f
                )
            );

        hairLight =
            MakeMaterial(
                "NPC_HairLight",
                new Color(
                    .65f,.48f,.20f,1f
                )
            );

        shirtBlue =
            MakeMaterial(
                "NPC_Blue",
                new Color(
                    .10f,.32f,.64f,1f
                )
            );

        shirtRed =
            MakeMaterial(
                "NPC_Red",
                new Color(
                    .66f,.08f,.08f,1f
                )
            );

        shirtGreen =
            MakeMaterial(
                "NPC_Green",
                new Color(
                    .10f,.43f,.20f,1f
                )
            );

        shirtYellow =
            MakeMaterial(
                "NPC_Yellow",
                new Color(
                    .88f,.58f,.07f,1f
                )
            );

        shirtPurple =
            MakeMaterial(
                "NPC_Purple",
                new Color(
                    .43f,.18f,.55f,1f
                )
            );

        shirtGrey =
            MakeMaterial(
                "NPC_Grey",
                new Color(
                    .30f,.33f,.36f,1f
                )
            );

        trousersDark =
            MakeMaterial(
                "NPC_TrousersDark",
                new Color(
                    .08f,.09f,.11f,1f
                )
            );

        trousersBlue =
            MakeMaterial(
                "NPC_TrousersBlue",
                new Color(
                    .08f,.16f,.28f,1f
                )
            );

        shoes =
            MakeMaterial(
                "NPC_Shoes",
                new Color(
                    .025f,.025f,.025f,1f
                )
            );
    }

    static Material MakeMaterial(
        string name,
        Color color)
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
                "_Smoothness"
            )
        )
            mat.SetFloat(
                "_Smoothness",
                .22f
            );

        if (
            mat.HasProperty(
                "_Glossiness"
            )
        )
            mat.SetFloat(
                "_Glossiness",
                .22f
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
        GameObject group =
            new GameObject(name);

        group.transform.SetParent(
            root.transform
        );

        return group;
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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial =
                material;

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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial =
                material;

        RemoveCollider(obj);

        return obj;
    }

    static void RemoveCollider(
        GameObject obj)
    {
        Collider collider =
            obj.GetComponent<Collider>();

        if (collider != null)
            Object.DestroyImmediate(
                collider
            );
    }
}

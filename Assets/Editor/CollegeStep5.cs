using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CollegeStep5
{
    const float CELL = 2f;
    const float FLOOR_HEIGHT = 3.2f;
    const float WALL_HEIGHT = 3f;
    const float WALL = 0.18f;

    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        Scene scene =
            EditorSceneManager.OpenScene(scenePath);

        // -----------------------------------------
        // Nettoyage / remplacement des blocs pleins
        // -----------------------------------------

        DestroyIfExists(
            "[SPECIAL] Vie scolaire (vie-scolaire)"
        );

        DestroyIfExists(
            "[R+1] CDI (cdi)"
        );

        DestroyIfExists(
            "[ESCALIER] escaliers-centre"
        );

        DestroyIfExists(
            "Playable_Zone_Step5"
        );

        GameObject root =
            new GameObject("Playable_Zone_Step5");

        // -----------------------------------------
        // Matériaux
        // -----------------------------------------

        Material walls =
            GetMaterial(
                "Step5_Walls",
                new Color(0.86f, 0.86f, 0.82f)
            );

        Material floorRdc =
            GetMaterial(
                "Step5_Floor_RDC",
                new Color(0.55f, 0.56f, 0.58f)
            );

        Material floorR1 =
            GetMaterial(
                "Step5_Floor_R1",
                new Color(0.72f, 0.64f, 0.42f)
            );

        Material stairsMat =
            GetMaterial(
                "Step5_Stairs",
                new Color(0.42f, 0.63f, 0.77f)
            );

        Material doorMat =
            GetMaterial(
                "Step5_Doors",
                new Color(0.35f, 0.17f, 0.07f)
            );

        Material railMat =
            GetMaterial(
                "Step5_Rails",
                new Color(0.25f, 0.27f, 0.30f)
            );

        // =========================================
        // 1. VIE SCOLAIRE - RDC
        //
        // Ancienne carte :
        // x=21 y=27 width=5 height=3
        // porte vers y=30
        // =========================================

        CreateRoom(
            root.transform,
            "Vie_Scolaire_RDC",
            21f, 27f,
            5f, 3f,
            0f,
            23.5f,
            floorRdc,
            walls,
            doorMat
        );

        // =========================================
        // 2. CDI - R+1
        //
        // Ancienne carte :
        // x=26 y=21 width=10 height=3
        // porte vers y=24
        // =========================================

        CreateRoom(
            root.transform,
            "CDI_R1",
            26f, 21f,
            10f, 3f,
            FLOOR_HEIGHT,
            31.5f,
            floorR1,
            walls,
            doorMat
        );

        // =========================================
        // 3. ESCALIER PHYSIQUE
        //
        // Zone approximative :
        // x ~ 23..25
        // y ~ 24..27
        //
        // montée vers le nord (-Z)
        // =========================================

        CreateStaircase(
            root.transform,
            stairsMat,
            railMat
        );

        // =========================================
        // 4. PASSERELLE R+1
        //
        // relie le haut de l'escalier au CDI
        // =========================================

        CreateUpperWalkway(
            root.transform,
            floorR1,
            railMat
        );

        // =========================================
        // 5. Joueur devant l'entrée
        // =========================================

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            CharacterController cc =
                player.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.transform.position =
                MapPoint(
                    23.5f,
                    31.2f,
                    0.08f
                );

            // Regarde vers la cour / le nord du plan
            player.transform.rotation =
                Quaternion.Euler(
                    0f,
                    180f,
                    0f
                );

            if (cc != null)
                cc.enabled = true;
        }

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "COLLEGE TREASURE 3D - ETAPE 5 TERMINEE"
        );
    }

    // =====================================================
    // PIECE CREUSE
    // =====================================================

    static void CreateRoom(
        Transform parent,
        string name,
        float mx,
        float mz,
        float mw,
        float md,
        float floorY,
        float doorMapX,
        Material floorMaterial,
        Material wallMaterial,
        Material doorMaterial
    )
    {
        GameObject room =
            new GameObject(name);

        room.transform.SetParent(parent);

        float centerX =
            MapX(mx + mw / 2f);

        float centerZ =
            MapZ(mz + md / 2f);

        float worldWidth =
            mw * CELL;

        float worldDepth =
            md * CELL;

        // Sol
        Cube(
            name + "_Floor",
            new Vector3(
                centerX,
                floorY - 0.10f,
                centerZ
            ),
            new Vector3(
                worldWidth,
                0.20f,
                worldDepth
            ),
            floorMaterial,
            room.transform
        );

        // mur nord
        Cube(
            name + "_North",
            new Vector3(
                centerX,
                floorY + WALL_HEIGHT / 2f,
                MapZ(mz)
            ),
            new Vector3(
                worldWidth,
                WALL_HEIGHT,
                WALL
            ),
            wallMaterial,
            room.transform
        );

        // mur gauche
        Cube(
            name + "_West",
            new Vector3(
                MapX(mx),
                floorY + WALL_HEIGHT / 2f,
                centerZ
            ),
            new Vector3(
                WALL,
                WALL_HEIGHT,
                worldDepth
            ),
            wallMaterial,
            room.transform
        );

        // mur droit
        Cube(
            name + "_East",
            new Vector3(
                MapX(mx + mw),
                floorY + WALL_HEIGHT / 2f,
                centerZ
            ),
            new Vector3(
                WALL,
                WALL_HEIGHT,
                worldDepth
            ),
            wallMaterial,
            room.transform
        );

        // -------------------------------------
        // Mur sud avec ouverture de 1.6 m
        // -------------------------------------

        float southZ =
            MapZ(mz + md);

        float leftX =
            MapX(mx);

        float rightX =
            MapX(mx + mw);

        float doorCenterX =
            MapX(doorMapX);

        float doorWidth =
            1.6f;

        float leftSegment =
            doorCenterX
            - doorWidth / 2f
            - leftX;

        float rightSegment =
            rightX
            - (
                doorCenterX
                + doorWidth / 2f
            );

        if (leftSegment > 0.05f)
        {
            Cube(
                name + "_South_Left",
                new Vector3(
                    leftX + leftSegment / 2f,
                    floorY + WALL_HEIGHT / 2f,
                    southZ
                ),
                new Vector3(
                    leftSegment,
                    WALL_HEIGHT,
                    WALL
                ),
                wallMaterial,
                room.transform
            );
        }

        if (rightSegment > 0.05f)
        {
            Cube(
                name + "_South_Right",
                new Vector3(
                    doorCenterX
                    + doorWidth / 2f
                    + rightSegment / 2f,

                    floorY + WALL_HEIGHT / 2f,
                    southZ
                ),
                new Vector3(
                    rightSegment,
                    WALL_HEIGHT,
                    WALL
                ),
                wallMaterial,
                room.transform
            );
        }

        // linteau au-dessus de la porte
        Cube(
            name + "_Door_Header",
            new Vector3(
                doorCenterX,
                floorY + 2.75f,
                southZ
            ),
            new Vector3(
                doorWidth,
                0.5f,
                WALL
            ),
            wallMaterial,
            room.transform
        );

        CreateDoor(
            room.transform,
            name + "_Door",
            new Vector3(
                doorCenterX
                - doorWidth / 2f,
                floorY,
                southZ - 0.05f
            ),
            doorWidth,
            doorMaterial
        );
    }

    // =====================================================
    // PORTE
    // =====================================================

    static void CreateDoor(
        Transform parent,
        string name,
        Vector3 hingePosition,
        float width,
        Material material
    )
    {
        GameObject pivot =
            new GameObject(name);

        pivot.transform.SetParent(parent);
        pivot.transform.position =
            hingePosition;

        DoorController controller =
            pivot.AddComponent<DoorController>();

        controller.openAngle = -95f;
        controller.speed = 4f;

        GameObject door =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        door.name =
            name + "_Panel";

        door.transform.SetParent(
            pivot.transform
        );

        door.transform.localPosition =
            new Vector3(
                width / 2f,
                1.25f,
                0f
            );

        door.transform.localScale =
            new Vector3(
                width,
                2.5f,
                0.12f
            );

        door.GetComponent<Renderer>()
            .sharedMaterial =
            material;
    }

    // =====================================================
    // ESCALIER
    // =====================================================

    static void CreateStaircase(
        Transform parent,
        Material stairMaterial,
        Material railMaterial
    )
    {
        GameObject stairs =
            new GameObject("Escalier_RDC_R1");

        stairs.transform.SetParent(parent);

        const int count = 16;

        float startX =
            MapX(24f);

        float startZ =
            MapZ(27f);

        float endZ =
            MapZ(24.4f);

        float width =
            2.4f;

        float dz =
            (endZ - startZ) / count;

        float stepRise =
            FLOOR_HEIGHT / count;

        float stepDepth =
            Mathf.Abs(dz) + 0.08f;

        for (int i = 0; i < count; i++)
        {
            float height =
                stepRise * (i + 1);

            float z =
                startZ
                + dz * (i + 0.5f);

            Cube(
                "Step_" + (i + 1),
                new Vector3(
                    startX,
                    height / 2f,
                    z
                ),
                new Vector3(
                    width,
                    height,
                    stepDepth
                ),
                stairMaterial,
                stairs.transform
            );
        }

        // petit palier supérieur
        Cube(
            "Top_Landing",
            new Vector3(
                startX,
                FLOOR_HEIGHT - 0.1f,
                endZ - 0.5f
            ),
            new Vector3(
                2.4f,
                0.2f,
                1.4f
            ),
            stairMaterial,
            stairs.transform
        );

        // rambardes
        Cube(
            "Stair_Rail_Left",
            new Vector3(
                startX - 1.35f,
                FLOOR_HEIGHT / 2f + 0.6f,
                (startZ + endZ) / 2f
            ),
            new Vector3(
                0.10f,
                FLOOR_HEIGHT + 1.2f,
                Mathf.Abs(endZ - startZ)
            ),
            railMaterial,
            stairs.transform
        );

        Cube(
            "Stair_Rail_Right",
            new Vector3(
                startX + 1.35f,
                FLOOR_HEIGHT / 2f + 0.6f,
                (startZ + endZ) / 2f
            ),
            new Vector3(
                0.10f,
                FLOOR_HEIGHT + 1.2f,
                Mathf.Abs(endZ - startZ)
            ),
            railMaterial,
            stairs.transform
        );
    }

    // =====================================================
    // PASSERELLE HAUTE
    // =====================================================

    static void CreateUpperWalkway(
        Transform parent,
        Material floorMaterial,
        Material railMaterial
    )
    {
        GameObject bridge =
            new GameObject("Passerelle_R1");

        bridge.transform.SetParent(parent);

        float x1 =
            MapX(24f);

        float x2 =
            MapX(31.5f);

        float centerX =
            (x1 + x2) / 2f;

        float length =
            Mathf.Abs(x2 - x1);

        float z =
            MapZ(24.5f);

        Cube(
            "Passerelle_Floor",
            new Vector3(
                centerX,
                FLOOR_HEIGHT - 0.10f,
                z
            ),
            new Vector3(
                length,
                0.20f,
                2.0f
            ),
            floorMaterial,
            bridge.transform
        );

        // rambarde côté extérieur
        Cube(
            "Passerelle_Rail",
            new Vector3(
                centerX,
                FLOOR_HEIGHT + 0.55f,
                z + 1.05f
            ),
            new Vector3(
                length,
                1.1f,
                0.10f
            ),
            railMaterial,
            bridge.transform
        );
    }

    // =====================================================
    // OUTILS
    // =====================================================

    static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material,
        Transform parent
    )
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.transform.SetParent(parent);

        obj.GetComponent<Renderer>()
            .sharedMaterial =
            material;

        return obj;
    }

    static float MapX(float x)
    {
        return (x - 32f) * CELL;
    }

    static float MapZ(float z)
    {
        return (z - 22f) * CELL;
    }

    static Vector3 MapPoint(
        float x,
        float z,
        float y
    )
    {
        return new Vector3(
            MapX(x),
            y,
            MapZ(z)
        );
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

        mat =
            new Material(shader);

        mat.color = color;

        AssetDatabase.CreateAsset(
            mat,
            path
        );

        return mat;
    }

    static void DestroyIfExists(
        string name
    )
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep6
{
    const float CELL = 2f;
    const float R1 = 3.2f;

    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        // ----------------------------------------
        // Supprime la balise temporaire
        // ----------------------------------------

        DestroyIfExists(
            "ESCALIER_BEACON"
        );

        // ----------------------------------------
        // Ancien système d'interaction
        // ----------------------------------------

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            DoorInteractor old =
                player.GetComponent<
                    DoorInteractor
                >();

            if (old != null)
            {
                Object.DestroyImmediate(
                    old
                );
            }

            if (
                player.GetComponent<
                    PlayerInteraction3D
                >()
                == null
            )
            {
                player.AddComponent<
                    PlayerInteraction3D
                >();
            }
        }

        // ----------------------------------------
        // GameManager
        // ----------------------------------------

        DestroyIfExists(
            "TreasureGameManager"
        );

        GameObject manager =
            new GameObject(
                "TreasureGameManager"
            );

        manager.AddComponent<
            TreasureGameManager
        >();

        // ----------------------------------------
        // Matériau indices
        // ----------------------------------------

        Material clueMaterial =
            GetMaterial(
                "Clue_Green",
                new Color(
                    0.10f,
                    0.90f,
                    0.70f
                )
            );

        // ----------------------------------------
        // INDICE 1 : VIE SCOLAIRE
        //
        // Jeu GitHub original :
        // C1 = position 25,30
        // ----------------------------------------

        CreateClue(
            "Indice_1_Vie_Scolaire",

            new Vector3(
                MapX(24f),
                0.75f,
                MapZ(28.5f)
            ),

            1,

            "Indice 1 - Vie scolaire",

            "Premier indice : "
            + "là où les absences "
            + "sont notées.",

            "Va au CDI pour trouver "
            + "l'indice 2.",

            clueMaterial
        );

        // ----------------------------------------
        // INDICE 2 : CDI
        //
        // Jeu GitHub original :
        // C2 = position 31,24
        // ----------------------------------------

        CreateClue(
            "Indice_2_CDI",

            new Vector3(
                MapX(31f),
                R1 + 0.75f,
                MapZ(22.5f)
            ),

            2,

            "Indice 2 - CDI",

            "Cherche parmi les livres "
            + "et les savoirs.",

            "Va à l'auditorium pour "
            + "trouver l'indice 3.",

            clueMaterial
        );

        // ----------------------------------------
        // Repositionnement au départ :
        // devant Vie scolaire
        // ----------------------------------------

        if (player != null)
        {
            CharacterController cc =
                player.GetComponent<
                    CharacterController
                >();

            if (cc != null)
                cc.enabled = false;

            player.transform.position =
                new Vector3(
                    MapX(23.5f),
                    0.10f,
                    MapZ(31f)
                );

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
            "COLLEGE TREASURE 3D "
            + "- ETAPE 6 TERMINEE"
        );
    }

    static void CreateClue(
        string name,
        Vector3 position,
        int order,
        string title,
        string text,
        string objective,
        Material material
    )
    {
        DestroyIfExists(name);

        GameObject clue =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        clue.name = name;

        clue.transform.position =
            position;

        clue.transform.localScale =
            new Vector3(
                0.55f,
                0.55f,
                0.55f
            );

        clue.GetComponent<Renderer>()
            .sharedMaterial =
            material;

        ClueObject data =
            clue.AddComponent<
                ClueObject
            >();

        data.order = order;
        data.clueTitle = title;
        data.clueText = text;
        data.nextObjective = objective;
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
            AssetDatabase
                .LoadAssetAtPath<
                    Material
                >(path);

        if (mat != null)
        {
            mat.color = color;
            return mat;
        }

        Shader shader =
            Shader.Find("Standard");

        if (shader == null)
        {
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );
        }

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
        {
            Object.DestroyImmediate(obj);
        }
    }
}

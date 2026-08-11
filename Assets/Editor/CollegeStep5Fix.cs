using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep5Fix
{
    const float CELL = 2f;

    public static void Fix()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        // ==================================================
        // 1. OUVERTURE DU MUR NORD DE LA VIE SCOLAIRE
        // ==================================================

        Destroy("Vie_Scolaire_RDC_North");
        Destroy("Vie_Scolaire_North_Left_Fix");
        Destroy("Vie_Scolaire_North_Right_Fix");

        Material walls =
            AssetDatabase.LoadAssetAtPath<Material>(
                "Assets/Materials/Step5_Walls.mat"
            );

        float leftX  = MapX(21f);
        float rightX = MapX(26f);

        float openingCenter =
            MapX(24f);

        float openingWidth = 2.0f;

        float northZ =
            MapZ(27f);

        float leftLength =
            openingCenter
            - openingWidth / 2f
            - leftX;

        float rightLength =
            rightX
            - (
                openingCenter
                + openingWidth / 2f
            );

        Cube(
            "Vie_Scolaire_North_Left_Fix",
            new Vector3(
                leftX + leftLength / 2f,
                1.5f,
                northZ
            ),
            new Vector3(
                leftLength,
                3f,
                0.18f
            ),
            walls
        );

        Cube(
            "Vie_Scolaire_North_Right_Fix",
            new Vector3(
                openingCenter
                + openingWidth / 2f
                + rightLength / 2f,

                1.5f,
                northZ
            ),
            new Vector3(
                rightLength,
                3f,
                0.18f
            ),
            walls
        );

        // ==================================================
        // 2. ESCALIER BLEU VIF
        // ==================================================

        Material blue =
            GetBlueMaterial();

        GameObject stairs =
            GameObject.Find(
                "Escalier_RDC_R1"
            );

        if (stairs != null)
        {
            Renderer[] renderers =
                stairs.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                r.sharedMaterial = blue;
            }
        }

        // ==================================================
        // 3. BALISE BLEUE TEMPORAIRE
        // ==================================================

        Destroy("ESCALIER_BEACON");

        GameObject beacon =
            Cube(
                "ESCALIER_BEACON",

                new Vector3(
                    MapX(24f) + 1.6f,
                    3f,
                    MapZ(26.5f)
                ),

                new Vector3(
                    0.25f,
                    6f,
                    0.25f
                ),

                blue
            );

        Collider beaconCollider =
            beacon.GetComponent<Collider>();

        if (beaconCollider != null)
            beaconCollider.enabled = false;

        // ==================================================
        // 4. JOUEUR JUSTE DEVANT L'ESCALIER
        // ==================================================

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            CharacterController cc =
                player.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.transform.position =
                new Vector3(
                    MapX(24f),
                    0.10f,
                    MapZ(28.3f)
                );

            // regarde vers l'escalier
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

        Debug.Log(
            "ETAPE 5 CORRIGEE : ESCALIER ACCESSIBLE ET VISIBLE"
        );
    }

    static float MapX(float x)
    {
        return (x - 32f) * CELL;
    }

    static float MapZ(float z)
    {
        return (z - 22f) * CELL;
    }

    static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material
    )
    {
        GameObject o =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        o.name = name;
        o.transform.position = position;
        o.transform.localScale = scale;

        if (material != null)
            o.GetComponent<Renderer>()
             .sharedMaterial = material;

        return o;
    }

    static Material GetBlueMaterial()
    {
        string path =
            "Assets/Materials/Step5_Stair_Highlight.mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (mat != null)
            return mat;

        Shader shader =
            Shader.Find("Standard");

        if (shader == null)
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

        mat =
            new Material(shader);

        mat.color =
            new Color(
                0.05f,
                0.55f,
                1.0f
            );

        AssetDatabase.CreateAsset(
            mat,
            path
        );

        return mat;
    }

    static void Destroy(string name)
    {
        GameObject o =
            GameObject.Find(name);

        if (o != null)
            Object.DestroyImmediate(o);
    }
}

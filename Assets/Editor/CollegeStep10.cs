using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep10
{
    const float C = 2f;

    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Delete("[RDC] Self élèves (self-eleves)");
        Delete("Self_Eleves_Visitable");
        Delete("Indice_6_Self");

        Material wall =
            Mat("Self_Walls",
                new Color(.88f,.89f,.90f));

        Material floor =
            Mat("Self_Floor",
                new Color(.72f,.70f,.62f));

        Material doorMat =
            Mat("Self_Door",
                new Color(.30f,.16f,.07f));

        Material tableMat =
            Mat("Self_Tables",
                new Color(.55f,.38f,.20f));

        Material chairMat =
            Mat("Self_Chairs",
                new Color(.25f,.28f,.30f));

        Material green =
            Mat("Clue_Green",
                new Color(.10f,.90f,.70f));

        GameObject root =
            new GameObject("Self_Eleves_Visitable");

        // Carte originale :
        // x = 49 .. 59
        // y = 14 .. 17

        float x1 = X(49f);
        float x2 = X(59f);

        float z1 = Z(14f);
        float z2 = Z(17f);

        float cx = (x1+x2)/2f;
        float cz = (z1+z2)/2f;

        // =================================================
        // SOL
        // =================================================

        Cube(
            "Self_Floor",
            new Vector3(cx,-.10f,cz),
            new Vector3(
                x2-x1,
                .20f,
                z2-z1
            ),
            floor,
            root.transform
        );

        // =================================================
        // MURS NORD / EST / OUEST
        // =================================================

        Cube(
            "Self_North",
            new Vector3(cx,1.5f,z1),
            new Vector3(x2-x1,3f,.18f),
            wall,
            root.transform
        );

        Cube(
            "Self_West",
            new Vector3(x1,1.5f,cz),
            new Vector3(.18f,3f,z2-z1),
            wall,
            root.transform
        );

        Cube(
            "Self_East",
            new Vector3(x2,1.5f,cz),
            new Vector3(.18f,3f,z2-z1),
            wall,
            root.transform
        );

        // =================================================
        // MUR SUD AVEC PORTE
        // =================================================

        float doorX = X(51.5f);
        float doorWidth = 1.8f;

        float leftLength =
            doorX-doorWidth/2f-x1;

        float rightLength =
            x2-(doorX+doorWidth/2f);

        Cube(
            "Self_South_Left",
            new Vector3(
                x1+leftLength/2f,
                1.5f,
                z2
            ),
            new Vector3(
                leftLength,
                3f,
                .18f
            ),
            wall,
            root.transform
        );

        Cube(
            "Self_South_Right",
            new Vector3(
                doorX+doorWidth/2f+rightLength/2f,
                1.5f,
                z2
            ),
            new Vector3(
                rightLength,
                3f,
                .18f
            ),
            wall,
            root.transform
        );

        Cube(
            "Self_Door_Header",
            new Vector3(
                doorX,
                2.75f,
                z2
            ),
            new Vector3(
                doorWidth,
                .5f,
                .18f
            ),
            wall,
            root.transform
        );

        // =================================================
        // PORTE
        // =================================================

        GameObject pivot =
            new GameObject("Self_Door");

        pivot.transform.SetParent(root.transform);

        pivot.transform.position =
            new Vector3(
                doorX-doorWidth/2f,
                0f,
                z2
            );

        DoorController dc =
            pivot.AddComponent<DoorController>();

        dc.openAngle = -95f;
        dc.speed = 4f;

        GameObject panel =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        panel.name = "Self_Door_Panel";

        panel.transform.SetParent(
            pivot.transform
        );

        panel.transform.localPosition =
            new Vector3(
                doorWidth/2f,
                1.25f,
                0f
            );

        panel.transform.localScale =
            new Vector3(
                doorWidth,
                2.5f,
                .12f
            );

        panel.GetComponent<Renderer>()
            .sharedMaterial = doorMat;

        // =================================================
        // TABLES DU SELF
        // =================================================

        for(int row=0; row<2; row++)
        {
            for(int col=0; col<4; col++)
            {
                float tx =
                    X(52f + col*1.6f);

                float tz =
                    Z(15f + row*1.0f);

                Cube(
                    "Table_"+row+"_"+col,
                    new Vector3(tx,.48f,tz),
                    new Vector3(
                        2.0f,
                        .12f,
                        1.0f
                    ),
                    tableMat,
                    root.transform
                );

                Cube(
                    "ChairA_"+row+"_"+col,
                    new Vector3(
                        tx,
                        .35f,
                        tz-.85f
                    ),
                    new Vector3(
                        .55f,
                        .7f,
                        .55f
                    ),
                    chairMat,
                    root.transform
                );

                Cube(
                    "ChairB_"+row+"_"+col,
                    new Vector3(
                        tx,
                        .35f,
                        tz+.85f
                    ),
                    new Vector3(
                        .55f,
                        .7f,
                        .55f
                    ),
                    chairMat,
                    root.transform
                );
            }
        }

        // =================================================
        // CHEMIN DEVANT LE SELF
        // =================================================

        Cube(
            "Self_Entrance_Path",
            new Vector3(
                doorX,
                -.06f,
                Z(18f)
            ),
            new Vector3(
                5f,
                .12f,
                4f
            ),
            floor,
            root.transform
        );

        // =================================================
        // INDICE 6
        //
        // fidèle au jeu original :
        // C6 = grille (51,18)
        //
        // placé dehors, devant le Self
        // =================================================

        GameObject clue =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        clue.name = "Indice_6_Self";

        clue.transform.position =
            new Vector3(
                X(51f),
                .9f,
                Z(18f)
            );

        clue.transform.localScale =
            new Vector3(
                1.25f,
                1.25f,
                1.25f
            );

        clue.GetComponent<Renderer>()
            .sharedMaterial = green;

        ClueObject clueData =
            clue.AddComponent<ClueObject>();

        clueData.order = 6;

        clueData.clueTitle =
            "Indice 6 - Self élèves";

        clueData.clueText =
            "Le trésor se rapproche là où les élèves reprennent des forces.";

        clueData.nextObjective =
            "Retourne dans la cour centrale, près de Conseils et Multi 1.";

        // Lumière forte pour le repérer
        GameObject lightObj =
            new GameObject("IndiceLight");

        lightObj.transform.SetParent(
            clue.transform
        );

        lightObj.transform.localPosition =
            Vector3.zero;

        Light li =
            lightObj.AddComponent<Light>();

        li.type = LightType.Point;
        li.range = 8f;
        li.intensity = 5f;
        li.color =
            new Color(.1f,1f,.6f);

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 10 TERMINEE - SELF + INDICE 6"
        );
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

        if(parent != null)
            obj.transform.SetParent(parent);

        obj.GetComponent<Renderer>()
            .sharedMaterial = material;

        return obj;
    }

    static Material Mat(
        string name,
        Color color)
    {
        string path =
            "Assets/Materials/"
            + name
            + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if(mat != null)
        {
            mat.color = color;
            return mat;
        }

        Shader shader =
            Shader.Find("Standard");

        if(shader == null)
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

        mat = new Material(shader);
        mat.color = color;

        AssetDatabase.CreateAsset(
            mat,
            path
        );

        return mat;
    }

    static float X(float value)
    {
        return (value-32f)*C;
    }

    static float Z(float value)
    {
        return (value-22f)*C;
    }

    static void Delete(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if(obj != null)
            Object.DestroyImmediate(obj);
    }
}

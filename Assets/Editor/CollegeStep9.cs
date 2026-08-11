using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep9
{
    const float C = 2f;
    const float R1 = 3.2f;

    public static void Create()
    {
        string scenePath = "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Delete("[R+1] Musique (musique)");
        Delete("[R+1] Arts (arts)");
        Delete("[ESCALIER] escaliers-haut");

        Delete("Arts_Musique_Visitable");
        Delete("Escalier_Arts_R1");
        Delete("Indice_5_Arts_Musique");

        Material walls =
            Mat("AM_Walls", new Color(.90f,.78f,.35f));

        Material floor =
            Mat("AM_Floor", new Color(.72f,.62f,.42f));

        Material blue =
            Mat("AM_Stairs", new Color(.08f,.55f,1f));

        Material door =
            Mat("AM_Doors", new Color(.35f,.17f,.07f));

        Material green =
            Mat("Clue_Green", new Color(.10f,.90f,.70f));

        Material dark =
            Mat("AM_Props", new Color(.22f,.22f,.25f));

        // ==================================================
        // AILE ARTS / MUSIQUE
        // R+1
        //
        // Musique : x10..16
        // Arts    : x15..20
        // ==================================================

        GameObject root =
            new GameObject("Arts_Musique_Visitable");

        float x1 = X(10f);
        float x2 = X(20f);

        float z1 = Z(12f);
        float z2 = Z(17f);

        float cx = (x1+x2)/2f;
        float cz = (z1+z2)/2f;

        // Sol du R+1
        Cube(
            "AM_Floor",
            new Vector3(cx,R1-.1f,cz),
            new Vector3(x2-x1,.2f,z2-z1),
            floor,
            root.transform
        );

        // Mur nord
        Cube(
            "AM_North",
            new Vector3(cx,R1+1.5f,z1),
            new Vector3(x2-x1,3f,.18f),
            walls,
            root.transform
        );

        // Mur ouest
        Cube(
            "AM_West",
            new Vector3(x1,R1+1.5f,cz),
            new Vector3(.18f,3f,z2-z1),
            walls,
            root.transform
        );

        // Mur est
        Cube(
            "AM_East",
            new Vector3(x2,R1+1.5f,cz),
            new Vector3(.18f,3f,z2-z1),
            walls,
            root.transform
        );

        // ==================================================
        // MUR SUD AVEC 2 PORTES :
        // MUSIQUE vers x=13
        // ARTS vers x=17
        // ==================================================

        float sx1 = X(10f);
        float sx2 = X(12.2f);
        float sx3 = X(13.8f);
        float sx4 = X(16.2f);
        float sx5 = X(17.8f);
        float sx6 = X(20f);

        WallSegment(
            "AM_South_1",
            sx1,
            sx2,
            z2,
            walls,
            root.transform
        );

        WallSegment(
            "AM_South_2",
            sx3,
            sx4,
            z2,
            walls,
            root.transform
        );

        WallSegment(
            "AM_South_3",
            sx5,
            sx6,
            z2,
            walls,
            root.transform
        );

        CreateDoor(
            "Musique_Door",
            X(13f),
            z2,
            door,
            root.transform
        );

        CreateDoor(
            "Arts_Door",
            X(17f),
            z2,
            door,
            root.transform
        );

        // ==================================================
        // CLOISON ENTRE MUSIQUE ET ARTS
        // ==================================================

        Cube(
            "AM_Partition",
            new Vector3(
                X(15.3f),
                R1+1.5f,
                Z(14.2f)
            ),
            new Vector3(
                .18f,
                3f,
                4.2f
            ),
            walls,
            root.transform
        );

        // ==================================================
        // ELEMENTS MUSIQUE
        // ==================================================

        Cube(
            "Piano",
            new Vector3(
                X(12.2f),
                R1+.45f,
                Z(13.3f)
            ),
            new Vector3(
                2.5f,
                .9f,
                1.3f
            ),
            dark,
            root.transform
        );

        for(int i=0;i<4;i++)
        {
            Cube(
                "Music_Chair_"+i,
                new Vector3(
                    X(11.5f+i*.75f),
                    R1+.35f,
                    Z(15f)
                ),
                new Vector3(
                    .55f,
                    .7f,
                    .55f
                ),
                dark,
                root.transform
            );
        }

        // ==================================================
        // ELEMENTS ARTS
        // ==================================================

        for(int i=0;i<3;i++)
        {
            Cube(
                "Easel_"+i,
                new Vector3(
                    X(16.2f+i*.9f),
                    R1+.8f,
                    Z(14.2f)
                ),
                new Vector3(
                    .25f,
                    1.6f,
                    .8f
                ),
                dark,
                root.transform
            );
        }

        // ==================================================
        // ESCALIER BLEU PHYSIQUE RDC -> R+1
        // ==================================================

        GameObject stairs =
            new GameObject("Escalier_Arts_R1");

        int steps = 16;

        float startX = X(21.5f);
        float endX = X(17.7f);

        float stairZ = Z(18.3f);

        float dx =
            (endX-startX)/steps;

        float rise =
            R1/steps;

        for(int i=0;i<steps;i++)
        {
            float topY =
                (i+1)*rise;

            float px =
                startX + dx*(i+.5f);

            Cube(
                "Arts_Step_"+(i+1),
                new Vector3(
                    px,
                    topY/2f,
                    stairZ
                ),
                new Vector3(
                    Mathf.Abs(dx)+.04f,
                    topY,
                    2.2f
                ),
                blue,
                stairs.transform
            );
        }

        // ==================================================
        // PALIER DU HAUT VERS LA PORTE ARTS
        // ==================================================

        float bridgeZ1 = Z(17f);
        float bridgeZ2 = Z(18.3f);

        Cube(
            "Arts_Upper_Landing",
            new Vector3(
                X(17.7f),
                R1-.1f,
                (bridgeZ1+bridgeZ2)/2f
            ),
            new Vector3(
                2.2f,
                .2f,
                Mathf.Abs(bridgeZ2-bridgeZ1)+.3f
            ),
            blue,
            stairs.transform
        );

        // ==================================================
        // INDICE 5
        // juste devant Arts, au sommet de l'escalier
        // ==================================================

        GameObject clue =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        clue.name =
            "Indice_5_Arts_Musique";

        clue.transform.position =
            new Vector3(
                X(17f),
                R1+.85f,
                Z(17.7f)
            );

        clue.transform.localScale =
            new Vector3(
                1.15f,
                1.15f,
                1.15f
            );

        clue.GetComponent<Renderer>()
            .sharedMaterial = green;

        ClueObject clueData =
            clue.AddComponent<ClueObject>();

        clueData.order = 5;

        clueData.clueTitle =
            "Indice 5 - Arts et Musique";

        clueData.clueText =
            "Les créations et les sons indiquent la suite du parcours.";

        clueData.nextObjective =
            "Va au self élèves pour trouver l'indice 6.";

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
        li.range = 7f;
        li.intensity = 4f;
        li.color =
            new Color(.1f,1f,.6f);

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 9 TERMINEE - ARTS MUSIQUE + INDICE 5"
        );
    }

    static void WallSegment(
        string name,
        float a,
        float b,
        float z,
        Material material,
        Transform parent)
    {
        Cube(
            name,
            new Vector3(
                (a+b)/2f,
                R1+1.5f,
                z
            ),
            new Vector3(
                Mathf.Abs(b-a),
                3f,
                .18f
            ),
            material,
            parent
        );
    }

    static void CreateDoor(
        string name,
        float centerX,
        float z,
        Material material,
        Transform parent)
    {
        float width = 1.6f;

        GameObject pivot =
            new GameObject(name);

        pivot.transform.SetParent(parent);

        pivot.transform.position =
            new Vector3(
                centerX-width/2f,
                R1,
                z
            );

        DoorController dc =
            pivot.AddComponent<DoorController>();

        dc.openAngle = -95f;
        dc.speed = 4f;

        GameObject panel =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        panel.name = name+"_Panel";

        panel.transform.SetParent(
            pivot.transform
        );

        panel.transform.localPosition =
            new Vector3(
                width/2f,
                1.25f,
                0f
            );

        panel.transform.localScale =
            new Vector3(
                width,
                2.5f,
                .12f
            );

        panel.GetComponent<Renderer>()
            .sharedMaterial = material;
    }

    static GameObject Cube(
        string name,
        Vector3 pos,
        Vector3 scale,
        Material material,
        Transform parent)
    {
        GameObject obj =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        obj.name = name;
        obj.transform.position = pos;
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

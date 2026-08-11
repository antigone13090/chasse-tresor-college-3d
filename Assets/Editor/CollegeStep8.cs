using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep8
{
    const float C = 2f;

    public static void Create()
    {
        string path = "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(path);

        Delete("[RDC] Gymnase (gymnase)");
        Delete("Gymnase_Visitable");
        Delete("Indice_4_Gymnase");

        Material wall =
            Mat("Gym_Wall",
                new Color(.82f,.84f,.85f));

        Material floor =
            Mat("Gym_Floor",
                new Color(.65f,.45f,.25f));

        Material doorMat =
            Mat("Gym_Door",
                new Color(.25f,.12f,.05f));

        Material green =
            Mat("Clue_Green",
                new Color(.10f,.90f,.70f));

        Material bench =
            Mat("Gym_Bench",
                new Color(.35f,.35f,.38f));

        GameObject root =
            new GameObject("Gymnase_Visitable");

        float x1 = X(7f);
        float x2 = X(17f);

        float z1 = Z(34f);
        float z2 = Z(41f);

        float cx = (x1+x2)/2f;
        float cz = (z1+z2)/2f;

        // SOL
        Cube(
            "Gym_Floor",
            new Vector3(cx,-.1f,cz),
            new Vector3(
                x2-x1,
                .2f,
                z2-z1
            ),
            floor,
            root.transform
        );

        // MUR NORD
        Cube(
            "Gym_North",
            new Vector3(cx,1.5f,z1),
            new Vector3(
                x2-x1,
                3f,
                .18f
            ),
            wall,
            root.transform
        );

        // MUR SUD
        Cube(
            "Gym_South",
            new Vector3(cx,1.5f,z2),
            new Vector3(
                x2-x1,
                3f,
                .18f
            ),
            wall,
            root.transform
        );

        // MUR OUEST
        Cube(
            "Gym_West",
            new Vector3(x1,1.5f,cz),
            new Vector3(
                .18f,
                3f,
                z2-z1
            ),
            wall,
            root.transform
        );

        // =================================================
        // MUR EST AVEC PORTE
        // =================================================

        float doorZ = Z(36f);
        float halfDoor = 1f;

        float part1 =
            doorZ-halfDoor-z1;

        float part2 =
            z2-(doorZ+halfDoor);

        Cube(
            "Gym_East_1",
            new Vector3(
                x2,
                1.5f,
                z1+part1/2f
            ),
            new Vector3(
                .18f,
                3f,
                part1
            ),
            wall,
            root.transform
        );

        Cube(
            "Gym_East_2",
            new Vector3(
                x2,
                1.5f,
                doorZ+halfDoor+part2/2f
            ),
            new Vector3(
                .18f,
                3f,
                part2
            ),
            wall,
            root.transform
        );

        // LINTEAU
        Cube(
            "Gym_Door_Header",
            new Vector3(
                x2,
                2.75f,
                doorZ
            ),
            new Vector3(
                .18f,
                .5f,
                2f
            ),
            wall,
            root.transform
        );

        // =================================================
        // PORTE
        // =================================================

        GameObject pivot =
            new GameObject("Gymnase_Door");

        pivot.transform.SetParent(root.transform);

        pivot.transform.position =
            new Vector3(
                x2,
                0f,
                doorZ-1f
            );

        DoorController dc =
            pivot.AddComponent<DoorController>();

        dc.openAngle = -95f;
        dc.speed = 4f;

        GameObject panel =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        panel.name =
            "Gymnase_Door_Panel";

        panel.transform.SetParent(
            pivot.transform
        );

        panel.transform.localPosition =
            new Vector3(
                0f,
                1.25f,
                1f
            );

        panel.transform.localScale =
            new Vector3(
                .12f,
                2.5f,
                2f
            );

        panel.GetComponent<Renderer>()
            .sharedMaterial =
            doorMat;

        // =================================================
        // GRADINS SIMPLES
        // =================================================

        for(int i=0;i<3;i++)
        {
            Cube(
                "Gradin_"+i,
                new Vector3(
                    x1+1.0f+i*.6f,
                    .20f+i*.20f,
                    cz
                ),
                new Vector3(
                    .6f,
                    .4f+i*.4f,
                    8f
                ),
                bench,
                root.transform
            );
        }

        // =================================================
        // DEUX PANIERS SIMPLIFIES
        // =================================================

        CreateBasket(
            "Panier_1",
            new Vector3(
                cx,
                2.2f,
                z1+1f
            ),
            root.transform,
            wall
        );

        CreateBasket(
            "Panier_2",
            new Vector3(
                cx,
                2.2f,
                z2-1f
            ),
            root.transform,
            wall
        );

        // =================================================
        // INDICE 4
        //
        // volontairement placé DEVANT le gymnase
        // pour rester proche du jeu original
        // =================================================

        GameObject clue =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        clue.name =
            "Indice_4_Gymnase";

        clue.transform.position =
            new Vector3(
                X(17.8f),
                .9f,
                Z(36f)
            );

        clue.transform.localScale =
            new Vector3(
                1.1f,
                1.1f,
                1.1f
            );

        clue.GetComponent<Renderer>()
            .sharedMaterial =
            green;

        ClueObject data =
            clue.AddComponent<ClueObject>();

        data.order = 4;

        data.clueTitle =
            "Indice 4 - Gymnase";

        data.clueText =
            "Ici, on court, on saute, on joue.";

        data.nextObjective =
            "Monte vers Arts et Musique pour trouver l'indice 5.";

        // Lumière
        GameObject lightObject =
            new GameObject("IndiceLight");

        lightObject.transform.SetParent(
            clue.transform
        );

        lightObject.transform.localPosition =
            Vector3.zero;

        Light li =
            lightObject.AddComponent<Light>();

        li.type = LightType.Point;
        li.range = 7f;
        li.intensity = 4f;
        li.color =
            new Color(.1f,1f,.6f);

        EditorSceneManager.SaveScene(
            scene,
            path
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 8 TERMINEE - GYMNASE + INDICE 4"
        );
    }

    static void CreateBasket(
        string name,
        Vector3 pos,
        Transform parent,
        Material mat)
    {
        Cube(
            name+"_Pole",
            pos,
            new Vector3(.15f,3f,.15f),
            mat,
            parent
        );

        Cube(
            name+"_Board",
            pos + new Vector3(0f,1.2f,0f),
            new Vector3(2f,1f,.15f),
            mat,
            parent
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
        obj.transform.position =
            position;
        obj.transform.localScale =
            scale;

        if(parent != null)
            obj.transform.SetParent(parent);

        obj.GetComponent<Renderer>()
            .sharedMaterial =
            material;

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
            AssetDatabase
            .LoadAssetAtPath<Material>(
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

        mat =
            new Material(shader);

        mat.color = color;

        AssetDatabase.CreateAsset(
            mat,
            path
        );

        return mat;
    }

    static float X(float v)
    {
        return (v-32f)*C;
    }

    static float Z(float v)
    {
        return (v-22f)*C;
    }

    static void Delete(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if(obj != null)
            Object.DestroyImmediate(obj);
    }
}

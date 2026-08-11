using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public static class CollegeStep3
{
    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        Scene scene =
            EditorSceneManager.OpenScene(scenePath);

        Directory.CreateDirectory("Assets/Materials");

        // Nettoyage de l'étape précédente
        DestroyIfExists("Ground");
        DestroyIfExists("PrototypeBlock");
        DestroyIfExists("Wall_Left");
        DestroyIfExists("Wall_Right");
        DestroyIfExists("Wall_Back");
        DestroyIfExists("College_Prototype");

        // ============================================
        // MATERIAUX
        // ============================================

        Material courtyardMaterial =
            CreateMaterial(
                "Courtyard",
                new Color(0.48f, 0.58f, 0.45f)
            );

        Material corridorMaterial =
            CreateMaterial(
                "CorridorFloor",
                new Color(0.55f, 0.55f, 0.55f)
            );

        Material roomMaterial =
            CreateMaterial(
                "RoomFloor",
                new Color(0.65f, 0.58f, 0.48f)
            );

        Material wallMaterial =
            CreateMaterial(
                "Walls",
                new Color(0.84f, 0.84f, 0.80f)
            );

        Material doorMaterial =
            CreateMaterial(
                "Door",
                new Color(0.38f, 0.20f, 0.08f)
            );

        // ============================================
        // PARENT GENERAL
        // ============================================

        GameObject college =
            new GameObject("College_Prototype");

        // ============================================
        // COUR
        // ============================================

        GameObject courtyard =
            new GameObject("COUR");

        courtyard.transform.SetParent(college.transform);

        CreateCube(
            "Cour_Ground",
            new Vector3(0f, -0.1f, -10f),
            new Vector3(16f, 0.2f, 12f),
            courtyardMaterial,
            courtyard.transform
        );

        // ============================================
        // COULOIR
        // ============================================

        GameObject corridor =
            new GameObject("COULOIR");

        corridor.transform.SetParent(college.transform);

        CreateCube(
            "Couloir_Ground",
            new Vector3(0f, -0.1f, 2f),
            new Vector3(4f, 0.2f, 12f),
            corridorMaterial,
            corridor.transform
        );

        CreateCube(
            "Couloir_Mur_Gauche",
            new Vector3(-2.1f, 1.5f, 2f),
            new Vector3(0.2f, 3f, 12f),
            wallMaterial,
            corridor.transform
        );

        CreateCube(
            "Couloir_Mur_Droit",
            new Vector3(2.1f, 1.5f, 2f),
            new Vector3(0.2f, 3f, 12f),
            wallMaterial,
            corridor.transform
        );

        // ============================================
        // SALLE
        // ============================================

        GameObject room =
            new GameObject("SALLE_TEST");

        room.transform.SetParent(college.transform);

        CreateCube(
            "Salle_Ground",
            new Vector3(0f, -0.1f, 12f),
            new Vector3(8f, 0.2f, 8f),
            roomMaterial,
            room.transform
        );

        // murs latéraux
        CreateCube(
            "Salle_Mur_Gauche",
            new Vector3(-4.1f, 1.5f, 12f),
            new Vector3(0.2f, 3f, 8f),
            wallMaterial,
            room.transform
        );

        CreateCube(
            "Salle_Mur_Droit",
            new Vector3(4.1f, 1.5f, 12f),
            new Vector3(0.2f, 3f, 8f),
            wallMaterial,
            room.transform
        );

        // mur arrière
        CreateCube(
            "Salle_Mur_Fond",
            new Vector3(0f, 1.5f, 16.1f),
            new Vector3(8f, 3f, 0.2f),
            wallMaterial,
            room.transform
        );

        // façade avec ouverture pour la porte
        CreateCube(
            "Salle_Facade_Gauche",
            new Vector3(-2.35f, 1.5f, 8.1f),
            new Vector3(3.3f, 3f, 0.2f),
            wallMaterial,
            room.transform
        );

        CreateCube(
            "Salle_Facade_Droite",
            new Vector3(2.35f, 1.5f, 8.1f),
            new Vector3(3.3f, 3f, 0.2f),
            wallMaterial,
            room.transform
        );

        // ============================================
        // PORTE 3D AVEC PIVOT
        // ============================================

        GameObject doorPivot =
            new GameObject("Porte_Salle");

        doorPivot.transform.SetParent(room.transform);

        // charnière à gauche
        doorPivot.transform.position =
            new Vector3(-0.7f, 0f, 8f);

        doorPivot.AddComponent<DoorController>();

        GameObject door =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        door.name = "Porte";

        door.transform.SetParent(
            doorPivot.transform
        );

        door.transform.localPosition =
            new Vector3(0.7f, 1.25f, 0f);

        door.transform.localScale =
            new Vector3(1.4f, 2.5f, 0.15f);

        door.GetComponent<Renderer>().sharedMaterial =
            doorMaterial;

        // ============================================
        // JOUEUR
        // ============================================

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            player.transform.position =
                new Vector3(0f, 0.1f, -13f);

            player.transform.rotation =
                Quaternion.identity;

            if (player.GetComponent<DoorInteractor>() == null)
            {
                player.AddComponent<DoorInteractor>();
            }
        }

        // ============================================
        // SAUVEGARDE
        // ============================================

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "COLLEGE TREASURE 3D - ETAPE 3 TERMINEE"
        );
    }

    static GameObject CreateCube(
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

        Renderer renderer =
            obj.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = material;

        return obj;
    }

    static Material CreateMaterial(
        string name,
        Color color
    )
    {
        string path =
            "Assets/Materials/" +
            name +
            ".mat";

        Material material =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (material != null)
        {
            material.color = color;
            return material;
        }

        Shader shader =
            Shader.Find("Standard");

        if (shader == null)
            shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

        material =
            new Material(shader);

        material.color = color;

        AssetDatabase.CreateAsset(
            material,
            path
        );

        return material;
    }

    static void DestroyIfExists(string name)
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
            Object.DestroyImmediate(obj);
    }
}

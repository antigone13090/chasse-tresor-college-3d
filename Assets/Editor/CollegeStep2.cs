using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CollegeStep2
{
    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        Scene scene =
            EditorSceneManager.OpenScene(scenePath);

        // Supprime l'ancienne caméra fixe
        GameObject oldCamera =
            GameObject.Find("Main Camera");

        if (oldCamera != null)
            Object.DestroyImmediate(oldCamera);

        // Supprime un ancien Player éventuel
        GameObject oldPlayer =
            GameObject.Find("Player");

        if (oldPlayer != null)
            Object.DestroyImmediate(oldPlayer);

        // ===========================
        // PLAYER
        // ===========================

        GameObject player =
            new GameObject("Player");

        player.transform.position =
            new Vector3(0f, 1.1f, -6f);

        CharacterController controller =
            player.AddComponent<CharacterController>();

        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center =
            new Vector3(0f, 0.9f, 0f);

        controller.stepOffset = 0.3f;
        controller.slopeLimit = 45f;

        player.AddComponent<FPSController>();

        // ===========================
        // CAMERA FPS
        // ===========================

        GameObject cameraObject =
            new GameObject("Player Camera");

        cameraObject.tag = "MainCamera";

        cameraObject.transform.SetParent(
            player.transform
        );

        cameraObject.transform.localPosition =
            new Vector3(0f, 1.6f, 0f);

        cameraObject.transform.localRotation =
            Quaternion.identity;

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.fieldOfView = 70f;
        camera.nearClipPlane = 0.05f;

        cameraObject.AddComponent<AudioListener>();

        // ===========================
        // QUELQUES MURS DE TEST
        // ===========================

        CreateWall(
            "Wall_Left",
            new Vector3(-5f, 1.5f, 0f),
            new Vector3(0.3f, 3f, 12f)
        );

        CreateWall(
            "Wall_Right",
            new Vector3(5f, 1.5f, 0f),
            new Vector3(0.3f, 3f, 12f)
        );

        CreateWall(
            "Wall_Back",
            new Vector3(0f, 1.5f, 6f),
            new Vector3(10f, 3f, 0.3f)
        );

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "COLLEGE TREASURE 3D - ETAPE 2 TERMINEE"
        );
    }

    private static void CreateWall(
        string name,
        Vector3 position,
        Vector3 scale
    )
    {
        GameObject existing =
            GameObject.Find(name);

        if (existing != null)
            Object.DestroyImmediate(existing);

        GameObject wall =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
    }
}

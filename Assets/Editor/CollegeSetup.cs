using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public static class CollegeSetup
{
    public static void CreateStep1()
    {
        Directory.CreateDirectory("Assets/Scenes");

        // Nouvelle scène entièrement vide
        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single
        );

        // ==========================
        // SOL
        // ==========================
        GameObject ground =
            GameObject.CreatePrimitive(PrimitiveType.Plane);

        ground.name = "Ground";

        ground.transform.position =
            new Vector3(0f, 0f, 0f);

        ground.transform.localScale =
            new Vector3(3f, 1f, 3f);


        // ==========================
        // PREMIER BLOC 3D
        // ==========================
        GameObject block =
            GameObject.CreatePrimitive(PrimitiveType.Cube);

        block.name = "PrototypeBlock";

        block.transform.position =
            new Vector3(0f, 0.5f, 0f);

        block.transform.localScale =
            new Vector3(2f, 1f, 2f);


        // ==========================
        // LUMIERE
        // ==========================
        GameObject lightObject =
            new GameObject("Directional Light");

        Light light =
            lightObject.AddComponent<Light>();

        light.type = LightType.Directional;
        light.intensity = 1.2f;

        lightObject.transform.rotation =
            Quaternion.Euler(50f, -30f, 0f);


        // ==========================
        // CAMERA
        // ==========================
        GameObject cameraObject =
            new GameObject("Main Camera");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        cameraObject.tag = "MainCamera";

        cameraObject.transform.position =
            new Vector3(8f, 7f, -8f);

        cameraObject.transform.LookAt(
            new Vector3(0f, 0.5f, 0f)
        );


        // ==========================
        // SAUVEGARDE
        // ==========================
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        EditorBuildSettings.scenes =
            new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene(
                    scenePath,
                    true
                )
            };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "COLLEGE TREASURE 3D - ETAPE 1 TERMINEE"
        );
    }
}

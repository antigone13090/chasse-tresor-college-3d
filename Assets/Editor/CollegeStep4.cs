using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CollegeStep4
{
    [Serializable]
    public class BuildingData
    {
        public string id;
        public string label;
        public string floor;

        public float x;
        public float y;
        public float width;
        public float height;

        public string color;

        public bool collidable;
    }

    [Serializable]
    public class CollegeData
    {
        public int mapWidth;
        public int mapHeight;

        public BuildingData[] buildings;
    }

    const float CELL_SIZE = 2.0f;

    const float STORY_HEIGHT = 3.2f;

    public static void Create()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        string jsonPath =
            "Assets/Data/college_buildings.json";

        Scene scene =
            EditorSceneManager.OpenScene(scenePath);

        // -------------------------------------------
        // Lecture des données issues de GitHub
        // -------------------------------------------

        string json =
            File.ReadAllText(jsonPath);

        CollegeData data =
            JsonUtility.FromJson<CollegeData>(json);

        if (data == null ||
            data.buildings == null)
        {
            throw new Exception(
                "Impossible de lire college_buildings.json"
            );
        }

        Debug.Log(
            "Carte GitHub chargée : "
            + data.mapWidth
            + " x "
            + data.mapHeight
            + " / "
            + data.buildings.Length
            + " zones"
        );

        // -------------------------------------------
        // Suppression ancien prototype étape 3
        // -------------------------------------------

        DestroyIfExists("College_Prototype");

        DestroyIfExists("College_From_GitHub");

        DestroyIfExists("Campus_Ground");

        // -------------------------------------------
        // Racine du collège
        // -------------------------------------------

        GameObject root =
            new GameObject(
                "College_From_GitHub"
            );

        // -------------------------------------------
        // Grand terrain
        // -------------------------------------------

        Material groundMaterial =
            CreateMaterial(
                "#7A8676"
            );

        GameObject campusGround =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        campusGround.name =
            "Campus_Ground";

        campusGround.transform.position =
            new Vector3(
                0f,
                -0.15f,
                0f
            );

        campusGround.transform.localScale =
            new Vector3(
                data.mapWidth * CELL_SIZE,
                0.3f,
                data.mapHeight * CELL_SIZE
            );

        campusGround
            .GetComponent<Renderer>()
            .sharedMaterial =
            groundMaterial;

        campusGround.transform.SetParent(
            root.transform
        );

        // -------------------------------------------
        // Création de toutes les zones
        // -------------------------------------------

        foreach (
            BuildingData b
            in data.buildings
        )
        {
            if (b.floor == "EXT")
            {
                CreateExterior(
                    b,
                    data,
                    root.transform
                );
            }
            else if (b.floor == "STAIRS")
            {
                CreateStairMarker(
                    b,
                    data,
                    root.transform
                );
            }
            else
            {
                CreateBuilding(
                    b,
                    data,
                    root.transform
                );
            }
        }

        // -------------------------------------------
        // Repositionnement du joueur
        // près de l'entrée principale
        // -------------------------------------------

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            CharacterController cc =
                player.GetComponent<
                    CharacterController
                >();

            if (cc != null)
                cc.enabled = false;

            // Centre approximatif de l'entrée :
            // x=21..25
            // y=29..32

            player.transform.position =
                MapPoint(
                    23f,
                    30.5f,
                    data,
                    0.12f
                );

            // Face vers la partie centrale
            player.transform.rotation =
                Quaternion.Euler(
                    0f,
                    180f,
                    0f
                );

            if (cc != null)
                cc.enabled = true;
        }

        // -------------------------------------------
        // Lumière
        // -------------------------------------------

        GameObject lightObject =
            GameObject.Find(
                "Directional Light"
            );

        if (lightObject == null)
        {
            lightObject =
                new GameObject(
                    "Directional Light"
                );

            Light light =
                lightObject.AddComponent<
                    Light
                >();

            light.type =
                LightType.Directional;

            light.intensity =
                1.2f;

            lightObject.transform.rotation =
                Quaternion.Euler(
                    50f,
                    -30f,
                    0f
                );
        }

        // -------------------------------------------
        // Sauvegarde
        // -------------------------------------------

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        Debug.Log(
            "COLLEGE TREASURE 3D "
            + "- ETAPE 4 TERMINEE"
        );
    }

    // ===============================================
    // BATIMENTS
    // ===============================================

    static void CreateBuilding(
        BuildingData b,
        CollegeData data,
        Transform parent
    )
    {
        int stories =
            StoryCount(b.floor);

        float totalHeight =
            stories * STORY_HEIGHT;

        Vector3 position =
            MapRectCenter(
                b,
                data,
                totalHeight / 2f
            );

        Vector3 scale =
            new Vector3(
                b.width * CELL_SIZE,
                totalHeight,
                b.height * CELL_SIZE
            );

        GameObject building =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        building.name =
            "[" + b.floor + "] "
            + b.label
            + " (" + b.id + ")";

        building.transform.position =
            position;

        building.transform.localScale =
            scale;

        building.transform.SetParent(
            parent
        );

        Material material =
            CreateMaterial(
                b.color
            );

        building
            .GetComponent<Renderer>()
            .sharedMaterial =
            material;
    }

    // ===============================================
    // ZONES EXTERIEURES
    // ===============================================

    static void CreateExterior(
        BuildingData b,
        CollegeData data,
        Transform parent
    )
    {
        GameObject area =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        area.name =
            "[EXT] " + b.label;

        area.transform.position =
            MapRectCenter(
                b,
                data,
                0.04f
            );

        area.transform.localScale =
            new Vector3(
                b.width * CELL_SIZE,
                0.08f,
                b.height * CELL_SIZE
            );

        area.transform.SetParent(parent);

        area
            .GetComponent<Renderer>()
            .sharedMaterial =
            CreateMaterial(
                b.color
            );

        Collider collider =
            area.GetComponent<Collider>();

        if (collider != null)
            collider.enabled = false;
    }

    // ===============================================
    // EMPLACEMENTS ESCALIERS
    // ===============================================

    static void CreateStairMarker(
        BuildingData b,
        CollegeData data,
        Transform parent
    )
    {
        GameObject stairs =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        stairs.name =
            "[ESCALIER] " + b.id;

        stairs.transform.position =
            MapRectCenter(
                b,
                data,
                0.09f
            );

        stairs.transform.localScale =
            new Vector3(
                b.width * CELL_SIZE,
                0.18f,
                b.height * CELL_SIZE
            );

        stairs.transform.SetParent(
            parent
        );

        stairs
            .GetComponent<Renderer>()
            .sharedMaterial =
            CreateMaterial(
                b.color
            );

        Collider collider =
            stairs.GetComponent<Collider>();

        if (collider != null)
            collider.enabled = false;
    }

    // ===============================================
    // CONVERSION PLAN -> UNITY
    // ===============================================

    static Vector3 MapRectCenter(
        BuildingData b,
        CollegeData data,
        float worldY
    )
    {
        float mapCenterX =
            data.mapWidth / 2f;

        float mapCenterZ =
            data.mapHeight / 2f;

        float x =
            (
                b.x
                + b.width / 2f
                - mapCenterX
            )
            * CELL_SIZE;

        float z =
            (
                b.y
                + b.height / 2f
                - mapCenterZ
            )
            * CELL_SIZE;

        return new Vector3(
            x,
            worldY,
            z
        );
    }

    static Vector3 MapPoint(
        float mapX,
        float mapY,
        CollegeData data,
        float worldY
    )
    {
        return new Vector3(
            (
                mapX
                - data.mapWidth / 2f
            )
            * CELL_SIZE,

            worldY,

            (
                mapY
                - data.mapHeight / 2f
            )
            * CELL_SIZE
        );
    }

    // ===============================================
    // NOMBRE D'ETAGES
    // ===============================================

    static int StoryCount(
        string floor
    )
    {
        switch (floor)
        {
            case "R+2":
                return 3;

            case "R+1":
                return 2;

            case "RDC":
                return 1;

            case "SPECIAL":
                return 1;

            default:
                return 1;
        }
    }

    // ===============================================
    // MATERIAUX
    // ===============================================

    static Material CreateMaterial(
        string htmlColor
    )
    {
        string key =
            htmlColor
            .Replace("#", "")
            .ToLowerInvariant();

        string path =
            "Assets/Materials/"
            + "Map_"
            + key
            + ".mat";

        Material existing =
            AssetDatabase.LoadAssetAtPath<
                Material
            >(path);

        Color color =
            Color.gray;

        ColorUtility.TryParseHtmlString(
            htmlColor,
            out color
        );

        if (existing != null)
        {
            existing.color =
                color;

            return existing;
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

        if (shader == null)
        {
            throw new Exception(
                "Shader Unity introuvable"
            );
        }

        Material material =
            new Material(shader);

        material.color =
            color;

        AssetDatabase.CreateAsset(
            material,
            path
        );

        return material;
    }

    static void DestroyIfExists(
        string name
    )
    {
        GameObject obj =
            GameObject.Find(name);

        if (obj != null)
        {
            UnityEngine.Object
                .DestroyImmediate(obj);
        }
    }
}

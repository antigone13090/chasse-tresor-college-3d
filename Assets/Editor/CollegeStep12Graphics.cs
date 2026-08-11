using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;

public static class CollegeStep12Graphics
{
    public static void Apply()
    {
        string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(scenePath);

        Debug.Log("=== ETAPE 12 : GRAPHISMES ===");

        SetupSky();
        SetupLighting();
        SetupFog();
        SetupQuality();
        ApplyMaterials();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 12 TERMINEE - CIEL + LUMIERE + MATERIAUX"
        );
    }

    // =====================================================
    // CIEL
    // =====================================================

    static void SetupSky()
    {
        Shader skyShader =
            Shader.Find("Skybox/Procedural");

        if (skyShader == null)
        {
            Debug.LogWarning(
                "Skybox/Procedural non disponible."
            );

            return;
        }

        string path =
            "Assets/Materials/College_Sky.mat";

        Material sky =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (sky == null)
        {
            sky =
                new Material(skyShader);

            AssetDatabase.CreateAsset(
                sky,
                path
            );
        }

        if (sky.HasProperty("_SunSize"))
            sky.SetFloat("_SunSize", 0.035f);

        if (sky.HasProperty("_SunSizeConvergence"))
            sky.SetFloat(
                "_SunSizeConvergence",
                5f
            );

        if (sky.HasProperty("_AtmosphereThickness"))
            sky.SetFloat(
                "_AtmosphereThickness",
                1.15f
            );

        if (sky.HasProperty("_SkyTint"))
            sky.SetColor(
                "_SkyTint",
                new Color(
                    0.40f,
                    0.56f,
                    0.76f
                )
            );

        if (sky.HasProperty("_GroundColor"))
            sky.SetColor(
                "_GroundColor",
                new Color(
                    0.38f,
                    0.36f,
                    0.32f
                )
            );

        if (sky.HasProperty("_Exposure"))
            sky.SetFloat(
                "_Exposure",
                1.15f
            );

        RenderSettings.skybox = sky;

        RenderSettings.ambientMode =
            AmbientMode.Trilight;

        RenderSettings.ambientSkyColor =
            new Color(
                0.58f,
                0.68f,
                0.82f
            );

        RenderSettings.ambientEquatorColor =
            new Color(
                0.46f,
                0.49f,
                0.50f
            );

        RenderSettings.ambientGroundColor =
            new Color(
                0.25f,
                0.24f,
                0.22f
            );

        RenderSettings.ambientIntensity =
            0.85f;

        RenderSettings.reflectionIntensity =
            0.75f;

        DynamicGI.UpdateEnvironment();
    }

    // =====================================================
    // SOLEIL
    // =====================================================

    static void SetupLighting()
    {
        Light sun = null;

        GameObject directional =
            GameObject.Find(
                "Directional Light"
            );

        if (directional != null)
            sun =
                directional.GetComponent<Light>();

        if (sun == null)
        {
            GameObject obj =
                new GameObject(
                    "Directional Light"
                );

            sun =
                obj.AddComponent<Light>();

            sun.type =
                LightType.Directional;
        }

        sun.transform.rotation =
            Quaternion.Euler(
                48f,
                -32f,
                0f
            );

        sun.color =
            new Color(
                1.0f,
                0.94f,
                0.84f
            );

        sun.intensity = 1.15f;

        sun.shadows =
            LightShadows.Soft;

        sun.shadowStrength =
            0.82f;

        sun.shadowBias =
            0.04f;

        sun.shadowNormalBias =
            0.35f;

        RenderSettings.sun = sun;
    }

    // =====================================================
    // PROFONDEUR ATMOSPHERIQUE
    // =====================================================

    static void SetupFog()
    {
        RenderSettings.fog = true;

        RenderSettings.fogMode =
            FogMode.Linear;

        RenderSettings.fogColor =
            new Color(
                0.67f,
                0.72f,
                0.76f
            );

        RenderSettings.fogStartDistance =
            75f;

        RenderSettings.fogEndDistance =
            155f;
    }

    // =====================================================
    // QUALITE
    // =====================================================

    static void SetupQuality()
    {
        QualitySettings.shadowDistance =
            120f;

        QualitySettings.shadowResolution =
            ShadowResolution.High;

        QualitySettings.shadowCascades = 4;

        QualitySettings.lodBias = 1.5f;

        if (
            GraphicsSettings.currentRenderPipeline
            == null
        )
        {
            QualitySettings.antiAliasing = 4;
        }
    }

    // =====================================================
    // MATERIAUX
    // =====================================================

    static void ApplyMaterials()
    {
        Material wall =
            MakeMaterial(
                "GFX_Walls",
                new Color(
                    0.79f,
                    0.80f,
                    0.77f
                ),
                0f,
                0.20f
            );

        Material exterior =
            MakeMaterial(
                "GFX_ExteriorGround",
                new Color(
                    0.47f,
                    0.50f,
                    0.45f
                ),
                0f,
                0.10f
            );

        Material concrete =
            MakeMaterial(
                "GFX_Concrete",
                new Color(
                    0.58f,
                    0.58f,
                    0.54f
                ),
                0f,
                0.18f
            );

        Material corridor =
            MakeMaterial(
                "GFX_CorridorFloor",
                new Color(
                    0.48f,
                    0.45f,
                    0.39f
                ),
                0f,
                0.25f
            );

        Material wood =
            MakeMaterial(
                "GFX_Wood",
                new Color(
                    0.34f,
                    0.17f,
                    0.07f
                ),
                0f,
                0.32f
            );

        Material stairs =
            MakeMaterial(
                "GFX_Stairs",
                new Color(
                    0.15f,
                    0.40f,
                    0.63f
                ),
                0f,
                0.25f
            );

        Material gym =
            MakeMaterial(
                "GFX_GymFloor",
                new Color(
                    0.55f,
                    0.34f,
                    0.16f
                ),
                0f,
                0.42f
            );

        Material self =
            MakeMaterial(
                "GFX_SelfFloor",
                new Color(
                    0.67f,
                    0.65f,
                    0.57f
                ),
                0f,
                0.30f
            );

        Material auditorium =
            MakeMaterial(
                "GFX_AuditoriumFloor",
                new Color(
                    0.22f,
                    0.18f,
                    0.17f
                ),
                0f,
                0.18f
            );

        Material arts =
            MakeMaterial(
                "GFX_ArtsFloor",
                new Color(
                    0.65f,
                    0.54f,
                    0.32f
                ),
                0f,
                0.30f
            );

        Renderer[] renderers =
            Object.FindObjectsByType<Renderer>(
                FindObjectsSortMode.None
            );

        int changed = 0;

        foreach (Renderer r in renderers)
        {
            if (r == null)
                continue;

            if (
                r.GetComponentInParent<ClueObject>()
                != null
            )
                continue;

            if (
                r.GetComponentInParent<TreasureObject>()
                != null
            )
                continue;

            string n =
                r.gameObject.name.ToLower();

            string parent =
                r.transform.parent != null
                ? r.transform.parent.name.ToLower()
                : "";

            string all =
                n + " " + parent;

            // -----------------------------
            // ESCALIERS
            // -----------------------------

            if (
                all.Contains("step")
                ||
                all.Contains("escalier")
                ||
                all.Contains("stairs")
            )
            {
                r.sharedMaterial = stairs;
                changed++;
                continue;
            }

            // -----------------------------
            // PORTES
            // -----------------------------

            if (
                all.Contains("door")
                ||
                all.Contains("porte")
            )
            {
                r.sharedMaterial = wood;
                changed++;
                continue;
            }

            // -----------------------------
            // SOL GYMNASE
            // -----------------------------

            if (
                all.Contains("gym_floor")
                ||
                all.Contains("gymnase_floor")
            )
            {
                r.sharedMaterial = gym;
                changed++;
                continue;
            }

            // -----------------------------
            // SELF
            // -----------------------------

            if (
                all.Contains("self_floor")
            )
            {
                r.sharedMaterial = self;
                changed++;
                continue;
            }

            // -----------------------------
            // AUDITORIUM
            // -----------------------------

            if (
                all.Contains("auditorium_floor")
                ||
                all.Contains("aud_floor")
            )
            {
                r.sharedMaterial =
                    auditorium;

                changed++;
                continue;
            }

            // -----------------------------
            // ARTS / MUSIQUE
            // -----------------------------

            if (
                all.Contains("am_floor")
                ||
                all.Contains("arts_floor")
            )
            {
                r.sharedMaterial = arts;
                changed++;
                continue;
            }

            // -----------------------------
            // MURS
            // -----------------------------

            if (
                all.Contains("wall")
                ||
                all.Contains("north")
                ||
                all.Contains("south")
                ||
                all.Contains("east")
                ||
                all.Contains("west")
                ||
                all.Contains("header")
            )
            {
                r.sharedMaterial = wall;
                changed++;
                continue;
            }

            // -----------------------------
            // SOLS / COULOIRS
            // -----------------------------

            if (
                all.Contains("floor")
                ||
                all.Contains("path")
                ||
                all.Contains("passerelle")
                ||
                all.Contains("landing")
            )
            {
                r.sharedMaterial = corridor;
                changed++;
                continue;
            }

            // -----------------------------
            // GRAND TERRAIN
            // -----------------------------

            if (
                n == "ground"
                ||
                n == "campus_ground"
            )
            {
                r.sharedMaterial = exterior;
                changed++;
                continue;
            }

            // -----------------------------
            // COUR / EXTERIEUR
            // -----------------------------

            if (
                all.Contains("cour")
                ||
                all.Contains("parvis")
                ||
                all.Contains("preau")
            )
            {
                r.sharedMaterial = concrete;
                changed++;
                continue;
            }
        }

        Debug.Log(
            "Materiaux modifies : "
            + changed
        );
    }

    // =====================================================
    // CREATION MATERIAU PBR
    // =====================================================

    static Material MakeMaterial(
        string name,
        Color color,
        float metallic,
        float smoothness)
    {
        string path =
            "Assets/Materials/"
            + name
            + ".mat";

        Material mat =
            AssetDatabase.LoadAssetAtPath<Material>(
                path
            );

        if (mat == null)
        {
            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit"
                );

            if (shader == null)
                shader =
                    Shader.Find(
                        "Standard"
                    );

            mat =
                new Material(shader);

            AssetDatabase.CreateAsset(
                mat,
                path
            );
        }

        mat.color = color;

        if (
            mat.HasProperty("_Metallic")
        )
        {
            mat.SetFloat(
                "_Metallic",
                metallic
            );
        }

        if (
            mat.HasProperty("_Smoothness")
        )
        {
            mat.SetFloat(
                "_Smoothness",
                smoothness
            );
        }

        if (
            mat.HasProperty("_Glossiness")
        )
        {
            mat.SetFloat(
                "_Glossiness",
                smoothness
            );
        }

        EditorUtility.SetDirty(mat);

        return mat;
    }
}

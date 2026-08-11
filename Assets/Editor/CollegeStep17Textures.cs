using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class CollegeStep17Textures
{
    const string ROOT =
        "Assets/Textures/ambientCG";

    public static void Apply()
    {
        const string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        Debug.Log(
            "=== ETAPE 17 : TEXTURES PBR ==="
        );

        ConfigureAsset(
            "Concrete010"
        );

        ConfigureAsset(
            "WoodFloor041"
        );

        ConfigureAsset(
            "Tiles040"
        );

        ConfigureAsset(
            "Grass008"
        );

        ConfigureAsset(
            "Metal036"
        );

        // =================================================
        // EXTERIEUR
        // =================================================

        ApplyMaterial(
            "GFX_ExteriorGround",
            "Grass008",
            new Color(
                .72f,.82f,.68f,1f
            ),
            10f,
            0f,
            .15f,
            .90f
        );

        ApplyMaterial(
            "GFX_Concrete",
            "Concrete010",
            new Color(
                .82f,.82f,.80f,1f
            ),
            6f,
            0f,
            .18f,
            .75f
        );

        // =================================================
        // MURS
        // =================================================

        ApplyMaterial(
            "GFX_Walls",
            "Concrete010",
            new Color(
                .94f,.93f,.88f,1f
            ),
            5f,
            0f,
            .15f,
            .62f
        );

        // =================================================
        // COULOIRS / SOLS
        // =================================================

        ApplyMaterial(
            "GFX_CorridorFloor",
            "Tiles040",
            new Color(
                .82f,.80f,.74f,1f
            ),
            6f,
            0f,
            .35f,
            .72f
        );

        ApplyMaterial(
            "GFX_SelfFloor",
            "Tiles040",
            new Color(
                .94f,.91f,.82f,1f
            ),
            7f,
            0f,
            .38f,
            .70f
        );

        // =================================================
        // PARQUET
        // =================================================

        ApplyMaterial(
            "GFX_GymFloor",
            "WoodFloor041",
            new Color(
                .92f,.72f,.48f,1f
            ),
            5f,
            0f,
            .38f,
            .85f
        );

        ApplyMaterial(
            "GFX_ArtsFloor",
            "WoodFloor041",
            new Color(
                .88f,.73f,.53f,1f
            ),
            4f,
            0f,
            .32f,
            .75f
        );

        ApplyMaterial(
            "GFX_AuditoriumFloor",
            "WoodFloor041",
            new Color(
                .43f,.31f,.25f,1f
            ),
            5f,
            0f,
            .25f,
            .80f
        );

        // =================================================
        // PORTES / BOIS
        // =================================================

        ApplyMaterial(
            "GFX_Wood",
            "WoodFloor041",
            new Color(
                .55f,.32f,.16f,1f
            ),
            2.2f,
            0f,
            .32f,
            .80f
        );

        ApplyMaterial(
            "DECOR_BenchWood",
            "WoodFloor041",
            new Color(
                .62f,.39f,.20f,1f
            ),
            2.5f,
            0f,
            .28f,
            .75f
        );

        ApplyMaterial(
            "INT_Wood",
            "WoodFloor041",
            new Color(
                .66f,.42f,.23f,1f
            ),
            2.5f,
            0f,
            .30f,
            .72f
        );

        ApplyMaterial(
            "INT_DarkWood",
            "WoodFloor041",
            new Color(
                .36f,.20f,.11f,1f
            ),
            2.5f,
            0f,
            .25f,
            .75f
        );

        // =================================================
        // METAL
        // =================================================

        ApplyMaterial(
            "GFX_Stairs",
            "Metal036",
            new Color(
                .30f,.48f,.65f,1f
            ),
            4f,
            .72f,
            .42f,
            .85f
        );

        ApplyMaterial(
            "DECOR_Metal",
            "Metal036",
            new Color(
                .40f,.42f,.44f,1f
            ),
            3f,
            .82f,
            .42f,
            .82f
        );

        ApplyMaterial(
            "INT_Metal",
            "Metal036",
            new Color(
                .44f,.46f,.48f,1f
            ),
            3f,
            .78f,
            .40f,
            .82f
        );

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 17 TERMINEE - TEXTURES PBR APPLIQUEES"
        );
    }

    // =====================================================
    // CONFIGURATION DES TEXTURES
    // =====================================================

    static void ConfigureAsset(
        string id)
    {
        string color =
            FindMap(
                id,
                "Color"
            );

        string normal =
            FindMap(
                id,
                "NormalGL"
            );

        string ao =
            FindMap(
                id,
                "AmbientOcclusion"
            );

        SetupTexture(
            color,
            false
        );

        SetupTexture(
            normal,
            true
        );

        SetupTexture(
            ao,
            false,
            false
        );

        Debug.Log(
            id
            + " : Color="
            + (!string.IsNullOrEmpty(color))
            + " Normal="
            + (!string.IsNullOrEmpty(normal))
            + " AO="
            + (!string.IsNullOrEmpty(ao))
        );
    }

    static void SetupTexture(
        string path,
        bool normal,
        bool sRGB = true)
    {
        if (
            string.IsNullOrEmpty(path)
        )
            return;

        TextureImporter importer =
            AssetImporter.GetAtPath(
                path
            )
            as TextureImporter;

        if (importer == null)
            return;

        importer.textureType =
            normal
            ? TextureImporterType.NormalMap
            : TextureImporterType.Default;

        importer.sRGBTexture =
            normal
            ? false
            : sRGB;

        importer.wrapMode =
            TextureWrapMode.Repeat;

        importer.filterMode =
            FilterMode.Trilinear;

        importer.anisoLevel = 8;

        importer.mipmapEnabled =
            true;

        importer.textureCompression =
            TextureImporterCompression
                .CompressedHQ;

        importer.SaveAndReimport();
    }

    // =====================================================
    // APPLICATION PBR
    // =====================================================

    static void ApplyMaterial(
        string materialName,
        string textureId,
        Color tint,
        float tiling,
        float metallic,
        float smoothness,
        float normalStrength)
    {
        string materialPath =
            "Assets/Materials/"
            + materialName
            + ".mat";

        Material mat =
            AssetDatabase
            .LoadAssetAtPath<Material>(
                materialPath
            );

        if (mat == null)
        {
            Debug.LogWarning(
                "Materiau absent : "
                + materialName
            );

            return;
        }

        string colorPath =
            FindMap(
                textureId,
                "Color"
            );

        string normalPath =
            FindMap(
                textureId,
                "NormalGL"
            );

        string aoPath =
            FindMap(
                textureId,
                "AmbientOcclusion"
            );

        Texture2D color =
            LoadTexture(
                colorPath
            );

        Texture2D normal =
            LoadTexture(
                normalPath
            );

        Texture2D ao =
            LoadTexture(
                aoPath
            );

        // -------------------------------------------------
        // COULEUR
        // -------------------------------------------------

        if (
            mat.HasProperty(
                "_BaseColor"
            )
        )
        {
            mat.SetColor(
                "_BaseColor",
                tint
            );
        }

        if (
            mat.HasProperty(
                "_Color"
            )
        )
        {
            mat.SetColor(
                "_Color",
                tint
            );
        }

        // -------------------------------------------------
        // ALBEDO
        // -------------------------------------------------

        if (
            color != null
            &&
            mat.HasProperty(
                "_BaseMap"
            )
        )
        {
            mat.SetTexture(
                "_BaseMap",
                color
            );

            mat.SetTextureScale(
                "_BaseMap",
                Vector2.one * tiling
            );
        }

        if (
            color != null
            &&
            mat.HasProperty(
                "_MainTex"
            )
        )
        {
            mat.SetTexture(
                "_MainTex",
                color
            );

            mat.SetTextureScale(
                "_MainTex",
                Vector2.one * tiling
            );
        }

        // -------------------------------------------------
        // NORMAL MAP
        // -------------------------------------------------

        if (
            normal != null
            &&
            mat.HasProperty(
                "_BumpMap"
            )
        )
        {
            mat.SetTexture(
                "_BumpMap",
                normal
            );

            mat.SetTextureScale(
                "_BumpMap",
                Vector2.one * tiling
            );

            if (
                mat.HasProperty(
                    "_BumpScale"
                )
            )
            {
                mat.SetFloat(
                    "_BumpScale",
                    normalStrength
                );
            }

            mat.EnableKeyword(
                "_NORMALMAP"
            );
        }

        // -------------------------------------------------
        // AMBIENT OCCLUSION
        // -------------------------------------------------

        if (
            ao != null
            &&
            mat.HasProperty(
                "_OcclusionMap"
            )
        )
        {
            mat.SetTexture(
                "_OcclusionMap",
                ao
            );

            mat.SetTextureScale(
                "_OcclusionMap",
                Vector2.one * tiling
            );

            if (
                mat.HasProperty(
                    "_OcclusionStrength"
                )
            )
            {
                mat.SetFloat(
                    "_OcclusionStrength",
                    .72f
                );
            }

            mat.EnableKeyword(
                "_OCCLUSIONMAP"
            );
        }

        // -------------------------------------------------
        // PBR
        // -------------------------------------------------

        if (
            mat.HasProperty(
                "_Metallic"
            )
        )
        {
            mat.SetFloat(
                "_Metallic",
                metallic
            );
        }

        if (
            mat.HasProperty(
                "_Smoothness"
            )
        )
        {
            mat.SetFloat(
                "_Smoothness",
                smoothness
            );
        }

        if (
            mat.HasProperty(
                "_Glossiness"
            )
        )
        {
            mat.SetFloat(
                "_Glossiness",
                smoothness
            );
        }

        EditorUtility.SetDirty(
            mat
        );

        Debug.Log(
            "PBR : "
            + materialName
            + " <- "
            + textureId
        );
    }

    // =====================================================
    // RECHERCHE DES MAPS
    // =====================================================

    static string FindMap(
        string id,
        string suffix)
    {
        if (
            !Directory.Exists(ROOT)
        )
            return null;

        string[] files =
            Directory.GetFiles(
                ROOT,
                id
                + "_*"
                + suffix
                + ".*",
                SearchOption.AllDirectories
            );

        if (
            files == null
            ||
            files.Length == 0
        )
            return null;

        return files[0]
            .Replace(
                "\\",
                "/"
            );
    }

    static Texture2D LoadTexture(
        string path)
    {
        if (
            string.IsNullOrEmpty(path)
        )
            return null;

        return AssetDatabase
            .LoadAssetAtPath<Texture2D>(
                path
            );
    }
}

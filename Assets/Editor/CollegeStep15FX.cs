using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class CollegeStep15FX
{
    public static void Apply()
    {
        const string scenePath =
            "Assets/Scenes/CollegePrototype.unity";

        var scene =
            EditorSceneManager.OpenScene(
                scenePath
            );

        AudioClip clue =
            AssetDatabase.LoadAssetAtPath<AudioClip>(
                "Assets/Audio/fx_clue.wav"
            );

        AudioClip footstep =
            AssetDatabase.LoadAssetAtPath<AudioClip>(
                "Assets/Audio/fx_footstep.wav"
            );

        AudioClip door =
            AssetDatabase.LoadAssetAtPath<AudioClip>(
                "Assets/Audio/fx_door.wav"
            );

        AudioClip victory =
            AssetDatabase.LoadAssetAtPath<AudioClip>(
                "Assets/Audio/fx_victory.wav"
            );

        if (
            clue == null
            ||
            footstep == null
            ||
            door == null
            ||
            victory == null
        )
        {
            Debug.LogError(
                "Un ou plusieurs fichiers audio sont absents."
            );

            return;
        }

        RemovePreviousComponents();

        AddGlobalAudio(
            clue,
            footstep,
            victory
        );

        AddDoorSounds(
            door
        );

        AddClueEffects();

        AddTreasureEffect();

        EditorSceneManager.SaveScene(
            scene,
            scenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "ETAPE 15 TERMINEE - AUDIO + HALOS + PARTICULES"
        );
    }

    static void RemovePreviousComponents()
    {
        CollegeAudioFX[] audio =
            Object.FindObjectsByType<
                CollegeAudioFX
            >(
                FindObjectsSortMode.None
            );

        foreach (var c in audio)
            Object.DestroyImmediate(
                c.gameObject
            );

        DoorSoundFX[] doors =
            Object.FindObjectsByType<
                DoorSoundFX
            >(
                FindObjectsSortMode.None
            );

        foreach (var c in doors)
            Object.DestroyImmediate(c);

        ClueGlowPulse[] clues =
            Object.FindObjectsByType<
                ClueGlowPulse
            >(
                FindObjectsSortMode.None
            );

        foreach (var c in clues)
            Object.DestroyImmediate(c);

        TreasureCelebrationFX[] treasure =
            Object.FindObjectsByType<
                TreasureCelebrationFX
            >(
                FindObjectsSortMode.None
            );

        foreach (var c in treasure)
            Object.DestroyImmediate(
                c.gameObject
            );

        GameObject old =
            GameObject.Find(
                "TreasureVictoryFX"
            );

        if (old != null)
            Object.DestroyImmediate(old);
    }

    static void AddGlobalAudio(
        AudioClip clue,
        AudioClip footstep,
        AudioClip victory)
    {
        GameObject obj =
            new GameObject(
                "GameFeedbackFX"
            );

        CollegeAudioFX fx =
            obj.AddComponent<
                CollegeAudioFX
            >();

        fx.clueSound = clue;
        fx.footstepSound = footstep;
        fx.victorySound = victory;
    }

    static void AddDoorSounds(
        AudioClip doorClip)
    {
        DoorController[] doors =
            Object.FindObjectsByType<
                DoorController
            >(
                FindObjectsSortMode.None
            );

        foreach (
            DoorController door
            in doors)
        {
            DoorSoundFX fx =
                door.gameObject
                .AddComponent<DoorSoundFX>();

            fx.doorSound =
                doorClip;
        }

        Debug.Log(
            "Portes sonorisees : "
            + doors.Length
        );
    }

    static void AddClueEffects()
    {
        ClueObject[] clues =
            Object.FindObjectsByType<
                ClueObject
            >(
                FindObjectsSortMode.None
            );

        foreach (
            ClueObject clue
            in clues)
        {
            Transform existing =
                clue.transform.Find(
                    "IndiceGlow"
                );

            if (existing != null)
                Object.DestroyImmediate(
                    existing.gameObject
                );

            GameObject lightObject =
                new GameObject(
                    "IndiceGlow"
                );

            lightObject.transform.SetParent(
                clue.transform,
                false
            );

            lightObject.transform.localPosition =
                Vector3.up * .35f;

            Light light =
                lightObject.AddComponent<Light>();

            light.type =
                LightType.Point;

            light.color =
                new Color(
                    .18f,
                    1f,
                    .32f
                );

            light.range = 4.5f;
            light.intensity = 2.6f;

            ClueGlowPulse pulse =
                clue.gameObject
                .AddComponent<ClueGlowPulse>();

            pulse.glow = light;
        }

        Debug.Log(
            "Indices animes : "
            + clues.Length
        );
    }

    static void AddTreasureEffect()
    {
        GameObject treasure =
            GameObject.Find(
                "Tresor_Final"
            );

        if (treasure == null)
        {
            Debug.LogWarning(
                "Tresor_Final absent."
            );

            return;
        }

        GameObject fx =
            new GameObject(
                "TreasureVictoryFX"
            );

        fx.transform.position =
            treasure.transform.position
            + Vector3.up * 1.5f;

        Light glow =
            fx.AddComponent<Light>();

        glow.type =
            LightType.Point;

        glow.color =
            new Color(
                1f,
                .66f,
                .12f
            );

        glow.range = 7f;
        glow.intensity = 1.2f;

        ParticleSystem ps =
            fx.AddComponent<
                ParticleSystem
            >();

        var main = ps.main;

        main.loop = false;
        main.duration = 2.4f;

        main.startLifetime =
            new ParticleSystem
            .MinMaxCurve(
                1.1f,
                2.2f
            );

        main.startSpeed =
            new ParticleSystem
            .MinMaxCurve(
                1.8f,
                4.0f
            );

        main.startSize =
            new ParticleSystem
            .MinMaxCurve(
                .07f,
                .17f
            );

        main.startColor =
            new Color(
                1f,
                .70f,
                .12f,
                1f
            );

        main.gravityModifier =
            .35f;

        var emission = ps.emission;

        emission.rateOverTime = 0f;

        emission.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(
                    0f,
                    80
                ),
                new ParticleSystem.Burst(
                    .25f,
                    45
                )
            }
        );

        var shape = ps.shape;

        shape.shapeType =
            ParticleSystemShapeType.Sphere;

        shape.radius = .65f;

        TreasureCelebrationFX celebration =
            fx.AddComponent<
                TreasureCelebrationFX
            >();

        celebration.particles = ps;
        celebration.celebrationLight = glow;
    }
}

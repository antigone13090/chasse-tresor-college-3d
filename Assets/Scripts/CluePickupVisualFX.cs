using System.Collections.Generic;
using UnityEngine;

public class CluePickupVisualFX : MonoBehaviour
{
    TreasureGameManager game;
    int lastClues;

    readonly HashSet<ClueObject> playedClues =
        new HashSet<ClueObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        if (FindFirstObjectByType<CluePickupVisualFX>() != null)
            return;

        GameObject host = new GameObject("CluePickupVisualFX_IA");
        host.AddComponent<CluePickupVisualFX>();
    }

    void Start()
    {
        game = FindFirstObjectByType<TreasureGameManager>();

        if (game != null)
            lastClues = game.foundClues;

        foreach (ClueObject clue in FindObjectsByType<ClueObject>(FindObjectsSortMode.None))
        {
            if (clue.collected)
                playedClues.Add(clue);
        }
    }

    void Update()
    {
        if (game == null)
        {
            game = FindFirstObjectByType<TreasureGameManager>();
            if (game == null)
                return;
        }

        if (game.foundClues <= lastClues)
            return;

        foreach (ClueObject clue in FindObjectsByType<ClueObject>(FindObjectsSortMode.None))
        {
            if (!clue.collected || playedClues.Contains(clue))
                continue;

            playedClues.Add(clue);
            SpawnEffect(clue.transform.position);
        }

        lastClues = game.foundClues;
    }

    void SpawnEffect(Vector3 position)
    {
        GameObject fx = new GameObject("CluePickupParticles_IA");
        fx.transform.position = position + Vector3.up * 0.35f;

        ParticleSystem particles = fx.AddComponent<ParticleSystem>();
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particles.main;
        main.playOnAwake = false;
        main.loop = false;
        main.duration = 0.45f;
        main.startLifetime = 0.75f;
        main.startSpeed = 2.2f;
        main.startSize = 0.12f;
        main.startColor = new Color(0.28f, 0.88f, 0.78f, 1f);
        main.maxParticles = 24;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, 18)
        });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.18f;

        particles.Play();
        Destroy(fx, 1.6f);
    }
}

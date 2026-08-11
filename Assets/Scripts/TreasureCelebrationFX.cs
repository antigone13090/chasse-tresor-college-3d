using UnityEngine;

public class TreasureCelebrationFX : MonoBehaviour
{
    public ParticleSystem particles;
    public Light celebrationLight;

    TreasureGameManager game;

    bool played;

    float timer;

    void Start()
    {
        game =
            FindFirstObjectByType<TreasureGameManager>();

        if (particles != null)
            particles.Stop(
                true,
                ParticleSystemStopBehavior
                .StopEmittingAndClear
            );

        if (celebrationLight != null)
            celebrationLight.intensity = 1.2f;
    }

    void Update()
    {
        if (
            game == null
            ||
            !game.victory
        )
            return;

        if (!played)
        {
            played = true;
            timer = 3f;

            if (particles != null)
                particles.Play();
        }

        if (
            timer > 0f
            &&
            celebrationLight != null
        )
        {
            timer -= Time.deltaTime;

            celebrationLight.intensity =
                3.2f
                + Mathf.Sin(
                    Time.time * 10f
                ) * 1.4f;
        }
    }
}

using UnityEngine;

public class CollegeAudioFX : MonoBehaviour
{
    public AudioClip clueSound;
    public AudioClip footstepSound;
    public AudioClip victorySound;

    TreasureGameManager game;
    CharacterController controller;
    AudioSource effects;
    AudioSource footsteps;
    AudioSource music;

    int lastClues;
    bool lastVictory;

    float stepTimer;

    void Start()
    {
        game =
            FindFirstObjectByType<TreasureGameManager>();

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
            controller =
                player.GetComponent<CharacterController>();

        effects =
            gameObject.AddComponent<AudioSource>();

        effects.playOnAwake = false;
        effects.spatialBlend = 0f;
        effects.volume = .75f;

        footsteps =
            gameObject.AddComponent<AudioSource>();

        footsteps.playOnAwake = false;
        footsteps.spatialBlend = 0f;
        footsteps.volume = .28f;

        GameObject background =
            GameObject.Find("BackgroundMusic");

        if (background != null)
            music =
                background.GetComponent<AudioSource>();

        if (game != null)
        {
            lastClues =
                game.foundClues;

            lastVictory =
                game.victory;
        }
    }

    void Update()
    {
        if (game != null)
        {
            if (game.foundClues > lastClues)
            {
                if (clueSound != null)
                    effects.PlayOneShot(
                        clueSound,
                        .85f
                    );
            }

            if (
                game.victory
                &&
                !lastVictory
            )
            {
                if (victorySound != null)
                    effects.PlayOneShot(
                        victorySound,
                        1f
                    );

                if (music != null)
                    music.volume = .10f;
            }

            lastClues =
                game.foundClues;

            lastVictory =
                game.victory;
        }

        UpdateFootsteps();
    }

    void UpdateFootsteps()
    {
        if (
            controller == null
            ||
            footstepSound == null
        )
            return;

        Vector3 velocity =
            controller.velocity;

        velocity.y = 0f;

        bool walking =
            controller.isGrounded
            &&
            velocity.magnitude > .45f;

        if (!walking)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            footsteps.pitch =
                Random.Range(
                    .92f,
                    1.08f
                );

            footsteps.PlayOneShot(
                footstepSound
            );

            float speed =
                velocity.magnitude;

            stepTimer =
                speed > 5.5f
                ? .30f
                : .43f;
        }
    }
}

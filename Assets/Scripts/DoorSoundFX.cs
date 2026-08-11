using UnityEngine;

public class DoorSoundFX : MonoBehaviour
{
    public AudioClip doorSound;

    Quaternion lastRotation;
    bool moving;

    AudioSource source;

    void Start()
    {
        source =
            gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.spatialBlend = 1f;

        source.minDistance = 1.5f;
        source.maxDistance = 12f;

        source.volume = .45f;

        lastRotation =
            transform.localRotation;
    }

    void Update()
    {
        float delta =
            Quaternion.Angle(
                lastRotation,
                transform.localRotation
            );

        bool nowMoving =
            delta > .025f;

        if (
            nowMoving
            &&
            !moving
            &&
            doorSound != null
        )
        {
            source.pitch =
                Random.Range(
                    .93f,
                    1.07f
                );

            source.PlayOneShot(
                doorSound
            );
        }

        moving = nowMoving;

        lastRotation =
            transform.localRotation;
    }
}

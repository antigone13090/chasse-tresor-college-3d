using UnityEngine;

public class ClueGlowPulse : MonoBehaviour
{
    public Light glow;

    ClueObject clue;

    float baseIntensity;
    Vector3 initialPosition;

    void Start()
    {
        clue =
            GetComponent<ClueObject>();

        initialPosition =
            transform.position;

        if (glow != null)
            baseIntensity =
                glow.intensity;
    }

    void Update()
    {
        if (
            clue != null
            &&
            clue.collected
        )
        {
            if (glow != null)
                glow.enabled = false;

            return;
        }

        float wave =
            Mathf.Sin(
                Time.time * 3.2f
            );

        transform.position =
            initialPosition
            + Vector3.up
            * (
                .055f
                + wave * .055f
            );

        transform.Rotate(
            0f,
            30f * Time.deltaTime,
            0f,
            Space.World
        );

        if (glow != null)
        {
            glow.intensity =
                baseIntensity
                * (
                    .82f
                    + .22f * wave
                );
        }
    }
}

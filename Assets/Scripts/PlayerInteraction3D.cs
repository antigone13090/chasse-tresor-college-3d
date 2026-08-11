using UnityEngine;

public class PlayerInteraction3D : MonoBehaviour
{
    public float cluePickupDistance = 2.5f;
    public float treasurePickupDistance = 3f;
    public float interactionDistance = 4f;

    Camera playerCamera;

    void Start()
    {
        playerCamera =
            GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        // Indice proche
        ClueObject clue =
            FindNearestClue();

        if (clue != null)
        {
            clue.Interact();
            return;
        }

        // Trésor proche
        TreasureObject treasure =
            FindNearestTreasure();

        if (treasure != null)
        {
            treasure.Interact();
            return;
        }

        // Objet visé
        if (playerCamera != null)
        {
            Ray ray =
                new Ray(
                    playerCamera.transform.position,
                    playerCamera.transform.forward
                );

            RaycastHit hit;

            if (
                Physics.Raycast(
                    ray,
                    out hit,
                    interactionDistance
                )
            )
            {
                ClueObject aimedClue =
                    hit.collider
                    .GetComponentInParent<ClueObject>();

                if (aimedClue != null)
                {
                    aimedClue.Interact();
                    return;
                }

                TreasureObject aimedTreasure =
                    hit.collider
                    .GetComponentInParent<TreasureObject>();

                if (aimedTreasure != null)
                {
                    aimedTreasure.Interact();
                    return;
                }

                DoorController door =
                    hit.collider
                    .GetComponentInParent<DoorController>();

                if (door != null)
                {
                    door.Toggle();
                    return;
                }
            }
        }

        DoorController nearestDoor =
            FindNearestDoor();

        if (nearestDoor != null)
            nearestDoor.Toggle();
    }

    ClueObject FindNearestClue()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                cluePickupDistance
            );

        ClueObject best = null;
        float distance = float.MaxValue;

        foreach (Collider c in hits)
        {
            ClueObject clue =
                c.GetComponentInParent<ClueObject>();

            if (clue == null || clue.collected)
                continue;

            float d =
                Vector3.Distance(
                    transform.position,
                    clue.transform.position
                );

            if (d < distance)
            {
                distance = d;
                best = clue;
            }
        }

        return best;
    }

    TreasureObject FindNearestTreasure()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                treasurePickupDistance
            );

        TreasureObject best = null;
        float distance = float.MaxValue;

        foreach (Collider c in hits)
        {
            TreasureObject treasure =
                c.GetComponentInParent<TreasureObject>();

            if (
                treasure == null
                ||
                treasure.collected
            )
                continue;

            float d =
                Vector3.Distance(
                    transform.position,
                    treasure.transform.position
                );

            if (d < distance)
            {
                distance = d;
                best = treasure;
            }
        }

        return best;
    }

    DoorController FindNearestDoor()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                interactionDistance
            );

        DoorController best = null;
        float distance = float.MaxValue;

        foreach (Collider c in hits)
        {
            DoorController door =
                c.GetComponentInParent<DoorController>();

            if (door == null)
                continue;

            float d =
                Vector3.Distance(
                    transform.position,
                    door.transform.position
                );

            if (d < distance)
            {
                distance = d;
                best = door;
            }
        }

        return best;
    }
}

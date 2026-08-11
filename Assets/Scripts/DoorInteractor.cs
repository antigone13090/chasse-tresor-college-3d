using UnityEngine;

public class DoorInteractor : MonoBehaviour
{
    public float interactionDistance = 4f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        DoorController door = FindDoor();

        if (door != null)
        {
            door.Toggle();
            Debug.Log("Porte actionnée : " + door.name);
        }
        else
        {
            Debug.Log("Aucune porte à portée.");
        }
    }

    DoorController FindDoor()
    {
        // 1. On privilégie la porte regardée
        if (playerCamera != null)
        {
            Ray ray = new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward
            );

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                DoorController aimedDoor =
                    hit.collider.GetComponentInParent<DoorController>();

                if (aimedDoor != null)
                    return aimedDoor;
            }
        }

        // 2. Sinon on cherche la porte la plus proche
        Collider[] colliders =
            Physics.OverlapSphere(
                transform.position,
                interactionDistance
            );

        DoorController nearestDoor = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            DoorController candidate =
                col.GetComponentInParent<DoorController>();

            if (candidate == null)
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    candidate.transform.position
                );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestDoor = candidate;
            }
        }

        return nearestDoor;
    }
}

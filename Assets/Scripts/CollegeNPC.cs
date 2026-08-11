using UnityEngine;

public class CollegeNPC : MonoBehaviour
{
    public Vector3[] waypoints;

    public float speed = 1.4f;
    public float waitTime = 1.2f;

    public bool walking = true;

    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;
    public Transform body;
    public Transform head;

    int targetIndex = 0;

    float waitTimer = 0f;
    float walkPhase = 0f;

    Vector3 baseBodyPosition;
    Vector3 baseHeadPosition;

    void Start()
    {
        if (body != null)
            baseBodyPosition =
                body.localPosition;

        if (head != null)
            baseHeadPosition =
                head.localPosition;

        if (
            waypoints != null
            &&
            waypoints.Length > 0
        )
        {
            targetIndex =
                FindNearestWaypoint();
        }
    }

    int FindNearestWaypoint()
    {
        int best = 0;
        float bestDistance =
            float.MaxValue;

        for (
            int i = 0;
            i < waypoints.Length;
            i++
        )
        {
            float d =
                Vector3.Distance(
                    transform.position,
                    waypoints[i]
                );

            if (d < bestDistance)
            {
                bestDistance = d;
                best = i;
            }
        }

        return best;
    }

    void Update()
    {
        if (
            !walking
            ||
            waypoints == null
            ||
            waypoints.Length < 2
        )
        {
            IdleAnimation();
            return;
        }

        if (waitTimer > 0f)
        {
            waitTimer -=
                Time.deltaTime;

            IdleAnimation();
            return;
        }

        Vector3 target =
            waypoints[targetIndex];

        Vector3 direction =
            target
            - transform.position;

        direction.y = 0f;

        if (
            direction.magnitude
            < .20f
        )
        {
            targetIndex++;

            if (
                targetIndex
                >= waypoints.Length
            )
            {
                targetIndex = 0;
            }

            waitTimer =
                waitTime
                + Random.Range(
                    0f,
                    .8f
                );

            return;
        }

        direction.Normalize();

        transform.position +=
            direction
            * speed
            * Time.deltaTime;

        Quaternion wantedRotation =
            Quaternion.LookRotation(
                direction,
                Vector3.up
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                wantedRotation,
                Time.deltaTime * 7f
            );

        WalkingAnimation();
    }

    void WalkingAnimation()
    {
        walkPhase +=
            Time.deltaTime
            * speed
            * 7f;

        float swing =
            Mathf.Sin(walkPhase)
            * 28f;

        if (leftArm != null)
        {
            leftArm.localRotation =
                Quaternion.Euler(
                    swing,
                    0f,
                    0f
                );
        }

        if (rightArm != null)
        {
            rightArm.localRotation =
                Quaternion.Euler(
                    -swing,
                    0f,
                    0f
                );
        }

        if (leftLeg != null)
        {
            leftLeg.localRotation =
                Quaternion.Euler(
                    -swing,
                    0f,
                    0f
                );
        }

        if (rightLeg != null)
        {
            rightLeg.localRotation =
                Quaternion.Euler(
                    swing,
                    0f,
                    0f
                );
        }

        float bob =
            Mathf.Abs(
                Mathf.Sin(walkPhase * 2f)
            )
            * .025f;

        if (body != null)
        {
            body.localPosition =
                baseBodyPosition
                + Vector3.up * bob;
        }

        if (head != null)
        {
            head.localPosition =
                baseHeadPosition
                + Vector3.up * bob;
        }
    }

    void IdleAnimation()
    {
        float t =
            Time.time
            + transform.position.x
            * .05f;

        if (leftArm != null)
            leftArm.localRotation =
                Quaternion.Euler(
                    Mathf.Sin(t) * 2f,
                    0f,
                    0f
                );

        if (rightArm != null)
            rightArm.localRotation =
                Quaternion.Euler(
                    -Mathf.Sin(t) * 2f,
                    0f,
                    0f
                );

        if (leftLeg != null)
            leftLeg.localRotation =
                Quaternion.identity;

        if (rightLeg != null)
            rightLeg.localRotation =
                Quaternion.identity;

        if (body != null)
        {
            body.localPosition =
                baseBodyPosition
                + Vector3.up
                * (
                    Mathf.Sin(t * 1.4f)
                    * .008f
                );
        }
    }
}

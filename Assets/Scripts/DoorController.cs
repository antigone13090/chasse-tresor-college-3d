using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = -95f;
    public float speed = 4f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool opened = false;

    void Start()
    {
        closedRotation = transform.localRotation;

        openRotation =
            closedRotation *
            Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        Quaternion target =
            opened ? openRotation : closedRotation;

        transform.localRotation =
            Quaternion.Slerp(
                transform.localRotation,
                target,
                speed * Time.deltaTime
            );
    }

    public void Toggle()
    {
        opened = !opened;
    }
}

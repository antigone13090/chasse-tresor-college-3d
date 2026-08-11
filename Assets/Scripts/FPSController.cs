using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -20f;

    private CharacterController controller;
    private Camera playerCamera;

    private float verticalVelocity = 0f;
    private float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float x = 0f;
        float z = 0f;

        // Clavier AZERTY : ZQSD
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
            z += 1f;

        if (Input.GetKey(KeyCode.S))
            z -= 1f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            x -= 1f;

        if (Input.GetKey(KeyCode.D))
            x += 1f;

        Vector3 direction =
            transform.right * x +
            transform.forward * z;

        direction = Vector3.ClampMagnitude(direction, 1f);

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement =
            direction * speed;

        movement.y = verticalVelocity;

        controller.Move(
            movement * Time.deltaTime
        );

        // Souris
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity;

        transform.Rotate(
            Vector3.up * mouseX
        );

        cameraPitch -= mouseY;
        cameraPitch =
            Mathf.Clamp(cameraPitch, -80f, 80f);

        playerCamera.transform.localRotation =
            Quaternion.Euler(
                cameraPitch,
                0f,
                0f
            );

        // Echap libère la souris
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible = true;
        }

        // Clic gauche reprend la souris
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState =
                CursorLockMode.Locked;

            Cursor.visible = false;
        }
    }
}

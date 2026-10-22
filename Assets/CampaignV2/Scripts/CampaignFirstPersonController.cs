using UnityEngine;

namespace CollegeTreasure.Campaign
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CampaignFirstPersonController : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float mouseSensitivity = 2f;
        public float keyboardLookSpeed = 70f;

        CharacterController characterController;
        Camera playerCamera;
        float verticalVelocity;
        float pitch;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (Time.timeScale <= 0f || characterController == null || playerCamera == null)
                return;

            float horizontal = 0f;
            float vertical = 0f;

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A)) horizontal -= 1f;
            if (Input.GetKey(KeyCode.D)) horizontal += 1f;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W)) vertical += 1f;
            if (Input.GetKey(KeyCode.S)) vertical -= 1f;

            Vector3 direction = Vector3.ClampMagnitude(
                transform.right * horizontal + transform.forward * vertical,
                1f);

            if (characterController.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;

            verticalVelocity += -20f * Time.deltaTime;
            float speed = Input.GetKey(KeyCode.LeftShift) ? walkSpeed * 1.45f : walkSpeed;
            Vector3 movement = direction * speed;
            movement.y = verticalVelocity;
            characterController.Move(movement * Time.deltaTime);

            float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
            float look = Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (Input.GetKey(KeyCode.LeftArrow)) yaw -= keyboardLookSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow)) yaw += keyboardLookSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow)) look += keyboardLookSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow)) look -= keyboardLookSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up * yaw);
            pitch = Mathf.Clamp(pitch - look, -80f, 80f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}

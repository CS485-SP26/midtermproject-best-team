using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform playerTarget;

        [Header("Settings")]
        [SerializeField] private float mouseSensitivity = 0.3f;
        [SerializeField] private float distanceFromPlayer = 5f;
        [SerializeField] private float heightOffset = 1.5f;
        [SerializeField] private float minVerticalAngle = -30f;
        [SerializeField] private float maxVerticalAngle = 70f;
        [SerializeField] private float smoothSpeed = 100f;

        private float yaw;
        private float pitch;

        private bool isMouseUnlocked = false;

        void Start()
        {
            LockCursor();

            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }

        void LateUpdate()
        {
            if (playerTarget == null)
                return;

            // Only rotate camera if mouse is locked
            if (!isMouseUnlocked)
            {
                HandleMouseLook();
            }

            UpdateCameraPosition();
        }

        void HandleMouseLook()
        {
            if (Mouse.current == null) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);
        }

        void UpdateCameraPosition()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            Vector3 targetPosition = playerTarget.position + Vector3.up * heightOffset;
            Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * distanceFromPlayer;

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );

            transform.rotation = rotation;
        }

        // 🔥 This is called automatically by PlayerInput (Send Messages)
        public void OnMouse()
        {
            isMouseUnlocked = !isMouseUnlocked;

            if (isMouseUnlocked)
                UnlockCursor();
            else
                LockCursor();
        }

        void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
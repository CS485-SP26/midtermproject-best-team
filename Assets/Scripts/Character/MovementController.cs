using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] protected float acceleration = 20f;
        [SerializeField] protected float maxVelocity = 5f;

        [Header("Camera Reference")]
        [SerializeField] protected Transform cameraTransform;

        protected Rigidbody rb;
        protected Vector2 moveInput;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 lateralInput)
        {
            moveInput = lateralInput;
        }

        public void Stop()
        {
            rb.linearVelocity = Vector3.zero;
            moveInput = Vector2.zero;
        }

        public virtual void Jump() { }

        public virtual float GetHorizontalSpeedPercent()
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            return Mathf.Clamp01(horizontalVelocity.magnitude / maxVelocity);
        }

        protected virtual void FixedUpdate()
        {
            // Overridden in PhysicsMovement
        }
    }
}
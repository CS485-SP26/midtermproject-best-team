using UnityEngine;

namespace Character
{
    public class PhysicsMovement : MovementController
    {
        [Header("Physics Settings")]
        [SerializeField] private float jumpForce = 7f;
        [SerializeField] private float drag = 4f;
        [SerializeField] private float rotationSpeed = 15f;

        private bool isGrounded;
        private bool jumpRequested;

        protected override void Start()
        {
            base.Start();
            rb.linearDamping = drag;
            rb.freezeRotation = true;
        }

        public override void Jump()
        {
            if (isGrounded)
                jumpRequested = true;
        }

        protected override void FixedUpdate()
        {
            ApplyMovement();
            ClampVelocity();
            ApplyRotation();
            ApplyJump();
        }

        void ApplyMovement()
        {
            if (cameraTransform == null) return;

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 movement = camForward * moveInput.y + camRight * moveInput.x;

            if (movement.sqrMagnitude < 0.01f)
                return;

            movement.Normalize();

            rb.AddForce(movement * acceleration, ForceMode.Acceleration);
        }

        void ApplyRotation()
        {
            if (cameraTransform == null) return;

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            Vector3 direction = camForward * moveInput.y + camRight * moveInput.x;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }

        void ApplyJump()
        {
            if (!jumpRequested) return;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpRequested = false;
            isGrounded = false;
        }

        void ClampVelocity()
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (horizontalVelocity.magnitude > maxVelocity)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxVelocity;
                rb.linearVelocity = new Vector3(
                    horizontalVelocity.x,
                    rb.linearVelocity.y,
                    horizontalVelocity.z
                );
            }
        }

        void OnCollisionStay(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    isGrounded = true;
                    return;
                }
            }
        }

        void OnCollisionExit(Collision collision)
        {
            isGrounded = false;
        }
    }
}
using UnityEngine;

// TODO: Consider the benefits of refactoring to namespace Movement
namespace Character
{
    public class PhysicsMovement : MovementController
    {
        [SerializeField] float jumpForce = 7f;
        [SerializeField] float drag = 0.5f;
        [SerializeField] float rotationSpeed = 0.1f;

        bool isGrounded;
        bool jumpRequested;

        protected override void Start()
        {
            base.Start();
            rb.linearDamping = drag;
            rb.freezeRotation = true;
        }

        public override float GetHorizontalSpeedPercent()
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            return Mathf.Clamp01(horizontalVelocity.magnitude / maxVelocity);;
        }

        public override void Jump() 
        {
            // TODO: integrate jump support from week 2-3    
            if (isGrounded)
                jumpRequested = true;
        }

        protected override void FixedUpdate()
        {
            //base.FixedUpdate(); // TODO: remove base.FixedUpdate() when starting your integration
            ApplyMovement();
            ClampVelocity();
            ApplyRotation();
            ApplyJump();
        }
        
        void ApplyMovement()
        {
            // TODO integrate your physics from week 2-3 

            Vector3 movement = Vector3.zero;
            movement += transform.right * moveInput.x;
            movement += transform.forward * moveInput.y;

            if (movement.sqrMagnitude < 0.01f)
                return;

            movement.Normalize();
            rb.AddForce(movement * acceleration, ForceMode.Acceleration);

        }

        void ApplyJump()
        {
            // TODO integrate your jump logic from week 2-3 

            if (!jumpRequested) return;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpRequested = false;
            isGrounded = false;
        }

        // TODO integrate collision support from week 2-3 

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

        void ClampVelocity()
        {
            // Clamp horizontal velocity while preserving vertical (for jumping/falling)
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            
            if (horizontalVelocity.magnitude > maxVelocity)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxVelocity;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
            }
        }

        void ApplyRotation()
        {
            Vector3 direction = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (direction.magnitude > 0.5f)
            {
                // 1. Calculate the target rotation (where we WANT to look)
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

                // 2. Smoothly rotate from our current rotation toward the target
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    rotationSpeed * Time.fixedDeltaTime
                );
            }
        }
    }
}

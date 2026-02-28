using UnityEngine;
using UnityEngine.InputSystem;
using Farming;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))] // Input is required and we don't store a reference
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TileSelector tileSelector;

        MovementController moveController;
        AnimatedController animatedController;
        private IInteractable currentInteractable;

        void Start()
        {
            moveController = GetComponent<MovementController>();
            animatedController = GetComponent<AnimatedController>();

            // TODO: Consider Debug.Assert vs RequireComponent(typeof(...))
            Debug.Assert(animatedController, "PlayerController requires an animatedController");
            Debug.Assert(moveController, "PlayerController requires a MovementController");
            Debug.Assert(tileSelector, "PlayerController requires a TileSelector.");
        }

        public void OnMove(InputValue inputValue)
        {
            Vector2 inputVector = inputValue.Get<Vector2>();
            moveController.Move(inputVector);
        }

        public void OnJump(InputValue inputValue)
        {
            moveController.Jump();
        }

        public void OnInteract(InputValue value)
        {
            // 1️⃣ Priority: check general interactable trigger (store, NPC, etc.)
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                return;
            }

            // 2️⃣ Fallback: check farm tile selection
            FarmTile tile = tileSelector.GetSelectedTile();
            if (tile == null) return;

            // Delegate tile interaction to Farmer
            Farmer farmer = GetComponent<Farmer>();
            if (farmer != null)
            {
                farmer.TryTileInteraction(tile);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
            }
        }

        // Trigger exit: clear interactable when leaving
        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable == currentInteractable)
            {
                currentInteractable = null;
            }
        }

    }
}

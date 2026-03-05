using UnityEngine;
using UnityEngine.InputSystem;
using Farming;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TileSelector tileSelector;

        private MovementController moveController;
        private AnimatedController animatedController;
        private IInteractable currentInteractable;

        void Start()
        {
            moveController = GetComponent<MovementController>();
            animatedController = GetComponent<AnimatedController>();

            Debug.Assert(animatedController, "PlayerController requires an AnimatedController");
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
            if (inputValue.isPressed)
                moveController.Jump();
        }

        public void OnInteract(InputValue value)
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
                return;
            }

            FarmTile tile = tileSelector.GetSelectedTile();
            if (tile == null) return;

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
                currentInteractable = interactable;
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable == currentInteractable)
                currentInteractable = null;
        }
    }
}
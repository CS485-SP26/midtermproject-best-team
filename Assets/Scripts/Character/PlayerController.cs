using UnityEngine;
using UnityEngine.InputSystem;
using Farming;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))] // Input is required and we don't store a reference
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TileSelector tileSelector;
        [SerializeField] private ProgressBar wateringBar; // UI bar showing remaining water
        [SerializeField] private float maxWaterAmount = 400f; // total water capacity
        [SerializeField] private float waterPerTile = 20f;   // amount used per watering

        MovementController moveController;
        AnimatedController animatedController;

        private float currentWaterAmount; // starts full
        private IInteractable currentInteractable;

        void Start()
        {
            moveController = GetComponent<MovementController>();
            animatedController = GetComponent<AnimatedController>();

            // TODO: Consider Debug.Assert vs RequireComponent(typeof(...))
            Debug.Assert(animatedController, "PlayerController requires an animatedController");
            Debug.Assert(moveController, "PlayerController requires a MovementController");
            Debug.Assert(tileSelector, "PlayerController requires a TileSelector.");

            // Start full
            currentWaterAmount = maxWaterAmount;
            UpdateWaterBar();
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

            switch (tile.GetCondition)
            {
                case FarmTile.Condition.Grass:
                    tile.Interact(); // Grass → Tilled
                    animatedController.SetTrigger("Till");
                    break;

                case FarmTile.Condition.Tilled:
                    if (currentWaterAmount >= waterPerTile)
                    {
                        tile.Interact(); // Tilled → Watered
                        animatedController.SetTrigger("Water");
                        currentWaterAmount -= waterPerTile;
                        UpdateWaterBar();
                    }
                    else
                    {
                        Debug.Log("Not enough water!");
                    }
                    break;

                case FarmTile.Condition.Watered:
                    tile.Interact();
                    break;
            }
        }

        private void UpdateWaterBar()
        {
            wateringBar.SetProgress(currentWaterAmount / maxWaterAmount);
        }

        public void RefillWater(float amount)
        {
            currentWaterAmount = Mathf.Min(currentWaterAmount + amount, maxWaterAmount);
            UpdateWaterBar();
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

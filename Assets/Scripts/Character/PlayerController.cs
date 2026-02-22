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

        [SerializeField] private ProgressBar wateringBar; // UI bar showing remaining water
        [SerializeField] private float maxWaterAmount = 400f; // total water capacity
        [SerializeField] private float waterPerTile = 20f;   // amount used per watering

        private float currentWaterAmount; // starts full

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

        public void OnEnter(InputValue value)
        {
            // Check if near any store
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f); // radius = how close you must be
            foreach (var hit in hits)
            {
                SceneEntrance store = hit.GetComponent<SceneEntrance>();
                if (store != null)
                {
                    store.EnterScene();  // Calls the store's scene change
                    return;           // Skip farm interaction
                }
            }

            // If no store nearby, do nothing (or optionally show a debug)
            Debug.Log("No store nearby.");
        }

        public void OnInteract(InputValue value)
        {
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
                    Debug.Log("Tile already watered!");
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


    }
}

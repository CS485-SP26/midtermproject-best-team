using Character;
using UnityEngine;
using Core;

namespace Farming
{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private GameObject waterCan;
        [SerializeField] private GameObject gardenHoe;

        [SerializeField] private ProgressBar waterLevelUI;


        [SerializeField] private int maxWater = 10;

        private AnimatedController animatedController;

        void Start()
        {
            Debug.Assert(waterCan, "Missing watering can reference.");
            Debug.Assert(gardenHoe, "Missing hoe reference.");
            Debug.Assert(waterLevelUI, "Missing water UI reference.");

            animatedController = GetComponent<AnimatedController>();

            SetTool("None");

            waterLevelUI.SetText("Water Level");

            // Subscribe to GameManager water updates
            GameManager.Instance.OnWaterChanged += UpdateWaterUI;

            // Initialize UI with current water
            UpdateWaterUI(GameManager.Instance.Water);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnWaterChanged -= UpdateWaterUI;
        }

        private void UpdateWaterUI(int currentWater)
        {
            waterLevelUI.Fill = (float)currentWater / maxWater;
        }

        public void SetTool(string tool)
        {
            waterCan.SetActive(false);
            gardenHoe.SetActive(false);

            switch (tool)
            {
                case "WateringCan":
                    waterCan.SetActive(true);
                    break;
                case "GardenHoe":
                    gardenHoe.SetActive(true);
                    break;
            }
        }

        public void TryTileInteraction(FarmTile tile)
        {
            if (tile == null) return;

            switch (tile.GetCondition)
            {
                case FarmTile.Condition.Grass:
                    // Turn Grass → Tilled
                    animatedController.SetTrigger("Till");
                    tile.Interact();
                    break;

                case FarmTile.Condition.Tilled:
                    // Water the tilled soil
                    if (GameManager.Instance.Water >= 1)
                    {
                        animatedController.SetTrigger("Water");
                        tile.Interact();
                        GameManager.Instance.AddWater(-1);
                    }
                    else
                    {
                        Debug.Log("Not enough water to water the tilled soil");
                    }
                    break;

                case FarmTile.Condition.Watered:
                    // Plant seeds if available
                    if (GameManager.Instance.Seeds > 0)
                    {
                        animatedController.SetTrigger("Plant");
                        tile.Interact();
                        GameManager.Instance.AddSeeds(-1);
                    }
                    else
                    {
                        Debug.Log("No seeds available to plant");
                    }
                    break;

                case FarmTile.Condition.Planted:
                    Debug.Log("Plant is growing. Wait 2 days to harvest.");
                    break;

                case FarmTile.Condition.Grown:
                    // Optionally harvest
                    Debug.Log("Plant fully grown.");
                    break;
            }
        }
    }
}

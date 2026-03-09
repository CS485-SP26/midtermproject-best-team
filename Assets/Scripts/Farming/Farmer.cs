using Character;
using UnityEngine;
using Core;
using System.Collections;

namespace Farming
{
    [RequireComponent(typeof(AnimatedController))]
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private GameObject waterCan;
        [SerializeField] private GameObject gardenHoe;
        [SerializeField] private ProgressBar waterLevelUI;
        [SerializeField] private int maxWater = 10;

        // Reference to CelebrationManager to trigger celebrations
        [SerializeField] private CelebrationManager celebrationManager;

        private AnimatedController animatedController;

        void Start()
        {
            Debug.Assert(waterCan, "Missing watering can reference.");
            Debug.Assert(gardenHoe, "Missing hoe reference.");
            animatedController = GetComponent<AnimatedController>();
            SetTool("None");

            if (waterLevelUI != null)
                waterLevelUI.SetText("Water Level");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnWaterChanged += UpdateWaterUI;
                UpdateWaterUI(GameManager.Instance.Water);

                // Check if a celebration was purchased in the store
                // and trigger it when returning to the farm
                if (GameManager.Instance.GetCelebrationPending())
                {
                    GameManager.Instance.SetCelebrationPending(false);
                    if (celebrationManager != null)
                        StartCoroutine(DelayedCelebration());
                }
            }
            else
            {
                Debug.LogError("GameManager.Instance is null in Farmer.Start()");
            }
        }

        // Waits 1.5 seconds before triggering celebration
        // so the scene is fully loaded first
        private IEnumerator DelayedCelebration()
        {
            yield return new WaitForSeconds(1.5f);
            if (celebrationManager != null)
                celebrationManager.TriggerStoreCelebration();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnWaterChanged -= UpdateWaterUI;
        }

        private void UpdateWaterUI(int currentWater)
        {
            if (waterLevelUI != null)
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
                    animatedController.SetTrigger("Till");
                    tile.Interact();
                    break;

                case FarmTile.Condition.Tilled:
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
                    tile.Interact();
                    GameManager.Instance.AddPlants(1);
                    Debug.Log("Harvested! Plants: " + GameManager.Instance.Plants);
                    break;
            }
        }
    }
}
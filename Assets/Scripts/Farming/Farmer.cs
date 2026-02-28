using Character;
using UnityEngine;


namespace Farming
{

    [RequireComponent(typeof(AnimatedController))] // AnimatedController is required and we don't store a reference
    public class Farmer : MonoBehaviour
    {

        [SerializeField] private GameObject waterCan; // Reference to the watering can GameObject

        [SerializeField] private GameObject gardenHoe; // Reference to the hoe GameObject

        [SerializeField] private ProgressBar waterLevelUI;
 
        [SerializeField] private float waterLevel = 1.000f;

        [SerializeField] private float waterPerUse  = 0.100f;

        AnimatedController animatedController;

        void Start()
        {

            Debug.Assert(waterCan, "Farmer requires a reference to the watering can GameObject.");
            Debug.Assert(gardenHoe, "Farmer requires a reference to the hoe GameObject.");
            Debug.Assert(waterLevelUI, "Farmer requires a reference to the water level UI.");
            
            SetTool("None"); // start with no tool active
            animatedController = GetComponent<AnimatedController>();

            waterLevelUI.SetText("Water Level");
            waterLevelUI.Fill = waterLevel;

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
            if (tile == null) return; // No tile selected, do nothing
        
            switch (tile.GetCondition)
            {
                case FarmTile.Condition.Grass: 
                    animatedController.SetTrigger("Till"); 
                    tile.Interact(); // updates the condition, play the anim after
                    break;
                case FarmTile.Condition.Tilled: 
                    if (waterLevel >= waterPerUse)
                    {
                    animatedController.SetTrigger("Water"); 
                    tile.Interact(); // updates the condition, play the anim after
                    waterLevel -= waterPerUse;
                    waterLevelUI.Fill = waterLevel;

                    Debug.Log($"Water used. New water level: {waterLevel}");
                    }
                    break;
                
                default: 
                break;
            }
        }
    
    }
}
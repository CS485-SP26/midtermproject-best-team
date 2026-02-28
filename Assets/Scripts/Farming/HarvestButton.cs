using UnityEngine;
using Farming;
using Core;

public class HarvestButton : MonoBehaviour
{
    // The UI button to show when near a grown tile
    [SerializeField] private GameObject harvestButtonUI;

    // How close the player needs to be to harvest
    [SerializeField] private float harvestRange = 2f;

    private FarmTile nearestGrownTile;

    void Start()
    {
        // Hide harvest button at start
        harvestButtonUI.SetActive(false);
    }

    void Update()
    {
        // Check every frame for a nearby grown tile
        nearestGrownTile = FindNearestGrownTile();

        // Show or hide the button based on whether a grown tile is nearby
        harvestButtonUI.SetActive(nearestGrownTile != null);
    }

    // Called by the harvest UI button's OnClick event
    public void OnHarvestClicked()
    {
        if (nearestGrownTile == null) return;
        nearestGrownTile.Harvest();
        Debug.Log("Harvested! Plants: " + GameManager.Instance.HarvestedPlants);
    }

    // Finds the nearest Grown tile within harvest range
    private FarmTile FindNearestGrownTile()
    {
        FarmTile[] allTiles = FindObjectsByType<FarmTile>(FindObjectsSortMode.None);
        FarmTile nearest = null;
        float closestDistance = harvestRange;

        foreach (FarmTile tile in allTiles)
        {
            if (tile.GetCondition != FarmTile.Condition.Grown) continue;

            float distance = Vector3.Distance(
                transform.position,
                tile.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = tile;
            }
        }

        return nearest;
    }
}
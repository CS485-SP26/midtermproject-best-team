using UnityEngine;
using TMPro;
using Core;

public class StoreButtons : MonoBehaviour
{
    // Reference to CelebrationManager to trigger celebration
    [SerializeField] private CelebrationManager celebrationManager;

    // Celebration item button to show/hide based on funds
    [SerializeField] private GameObject celebrationItemButton;

    [SerializeField] private ProgressBar waterLevelUI;
    [SerializeField] private int maxWater = 10;

    // Called by Buy Seeds button OnClick
    public void OnBuySeedClicked()
    {
        GameManager.Instance.BuySeed(10, 5);
        Debug.Log("Seeds: " + GameManager.Instance.Seeds);
        Debug.Log("Funds: " + GameManager.Instance.Funds);
    }

    // Called by Sell Plants button OnClick
    // Sells all harvested plants at $20 each
    public void OnSellPlantsClicked()
    {
        int plantsToSell = GameManager.Instance.Plants;
        GameManager.Instance.AddPlants(-plantsToSell);
        GameManager.Instance.AddFunds(20 * plantsToSell);
        Debug.Log("Plants: " + GameManager.Instance.Plants);
        Debug.Log("Funds: " + GameManager.Instance.Funds);
        UpdateUI();
    }

    // Called by Exit button OnClick
    public void OnExitClicked()
    {
        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }

    // Called by Refill Water button OnClick
    // Caps water at max of 10 and deducts $30
    public void OnRefillWaterClicked()
    {
        //int refillAmount = 10;
        if (GameManager.Instance.Funds >= 15 && GameManager.Instance.Water<10)
        {
            GameManager.Instance.AddFunds(-15);
            GameManager.Instance.AddWater(1);
           // int newWater = Mathf.Min(GameManager.Instance.Water + refillAmount, 10);
          // GameManager.Instance.AddWater(newWater - GameManager.Instance.Water);
            Debug.Log("Water refilled! Current water: " + GameManager.Instance.Water);
        }
        else if (GameManager.Instance.Water >= 10)
        {
            Debug.Log("Water already full");
        }
        else
        {
            Debug.Log("Not enough funds for water!");
        }
    }

    // Called by Celebration Item button OnClick
    // Costs $200 and triggers a celebration when player returns to farm
    public void OnBuyCelebrationItemClicked()
    {
        if (GameManager.Instance.Funds >= 200)
        {
            GameManager.Instance.AddFunds(-200);
            GameManager.Instance.SetCelebrationPending(true);
            Debug.Log("Celebration item purchased!");
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough funds for celebration item!");
        }
    }

    // Updates button visibility based on current funds
    private void UpdateUI()
    {
        // Only show celebration button if player has $200 or more
        if (celebrationItemButton != null)
            celebrationItemButton.SetActive(GameManager.Instance.Funds >= 200);
    }

    void Start()
    {
        UpdateUI();

        if (GameManager.Instance !=null){
        GameManager.Instance.OnWaterChanged+= UpdateWaterUI;

        UpdateWaterUI(GameManager.Instance.Water);
        }
    }

    void OnDestroy()
    {
        if(GameManager.Instance !=null)
        GameManager.Instance.OnWaterChanged -= UpdateWaterUI;

    }

    private void UpdateWaterUI(int currentWater)
    {
        if (waterLevelUI !=null)
        waterLevelUI.Fill = (float)currentWater/maxWater;
    }
    
}
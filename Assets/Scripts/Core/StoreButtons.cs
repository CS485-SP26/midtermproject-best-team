using UnityEngine;
using TMPro;
using Core;

public class StoreButtons : MonoBehaviour
{
    // UI text elements that display current counts
    [SerializeField] private TextMeshProUGUI seedsText;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI harvestedPlantsText;

    // Sell Plants button so we can show/hide it based on plant count
    [SerializeField] private GameObject sellPlantsButton;

    void Start()
    {
        UpdateUI();
    }

    // Called by Buy Seeds button OnClick
    public void OnBuySeedClicked()
    {
        GameManager.Instance.BuySeed(10, 5);
        Debug.Log("Seeds: " + GameManager.Instance.Seeds);
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
        int refillAmount = 10;
        if (GameManager.Instance.Funds >= 30)
        {
            GameManager.Instance.AddFunds(-30);
            int newWater = Mathf.Min(GameManager.Instance.Water + refillAmount, 10);
            GameManager.Instance.AddWater(newWater - GameManager.Instance.Water);
            Debug.Log("Water refilled! Current water: " + GameManager.Instance.Water);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough funds for water!");
        }
    }

    // Called by Sell Plants button OnClick
    public void OnSellPlantsClicked()
    {
        GameManager.Instance.SellPlants();
        Debug.Log("Sold plants! Funds: " + GameManager.Instance.Funds);
        UpdateUI();
    }

    // Updates all UI text and button visibility
    private void UpdateUI()
    {
        if (seedsText != null)
            seedsText.text = "Seeds: " + GameManager.Instance.Seeds;

        if (fundsText != null)
            fundsText.text = "Funds: $" + GameManager.Instance.Funds;

        if (harvestedPlantsText != null)
            harvestedPlantsText.text = "Plants: " + GameManager.Instance.HarvestedPlants;

        // Only show sell button if player has plants to sell
        if (sellPlantsButton != null)
            sellPlantsButton.SetActive(GameManager.Instance.HarvestedPlants > 0);
    }
}
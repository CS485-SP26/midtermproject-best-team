using UnityEngine;
using TMPro;
using Core;

public class StoreButtons : MonoBehaviour
{
    public void OnBuySeedClicked()
    {
        GameManager.Instance.BuySeed(10, 5); // Buy 10 seeds for $5
        Debug.Log("Seeds: " + GameManager.Instance.Seeds);
        Debug.Log("Funds: " + GameManager.Instance.Funds);
    }

    public void OnSellPlantsClicked()
    {
        int plantsToSell = GameManager.Instance.Plants;

        GameManager.Instance.AddPlants(-plantsToSell); // Buy 10 seeds for $5
        GameManager.Instance.AddFunds(20 * plantsToSell);
        Debug.Log("Plants: " + GameManager.Instance.Plants);
        Debug.Log("Funds: " + GameManager.Instance.Funds);
    }

    public void OnExitClicked()
    {
        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }

    public void OnRefillWaterClicked()
    {
        int refillAmount = 10; // How much water to give per refill
        if (GameManager.Instance.Funds >= 30)
        {
            GameManager.Instance.AddFunds(-30); // Deduct money

            // Refill water but do not exceed MaxWater
            int newWater = Mathf.Min(GameManager.Instance.Water + refillAmount, 10);
            GameManager.Instance.AddWater(newWater - GameManager.Instance.Water); // Add only the difference

            Debug.Log("Water refilled! Current water: " + GameManager.Instance.Water);
        }
        else
        {
            Debug.Log("Not enough funds for water!");
        }
    }
}

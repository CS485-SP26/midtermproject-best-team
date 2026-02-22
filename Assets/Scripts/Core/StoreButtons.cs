using UnityEngine;
using TMPro;
using Core;

public class StoreButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI seedsText;
    [SerializeField] private TextMeshProUGUI fundsText;

    void Start()
    {
        UpdateUI();
    }

    public void OnBuySeedClicked()
    {
        GameManager.Instance.BuySeedFromStore();
        Debug.Log("Seeds: " + GameManager.Instance.Seeds);
        Debug.Log("Funds: " + GameManager.Instance.Funds);
        UpdateUI();
    }

    public void OnExitClicked()
    {
        GameManager.Instance.LoadScenebyName("Scene1-FarmingSim");
    }

    public void OnRefillWaterClicked()
    {
        if (GameManager.Instance.Funds >= 30)
        {
            GameManager.Instance.AddFunds(-30);
            Debug.Log("Water refilled!");
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough funds for water!");
        }
    }

    private void UpdateUI()
    {
        if (seedsText != null)
            seedsText.text = "Seeds: " + GameManager.Instance.Seeds;
        if (fundsText != null)
            fundsText.text = "Funds: $" + GameManager.Instance.Funds;
    }
}
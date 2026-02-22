using UnityEngine;
using UnityEngine.UI;
using Core;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button purchaseSeedsButton;
    [SerializeField] private Text seedsText;
    [SerializeField] private Text fundsText;
    [SerializeField] private int seedCost = 10;
    [SerializeField] private int seedsPerPurchase = 5;

    void Start()
    {
        purchaseSeedsButton.onClick.AddListener(OnPurchaseSeeds);
        UpdateUI();
        GameManager.Instance.OnFundsChanged += OnFundsChanged;
        GameManager.Instance.OnSeedsChanged += OnSeedsChanged;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnFundsChanged -= OnFundsChanged;
        GameManager.Instance.OnSeedsChanged -= OnSeedsChanged;
    }

    private void OnPurchaseSeeds()
    {
        GameManager.Instance.BuySeed(seedCost, seedsPerPurchase);
        UpdateUI();
    }

    private void OnFundsChanged(int newFunds) => UpdateUI();
    private void OnSeedsChanged(int newSeeds) => UpdateUI();

    private void UpdateUI()
    {
        int currentFunds = GameManager.Instance.Funds;
        seedsText.text = "Seeds: " + GameManager.Instance.Seeds;
        fundsText.text = "Funds: $" + currentFunds;
        purchaseSeedsButton.gameObject.SetActive(currentFunds >= seedCost);
    }
}
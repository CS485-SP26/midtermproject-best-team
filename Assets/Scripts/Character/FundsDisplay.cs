using UnityEngine;
using TMPro;
using Core;

public class FundsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fundsText;

    void OnEnable()
    {
        GameManager.Instance.OnFundsChanged += UpdateFundsUI;
        UpdateFundsUI(GameManager.Instance.Funds);
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnFundsChanged -= UpdateFundsUI;
    }

    private void UpdateFundsUI(int currentFunds)
    {
        if (fundsText != null)
            fundsText.text = "Funds: $" + currentFunds;
    }
}

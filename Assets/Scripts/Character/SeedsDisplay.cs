using UnityEngine;
using TMPro;
using Core;
using System;

public class SeedDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI seedsText;

    void OnEnable()
    {
        GameManager.Instance.OnSeedsChanged += UpdateSeedsUI;
        UpdateSeedsUI(GameManager.Instance.Seeds);

    }

    void OnDisable()
    {
        if(GameManager.Instance !=null)
        GameManager.Instance.OnSeedsChanged -= UpdateSeedsUI;
    }

    private void UpdateSeedsUI(int currentSeeds)
    {
        if (seedsText != null)
            seedsText.text = "Seeds: " + currentSeeds;
    }

}

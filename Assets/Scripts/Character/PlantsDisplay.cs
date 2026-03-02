using UnityEngine;
using TMPro;
using Core;
using System;

public class PlantDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI plantsText;

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
        GameManager.Instance.OnPlantsChanged += UpdateSeedsUI;
        UpdateSeedsUI(GameManager.Instance.Plants);
   
        }
        
    }

    void OnDisable()
    {
        if(GameManager.Instance !=null)
        GameManager.Instance.OnPlantsChanged -= UpdateSeedsUI;
    }

    private void UpdateSeedsUI(int currentPlants)
    {
        if (plantsText != null)
            plantsText.text = "Plants: " + currentPlants;
    }

}

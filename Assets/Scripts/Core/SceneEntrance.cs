using UnityEngine;
using Core;
using Farming;

public class SceneEntrance : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject enterPrompt;
    [SerializeField] private string sceneName;

    public void Interact()
    {
        WinCondition wc = FindFirstObjectByType<WinCondition>();
        if (wc != null) wc.SaveTiles();

        GameManager.Instance.LoadScenebyName(sceneName);
        enterPrompt.SetActive(false); // optional: hide prompt immediately
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(true); // show prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(false); // hide prompt
        }
    }
}

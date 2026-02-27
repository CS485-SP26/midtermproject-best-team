using UnityEngine;
using Core;
using Farming;

public class SceneEntrance : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject enterPrompt;
    [SerializeField] private string sceneName;

    public void Interact()
    {
        // Find the tile manager
        FarmTileManager manager = FindFirstObjectByType<FarmTileManager>();
        manager?.SaveTileStates();

        // Load the new scene
        GameManager.Instance.LoadScenebyName(sceneName);

        // Hide prompt
        enterPrompt.SetActive(false);
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

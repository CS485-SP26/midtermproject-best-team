using UnityEngine;
using Core;
using Farming;
public class SceneEntrance : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject enterPrompt;
    [SerializeField] private string sceneName;

    public void Interact()
    {
        FarmTileManager manager = FindFirstObjectByType<FarmTileManager>();
        manager?.SaveTileStates();
        GameManager.Instance.LoadScenebyName(sceneName);
        enterPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(false);
        }
    }
}
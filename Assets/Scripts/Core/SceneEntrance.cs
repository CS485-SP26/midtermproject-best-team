using UnityEngine;
using Core;
using Farming;

public class SceneEntrance : MonoBehaviour
{
    public GameObject enterPrompt;
    [SerializeField] public string sceneName;
    private bool playerInRange = false;

    public void EnterScene()
    {
        if (playerInRange)
        {
            WinCondition wc = FindFirstObjectByType<WinCondition>();
            if (wc != null) wc.SaveTiles();
            GameManager.Instance.LoadScenebyName(sceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterPrompt.SetActive(false);
            playerInRange = false;
        }
    }
}
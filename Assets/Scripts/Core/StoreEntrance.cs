using UnityEngine;
using Core;

public class StoreEntrance : MonoBehaviour
{
    public GameObject enterPrompt;
    public string storeSceneName = "Scene2-Store";
    private bool playerInRange = false;

    public void EnterStore()
    {
        if (playerInRange)
        {
            GameManager.Instance.LoadScenebyName(storeSceneName);
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

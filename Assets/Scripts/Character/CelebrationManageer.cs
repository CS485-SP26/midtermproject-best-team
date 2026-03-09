using UnityEngine;
using Core;
using Character;
using System.Collections;

public class CelebrationManager : MonoBehaviour
{
    [Header("Animation")]
    // Reference to the player's animated controller to trigger emote
    [SerializeField] private AnimatedController animatedController;

    [Header("Particle Effect")]
    // Particle system that plays when a milestone is reached
    [SerializeField] private ParticleSystem celebrationParticles;

    [Header("UI")]
    // UI element that shows the reward message
    [SerializeField] private GameObject rewardUI;
    [SerializeField] private TMPro.TextMeshProUGUI rewardText;

    // Tracks the last milestone that was celebrated
    private int lastCelebratedMilestone = 0;
    private const int MILESTONE_AMOUNT = 200;

    void Start()
    {
        // Hide reward UI at start
        if (rewardUI != null)
            rewardUI.SetActive(false);

        // Subscribe to funds changed event
        if (GameManager.Instance != null)
            GameManager.Instance.OnFundsChanged += OnFundsChanged;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnFundsChanged -= OnFundsChanged;
    }

    // Called automatically whenever funds change
    private void OnFundsChanged(int newFunds)
    {
        int currentMilestone = (newFunds / MILESTONE_AMOUNT) * MILESTONE_AMOUNT;
        if (currentMilestone > lastCelebratedMilestone && currentMilestone > 0)
        {
            lastCelebratedMilestone = currentMilestone;
            Celebrate("$" + currentMilestone + " milestone reached!");
        }
    }

    // Called when player buys the celebration item from store
    public void TriggerStoreCelebration()
    {
        Celebrate("Celebration item purchased!");
    }

    // Main celebration method
    private void Celebrate(string message)
    {
        Debug.Log("Celebrating: " + message);

        // Find RewardPanel dynamically in case scene was reloaded
        if (rewardUI == null)
            rewardUI = GameObject.Find("RewardPanel");
        if (rewardText == null)
            rewardText = GameObject.Find("RewardText")?.GetComponent<TMPro.TextMeshProUGUI>();

        // Trigger celebration emote on player
        if (animatedController != null)
        {
            try { animatedController.SetTrigger("Celebrate"); }
            catch { Debug.Log("Celebrate trigger not set up yet."); }
        }

        // Play particle effect and audio
        if (celebrationParticles != null)
        {
            AudioSource fireworksAudio = celebrationParticles.GetComponent<AudioSource>();
            celebrationParticles.Play();
            if (fireworksAudio != null)
                fireworksAudio.Play();
            Invoke(nameof(StopFireworks), 6f);
        }

        // Show reward UI message
        if (rewardUI != null && rewardText != null)
        {
            rewardText.text = message;
            rewardUI.SetActive(true);
            Invoke(nameof(HideRewardUI), 6f);
        }
        else
        {
            Debug.LogWarning("RewardUI or RewardText not found!");
        }

        // Lock Y position during dance to prevent sinking
        StartCoroutine(LockYPosition(6f));
    }

    // Locks the player's Y position during the dance
    // so the animation doesn't sink them into the ground
    private IEnumerator LockYPosition(float duration)
    {
        float lockedY = transform.position.y;
        float elapsed = 0f;
        Rigidbody rb = GetComponent<Rigidbody>();

        while (elapsed < duration)
        {
            Vector3 pos = transform.position;
            pos.y = lockedY;
            transform.position = pos;
            if (rb != null)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Hides the reward UI after delay
    private void HideRewardUI()
    {
        if (rewardUI != null)
            rewardUI.SetActive(false);
    }

    // Stops the fireworks after delay
    private void StopFireworks()
    {
        if (celebrationParticles != null)
        {
            celebrationParticles.Stop();
            AudioSource fireworksAudio = celebrationParticles.GetComponent<AudioSource>();
            if (fireworksAudio != null)
                fireworksAudio.Stop();
        }
    }
}
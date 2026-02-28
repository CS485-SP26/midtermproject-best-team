using UnityEngine;
using UnityEngine.SceneManagement;
using Farming;
using Core;
using System.Linq;

namespace Farming
{
    public class WinCondition : MonoBehaviour
    {
        [SerializeField] private FarmTileManager tileManager;
        [SerializeField] private GameObject congratsUI;
        [SerializeField] private int rewardAmount = 50;

        private bool rewardGiven = false;

        void Start()
        {
            if (!tileManager)
            {
                Debug.LogError("WinCondition requires a reference to FarmTileManager.");
                return;
            }

            congratsUI.SetActive(false);

            // Check if reward was already given today
            if (GameManager.Instance != null)
                rewardGiven = !GameManager.Instance.CanReceiveReward();

            // Restore saved tile states when returning from store
            RestoreTileStates();
        }

        void Update()
        {
            if (!rewardGiven && AllTilesWatered())
            {
                GiveReward();
                rewardGiven = true;
            }
        }

        // Checks if all tiles are in an active growing state
        private bool AllTilesWatered()
        {
            var tiles = tileManager.GetTiles();
            return tiles.Length > 0 && tiles.All(tile =>
                tile.GetCondition == FarmTile.Condition.Watered ||
                tile.GetCondition == FarmTile.Condition.Planted ||
                tile.GetCondition == FarmTile.Condition.Grown
            );
        }

        // Gives the player a reward and marks it as paid for today
        private void GiveReward()
        {
            if (GameManager.Instance == null) return;
            congratsUI.SetActive(true);
            GameManager.Instance.AddFunds(rewardAmount);
            GameManager.Instance.MarkRewardPaid();
            Debug.Log("All tiles watered! Reward granted: " + rewardAmount);
        }

        // Saves all tile states before leaving the scene
        public void SaveTiles()
        {
            var tiles = tileManager.GetTiles();
            var states = tiles.Select(t => t.GetCondition).ToArray();
            GameManager.Instance.SaveTileStates(states);
            Debug.Log("Saved " + states.Length + " tile states.");
        }

        // Restores tile states when returning from the store
        private void RestoreTileStates()
        {
            if (GameManager.Instance == null) return;

            var saved = GameManager.Instance.GetSavedTileStates();
            var tiles = tileManager.GetTiles();

            if (saved == null || saved.Length != tiles.Length) return;

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].SetState(saved[i]);
            }

            Debug.Log("Restored " + tiles.Length + " tile states.");
        }
    }
}
using UnityEngine;
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

            // Check if the reward was already given from previous sessions
            if (GameManager.Instance != null)
            {
                rewardGiven = !GameManager.Instance.CanReceiveReward();
            }
        }

        void Update()
        {
            if (!rewardGiven && AllTilesWatered())
            {
                GiveReward();
                rewardGiven = true;
            }
        }

        private bool AllTilesWatered()
        {
            var tiles = tileManager.GetTiles();
            return tiles.Length > 0 && tiles.All(tile =>
                tile.GetCondition == FarmTile.Condition.Watered ||
                tile.GetCondition == FarmTile.Condition.Planted ||
                tile.GetCondition == FarmTile.Condition.Grown
            );
        }

        private void GiveReward()
        {
            if (GameManager.Instance == null) return;

            congratsUI.SetActive(true);
            GameManager.Instance.AddFunds(rewardAmount);
            GameManager.Instance.MarkRewardPaid();

            Debug.Log("All tiles watered! Reward granted: " + rewardAmount);
        }
    }
}

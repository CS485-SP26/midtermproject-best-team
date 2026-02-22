using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Farming;
using Core;

namespace Farming
{
    public class WinCondition : MonoBehaviour
    {
        [SerializeField] private FarmTileManager tileManager;
        [SerializeField] private GameObject congratsUI;
        [SerializeField] private int rewardAmount = 50;

        private bool rewardGiven = false;
        private FarmTile[] tiles;

        void Start()
        {
            tiles = GetComponentsInChildren<FarmTile>()
                .OrderBy(t => t.gameObject.name)
                .ToArray();
            Debug.Log("Found " + tiles.Length + " tiles");
            congratsUI.SetActive(false);

            var saved = GameManager.Instance.GetSavedTileStates();
            if (saved != null && saved.Length == tiles.Length)
            {
                int wateredCount = 0;
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (saved[i] == FarmTile.Condition.Watered)
                    {
                        tiles[i].Water();
                        wateredCount++;
                    }
                    else if (saved[i] == FarmTile.Condition.Tilled)
                        tiles[i].Till();
                }
                Debug.Log("Restored " + wateredCount + " watered tiles");
            }
            else
            {
                Debug.Log("No saved states found, saved is " + (saved == null ? "null" : saved.Length.ToString()));
            }
        }

        public void SaveTiles()
        {
            if (tiles != null)
            {
                Debug.Log("Explicitly saving " + tiles.Length + " tiles");
                GameManager.Instance.SaveTileStates(tiles);
            }
        }

        void Update()
        {
            if (!rewardGiven && AllTilesWatered())
            {
                congratsUI.SetActive(true);
                GameManager.Instance.AddFunds(rewardAmount);
                rewardGiven = true;
            }
        }

        private bool AllTilesWatered()
        {
            foreach (FarmTile tile in tiles)
            {
                if (tile.GetCondition != FarmTile.Condition.Watered)
                    return false;
            }
            return tiles.Length > 0;
        }
    }
}
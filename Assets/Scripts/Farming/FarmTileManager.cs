using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Environment;
using Core;

namespace Farming
{
    public class FarmTileManager : MonoBehaviour
    {
        [SerializeField] private GameObject farmTilePrefab;
        [SerializeField] private DayController dayController;
        [SerializeField] private int rows = 4;
        [SerializeField] private int cols = 4;
        [SerializeField] private float tileGap = 0.1f;

        private List<FarmTile> tiles = new List<FarmTile>();

        void Start()
        {
            Debug.Assert(farmTilePrefab, "FarmTileManager requires a farmTilePrefab");
            Debug.Assert(dayController, "FarmTileManager requires a dayController");

            ValidateGrid();       // Ensure all tiles exist
            tiles = tiles.OrderBy(t => t.gameObject.name).ToList();
            LoadTileStates();     // Restore saved states
        }

        void OnEnable()
        {
            dayController.dayPassedEvent.AddListener(OnDayPassed);
        }

        void OnDisable()
        {
            dayController.dayPassedEvent.RemoveListener(OnDayPassed);
        }

        public void OnDayPassed()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCurrentDay(dayController.CurrentDay);
            }
            IncrementDays(1);

            // Save tiles automatically at the end of the day
            SaveTileStates();
        }

        public void IncrementDays(int count)
        {
            while (count > 0)
            {
                foreach (FarmTile farmTile in tiles)
                    farmTile.OnDayPassed();
                count--;
            }
        }

        public FarmTile[] GetTiles() => tiles.ToArray();

        // ---------------- SAVE/LOAD ----------------

        public void SaveTileStates()
        {
            if (tiles == null || tiles.Count == 0) return;

            // Collect tile conditions
            FarmTile.Condition[] states = new FarmTile.Condition[tiles.Count];
            for (int i = 0; i < tiles.Count; i++)
            {
                states[i] = tiles[i].GetCondition;
            }

            // Save to GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveTileStates(states);
            }
            Debug.Log("FarmTileManager: Saved " + states.Length + " tile states.");
        }

        public void LoadTileStates()
        {

            if (GameManager.Instance == null)
            {
            Debug.LogWarning("FarmTileManager: GameManager.Instance is null in LoadTileStates");
            return;
            }

            var saved = GameManager.Instance.GetSavedTileStates();
            if (saved == null || saved.Length != tiles.Count) return;

            // Sort tiles by name to match saved array
            tiles.Sort((a, b) => string.Compare(a.gameObject.name, b.gameObject.name));

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].SetState(saved[i]);
            }

            Debug.Log("Loaded " + tiles.Count + " tile states successfully.");
        }

        // ---------------- Grid Management ----------------

        void InstantiateTiles()
        {
            Vector3 spawnPos = transform.position;
            int count = 0;
            GameObject clone = null;
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    clone = Instantiate(farmTilePrefab, spawnPos, Quaternion.identity);
                    clone.name = "Farm Tile " + count++;
                    spawnPos.x += clone.transform.localScale.x + tileGap;
                    clone.transform.parent = transform;
                    tiles.Add(clone.GetComponent<FarmTile>());
                }
                spawnPos.z += clone.transform.localScale.z + tileGap;
                spawnPos.x = transform.position.x;
            }
        }

        void OnValidate()
        {
            #if UNITY_EDITOR
            EditorApplication.delayCall += () => { if (this != null) ValidateGrid(); };
            #endif
        }

        void ValidateGrid()
        {
            if (!farmTilePrefab) return;
            tiles.Clear();
            foreach (Transform child in transform)
                if (child.gameObject.TryGetComponent<FarmTile>(out var tile))
                    tiles.Add(tile);

            int newCount = rows * cols;
            if (tiles.Count != newCount)
            {
                DestroyTiles();
                InstantiateTiles();
            }
        }

        void DestroyTiles()
        {
            foreach (FarmTile tile in tiles)
            {
                #if UNITY_EDITOR
                DestroyImmediate(tile.gameObject);
                #else
                Destroy(tile.gameObject);
                #endif
            }
            tiles.Clear();
        }
    }
}

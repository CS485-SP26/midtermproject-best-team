using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Farming;
using Environment;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────
        // SINGLETON PATTERN
        // Ensures only one GameManager exists at a time.
        // Uses Awake for earlier initialization.
        // ─────────────────────────────────────────────
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (water == 0)
                Water = 10;
        }

        // ─────────────────────────────────────────────
        // DAY TRACKING
        // Tracks the current day and when the last
        // reward was given so it's not given twice.
        // ─────────────────────────────────────────────
        public int CurrentDay { get; private set; } = 1;
        public int LastRewardDay { get; private set; } = -1;
        public bool HasRecievedReward { get; private set; } = false;

        // Updates the current day number
        public void SetCurrentDay(int day)
        {
            CurrentDay = day;
        }

        // Returns true if the reward hasn't been given today
        public bool CanReceiveReward()
        {
            return LastRewardDay != CurrentDay;
        }

        // Marks the reward as paid for today
        public void MarkRewardPaid()
        {
            HasRecievedReward = true;
        }

        // ─────────────────────────────────────────────
        // FUNDS
        // Tracks the player's current money.
        // OnFundsChanged fires whenever funds change
        // so UI elements can update automatically.
        // ─────────────────────────────────────────────
        private int funds;
        public int Funds {
            get { return funds; }
            private set {
                funds = value;
                OnFundsChanged?.Invoke(funds);
            }
        }
        public event Action<int> OnFundsChanged;

        // Adds or subtracts funds (use negative value to spend)
        public void AddFunds(int amount)
        {
            Funds += amount;
            Debug.Log("Funds: " + Funds);
        }

        // ─────────────────────────────────────────────
        // SEEDS
        // Tracks how many seeds the player has.
        // Seeds are spent when planting and gained
        // by purchasing from the store.
        // ─────────────────────────────────────────────
        private int seeds;
        public int Seeds {
            get { return seeds; }
            private set {
                seeds = value;
                OnSeedsChanged?.Invoke(seeds);
            }
        }
        public event Action<int> OnSeedsChanged;

        // Adds or subtracts seeds (use negative value to spend)
        public void AddSeeds(int amount)
        {
            Seeds += amount;
            Debug.Log("Seeds: " + Seeds);
        }

        // Attempts to buy seeds from the store.
        // Deducts cost from funds and adds seeds if
        // the player has enough money.
        public void BuySeed(int cost, int amount)
        {
            if (Funds >= cost)
            {
                Funds -= cost;
                Seeds += amount;
                Debug.Log("Bought " + amount + " seed(s) for $" + cost);
            }
            else
            {
                Debug.Log("Not enough funds to buy seed.");
            }
        }

        // ─────────────────────────────────────────────
        // WATER
        // Tracks the player's current water level.
        // Water is consumed when watering tiles and
        // restored by purchasing from the store.
        // ─────────────────────────────────────────────
        private int water;
        public int Water {
            get { return water; }
            private set {
                water = value;
                OnWaterChanged?.Invoke(water);
            }
        }
        public event Action<int> OnWaterChanged;

        // Adds or subtracts water (use negative value to consume)
        public void AddWater(int amount)
        {
            Water += amount;
            Debug.Log("Water: " + Water);
        }

        // ─────────────────────────────────────────────
        // PLANTS
        // Tracks how many plants the player has harvested.
        // Harvested plants can be sold in the store
        // to earn funds.
        // ─────────────────────────────────────────────
        private int plants;
        public int Plants {
            get { return plants; }
            private set {
                plants = value;
                OnPlantsChanged?.Invoke(plants);
            }
        }
        public event Action<int> OnPlantsChanged;

        // Adds or subtracts plants (use negative value to sell)
        public void AddPlants(int amount)
        {
            Plants += amount;
            Debug.Log("Plants: " + Plants);
        }

        // ─────────────────────────────────────────────
        // CELEBRATION
        // Tracks if a celebration item was purchased
        // in the store so Scene1 can trigger it when
        // the player returns.
        // ─────────────────────────────────────────────
        private bool celebrationPending = false;

        // Sets whether a celebration is pending
        public void SetCelebrationPending(bool value)
        {
            celebrationPending = value;
        }

        // Returns whether a celebration is pending
        public bool GetCelebrationPending()
        {
            return celebrationPending;
        }

        // ─────────────────────────────────────────────
        // TILE STATE PERSISTENCE
        // Saves and restores farm tile conditions
        // when switching between scenes so progress
        // is not lost when visiting the store.
        // ─────────────────────────────────────────────
        private FarmTile.Condition[] savedTileStates;

        // Saves the current condition of all farm tiles
        public void SaveTileStates(FarmTile.Condition[] states)
        {
            savedTileStates = new FarmTile.Condition[states.Length];
            Array.Copy(states, savedTileStates, states.Length);
            Debug.Log("GameManager: Saved " + states.Length + " tile states.");
        }

        // Returns the saved tile states so they can be restored
        public FarmTile.Condition[] GetSavedTileStates()
        {
            return savedTileStates;
        }

        // ─────────────────────────────────────────────
        // SCENE MANAGEMENT
        // Loads a scene by name using Unity's
        // SceneManager system.
        // ─────────────────────────────────────────────
        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }

        private Image fillImage;
    }
}
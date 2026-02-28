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
        // Uses Awake instead of Start for earlier initialization.
        // ─────────────────────────────────────────────
        [SerializeField] private DayController dayController;
        public static GameManager Instance { get; private set; }

        public bool HasRecievedReward {get; private set;} = false;
       
       //Awake Singleton
       private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Give player starting water if none exists
            if (water == 0)
            {
                Water = 10;
            }
        }

        // ─────────────────────────────────────────────
        // DAY TRACKING
        // Tracks the current day and when the last
        // reward was given so it's not given twice
        // on the same day.
        // ─────────────────────────────────────────────
        public int CurrentDay { get; private set; } = 1;
        public int LastRewardDay { get; private set; } = -1;

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
            LastRewardDay = CurrentDay;
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

        // Convenience method for the store Buy Seeds button.
        // Buys 5 seeds for $10.
        public void BuySeedFromStore()
        {
            BuySeed(10, 5);
        }

        // ─────────────────────────────────────────────
        // WATER
        // Tracks the player's current water level.
        // Water is consumed when watering tiles and
        // restored by refilling at the equipment shed
        // or purchasing from the store.
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
        // HARVESTED PLANTS
        // Tracks how many plants the player has harvested.
        // Harvested plants can be sold in the store
        // to earn funds.
        // ─────────────────────────────────────────────
        private int harvestedPlants;
        public int HarvestedPlants {
            get { return harvestedPlants; }
            private set {
                harvestedPlants = value;
                OnHarvestedPlantsChanged?.Invoke(harvestedPlants);
            }
        }
        public event Action<int> OnHarvestedPlantsChanged;

        // Adds harvested plants to the player's inventory
        public void AddHarvestedPlants(int amount)
        {
            HarvestedPlants += amount;
            Debug.Log("Harvested Plants: " + HarvestedPlants);
        }

        // Sells all harvested plants at $20 each.
        // Adds earnings to funds and resets harvested
        // plant count back to zero.
        public void SellPlants()
        {
            if (HarvestedPlants > 0)
            {
                int earnings = HarvestedPlants * 20;
                AddFunds(earnings);
                HarvestedPlants = 0;
                Debug.Log("Sold plants for $" + earnings);
            }
            else
            {
                Debug.Log("No plants to sell!");
            }
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
    
        //tracks if player can receive reward at current day if it was not already given

        public bool CanReceiveReward()
        {
            return LastRewardDay != CurrentDay;

        }
        //was the reward paid today
        public void MarkRewardPaid()
        {
            HasRecievedReward = true;
        }
    
    }
}


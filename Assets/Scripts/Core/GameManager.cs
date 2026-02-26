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
        [SerializeField] private DayController dayController;
        public static GameManager Instance { get; private set;}
        //Keeps track of the current day 
        public int CurrentDay {get; private set;}=1;
        //Will help track when reward was last given 
        public int LastRewardDay {get; private set;}=-1;
       
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
        }

        private int funds;
        public int Funds {
            get { return funds; }
            private set
            {
                funds = value;
                OnFundsChanged?.Invoke(funds);
            }
        }
        public void AddFunds(int amount)
        {
            Funds += amount;
            Debug.Log("Funds: " + Funds);
        }
        public event Action<int> OnFundsChanged;

        private int seeds;
        public int Seeds {
            get { return seeds; }
            private set {
                seeds = value;
                OnSeedsChanged?.Invoke(seeds);
            }
        }
        public void AddSeeds(int amount)
        {
            Seeds += amount;
            Debug.Log("Seeds: " + Seeds);
        }
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
        public event Action<int> OnSeedsChanged;

        private int water;
        public int Water {
            get { return water; }
            private set
            {
                water = value;
                OnWaterChanged?.Invoke(water);
            }
        }
        public void AddWater(int amount)
        {
            Water += amount;
            Debug.Log("Water: " + Water);
        }
        public event Action<int> OnWaterChanged;
        
        private int plants;
        public int Plants {
            get { return plants; }
            private set
            {
                plants = value;
                OnPlantsChanged?.Invoke(plants);
            }
        }
        public void AddPlants(int amount)
        {
            Funds += amount;
            Debug.Log("Plants: " + Plants);
        }
        public event Action<int> OnPlantsChanged;

        private FarmTile.Condition[] savedTileStates;

        public void SaveTileStates(FarmTile[] tiles)
        {
            savedTileStates = new FarmTile.Condition[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                savedTileStates[i] = tiles[i].GetCondition;
            }
        }

        public FarmTile.Condition[] GetSavedTileStates()
        {
            return savedTileStates;
        }

        private Image fillImage;

        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }
    
        //tracking days so that reward is given at separate times-after all tiles are in watered state
        public void SetCurrentDay(int day)
        {
            CurrentDay=day;

        }
        //tracks if player can receive reward at current day if it was not already given

        public bool CanReceiveReward()
        {
            return LastRewardDay != CurrentDay;

        }
        //was the reward paid today
        public void MarkRewardPaid()
        {
            LastRewardDay=CurrentDay;
        }
    
    }
}

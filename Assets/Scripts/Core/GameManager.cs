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
        private static GameManager instance;
        private int funds;
       
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
       /* public static GameManager Instance {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }*/

      
        public int Funds {
            get { return funds; }
            private set
            {
                funds = value;
                OnFundsChanged?.Invoke(funds);
            }
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
        public event Action<int> OnSeedsChanged;

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

        public void AddSeeds(int amount)
        {
            Seeds += amount;
            Debug.Log("Seeds: " + Seeds);
        }

        private Image fillImage;

       /* void Start()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }*/

        public void AddFunds(int amount)
        {
            Funds += amount;
            Debug.Log("Funds: " + Funds);
        }

        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
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

        public void BuySeedFromStore()
        {
            BuySeed(10, 5);
        }

        public void AddWater(int amount)
        {
            //added water logic
            if(Funds >= amount)
            {
                Funds-= amount;//Assuming water costs money
            }
            else
            {
                Debug.Log("Not enough funds to buy water");
            }
            
            Debug.Log("Added " + amount + " units of water.");
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
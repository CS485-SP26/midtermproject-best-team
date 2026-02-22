using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Farming;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance {
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

        void Start()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

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
            Debug.Log("Added " + amount + " units of water.");
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

namespace Core
{

    public class GameManager:MonoBehaviour
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
            } /* no set read only */
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

        private Image fillImage;

        void Start()
        {
            // if (instance != null && instance != this)
            // {
            //     Destroy(this.gameObject);
            //     return;
            // }

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

        public void BuySeed(int amount)
        {
            if (Funds >= amount)
            {
                Funds -= amount;
                Debug.Log("Bought seed for $" + amount);
            }
            else
            {
                Debug.Log("Not enough funds to buy seed.");
            }
        }

        public void AddWater(int amount)
        {
            // Implement water logic here

            
            if (Funds >= amount)
            {
            Funds -= amount; // Assuming water costs money
            }
            else
            {
                Debug.Log("Not enough funds to buy water.");
            }

            Debug.Log("Added " + amount + " units of water.");
        }
    }
}

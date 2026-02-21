using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

        void Start()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
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
    }
}

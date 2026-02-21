using UnityEngine;
using UnityEngine.SceneManagement;

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

        int funds;

        public int Funds { get; set; }

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

        public void LoadScenebyName(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}

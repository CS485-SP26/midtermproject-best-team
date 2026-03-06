using Environment;
using Unity.VisualScripting;
using UnityEngine;

namespace Environment
{

    public class SeasonManager : MonoBehaviour
    {

        public enum Season
        {
            Winter,
            Spring,
            Summer,
            Fall
        }
        [SerializeField] private SeasonData[] seasons = new SeasonData[4];


        private SeasonData scratchData;

        public SeasonData RuntimeData
        {
            get
            {
                return scratchData;
            }
            set
            {
                //create a safe clone of data
                scratchData = Instantiate(value);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SetSeason(Season.Spring);
        }

        public void SetSeason(Season value)
        {

            RuntimeData = seasons[(int)value];
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

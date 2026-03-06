using UnityEngine;


namespace Environment{
[CreateAssetMenu(fileName = "SeasonData", menuName = "Scriptable Objects/SeasonData")]
    public class SeasonData : ScriptableObject
    {

        [Range(-20f, 140f)]
        [Tooltip("Temperature in Fahrenheit")]
        
        public   float AvgTemp;
        [Range(0f, 24f)]
        [Tooltip("Day length in hours")]
        public   float DayLength;
        
        public   Color SunColor;
    }
}
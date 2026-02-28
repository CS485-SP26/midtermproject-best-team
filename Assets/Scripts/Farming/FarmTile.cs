using System.Collections.Generic;
using UnityEngine;
using Environment;
using Core;
using System.Xml.Serialization;
using UnityEngine.Tilemaps;

namespace Farming
{
    public class FarmTile : MonoBehaviour
    {
        public enum Condition { Grass, Tilled, Watered, Planted, Grown, Withered }
        [SerializeField] private Condition tileCondition = Condition.Grass;
        [SerializeField] private FarmTileManager manager;

        [Header("Visuals")]
        [SerializeField] private Material grassMaterial;
        [SerializeField] private Material tilledMaterial;
        [SerializeField] private Material wateredMaterial;

        [SerializeField] private Material witheredMaterial;

        [Header("Plant")]
        [SerializeField] private GameObject plantPrefab;
        [SerializeField] private Vector3 plantScale = new Vector3(0.5f, 12.5f, 0.5f);
        [SerializeField] private Vector3 grownScale = new Vector3(1.0f, 25.0f, 1.0f);
        [SerializeField] private GameObject WitheredPlantPrefab;

        MeshRenderer tileRenderer;

        [Header("Audio")]
        [SerializeField] private AudioSource stepAudio;
        [SerializeField] private AudioSource tillAudio;
        [SerializeField] private AudioSource waterAudio;

        //has two days before plant withers
        [Header("Withering")]
        [SerializeField] private int daysUntilWithered = 2;
       //tracks how many days its been since plant has been watered, timer to reset
        private int daysSinceWatered = 0;
        private int daysSinceLastInteraction = 0;

        List<Material> materials = new List<Material>();

        public FarmTile.Condition GetCondition { get { return tileCondition; } }

        void Start()
        {
           
            tileRenderer = GetComponent<MeshRenderer>();
            Debug.Assert(tileRenderer, "FarmTile requires a MeshRenderer");

            foreach (Transform edge in transform)
            {
                materials.Add(edge.gameObject.GetComponent<MeshRenderer>().material);
            }

        if (WitheredPlantPrefab !=null) WitheredPlantPrefab.SetActive(false);
         if (plantPrefab !=null) plantPrefab.SetActive(false);
    
       UpdateVisual();
        }

        public void Interact()
        {    

            Condition before = tileCondition;

            switch (tileCondition)
            {
                case Condition.Grass:
                    Till();
                    break;

                case Condition.Tilled:
                Debug.Log("Tile interacted in Tilled state. GM is " +
                (GameManager.Instance == null ? "NULL" : "NOT NULL"));
                    Water();
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddFunds(10);
                        Debug.Log("Funds added from FarmTile");
                    }
                    break;

                case Condition.Watered:
                    Plant();
                    break;

                case Condition.Planted:
                    Debug.Log("Plant already planted.");
                    break;

                case Condition.Grown:
                    Debug.Log("Plant fully grown!");
                    WaterPlant();
                    break;
                case Condition.Withered:
                    Till();
                    break;
            }
        }

        public void SetState(Condition state)
        {
            tileCondition = state;
            switch (state)
            {
                case Condition.Grass:
                    if (plantPrefab != null) plantPrefab.SetActive(false);
                    if(WitheredPlantPrefab != null) WitheredPlantPrefab.SetActive(false);
                UpdateVisual();
                break;

                case Condition.Tilled:
                case Condition.Watered:
                    if (plantPrefab != null) plantPrefab.SetActive(false);
                    
                if(WitheredPlantPrefab != null) WitheredPlantPrefab.SetActive(false);
                UpdateVisual();
                break;

                case Condition.Planted:
                    if (plantPrefab != null)
                    {
                        plantPrefab.SetActive(true);
                        plantPrefab.transform.localScale = plantScale;
                    }

                    if(WitheredPlantPrefab != null) WitheredPlantPrefab.SetActive(false);
                    UpdateVisual();
                    break;

                case Condition.Grown:
                    if (plantPrefab != null)
                    {
                        plantPrefab.SetActive(true);
                        plantPrefab.transform.localScale = grownScale;
                    }

                    daysSinceWatered=0;
                    if(WitheredPlantPrefab != null) WitheredPlantPrefab.SetActive(false);

                    UpdateVisual();
                    break;

                case Condition.Withered:
                if (plantPrefab != null) plantPrefab.SetActive(false);

                if(WitheredPlantPrefab != null)
                    {
                        WitheredPlantPrefab.SetActive(true);
                    }
                    UpdateVisual();
                    break;
            }
        }

        public void Till()
        {
            tileCondition = FarmTile.Condition.Tilled;

            if (plantPrefab != null) plantPrefab.SetActive(false);
            if (WitheredPlantPrefab != null) WitheredPlantPrefab.SetActive(false);

            if (tileRenderer == null)
                tileRenderer = GetComponent<MeshRenderer>();

            UpdateVisual();
            tillAudio?.Play();
        }

        public void Water()
        {
            tileCondition = FarmTile.Condition.Watered;
            daysSinceLastInteraction = 0;
         
            if (tileRenderer == null)
             tileRenderer = GetComponent<MeshRenderer>();
             UpdateVisual();
            waterAudio?.Play();
        }

        public void Plant()
        {
            if (plantPrefab == null)
                return;

            Debug.Log("Planted");
            tileCondition = Condition.Planted;
            plantPrefab.SetActive(true);
            plantPrefab.transform.localScale = plantScale;

            daysSinceLastInteraction = 0;
        }

        private void Grow()
        {
            if (plantPrefab == null)
                return;

            Debug.Log("Plant grown");
            tileCondition = Condition.Grown;
            plantPrefab.transform.localScale = grownScale;

            daysSinceWatered=0;
            daysSinceLastInteraction = 0;
        }

       private void WaterPlant()
        {
            daysSinceWatered = 0;
            daysSinceLastInteraction = 0;
            Debug.Log("Grown plant watered");
        }
        private void WitherNow()
        {

            if (witheredMaterial == null)
            Debug.Log("plant has died");
            tileCondition = Condition.Withered;

            if(plantPrefab != null)
             plantPrefab.SetActive(false);

            if(WitheredPlantPrefab !=null)
             WitheredPlantPrefab.SetActive(true);

             UpdateVisual();
            
        }
        
        private void UpdateVisual()
        {
            if (tileRenderer == null) return;

            switch (tileCondition)
            {
                case Condition.Grass:
                    tileRenderer.material = grassMaterial;
                    break;

                case Condition.Tilled:
                    tileRenderer.material = tilledMaterial;
                    break;

                case Condition.Watered:
                    tileRenderer.material = wateredMaterial;
                    break;

                case Condition.Planted:
                    tileRenderer.material = wateredMaterial;
                    break;

                case Condition.Grown:
                    tileRenderer.material = wateredMaterial;
                    break;

                case Condition.Withered:
                tileRenderer.material = witheredMaterial;
                break;
            }
        }

        public void SetHighlight(bool active)
        {
            foreach (Material m in materials)
            {
                if (active)
                {
                    m.EnableKeyword("_EMISSION");
                }
                else
                {
                    m.DisableKeyword("_EMISSION");
                }
            }
            if (active) stepAudio.Play();
        }
       public void OnDayPassed()
        {
            daysSinceLastInteraction++;

            if (tileCondition == Condition.Grown)
            {
              daysSinceWatered++;

                if (daysSinceWatered >= daysUntilWithered)
                {
                    WitherNow();
                    return;
                }
            }

            if (tileCondition == Condition.Planted && daysSinceLastInteraction >= 1)
            {
                Grow();
            }

            else if (daysSinceLastInteraction >= 2)
            {
                if (tileCondition == Condition.Watered)
                    tileCondition = Condition.Tilled;
                else if (tileCondition == Condition.Tilled)
                    tileCondition = Condition.Grass;
            }

            UpdateVisual();
        }

    
    }
}


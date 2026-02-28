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
        private int daysGrown = 0;

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

<<<<<<< HEAD
            if (plantPrefab != null)
                plantPrefab.SetActive(false);
=======
        if (WitheredPlantPrefab !=null) WitheredPlantPrefab.SetActive(false);
         if (plantPrefab !=null) plantPrefab.SetActive(false);
    
       UpdateVisual();
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
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
                    Debug.Log("Plant is growing. Wait for it to grow.");
                    break;

                case Condition.Grown:
<<<<<<< HEAD
                    Debug.Log("Plant fully grown! Press Harvest to harvest.");
                    break;

                case Condition.Withered:
                    // Tilling a withered plant clears it back to soil
                    Till();
                    Debug.Log("Tilled withered plant back to soil.");
=======
                    Debug.Log("Plant fully grown!");
                    WaterPlant();
                    break;
                case Condition.Withered:
                    Till();
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
                    break;
            }
        }

        // Called by HarvestButton when player harvests a grown plant
        public void Harvest()
        {
            if (tileCondition != Condition.Grown) return;
            Debug.Log("Harvested plant!");
            GameManager.Instance.AddHarvestedPlants(1);
            if (plantPrefab != null)
                plantPrefab.SetActive(false);
            // Reset tile back to tilled after harvesting
            tileCondition = Condition.Tilled;
            daysGrown = 0;
            UpdateVisual();
        }

        public void SetState(Condition state)
        {
            tileCondition = state;
            switch (state)
            {
                case Condition.Grass:
<<<<<<< HEAD
                case Condition.Tilled:
                case Condition.Watered:
                    if (plantPrefab != null) plantPrefab.SetActive(false);
                    UpdateVisual();
                    break;
=======
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
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798

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

                case Condition.Withered:
                    if (plantPrefab != null)
                        plantPrefab.SetActive(false);
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
<<<<<<< HEAD
            if (plantPrefab != null)
                plantPrefab.SetActive(false);
=======

>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
            UpdateVisual();
            tillAudio?.Play();
        }

        public void Water()
        {
            tileCondition = FarmTile.Condition.Watered;
            daysSinceLastInteraction = 0;
         
            if (tileRenderer == null)
<<<<<<< HEAD
                tileRenderer = GetComponent<MeshRenderer>();
            daysSinceLastInteraction = 0;
            UpdateVisual();
=======
             tileRenderer = GetComponent<MeshRenderer>();
             UpdateVisual();
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
            waterAudio?.Play();
        }

        public void Plant()
        {
            if (plantPrefab == null) return;
            Debug.Log("Planted");
            tileCondition = Condition.Planted;
            plantPrefab.SetActive(true);
            plantPrefab.transform.localScale = plantScale;
<<<<<<< HEAD
=======

>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
            daysSinceLastInteraction = 0;
        }

        private void Grow()
        {
            if (plantPrefab == null) return;
            Debug.Log("Plant grown");
            tileCondition = Condition.Grown;
            plantPrefab.transform.localScale = grownScale;
<<<<<<< HEAD
            daysGrown = 0;
        }

        private void Wither()
        {
            Debug.Log("Plant withered!");
            tileCondition = Condition.Withered;
            if (plantPrefab != null)
                plantPrefab.SetActive(false);
            UpdateVisual();
=======

            daysSinceWatered=0;
            daysSinceLastInteraction = 0;
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
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
<<<<<<< HEAD
                case Condition.Withered:
                    // Use withered material if assigned, otherwise fall back to grass
                    tileRenderer.material = witheredMaterial != null ? witheredMaterial : grassMaterial;
                    break;
=======

                case Condition.Withered:
                tileRenderer.material = witheredMaterial;
                break;
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
            }
        }

        public void SetHighlight(bool active)
        {
            foreach (Material m in materials)
            {
                if (active)
                    m.EnableKeyword("_EMISSION");
                else
                    m.DisableKeyword("_EMISSION");
            }
            if (active) stepAudio.Play();
        }
       public void OnDayPassed()
        {
            daysSinceLastInteraction++;

<<<<<<< HEAD
            switch (tileCondition)
=======
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
>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798
            {
                case Condition.Planted:
                    // Wither if not watered within 2 days
                    if (daysSinceLastInteraction >= 2)
                        Wither();
                    else
                        Grow();
                    break;

                case Condition.Grown:
                    // Wither if not harvested within 2 days of growing
                    daysGrown++;
                    if (daysGrown >= 2)
                        Wither();
                    break;

                case Condition.Watered:
                    tileCondition = Condition.Tilled;
                    UpdateVisual();
                    break;

                case Condition.Tilled:
                    tileCondition = Condition.Grass;
                    UpdateVisual();
                    break;
            }
        }

    
    }
<<<<<<< HEAD
}
=======
}

>>>>>>> 273fdbef3fabf026015ec4a076e2e90be3ce0798

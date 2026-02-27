using System.Collections.Generic;
using UnityEngine;
using Environment;
using Core;

namespace Farming
{
    public class FarmTile : MonoBehaviour
    {
        public enum Condition { Grass, Tilled, Watered, Planted, Grown }
        [SerializeField] private Condition tileCondition = Condition.Grass;
        [SerializeField] private FarmTileManager manager;

        [Header("Visuals")]
        [SerializeField] private Material grassMaterial;
        [SerializeField] private Material tilledMaterial;
        [SerializeField] private Material wateredMaterial;

        [Header("Plant")]
        [SerializeField] private GameObject plantPrefab;
        [SerializeField] private Vector3 plantScale = new Vector3(0.5f, 12.5f, 0.5f);
        [SerializeField] private Vector3 grownScale = new Vector3(1.0f, 25.0f, 1.0f);

        MeshRenderer tileRenderer;

        [Header("Audio")]
        [SerializeField] private AudioSource stepAudio;
        [SerializeField] private AudioSource tillAudio;
        [SerializeField] private AudioSource waterAudio;

        List<Material> materials = new List<Material>();
        private int daysSinceLastInteraction = 0;

        public FarmTile.Condition GetCondition { get { return tileCondition; } }

        void Start()
        {
            tileRenderer = GetComponent<MeshRenderer>();
            Debug.Assert(tileRenderer, "FarmTile requires a MeshRenderer");

            foreach (Transform edge in transform)
            {
                materials.Add(edge.gameObject.GetComponent<MeshRenderer>().material);
            }

            // Hide Plant
            if (plantPrefab != null)
            {
                plantPrefab.SetActive(false);
            }
        }

        public void Interact()
        {
            switch (tileCondition)
            {
                case Condition.Grass:
                    Till();
                    break;

                case Condition.Tilled:
                    Water();
                    GameManager.Instance.AddFunds(10);
                    break;

                case Condition.Watered:
                    Plant();
                    break;

                case Condition.Planted:
                    Debug.Log("Plant already planted.");
                    break;

                case Condition.Grown:
                    Debug.Log("Plant fully grown!");
                    break;
            }

            daysSinceLastInteraction = 0;
        }

        public void SetState(Condition state)
        {
            tileCondition = state;

            switch (state)
            {
                case Condition.Grass:
                    if (plantPrefab != null) plantPrefab.SetActive(false);
                    UpdateVisual();
                break;

                case Condition.Tilled:
                case Condition.Watered:
                    if (plantPrefab != null) plantPrefab.SetActive(false);
                    UpdateVisual();
                break;

                case Condition.Planted:
                    if (plantPrefab != null)
                    {
                        plantPrefab.SetActive(true);
                        plantPrefab.transform.localScale = plantScale;
                    }
                    UpdateVisual();
                    break;

                case Condition.Grown:
                    if (plantPrefab != null)
                    {
                        plantPrefab.SetActive(true);
                        plantPrefab.transform.localScale = grownScale;
                    }
                    UpdateVisual();
                    break;
            }
        }

        public void Till()
        {
            tileCondition = FarmTile.Condition.Tilled;
            if (tileRenderer == null)
                tileRenderer = GetComponent<MeshRenderer>();
            UpdateVisual();
            tillAudio?.Play();
        }

        public void Water()
        {
            tileCondition = FarmTile.Condition.Watered;
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
        }

        private void Grow()
        {
            if (plantPrefab == null)
                return;

            Debug.Log("Plant grown");
            tileCondition = Condition.Grown;
            plantPrefab.transform.localScale = grownScale;
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

            if (tileCondition == Condition.Planted && daysSinceLastInteraction >= 2)
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

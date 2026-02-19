using UnityEngine;
using Farming;

namespace Character
{
    public class RaycastSelector : TileSelector
    {
        [SerializeField] private float rayHeight = 2f;
        [SerializeField] private float rayDistance = 5f;

        private void Update()
        {
            Vector3 origin = transform.position + Vector3.up * rayHeight;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance))
            {
                FarmTile tile = hit.collider.GetComponentInParent<FarmTile>();
                if (tile != activeTile)
                    SetActiveTile(tile);
            }
        }
    }
}

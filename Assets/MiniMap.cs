using Unity.Mathematics;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

public Transform player;

void LateUpdate()
    {
        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position= newposition;
    
      // transform.rotation = quaternion.Euler(90f,player.eulerAngles.y, 0f);
    
    }


}

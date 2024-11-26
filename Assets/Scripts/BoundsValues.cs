using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsValues : MonoBehaviour
{
    // Start is called before the first frame update 
    public float flyingRangeRadius;
    public float flyingSpeedUpRadius;
    [Tooltip("How close can 2 boids be next to eachother")]
    public float avoidanceDistance;

    void Values()
    {

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, flyingRangeRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, flyingSpeedUpRadius);
    }
}

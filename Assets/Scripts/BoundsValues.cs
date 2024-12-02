using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsValues : MonoBehaviour
{
    [Header("Flight Values")]
    public float flyingRangeRadius;
    public float flyingSpeedUpRadius;

    [Tooltip("How close can 2 boids be next to eachother")]
    public float avoidanceDistance;

    [Header("Optimization")]
    public int maxColiderCount;

    List<BoidLaws> boidLaws = new List<BoidLaws>();

    void Start()
    {
        SetAllBoidValues();
    }

    //To make sure there arent unneciserry calls, this is where all the boid values are stored and set
    public void SetAvoidanceDistance(float setAvoidanceDistance)
    {
        foreach (var item in boidLaws)
        {
            item.SetAvoidanceDistance(setAvoidanceDistance);
        }
        avoidanceDistance = setAvoidanceDistance;
    }
    public void SetFlyingRangeRadius(float setFlyingRangeRadius)
    {
        foreach (var item in boidLaws)
        {
            item.SetFlyingRangeRadius(setFlyingRangeRadius);
        }
    }
    public void SetFlyingSpeedUpRadius(float setFlyingSpeedUpRadius)
    {
        foreach (var item in boidLaws)
        {
            item.SetFlyingSpeedUpRadius(setFlyingSpeedUpRadius);
        }
    }
    public void SetMaxColiderCount(int maxColiderCount)
    {
        foreach (var item in boidLaws)
        {
            item.SetMaxColiderCount(maxColiderCount);
        }
    }

    public void SetAllBoidValues()
    {
        //Get all the boids and set their value
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Boid");
        foreach (var item in taggedObjects)
        {
            boidLaws.Add(item.GetComponent<BoidLaws>());
        }

        SetAvoidanceDistance(avoidanceDistance);
        SetFlyingRangeRadius(flyingRangeRadius);
        SetFlyingSpeedUpRadius(flyingSpeedUpRadius);
        SetMaxColiderCount(maxColiderCount);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, flyingRangeRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, flyingSpeedUpRadius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoid : MonoBehaviour
{
    public GameObject Boid;
    public Vector3 positionRange = new Vector3(1f, 0f, 1f);
    public int startingSpawnCount;
    
    void Start ()
    {
        for (int i = 0; i < startingSpawnCount; i++)
        {
            SpawnRandomBoid();
        }
        
    }
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            SpawnRandomBoid();
        }
    }

    public void SpawnRandomBoid()
    {
        //Random location
        Vector3 randomPosition = new Vector3(
            Random.Range(-positionRange.x, positionRange.x),
            Random.Range(-positionRange.y, positionRange.y),
            Random.Range(-positionRange.z, positionRange.z)
        );

        //Random rotation
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        
        //create a new boid
        Instantiate(Boid, randomPosition, randomRotation) ;	
    }
}
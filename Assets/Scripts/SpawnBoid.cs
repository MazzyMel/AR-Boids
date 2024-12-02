using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoid : MonoBehaviour
{
    public int startingSpawnCount;

    //public GameObject Boid;
    private Vector3 positionRange = new Vector3(0.3f,0.3f,0.3f);
    private BoundsValues boundsValues;

    void Start ()
    {
        boundsValues = GameObject.Find("Bounds").GetComponent<BoundsValues>();

        for (int i = 0; i < startingSpawnCount; i++)
        {
            SpawnRandomBoid();
        }
    }
    void Update()
    {
        //Create and delete boids using buttons
        if(OVRInput.GetUp(OVRInput.Button.Two) || Input.GetKeyUp("z"))
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnRandomBoid();
                boundsValues.SetAllBoidValues();
            }
        }

        if((OVRInput.GetUp(OVRInput.Button.One) || Input.GetKeyUp("space")) && transform.childCount>0)
        {
            if(transform.childCount >0)
            {
                for (int i = 0; i < 10; i++)
                {
                    KillBoid(i);
                    boundsValues.SetAllBoidValues();
                }
            }
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
        Instantiate(Resources.Load("LowPollyPigon"), randomPosition, randomRotation, gameObject.transform);	
    }
    public void KillBoid(int i)
    {
        Destroy(transform.GetChild(i).gameObject);
    }
}
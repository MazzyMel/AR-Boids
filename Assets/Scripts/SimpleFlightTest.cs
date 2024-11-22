using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public class SimpleFlightTest : MonoBehaviour
{
    Transform referencePoint;
    public float rotationSpeed;
    public float flyingRangeRadius;
    public float speed = 2;

    public void Start()
    {
        referencePoint = GameObject.Find("Bounds").transform;
    }

    void Update()
    {
        // Moves the object forward at two units per second.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, referencePoint.position) > flyingRangeRadius)
        {
            Vector3 direction = referencePoint.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
    }
}
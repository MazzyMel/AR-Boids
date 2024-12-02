using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Meta.XR.MRUtilityKit.SceneDecorator;

public class BoidLaws : MonoBehaviour
{

    [Header("Flying properties")]
    Transform referencePoint;
    public float rotationSpeed;
    public float baseSpeed;

    [Header("Sight properties")]
    public float eyesightRadius;

    //To make large crounds not lag too much, i limit how many boids can talk to eachother at a time
    [Header("Optimization values")]
    public int maxColiderCount;

    [HideInInspector]
    private Collider[] colliders;
    private float speed;
    private float flyingRangeRadius;
    private float flyingSpeedUpRadius;
    private float avoidanceDistance;
    private Animator animator;

    public void Start()
    {
        speed = baseSpeed;
        referencePoint = GameObject.Find("Bounds").transform;
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetFlyingRangeRadius(float value)
    {
        flyingRangeRadius = value;
    }
    public void SetFlyingSpeedUpRadius(float value)
    {
        flyingSpeedUpRadius = value;
    }
    public void SetAvoidanceDistance(float value)
    {
        avoidanceDistance = value;
    }
    public void SetMaxColiderCount(int value)
    {
        maxColiderCount = value;
    }
    
    void Update()
    {
        // Moves the boid forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        // If it flies too far then turn away
        StayInArea();

        colliders = Physics.OverlapSphere(transform.position, eyesightRadius);

        //the 3 RULES
        //to make sure the boid doesnt have to check every other boids distance, I only grab the shortest distance one to mesure and avoid

        BoidAvoidance();
        BoidAlignment();
        BoidCohesion();

        //Makes the animation faster when speeding up
        if (animator)
        {
            animator.speed = speed * 2;
        }
    }
    
    void StayInArea()
    {
        float distanceFromFocusPoint = Vector3.Distance(transform.position, referencePoint.position);

        if(distanceFromFocusPoint > flyingRangeRadius)
        {
            //Get a rotation that gets you to the bounds
            Vector3 direction = Vector3.Reflect(transform.position, (transform.position - referencePoint.position).normalized);
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            //the further away you are from bounds the faster the Boids should move
            if(distanceFromFocusPoint > flyingSpeedUpRadius)
            {
                speed = baseSpeed * (distanceFromFocusPoint - flyingSpeedUpRadius)+1;
                transform.LookAt(referencePoint);
            }
            else
            {
                speed = baseSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    
    void BoidAvoidance()
    {
        int i=0;
        foreach (var item in colliders)
        {
            if (i >= maxColiderCount)
            {
                return;
            }

            float distanceBetweenColiders = Vector3.Distance(transform.position, item.gameObject.transform.position);
            if(distanceBetweenColiders <avoidanceDistance)
            {
                //Take the neerest Boids position and makes a vectoraway from it
                Vector3 awayFromTarget = transform.position - item.gameObject.transform.position;
                awayFromTarget.Normalize();

                //Move the boid away
                transform.position +=  awayFromTarget * speed/2 * Time.deltaTime;
                
                if (awayFromTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(awayFromTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            i++;
        }
    }

    void BoidAlignment()
    {
        Vector4 cumulative = Vector4.zero;
        int i = 0;
        foreach (Collider obj in colliders)
        {
            if (i >= maxColiderCount)
            {
                return;
            }

            // Get rotation as a Quaternion and combine
            Quaternion rotation = obj.gameObject.transform.rotation;
            cumulative += new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);
            i++;
        }
        // Normalize the cumulative vector to get the averege rotation
        cumulative /= colliders.Length;
        cumulative.Normalize();

        Quaternion targetRotation = new Quaternion(cumulative.x, cumulative.y, cumulative.z, cumulative.w);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void BoidCohesion()
    {
        Vector3 cumulative = Vector3.zero;
        int i = 0;
        foreach (Collider obj in colliders)
        {
            if (i >= maxColiderCount)
            {
                return;
            }

            // Combine all locations together
            cumulative += obj.gameObject.transform.position;
            i++;
        }
        //devide by the count to get the averege location
        cumulative /= colliders.Length;
        if(
            //cumulative.x != Mathf.Infinity && cumulative.y != Mathf.Infinity && cumulative.z != Mathf.Infinity
            !float.IsNaN(cumulative.x) && !float.IsNaN(cumulative.y) && !float.IsNaN(cumulative.z)
        ){
            transform.position = Vector3.Slerp(transform.position, cumulative, rotationSpeed * Time.deltaTime);
        }
        
    }

    //Show radius size in scene:
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, eyesightRadius);
    }
}
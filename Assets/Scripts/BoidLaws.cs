using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Meta.XR.MRUtilityKit.SceneDecorator;

public class BoidLaws : MonoBehaviour
{

    [Header("Flying properties")]
    Transform referencePoint;
    public float rotationSpeed;
    public float flyingRangeRadius;
    public float speed = 2;

    [Header("Sight properties")]
    [Tooltip("How close can 2 boids be next to eachother")]
    public float avoidanceDistance;
    public float EyesightRadius;
    public float EyesightAngle;

    [HideInInspector]
    private Collider[] colliders;



    // private GameObject[]
    public void Start()
    {
        referencePoint = GameObject.Find("Bounds").transform;
    }

    void Update()
    {
        // Moves the object forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        // If it flies too far then turn away
        StayInArea();

        colliders = Physics.OverlapSphere(transform.position, EyesightRadius);

        //the 3 RULES
        //to make sure the boid doesnt have to check every other boids distance, I only grab the shortest distance one to mesure and avoid

        BoidAvoidance();
        BoidAlignment();
        BoidCohesion();
    }

    List<GameObject> GetObjectsInRadiusAndAngle()
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Collider collider in colliders)
        {
            Vector3 toObject = (collider.gameObject.transform.position - transform.position).normalized;
            Vector3 directionToTarget = (collider.gameObject.transform.position - transform.position).normalized;
            
            float angle = Vector3.Angle(directionToTarget, toObject);
            if (angle <= EyesightAngle)
            {
                result.Add(collider.gameObject);
            }
        }
        return result;
    }
    
    void StayInArea()
    {
        if(Vector3.Distance(transform.position, referencePoint.position) > flyingRangeRadius)
        {
            Vector3 direction = Vector3.Reflect(transform.position, (transform.position - referencePoint.position).normalized);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void BoidAvoidance()
    {
        foreach (var item in colliders)
        {
            if(Vector3.Distance(transform.position, item.gameObject.transform.position) <avoidanceDistance)
            {
                // Take the neerest Boids position and makes a vector.
                Vector3 awayFromTarget = transform.position - item.gameObject.transform.position;
                awayFromTarget.Normalize();
                
                //move the boid away:
                
                transform.position +=  awayFromTarget * speed/2 * Time.deltaTime;

                // Debug.DrawLine(transform.position, item.gameObject.transform.position);
                if (awayFromTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(awayFromTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }

            //For testing only, draw lines between visable boids:
            /*
            Debug.DrawLine((transform.position +item.gameObject.transform.position )/2 , item.gameObject.transform.position, Color.red);
            Debug.DrawLine(transform.position, (transform.position + item.gameObject.transform.position )/2 );
            */
        }
    }

    void BoidAlignment()
    {
        Vector4 cumulative = Vector4.zero;

        foreach (Collider obj in colliders)
        {
            // Get rotation as a Quaternion and combine
            Quaternion rotation = obj.gameObject.transform.rotation;
            cumulative += new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);
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

        foreach (Collider obj in colliders)
        {
            // Combine all locations together
            cumulative += obj.gameObject.transform.position;
        }
        cumulative /= colliders.Length;

        transform.position = Vector3.Slerp(transform.position, cumulative, rotationSpeed * Time.deltaTime);
    }

    //Show radius size in scene:
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EyesightRadius);
    }
}
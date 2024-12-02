using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHands : MonoBehaviour
{
    public GameObject bounds;
    public float minAvoidanceRadiusSize;
    public float maxAvoidanceRadiusSize;

    void Update()
    {
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.Hands) )
        {
            //Sets the avoidance value propotonal to the right hand trigger 
            bounds.GetComponent<BoundsValues>().SetAvoidanceDistance(
                minAvoidanceRadiusSize + (( maxAvoidanceRadiusSize - minAvoidanceRadiusSize) *
                OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger))
            );
            
            //Sets the bounds object to follow the right hand, when the grip is squeezed
            if(OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) >0.7f)
            {
                bounds.transform.parent = transform;
            }
            else
            {
                bounds.transform.parent = null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{

    public TextMeshProUGUI textField;
    int fps = 0;
    public GameObject boidParent;

    void Start()
    {
        textField = gameObject.GetComponent<TextMeshProUGUI>();
    }
    //Display the FPS and boid count
    void Update () 
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        textField.text = "FPS: "+ fps.ToString() + "<br>Boids: "+boidParent.transform.childCount;
    }
}

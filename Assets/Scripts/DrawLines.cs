using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SimpleFlightTest))]
public class DrawLines : Editor
{
    private void OnSceneGUI()
    {
        SimpleFlightTest bird = (SimpleFlightTest)target;
        Handles.DrawLine(bird.transform.position, bird.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

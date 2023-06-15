using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject box1;
    public GameObject box2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion targetRotation = box1.transform.rotation;

        //// choose an object and multiply its axis to reverse
        //var rotation = box2.transform.rotation;

        //rotation.y = (targetRotation * Quaternion.Euler(0, 180f, 0)).y;
        //box2.transform.rotation = rotation;

        var camRotation = Camera.main.transform.rotation;
        var boxRoation = box1.transform.rotation;
        boxRoation.y = -camRotation.y;
        box1.transform.rotation = boxRoation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plane;
    public GameObject cube;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var heightCube = cube.transform.localScale.y;
        var distance = Vector2.Distance(new Vector2(0,cube.transform.position.y),new Vector2(0,plane.transform.position.y));
        //Debug.Log($"Distance {distance}");
        if (distance <= (heightCube / 2))
        {
            Debug.Log("Hit");
        }
        //var ray = new Ray(cube.transform.position, cube.transform.up);
        //RaycastHit hit;
        
        //if(Physics.Raycast(ray,out hit, 10000))
        //{
        //    Debug.Log(hit.transform.name);
        //}    
    }
}

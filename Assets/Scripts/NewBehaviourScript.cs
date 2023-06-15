using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject theBox;
    public GameObject sphere2;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Instantiate(theBox);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(theBox.transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //Debug.Log($"onScreen: {onScreen}");

        //float angel = Vector3.Angle(theBox.transform.forward, sphere2.transform.position - transform.position);
        //Debug.Log($"Angle: {Mathf.Abs(angel)}");
        //if (Mathf.Abs(angel) > 30)
        //    print("Object2 if front Obj1");
        //gameObject.SetActive(onScreen);
    }
}

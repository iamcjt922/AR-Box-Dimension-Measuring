
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Rendering;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ARCusor : MonoBehaviour
{
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    public ARPlaneManager aRPlaneManager;
    
    public GameObject DefaultARPlane;

    public PlaneAreaBehaviour planeAreaBehaviour;
    public bool placed = false;
    bool hasHits = false;


    public LockScript lockScript;
    public MeasurementController measurementController;
    private void Start()
    {
        cursorChildObject.SetActive(false);
    }
    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
   
        UpadteCursor();
     
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {


            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //if (planeAreaBehaviour.biggestPlane != null)
                //{
                //    var position = transform.position;
                //    position.y = planeAreaBehaviour.biggestPlane.transform.position.y;
                //    objectToPlace.transform.position = position;
                //}
                //else
                //{
                //    objectToPlace.transform.position = transform.position;

                //}
                objectToPlace.transform.position = transform.position;
                // if (lockScript.isClock == false)
                // {

                // }
                this.gameObject.SetActive(true);
                objectToPlace.SetActive(true);
                measurementController.PlaceBox();
                placed = !placed;
                if (measurementController.lockScript.isClock)
                {
                    placed = true;
                }
                //this.cursorChildObject.SetActive(!objectToPlace.active);
                //objectToPlace.SetActive(true);

            }

          


        }
        

    }
    
    void UpadteCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
       
        if (hits.Count> 0)
        {
           
            cursorChildObject.SetActive(true);
            if (placed == false) {
               
                var position= hits[0].pose.position;
                //if (planeAreaBehaviour.biggestPlane != null)
                //{
                //    position.y = planeAreaBehaviour.biggestPlane.transform.position.y;

                //}
                transform.position = position;
                transform.rotation = hits[0].pose.rotation;
            }

         
      


        }

    }
    

}

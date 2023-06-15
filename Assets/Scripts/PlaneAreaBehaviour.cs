using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#nullable enable
public class PlaneAreaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public ARPlaneManager aRPlaneManager;
    public float maxSize = 0.0f;
    public ARPlane? biggestPlane;
    private void OnEnable()
    {
        aRPlaneManager.planesChanged += ArPlane_BoundaryChanged;
    }

   
    private void ArPlane_BoundaryChanged(ARPlanesChangedEventArgs obj)
    {
     
        foreach (var plane in obj.removed)
        {
            if (plane.trackableId == biggestPlane?.trackableId)
            {
                //biggestPlane = null;
                //maxSize = 0;
            }
        }
        foreach (var plane in obj.added)
        {

            var max = CalculatePlaneArea(plane);
            if (max > maxSize)
            {
                maxSize = max;
                biggestPlane = plane;
                Debug.Log($"biggestPlane size: {maxSize},position {biggestPlane.transform.position}");
            }
        }
        foreach(var plane in obj.updated)
        {
            var max = CalculatePlaneArea(plane);
            if (max > maxSize)
            {
                maxSize = max;
                biggestPlane = plane;
                Debug.Log($"biggestPlane size: {maxSize},position {biggestPlane.transform.position}");
            }
        }
      
    }
    private float CalculatePlaneArea(ARPlane plane)
    {    
        return plane.size.x * plane.size.y;
    }
}

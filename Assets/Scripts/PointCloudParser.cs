using System.Collections;
using System.Collections.Generic;
using RengeGames.HealthBars;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudParser : MonoBehaviour
{
    private ARPointCloudManager pointCloudManager;
    private GameObject theBox;
    private MinimalBoundingBoxScript minimalBoundingBoxScript;
    
    private ARCusor aRCusor;
    public List<ARPoint> golbalPoints = new List<ARPoint>();
    private GameObject sphere;
    int minW = 3;
    private List<GameObject> spheres=new List<GameObject>();
    public bool stopScan = false;
    public LockScript lockScript;
    private bool setBoxActive = false;
    public TextMeshProUGUI percent;
    public UltimateCircularHealthBar hb;
    private PlaneAreaBehaviour planeAreaBehaviour;
    public PointCloudParser(ARPointCloudManager aRPointCloudManager,GameObject theBox,ARCusor aRCusor,GameObject sphere,LockScript lockScript, UltimateCircularHealthBar hb, TextMeshProUGUI percent,   PlaneAreaBehaviour planeAreaBehaviour)
    {
        
        this.pointCloudManager = aRPointCloudManager;
        this.theBox = theBox;
        minimalBoundingBoxScript = new MinimalBoundingBoxScript(theBox);
        this.aRCusor = aRCusor;
        this.sphere = sphere;
        this.lockScript = lockScript;
        this.hb = hb;
        this.percent = percent;
        this.planeAreaBehaviour = planeAreaBehaviour;
        //PCLNuget.;

        
    }

    public void OnEnable()
    {
        pointCloudManager.pointCloudsChanged += PointCloudManager_pointCloudsChanged;
       

    }
    private int findNearestPoint(ARPoint point, List<ARPoint> listPoints)
    {
        var minDistance = 10000f;
      
        int index=0;
      
       for(int i = 0; i < listPoints.Count; i++)
        {
            var distance = Vector3.Distance(point.toVector(), listPoints[i].toVector());
            if (distance < minDistance) {
                minDistance = distance;
                index = i;
               
            }
        }
        Debug.Log($"min distance: {minDistance}");
        return index;
    }

    private bool isInViewPort(ARPoint point)
    {
    
        var cameraPosition = Camera.main.transform.position;
        var cursorPosition = aRCusor.transform.position;
        var pointPosition = point.toVector();
        cameraPosition.y = 0;
        cursorPosition.y = 0;
        pointPosition.y = 0;
        var distanceFromCameraToCenter = Vector3.Distance(cameraPosition,cursorPosition);
        var distance = Vector3.Distance(cameraPosition, pointPosition);
        Debug.Log($"DistanceFromCamera: {distanceFromCameraToCenter}, distance position: {distance}");
        if (distance <= distanceFromCameraToCenter)
        {
            //Physics.Raycast(point,)
            var height =point.toVector().y/2;
            var distancePlane = Vector2.Distance(new Vector2(0, point.toVector().y), new Vector2(0,planeAreaBehaviour.biggestPlane.transform.position.y));
           
            if (distancePlane <= (height))
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        else
        {
            return false;
        }


        //var distanceFromCameraToCenter = Vector3.Distance(aRCusor.transform.position,Camera.main.transform.position);


        //var distance = Vector3.Distance(point.toVector(),Camera.main.transform.position);
        //Debug.Log($"DistanceFromCamera: {distanceFromCameraToCenter}, distance position: {distance}");
        //if (distance <= distanceFromCameraToCenter)
        //{
        //    return true;
        //}
        //return false;

        //Debug.Log($"Distance: {distance}, cursor position: {aRCusor.cursorChildObject.transform.position}");
        //if (distance <= 0.4 && distance >= -0.4)
        //{

        //    return true;
        //}
        //return false;
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(point.toVector());

        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //if (onScreen)
        //{

        //    float angel = Vector3.Angle(aRCusor.transform.forward, point.toVector() - transform.position);
        //    if (angel >= 90)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //else
        //{
        //    return false;

        //}


    }
    private int findMaxHeight(List<ARPoint> arPoints)
    {
        int index = 0;
       var maxHeight = -100000f;
        for(int i = 0; i < arPoints.Count; i++)
        {
            if (arPoints[i].y > maxHeight)
            {
                maxHeight = arPoints[i].y;
                index = i;
            }
        }
        return index;
    }
    private List<ARPoint> heightFilters(List<ARPoint> aRPoints,float maxHeight)
    {
        var avg = maxHeight *0.2f;
        if (maxHeight < 0) {
            avg = maxHeight / 0.2f;
        }
        
        List<ARPoint> list = new List<ARPoint>();
        for(int i = 0; i < aRPoints.Count; i++) {
            if (aRPoints[i].y >= avg)
            {
                list.Add(aRPoints[i]);
            }
        }
        return list;

        //return aRPoints;
    }
    private List<ARPoint> sortList(List<ARPoint> list)
    {
        var aRPoints = list;
        for(int i = 0; i < aRPoints.Count-1; i++)
        {
            int minIndex = i;
            for(int j = i + 1; j < aRPoints.Count; j++)
            {
                if (aRPoints[i].y < aRPoints[minIndex].y)
                {
                    minIndex = j;
                }   
            }
            var temp = aRPoints[i];
            aRPoints[i] = aRPoints[minIndex];
            aRPoints[minIndex] = temp;
        }
        Debug.Log($"first item height: {aRPoints[0].y}, last item height: {aRPoints[aRPoints.Count-1].y}");
        var result= aRPoints.GetRange(0, list.Count / 2);
        Debug.Log($"Height filter: {result.Count}");
        //return result;
        return result;
    }
    private void mergePoint(ARPointCloud aRPointCloud, bool mergeNearPoint)
    {

        for (int i = 0; i < aRPointCloud.positions.Value.Length; i++)
        {
            var pos = aRPointCloud.positions.Value[i];
            var id = aRPointCloud.identifiers.Value[i];


            ARPoint newPoint = new ARPoint(id, pos, 1);
            if (isInViewPort(newPoint) == false)
            {
                return;
            }
            if (golbalPoints.Count == 0)
            {
                if (isInViewPort(newPoint))
                {
                    golbalPoints.Add(newPoint);
                }


            }
            else
            {
                var oldPoint = golbalPoints.Find((x) => x.id == newPoint.id);
                if (oldPoint != null)
                {

                    Debug.Log($"oldPoint Id: {oldPoint.id}, newPoint Id: {newPoint.id} ");
                    var index = golbalPoints.IndexOf(oldPoint);
                    Debug.Log($"Index: {index}");
                    oldPoint.weight += newPoint.weight;
                    golbalPoints[index] = oldPoint;
                    //if (oldPoint.weight >= 3)
                    //{
                    //    var newSphere = Instantiate(sphere, oldPoint.toVector(), Quaternion.identity);
                    //    newSphere.name = oldPoint.id.ToString();
                    //    spheres.Add(newSphere);
                    //}

                    //int j = findNearestPoint(oldPoint, golbalPoints);
                    //if (golbalPoints[j].id != oldPoint.id)
                    //{
                    //    if (golbalPoints[j].weight > oldPoint.weight)
                    //    {
                    //        var point = golbalPoints[j];
                    //        point.weight += oldPoint.weight;
                    //        golbalPoints[j] = point;
                    //    }
                    //    else if(oldPoint.weight>golbalPoints[j].weight)
                    //    {
                    //        var point = oldPoint;
                    //        point.weight += golbalPoints[j].weight;
                    //        golbalPoints[index] = point;
                    //    }
                    //}

                    //golbalPoints[index] = oldPoint;
                }
                else
                {
                    if (mergeNearPoint)
                    {
                        var index = findNearestPoint(newPoint, golbalPoints);
                        if (golbalPoints[index].weight > newPoint.weight)
                        {
                            var point = golbalPoints[index];
                            point.weight += newPoint.weight;
                            golbalPoints[index] = point;

                            //if (point.weight >= 3)
                            //{
                            //    var newSphere = Instantiate(sphere, point.toVector(), Quaternion.identity);
                            //    spheres.Add(newSphere);
                            //    newSphere.name = point.id.ToString();
                            //}
                        }
                        else
                            {
                                if (isInViewPort(newPoint))
                                {

                                    golbalPoints.Add(newPoint);

                                }
                                else
                                {
                                    Debug.Log($"not in viewport: {newPoint.toVector()}");
                                }
                            }
                        } else if (isInViewPort(newPoint))
                        {
                         golbalPoints.Add(newPoint);
                        }


                    }

                }
            }
    }
    private void PointCloudManager_pointCloudsChanged(ARPointCloudChangedEventArgs obj)
    {
        if (stopScan == false)
        {
            if (lockScript.isClock)
            {

                if (aRCusor.placed)
                {
                    foreach (var pointCloud in obj.removed)
                    {
                        for (int i = 0; i < pointCloud.positions.Value.Length; i++)
                        {
                            var pos = pointCloud.positions.Value[i];
                            var id = pointCloud.identifiers.Value[i];
                            var point = golbalPoints.Find((x) => x.id == id);
                            if (point != null)
                            {
                                golbalPoints.Remove(point);
                                //var sphere = spheres.Find(x => x.name == id.ToString());
                                //if (sphere != null)
                                //{
                                //    sphere.SetActive(false);
                                //    spheres.Remove(sphere);
                                //}
                            }

                        }
                    }
                    //foreach (var pointCloud in obj.added)
                    //{
                    //    mergePoint(pointCloud, true);
                    //}

                    foreach (var pointCloud in obj.updated)
                    {
                        mergePoint(pointCloud, false);
                    }



                }
            }
        }
        

    }
    public void placeBox() {
        var finalPoints = golbalPoints.FindAll((x) => x.weight >= 3);

        Debug.Log($"globalPoints: {golbalPoints.Count}, finalPoints: {finalPoints.Count}");

        var percent = ((float)finalPoints.Count / (float)golbalPoints.Count);
        Debug.Log($"Percent: {percent}");
        stopScan = true;
        int maxHeightIndex = findMaxHeight(finalPoints);
        var pointMaxHeight = finalPoints[maxHeightIndex];
        var height = Vector2.Distance(new Vector2(0, pointMaxHeight.y),new Vector2(0,planeAreaBehaviour.transform.position.y));
        Debug.Log($"MaxHeight: {height}");
        //var listFiltered = heightFilters(finalPoints, pointMaxHeight.y);
        //var listFiltered = sortList(finalPoints);
        var listFiltered = finalPoints;
        var planePosition = aRCusor.transform.position;
        planePosition.y = planeAreaBehaviour.biggestPlane.transform.position.y;

        listFiltered.Add(new ARPoint(123, planePosition, 3));
        Debug.Log($"listFiltered: {listFiltered.Count}");
        List<Vector3> vectors = new List<Vector3>();

        foreach (var point in listFiltered)
        {
            vectors.Add(point.toVector());
            //Instantiate(sphere, point.toVector(), Quaternion.identity);

        }
        theBox.SetActive(true);


        minimalBoundingBoxScript.OnDrawTheBox(points: vectors, pointMaxHeight.y, planePosition);
        //var position=theBox.transform.position;
        //position
        //stopScan = true;
        //theBox.transform.position = planePosition;

    }

}

public class ARPoint
{
    public float x;
    public float y;
    public float z;
    public int weight;
    public ulong id;
    public ARPoint(ulong id, Vector3 pos,int weight)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        this.weight = weight;
        this.id = id;
    }
    public bool isEqual(ARPoint point)
    {
        return this.x == point.x && this.y == point.y && this.z== point.z;
    }
    public  Vector3 toVector()
    {
        return new Vector3(x, y, z);
    }
}
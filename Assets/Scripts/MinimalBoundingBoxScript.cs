using System.Collections.Generic;
using g3;
using UnityEngine;
public class MinimalBoundingBoxScript : MonoBehaviour
{
    
    // Just for the demo I used Transforms so I can simply move them around in the scene
    public Transform[] transforms;
    public GameObject theBox;
    public MinimalBoundingBoxScript(GameObject theBox)
    {
        this.theBox = theBox;
    }
    public void  OnDrawTheBox(List<Vector3> points,float maxHeight,Vector3 centerPosition)
    {
        // First wehave to convert the Unity Vector3 array
        // into the g3 type g3.Vector3d

        var points3d = new Vector3d[points.Count];
        for (var i = 0; i < points.Count; i++)
        {
            // Thanks to the g3 library implictely casted from UnityEngine.Vector3 to g3.Vector3d
            var vector = points[i];
            points3d[i] = new g3.Vector3d(vector.x, vector.y, vector.z);

        }

        // BOOM MAGIC!!!
        var orientedBoundingBox = new ContOrientedBox3(points3d);


        // Now just convert the information back to Unity Vector3 positions and axis
        // Since g3.Vector3d uses doubles but Unity Vector3 uses floats
        // we have to explicitly cast to Vector3
        Vector3 center = (Vector3)orientedBoundingBox.Box.Center.toVector();

        var axisX = (Vector3)orientedBoundingBox.Box.AxisX.toVector();
        var axisY = (Vector3)orientedBoundingBox.Box.AxisY.toVector();
        var axisZ = (Vector3)orientedBoundingBox.Box.AxisZ.toVector();
        var extends = (Vector3)orientedBoundingBox.Box.Extent.toVector();

        //Debug.Log($"axisX: {axisX}, axisY: {axisY}, axisZ: {axisZ}");
        //var scale = theBox.transform.localScale;
        //scale.x = axisX.x;
        //scale.y = axisY.y;
        //scale.z = axisZ.z;
        //theBox.transform.localScale = scale;

        // Now we can simply calculate our 8 vertices of the bounding box
        var A = center - extends.z * axisZ - extends.x * axisX - axisY * extends.y;
        var B = center - extends.z * axisZ + extends.x * axisX - axisY * extends.y;
        var C = center - extends.z * axisZ + extends.x * axisX + axisY * extends.y;
        var D = center - extends.z * axisZ - extends.x * axisX + axisY * extends.y;

        var E = center + extends.z * axisZ - extends.x * axisX - axisY * extends.y;
        var F = center + extends.z * axisZ + extends.x * axisX - axisY * extends.y;
        var G = center + extends.z * axisZ + extends.x * axisX + axisY * extends.y;
        var H = center + extends.z * axisZ - extends.x * axisX + axisY * extends.y;

        var scale = theBox.transform.localScale;
        //scale.x = Vector3.Distance(A, B);
        //scale.y = Vector3.Distance(B, C);
        //scale.z = Vector3.Distance(A, H);
        var distanceY = Vector3.Distance(A, B);
        var distanceX = Vector3.Distance(B, C);
        var distanceZ = Vector3.Distance(A, H);
        Debug.Log($"DistanceY: {distanceY}");
        //if (maxHeight == distanceX)
        //{
        //    Debug.Log($"Max Height is X: {distanceX}");
        //    distanceX = distanceY;
        //    distanceY = maxHeight;

        //}else if (maxHeight == distanceZ)
        //{
        //    distanceZ = distanceY;
        //    distanceY = maxHeight;
        //    Debug.Log($"Max Height is Z: {distanceZ}");
        //}
        scale.y = distanceY;
        scale.x = distanceX;
        scale.z = distanceZ;


        theBox.transform.localScale = scale;
        theBox.transform.position = centerPosition;
        theBox.SetActive(true);
        //// And finally visualize it
        //Gizmos.DrawLine(A, B);
        //Gizmos.DrawLine(B, C);
        //Gizmos.DrawLine(C, D);
        //Gizmos.DrawLine(D, A);

        //Gizmos.DrawLine(E, F);
        //Gizmos.DrawLine(F, G);
        //Gizmos.DrawLine(G, H);
        //Gizmos.DrawLine(H, E);

        //Gizmos.DrawLine(A, E);
        //Gizmos.DrawLine(B, F);
        //Gizmos.DrawLine(D, H);
        //Gizmos.DrawLine(C, G);
    }
    public void OnDrawGizmos()
    {
        //OpenCvSharp.

        // First wehave to convert the Unity Vector3 array
        // into the g3 type g3.Vector3d
     
        var points3d = new Vector3d[transforms.Length];
        for (var i = 0; i < transforms.Length; i++)
        {
            // Thanks to the g3 library implictely casted from UnityEngine.Vector3 to g3.Vector3d
            var vector = transforms[i].position;
            points3d[i] = new g3.Vector3d(vector.x,vector.y,vector.z);
           
        }

        // BOOM MAGIC!!!
        var orientedBoundingBox = new ContOrientedBox3(points3d);

       
        // Now just convert the information back to Unity Vector3 positions and axis
        // Since g3.Vector3d uses doubles but Unity Vector3 uses floats
        // we have to explicitly cast to Vector3
        Vector3 center = (Vector3)orientedBoundingBox.Box.Center.toVector();

        var axisX = (Vector3)orientedBoundingBox.Box.AxisX.toVector();
        var axisY = (Vector3)orientedBoundingBox.Box.AxisY.toVector();
        var axisZ = (Vector3)orientedBoundingBox.Box.AxisZ.toVector();
        var extends = (Vector3)orientedBoundingBox.Box.Extent.toVector();

        //Debug.Log($"axisX: {axisX}, axisY: {axisY}, axisZ: {axisZ}");
        //var scale = theBox.transform.localScale;
        //scale.x = axisX.x;
        //scale.y = axisY.y;
        //scale.z = axisZ.z;
        //theBox.transform.localScale = scale;

        // Now we can simply calculate our 8 vertices of the bounding box
        var A = center - extends.z * axisZ - extends.x * axisX - axisY * extends.y;
        var B = center - extends.z * axisZ + extends.x * axisX - axisY * extends.y;
        var C = center - extends.z * axisZ + extends.x * axisX + axisY * extends.y;
        var D = center - extends.z * axisZ - extends.x * axisX + axisY * extends.y;

        var E = center + extends.z * axisZ - extends.x * axisX - axisY * extends.y;
        var F = center + extends.z * axisZ + extends.x * axisX - axisY * extends.y;
        var G = center + extends.z * axisZ + extends.x * axisX + axisY * extends.y;
        var H = center + extends.z * axisZ - extends.x * axisX + axisY * extends.y;

        //var scale = theBox.transform.localScale;
        //scale.x = Vector3.Distance(A, B);
        //scale.y = Vector3.Distance(B, C);
        //scale.z = Vector3.Distance(A, H);
        //theBox.transform.localScale = scale;
 
        // And finally visualize it
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(B, C);
        Gizmos.DrawLine(C, D);
        Gizmos.DrawLine(D, A);

        Gizmos.DrawLine(E, F);
        Gizmos.DrawLine(F, G);
        Gizmos.DrawLine(G, H);
        Gizmos.DrawLine(H, E);

        Gizmos.DrawLine(A, E);
        Gizmos.DrawLine(B, F);
        Gizmos.DrawLine(D, H);
        Gizmos.DrawLine(C, G);
     


    }
}

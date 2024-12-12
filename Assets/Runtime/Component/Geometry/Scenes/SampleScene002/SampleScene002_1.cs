using System.Collections.Generic;
using GeometryAssist;
using UnityEngine;
using static GeometryAssist.GeometryAssistTool;
public class SampleScene002_1 : MonoBehaviour
{
    public OrientedBoundsVisual obbVisual;
    public Transform rayTr;
    public float distance = 0.5f;
    public void Start()
    {

    }

    SphereBounds[] spheres;
    void OnDrawGizmos()
    {
        if (spheres != null)
        {
            for (int i = 0; i < spheres.Length; i++)
            {
                var s = spheres[i];
                Gizmos.DrawWireSphere(s.center, s.radius);
            }
        }
    }

    public void Update()
    {
        var obb = obbVisual.obb;
        var p = rayTr.position;
        var dir = rayTr.forward;
        dir *= distance;
        Debug.DrawRay(p, dir, Color.white);
        if (PointInsideOrientedBounds(p, obb))
        {
            Debug.Log("在内部");
        }
        // if (Tool.IntersectLine(p, p + dir, obb, out var dis))
        // {
        //     Debug.Log(dis);
        // }
        if (GeometryAssistTool.Intersect(new Ray(p, dir), obb, out var dis))
        {
            Debug.Log(dis);
        }
    }

}
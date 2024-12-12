using System.Collections.Generic;
using GeometryAssist;
using UnityEngine;
using static GeometryAssist.GeometryAssistTool;
public class SampleScene002_2 : MonoBehaviour
{
    public OrientedBoundsVisual obbVisual;
    public OrientedBoundsVisual obbVisual2;

    SphereBounds sphere = default;
    void OnDrawGizmos()
    {
        if (!sphere.Equals(default))
        {
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
        }
    }

    public void Update()
    {
        GeometryAssistTool.OBBToSphereBounds_HighPrecision(obbVisual2.obb, out sphere);
        if (GeometryAssistTool.OBBIntersect(obbVisual.obb, sphere))
        {
            obbVisual.color = Color.red;
        }
        else
        {
            obbVisual.color = Color.yellow;
        }
    }
}
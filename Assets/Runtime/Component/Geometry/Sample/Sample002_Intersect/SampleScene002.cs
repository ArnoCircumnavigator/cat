using System.Collections.Generic;
using GeometryAssist;
using UnityEngine;
using static GeometryAssist.GeometryAssistTool;
public class SampleScene002 : MonoBehaviour
{
    public int count = 10;
    OrientedBoundsVisual[] visuals;
    public void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var g = new GameObject();
            g.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            g.transform.rotation = Random.rotationUniform;
            var visual = g.AddComponent<OrientedBoundsVisual>();
            visual.obb.extents = new Vector3(Random.Range(1, 5), Random.Range(1, 4), Random.Range(1, 3));
        }
        visuals = GameObject.FindObjectsOfType<OrientedBoundsVisual>();
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

    float moveRange = 10f; // 移动范围半径
    float rotationRange = 360.0f; // 旋转角度范围
    public void Update()
    {
        // foreach (var v in visuals)
        // {
        //     float randomX = Random.Range(-moveRange, moveRange);
        //     float randomY = Random.Range(-moveRange, moveRange);
        //     float randomZ = Random.Range(-moveRange, moveRange);

        //     v.transform.position = new Vector3(randomX, randomY, randomZ);

        //     // 旋转物体
        //     float randomRotationX = Random.Range(-rotationRange, rotationRange);
        //     float randomRotationY = Random.Range(-rotationRange, rotationRange);
        //     float randomRotationZ = Random.Range(-rotationRange, rotationRange);

        //     v.transform.rotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);
        // }


        var length = visuals.Length;
        spheres = new SphereBounds[length];
        for (int i = 0; i < length; i++)
        {
            //OBBToBoundingSphere_HighPrecision(visuals[i].obb, out var s);
            OBBToSphereBounds_LowPrecision(visuals[i].obb, out var s);
            spheres[i] = s;

            visuals[i].color = Color.yellow;
        }

        List<int> intersectIndexes = new List<int>();

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (i == j)
                    continue;
                if (Intersect(spheres[i], spheres[j]))
                {
                    //Debug.Log("球碰一起了");
                    if (OBBIntersect(visuals[i].obb, visuals[j].obb))
                    {
                        intersectIndexes.Add(i);
                        intersectIndexes.Add(j);
                    }
                }
                // if (OBBIntersect(visuals[i].obb, visuals[j].obb))
                // {
                //     intersectIndexes.Add(i);
                //     intersectIndexes.Add(j);
                // }
            }
        }

        foreach (var index in intersectIndexes)
        {
            visuals[index].color = Color.red;
        }
    }

}
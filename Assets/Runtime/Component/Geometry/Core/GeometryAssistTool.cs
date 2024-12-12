/*----------------------------------------------------------------
 * User    : Arno [Copyright (C) 2024]
 * Time    ：2024/1/22
 * CLR     ：
 * filename：
 * E-male  ：1961925755@qq.com
 
 * Desc    : Focused on addressing intersection issues in spatial geometry
 *
 * ----------------------------------------------------------------
 * Modification
 *
 *----------------------------------------------------------------*/

using System.Linq;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace GeometryAssist
{
    [BurstCompile]
    public static partial class GeometryAssistTool
    {
        /// <summary>
        /// 通过mesh计算包围球
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        static SphereBounds CalculateBoundingSphere(Mesh mesh)
        {
            float3 center = CalculateCentroid(mesh.vertices.Select(c => (float3)c).ToArray());
            float radius = 0.0f;
            foreach (float3 vertex in mesh.vertices)
            {
                float distanceSquared = math.lengthsq(center - vertex);
                radius = math.max(radius, math.sqrt(distanceSquared));
            }
            return new SphereBounds(center, radius);
        }

        /// <summary>
        /// 通过顶点计算形心
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        static float3 CalculateCentroid(float3[] vertices)
        {
            float3 centroid = float3.zero;
            foreach (float3 vertex in vertices)
            {
                centroid += vertex;
            }
            centroid /= vertices.Length;
            return centroid;
        }

        /// <summary>
        /// 判断点是否在obb中
        /// </summary>
        /// <param name="point"></param>
        /// <param name="obb"></param>
        /// <returns></returns>
        [BurstCompile]
        public static bool PointInsideOrientedBounds(in float3 point, in OrientedBounds obb)
        {
            // 将点的世界坐标变换到OBB的局部空间
            float3 localPoint = math.rotate(math.inverse(obb.rotation), point - obb.center);

            // 判断点在OBB局部空间的每一个轴上是否都在范围内
            bool inside =
                math.abs(localPoint.x) <= obb.extents.x &&
                math.abs(localPoint.y) <= obb.extents.y &&
                math.abs(localPoint.z) <= obb.extents.z;

            return inside;
        }

        /// <summary>
        /// 线段与OBB是否相交
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <param name="obb"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        [BurstCompile]
        public static bool Intersect(
            in float3 lineStart,
            in float3 lineEnd,
            in OrientedBounds obb,
            out float distance)
        {
            Ray ray = new Ray(lineStart, math.normalize(lineEnd - lineStart));
            //float3 localLineEnd = math.rotate(math.inverse(obb.rotation), lineEnd - obb.center);

            // 首先使用无限长的线计算交点
            if (Intersect(ray, obb, out distance))
            {
                // 然后检查计算出的距离是否在线段长度之内
                // 计算线段的长度平方
                float segmentLengthSq = math.lengthsq(lineEnd - lineStart);
                // 如果距离的平方小于线段长度的平方，则线段与OBB相交
                if (distance * distance <= segmentLengthSq)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 射线与OBB是否相交
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="obb"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        [BurstCompile]
        public static bool Intersect(in Ray ray, in OrientedBounds obb, out float distance)
        {
            // 将射线起点和方向变换到OBB的局部空间
            float3 localRayOrigin = math.rotate(math.inverse(obb.rotation), (float3)ray.origin - obb.center);
            float3 localRayDirection = math.rotate(math.inverse(obb.rotation), ray.direction);

            // 用局部空间的线与AABB进行交点测试
            bool intersects = Intersect(localRayOrigin, localRayDirection, obb.extents, out distance);

            return intersects;
        }

        /// <summary>
        /// 定长射线与OBB是否相交
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="obb"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        [BurstCompile]
        public static bool Intersect(in FixedLengthRay ray, in OrientedBounds obb, out float distance)
        {
            // 将射线起点和方向变换到OBB的局部空间
            float3 localRayOrigin = math.rotate(math.inverse(obb.rotation), (float3)ray.origin - obb.center);
            float3 localRayDirection = math.rotate(math.inverse(obb.rotation), math.normalize(ray.direction));

            // 用局部空间的线与AABB进行交点测试
            bool intersects = Intersect(localRayOrigin, localRayDirection, obb.extents, out distance)
                                &&
                                distance <= math.length(ray.direction);
            return intersects;
        }

        /// <summary>
        /// 射线与AABB是否相交，且给出距离
        /// </summary>
        /// <param name="rayOrigin">射线原点</param>
        /// <param name="rayDirection">射线方向</param>
        /// <param name="extents">aabb半尺寸</param>
        /// <param name="distance">距离（如果相交）</param>
        /// <returns>相交与否</returns>
        [BurstCompile]
        static bool Intersect(in float3 rayOrigin,in float3 rayDirection,in float3 extents, out float distance)
        {
            distance = 0f;
            float3 tMin = (-extents - rayOrigin) / rayDirection;
            float3 tMax = (extents - rayOrigin) / rayDirection;
            float3 t1 = math.min(tMin, tMax);
            float3 t2 = math.max(tMin, tMax);

            float tNear = math.max(math.max(t1.x, t1.y), t1.z);
            float tFar = math.min(math.min(t2.x, t2.y), t2.z);

            if (tNear > tFar || tFar < 0f)
            {
                return false;
            }

            distance = tNear > 0f ? tNear : tFar;
            return true;
        }

        /// <summary>
        /// 计算两个包围球是否相交
        /// </summary>
        /// <param name="sphere1">球</param>
        /// <param name="sphere2">另一个球</param>
        /// <returns></returns>
        [BurstCompile]
        public static bool Intersect(in SphereBounds sphere1, in SphereBounds sphere2)
        {
            if (sphere1.radius.Approximately(0, 0.001f) || sphere2.radius.Approximately(0, 0.001f))
                return false;
            float distanceSquared = math.lengthsq(sphere2.center - sphere1.center);
            float radiusSum = sphere1.radius + sphere2.radius;
            return distanceSquared <= radiusSum * radiusSum;
        }

        /// <summary>
        /// 将obb转化为包围球(高精度)
        /// </summary>
        /// <param name="obb"></param>
        /// <returns></returns>
        [BurstCompile]
        public unsafe static void OBBToSphereBounds_HighPrecision(in OrientedBounds obb, out SphereBounds boundingSphere)
        {
            var vertices = stackalloc float3[8];
            GetOBBVertices(obb, vertices);
            float maxDistanceSquared = 0.0f;
            for (int i = 0; i < 8; i++)
            {
                var vertex = vertices[i];
                float distanceSquared = math.lengthsq(obb.center - vertex);
                if (distanceSquared > maxDistanceSquared)
                {
                    maxDistanceSquared = distanceSquared;
                }
            }
            float radius = math.sqrt(maxDistanceSquared);

            boundingSphere = new SphereBounds(obb.center, radius);
        }

        /// <summary>
        /// 将obb转化为包围球(低精度,高性能)
        /// </summary>
        /// <param name="obb"></param>
        /// <returns></returns>
        [BurstCompile]
        public unsafe static void OBBToSphereBounds_LowPrecision(in OrientedBounds obb, out SphereBounds boundingSphere)
        {
            Vector3 t = obb.extents;
            float half_diagonal = t.magnitude;//对角线的一半
            boundingSphere = new SphereBounds(obb.center, half_diagonal + 0.1f);
        }

        /// <summary>
        /// 比下面的更优
        /// </summary>
        /// <param name="obb"></param>
        /// <param name="vertices"></param>
        [BurstCompile]
        public unsafe static void GetOBBVertices(in OrientedBounds obb, float3* vertices)
        {
            float3x3 rotationMatrix = math.float3x3(obb.rotation);
            float3x3 axis = new float3x3(
                rotationMatrix.c0 * obb.extents.x,
                rotationMatrix.c1 * obb.extents.y,
                rotationMatrix.c2 * obb.extents.z
            );

            // 通过对角线组合来产生OBB的8个顶点
            vertices[0] = obb.center - axis[0] - axis[1] - axis[2];
            vertices[1] = obb.center + axis[0] - axis[1] - axis[2];
            vertices[2] = obb.center - axis[0] + axis[1] - axis[2];
            vertices[3] = obb.center + axis[0] + axis[1] - axis[2];
            vertices[4] = obb.center - axis[0] - axis[1] + axis[2];
            vertices[5] = obb.center + axis[0] - axis[1] + axis[2];
            vertices[6] = obb.center - axis[0] + axis[1] + axis[2];
            vertices[7] = obb.center + axis[0] + axis[1] + axis[2];
        }

        //[BurstCompile]
        //public static bool OBBIntersect(in BoundingSphere sphere, in OrientedBounds obb)
        //{
        //    float3 sphereCenterInOBB = math.rotate(math.inverse(obb.rotation), sphere.center - obb.center);

        //    // 获取球心到OBB的每个面的距离
        //    float3 distances = math.abs(sphereCenterInOBB) - obb.extents;

        //    // 查找最大的分离轴
        //    float maxSeparation = math.max(math.max(distances.x, distances.y), distances.z);

        //    // 如果最大分离轴小于等于球的半径，意味着相交
        //    return maxSeparation <= sphere.radius;
        //}

        [BurstCompile]
        public static bool OBBIntersect(in OrientedBounds obb, in SphereBounds sphere)
        {
            // 将球体中心点从世界坐标系转换到OBB的局部坐标系
            float3 sphereCenterInOBB = math.mul(math.inverse(obb.rotation), (sphere.center - obb.center));

            // 找出球心到OBB中心的最短距离
            float3 closestPointInOBB = math.max(math.min(sphereCenterInOBB, obb.extents), -obb.extents);

            //检查距离
            float distanceSquared = math.distancesq(closestPointInOBB, sphereCenterInOBB);

            // 如果最短距离的平方小于球半径的平方，则相交
            return distanceSquared <= (sphere.radius * sphere.radius);
        }

        /// <summary>
        /// 计算两个obb是否相交（精度很高，单消耗也大 单线程10万次计算大概6.65ms）
        /// </summary>
        /// <param name="obb1"></param>
        /// <param name="obb2"></param>
        /// <returns></returns>
        [BurstCompile]
        public static unsafe bool OBBIntersect(in OrientedBounds obb1, in OrientedBounds obb2)
        {
            // 计算一个OBB的局部坐标系（3个轴）
            var axes1 = stackalloc float3[3];
            axes1[0] = math.normalize(math.rotate(obb1.rotation, new float3(1, 0, 0))); // OBB1的局部X轴
            axes1[1] = math.normalize(math.rotate(obb1.rotation, new float3(0, 1, 0))); // OBB1的局部Y轴
            axes1[2] = math.normalize(math.rotate(obb1.rotation, new float3(0, 0, 1))); // OBB1的局部Z轴

            // 计算另一个OBB的局部坐标系（同样3个轴）
            var axes2 = stackalloc float3[3];
            axes2[0] = math.normalize(math.rotate(obb2.rotation, new float3(1, 0, 0))); // OBB2的局部X轴
            axes2[1] = math.normalize(math.rotate(obb2.rotation, new float3(0, 1, 0))); // OBB2的局部Y轴
            axes2[2] = math.normalize(math.rotate(obb2.rotation, new float3(0, 0, 1))); // OBB2的局部Z轴

            // 检查OBB1的每一个轴
            for (int i = 0; i < 3; i++)
            {
                if (!OverlapsOnAxis(obb1, obb2, axes1[i]))
                    return false;
            }

            // 检查OBB2的每一个轴
            for (int i = 0; i < 3; i++)
            {
                if (!OverlapsOnAxis(obb1, obb2, axes2[i]))
                    return false;
            }

            // 检查叉积形成的轴
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float3 axis = math.cross(axes1[i], axes2[j]);
                    if (math.lengthsq(axis) > 1E-6f && !OverlapsOnAxis(obb1, obb2, axis))
                        return false;
                }
            }

            // 如果所有轴上都重叠，那么OBB相交
            return true;
        }

        // 这个辅助函数用来检查两个OBB在指定轴上的投影是否重叠
        [BurstCompile]
        static bool OverlapsOnAxis(in OrientedBounds obb1, in OrientedBounds obb2, in float3 axis)
        {
            // 计算OBB在指定轴上的投影区间
            ProjectOBB(obb1, axis, out Interval interval1);
            ProjectOBB(obb2, axis, out Interval interval2);

            // 检查区间是否重叠
            return interval1.min <= interval2.max && interval2.min <= interval1.max;
        }

        [BurstCompile]
        static unsafe void ProjectOBB(in OrientedBounds obb, in float3 axis, out Interval interval)
        {
            var vertices = stackalloc float3[8];

            // 生成OBB的8个顶点
            GetOBBVertices(obb, vertices);

            // 初始化投影的最小/最大值
            float min = math.dot(axis, vertices[0]);
            float max = min;

            // 计算每个顶点在轴上的投影，并更新最小/最大值
            for (int i = 1; i < 8; i++)
            {
                float projection = math.dot(axis, vertices[i]);
                min = math.min(min, projection);
                max = math.max(max, projection);
            }

            // 返回投影区间
            interval = new Interval(min, max);
        }

        [BurstCompile]
        struct Interval
        {
            public float min;
            public float max;
            public Interval(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
        }

        /// <summary>
        /// float约等于
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Approximately(this float v, in float target, in float epsilon)
        {
            return math.abs(target - v) < epsilon;
        }

        /// <summary>
        /// vector3 约等于
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Approximately(this Vector3 v, in Vector3 target, in float epsilon)
        {
            return
            math.abs(target.x - v.x) < epsilon
            &&
            math.abs(target.y - v.y) < epsilon
            &&
            math.abs(target.z - v.z) < epsilon
            ;
        }

        /// <summary>
        /// float3 约等于
        /// </summary>
        /// <param name="v"></param>
        /// <param name="target"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Approximately(this float3 v, in float3 target, in float epsilon)
        {
            return
            math.abs(target.x - v.x) < epsilon
            &&
            math.abs(target.y - v.y) < epsilon
            &&
            math.abs(target.z - v.z) < epsilon
            ;
        }
    }
}
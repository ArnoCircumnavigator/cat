/*----------------------------------------------------------------
 * User    : Arno [Copyright (C) 2024]
 * Time    ：2024/1/22
 * CLR     ：
 * filename：
 * E-male  ：1961925755@qq.com
 
 * Desc    : 
 *
 * ----------------------------------------------------------------
 * Modification
 *
 *----------------------------------------------------------------*/

using Unity.Burst;
using Unity.Mathematics;

namespace GeometryAssist
{
    /*
    注：手绘分析时，法线是纸面向上的
    */

    /// <summary>
    /// 有向弧（优弧，劣弧均可）
    /// </summary>
    [BurstCompile]
    public struct Arc
    {
        /// <summary>
        /// 弧心
        /// </summary>
        public float3 center;
        /// <summary>
        /// 由弧心到弧起点的向量 归一化
        /// </summary>
        public float3 startDir;
        /// <summary>
        /// 弧所在平面法线  归一化
        /// </summary>
        public float3 normal;
        /// <summary>
        /// 半径
        /// </summary>
        public float radius;
        /// <summary>
        /// 弧度(带正负，大于0为逆时针，小于0为顺时针)
        /// </summary>
        public float radian;

        public Arc(float3 center, float3 normal, float3 startDir, float radius, float radian)
        {
#if UNITY_EDITOR
            if (!(math.dot(math.normalize(normal), math.normalize(startDir)) <= 0.001f))
            {
                //发现和arcStart与normal不垂直！无法确定空间结构
                throw new System.Exception("normal与startDir不垂直,无法确定空间结构");
            }
#endif
            this.center = center;
            this.normal = normal;
            this.startDir = startDir;
            this.radius = radius;
            this.radian = radian;
        }
    }
}
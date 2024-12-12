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
    /// <summary>
    /// 包围球
    /// </summary>
    [BurstCompile]
    public struct SphereBounds
    {
        public float3 center;
        public float radius;

        public SphereBounds(float3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}

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
    /// 定向包围盒
    /// </summary>
    [BurstCompile]
    public struct OrientedBounds
    {
        public float3 center;  // 边界盒中心
        public float3 extents; // 边界盒的半尺寸
        public quaternion rotation; // 边界盒的旋转

        public OrientedBounds(float3 center, float3 size, quaternion rotation)
        {
            this.center = center;
            this.extents = size * 0.5f;
            this.rotation = rotation;
        }
    }
}

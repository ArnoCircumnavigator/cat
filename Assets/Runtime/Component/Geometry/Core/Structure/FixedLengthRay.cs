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
    /// 定长射线(可以理解为有方向的线段)
    /// </summary>
    [BurstCompile]
    public struct FixedLengthRay
    {
        /// <summary>
        /// 射线起点
        /// </summary>
        public float3 origin;
        /// <summary>
        /// 射线方向，该向量的模，就是射线的长度
        /// </summary>
        public float3 direction;

        public FixedLengthRay(float3 origin, float3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
    }
}

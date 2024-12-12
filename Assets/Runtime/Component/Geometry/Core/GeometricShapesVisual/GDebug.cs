using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace GeometryAssist
{
    public static class GDebug
    {
        static GeometricShapesVisualMonoBehaviour _mono;
        static GeometricShapesVisualMonoBehaviour mono
        {
            get
            {
                if (_mono == null)
                {
                    //构建一个mono脚本
                    GameObject go = new GameObject(Configer.GameobjectNamePrefix);
                    _mono = go.AddComponent<GeometricShapesVisualMonoBehaviour>();
                }
                return _mono;
            }
        }

        /// <summary>
        /// 画obb
        /// </summary>
        /// <param name="obb"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawWireCube(OrientedBounds obb, Color color, float duration = 0)
        {
            mono.AddHandle(() =>
            {
                // 保存Gizmo原有数据
                Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
                Color oldcolor = Gizmos.color;

                //使用Gizmos绘制这个obb
                Gizmos.color = color;
                Gizmos.matrix = Matrix4x4.TRS(obb.center, obb.rotation, Vector3.one);
                //现在绘制一个立方体，它自动应用了当前的Gizmos矩阵（包含了变换）
                Gizmos.DrawWireCube(float3.zero, obb.extents * 2); // extents需要乘以2因为DrawWireCube的参数是全尺寸

                // 恢复Gizmo原有数据
                Gizmos.matrix = oldGizmosMatrix;
                Gizmos.color = oldcolor;
            }, duration);
        }


        /// <summary>
        /// 画包围球
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="color"></param>
        /// <param name="matrix"></param>
        /// <param name="duration"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawWireSphere(SphereBounds sphere, Color color, float duration = 0)
        {
            mono.AddHandle(() =>
            {
                // 保存Gizmo原有数据
                Color oldcolor = Gizmos.color;

                //设置颜色
                Gizmos.color = color;
                //绘制包围球
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);

                // 恢复Gizmo原有数据
                Gizmos.color = oldcolor;
            }, duration);
        }

        /// <summary>
        /// 画线段
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0)
        {
            mono.AddHandle(() =>
           {
               // 保存Gizmo原有数据
               Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
               Color oldcolor = Gizmos.color;

               //设置颜色
               Gizmos.color = color;
               //绘制线段
               Gizmos.DrawLine(start, end);

               // 恢复Gizmo原有数据
               Gizmos.matrix = oldGizmosMatrix;
               Gizmos.color = oldcolor;
           }, duration);
        }

        /// <summary>
        /// 画定长射线
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="color"></param>
        /// <param name="matrix"></param>
        /// <param name="duration"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRay(FixedLengthRay ray, Color color, float duration = 0)
        {
            mono.AddHandle(() =>
            {
                // 保存Gizmo原有数据
                Color oldcolor = Gizmos.color;

                //设置颜色
                Gizmos.color = color;

                //绘制线段
                Gizmos.DrawLine(ray.origin, ray.origin + ray.direction);

                // 恢复Gizmo原有数据
                Gizmos.color = oldcolor;
            }, duration);
        }

        /// <summary>
        /// 绘制一段弧（弧起点处会额外画一小个球，用于表达弧的起点）
        /// </summary>
        /// <param name="arc"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawArc(Arc arc, Color color, float duration = 0)
        {
            const int segments = 100;//平滑度
            const float step = 2 * Mathf.PI / segments;
            mono.AddHandle(() =>
            {
                // 保存Gizmo原有数据
                Color oldcolor = Gizmos.color;

                //设置颜色
                Gizmos.color = color;

                var rotation = quaternion.AxisAngle(arc.normal, step);
                float radian_scalar = math.abs(arc.radian);
                float3 os = arc.startDir * arc.radius;
                Gizmos.DrawSphere(arc.center + os, 0.005f);//在起点处画一个小球
                for (float tmp = step; tmp <= radian_scalar; tmp += step)
                {
                    float3 oe = math.mul(rotation, os);
                    Gizmos.DrawLine(arc.center + os, arc.center + oe);
                    os = oe;
                }

                // 恢复Gizmo原有数据
                Gizmos.color = oldcolor;
            }, duration);
        }
    }
}

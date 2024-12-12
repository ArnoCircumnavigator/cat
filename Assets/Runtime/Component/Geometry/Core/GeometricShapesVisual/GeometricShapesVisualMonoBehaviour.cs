using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeometryAssist
{
# nullable enable
    class DrawGizmosHandle
    {
        public Action? handle;
        public float duration;
        public float startTime; // 添加这个字段来记录开始时间
        public bool hasExecuted; // 用来检查是否至少执行过一次
    }

    internal class GeometricShapesVisualMonoBehaviour : MonoBehaviour
    {
        public HashSet<DrawGizmosHandle> handles = new HashSet<DrawGizmosHandle>();

        void OnDrawGizmos()
        {
            if (handles == null) return;

            var currentTime = Time.time;
            // 使用迭代器避免在遍历时修改集合
            List<DrawGizmosHandle> handlesToRemove = new List<DrawGizmosHandle>();

            foreach (var handle in handles)
            {
                // 检查是否已经执行并且时间是否超过duration
                if (handle.hasExecuted && currentTime - handle.startTime > handle.duration)
                {
                    handlesToRemove.Add(handle);
                    continue;
                }

                // 执行handle并设置已执行标志
                handle.handle?.Invoke();
                handle.hasExecuted = true;
            }

            // 移除所有需要被移除的handles
            foreach (var handleToRemove in handlesToRemove)
            {
                handles.Remove(handleToRemove);
            }
        }

        public void AddHandle(Action action, float duration)
        {
            var newHandle = new DrawGizmosHandle
            {
                handle = action,
                duration = duration,
                startTime = Time.time // 记录添加时的时间
            };

            handles.Add(newHandle);
        }
    }
}
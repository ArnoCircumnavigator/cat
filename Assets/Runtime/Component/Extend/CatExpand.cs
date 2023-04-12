using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.Extend
{
    /// <summary>
    /// GameObject扩展
    /// </summary>
    public static class CatExpand
    {
        private static Dictionary<Transform, bool> MoveStatus = new Dictionary<Transform, bool>();
        /// <summary>
        /// 根据子物体名(全称/部称)获取指定的组件(只返回第一个符合条件的组件)
        /// </summary>
        /// <typeparam name="T">要获得的组件类型</typeparam>
        /// <param name="thisGO"></param>
        /// <param name="childName">全称匹配(名字),部称匹配(%+部称)</param>
        /// <returns></returns>
        public static T GetChildComponent<T>(this GameObject thisGO, string childName) where T : UnityEngine.Component
        {
            T temp;
            Transform[] at = thisGO.GetComponentsInChildren<Transform>();
            foreach (Transform item in at)
            {
                if ((childName.IndexOf("%") == 0 && item.name.Contains(childName.Substring(1, childName.Length - 1))) || item.name.Equals(childName))
                {
                    temp = item.gameObject.GetComponent<T>();
                    return temp;
                }
            }
            return default;
        }

        public static List<T> GetChildComponentsWithTag<T>(this GameObject thisGO, string Tag) where T : UnityEngine.Component
        {
            List<T> temp = new List<T>();
            Transform[] at = thisGO.GetComponentsInChildren<Transform>();
            foreach (Transform item in at)
            {
                if ((Tag.IndexOf("%") == 0 && item.tag.Contains(Tag.Substring(1, Tag.Length - 1))) || item.CompareTag(Tag))
                {
                    temp.Add(item.gameObject.GetComponent<T>());
                    return temp;
                }
            }
            return default;
        }

        /// <summary>
        /// 获取子物体上的所有某个类型的组件
        /// </summary>
        /// <typeparam name="T">要获得的组件类型</typeparam>
        /// <param name="thisGO"></param>
        /// <returns></returns>
        public static T[] GetChildernComponents<T>(this GameObject thisGO) where T : UnityEngine.Component
        {
            Transform[] at = thisGO.GetComponentsInChildren<Transform>();
            if (at.Length == 0)
            {
                Debug.LogWarning("物体：" + thisGO.name + "下不存在子物体！");
                return null;
            }
            else
            {
                List<T> temp = new List<T>();
                for (int i = 0; i < at.Length; i++)
                {
                    if (at[i].gameObject.ComponentIsExist<T>())
                    {
                        if (at[i].gameObject != thisGO) temp.Add(at[i].gameObject.GetComponent<T>());
                    }
                }
                return temp.ToArray();
            }
        }

        /// <summary>
        /// 获取所有同名的子物体
        /// </summary>
        /// <typeparam name="T">要获得的组件类型</typeparam>
        /// <param name="thisGO"></param>
        /// <returns></returns>
        public static GameObject[] GetChildern(this GameObject thisGO, string childName)
        {
            Transform[] at = thisGO.GetComponentsInChildren<Transform>();
            if (at.Length == 0)
            {
                Debug.LogWarning("物体：" + thisGO.name + "下不存在子物体！");
                return null;
            }
            else
            {
                List<GameObject> temp = new List<GameObject>();
                for (int i = 0; i < at.Length; i++)
                {
                    if (at[i].name.Equals(childName)) temp.Add(at[i].gameObject);
                }
                return temp.ToArray();
            }
        }

        /// <summary>
        /// 判断某个物体上是否存在某个组件
        /// </summary>
        /// <typeparam name="T">要判断的组件类型</typeparam>
        /// <param name="thisGO"></param>
        /// <returns></returns>
        public static bool ComponentIsExist<T>(this GameObject thisGO) where T : UnityEngine.Component
        {
            T[] ac = thisGO.GetComponents<T>();
            return ac.Length != 0;
        }
        /// <summary>
        /// 重新设定Texture2D的大小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tarWidth">新的宽度</param>
        /// <param name="tarHeight">新的高度</param>
        public static Texture2D ResetScale(this Texture2D source, int tarWidth, int tarHeight)
        {
            Texture2D result = new Texture2D(tarWidth, tarHeight, source.format, false);
            Color temp;
            for (int i = 0; i < tarWidth; i++)
            {
                for (int j = 0; j < tarHeight; j++)
                {
                    temp = source.GetPixelBilinear((float)i / result.width, (float)j / result.height);
                    result.SetPixel(i, j, temp);
                }
            }
            result.Apply();
            return result;
        }
        /// <summary>
        /// 位置锁定(必须在update中调用)
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="tar">目标位置</param>
        /// <param name="moveBoundry">移动边界距离</param>
        /// <param name="anchrBoundry">静止边界距离</param>
        /// <param name="interpolation">插值</param>
        public static void Widden(this Transform cur, Transform tar, float moveBoundry = 0.05f, float anchrBoundry = 0.05f, float interpolation = 0.05f)
        {
            if (cur == null) return;
            if (tar == null) return;
            if (!MoveStatus.ContainsKey(cur)) MoveStatus.Add(cur, false);
            //静止状态下在允许的范围内不进行移动
            if (Vector3.Distance(tar.position, cur.position) <= moveBoundry && MoveStatus[cur] == false) return;
            //移动状态确认
            if (MoveStatus[cur] == false) MoveStatus[cur] = true;
            cur.position = Vector3.Lerp(cur.position, tar.position, interpolation);
            if (Vector3.Distance(tar.position, cur.position) <= anchrBoundry)
            {
                //将状态切换到静止状态
                cur.position = tar.position;
                MoveStatus[cur] = false;
            }
        }
        /// <summary>
        /// 朝向锁定,必须位于Update方法中使用
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="tar">目标物体</param>
        public static void LookTo(this Transform cur, Transform tar)
        {
            cur.forward = cur.position - tar.position;
        }
    }
}



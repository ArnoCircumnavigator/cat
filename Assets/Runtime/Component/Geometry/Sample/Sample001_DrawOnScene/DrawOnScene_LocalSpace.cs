using UnityEngine;

namespace GeometryAssist.Sample
{
    public class DrawOnScene_LocalSpace : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            /*-----------
            局部空间坐标系下的样例
            就是将值转化到世界空间下进行绘制
            --------------*/

            Transform child = this.transform;
            Matrix4x4 matrix = child.localToWorldMatrix;

            var obb = new OrientedBounds(center: child.localPosition,
                                         size: Vector3.one,
                                         rotation: child.localRotation);

            //把obb换到世界中去
            //在新版本unity（2022）中,下面这行使用matrix.GetPosition();代替
            obb.center = new Vector3(matrix.m03, matrix.m13, matrix.m23);
            obb.rotation = matrix.rotation * obb.rotation;

            //画obb
            GDebug.DrawWireCube(obb, Color.yellow);
        }
    }
}

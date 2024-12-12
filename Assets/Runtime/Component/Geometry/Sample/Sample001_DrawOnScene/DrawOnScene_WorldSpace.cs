using UnityEngine;

namespace GeometryAssist.Sample
{
    public class DrawOnScene_WorldSpace : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            /*-----------
            世界空间坐标系下的样例
            --------------*/

            //画obb
            var obb = new OrientedBounds(center: this.transform.position,
                                         size: Vector3.one,
                                         rotation: this.transform.rotation);
            GDebug.DrawWireCube(obb, Color.yellow);

            //画球
            var sphere = new SphereBounds(center: this.transform.position,
                                          radius: 1);
            GDebug.DrawWireSphere(sphere, Color.yellow);

            //画有向弧
            var arc = new Arc(this.transform.position,
                               this.transform.up,
                               this.transform.forward,
                               1.5f,
                               92 * Mathf.Deg2Rad);
            GDebug.DrawArc(arc, Color.red);
            GDebug.DrawArc(arc, Color.green);

            //画定长射线
            var ray = new FixedLengthRay(origin: this.transform.position,
                                         direction: this.transform.right);
            GDebug.DrawRay(ray, Color.magenta);

            //画线段
            GDebug.DrawLine(this.transform.position, this.transform.position - this.transform.up, Color.blue);
        }
    }
}

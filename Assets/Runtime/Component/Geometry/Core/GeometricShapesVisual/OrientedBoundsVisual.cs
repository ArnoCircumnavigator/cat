using GeometryAssist;
using UnityEngine;

public class OrientedBoundsVisual : MonoBehaviour
{
    public Color color = Color.yellow;
    public Vector3 size = Vector3.one;

    public OrientedBounds obb = new OrientedBounds();


    public void Update()
    {
        obb.center = this.transform.position;
        obb.rotation = this.transform.rotation;
        obb.extents = size * .5f;
    }

    void OnDrawGizmos()
    {
        DrawOrientedBounds(obb);
    }

    void DrawOrientedBounds(OrientedBounds obb)
    {
        // 转换为Unity的数据类型
        Vector3 center = obb.center;
        Quaternion rotation = obb.rotation;
        Vector3 extents = obb.extents;

        // 保存当前的Gizmo矩阵
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

        // 设置Gizmos绘制的坐标系统
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

        // 设置Gizmos颜色
        Gizmos.color = color;

        // 绘制OBB
        Gizmos.DrawWireCube(Vector3.zero, extents * 2);

        // 恢复默认的Gizmo矩阵
        Gizmos.matrix = oldGizmosMatrix;
    }
}